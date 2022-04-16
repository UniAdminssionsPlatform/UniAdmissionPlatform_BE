using System;
using System.Collections.Generic;

#nullable disable

namespace UniAdmissionPlatform.DataTier.Models
{
    public partial class UniversityNews
    {
        public int UniversityId { get; set; }
        public int NewsId { get; set; }

        public virtual News News { get; set; }
        public virtual University University { get; set; }
    }
}
