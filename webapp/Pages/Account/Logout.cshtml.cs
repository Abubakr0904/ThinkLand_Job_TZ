using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using webapp.Entities;

namespace webapp.Pages.Account;

public class LogoutModel : PageModel
{
    private readonly SignInManager<AppUser> _signInM;

    public LogoutModel(SignInManager<AppUser> signInManager)
    {
        _signInM = signInManager;
    }
    public async Task<IActionResult> OnGet()
    {
        await _signInM.SignOutAsync();
        return RedirectToPage("/Index");
    }
}