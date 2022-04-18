using System;

namespace UniAdmissionPlatform.BusinessTier.Requests.Event
{
    public class UpdateEventRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string ThumbnailUrl { get; set; }
        public string FileUrl { get; set; }
        public int? Status { get; set; }
        public string HostName { get; set; }
        public string TargetStudent { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}