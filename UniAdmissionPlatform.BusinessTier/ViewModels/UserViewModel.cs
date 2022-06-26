using System;
using UniAdmissionPlatform.BusinessTier.Commons.Attributes;

namespace UniAdmissionPlatform.BusinessTier.ViewModels
{
    public class UserBaseViewModel
    {
        public int? Id { get; set; }
        [String]
        public string Uid { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
    
    public class UserAccountBaseViewModel
    {
        public int? Status { get; set; }
        public ManagerAccountBaseViewModel Account { get; set; }
    }
}