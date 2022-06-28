using UniAdmissionPlatform.BusinessTier.Commons.Attributes;

namespace UniAdmissionPlatform.BusinessTier.ViewModels
{
    public class MajorGroupBaseViewModel
    {
        public int? Id { get; set; }
        [String]
        public string Name { get; set; }
        [String]
        public string Description { get; set; }
        [String]
        public string ThumbnailUrl { get; set; }
    }
    
    public class MajorGroupNameViewModel
    {
        public string Name { get; set; }
    }
}