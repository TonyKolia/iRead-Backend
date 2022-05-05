﻿using System;
using System.Collections.Generic;

namespace iRead.DBModels.Models
{
    public partial class Category
    {
        public Category()
        {
            Books = new HashSet<Book>();
        }

        public int Id { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}
