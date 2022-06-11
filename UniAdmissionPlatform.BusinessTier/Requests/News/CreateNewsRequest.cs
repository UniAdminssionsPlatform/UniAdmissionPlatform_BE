using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using UniAdmissionPlatform.BusinessTier.Commons.Attributes;

namespace UniAdmissionPlatform.BusinessTier.Requests.News
{
    public class CreateNewsRequest
    {
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string ThumbnailUrl { get; set; }
        public bool? IsPublish { get; set; }
        public List<int> TagIds { get; set; }
    }
}