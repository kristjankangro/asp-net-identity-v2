namespace IdentityNetCore.Service;

public class SmtpOptions
{
	public string Host { get; set; } = string.Empty;
	public int Port { get; set; } = 25;
	public string UserName { get; set; } = string.Empty;
	public string Password { get; set; } = string.Empty;
}