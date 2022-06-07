using System;

namespace UniAdmissionPlatform.BusinessTier.Requests.HighSchool
{
    public class UpdateHighSchoolProfileRequest
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public int? DistrictId { get; set; }
        public string HighSchoolCode { get; set; }
        public string HighSchoolManagerCode { get; set; }
        public string WebsiteUrl { get; set; }
        public string ThumbnailUrl { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public string ProfileImageUrl { get; set; }
    }
}