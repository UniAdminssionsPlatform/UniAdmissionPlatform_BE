using UniAdmissionPlatform.BusinessTier.Commons.Attributes;
namespace UniAdmissionPlatform.BusinessTier.ViewModels
{
    public class EventTypeBaseViewModel
    {
        public int? Id { get; set; }
        [String]
        public string Name { get; set; }

    }
    
    public class EventTypeViewModelWithEvents : EventTypeBaseViewModel
    {
        // public virtual ICollection<EventBaseModel> Events { get; set; }
    }
}