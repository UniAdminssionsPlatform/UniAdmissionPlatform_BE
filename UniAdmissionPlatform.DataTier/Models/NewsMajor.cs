using System;
using System.Collections.Generic;

#nullable disable

namespace UniAdmissionPlatform.DataTier.Models
{
    public partial class NewsMajor
    {
        public int NewsId { get; set; }
        public int MajorId { get; set; }

        public virtual Major Major { get; set; }
        public virtual News News { get; set; }
    }
}
