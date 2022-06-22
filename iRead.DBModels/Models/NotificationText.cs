using System;
using System.Collections.Generic;

namespace iRead.DBModels.Models
{
    public partial class NotificationText
    {
        public int Id { get; set; }
        public string NotificationText1 { get; set; } = null!;
        public string? Type { get; set; }
    }
}
