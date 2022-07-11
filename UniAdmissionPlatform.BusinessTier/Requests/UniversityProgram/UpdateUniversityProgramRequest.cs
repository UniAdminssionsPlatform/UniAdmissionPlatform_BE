namespace UniAdmissionPlatform.BusinessTier.Requests.UniversityProgram
{
    public class UpdateUniversityProgramRequest
    {
        public string Name { get; set; }
        public int? MajorDepartmentId { get; set; }
        public string Description { get; set; }
        public int SchoolYearId { get; set; }
        public int? SubjectGroupId { get; set; }
        public int? Quantity { get; set; }
    }
}