using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using webapp.Entities;
using webapp.ViewModels;

namespace webapp.Pages.Users;

public class EditModel : PageModel
{
    private readonly UserManager<AppUser> _userM;
    private RoleManager<IdentityRole<Guid>> _roleM;
    private readonly IServiceProvider _serviceProvider;

    public EditModel(IServiceProvider serviceProvider, UserManager<AppUser> userM, SignInManager<AppUser> signInM, RoleManager<IdentityRole<Guid>> roleM)
    {
        _userM = userM;
        _roleM = roleM;
        _serviceProvider = serviceProvider;
    }
    [BindProperty]
    public EditUserViewModel EditUser { get; set; }
    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if(id.HasValue)
        {
            if(string.IsNullOrWhiteSpace(id.ToString()))
            {
                return NotFound("User with given id is not found");
            }
            var existingUser = await _userM.FindByIdAsync(id.Value.ToString());
            if(existingUser != null)
            {
                EditUser = new EditUserViewModel()
                {
                    Id = existingUser.Id,
                    Username = existingUser.UserName,
                    Email = existingUser.Email,
                    Fullname = existingUser.FullName,
                    JoinedAt = existingUser.JoinedAt,
                    UpdatedAt = existingUser.UpdatedAt,
                    Roles = existingUser.Roles,
                    Password = existingUser.Password,
                    ConfirmPassword = existingUser.Password,
                    OldPassword = existingUser.Password
                };
            }
            else
            {
                return NotFound("User with given id is Not Found");
            }
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var existingUser = await _userM.FindByNameAsync(EditUser.Username);
        if(existingUser == null)
        {
            return BadRequest(new { Error = "User does not exist" });
        }
        if(existingUser.FullName == EditUser.Fullname && existingUser.Password == EditUser.Password && existingUser.Roles == EditUser.Roles && existingUser.Email == EditUser.Email)
        {
            return RedirectToPage("./Index");
        }
        else
        if(EditUser.OldPassword != EditUser.Password)
        {
            existingUser.PasswordHash = _userM.PasswordHasher.HashPassword(existingUser, EditUser.Password);
            existingUser.Password = EditUser.Password;
        }
        existingUser.Email = EditUser.Email;
        existingUser.UpdatedAt = DateTimeOffset.UtcNow;
        existingUser.FullName = EditUser.Fullname;
        
        await _userM.RemoveFromRolesAsync(existingUser, await _userM.GetRolesAsync(existingUser));
        existingUser.Roles = "";
        if(!string.IsNullOrWhiteSpace(EditUser.Roles) && EditUser.Roles != existingUser.Roles)
        {
            var roles = EditUser.Roles.Split(",").Select(x => x.Trim()) ?? new List<string> { EditUser.Roles };
            foreach (var role in roles)
            {
                if(await _roleM.RoleExistsAsync(role) && !await _userM.IsInRoleAsync(existingUser, role))
                {
                    await _userM.AddToRoleAsync(existingUser, role);
                }
            }
            existingUser.Roles = string.Join(", ", roles.OrderBy(x => x));
        }

        var updateResult = await _userM.UpdateAsync(existingUser);
        if(!updateResult.Succeeded)
        {
            
            Console.WriteLine($"{JsonSerializer.Serialize(updateResult.Errors)}");
        }
        return RedirectToPage("./Index");
    }
}