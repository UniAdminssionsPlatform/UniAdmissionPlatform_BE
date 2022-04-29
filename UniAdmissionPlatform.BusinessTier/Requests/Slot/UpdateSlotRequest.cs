using System;
using System.ComponentModel.DataAnnotations;

namespace UniAdmissionPlatform.BusinessTier.Requests.Slot
{
    public class UpdateSlotRequest
    {
        [Required]
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}