using iRead.API.Models;
using iRead.API.Models.Order;
using iRead.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace iRead.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : CustomControllerBase
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository _orderRepository, ILogger<CustomControllerBase> logger) : base(logger)
        {
            this._orderRepository = _orderRepository;
        }

        [HttpPost]
        public async Task<ActionResult<string>> Create([FromBody] NewOrder order)
        {
            try
            {
                //validate order and stock
                var createdOrder = await _orderRepository.CreateOrder(order);
                return ReturnResponse(ResponseType.Created, "Created successfully", createdOrder);
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
        [Route("user/{userId}")]
        public async Task<ActionResult<IEnumerable<OrderResponse>>> GetByUser(int userId)
        {
            var orders = await _orderRepository.GetUserOrders(userId);
            return ReturnIfNotEmpty(orders, $"No orders found for user with id {userId}.", false);
        }
    }
}
