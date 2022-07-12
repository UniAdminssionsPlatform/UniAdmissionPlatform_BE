using System;

namespace UniAdmissionPlatform.BusinessTier.Responses.Email
{
    public class RejectedEventToUniversityAdminModel
    {
        public string NameOfUniAdmin { get; set; }
        public string NameOfHighSchool { get; set; }
        public string EventName { get; set; }
        public string RejectReason { get; set; }

        public DateTime RejectedTime { get; set; }
        public DateTime? SlotStartDate { get; set; }
        public DateTime? SlotEndDate { get; set; }
        public string BookingDetailUrl { get; set; }
    }
}