using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.DependencyInjection;
using UniAdmissionPlatform.BusinessTier.Services;

namespace UniAdmissionPlatform.WebApi.AppStart
{
    public static class FirebaseServiceConfig
    {
        public static void InitFirebase(this IServiceCollection services)
        {
            FirebaseApp.Create(new AppOptions
            {
                Credential =
                    GoogleCredential.FromFile("Resources/Firebase/uni-admission-platform-firebase-adminsdk-wip1g-4b8c762fc1.json")
            });
            services.AddTransient<IFirebaseService, FirebaseService>();
        }
    }
}