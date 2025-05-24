using System.ComponentModel.DataAnnotations;

namespace IdentityNetCore.Models;

public class SignupViewModel
{
	[Required, DataType(DataType.EmailAddress, ErrorMessage = "Email not valid")]
	public string Email { get; set; }

	[Required, DataType(DataType.Password, ErrorMessage = "Password incirrect")]
	public string Password { get; set; }
}