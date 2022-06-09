using System;
using System.Collections.Generic;

namespace iRead.DBModels.Models
{
    public partial class MemberContactInfo
    {
        public int UserId { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string PostalCode { get; set; } = null!;
        public string Telephone { get; set; } = null!;
        public string Email { get; set; } = null!;

        public virtual User User { get; set; } = null!;
    }
}
