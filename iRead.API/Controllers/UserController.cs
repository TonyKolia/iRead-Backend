using iRead.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace iRead.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : CustomControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository _userRepository, ILogger<CustomControllerBase> logger) : base(logger)
        {
            this._userRepository = _userRepository;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<User>> Get(int id)
        {
            var user = await _userRepository.GetUser(id);
            if (user != null)
            {
                user.Password = "XXXXXXXX";
                return Ok(user);
            }
            else
                return NotFound($"User with id {id} not found.");
        }

        [HttpGet]
        [Route("{username}")]
        public async Task<ActionResult<User>> Get(string username)
        {
            var user = await _userRepository.GetUser(username);
            if (user != null)
            {
                user.Password = "XXXXXXXX";
                return Ok(user);
            }
            else
                return NotFound($"User with username {username} not found.");
        }

    }
}
