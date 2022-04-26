﻿using System;
using System.Collections.Generic;

#nullable disable

namespace UniAdmissionPlatform.DataTier.Models
{
    public partial class University
    {
        public University()
        {
            Accounts = new HashSet<Account>();
            MajorDepartments = new HashSet<MajorDepartment>();
            UniversityEvents = new HashSet<UniversityEvent>();
            UniversityNews = new HashSet<UniversityNews>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Description { get; set; }
        public int? ProvinceId { get; set; }
        public string UniversityCode { get; set; }
        public string WebsiteUrl { get; set; }
        public string ThumbnailUrl { get; set; }
        public string ShortDescription { get; set; }
        public string ProfileImageUrl { get; set; }
        public string Email { get; set; }
        public int Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual Province Province { get; set; }
        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<MajorDepartment> MajorDepartments { get; set; }
        public virtual ICollection<UniversityEvent> UniversityEvents { get; set; }
        public virtual ICollection<UniversityNews> UniversityNews { get; set; }
    }
}
