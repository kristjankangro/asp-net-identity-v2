using System.Threading.Tasks;
using IdentityNetCore.Models;
using IdentityNetCore.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityNetCore.Controllers;

public class IdentityController: Controller
{
	
	private readonly UserManager<IdentityUser> _userManager;
	private readonly IEmailSender _emailSender;

	public IdentityController(UserManager<IdentityUser> userManager, IEmailSender emailSender)
	{
		_userManager = userManager;
		_emailSender = emailSender;
	}

	[HttpPost]
	public async Task<IActionResult> Signup(SignupViewModel model)
	{
		if (ModelState.IsValid)
		{
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

					await _emailSender.SendEmailAsync(
						"info@gmail.com",
						user.Email, 
						"Confirm your email",
						$"Please confirm your email by clicking.{confirmLink}"
						);
					return RedirectToAction("Signin");
				}
				
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
			}
		}
		
		return View(model);
	}
	
	public async Task<IActionResult> ConfirmEmail(string userId, string token)
	{
		return Ok();
		// if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
		// {
		// 	return BadRequest("Invalid user ID or token.");
		// }
		//
		// var user = await _userManager.FindByIdAsync(userId);
		// if (user == null)
		// {
		// 	return NotFound("User not found.");
		// }
		//
		// var result = await _userManager.ConfirmEmailAsync(user, token);
		// if (result.Succeeded)
		// {
		// 	return View("ConfirmEmailSuccess");
		// }
		//
		// return View("ConfirmEmailFailure");
	}
	
	public async Task<IActionResult> Signup()
	{
		return View("Signup");
	}
	
	public async Task<IActionResult> Signin()
	{
		// This is the main entry point for the Identity UI.
		// You can customize this to redirect to a specific page or return a view.
		return View();
	}	public async Task<IActionResult> AccessDenied()
	{
		// This is the main entry point for the Identity UI.
		// You can customize this to redirect to a specific page or return a view.
		return View();
	}
}