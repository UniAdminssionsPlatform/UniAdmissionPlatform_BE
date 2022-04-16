using System;
using System.Collections.Generic;

#nullable disable

namespace UniAdmissionPlatform.DataTier.Models
{
    public partial class Organization
    {
        public Organization()
        {
            Accounts = new HashSet<Account>();
            OrganizationEvents = new HashSet<OrganizationEvent>();
            Speakers = new HashSet<Speaker>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int OrganizationTypeId { get; set; }
        public int TaxId { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string WebsiteUrl { get; set; }
        public int Status { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual OrganizationType OrganizationType { get; set; }
        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<OrganizationEvent> OrganizationEvents { get; set; }
        public virtual ICollection<Speaker> Speakers { get; set; }
    }
}
