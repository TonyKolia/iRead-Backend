using System;
using System.Collections.Generic;

namespace iRead.DBModels.Models
{
    public partial class UserNotification
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string NotificationText { get; set; } = null!;
        public DateTime DateCreated { get; set; }
        public int Viewed { get; set; }

        public virtual User? User { get; set; }
    }
}
