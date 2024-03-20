using Microsoft.AspNetCore.Hosting;
using Minimal_API_With_File_Upload.FileuploadService;

namespace Minimal_API_With_File_Upload.Fileupload
{
    public class FileUpladService : IFileUpladService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public FileUpladService(IWebHostEnvironment webHostEnvironment)
        {
            this._webHostEnvironment = webHostEnvironment;
        }
        public async  Task<string> FileUpload(IFormFile file, string Location, HttpContext context)
        {
            string WebPhotoPath = "";
            if (file != null && file.Length > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                string uploadFolder = Path.Combine(this._webHostEnvironment.WebRootPath, @"Uploads\");
                string uploadPath = Path.Combine(uploadFolder + DateTime.Now.ToString("MMyyyy") + @"\");

                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                fileName = Location + "." + Path.GetExtension(fileName);

                var filePath = Path.Combine(
                    uploadPath,
                    fileName
                );

                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                WebPhotoPath = "https://" + context.Request.Host.Value + @"\Uploads\" + DateTime.Now.ToString("MMyyyy") + @"\" + fileName;
            }
            return WebPhotoPath;
        }
    }
}
