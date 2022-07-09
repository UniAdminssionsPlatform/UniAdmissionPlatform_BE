using System;

namespace UniAdmissionPlatform.BusinessTier.ViewModels
{
    public class NotificationViewModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public int? AccountId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}