using System.Collections.Generic;
using UniAdmissionPlatform.BusinessTier.Commons.Attributes;
namespace UniAdmissionPlatform.BusinessTier.ViewModels
{
    public class MajorDepartmentBaseViewModel
    {
        public int? Id { get; set; }
        [String]
        public string Name { get; set; }
        public int? MajorId { get; set; }
        public int? UniversityId { get; set; }
    }

    public class MajorDepartmentWithMajorAndUniversity
    {
        public UniversityBaseViewModel University { get; set; }
        public MajorBaseViewModel Major { get; set; }
    }
}