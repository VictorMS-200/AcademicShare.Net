using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace AcademicShare.Web.Models.Dtos;


public class User : IdentityUser
{
	public required string University { get; set; } 
	public required string Registration { get; set; }
	public required string Course { get; set; }
    public virtual UserProfile? Profile { get; set; }
	public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
	public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
	public virtual ICollection<Like> Likes { get; set; } = new List<Like>();
	public virtual ICollection<Follow> Follow { get; set; } = new List<Follow>();
	public DateTime CreateAt { get; set; }
	public DateTime UpdateAt { get; set; }
}

