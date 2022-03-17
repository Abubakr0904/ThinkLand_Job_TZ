using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using webapp.Entities;
using webapp.ViewModels;

namespace webapp.Pages.Account;
public class LoginModel : PageModel
{
    private readonly UserManager<AppUser> _userM;
    private readonly SignInManager<AppUser> _signInM;

    public LoginModel(UserManager<AppUser> useerM, SignInManager<AppUser> signInM)
    {
        _userM = useerM;
        _signInM = signInM;
    }
    [BindProperty]
    public LoginViewModel loginViewModel { get; set; }
    
    public async Task<IActionResult> OnPostAsync()
    {
        if(!ModelState.IsValid)
        {
            return Page();
        }

        var user = await _userM.Users.FirstOrDefaultAsync(u => u.UserName == loginViewModel.UserName);
        if(user == default)
        {
            ModelState.AddModelError("Password", "Invalid userName or password.");
            loginViewModel.ErrorMessage = "You are not signed up, Please, sign up first!";
            return Page();
        }

        var result = await _signInM.PasswordSignInAsync(user, loginViewModel.Password, false, false);
        if(result.Succeeded)
        {
            return RedirectToPage(loginViewModel.ReturnUrl ?? "/Index");
        }
        else
        {
            ModelState.AddModelError("Password", "Invalid userName or password.");
            loginViewModel.ErrorMessage = "Invalid User Name or Password.";
            return Page();
        }
    }
}