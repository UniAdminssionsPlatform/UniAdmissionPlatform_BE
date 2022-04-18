using System;

namespace UniAdmissionPlatform.BusinessTier.ViewModels
{
    public class SlotFilterForSchoolAdmin
    {
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int? Status { get; set; }
    }
    public class SlotViewModel
    {
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int? HighSchoolId { get; set; }
        public int? Status { get; set; }
    }
}