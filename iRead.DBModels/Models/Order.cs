using System;
using System.Collections.Generic;

namespace iRead.DBModels.Models
{
    public partial class Order
    {
        public Order()
        {
            Books = new HashSet<Book>();
        }

        public int Id { get; set; }
        public int? UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public int? Status { get; set; }

        public virtual OrderStatus? StatusNavigation { get; set; }
        public virtual User? User { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}
