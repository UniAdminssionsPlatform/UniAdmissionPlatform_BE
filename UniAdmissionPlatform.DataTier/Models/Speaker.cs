using System;
using System.Collections.Generic;

#nullable disable

namespace UniAdmissionPlatform.DataTier.Models
{
    public partial class Speaker
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int EventId { get; set; }
        public int Organization { get; set; }
        public string Description { get; set; }

        public virtual Event Event { get; set; }
        public virtual Organization OrganizationNavigation { get; set; }
    }
}
