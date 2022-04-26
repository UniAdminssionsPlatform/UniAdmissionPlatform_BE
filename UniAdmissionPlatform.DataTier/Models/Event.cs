using System;
using System.Collections.Generic;

#nullable disable

namespace UniAdmissionPlatform.DataTier.Models
{
    public partial class Event
    {
        public Event()
        {
            EventChecks = new HashSet<EventCheck>();
            HighSchoolEvents = new HashSet<HighSchoolEvent>();
            OrganizationEvents = new HashSet<OrganizationEvent>();
            Participations = new HashSet<Participation>();
            Speakers = new HashSet<Speaker>();
            UniversityEvents = new HashSet<UniversityEvent>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string ThumbnailUrl { get; set; }
        public string FileUrl { get; set; }
        public int Status { get; set; }
        public string HostName { get; set; }
        public string TargetStudent { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public int EventTypeId { get; set; }
        public string Address { get; set; }
        public int? ProvinceId { get; set; }
        public string MeetingUrl { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public virtual EventType EventType { get; set; }
        public virtual Province Province { get; set; }
        public virtual ICollection<EventCheck> EventChecks { get; set; }
        public virtual ICollection<HighSchoolEvent> HighSchoolEvents { get; set; }
        public virtual ICollection<OrganizationEvent> OrganizationEvents { get; set; }
        public virtual ICollection<Participation> Participations { get; set; }
        public virtual ICollection<Speaker> Speakers { get; set; }
        public virtual ICollection<UniversityEvent> UniversityEvents { get; set; }
    }
}
