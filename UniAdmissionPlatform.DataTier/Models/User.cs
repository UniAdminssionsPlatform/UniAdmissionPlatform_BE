using System;
using System.Collections.Generic;

#nullable disable

namespace UniAdmissionPlatform.DataTier.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public int Uid { get; set; }
        public int Status { get; set; }

        public virtual Account Account { get; set; }
    }
}
