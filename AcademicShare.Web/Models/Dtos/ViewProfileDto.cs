using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademicShare.Web.Models.Dtos;

public class ViewProfileDto
{
    public string? Id { get; set; }
    public string? UserName { get; set; }
    public string University { get; set; } = string.Empty;
    public string Registration { get; set; } = string.Empty;
    public string Course { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public string ProfileId { get; set; } = string.Empty;
    public virtual UserProfile? Profile { get; set; }
    public ICollection<Comment>? Comments { get; set; }
    public ICollection<Post>? Posts { get; set; }
    public DateTime CreateAt { get; set; }
}

