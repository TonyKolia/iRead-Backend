using System;
using System.Collections.Generic;

namespace iRead.DBModels.Models
{
    public partial class Publisher
    {
        public Publisher()
        {
            Books = new HashSet<Book>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}
