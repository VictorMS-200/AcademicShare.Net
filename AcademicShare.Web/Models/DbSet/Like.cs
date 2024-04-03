using System.ComponentModel.DataAnnotations;

namespace AcademicShare.Web.Models.Dtos;


public class Like
{
    [Key]
    public int LikeId { get; set; }
    public int PostLikeId { get; set; }
    public virtual Post? Post { get; set; }
    public string? UserLikeId { get; set; }
    public virtual User? User { get; set; }
}
