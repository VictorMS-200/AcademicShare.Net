

namespace AcademicShare.Web.Models.Dtos;

public class ViewUserDto
{
    public Guid Id { get; set; }
    public string? UserName { get; set; }
    public string? University { get; set; }
	public string? Registration { get; set; }
	public string? Course { get; set; }
    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
    public virtual ICollection<Follow> Follow { get; set; } = new List<Follow>();
}
