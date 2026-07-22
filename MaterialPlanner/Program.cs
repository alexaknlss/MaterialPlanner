using MaterialPlanner.Data;
using MaterialPlanner.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// ============================================================
// CONFIGURACIÓN DE SERVICIOS
// ============================================================

// MVC + Razor Pages
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Database
builder.Services.AddDbContext<MaterialPlannerContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

// ============================================================
// IDENTITY - CONFIGURACIÓN COMPLETA Y SINCRONIZADA
// ============================================================
builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        // =========================================
        // CONFIGURACIÓN DE INICIO DE SESIÓN
        // =========================================
        options.SignIn.RequireConfirmedAccount = false;
        options.SignIn.RequireConfirmedEmail = false;
        options.SignIn.RequireConfirmedPhoneNumber = false;

        // =========================================
        // CONFIGURACIÓN DE CONTRASEÑAS
        // =========================================
        // Longitud mínima permitida (sincronizado con RegisterModel)
        options.Password.RequiredLength = 6;

        // No obliga tener números
        options.Password.RequireDigit = false;

        // No obliga letras mayúsculas
        options.Password.RequireUppercase = false;

        // No obliga letras minúsculas
        options.Password.RequireLowercase = false;

        // No obliga caracteres especiales (@, #, $, etc.)
        options.Password.RequireNonAlphanumeric = false;

        // =========================================
        // CONFIGURACIÓN DE USUARIO
        // =========================================
        // Asegura que los emails sean únicos
        options.User.RequireUniqueEmail = true;

        // Caracteres permitidos en el nombre de usuario
        options.User.AllowedUserNameCharacters =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";

        // =========================================
        // CONFIGURACIÓN DE BLOQUEO
        // =========================================
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.AllowedForNewUsers = true;
    })
    .AddEntityFrameworkStores<MaterialPlannerContext>()
    .AddDefaultTokenProviders();

// ============================================================
// CONFIGURACIÓN DE COOKIES DE AUTENTICACIÓN
// ============================================================
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(14);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// ============================================================
// CONFIGURACIÓN DE VALIDACIÓN DE DATOS
// ============================================================
builder.Services.Configure<IdentityOptions>(options =>
{
    // Configuración adicional de Identity
});

var app = builder.Build();

// ============================================================
// CONFIGURACIÓN DE ROTATIVA PDF
// ============================================================
RotativaConfiguration.Setup(
    app.Environment.WebRootPath,
    "NewRotativa"
);

// ============================================================
// PIPELINE DE SOLICITUDES HTTP
// ============================================================

// Manejo de errores
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    // En desarrollo, muestra errores detallados
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// ============================================================
// AUTENTICACIÓN Y AUTORIZACIÓN
// ============================================================
app.UseAuthentication();
app.UseAuthorization();

// ============================================================
// MAPEO DE RUTAS
// ============================================================

// Identity Pages
app.MapRazorPages();

// MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// ============================================================
// MIGRACIONES Y DATOS INICIALES
// ============================================================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var db = services.GetRequiredService<MaterialPlannerContext>();

        // Aplicar migraciones automáticamente
        db.Database.Migrate();

        // ============================================================
        // SEED DATA - CREAR USUARIO ADMIN POR DEFECTO
        // ============================================================
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        // Crear roles si no existen
        string[] roleNames = { "Admin", "User" };
        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
                Console.WriteLine($"Rol '{roleName}' creado exitosamente.");
            }
        }

        // Crear usuario administrador por defecto (solo si no existe)
        string adminEmail = "admin@st-group.com";
        string adminPassword = "Admin123";

        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, adminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
                Console.WriteLine($"Usuario administrador creado: {adminEmail}");
            }
            else
            {
                Console.WriteLine($"Error al crear usuario admin: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
        }

        Console.WriteLine("Base de datos inicializada correctamente.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error al inicializar la base de datos: {ex.Message}");
        // En producción, podrías registrar el error en un archivo de logs
    }
}

// ============================================================
// INICIAR LA APLICACIÓN
// ============================================================
app.Run();