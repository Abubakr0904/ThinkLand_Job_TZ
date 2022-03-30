using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using webapp.Entities;

namespace webapp.Pages.Users;

public class DeleteModel : PageModel
{
    private readonly UserManager<AppUser> _userM;

    public DeleteModel(UserManager<AppUser> userM)
    {
        _userM = userM;
    }
    public AppUser UserModel { get; set; }
    
    public string DateFormat { get; set; } = "dd MMM yyyy HH:mm:ss";  
    public async Task<IActionResult> OnGetAsync(Guid? id)
    {
        if(string.IsNullOrWhiteSpace(id.ToString()))
        {
            return NotFound("User with given parameters does not exist.");
        }
        UserModel = await _userM.FindByIdAsync(id.ToString());
        if(UserModel == null)
        {
            return NotFound("User with given parameters does not exist.");
        }
        return Page();
    }
    public async Task<IActionResult> OnPostAsync(Guid? id)
    {
        await _userM.DeleteAsync(await _userM.FindByIdAsync(id.ToString()));
        return RedirectToPage("./Index");
    }
}