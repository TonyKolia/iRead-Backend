using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace iRead.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : CustomControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _env;


        public ImageController(IWebHostEnvironment env, ILogger<CustomControllerBase> logger):base(logger)
        {
            _env = env;
            _config = new ConfigurationBuilder().SetBasePath(_env.ContentRootPath).AddJsonFile("appsettings.json").Build();
        }

        private static string GetFileExtension(string fileName) => fileName.Substring(fileName.LastIndexOf('.'), fileName.Length - fileName.LastIndexOf('.')).Remove(0, 1);

        [HttpGet]
        [Route("Book/{name}")]
        public async Task<ActionResult> GetBookImage(string name)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                    return BadRequest("No image name supplied.");

                if (!name.Contains('.'))
                    return BadRequest("No image type (extension) specified.");

                var bookImagesPathBase = _config.GetValue<string>("ImagePaths:Books");
                var file = await System.IO.File.ReadAllBytesAsync(Path.Combine(bookImagesPathBase, name));
                return File(file, $"image/{GetFileExtension(name)}");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error has occured");
            }
        }

        [HttpGet]
        [Route("User/{name}")]
        public async Task<ActionResult> GetUserImage(string name)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                    return BadRequest("No image name supplied.");

                if (!name.Contains('.'))
                    return BadRequest("No image type (extension) specified.");

                var userImagesPathBase = _config.GetValue<string>("ImagePaths:Users");
                var file = await System.IO.File.ReadAllBytesAsync(Path.Combine(userImagesPathBase, name));
                return File(file, $"image/{GetFileExtension(name)}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error has occured");
            }
        }
    }
}
