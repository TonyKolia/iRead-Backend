using System;
using System.Collections.Generic;

namespace iRead.DBModels.Models
{
    public partial class MemberPersonalInfo
    {
        public int UserId { get; set; }
        public string IdNumber { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public DateTime Birthdate { get; set; }
        public int Gender { get; set; }
        public int IdType { get; set; }

        public virtual Gender GenderNavigation { get; set; } = null!;
        public virtual IdentificationMethod IdTypeNavigation { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
