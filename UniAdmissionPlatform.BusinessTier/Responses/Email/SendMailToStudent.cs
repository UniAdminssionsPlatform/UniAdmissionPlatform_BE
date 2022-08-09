using System;

namespace UniAdmissionPlatform.BusinessTier.Responses.Email
{
    public class SendMailToStudent
    {
        public string NameOfStudent { get; set; }
        public string EventName { get; set; }
        public string NameOfHighSchool { get; set; }
        public DateTime? SlotStartDate { get; set; }
        public DateTime? SlotEndDate { get; set; }
        public string BookingDetailUrl { get; set; }
    }
}