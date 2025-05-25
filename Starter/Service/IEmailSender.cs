using System.Threading.Tasks;

namespace IdentityNetCore.Service;

public interface IEmailSender
{
	Task SendEmailAsync(string fromEmail, string toEmail, string subject, string message);
}