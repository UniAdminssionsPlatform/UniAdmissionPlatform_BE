﻿namespace UniAdmissionPlatform.BusinessTier.Requests.StudentCertificaiton
{
    public class UpdateStudentCertificationRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public float? Score { get; set; }
    }
}