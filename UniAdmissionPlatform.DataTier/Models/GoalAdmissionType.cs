﻿using System;
using System.Collections.Generic;

#nullable disable

namespace UniAdmissionPlatform.DataTier.Models
{
    public partial class GoalAdmissionType
    {
        public GoalAdmissionType()
        {
            GoalAdmissions = new HashSet<GoalAdmission>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<GoalAdmission> GoalAdmissions { get; set; }
    }
}
