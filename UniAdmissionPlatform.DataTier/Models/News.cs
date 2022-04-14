using System;
using System.Collections.Generic;

#nullable disable

namespace UniAdmissionPlatform.DataTier.Models
{
    public partial class News
    {
        public News()
        {
            NewsMajors = new HashSet<NewsMajor>();
            NewsTags = new HashSet<NewsTag>();
            UniversityNews = new HashSet<UniversityNews>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public string ThumbnailUrl { get; set; }
        public DateTime CreateDate { get; set; }

        public virtual ICollection<NewsMajor> NewsMajors { get; set; }
        public virtual ICollection<NewsTag> NewsTags { get; set; }
        public virtual ICollection<UniversityNews> UniversityNews { get; set; }
    }
}
