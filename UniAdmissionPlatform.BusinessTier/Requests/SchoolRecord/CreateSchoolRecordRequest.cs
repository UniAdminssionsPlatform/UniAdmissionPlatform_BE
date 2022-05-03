namespace UniAdmissionPlatform.BusinessTier.Requests.SchoolRecord
{
    public class CreateSchoolRecordRequest
    {
        public string Name { get; set; }
        public int? SchoolYearId { get; set; }
        public float? TotalScore { get; set; }
    }
}