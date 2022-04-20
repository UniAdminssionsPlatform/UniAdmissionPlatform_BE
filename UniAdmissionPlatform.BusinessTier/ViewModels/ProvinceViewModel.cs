using UniAdmissionPlatform.BusinessTier.Commons.Attributes;

namespace UniAdmissionPlatform.BusinessTier.ViewModels
{
    public class ProvinceBaseViewModel
    {
        public int? Id { get; set; }
        [String]
        public string Name { get; set; }
        public int? RegionId { get; set; }
    }

    public class ProvinceViewModelWithProvinces : ProvinceBaseViewModel
    {
        
    }
}