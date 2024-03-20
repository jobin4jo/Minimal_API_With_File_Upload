namespace Minimal_API_With_File_Upload.FileuploadService
{
    public interface IFileUpladService
    {
        Task<string> FileUpload(IFormFile file,string Location, HttpContext context);
    }
}
