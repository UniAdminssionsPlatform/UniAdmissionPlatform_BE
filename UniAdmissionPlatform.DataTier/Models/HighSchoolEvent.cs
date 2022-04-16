using System;
using System.Collections.Generic;

#nullable disable

namespace UniAdmissionPlatform.DataTier.Models
{
    public partial class HighSchoolEvent
    {
        public int HighSchoolId { get; set; }
        public int EventId { get; set; }

        public virtual Event Event { get; set; }
        public virtual HighSchool HighSchool { get; set; }
    }
}
