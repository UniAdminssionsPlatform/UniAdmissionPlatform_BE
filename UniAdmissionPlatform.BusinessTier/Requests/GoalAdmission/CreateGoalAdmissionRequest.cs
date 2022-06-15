namespace UniAdmissionPlatform.BusinessTier.Requests.GoalAdmission
{
    public class CreateGoalAdmissionRequest
    {
        public string Name { get; set; }
        public int? SchoolYearId { get; set; }
        public int? GoalAdmissionTypeId { get; set; }
        public string TargetStudent { get; set; }
    }
}