using System;
using System.Collections.Generic;

#nullable disable

namespace UniAdmissionPlatform.DataTier.Models
{
    public partial class Tag
    {
        public Tag()
        {
            NewsTags = new HashSet<NewsTag>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<NewsTag> NewsTags { get; set; }
    }
}
