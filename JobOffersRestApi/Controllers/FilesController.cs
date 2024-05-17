using JobOffersRestApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace JobOffersRestApi.Controllers;

[Route("files")]
[Authorize]
public class FilesController : ControllerBase
{
    private readonly IFilesService _filesService;

    public FilesController(IFilesService filesService)
    {
        _filesService = filesService;
    }

    [HttpGet]
    public ActionResult GetFile([FromQuery] string fileName)
    {
        var fileInfo = _filesService.Get(fileName);
        return File(fileInfo.FileContent, fileInfo.ContentType, fileName);
    }

    [HttpPost]
    public ActionResult UploadFile([FromForm] IFormFile? file)
    {
        _filesService.Upload(file);
        return Ok();
    }
}