﻿using UniAdmissionPlatform.BusinessTier.Commons.Attributes;

namespace UniAdmissionPlatform.BusinessTier.ViewModels
{
    public class HighSchoolBaseViewModel
    {
        public int? Id { get; set; }
        [String]
        public string Name { get; set; }
        [String]
        public string Description { get; set; }
        [String]
        public string Address { get; set; }
        [String]
        public string Email { get; set; }
        [String]
        public string PhoneNumber { get; set; }
        public int? ProvinceId { get; set; }
        [String]
        public string HighSchoolCode { get; set; }
        public int? Status { get; set; }
        [String]
        public string WebsiteUrl { get; set; }
        [String]
        public string ThumbnailUrl { get; set; }
        [String]
        public string ShortDescription { get; set; }
        [String]
        public string ProfileImageUrl { get; set; }
    }
    
    public class HighSchoolCodeViewModel
    {
        public int Id { get; set; }
        public string HighSchoolCode { get; set; }
        public string Name { get; set; }
    }
    
    public class HighSchoolManagerCodeViewModel
    {
        public int Id { get; set; }
        public string HighSchoolManagerCode { get; set; }
        public string Name { get; set; }
    }
    
    public class GetHighSchoolBaseViewModel
    {
        public int? Id { get; set; }
        [String]
        public string Name { get; set; }
        [String]
        public string Description { get; set; }
        [String]
        public string Address { get; set; }
        [String]
        public string Email { get; set; }
        [String]
        public string PhoneNumber { get; set; }
        public int? ProvinceId { get; set; }
        public int? Status { get; set; }
        [String]
        public string WebsiteUrl { get; set; }
        [String]
        public string ThumbnailUrl { get; set; }
        [String]
        public string ShortDescription { get; set; }
        [String]
        public string ProfileImageUrl { get; set; }
        [String]
        public string HighSchoolCode { get; set; }
        [String]
        public string HighSchoolManagerCode { get; set; }
        public int? DistrictId { get; set; }
    }
    
}