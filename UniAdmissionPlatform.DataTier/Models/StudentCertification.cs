using System;
using System.Collections.Generic;

#nullable disable

namespace UniAdmissionPlatform.DataTier.Models
{
    public partial class StudentCertification
    {
        public int StudentId { get; set; }
        public int CertificationId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public float Score { get; set; }

        public virtual Certification Certification { get; set; }
        public virtual Student Student { get; set; }
    }
}
