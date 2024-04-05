using Microsoft.AspNetCore.Mvc;
using AcademicShare.Web.Models.Dtos;
using AcademicShare.Web.Context;
using AcademicShare.Web.Models.Paginated;
using Microsoft.AspNetCore.Identity;

namespace AcademicShare.Web.Controllers;


[Route("[controller]")]
public class LikeController : Controller
{
    private readonly ILogger<Like> _logger;
    private readonly AcademicShareDbContext _context;
    private readonly UserManager<User> _userManager;

    public LikeController(
        ILogger<Like> logger,
        AcademicShareDbContext context,
        UserManager<User> userManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
    }
}
