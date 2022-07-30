namespace UniAdmissionPlatform.BusinessTier.Responses.User
{
    public class LoginResponse
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string Roles { get; set; }
        public int? UniversityId { get; set; }
        public int? HighSchoolId { get; set; }
        public int? OrganizationId { get; set; }
        public string Token { get; set; }
        public long ExpiresAt { get; set; }
        public long BufferTime { get; set; }
        public bool NeedRegister { get; set; }
        public string EmailContact { get; set; }
    }
}