using Ferpuser.BLL.Interfaces;
using Ferpuser.Models.Consts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Ferpuser.BLL.Services
{
    public class LocalFileUploader : IFileUploader
    {
        
        IWebHostEnvironment _HostingEnvironment { get; set; }

        public LocalFileUploader(IWebHostEnvironment HostingEnvironment)
        {
            _HostingEnvironment = HostingEnvironment;
        }

        public async Task<string> UploadFile(IFormFile Imagen)
        {
            string result = string.Empty;
            if (Imagen != null && !string.IsNullOrWhiteSpace(Imagen.FileName) && Imagen.Length > 0)
            {
                string pathUploads = Path.Combine(_HostingEnvironment.WebRootPath, Consts.LOCAL_RELATIVE_PATH_UPLOADS);
                string fileName = string.Format("{0}{1}", DateTime.Now.ToString("yyyyMMddHHmmss"), Imagen.FileName);
                if (!Directory.Exists(pathUploads))
                    Directory.CreateDirectory(pathUploads);

                string filePath = Path.Combine(pathUploads, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await Imagen.CopyToAsync(fileStream);
                }
                result = string.Format("~/{0}/{1}", Consts.LOCAL_RELATIVE_PATH_UPLOADS, fileName);
            }
            return result;
        }
    }   
}
