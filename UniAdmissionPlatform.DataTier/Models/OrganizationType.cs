using System;
using System.Collections.Generic;

#nullable disable

namespace UniAdmissionPlatform.DataTier.Models
{
    public partial class OrganizationType
    {
        public OrganizationType()
        {
            Organizations = new HashSet<Organization>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Organization> Organizations { get; set; }
    }
}
