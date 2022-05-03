using UniAdmissionPlatform.BusinessTier.Commons.Attributes;
namespace UniAdmissionPlatform.BusinessTier.ViewModels
{
    public class SchoolRecordBaseViewModel
    {
        public int? Id { get; set; }
        [String]
        public string Name { get; set; }
        public int? StudentId { get; set; }
        public int? SchoolYearId { get; set; }
        public float? TotalScore { get; set; }
    }
}