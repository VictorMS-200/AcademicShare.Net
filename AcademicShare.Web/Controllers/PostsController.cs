using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AcademicShare.Web.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using AcademicShare.Web.Models;
using System;
using AcademicShare.Web.Models.Dtos;
using AutoMapper;
using AcademicShare.Web.Models.Paginated;

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
        return View(await PaginatedList<Post>.CreateAsync(posts.AsNoTracking(), pageNumber ?? 1, pageSize));
    }

    // GET: Posts/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var post = await _context.Posts
            .FirstOrDefaultAsync(m => m.PostId == id);
        if (post == null) return NotFound();

        return View(post);
    }

    public IActionResult Create() => View();


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind(include: "Title,Content,Image,Teacher")] GetPostDto post)
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


            var newPost = _mapper.Map<Post>(post);
            newPost.Image = fileName;
            newPost.CreatedAt = DateTime.Now;
            newPost.UpdatedAt = DateTime.Now;
            newPost.User = await _userManager.GetUserAsync(User);

            _context.Add(newPost);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(post);
    }

    // GET: Posts/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var post = await _context.Posts.FindAsync(id);
        if (post == null) return NotFound();
        return View(post);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        [Bind("PostId,Title,Content,Image,Teacher,CreatedAt,UpdatedAt,UserId")] Post post)
    {
        if (id != post.PostId) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == post.UserId);
                post.User = user;
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

        var post = await _context.Posts
            .FirstOrDefaultAsync(m => m.PostId == id);
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

    private bool PostExists(int id) => _context.Posts.Any(e => e.PostId == id);
}
