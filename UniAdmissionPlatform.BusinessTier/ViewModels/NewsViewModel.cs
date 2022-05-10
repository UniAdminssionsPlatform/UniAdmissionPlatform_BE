using System;
using UniAdmissionPlatform.BusinessTier.Commons.Attributes;

namespace UniAdmissionPlatform.BusinessTier.ViewModels
{
    public class NewsBaseViewModel
    {
        public int? Id { get; set; }
        [String]
        public string Title { get; set; }
        [String]
        public string ShortDescription { get; set; }
        [String]
        public string Description { get; set; }
        [String]
        public string ThumbnailUrl { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}