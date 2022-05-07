using System;
using System.Collections.Generic;

namespace iRead.DBModels.Models
{
    public partial class Rating
    {
        public int UserId { get; set; }
        public int BookId { get; set; }
        public int Rating1 { get; set; }
        public string? Comment { get; set; }

        public virtual Book Book { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
