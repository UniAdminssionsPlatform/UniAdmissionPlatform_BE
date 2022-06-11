﻿using System;
using System.Collections.Generic;

#nullable disable

namespace UniAdmissionPlatform.DataTier.Models
{
    public partial class HighSchool
    {
        public HighSchool()
        {
            Accounts = new HashSet<Account>();
            HighSchoolEvents = new HashSet<HighSchoolEvent>();
            Reasons = new HashSet<Reason>();
            Slots = new HashSet<Slot>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public int DistrictId { get; set; }
        public string HighSchoolCode { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string HighSchoolManagerCode { get; set; }
        public int Status { get; set; }
        public string Email { get; set; }
        public string WebsiteUrl { get; set; }
        public string ThumbnailUrl { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public string ProfileImageUrl { get; set; }

        public virtual District District { get; set; }
        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<HighSchoolEvent> HighSchoolEvents { get; set; }
        public virtual ICollection<Reason> Reasons { get; set; }
        public virtual ICollection<Slot> Slots { get; set; }
    }
}
