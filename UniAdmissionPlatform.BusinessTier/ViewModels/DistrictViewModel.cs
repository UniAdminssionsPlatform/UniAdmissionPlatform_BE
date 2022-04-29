using UniAdmissionPlatform.BusinessTier.Commons.Attributes;

namespace UniAdmissionPlatform.BusinessTier.ViewModels
{
    public class DistrictViewModel
    {
        public int? Id { get; set; }
        [String]
        public string Name { get; set; }
        public int? ProvinceId { get; set; }
    }
}