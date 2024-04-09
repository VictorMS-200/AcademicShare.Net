using System;
using System.ComponentModel.DataAnnotations;

namespace AcademicShare.Web.Models.Dtos;

public class Comment
{
	[Key]
	public Guid CommentId { get; set; }
	public required string Content { get; set; }
	public virtual required Post Post { get; set; }
    public virtual required User? User { get; set; }
	public DateTime CreatedAt { get; set; }
}
