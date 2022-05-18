using System;

namespace UniAdmissionPlatform.BusinessTier.Requests.Account
{
    public class CreateAccountRequest
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
        public int? GenderId { get; set; }
        public string RoleId { get; set; }
        public int? HighSchoolId { get; set; }
        public int? UniversityId { get; set; }
        public int? OrganizationId { get; set; }
        public int WardId { get; set; }
        public string EmailContact { get; set; }
    }

    public class Test : CreateAccountRequest
    {
        
    }
}