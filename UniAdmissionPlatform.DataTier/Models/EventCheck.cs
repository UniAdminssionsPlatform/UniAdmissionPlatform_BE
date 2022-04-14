using System;
using System.Collections.Generic;

#nullable disable

namespace UniAdmissionPlatform.DataTier.Models
{
    public partial class EventCheck
    {
        public int Id { get; set; }
        public int SlotId { get; set; }
        public int Status { get; set; }
        public DateTime CreateTime { get; set; }

        public virtual Slot Slot { get; set; }
    }
}
