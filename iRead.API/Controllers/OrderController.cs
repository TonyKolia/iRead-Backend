using iRead.API.Models;
using iRead.API.Models.Order;
using iRead.API.Repositories.Interfaces;
using iRead.API.Utilities.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace iRead.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : CustomControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly IValidationUtilities _validationUtilities;

        public OrderController(IOrderRepository _orderRepository, IFavoriteRepository _favoriteRepository, IValidationUtilities _validationUtilities, ILogger<CustomControllerBase> logger) : base(logger)
        {
            this._orderRepository = _orderRepository;
            this._favoriteRepository = _favoriteRepository;
            this._validationUtilities = _validationUtilities;
        }

        [HttpPost]
        public async Task<ActionResult<string>> Create([FromBody] NewOrder order)
        {
            try
            {
                var validationResult = await _validationUtilities.ValidateOrder(order.Books, order.UserId);
                if (!validationResult.Success)
                    return ReturnResponse(ResponseType.BadRequest, "Errors occured.", validationResult);

                var createdOrderId = await _orderRepository.CreateOrder(order);
                await UpdateFavorites(order.UserId, order.Books);
                return ReturnResponse(ResponseType.Created, "Created successfully", new { OrderId = createdOrderId });
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return ReturnResponse(ResponseType.Error);
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<OrderResponse>> Get(int id)
        {
            var order = await _orderRepository.GetOrder(id);
            return ReturnIfNotEmpty(order, $"Order with id {id} not found.");
        }

        [HttpGet]
        [Route("User/{userId}")]
        public async Task<ActionResult<IEnumerable<OrderResponse>>> GetByUser(int userId)
        {
            var orders = await _orderRepository.GetUserOrders(userId);
            return ReturnIfNotEmpty(orders, $"No orders found for user with id {userId}.", false);
        }

        private async Task UpdateFavorites(int userId, IEnumerable<int> books)
        {
            foreach(var book in books)
            {
                var favoriteToUpdate = await _favoriteRepository.GetFavorite(userId, book);
                if (favoriteToUpdate == null)
                    continue;

                if (favoriteToUpdate.BookRead)
                    continue;

                var updatedFavorite = new Favorite
                {
                    BookId = favoriteToUpdate.Book.Id,
                    UserId = favoriteToUpdate.UserId,
                    DateAdded = favoriteToUpdate.DateAdded,
                    BookRead = 1
                };

                await _favoriteRepository.UpdateFavorite(updatedFavorite);
            }
        }
    }
}
