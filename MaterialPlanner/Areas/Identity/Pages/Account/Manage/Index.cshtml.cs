using System.Threading.Tasks;
using MaterialPlanner.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MaterialPlanner.Areas.Identity.Pages.Account.Manage;

[Authorize]
public class IndexModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;

    public IndexModel(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public string Username { get; set; }
    public string Email { get; set; }

    public async Task OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user != null)
        {
            Username = user.UserName;
            Email = user.Email;
        }
    }
}
