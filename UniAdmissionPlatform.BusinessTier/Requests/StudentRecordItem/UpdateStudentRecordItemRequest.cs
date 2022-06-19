using System.ComponentModel.DataAnnotations;

namespace UniAdmissionPlatform.BusinessTier.Requests.StudentRecordItem
{
    public class UpdateStudentRecordItemRequest
    {
        public float? Score { get; set; }
        public int? SchoolRecordId { get; set; }
        public int? SubjectId { get; set; }
    }
    
    public class UpdateStudentRecordItemBase
    {
        public int Id { get; set; }
        [Range(0, 10)]
        public float? Score { get; set; }
    }
}