using UniAdmissionPlatform.BusinessTier.Commons.Attributes;
namespace UniAdmissionPlatform.BusinessTier.ViewModels
{
    public class TagBaseViewModel
    {
        public int? Id { get; set; }
        [String]
        public string Name { get; set; }
    }
    
    public class TagViewModelWithTags : TagBaseViewModel
    {
        // public virtual ICollection<TagBaseModel> Tags { get; set; }
    }
}