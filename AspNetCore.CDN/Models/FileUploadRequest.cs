using AspNetCore.CDN.Enums;
using Microsoft.AspNetCore.Http;

namespace AspNetCore.CDN.Models
{
    public class FileUploadRequest
    {
        public string FileName {  get; set; }
        public IFormFile File { get; set; }
    }
}
