using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace iRead.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public ActionResult Test()
        {
            var file = System.IO.File.ReadAllBytes("C:\\Users\\tonyk\\Desktop\\test.png");
            return File(file, "image/png");
        }
    }
}
