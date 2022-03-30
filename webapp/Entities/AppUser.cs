using Microsoft.AspNetCore.Identity;

namespace webapp.Entities;

public class AppUser : IdentityUser<Guid>
{
    public string FullName { get; set; }
    public DateTimeOffset JoinedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public string Roles { get; set; }
    public string Password { get; set; }
}