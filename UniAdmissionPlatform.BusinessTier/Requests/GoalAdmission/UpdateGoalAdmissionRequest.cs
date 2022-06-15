namespace UniAdmissionPlatform.BusinessTier.Requests.GoalAdmission
{
    public class UpdateGoalAdmissionRequest
    {
        public string Name { get; set; }
        public int? SchoolYearId { get; set; }
        public int? GoalAdmissionTypeId { get; set; }
        public string TargetStudent { get; set; }
    }
}