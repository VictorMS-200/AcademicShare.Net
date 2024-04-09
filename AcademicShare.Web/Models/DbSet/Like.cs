using System.ComponentModel.DataAnnotations;

namespace AcademicShare.Web.Models.Dtos;


public class Like
{
    [Key]
    public Guid LikeId { get; set; }
    public virtual required Post Post { get; set; }
    public virtual required User User { get; set; }
}
