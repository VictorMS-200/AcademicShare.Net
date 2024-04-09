using System.ComponentModel.DataAnnotations;
using Microsoft.DotNet.Scaffolding.Shared;

namespace AcademicShare.Web.Models;


public class ProfileViewModel
{
    [Key]
    public int ProfileId { get; set; }
    public string? Location { get; set; }
    public string? Website { get; set; }
    public string? Bio { get; set; }
    public IFormFile? Banner { get; set; }
    public IFormFile? ProfilePicture { get; set; }
    public DateOnly? BirthDate { get; set; }
    public string? UserId { get; set; }
}
