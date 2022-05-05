using System;
using System.Collections.Generic;

namespace iRead.DBModels.Models
{
    public partial class Author
    {
        public Author()
        {
            Books = new HashSet<Book>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public DateTime Birthdate { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}
