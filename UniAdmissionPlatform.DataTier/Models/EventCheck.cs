using System;
using System.Collections.Generic;

#nullable disable

namespace UniAdmissionPlatform.DataTier.Models
{
    public partial class EventCheck
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public int SlotId { get; set; }
        public int Status { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual Event Event { get; set; }
        public virtual Slot Slot { get; set; }
    }
}
