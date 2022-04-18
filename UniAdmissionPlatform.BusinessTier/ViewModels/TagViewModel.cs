using UniAdmissionPlatform.BusinessTier.Commons.Attributes;
namespace UniAdmissionPlatform.BusinessTier.ViewModels
{
    public class TagBaseViewModel
    {
        public int? Id { get; set; }
        [String]
        public string Name { get; set; }
    }
    
    public class TagViewModelWithEvents : TagBaseViewModel
    {
        // public virtual ICollection<EventBaseModel> Events { get; set; }
    }
}