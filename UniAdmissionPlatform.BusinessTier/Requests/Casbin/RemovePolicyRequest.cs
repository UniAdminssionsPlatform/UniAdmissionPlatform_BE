using System.ComponentModel.DataAnnotations;

namespace UniAdmissionPlatform.BusinessTier.Requests.Casbin
{
    public class RemovePolicyRequest
    {
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Object { get; set; }
        [Required]
        public string Action { get; set; }
    }
}