using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AcademicShare.Web.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using AcademicShare.Web.Models;
using AcademicShare.Web.Models.Dtos;
using AutoMapper;
using AcademicShare.Web.Models.Paginated;
using System;

namespace AcademicShare.Web.Controllers;

[Authorize]
public class PostsController : Controller
{
    private readonly AcademicShareDbContext _context;
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly IWebHostEnvironment _hostEnvironment;
    private readonly IMapper _mapper;

    public PostsController(
        AcademicShareDbContext context,
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IWebHostEnvironment hostEnvironment,
        IMapper mapper)
    {
        _context = context;
        _signInManager = signInManager;
        _userManager = userManager;
        _hostEnvironment = hostEnvironment;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> Index(
        string searchString,
        int? pageNumber,
        string sortOrder)
    {
        ViewData["CurrentSort"] = sortOrder;
        if (searchString != null)
        {
            pageNumber = 1;
        }

        if (_context.Posts == null) return Problem("Entity set is null.");

        var posts = from m in _context.Posts
                    select m;

        if (!string.IsNullOrEmpty(searchString))
            posts = posts.Where(s => s.Title!.Contains(searchString));

        ViewBag.User = await _context.Users.FirstOrDefaultAsync(p => p.Id == _userManager.GetUserId(User));

        int pageSize = 9;
        return View(
            await PaginatedList<Post>.CreateAsync(
            posts.OrderByDescending(c => c.CreatedAt)
                .AsNoTracking()
                .Include(p => p.Posts)
                .Include(p => p.Comments)
                .Include(p => p.Likes),
            pageNumber ?? 1, pageSize));
    }

    // GET: Posts/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var post = await _context.Posts
            .FirstOrDefaultAsync(m => m.PostId.Equals(id));
        if (post == null) return NotFound();

        return View(post);
    }

    public IActionResult Create() => View();


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind(include: "Title,Content,Image,Teacher,Resume")] GetPostDto post)
    {
        if (ModelState.IsValid)
        {
            var fileName = string.Empty;
            if (post.Image is not null)
            {
                var wwwRootPath = _hostEnvironment.WebRootPath;
                fileName = Guid.NewGuid().ToString() + "_" + post.Image.FileName;
                var path = Path.Combine(wwwRootPath + "/imagesPost/", fileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await post.Image.CopyToAsync(fileStream);
                }

            }
            else fileName = "default.png";

            Console.WriteLine(post.Resume);

            var newPost = _mapper.Map<Post>(post);
            newPost.Image = fileName;
            newPost.CreatedAt = DateTime.Now;
            newPost.UpdatedAt = DateTime.Now;
            newPost.Posts = await _userManager.GetUserAsync(User);

            _context.Add(newPost);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(post);
    }

    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null) return NotFound();

        var post = await _context.Posts.AsNoTracking().FirstOrDefaultAsync(p => p.PostId.Equals(id));
        if (post == null) return NotFound();
        return View(post);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        Guid id,
        [Bind("PostId,Title,Content,Image,Teacher,CreatedAt,UpdatedAt,UserId")] Post post)
    {
        if (!id.Equals(post.PostId)) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == post.Posts.Id);
                post.Posts = user!;
                post.UpdatedAt = DateTime.Now;
                _context.Update(post);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(post.PostId)) NotFound();
                else throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(post);
    }

    // GET: Posts/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) NotFound();

        var post = await _context.Posts.FirstOrDefaultAsync(m => m.PostId.Equals(id));

        if (post == null) NotFound();

        return View(post);
    }

    // POST: Posts/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var post = await _context.Posts.FindAsync(id);
        if (post != null)
            _context.Posts.Remove(post);

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet, ActionName("ViewPost")]
    public async Task<IActionResult> ViewPost(Guid id)
    {
        if (id.Equals(null)) return NotFound();

        var post = await _context.Posts.AsNoTracking().FirstOrDefaultAsync(p => p.PostId.Equals(id));

        if (post == null) return NotFound();

        ViewPostDto postView = _mapper.Map<Post, ViewPostDto>(post);

        return View(postView);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ViewPost(
        [Bind(include: "CommentContent,PostId")]
        ViewPostDto Postcomment)
    {
        if (Postcomment == null) return NotFound();
        if (ModelState.IsValid)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == _userManager.GetUserId(User));

            #pragma warning disable CS8601 // Possible null reference assignment.
            var Comment = new Comment
            {
                Content = Postcomment.CommentContent,
                Post = await _context.Posts.FirstOrDefaultAsync(p => p.PostId.Equals(Postcomment.PostId)),
                User = user,
                CreatedAt = DateTime.Now,
            };
            #pragma warning restore CS8601 // Possible null reference assignment.
            _context.Comments.Add(Comment);

            await _context.SaveChangesAsync();

            return RedirectToAction("ViewPost", new { id = Postcomment.PostId });
        }
        return View(Postcomment);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> IndexLike(
        [Bind(include: "PostId")]
        ViewPostDto posts)
    {
        if (posts == null) return NotFound();
        var existingLikes = _context.Likes.Where(l => l.Post.PostId.Equals(posts.PostId) && l.User.Id == _userManager.GetUserId(User));

        if (existingLikes.Any())
        {
            _context.Likes.RemoveRange(existingLikes);
            await _context.SaveChangesAsync();

            return RedirectToAction("ViewPost", new { id = posts.PostId });
        }

        #pragma warning disable CS8601 // Possible null reference assignment.
        var like = new Like
        {
            Post = await _context.Posts.FirstOrDefaultAsync(p => p.PostId.Equals(posts.PostId)),
            User = await _context.Users.FirstOrDefaultAsync(u => u.Id == _userManager.GetUserId(User)),     
        };
        #pragma warning restore CS8601 // Possible null reference assignment.
        _context.Likes.Add(like);

        await _context.SaveChangesAsync();
        return RedirectToAction("ViewPost", new { id = posts.PostId });
    }

    [HttpGet]
    public async Task<IActionResult> LikedPosts(
        string UserName,
        string searchString,
        int? pageNumber,
        string sortOrder)
    {
        ViewData["CurrentSort"] = sortOrder;

        if (searchString != null)
        {
            pageNumber = 1;
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == _userManager.GetUserId(User));
        var likedPosts = _context.Likes.Where(l => l.User.Id == user.Id).Select(l => l.Post.PostId);
        var posts = _context.Posts.Where(p => likedPosts.Contains(p.PostId));

        var likedPostsList = from m in posts
                             select m;
        
        if (!string.IsNullOrEmpty(searchString))
            likedPostsList = likedPostsList.Where(s => s.Title!.Contains(searchString));

        ViewBag.User = await _context.Users.FirstOrDefaultAsync(p => p.Id == _userManager.GetUserId(User));
        
        int pageSize = 9;
        return View(await PaginatedList<Post>.CreateAsync(
            likedPostsList.OrderByDescending(c => c.CreatedAt).AsNoTracking(), pageNumber ?? 1, pageSize));
    }

    private bool PostExists(Guid id) => _context.Posts.Any(e => e.PostId.Equals(id));
}
