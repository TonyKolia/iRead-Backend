using System;
using System.Collections.Generic;

namespace iRead.DBModels.Models
{
    public partial class Book
    {
        public Book()
        {
            Authors = new HashSet<Author>();
            Categories = new HashSet<Category>();
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Isbn { get; set; } = null!;
        public int PageCount { get; set; }
        public string Description { get; set; } = null!;
        public string? ImagePath { get; set; }

        public virtual BooksStock BooksStock { get; set; } = null!;

        public virtual ICollection<Author> Authors { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
