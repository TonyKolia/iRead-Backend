using System;
using System.Collections.Generic;

namespace iRead.DBModels.Models
{
    public partial class Book
    {
        public Book()
        {
            Favorites = new HashSet<Favorite>();
            Ratings = new HashSet<Rating>();
            Authors = new HashSet<Author>();
            Categories = new HashSet<Category>();
            Orders = new HashSet<Order>();
            Publishers = new HashSet<Publisher>();
        }

        public int Id { get; set; }
        public string? Title { get; set; }
        public string Isbn { get; set; } = null!;
        public int PageCount { get; set; }
        public string? Description { get; set; }
        public string? ImagePath { get; set; }
        public DateTime? PublishDate { get; set; }
        public DateTime DateAdded { get; set; }

        public virtual BooksStock BooksStock { get; set; } = null!;
        public virtual ICollection<Favorite> Favorites { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }

        public virtual ICollection<Author> Authors { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Publisher> Publishers { get; set; }
    }
}
