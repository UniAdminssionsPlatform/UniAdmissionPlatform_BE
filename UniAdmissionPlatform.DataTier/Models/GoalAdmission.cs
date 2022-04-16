using System;
using System.Collections.Generic;

#nullable disable

namespace UniAdmissionPlatform.DataTier.Models
{
    public partial class GoalAdmission
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SchoolYearId { get; set; }
        public int GoalAdmissionTypeId { get; set; }
        public string TargetStudent { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual GoalAdmissionType GoalAdmissionType { get; set; }
        public virtual SchoolYear SchoolYear { get; set; }
    }
}
