using System.Threading.Tasks;
using IdentityNetCore.Models;
using IdentityNetCore.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityNetCore.Controllers;

public class IdentityController: Controller
{
	
	private readonly UserManager<IdentityUser> _userManager;
	private readonly SignInManager<IdentityUser> _signInManager;
	private readonly IEmailSender _emailSender;

	public IdentityController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,IEmailSender emailSender)
	{
		_userManager = userManager;
		_signInManager = signInManager;
		_emailSender = emailSender;
	}

	[HttpPost]
	public async Task<IActionResult> Signup(SignupViewModel model)
	{
		if (!ModelState.IsValid) return View(model);
		
		if (await _userManager.FindByEmailAsync(model.Email) != null)
		{
			ModelState.AddModelError("Email", "Email already exists.");
		}
		else
		{
			// Here you would typically create the user and save it to the database.
			var user = new IdentityUser { UserName = model.Email, Email = model.Email };
			var result = await _userManager.CreateAsync(user, model.Password);
			user = await _userManager.FindByEmailAsync(model.Email);
			if (result.Succeeded)
			{
				var confirmLink = Url.ActionLink("ConfirmEmail", "Identity", 
					new { userId = user.Id, 
						token = await _userManager.GenerateEmailConfirmationTokenAsync(user) });
				//todo email is not sent, options not coming in
				await _emailSender.SendEmailAsync(
					"info@gmail.com",
					user.Email, 
					"Confirm your email",
					confirmLink
				);
				return RedirectToAction("Signin");
			}
				
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError(string.Empty, error.Description);
			}
		}

		return View(model);
	}
	
	public async Task<IActionResult> ConfirmEmail(string userId, string token)
	{
		var user = await _userManager.FindByIdAsync(userId);
		var result = await _userManager.ConfirmEmailAsync(user, token);
		if (!result.Succeeded)
		{
			ModelState.AddModelError(string.Empty, "Email confirmation failed.");
			return new NotFoundResult();
		}

		return RedirectToAction("Signin");
	}
	
	public async Task<IActionResult> Signup()
	{
		return View("Signup");
	}
	
	public IActionResult Signin() => View(new SigninModel());
	
	[HttpPost]
	public async Task<IActionResult> Signin(SigninModel model)
	{
		if (!ModelState.IsValid) 
			return View(model);

		var result = await _signInManager.PasswordSignInAsync(
			model.Username, 
			model.Password, 
			model.RememberMe,
			false);
		if (result.Succeeded)
		{
			return RedirectToAction("Index", "Home");
		}
		
		ModelState.AddModelError("Login", "Invalid login attempt.");

		return View(model);
	}

	public async Task<IActionResult> AccessDenied()
	{
		// This is the main entry point for the Identity UI.
		// You can customize this to redirect to a specific page or return a view.
		return View();
	}
}