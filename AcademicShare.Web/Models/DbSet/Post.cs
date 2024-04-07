using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace AcademicShare.Web.Models.Dtos;

public class Post
{
    [Key]
    public Guid PostId { get; set; }
    [Required, MaxLength(80, ErrorMessage = "Title can't be extended 80 character")]
    public required string Title { get; set; }
    [Required, MaxLength(10000, ErrorMessage = "Content can't be extended 10000 character")]
    public required string Content { get; set; }
    public required string Resume { get; set; }
	public required string Image { get; set; }
    public required string Teacher { get; set; }
    [ForeignKey("Posts")]
    public required User User { get; set; }
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<Like> Likes { get; set; } = new List<Like>();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}