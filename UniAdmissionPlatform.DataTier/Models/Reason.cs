using System;
using System.Collections.Generic;

#nullable disable

namespace UniAdmissionPlatform.DataTier.Models
{
    public partial class Reason
    {
        public int Id { get; set; }
        public int? ReferenceId { get; set; }
        public int? Type { get; set; }
        public string Detail { get; set; }
        public int? HighSchoolId { get; set; }
        public int? UniversityId { get; set; }

        public virtual HighSchool HighSchool { get; set; }
        public virtual University University { get; set; }
    }
}
