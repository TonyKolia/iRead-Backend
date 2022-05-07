using iRead.API.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace iRead.API.Controllers
{
    public class CustomControllerBase : ControllerBase
    {
        protected readonly ILogger _logger;

        public CustomControllerBase(ILogger<CustomControllerBase> logger)
        {
            _logger = logger;
        }

        protected ActionResult ReturnIfNotEmpty<T> (T data, string notFoundMessage = "")
        {
            return data != null ? Ok(data.MapResponse()) : NotFound(notFoundMessage);
        }
    }
}
