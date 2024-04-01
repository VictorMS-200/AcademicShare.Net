using AcademicShare.Web.Context;
using AcademicShare.Web.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AcademicShare.Web.Controllers;

public class ProfileController : Controller
{
    private readonly AcademicShareDbContext _context;
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly IWebHostEnvironment _hostEnvironment;
    private readonly IMapper _mapper;


    public ProfileController(
        AcademicShareDbContext context, 
        UserManager<User> userManager, 
        SignInManager<User> signInManager, 
        IWebHostEnvironment hostEnvironment,
        IMapper mapper)
    {
        _hostEnvironment = hostEnvironment;
        _context = context;
        _signInManager = signInManager;
        _userManager = userManager;
        _mapper = mapper;
    }

    [HttpGet("Profile/{UserName}")]
    public async Task<IActionResult> Index(string? UserName)
    {
        var user = await _context.Users.FirstOrDefaultAsync(m => m.UserName!.Equals(UserName));
        if (user is null) return NotFound();

        var profile = await _context.Profiles.FirstOrDefaultAsync(p => p.UserId == user.Id);
        if (profile is null) return NotFound();

        ViewBag.User = user;
        ViewBag.PostsCount = await _context.Posts.Where(p => p.UserId == user.Id).CountAsync();
        ViewBag.Posts = await _context.Posts.Where(p => p.UserId == user.Id).Take(6).OrderByDescending(p => p.CreatedAt).ToListAsync();

        return View(profile);
    }

    [HttpPost]
    public async Task<IActionResult> Index(
        UserProfile profile)
    {
        if (ModelState.IsValid)
        {
            string fileNameBanner;
            string fileNameProfilePicture;

            if (profile.BannerFile is not null)
                fileNameBanner = await CreateFile(profile.BannerFile, "imagesProfile/banner");
            else
                fileNameBanner = profile.Banner!;

            if (profile.ProfilePictureFile is not null)
                fileNameProfilePicture = await CreateFile(profile.ProfilePictureFile, "imagesProfile/avatar");
            else
                fileNameProfilePicture = profile.ProfilePicture!;

            var user = await _userManager.GetUserAsync(User);
            var profileToUpdate = await _context.Profiles.FirstOrDefaultAsync(p => p.UserId == user!.Id);
            

            DeleteFile(profileToUpdate!.Banner!, fileNameBanner, "banner");
            DeleteFile(profileToUpdate.ProfilePicture!, fileNameProfilePicture, "avatar");

            profileToUpdate.Bio = profile.Bio;
            profileToUpdate.Location = profile.Location;
            profileToUpdate.Website = profile.Website;
            profileToUpdate.Banner = fileNameBanner;
            profileToUpdate.ProfilePicture = fileNameProfilePicture;
            profileToUpdate.UserId = user!.Id;
            profileToUpdate.UserProfileId = profile.UserProfileId;
            profileToUpdate.BirthDate = profile.BirthDate;
            profileToUpdate.FullName = profile.FullName;

            Console.WriteLine(profileToUpdate.UserProfileId);
            _context.Update(profileToUpdate);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { UserName = user.UserName });
        }

        return View(profile);
    }

    public async Task<string> CreateFile(
        IFormFile file, 
        string pathString)
    {
        string fileName;

        if (file is null) return Path.Combine(pathString, "default.png");

        var wwwRootPath = _hostEnvironment.WebRootPath;
        fileName = Guid.NewGuid().ToString() + "_" + file.FileName;
        var path = Path.Combine(wwwRootPath + "/" + pathString, fileName);

        using (var fileStream = new FileStream(path, FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }
        return fileName;
    }

    public void DeleteFile(
        string  fileName1, string fileName2, string pathString)
    {
        if (fileName1 is not null)
        {
            if (fileName1 != "default.png" && fileName1 != fileName2)
            {
                var path = Path.Combine(_hostEnvironment.WebRootPath + "/imagesProfile/" + pathString ,fileName1);
                Console.WriteLine(path);
                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);
            }
        }
    }
}
