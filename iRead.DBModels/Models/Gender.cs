using System;
using System.Collections.Generic;

namespace iRead.DBModels.Models
{
    public partial class Gender
    {
        public Gender()
        {
            MemberPersonalInfos = new HashSet<MemberPersonalInfo>();
        }

        public int Id { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<MemberPersonalInfo> MemberPersonalInfos { get; set; }
    }
}
