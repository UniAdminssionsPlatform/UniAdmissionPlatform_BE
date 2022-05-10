using System;
using System.Collections.Generic;

namespace UniAdmissionPlatform.BusinessTier.Responses.Email
{
    public class MailNewBookingRequestToSchoolAdminModel
    {
        public string NameOfSchoolAdmin { get; set; }
        public IList<string> NameOfUniversities { get; set; }
        public DateTime BookingTime { get; set; }
        public DateTime? SlotStartDate { get; set; }
        public DateTime? SlotEndDate { get; set; }
        public string BookingDetailUrl { get; set; }
    }
}