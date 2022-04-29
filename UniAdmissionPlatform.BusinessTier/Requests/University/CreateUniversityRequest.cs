namespace UniAdmissionPlatform.BusinessTier.Requests.University
{
    public class CreateUniversityRequest
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Description { get; set; }
        public int ProvinceId { get; set; }
        public string UniversityCode { get; set; }
        public string ThumbnailUrl { get; set; }
        public string WebsiteUrl { get; set; }
        public string Email { get; set; }
        public string ShortDescription { get; set; }
        public string ProfileImageUrl { get; set; }
        public int DistrictId { get; set; }
    }
}