using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace AcademicShare.Web.Models;


public class User : IdentityUser
{
	[Required]
	public string University { get; set; } = string.Empty;
	[Required]
	public string Registration { get; set; } = string.Empty;
	[Required]
	public string Course { get; set; } = string.Empty;
	public string Image { get; set; } = string.Empty;
	public string ProfileId { get; set; } = string.Empty;
	
    public virtual UserProfile? Profile { get; set; }
	public ICollection<Comment> Comments { get; set; } = new List<Comment>();
	public ICollection<Post> Posts { get; set; } = new List<Post>();
	public DateTime CreateAt { get; set; }
	public DateTime UpdateAt { get; set; }
	public bool IsTeacher { get; set; }
}

