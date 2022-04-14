using System;
using System.Collections.Generic;

#nullable disable

namespace UniAdmissionPlatform.DataTier.Models
{
    public partial class OrganizationEvent
    {
        public int OrganizationId { get; set; }
        public int EventId { get; set; }

        public virtual Event Event { get; set; }
        public virtual Organization Organization { get; set; }
    }
}
