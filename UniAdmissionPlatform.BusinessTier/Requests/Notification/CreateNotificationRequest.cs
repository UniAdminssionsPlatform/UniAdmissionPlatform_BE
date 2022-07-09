namespace UniAdmissionPlatform.BusinessTier.Requests.Notification
{
    public class CreateNotificationRequest
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public int? AccountId { get; set; }
    }
}