using System;
using System.Collections.Generic;

namespace iRead.DBModels.Models
{
    public partial class EmailText
    {
        public int Id { get; set; }
        public string Type { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public string Body { get; set; } = null!;
    }
}
