using UniAdmissionPlatform.BusinessTier.Commons.Attributes;

namespace UniAdmissionPlatform.BusinessTier.ViewModels
{
    public class NationalityBaseViewModel
    {
        public int? Id { get; set; }
        [String]
        public string Name { get; set; }
    }
}