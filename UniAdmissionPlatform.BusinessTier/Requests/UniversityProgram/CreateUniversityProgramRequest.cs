using System.Collections.Generic;

namespace UniAdmissionPlatform.BusinessTier.Requests.UniversityProgram
{
    public class CreateUniversityProgramRequest
    {
        public string Name { get; set; }
        public int? MajorDepartmentId { get; set; }
        public string Description { get; set; }
        public int SchoolYearId { get; set; }
        public int? SubjectGroupId { get; set; }
        public int? Quantity { get; set; }
        public double? RecordPoint { get; set; }
    }

    public class BulkCreateUniversityProgramMajorRequest
    {
        public int SchoolYearId { get; set; }
        public List<MajorDepartmentDetail> MajorDepartmentDetails { get; set; }

        public List<CreateUniversityProgramRequest> ToUniversityProgramRequests()
        {
            var result = new List<CreateUniversityProgramRequest>();

            foreach (var majorDepartmentDetail in MajorDepartmentDetails)
            {
                foreach (var subjectGroupDetail in majorDepartmentDetail.SubjectGroupDetails)
                {
                    result.Add(new CreateUniversityProgramRequest
                    {
                        Name = majorDepartmentDetail.Name,
                        Description = majorDepartmentDetail.Description,
                        MajorDepartmentId = majorDepartmentDetail.MajorDepartmentId,
                        SchoolYearId = this.SchoolYearId,
                        SubjectGroupId = subjectGroupDetail.SubjectGroupId,
                        Quantity = subjectGroupDetail.Quantity,
                        RecordPoint = subjectGroupDetail.RecordPoint
                    });
                }
            }

            return result;
        }
    }

    public class MajorDepartmentDetail
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int? MajorDepartmentId { get; set; }
        public List<SubjectGroupDetail> SubjectGroupDetails { get; set; }
    }

    public class SubjectGroupDetail
    {
        public int SubjectGroupId { get; set; }
        public int? Quantity { get; set; }
        public double? RecordPoint { get; set; }
    }
}