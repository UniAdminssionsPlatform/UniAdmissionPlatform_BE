using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using UniAdmissionPlatform.BusinessTier.Requests.Mail;
using MailSettings = UniAdmissionPlatform.BusinessTier.Settings.MailSettings;

namespace UniAdmissionPlatform.BusinessTier.Services
{
    public interface IMailService
    {
        Task SendHtmlEmailAsync(MailRequest mailRequest);
    }

    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        public MailService(IConfiguration configuration)
        {
            _mailSettings = new MailSettings
            {
                ApiKey = configuration["EmailService:ApiKey"],
                Mail = configuration["EmailService:Mail"],
                DisplayName = configuration["EmailService:DisplayName"],
            };
        }
        
        
        public async Task SendHtmlEmailAsync(MailRequest mailRequest)
        {
            if (string.IsNullOrWhiteSpace(mailRequest.ToEmail))
            {
                return;
            }
            var fromEmail = new EmailAddress(_mailSettings.Mail, _mailSettings.DisplayName);
            var client = new SendGridClient(_mailSettings.ApiKey);
            var subject = mailRequest.Subject;
            var toEmail = new EmailAddress(mailRequest.ToEmail, "User");
            var htmlContent = mailRequest.HtmlBody;
            var planTextContent = mailRequest.Body;
            var sendGridMessage = MailHelper.CreateSingleEmail(fromEmail, toEmail, subject, planTextContent, htmlContent);
            await client.SendEmailAsync(sendGridMessage);
        }
    }
}