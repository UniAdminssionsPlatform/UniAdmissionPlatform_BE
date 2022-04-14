using System;
using System.Collections.Generic;

#nullable disable

namespace UniAdmissionPlatform.DataTier.Models
{
    public partial class HighSchool
    {
        public HighSchool()
        {
            HighSchoolEvents = new HashSet<HighSchoolEvent>();
            Slots = new HashSet<Slot>();
            Students = new HashSet<Student>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public int ProvinceId { get; set; }

        public virtual Province Province { get; set; }
        public virtual ICollection<HighSchoolEvent> HighSchoolEvents { get; set; }
        public virtual ICollection<Slot> Slots { get; set; }
        public virtual ICollection<Student> Students { get; set; }
    }
}
