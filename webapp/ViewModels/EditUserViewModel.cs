using System.ComponentModel.DataAnnotations;

namespace webapp.ViewModels;

public class EditUserViewModel
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
    [Required]
    [MinLength(4)]
    public string Password { get; set; }
    [Required]
    [Compare(nameof(Password))]
    public string ConfirmPassword { get; set; }
    public string OldPassword { get; set; }
    public string ErrorMessage { get; set; }
}