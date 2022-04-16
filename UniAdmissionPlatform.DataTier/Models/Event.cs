using System;
using System.Collections.Generic;

#nullable disable

namespace UniAdmissionPlatform.DataTier.Models
{
    public partial class Event
    {
        public Event()
        {
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
        public int? SlotId { get; set; }
        public int Status { get; set; }
        public string HostName { get; set; }
        public string TargetStudent { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual Slot Slot { get; set; }
        public virtual ICollection<HighSchoolEvent> HighSchoolEvents { get; set; }
        public virtual ICollection<OrganizationEvent> OrganizationEvents { get; set; }
        public virtual ICollection<Participation> Participations { get; set; }
        public virtual ICollection<Speaker> Speakers { get; set; }
        public virtual ICollection<UniversityEvent> UniversityEvents { get; set; }
    }
}
