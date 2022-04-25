using UniAdmissionPlatform.BusinessTier.Commons.Attributes;
namespace UniAdmissionPlatform.BusinessTier.ViewModels
{
    public class UniversityBaseViewModel
    {
        public int? Id { get; set; }
        [String]
        public string Name { get; set; }
        [String]
        public string Address { get; set; }
        [String]
        public string PhoneNumber { get; set; }
        public int? ProvinceId { get; set; }
        [String]
        public string UniversityCode { get; set; }
        [String]
        public string WebsiteUrl { get; set; }
        [String]
        public string Email { get; set; }
        public int? Status { get; set; }
    }

    public class UniversityCodeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UniversityCode { get; set; }
    }
}