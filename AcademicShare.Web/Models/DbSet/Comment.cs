using System.ComponentModel.DataAnnotations;

namespace AcademicShare.Web.Models.Dtos;

public class Comment
{
	[Key]
	public int CommentId { get; set; }
	public string? Content { get; set; }
	public DateTime CreatedAt { get; set; }
	public int PostCommentId { get; set; }
	public virtual Post? Post { get; set; }
	public string? UserCommentId { get; set; }
    public virtual User? User { get; set; }
}
