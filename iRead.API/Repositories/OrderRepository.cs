using iRead.API.Models;
using iRead.API.Models.Order;
using iRead.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace iRead.API.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly iReadDBContext _db;

        public OrderRepository(iReadDBContext _db)
        {
            this._db = _db;
        }

        public async Task<string> CreateOrder(NewOrder newOrder)
        {
            var order = new Order
            {
                UserId = newOrder.UserId,
                OrderDate = DateTime.Now,
                ReturnDate = DateTime.Now.AddDays(10),//make parametrical 
                Status = 1,
                Books = await _db.Books.Where(x => newOrder.Books.Contains(x.Id)).ToListAsync()
            };

            _db.Entry(order).State = EntityState.Added;
            await _db.SaveChangesAsync();

            return order.Id.ToString();
        }

        public async Task<OrderResponse> GetOrder(int id)
        {
            var order = await _db.Orders.Select(x => new OrderResponse 
            {
                Id = x.Id,
                UserId = x.UserId,
                OrderDate = x.OrderDate,
                ReturnDate = x.ReturnDate,
                Status = x.StatusNavigation.Description ?? "",
                Books = x.Books.Select(b => new BookResponse 
                {
                    Id = b.Id,
                    Title = b.Title,
                    ISBN = b.Isbn,
                    PageCount = b.PageCount,
                    Description = b.Description,
                    ImagePath = b.ImagePath ?? "",
                    Authors = b.Authors.Select(a => new AuthorResponse 
                    {
                        Id = a.Id,
                        Name = a.Name,
                        Surname = a.Surname,
                        Birthdate = a.Birthdate
                    }),
                    Categories = b.Categories.Select(c => new CategoryResponse 
                    {
                        Id = c.Id,
                        Description = c.Description ?? ""
                    }),
                    Ratings = b.Ratings.Select(r => new RatingResponse 
                    {
                        Rating = r.Rating1,
                        Comment = r.Comment ?? ""
                    })
                })
            }).FirstOrDefaultAsync(x => x.Id == id);

            return order;

        }

        public async Task<IEnumerable<OrderResponse>> GetUserOrders(int userId)
        {
            var orders = await _db.Orders.Where(x => x.UserId == userId).Select(x => new OrderResponse
            {
                Id = x.Id,
                UserId = x.UserId,
                OrderDate = x.OrderDate,
                ReturnDate = x.ReturnDate,
                Status = x.StatusNavigation.Description ?? "",
                Books = x.Books.Select(b => new BookResponse
                {
                    Id = b.Id,
                    Title = b.Title,
                    ISBN = b.Isbn,
                    PageCount = b.PageCount,
                    Description = b.Description,
                    ImagePath = b.ImagePath ?? "",
                    Authors = b.Authors.Select(a => new AuthorResponse
                    {
                        Id = a.Id,
                        Name = a.Name,
                        Surname = a.Surname,
                        Birthdate = a.Birthdate
                    }),
                    Categories = b.Categories.Select(c => new CategoryResponse
                    {
                        Id = c.Id,
                        Description = c.Description ?? ""
                    }),
                    Ratings = b.Ratings.Select(r => new RatingResponse
                    {
                        Rating = r.Rating1,
                        Comment = r.Comment ?? ""
                    })
                })
            }).ToListAsync();

            return orders;

        }
    }
}
