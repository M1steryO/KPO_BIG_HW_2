using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace API_Gateway.Controllers;

[ApiController]
[Route("api")]
public class ApiController : ControllerBase
{
    [HttpGet("file/{id}")]
    public void Get(long id)
    {
    }

    public class FileUploadRequest
    {
        [Required] [FromForm(Name = "file")] public IFormFile File { get; set; }
    }

    [HttpPost("file/upload")]
    [Consumes("multipart/form-data")]
    public void Upload([FromForm] FileUploadRequest request)
    {
    }

    [HttpGet("analyze/{fileId}")]
    [Consumes("multipart/form-data")]
    public void GetAnalytics(long fileId)
    {
    }

    [HttpGet("analyze/word-cloud/{fileId}")]
    [Consumes("multipart/form-data")]
    public void GetAnalyticsImg(long fileId)
    {
    }
}