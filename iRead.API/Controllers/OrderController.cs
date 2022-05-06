using iRead.API.Models;
using iRead.API.Models.Order;
using iRead.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace iRead.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository _orderRepository)
        {
            this._orderRepository = _orderRepository;
        }

        [HttpPost]
        public async Task<ActionResult<string>> Create([FromBody] NewOrder order)
        {
            try
            {
                //validate order and stock
                return Ok(await _orderRepository.CreateOrder(order));
            }
            catch(Exception ex)
            {
                //log
                return StatusCode(StatusCodes.Status500InternalServerError, "An error has occured.");
            }

        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<OrderResponse>> Get(int id)
        {
            return Ok(await _orderRepository.GetOrder(id));
        }

        [HttpGet]
        [Route("user/{userId}")]
        public async Task<ActionResult<IEnumerable<OrderResponse>>> GetByUser(int userId)
        {
            return Ok(await _orderRepository.GetUserOrders(userId));
        }
    }
}
