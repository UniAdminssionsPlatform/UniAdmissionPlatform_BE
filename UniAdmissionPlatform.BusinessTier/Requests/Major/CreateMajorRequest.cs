namespace UniAdmissionPlatform.BusinessTier.Requests.Major
{
    public class CreateMajorRequest
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int MajorGroupId { get; set; }
    }
}