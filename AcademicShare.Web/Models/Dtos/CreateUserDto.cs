using System.ComponentModel.DataAnnotations;

namespace AcademicShare.Web.Models.Dtos;


public class CreateUserDto
{
    [Required(ErrorMessage ="The email field is required")]
    [EmailAddress(ErrorMessage = "The email field is must be a valid email address")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "The username field is required")]
    public string UserName { get; set; } = string.Empty;

    [Required(ErrorMessage = "The University field is required")]
    public string University { get; set; } = string.Empty;

    [Required(ErrorMessage = "The Course field is required")]
    public string Course { get; set; } = string.Empty;

    [Required(ErrorMessage = "The Registration field is required")]
    public string Registration { get; set; } = string.Empty;

    [Required(ErrorMessage = "The password field is required")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "The password field is required")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } = string.Empty;
}
