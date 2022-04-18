using System;

namespace UniAdmissionPlatform.BusinessTier.ViewModels
{
    public class EventBaseViewModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string ThumbnailUrl { get; set; }
        public string FileUrl { get; set; }
        public int Status { get; set; }
        public string HostName { get; set; }
        public string TargetStudent { get; set; }
        
    }
    
    public class EventViewModelWithEvents : EventBaseViewModel
    {
        // public virtual ICollection<EventBaseModel> Events { get; set; }
    }
}