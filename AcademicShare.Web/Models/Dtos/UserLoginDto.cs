using System.ComponentModel.DataAnnotations;

namespace AcademicShare.Web.Models.Dtos;


public class UserLoginDto
{
	[Required(ErrorMessage ="The email field is required")]
	[EmailAddress(ErrorMessage = "The email field is must be a valid email address")]
	public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "The password field is required")]
	[DataType(DataType.Password)]
	public string Password { get; set; } = string.Empty;

	[Display(Name = "Remember me?")]
	public bool RememberMe { get; set; }
}
