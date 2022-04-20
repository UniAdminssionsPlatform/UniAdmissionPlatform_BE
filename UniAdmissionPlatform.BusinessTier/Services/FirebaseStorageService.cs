using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Storage;
using Microsoft.Extensions.Configuration;
using UniAdmissionPlatform.BusinessTier.Responses;

namespace UniAdmissionPlatform.BusinessTier.Services
{
    public interface IFirebaseStorageService
    {
        Task<string> UploadImage(string extension, Stream fileStream);
    }

    public class FirebaseStorageService : IFirebaseStorageService
    {
        private readonly IConfiguration _configuration;

        public FirebaseStorageService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        
        public async Task<string> UploadImage(string extension, Stream fileStream)
        {
            var auth = new FirebaseAuthProvider(new FirebaseConfig(_configuration["Firebase:Api-key"]));
            var a = await auth.SignInWithEmailAndPasswordAsync(_configuration["FirebaseStoreAccount:Username"],
                _configuration["FirebaseStoreAccount:Password"]);

            Console.WriteLine(a.FirebaseToken);

            //todo: need config more
            var task = new FirebaseStorage(
                    _configuration["FirebaseStoreAccount:Bucket"],
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                        ThrowOnCancel = true,
                    })
                .Child("data")
                .Child("random")
                .Child(Path.GetRandomFileName() + DateTime.Now.ToString(CultureInfo.CurrentCulture)  + extension)
                .PutAsync(fileStream);

            var fileUrlWithParams = await task;
            var fileUrl = fileUrlWithParams.Split('?')[0] + "?alt=media";

            return fileUrl;
        }
        
        public async Task DeleteFile(string fileName)
        {
            var auth = new FirebaseAuthProvider(new FirebaseConfig(_configuration["Firebase:Api-key"]));
            var a = await auth.SignInWithEmailAndPasswordAsync(_configuration["FirebaseStoreAccount:Username"],
                _configuration["FirebaseStoreAccount:Password"]);

            var task = new FirebaseStorage(
                    _configuration["FirebaseStoreAccount:Bucket"],
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                        ThrowOnCancel = true,
                    })
                .Child("data")
                .Child("random")
                .Child("file.png")
                .DeleteAsync();

            await task;
        }
    }
}