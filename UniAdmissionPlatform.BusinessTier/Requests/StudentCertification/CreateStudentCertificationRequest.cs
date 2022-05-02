namespace UniAdmissionPlatform.BusinessTier.Requests.StudentCertification
{
    public class CreateStudentCertificationRequest
    {
        public int? CertificationId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public float? Score { get; set; }
    }
}