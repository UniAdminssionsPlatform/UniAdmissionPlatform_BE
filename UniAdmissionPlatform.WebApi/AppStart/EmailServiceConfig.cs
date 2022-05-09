using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.DependencyInjection;
using UniAdmissionPlatform.BusinessTier.Services;

namespace UniAdmissionPlatform.WebApi.AppStart
{
    public static class EmailServiceConfig
    {
        public static void InitEmailService(this IServiceCollection services)
        {
            services.AddTransient<IMailService, MailService>();
            services.AddTransient<IMailBookingService, MailBookingService>();
        }
    }
}