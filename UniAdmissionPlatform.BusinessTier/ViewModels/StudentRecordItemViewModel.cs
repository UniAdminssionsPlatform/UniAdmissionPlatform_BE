namespace UniAdmissionPlatform.BusinessTier.ViewModels
{
    public class StudentRecordItemBaseModel
    {
        public int Id { get; set; }
        public float? Score { get; set; }
        public int SchoolRecordId { get; set; }
        public int SubjectId { get; set; }
    }

    public class StudentRecordItemWithSubjectModel : StudentRecordItemBaseModel
    {
        public SubjectBaseViewModel Subject { get; set; }
    }
}
    public partial class StudentRecordItemBaseViewModel
    {
        public int? Id { get; set; }
        public float? Score { get; set; }
        public int? SchoolRecordId { get; set; }
        public int? SubjectId { get; set; }
    }
}



