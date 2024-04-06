using AcademicShare.Web.Context;
using AcademicShare.Web.Models.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AcademicShare.Web.Controllers;


[Route("[controller]")]
public class CommentController : Controller
{
    private readonly ILogger<CommentController> _logger;
    private readonly UserManager<User> _userManager;
    private readonly AcademicShareDbContext _context;

    public CommentController(
        ILogger<CommentController> logger,
        UserManager<User> userManager,
        AcademicShareDbContext context)
    {
        _logger = logger;
        _userManager = userManager;
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> ViewPost(Comment comment)
    {
        if (ModelState.IsValid)
        {
            comment.CreatedAt = DateTime.Now;
            #pragma warning disable CS8601 // Possible null reference assignment.
            comment.User.Id = _userManager.GetUserId(User);
            #pragma warning restore CS8601 // Possible null reference assignment.

            _context.Comments.Add(comment);
            
            await _context.SaveChangesAsync();
            return RedirectToAction("ViewPost", new { id = comment.Post.PostId });
        }
        return View(comment);
    }
}
