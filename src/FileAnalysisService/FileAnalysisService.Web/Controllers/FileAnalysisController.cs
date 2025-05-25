using FileAnalysisService.UseCases.Interfaces;
using FileAnalysisService.UseCases.DTOs;
using FileAnalysisService.Web.Common.Responses;
using Microsoft.AspNetCore.Mvc;

namespace FileAnalysisService.Web.Controllers;

[ApiController]
[Route("api/analyze")]
public class FileAnalysisController : ControllerBase
{
    private readonly IFileAnalysisService _service;
    private readonly IWordCloudAnalysisService _wordCloudAnalysisService;


    public FileAnalysisController(IFileAnalysisService service, IWordCloudAnalysisService wordCloudAnalysisService)

    {
        _service = service;
        _wordCloudAnalysisService = wordCloudAnalysisService;
    }

    [HttpGet("{fileId:long}")]
    public async Task<ActionResult<AnalysisResultDto>> Analyze(long fileId)
    {
        try
        {
            var result = await _service.AnalyzeAsync(fileId);
            return Ok(result);
        }
        catch (FileNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception err)
        {
            return StatusCode(500, ApiMessageResponse.Error("Something went wrong!"));
        }
    }

    [HttpGet("word-cloud/{fileId:long}")]
    public async Task<IActionResult> GetWordCloud(long fileId)
    {
        try
        {
            var imageDto = await _wordCloudAnalysisService.GenerateImgByFileIdAsync(fileId);
            return File(imageDto.Content, "image/png", imageDto.FileName);
        }
        catch (FileNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (HttpRequestException e)
        {
            return StatusCode(502, $"Внешний сервис недоступен: {e.Message}");
        }
        catch (Exception e)
        {
            return StatusCode(500, ApiMessageResponse.Error("Something went wrong!"));
        }
    }
}