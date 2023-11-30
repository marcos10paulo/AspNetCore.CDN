using AspNetCore.CDN.Models;
using System.Threading.Tasks;

namespace AspNetCore.CDN.Services
{
    public interface IFileServices
    {
        Task<bool> SaveFile(string basePath, FileUploadRequest file);
    }
}
