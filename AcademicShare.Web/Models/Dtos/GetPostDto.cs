using System.ComponentModel.DataAnnotations;

namespace AcademicShare.Web.Models.Dtos;


public class GetPostDto
{
    [Key]
    public Guid PostId { get; set; }
    [Required, MaxLength(80, ErrorMessage = "Title can't be extended 80 character")]
    public string Title { get; set; } = string.Empty;
    [Required, MaxLength(500, ErrorMessage = "Content can't be extended 500 character")]
    public string Content { get; set; } = string.Empty;
    [Required]
    public required IFormFile Image { get; set; }  
    public string Teacher { get; set; }  = string.Empty;
}
