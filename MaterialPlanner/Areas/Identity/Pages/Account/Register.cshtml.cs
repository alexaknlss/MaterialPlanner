using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using MaterialPlanner.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace MaterialPlanner.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class RegisterModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ILogger<RegisterModel> _logger;

    public RegisterModel(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ILogger<RegisterModel> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
    }

    [BindProperty]
    public InputModel Input { get; set; }

    public string ReturnUrl { get; set; }

    public class InputModel
    {
        [Required(ErrorMessage = "El correo electrónico es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de correo electrónico inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(100, ErrorMessage = "La contraseña debe tener mínimo {2} y máximo {1} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "La contraseña y confirmación no coinciden")]
        public string ConfirmPassword { get; set; }
    }

    public void OnGet(string returnUrl = null)
    {
        ReturnUrl = returnUrl;
    }

    public async Task<IActionResult> OnPostAsync(string returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");

        // ============================================================
        // VALIDACIÓN DE DOMINIO
        // ============================================================
        string dominioPermitido = "@st-group.com";

        if (!string.IsNullOrEmpty(Input?.Email) &&
            !Input.Email.EndsWith(dominioPermitido, StringComparison.OrdinalIgnoreCase))
        {
            ModelState.AddModelError("Input.Email", $"Solo se permiten correos del dominio {dominioPermitido}");
        }

        // ============================================================
        // VALIDACIÓN ADICIONAL: Verificar si el usuario ya existe
        // ============================================================
        if (!string.IsNullOrEmpty(Input?.Email))
        {
            var existingUser = await _userManager.FindByEmailAsync(Input.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Input.Email", "Este correo electrónico ya está registrado");
            }
        }

        // Si hay errores de validación, regresar al formulario
        if (!ModelState.IsValid)
        {
            // Log para depuración
            _logger.LogWarning($"Validación fallida para {Input?.Email ?? "null"}");
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                _logger.LogWarning($"Error de validación: {error.ErrorMessage}");
            }
            return Page();
        }

        try
        {
            // ============================================================
            // CREACIÓN DEL USUARIO
            // ============================================================
            var user = new ApplicationUser
            {
                UserName = Input.Email,
                Email = Input.Email,
                EmailConfirmed = true // Opcional: si no necesitas confirmación
            };

            _logger.LogInformation($"Intentando crear usuario: {Input.Email}");

            var result = await _userManager.CreateAsync(user, Input.Password);

            if (result.Succeeded)
            {
                _logger.LogInformation($"Usuario creado exitosamente: {Input.Email}");

                // Iniciar sesión automáticamente
                await _signInManager.SignInAsync(user, isPersistent: false);

                return LocalRedirect(returnUrl);
            }

            // ============================================================
            // MANEJO DE ERRORES DE IDENTITY
            // ============================================================
            foreach (var error in result.Errors)
            {
                _logger.LogWarning($"Error Identity: {error.Code} - {error.Description}");

                // Traducir errores comunes a mensajes amigables
                string mensajeAmigable = error.Description switch
                {
                    var desc when desc.Contains("Duplicate") || error.Code == "DuplicateEmail"
                        => "Este correo electrónico ya está registrado",

                    var desc when desc.Contains("Password") && error.Code == "PasswordTooShort"
                        => "La contraseña debe tener al menos 6 caracteres",

                    var desc when desc.Contains("Password") && error.Code == "PasswordRequiresDigit"
                        => "La contraseña debe contener al menos un número",

                    var desc when desc.Contains("Password") && error.Code == "PasswordRequiresUpper"
                        => "La contraseña debe contener al menos una letra mayúscula",

                    var desc when desc.Contains("Password") && error.Code == "PasswordRequiresLower"
                        => "La contraseña debe contener al menos una letra minúscula",

                    var desc when desc.Contains("Password") && error.Code == "PasswordRequiresNonAlphanumeric"
                        => "La contraseña debe contener al menos un carácter especial",

                    _ => error.Description // Mantener el mensaje original
                };

                ModelState.AddModelError(string.Empty, mensajeAmigable);
            }

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error inesperado al registrar {Input?.Email ?? "usuario"}");
            ModelState.AddModelError(string.Empty, "Ocurrió un error inesperado. Por favor, intenta de nuevo.");
            return Page();
        }
    }
}