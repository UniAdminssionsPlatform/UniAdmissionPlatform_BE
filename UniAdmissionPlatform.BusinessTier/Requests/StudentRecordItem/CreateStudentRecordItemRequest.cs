using System.ComponentModel.DataAnnotations;

namespace UniAdmissionPlatform.BusinessTier.Requests.StudentRecordItem
{
    public class CreateStudentRecordItemRequest
    {
        public float? Score { get; set; }
        public int? SchoolRecordId { get; set; }
        public int? SubjectId { get; set; }
    }
    
    public class CreateStudentRecordItemBase
    {
        [Range(0, 10)]
        public float? Score { get; set; }
        public int? SubjectId { get; set; }
    }

}