using System.ComponentModel.DataAnnotations;

namespace UniAdmissionPlatform.BusinessTier.Requests.User
{
    public class LoginRequest
    {
        [Required] public string FirebaseToken { get; set; }
    }
}