using System;
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
            Slots = new HashSet<Slot>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public int ProvinceId { get; set; }
        public string HighSchoolCode { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string HighSchoolManagerCode { get; set; }

        public virtual Province Province { get; set; }
        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<HighSchoolEvent> HighSchoolEvents { get; set; }
        public virtual ICollection<Slot> Slots { get; set; }
    }
}
