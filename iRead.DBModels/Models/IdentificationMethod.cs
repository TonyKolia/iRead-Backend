using System;
using System.Collections.Generic;

namespace iRead.DBModels.Models
{
    public partial class IdentificationMethod
    {
        public IdentificationMethod()
        {
            MemberPersonalInfos = new HashSet<MemberPersonalInfo>();
        }

        public int Id { get; set; }
        public string Description { get; set; } = null!;

        public virtual ICollection<MemberPersonalInfo> MemberPersonalInfos { get; set; }
    }
}
