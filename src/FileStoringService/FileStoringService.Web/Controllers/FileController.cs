using System.ComponentModel.DataAnnotations;
using FileStoringService.UseCases.Interfaces;
using FileStoringService.Web.Common.Responses;
using Microsoft.AspNetCore.Mvc;

namespace FileStoringService.Web.Controllers;

[ApiController]
[Route("api/file/")]
public class FileController : ControllerBase
{
    private readonly IFileStorageService _service;

    public FileController(IFileStorageService service)
    {
        _service = service;
    }

    public class FileUploadRequest
    {
        [Required] [FromForm(Name = "file")] public IFormFile File { get; set; }
    }

    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Upload([FromForm] FileUploadRequest request)
    {
        try
        {
            var file = request.File;

            var extension = Path.GetExtension(file.FileName);
            if (!string.Equals(extension, ".txt", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest(ApiMessageResponse.Error("Разрешены только .txt файлы."));
            }

            if (!string.Equals(file.ContentType, "text/plain", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest(ApiMessageResponse.Error("Недопустимый MIME-тип файла."));
            }

            await using var stream = file.OpenReadStream();
            var id = await _service.StoreFileAsync(file.FileName, file.ContentType, stream);
            return CreatedAtAction(nameof(Download), new { id }, new { id });
        }
        catch (Exception)
        {
            return StatusCode(500, ApiMessageResponse.Error("Something went wrong!"));
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Download(long id)
    {
        try
        {
            var (fileName, contentType, content) = await _service.GetFileByIdAsync(id);
            return File(content, contentType, fileName);
        }
        catch (FileNotFoundException err)
        {
            return NotFound(ApiMessageResponse.Ok(err.Message));
        }
        catch (Exception)
        {
            return StatusCode(500, ApiMessageResponse.Error("Something went wrong!"));
        }
    }
}