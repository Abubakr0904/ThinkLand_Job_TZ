using System.ComponentModel.DataAnnotations;

namespace webapp.ViewModels;

public class AppUserViewModel
{
    public Guid Id { get; set; }
    [Required]
    public string Username { get; set; }
    [Required]
    public string Email { get; set; }
    public string Fullname { get; set; }
    public DateTimeOffset JoinedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public string Roles { get; set; }
    public string Password { get; set; }
}