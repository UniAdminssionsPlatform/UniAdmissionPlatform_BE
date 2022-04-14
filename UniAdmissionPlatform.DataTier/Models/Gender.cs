using System;
using System.Collections.Generic;

#nullable disable

namespace UniAdmissionPlatform.DataTier.Models
{
    public partial class Gender
    {
        public Gender()
        {
            Accounts = new HashSet<Account>();
            Students = new HashSet<Student>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<Student> Students { get; set; }
    }
}
