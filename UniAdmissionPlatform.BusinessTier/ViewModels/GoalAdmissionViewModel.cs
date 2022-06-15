using System;
using UniAdmissionPlatform.BusinessTier.Commons.Attributes;
namespace UniAdmissionPlatform.BusinessTier.ViewModels
{
    public class GoalAdmissionBaseViewModel
    {
        public int? Id { get; set; }
        [String]
        public string Name { get; set; }
        public int? SchoolYearId { get; set; }
        public int? GoalAdmissionTypeId { get; set; }
        [String]
        public string TargetStudent { get; set; }
        
    }
}