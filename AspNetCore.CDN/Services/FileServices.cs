using AspNetCore.CDN.Models;
using System.IO;
using System.Threading.Tasks;

namespace AspNetCore.CDN.Services
{
    public class FileServices : IFileServices
    {
        public async Task<bool> SaveFile(string basePath, FileUploadRequest file)
        {
            try
            {
                if (!Directory.Exists(basePath))
                    Directory.CreateDirectory(basePath);

                string fullpath = $"{basePath}/{file.FileName}";

                if (file.File.Length > 0)
                {
                    using Stream fileStream = new FileStream(fullpath, FileMode.Create);
                    await file.File.CopyToAsync(fileStream);
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

    }
}
