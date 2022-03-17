using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace webapp.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "User Name is required!")]
    [MinLength(4, ErrorMessage = "User Name must contain at least 4 characters")]
    [MaxLength(64, ErrorMessage = "User Name must contain up to 64 characters")]
    [DisplayName("User Name")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Password field is required!")]
    [MinLength(4, ErrorMessage = "Password must contain at least 4 characters")]
    [DisplayName("Password")]
    public string Password { get; set; }
    public string ReturnUrl { get; set; }
    public string ErrorMessage { get; set; }
}