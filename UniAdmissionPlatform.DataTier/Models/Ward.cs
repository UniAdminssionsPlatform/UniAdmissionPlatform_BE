using System;
using System.Collections.Generic;

#nullable disable

namespace UniAdmissionPlatform.DataTier.Models
{
    public partial class Ward
    {
        public Ward()
        {
            Accounts = new HashSet<Account>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int DistrictId { get; set; }

        public virtual District District { get; set; }
        public virtual ICollection<Account> Accounts { get; set; }
    }
}
