using System.Threading.Tasks;
using Microsoft.Extensions.Options;
namespace IdentityNetCore.Service;
using MailAddress = System.Net.Mail.MailAddress;
using MailMessage = System.Net.Mail.MailMessage;
using NetworkCredential = System.Net.NetworkCredential;
using SmtpClient = System.Net.Mail.SmtpClient;

public class SmtpEmailSender : IEmailSender
{
	private readonly IOptions<SmtpOptions> _options;

	public SmtpEmailSender(IOptions<SmtpOptions> options)
	{
		_options = options;
	}

	public async Task SendEmailAsync(string fromEmail, string toEmail, string subject, string message)
	{
		var mailMessage = new MailMessage
		{
			From = new MailAddress(fromEmail),
			Subject = subject,
			Body = message,
			To = { new MailAddress(toEmail) }
		};
		using var client = new SmtpClient(_options.Value.Host, _options.Value.Port)
		{
			Credentials = new NetworkCredential(_options.Value.UserName, _options.Value.Password),
			EnableSsl = true
		};
		await client.SendMailAsync(mailMessage);
	}
}