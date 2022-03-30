using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using webapp.Entities;
using webapp.ViewModels;

namespace webapp.Pages.Users;

public class IndexModel : PageModel
{
    private readonly UserManager<AppUser> _userM;
    private readonly SignInManager<AppUser> _signInM;

    public IndexModel(SignInManager<AppUser> signInM, UserManager<AppUser> userM)
    {
        _userM = userM;
        _signInM = signInM;
    }
    public List<AppUserViewModel> Users { get; set; }

    public async Task OnGet()
    {
        var users = await _userM.Users.ToListAsync();
        Users = users.Select(x => 
        {
            return new AppUserViewModel()
            {
                Id = x.Id,
                Username = x.UserName,
                Email = x.Email,
                JoinedAt = x.JoinedAt,
                UpdatedAt = x.UpdatedAt.HasValue ? x.UpdatedAt : new DateTimeOffset(),
                Roles = x.Roles,
                Password = x.Password
            };
        }).ToList();
    }
}