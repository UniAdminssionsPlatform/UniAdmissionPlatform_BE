using System;

namespace UniAdmissionPlatform.BusinessTier.Requests.User
{
    public class RegisterRequest
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string ProfileImageUrl { get; set; }
        public string Religion { get; set; }
        public string IdCard { get; set; }
        public string PlaceOfBirth { get; set; }
        public string Nationality { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int GenderId { get; set; }
        public int WardId { get; set; }
        public string EmailContact { get; set; }
    }

    public class RegisterForStudentRequest : RegisterRequest
    {
        public string HighSchoolCode { get; set; }
    }
    
    public class RegisterForSchoolManagerRequest : RegisterRequest
    {
        public string HighSchoolManagerCode { get; set; }
    }
    
    public class RegisterForUniversityManagerRequest : RegisterRequest
    {
        public string UniversityCode { get; set; }
    }
}