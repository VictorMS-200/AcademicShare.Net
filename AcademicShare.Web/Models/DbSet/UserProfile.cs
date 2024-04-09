using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademicShare.Web.Models.Dtos;

public class UserProfile
{
    [Key]
    public int UserProfileId { get; set; }
    [MaxLength(160)]
    public string? Bio { get; set; }
    public string? Location { get; set; }
    public string? FullName { get; set; }
    public string? Website { get; set; }
    public string? Banner { get; set; }
    [NotMapped]
    public IFormFile? BannerFile { get; set; }
    public string? ProfilePicture { get; set; }
    [NotMapped]
    public IFormFile? ProfilePictureFile { get; set; }
    public DateOnly? BirthDate { get; set; }
    [ForeignKey("Profile")]
    public virtual User? User { get; set; }
}
