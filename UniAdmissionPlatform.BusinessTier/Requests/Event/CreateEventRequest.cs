namespace UniAdmissionPlatform.BusinessTier.Requests.Event
{
    public class CreateEventRequest
    {
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string ThumbnailUrl { get; set; }
        public string FileUrl { get; set; }
        public int Status { get; set; }
        public string HostName { get; set; }
        public string TargetStudent { get; set; }
        
    }
}