namespace UniAdmissionPlatform.BusinessTier.Requests.UniversityProgram
{
    public class CreateUniversityProgramRequest
    {
        public string Name { get; set; }
        public int? MajorDepartmentId { get; set; }
        public string Description { get; set; }
    }
}