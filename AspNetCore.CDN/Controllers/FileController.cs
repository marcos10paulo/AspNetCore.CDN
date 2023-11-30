using AspNetCore.CDN.Enums;
using AspNetCore.CDN.Helpers;
using AspNetCore.CDN.Models;
using AspNetCore.CDN.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.CDN.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileController : Controller
    {
        private readonly IFileServices _fileServices;
        private readonly IMemoryCache _memoryCache;
        private readonly string _basePath;
        private readonly string _returnType;

        public FileController(IFileServices fileServices, IMemoryCache memoryCache)
        {
            _fileServices = fileServices;
            _memoryCache = memoryCache;
            _basePath = "//programacao120/ind_pessoal_foto/";
            _returnType = "file";
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(string filename)
        {
            if (Request.Form.Files == null || Request.Form.Files.Count() == 0)
                return BadRequest("No Files attached!");

            string extension = Path.GetExtension(filename);

            Extensions fileType = extension.GetEnumByDescription<Extensions>();

            filename = Path.GetFileNameWithoutExtension(filename);

            bool moreOneFile = Request.Form.Files.Count() > 1;
            int count = 0;
            bool success = true;

            foreach (var file in Request.Form.Files)
            {
                count++;
                var upload = new FileUploadRequest
                {
                    FileName = moreOneFile ? $"{filename}-{count}{extension}" : $"{filename}{extension}",
                    File = file
                };

                success = await _fileServices.SaveFile(_basePath, upload);

                if (!success)
                    break;
            }

            if (success)
                return Ok();
            else 
                return BadRequest();            
        }

        [HttpGet]
        public async Task<IActionResult> GetSrc(string filename)
        {
            string fullname = Path.Combine(_basePath, filename);
            
            if (!_memoryCache.TryGetValue(_returnType + fullname, out string result))
            {
                if (_returnType == "file")
                    result = $"file://{fullname}";
                else
                    result = Convert.ToBase64String(System.IO.File.ReadAllBytes(fullname));

                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(15));

                _memoryCache.Set(_returnType + fullname, result, cacheEntryOptions);
            }

            if (string.IsNullOrEmpty(result))
                return BadRequest();

            return Ok(result);
        }
    }
}
