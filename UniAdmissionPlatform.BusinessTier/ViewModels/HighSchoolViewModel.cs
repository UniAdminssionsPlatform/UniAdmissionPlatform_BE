namespace UniAdmissionPlatform.BusinessTier.ViewModels
{
    public class HighSchoolBaseViewModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public int? ProvinceId { get; set; }
        public string HighSchoolCode { get; set; }
    }
    
    public class HighSchoolCodeViewModel
    {
        public int Id { get; set; }
        public string HighSchoolCode { get; set; }
        public string Name { get; set; }
    }
    
    public class GetHighSchoolBaseViewModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public int? ProvinceId { get; set; }
    }
    
}