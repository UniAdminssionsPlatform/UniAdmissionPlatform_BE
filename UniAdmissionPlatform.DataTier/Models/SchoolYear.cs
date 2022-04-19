﻿using System;
using System.Collections.Generic;

#nullable disable

namespace UniAdmissionPlatform.DataTier.Models
{
    public partial class SchoolYear
    {
        public SchoolYear()
        {
            GoalAdmissions = new HashSet<GoalAdmission>();
            SchoolRecords = new HashSet<SchoolRecord>();
        }

        public int Id { get; set; }
        public int Year { get; set; }

        public virtual ICollection<GoalAdmission> GoalAdmissions { get; set; }
        public virtual ICollection<SchoolRecord> SchoolRecords { get; set; }
    }
}
