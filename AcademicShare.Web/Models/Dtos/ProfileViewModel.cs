using System.ComponentModel.DataAnnotations;
using Microsoft.DotNet.Scaffolding.Shared;

namespace AcademicShare.Web.Models;


public class ProfileViewModel
{
    [Key]
    public int ProfileId { get; set; }
    public string Location { get; set; } = string.Empty;
    public string Website { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public IFormFile? Banner { get; set; }
    public IFormFile? ProfilePicture { get; set; }
    public DateOnly? BirthDate { get; set; }
    public string UserId { get; set; } = string.Empty;
}
