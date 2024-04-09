using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AcademicShare.Web.Models.Dtos;


public class Follow
{
    [Key]
    public Guid Id { get; set; }
    public string? FollowerId { get; set; }
    public string? FollowedId { get; set; }
}
