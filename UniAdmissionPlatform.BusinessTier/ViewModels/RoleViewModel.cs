using UniAdmissionPlatform.BusinessTier.Commons.Attributes;

namespace UniAdmissionPlatform.BusinessTier.ViewModels
{
    public class RoleBaseViewModel
    {
        [String]
        public string Id { get; set; }
        [String]
        public string Name { get; set; }
        [String]
        public string Description { get; set; }
    }
    public class RoleViewModelWithEvents : RoleBaseViewModel
    {
        
    }
}