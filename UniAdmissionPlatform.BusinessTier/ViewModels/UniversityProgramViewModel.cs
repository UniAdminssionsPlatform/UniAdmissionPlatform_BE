using UniAdmissionPlatform.BusinessTier.Commons.Attributes;
namespace UniAdmissionPlatform.BusinessTier.ViewModels
{
    public class UniversityProgramBaseViewModel
    {
        public int? Id { get; set; }
        [String]
        public string Name { get; set; }
        public int? MajorDepartmentId { get; set; }
        [String]
        public string Description { get; set; }
        public int? SchoolYearDescription { get; set; }
        public int? SchoolYearId { get; set; }
    }

    public class UniversityProgramWithMajorDepartmentAndSchoolYearModel : UniversityProgramBaseViewModel
    {
        public MajorDepartmentWithMajorAndUniversity MajorDepartment { get; set; }
        public SchoolYearBaseViewModel SchoolYear { get; set; }
    }
}