﻿using System;
using System.Collections.Generic;

#nullable disable

namespace UniAdmissionPlatform.DataTier.Models
{
    public partial class Account
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string ProfileImageUrl { get; set; }
        public string Religion { get; set; }
        public string IdCard { get; set; }
        public string PlaceOfBirth { get; set; }
        public string Nationality { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int GenderId { get; set; }
        public string RoleId { get; set; }

        public virtual Gender Gender { get; set; }
        public virtual User IdNavigation { get; set; }
        public virtual Role Role { get; set; }
    }
}
