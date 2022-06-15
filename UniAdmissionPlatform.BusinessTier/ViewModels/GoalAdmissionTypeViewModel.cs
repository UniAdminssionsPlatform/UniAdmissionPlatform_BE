using System;
using UniAdmissionPlatform.BusinessTier.Commons.Attributes;
namespace UniAdmissionPlatform.BusinessTier.ViewModels
{
    public class GoalAdmissionTypeBaseViewModel
    {
        public int? Id { get; set; }
        [String]
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}