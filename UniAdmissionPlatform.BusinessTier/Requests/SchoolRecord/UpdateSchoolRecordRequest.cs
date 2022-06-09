using System.Collections.Generic;
using UniAdmissionPlatform.BusinessTier.Requests.Common;
using UniAdmissionPlatform.BusinessTier.Requests.StudentRecordItem;

namespace UniAdmissionPlatform.BusinessTier.Requests.SchoolRecord
{
    public class UpdateSchoolRecordRequest
    {
        public string Name { get; set; }
        public int? SchoolYearId { get; set; }
        public float? TotalScore { get; set; }
        public PutRequest<CreateStudentRecordItemBase, UpdateStudentRecordItemBase, int> RecordItems { get; set; }
    }
}