using System.ComponentModel.DataAnnotations;

namespace UniAdmissionPlatform.BusinessTier.Requests.Casbin
{
    public class GetAllPermissionsRequest
    {
        [Required]
        public string Role { get; set; }
    }
}