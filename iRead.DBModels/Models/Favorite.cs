using System;
using System.Collections.Generic;

namespace iRead.DBModels.Models
{
    public partial class Favorite
    {
        public int UserId { get; set; }
        public int BookId { get; set; }
        public DateTime DateAdded { get; set; }
        public int BookRead { get; set; }

        public virtual Book Book { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
