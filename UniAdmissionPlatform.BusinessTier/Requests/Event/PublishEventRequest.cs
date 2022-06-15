namespace UniAdmissionPlatform.BusinessTier.Requests.Event
{
    public class PublishEventRequest
    {
        public int EventId { get; set; }
        public bool IsPublish { get; set; }
    }
}