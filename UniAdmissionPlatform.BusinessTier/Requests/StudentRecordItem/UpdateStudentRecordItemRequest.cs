namespace UniAdmissionPlatform.BusinessTier.Requests.StudentRecordItem
{
    public class UpdateStudentRecordItemRequest
    {
        public float? Score { get; set; }
        public int? SchoolRecordId { get; set; }
        public int? SubjectId { get; set; }
    }
}