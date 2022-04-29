using UniAdmissionPlatform.BusinessTier.Commons.Attributes;

namespace UniAdmissionPlatform.BusinessTier.ViewModels
{
    public class WardViewModel
    {
        public int? Id { get; set; }
        [String]
        public string Name { get; set; }
        public int? DistrictId { get; set; }
    }
}