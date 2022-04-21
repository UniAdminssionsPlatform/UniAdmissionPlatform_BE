﻿using System;

namespace UniAdmissionPlatform.BusinessTier.Requests.Event
{
    public class UpdateEventRequest
    {
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string ThumbnailUrl { get; set; }
        public string FileUrl { get; set; }
        public int? Status { get; set; }
        public string HostName { get; set; }
        public string TargetStudent { get; set; }
        public int? EventTypeId { get; set; }
        public string Address { get; set; }
        public int? ProvinceId { get; set; }
        public string MeetingUrl { get; set; }
    }
}