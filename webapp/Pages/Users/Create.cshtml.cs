using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using webapp.Entities;
using webapp.ViewModels;

namespace webapp.Pages.Users;

public class CreateModel : PageModel
{
    private readonly UserManager<AppUser> _userM;
    private readonly ILogger<CreateModel> _logger;

    public CreateModel(UserManager<AppUser> userM, ILogger<CreateModel> logger)
    {
        _userM = userM;
        _logger = logger;
    }
    [BindProperty]
    public NewUserViewModel NewUser { get; set; }

    public IActionResult OnGet()
    {
        NewUser = new NewUserViewModel();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if(!ModelState.IsValid)
        {
            return Page();
        }
        if(_userM.Users.Any(x => x.UserName == NewUser.Username || x.Email == NewUser.Email))
        {
            // TODO: Duplicate user validation message Not displaying. Do It!
            ModelState.AddModelError("ErrorMessage", "User with given email or username already exists");
            return Page();
        }
        var newUser = new AppUser()
        {
            Id = Guid.NewGuid(),
            UserName = NewUser.Username,
            FullName = NewUser.Fullname,
            JoinedAt = DateTimeOffset.UtcNow,
            Roles = NewUser.Roles,
            Email = NewUser.Email,
            Password = NewUser.Password
        };
        try
        {
            var identityResult = await _userM.CreateAsync(newUser, NewUser.Password);
            if(identityResult.Succeeded)
            {
                _logger.LogInformation($"User with name \"{NewUser.Username}\" created successfully.");
            }
            else 
            {
                _logger.LogError("Failed to create new user to db");
            }
        }
        catch (System.Exception ex)
        {
             _logger.LogError($"{ex.Message}. Error while creating new user", nameof(CreateModel));
        }
        return RedirectToPage("/Users/Index");
    }
}