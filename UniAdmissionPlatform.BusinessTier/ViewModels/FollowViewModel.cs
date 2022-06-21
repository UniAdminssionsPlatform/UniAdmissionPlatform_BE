using UniAdmissionPlatform.BusinessTier.Commons.Attributes;
namespace UniAdmissionPlatform.BusinessTier.ViewModels
{
    public class FollowBaseViewModel
    {
        public int? Status { get; set; }
        public int? UniversityId { get; set; }
        public int? StudentId { get; set; }
    }
}