

using AcademicShare.Web.Context;
using AcademicShare.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AcademicShare.Web.Controllers
{
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
            comment.UserCommentId = _userManager.GetUserId(User);
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction("ViewPost", new { id = comment.PostCommentId });
        }
        return View(comment);
    }
    }
}