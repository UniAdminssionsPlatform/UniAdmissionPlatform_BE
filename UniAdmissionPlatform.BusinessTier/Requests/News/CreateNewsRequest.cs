using System;
using System.ComponentModel.DataAnnotations;

namespace UniAdmissionPlatform.BusinessTier.Requests.News
{
    public class CreateNewsRequest
    {
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public string ThumbnailUrl { get; set; }
    }
}