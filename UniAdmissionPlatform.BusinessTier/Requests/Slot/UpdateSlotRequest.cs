using System;
using System.ComponentModel.DataAnnotations;

namespace UniAdmissionPlatform.BusinessTier.Requests.Slot
{
    public class UpdateSlotRequest
    {
        [Required]
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}