using System;
using System.Collections.Generic;

namespace UniAdmissionPlatform.BusinessTier.Responses.Email
{
    public class ApprovedEventToUniAdminModel
    {
        public string NameOfUniAdmin { get; set; }
        public string NameOfHighSchool { get; set; }
        public DateTime ApprovedTime { get; set; }
        public DateTime? SlotStartDate { get; set; }
        public DateTime? SlotEndDate { get; set; }
        public string BookingDetailUrl { get; set; }
    }
}