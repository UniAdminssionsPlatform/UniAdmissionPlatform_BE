using System;
using System.Collections.Generic;

#nullable disable

namespace UniAdmissionPlatform.DataTier.Models
{
    public partial class Participation
    {
        public int Id { get; set; }
        public int Status { get; set; }
        public int EventId { get; set; }
        public int StudentId { get; set; }

        public virtual Event Event { get; set; }
        public virtual Student Student { get; set; }
    }
}
