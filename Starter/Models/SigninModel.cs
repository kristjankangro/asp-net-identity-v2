using System.ComponentModel.DataAnnotations;

namespace IdentityNetCore.Models;

public class SigninModel
{
	[Required(ErrorMessage = "Username is required."), DataType(DataType.EmailAddress)]
	public string Username { get; set; }
	
	[Required(ErrorMessage = "Password is required.")]
	public string Password { get; set; }
	
	public bool RememberMe { get; set; }
}