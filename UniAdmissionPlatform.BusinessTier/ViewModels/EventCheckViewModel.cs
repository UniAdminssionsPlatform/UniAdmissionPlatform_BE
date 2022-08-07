using System;
using System.Security.Cryptography.X509Certificates;
using UniAdmissionPlatform.BusinessTier.Commons.Attributes;

namespace UniAdmissionPlatform.BusinessTier.ViewModels
{
    public class EventCheckBaseViewModel
    {
        public int? Id { get; set; }
        public int? EventId { get; set; }
        public int? SlotId { get; set; }
        public int? Status { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class EventCheckWithEventAndSlotModel : EventCheckBaseViewModel
    {
        public SlotViewModel Slot { get; set; }
        public EventBaseViewModel Event { get; set; }
        [Skip]
        public string Reason { get; set; }
    }
}