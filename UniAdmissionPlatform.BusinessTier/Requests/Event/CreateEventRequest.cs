using System;
using System.ComponentModel.DataAnnotations;

namespace UniAdmissionPlatform.BusinessTier.Requests.Event
{
    public class CreateEventRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string ShortDescription { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string ThumbnailUrl { get; set; }
        public string FileUrl { get; set; }
        [Required]
        public string HostName { get; set; }
        public int EventTypeId { get; set; }
        public string Address { get; set; }
        public int? DistrictId { get; set; }
        public string MeetingUrl { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}