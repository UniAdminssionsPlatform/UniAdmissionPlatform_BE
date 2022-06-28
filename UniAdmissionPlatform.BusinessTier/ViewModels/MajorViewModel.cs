using UniAdmissionPlatform.BusinessTier.Commons.Attributes;

namespace UniAdmissionPlatform.BusinessTier.ViewModels
{
    public class MajorBaseViewModel
    {
        public int? Id { get; set; }
        [String]
        public string Name { get; set; }
        [String]
        public string Code { get; set; }
        public int? MajorGroupId { get; set; }
    }
    
    public class MajorViewModelWithMajorGroup : MajorBaseViewModel
    {
        public MajorGroupNameViewModel MajorGroupNameViewModel { get; set; }
    }
}