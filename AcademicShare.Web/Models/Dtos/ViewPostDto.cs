using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademicShare.Web.Models.Dtos;

public class ViewPostDto
{
    public Guid PostId { get; set; }
    public string? UserId { get; set; }
    public string? PostTitle { get; set; }
    public string? PostContent { get; set; }
    public string? PostImage { get; set; }
    public string? CommentContent { get; set; }
    public string? CommentUserId { get; set; }
    public List<Comment>? Comments { get; set; }
    public DateTime CreatedAt { get; set; }
}
