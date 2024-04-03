using AcademicShare.Web.Context;
using AcademicShare.Web.Models;
using AcademicShare.Web.Models.Dtos;
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

        var viewProfile = _mapper.Map<ViewProfileDto>(user);

        return View(viewProfile);
    }

    [HttpPost("Profile/{UserName}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(
        [Bind("Profile")]
        ViewProfileDto UserProfile)
    {
        Console.WriteLine(UserProfile.Profile.Banner + " fsdfsdf");
        if (ModelState.IsValid)
        {
            string fileNameBanner;
            string fileNameProfilePicture;

            if (UserProfile.Profile.BannerFile is not null)
                fileNameBanner = await CreateFile(UserProfile.Profile.BannerFile, "imagesProfile/banner");
            else
                fileNameBanner = UserProfile.Profile.Banner!;

            if (UserProfile.Profile.ProfilePictureFile is not null)
                fileNameProfilePicture = await CreateFile(UserProfile.Profile.ProfilePictureFile, "imagesProfile/avatar");
            else
                fileNameProfilePicture = UserProfile.Profile.ProfilePicture!;

            var user = await _userManager.GetUserAsync(User);
            var profileToUpdate = await _context.Profiles.FirstOrDefaultAsync(p => p.UserId == user!.Id);
            

            DeleteFile(profileToUpdate!.Banner!, fileNameBanner, "banner");
            DeleteFile(profileToUpdate.ProfilePicture!, fileNameProfilePicture, "avatar");
            
            profileToUpdate.Bio = UserProfile.Profile.Bio;
            profileToUpdate.Location = UserProfile.Profile.Location;
            profileToUpdate.Website = UserProfile.Profile.Website;
            profileToUpdate.Banner = fileNameBanner;
            profileToUpdate.ProfilePicture = fileNameProfilePicture;
            profileToUpdate.UserId = user!.Id;
            profileToUpdate.UserProfileId = UserProfile.Profile.UserProfileId;
            profileToUpdate.BirthDate = UserProfile.Profile.BirthDate;
            profileToUpdate.FullName = UserProfile.Profile.FullName;

            _context.Update(profileToUpdate);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { UserName = user.UserName });
        }

        return View(UserProfile);
    }

    [HttpGet("Profile/{UserName}/Edit")]
    public async Task<IActionResult> Edit(string? UserName)
    {
        var user = await _context.Users.FirstOrDefaultAsync(m => m.UserName.Equals(UserName));

        if (user is null) return NotFound();

        var profile = await _context.Profiles.FirstOrDefaultAsync(p => p.UserId == user.Id);

        if (profile is null) return NotFound();

        if (user.Id != _userManager.GetUserId(User)) return Forbid();

        ViewBag.User = user;
        return View(user);
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
