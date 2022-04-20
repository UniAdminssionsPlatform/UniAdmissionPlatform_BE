using System;
using System.Runtime.CompilerServices;
using UniAdmissionPlatform.BusinessTier.Commons.Attributes;
namespace UniAdmissionPlatform.BusinessTier.ViewModels
{
    public class AccountBaseViewModel
    {
        public int? Id { get; set; }
        [String]
        public string FirstName { get; set; }
        [String]
        public string MiddleName { get; set; }
        [String]
        public string LastName { get; set; }
        [String]
        public string Address { get; set; }
        [String]
        public string PhoneNumber { get; set; }
        [String]
        public string ProfileImageUrl { get; set; }
        [String]
        public string Religion { get; set; }
        [String]
        public string IdCard { get; set; }
        [String]
        public string PlaceOfBirth { get; set; }
        [String]
        public string Nationality { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? GenderId { get; set; }
        [String]
        public string RoleId { get; set; }
        public int? HighSchoolId { get; set; }
        public int? UniversityId { get; set; }
        public int? OrganizationId { get; set; }
        
    }

    public class AccountViewModelWithHighSchool : AccountBaseViewModel
    {
        public HighSchoolBaseViewModel HighSchoolBaseViewModel { get; set; }
    }
    
    public class AccountViewModelWithAccounts : AccountBaseViewModel
    {
    }
}