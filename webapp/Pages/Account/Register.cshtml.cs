
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using webapp.Entities;
using webapp.ViewModels;

namespace webapp.Pages.Account;

public class RegisterModel : PageModel
{
    private readonly UserManager<AppUser> _userM;
    private readonly SignInManager<AppUser> _signInM;

    public RegisterModel(UserManager<AppUser> useerM, SignInManager<AppUser> signInM)
    {
        _userM = useerM;
        _signInM = signInM;
    }
    [BindProperty]
    public RegisterViewModel RegisterViewModel { get; set; }
    public async Task<IActionResult> OnPostAsync()
    {
        if(!ModelState.IsValid)
        {
            return Page();
        }

        var user = await _userM.Users.FirstOrDefaultAsync(u => u.UserName == RegisterViewModel.UserName);
        if(user == default)
        {
            var newUser = new AppUser()
            {
                UserName = RegisterViewModel.UserName,
                FullName = RegisterViewModel.FullName
            };

            var result = await _userM.CreateAsync(newUser, RegisterViewModel.Password);
            var entity = await _userM.FindByNameAsync(RegisterViewModel.UserName);
            if(result.Succeeded)
            {
                var loginResult = await _signInM.PasswordSignInAsync(entity, RegisterViewModel.Password, false, false);
                if(loginResult.Succeeded)
                {
                    return RedirectToPage(RegisterViewModel.ReturnUrl ?? "/Index");
                }
                else
                {
                    return RedirectToPage("./Login");
                }
            }
            else
            {
                RegisterViewModel.ErrorMessage = "Error with Server. Please, try again";
                return Page();
            }
        }

        RegisterViewModel.ErrorMessage = "You are already signed up. Please Sign In";
        return Page();
    }
}