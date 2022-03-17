using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace webapp.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "User Name is required!")]
    [MinLength(4, ErrorMessage = "User Name must contain at least 4 characters")]
    [MaxLength(64, ErrorMessage = "User Name must contain up to 64 characters")]
    [DisplayName("User Name")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Full Name is required!")]
    [MinLength(4, ErrorMessage = "Full Name must contain at least 4 characters")]
    [MaxLength(64, ErrorMessage = "Full Name must contain up to 64 characters")]
    [DisplayName("User Name")]
    public string FullName { get; set; }

    [Required(ErrorMessage = "Password field is required!")]
    [MinLength(4, ErrorMessage = "Password must contain at least 4 characters")]
    [DisplayName("Password")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Password field is required!")]
    [MinLength(4, ErrorMessage = "Password must contain at least 4 characters")]
    [Compare(nameof(Password), ErrorMessage = "Password fields must match!")]
    [DisplayName("PasswordSecond")]
    public string PasswordSecond { get; set; }
    public string ReturnUrl { get; set; }
    public string ErrorMessage { get; set; }
}