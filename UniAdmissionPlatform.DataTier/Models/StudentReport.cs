using Microsoft.EntityFrameworkCore;

namespace UniAdmissionPlatform.DataTier.Models
{
    [Keyless]
    public class StudentReport
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailContact { get; set; }
        public string DateOfBirth { get; set; }
        public int HighSchoolId { get; set; }
        public string HighSchoolName { get; set; }
        public string WardName { get; set; }
        public string DistrictName { get; set; }
        public string ProvinceName { get; set; }
    }
}