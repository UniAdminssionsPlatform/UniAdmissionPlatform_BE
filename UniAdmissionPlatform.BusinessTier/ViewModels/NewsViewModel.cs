using System;
using System.Collections.Generic;
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
        [Skip]
        public int? TagSearchId { get; set; }
        [Skip]
        public string Tags { get; set; }
        [Skip]
        public List<TagBaseViewModel> TagList { get; set; }
    }

    public class NewsWithPublishViewModel : NewsBaseViewModel
    {
        public bool? IsPublish { get; set; }
    }

    public class NewsWithUniversityViewModel : NewsBaseViewModel
    {
        public UniversityBaseViewModel University { get; set; }
    }
}