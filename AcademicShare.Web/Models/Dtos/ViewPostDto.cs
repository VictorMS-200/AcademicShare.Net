using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademicShare.Web.Models.Dtos;

public class ViewPostDto
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public string? Image { get; set; }
    public string? UserId { get; set; }
    public string? CommentContent { get; set; }
    public string? CommentUserId { get; set; }
    public Comment? Comment { get; set; }
    public List<Comment>? Comments { get; set; }
    public DateTime CreatedAt { get; set; }
}
