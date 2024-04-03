using System.ComponentModel.DataAnnotations;


namespace AcademicShare.Web.Models.Dtos;

public class Post
{
    [Key]
    public int PostId { get; set; }
    [Required, MaxLength(80, ErrorMessage = "Title can't be extended 80 character")]
    public string Title { get; set; } = string.Empty;
    [Required, MaxLength(500, ErrorMessage = "Content can't be extended 500 character")]
    public string Content { get; set; } = string.Empty;
	public string Image { get; set; } = string.Empty;
    public string Teacher { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
	public string? UserId { get; set; }
    public virtual User? User { get; set; }
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public virtual ICollection<Like> Likes { get; set; } = new List<Like>();
}