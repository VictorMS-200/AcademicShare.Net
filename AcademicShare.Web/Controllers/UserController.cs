using Microsoft.AspNetCore.Mvc;
using AcademicShare.Web.Models.Dtos;
using AcademicShare.Web.Context;
using AcademicShare.Web.Models.Paginated;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using System.Data.Entity;

namespace AcademicShare.Web.Controllers;


[Route("[controller]")]
public class UserController : Controller
{
    private readonly ILogger<Like> _logger;
    private readonly AcademicShareDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    public UserController(
        ILogger<Like> logger,
        AcademicShareDbContext context,
        UserManager<User> userManager,
        SignInManager<User> signInManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string SearchString, int page = 1, int pageSize = 10)
    {
        if(SearchString != null)
        {
            page = 1;
        }

        var users = _context.Users;
        
        var usersList = from m in users
                        select m;
        
        if (!string.IsNullOrEmpty(SearchString))
            usersList = usersList.Where(s => s.UserName!.Contains(SearchString));

        return View(await PaginatedList<User>.CreateAsync(
            usersList.AsNoTracking(), page, pageSize));
    }
}
