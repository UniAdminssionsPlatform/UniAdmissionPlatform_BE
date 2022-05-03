using System;

namespace UniAdmissionPlatform.BusinessTier.ViewModels
{
    public class SlotFilterForSchoolAdmin
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Status { get; set; }
    }

    public class SlotFilterForUniAdmin
    {
        public int? HighSchoolId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Status { get; set; }
    }
    public class SlotViewModel
    {
        public int Id { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? HighSchoolId { get; set; }
        public int? Status { get; set; }
    }
}