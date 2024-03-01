using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace CityInfo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly FileExtensionContentTypeProvider _fileExtensionContentTypeProvider;

        public FilesController(FileExtensionContentTypeProvider fileExtensionContentTypeProvider)
        {
            _fileExtensionContentTypeProvider = fileExtensionContentTypeProvider;
        }

        [HttpGet("{id}")]
        public ActionResult GetFile(string id)
        {
            var filePath = $"Files/{id}";
            // Return 404 if file not found
            if (!System.IO.File.Exists(filePath)) return NotFound();

            // Return 404 if content type couldn't be determined
            if (!_fileExtensionContentTypeProvider.TryGetContentType(filePath, out var contentType)) return StatusCode(500, "Couldn't determine the file's content type.");

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            // Return the file without a download name in case it is an html
            if (contentType == "text/html") return File(fileBytes, contentType);

            return File(fileBytes, contentType, id);
        }
    }
}
