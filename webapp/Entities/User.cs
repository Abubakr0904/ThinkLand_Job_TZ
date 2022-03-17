using Microsoft.AspNetCore.Identity;

namespace webapp.Entities;

public class AppUser : IdentityUser<Guid>
{
    public string FullName { get; set; }
    public ICollection<Expense> Expenses { get; set; }
}