﻿using System;
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
        [String]
        public string TargetStudent { get; set; }
        public int? EventTypeId { get; set; }
        [String]
        public string Address { get; set; }
        public int? ProvinceId { get; set; }
        [String]
        public string MeetingUrl { get; set; }
        
    }
    
    public class EventViewModelWithEvents : EventBaseViewModel
    {
        // public virtual ICollection<EventBaseModel> Events { get; set; }
    }
}