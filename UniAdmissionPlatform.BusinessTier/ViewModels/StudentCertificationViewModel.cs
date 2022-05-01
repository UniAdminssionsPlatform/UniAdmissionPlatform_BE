using UniAdmissionPlatform.BusinessTier.Commons.Attributes;

namespace UniAdmissionPlatform.BusinessTier.ViewModels
{
    public partial class StudentCertificationBaseViewModel
    {
        public int? StudentId { get; set; }
        public int? CertificationId { get; set; }
        [String]
        public string Name { get; set; }
        [String]
        public string Description { get; set; }
        [String]
        public string ImageUrl { get; set; }
        public float? Score { get; set; }
        
    }
}