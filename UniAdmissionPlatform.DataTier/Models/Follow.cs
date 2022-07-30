using System;
using System.Collections.Generic;

#nullable disable

namespace UniAdmissionPlatform.DataTier.Models
{
    public partial class Follow
    {
        public int? Status { get; set; }
        public int UniversityId { get; set; }
        public int StudentId { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual Student Student { get; set; }
        public virtual University University { get; set; }
    }
}
