using System;
using System.Collections.Generic;

#nullable disable

namespace UniAdmissionPlatform.DataTier.Models
{
    public partial class UniversityEvent
    {
        public int UniversityId { get; set; }
        public int EventId { get; set; }

        public virtual Event Event { get; set; }
        public virtual University University { get; set; }
    }
}
