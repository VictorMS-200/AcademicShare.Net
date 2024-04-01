using AcademicShare.Web.Models;
using AcademicShare.Web.Models.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AcademicShare.Web.Controllers;


public class AccountController : Controller
{
	private readonly UserManager<User> _userManager;
	private readonly SignInManager<User> _signInManager;
	private readonly IMapper _mapper;

	public AccountController(
		UserManager<User> userManager, 
		SignInManager<User> signInManager,
		IMapper mapper)
	{
		_userManager = userManager;
		_signInManager = signInManager;
		_mapper = mapper;
	}

	[HttpGet]
	public IActionResult Register()
	{
		return View();
	}

	[HttpPost]
	public async Task<IActionResult> Register(
		[Bind(include: "Email,FullName,UserName,University,Course,Registration,Password,ConfirmPassword")]
		CreateUserDto model)
	{
		if (ModelState.IsValid)
		{
			var emailIsInUse = await _userManager.FindByEmailAsync(model.Email!);

			if (emailIsInUse is not null)
			{
				ModelState.AddModelError(string.Empty, "Email is already in use");
				return View(model);
			}
			
			var UserProfile = _mapper.Map<User>(model);

			var result = await _userManager.CreateAsync(UserProfile, model.Password!);

			if (result.Succeeded)
			{
				await _signInManager.SignInAsync(UserProfile, isPersistent: false);
				return RedirectToAction("Index", "Home");
			}

			foreach (var error in result.Errors)
			{
				ModelState.AddModelError(string.Empty, error.Description);
			}
		}

		return View(model);
	}

	[HttpGet]
	public IActionResult Login()
	{
		return View();
	}

	[HttpPost]
	public async Task<IActionResult> Login(
		UserLoginDto model)
	{
		if (ModelState.IsValid)
		{
			var user = await _userManager.FindByEmailAsync(model.Email!);

			if (user is null)
			{
				ModelState.AddModelError(string.Empty, "Email not found");
				return View(model);
			}
			var result = await _signInManager.PasswordSignInAsync(
				user.UserName!, model.Password!, model.RememberMe, false);

			if (result.Succeeded)
			{
				return RedirectToAction("Index", "Home");
			}

			ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
		}

		return View(model);
	}

	public async Task<IActionResult> Logout()
	{
		await _signInManager.SignOutAsync();
		return RedirectToAction("Index", "Home");
	}
}
