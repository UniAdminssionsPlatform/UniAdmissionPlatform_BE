using System.Collections.Generic;
using UniAdmissionPlatform.BusinessTier.Commons.Attributes;

namespace UniAdmissionPlatform.BusinessTier.ViewModels
{
    public class SubjectGroupBaseViewModel
    {
        public int? Id { get; set; }
        [String]
        public string Name { get; set; }
    }

    public class SubjectGroupWithSubject
    {
        public int? Id { get; set; }
        [String]
        public string Name { get; set; }

        public IList<SubjectBaseViewModel> Subjects { get; set; }
    }
}