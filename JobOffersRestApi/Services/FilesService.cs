using JobOffersRestApi.Exceptions;
using Microsoft.AspNetCore.StaticFiles;

namespace JobOffersRestApi.Services;

public interface IFilesService
{
    Models.FileInfo Get(string fileName);
    void Upload(IFormFile? file);
}

public class FilesService : IFilesService
{
    public Models.FileInfo Get(string fileName)
    {
        var filePath = GetFilePath(fileName);

        if (!System.IO.File.Exists(filePath))
            throw new NotFoundException("File not found");

        var contentProvider = new FileExtensionContentTypeProvider();
        contentProvider.TryGetContentType(fileName, out var contentType);

        var fileContent = File.ReadAllBytes(filePath);
        var fileInfo = new Models.FileInfo()
        {
            FileContent = fileContent,
            ContentType = contentType
        };
        return fileInfo;
    }
    
    public void Upload(IFormFile? file)
    {
        if (file is null || file.Length <= 0)
            throw new BadHttpRequestException("File not uploaded");

        var filePath = GetFilePath(file.FileName);
        using var stream = new FileStream(filePath, FileMode.Create);
        file.CopyTo(stream);
    }

    private string GetFilePath(string fileName)
    {
        var rootPath = Directory.GetCurrentDirectory();
        return $"{rootPath}/wwwroot/PrivateFiles/{fileName}";
    }
}