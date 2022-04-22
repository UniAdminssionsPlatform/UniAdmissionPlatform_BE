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
        [String]
        public int IdentifyId { get; set; }
    }
    public class RoleViewModelWithRoles : RoleBaseViewModel
    {
        
    }
}