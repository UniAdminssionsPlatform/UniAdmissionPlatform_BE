using System;
using System.Collections.Generic;

#nullable disable

namespace UniAdmissionPlatform.DataTier.Models
{
    public partial class Certification
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ThumbnailUrl { get; set; }

        public virtual StudentCertification StudentCertification { get; set; }
    }
}
