using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using UniAdmissionPlatform.BusinessTier.Requests.StudentRecordItem;

namespace UniAdmissionPlatform.BusinessTier.Requests.SchoolRecord
{
    public class CreateSchoolRecordRequest
    {
        public string Name { get; set; }
        public int? SchoolYearId { get; set; }
        public float? TotalScore { get; set; }
        public List<CreateStudentRecordItemBase> RecordItems { get; set; }
    }
}