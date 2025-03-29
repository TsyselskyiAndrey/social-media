using Glowee.Application.Models.Email;

namespace Glowee.Application.Contracts.Email
{
    public interface IEmailSender
    {
        Task SendEmailAsync(EmailMessage email);
    }
}
