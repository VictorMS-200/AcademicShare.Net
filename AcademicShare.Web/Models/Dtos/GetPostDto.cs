using System.ComponentModel.DataAnnotations;

namespace AcademicShare.Web.Models.Dtos;


public class GetPostDto
{
    [Key]
    public Guid PostId { get; set; }
    [Required, MaxLength(80, ErrorMessage = "Title can't be extended 80 character")]
    public required string Title { get; set; }
    [Required, MaxLength(10000, ErrorMessage = "Content can't be extended 10000 character")]
    public required string Content { get; set; }
    public required string Resume { get; set; }
    [Required]
    public required IFormFile Image { get; set; }  
    public required string Teacher { get; set; }
}
