using System;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.DependencyInjection;
using UniAdmissionPlatform.BusinessTier.Services;
using UniAdmissionPlatform.Firestore;

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
            var path = Environment.CurrentDirectory;
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS" , path + "/Resources/Firebase/uni-admission-platform-firebase-adminsdk-wip1g-4b8c762fc1.json");
            services.AddScoped<ICommentService, CommentService>();
            services.AddTransient<IFirebaseService, FirebaseService>();
        }
    }
}