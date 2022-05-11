using System;
using System.Collections.Generic;

namespace iRead.DBModels.Models
{
    public partial class BooksPublisher
    {
        public int BookId { get; set; }
        public int PublisherId { get; set; }
        public DateTime? PublishDate { get; set; }

        public virtual Book Book { get; set; } = null!;
        public virtual Publisher Publisher { get; set; } = null!;
    }
}
