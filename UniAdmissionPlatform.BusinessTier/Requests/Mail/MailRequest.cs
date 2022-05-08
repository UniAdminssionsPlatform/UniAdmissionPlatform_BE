using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace UniAdmissionPlatform.BusinessTier.Requests.Mail
{
    public class MailRequest
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string HtmlBody { get; set; }
        public List<IFormFile> Attachments { get; set; }
    }
}