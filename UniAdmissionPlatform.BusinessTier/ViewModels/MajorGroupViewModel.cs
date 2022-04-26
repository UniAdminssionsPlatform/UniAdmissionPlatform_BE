using UniAdmissionPlatform.BusinessTier.Commons.Attributes;

namespace UniAdmissionPlatform.BusinessTier.ViewModels
{
    public class MajorGroupBaseViewModel
    {
        public int? Id { get; set; }
        [String]
        public string Name { get; set; }
    }
}