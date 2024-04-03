using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademicShare.Web.Models.Dtos;

public class ViewProfileDto
{
    public string? Id { get; set; }
    public string? UserName { get; set; }
    public string? Image { get; set; }
    public string? ProfileId { get; set; }
    public virtual UserProfile? Profile { get; set; }
    public ICollection<Comment>? Comments { get; set; }
    public ICollection<Post>? Posts { get; set; }
    public DateTime CreateAt { get; set; }
}

