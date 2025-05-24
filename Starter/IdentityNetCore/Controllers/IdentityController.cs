using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace IdentityNetCore.Controllers;

public class IdentityController: Controller
{
	public async Task<IActionResult> Signup()
	{
		// This is the main entry point for the Identity UI.
		// You can customize this to redirect to a specific page or return a view.
		return View();
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