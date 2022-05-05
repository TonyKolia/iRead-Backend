using System;
using System.Collections.Generic;

namespace iRead.DBModels.Models
{
    public partial class BooksStock
    {
        public int BookId { get; set; }
        public int Stock { get; set; }

        public virtual Book Book { get; set; } = null!;
    }
}
