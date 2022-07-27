using System;
using System.Collections.Generic;
using UniAdmissionPlatform.BusinessTier.Commons.Attributes;

namespace UniAdmissionPlatform.BusinessTier.ViewModels
{
    public class EventBaseViewModel
    {
        public int? Id { get; set; }
        [String]
        public string Name { get; set; }
        [String]
        public string ShortDescription { get; set; }
        [String]
        public string Description { get; set; }
        [String]
        public string ThumbnailUrl { get; set; }
        [String]
        public string FileUrl { get; set; }
        public int? Status { get; set; }
        [String]
        public string HostName { get; set; }
        public int? EventTypeId { get; set; }
        [String]
        public string Address { get; set; }
        public int? ProvinceId { get; set; }
        [String]
        public string MeetingUrl { get; set; }
        public int? DistrictId { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int? UniversityId { get; set; }
        public string UniversityName { get; set; }
    }

    public class EventWithIsApproveModel : EventBaseViewModel
    {
        public bool? IsApprove { get; set; }
    }

    public class EventWithSlotModel : EventBaseViewModel
    {
        public List<SlotWithEventCheckStatusModel> Slots { get; set; }
    }

    public class EventWithStatusInSlotViewModel 
    {
        public EventBaseViewModel Event { get; set; }
        public int? StatusInSlot { get; set; }
    }
    
    public class EventBySlotBaseViewModel
    {
        public EventBaseViewModel Event { get; set;}
        public int? SlotId { get; set; }
        
    }
    
    public class EventByUniIdBaseViewModel
    {
        public EventBaseViewModel Event { get; set;}
        public int? UniversityId { get; set; }

        public EventByUniIdBaseViewModel(EventBaseViewModel @event, int? universityId)
        {
            Event = @event;
            UniversityId = universityId;
        }
    }
    
    public class ListEventByUniIdBaseViewModel
    {
        public EventWithIsApproveModel Event { get; set;}
        public int? UniversityId { get; set; }

        public ListEventByUniIdBaseViewModel(EventWithIsApproveModel @event, int? universityId)
        {
            Event = @event;
            UniversityId = universityId;
        }
    }
    
    public class EventWithSlotViewModel
    {
        public EventBaseViewModel Event { get; set; }
        public SlotViewModel Slot { get; set; }
    }

    public class EventViewModelWithEvents : EventBaseViewModel
    {
        // public virtual ICollection<EventBaseModel> Events { get; set; }
    }
}