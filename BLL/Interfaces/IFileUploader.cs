using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Ferpuser.BLL.Interfaces
{
    public interface IFileUploader
    {
        Task<string> UploadFile(IFormFile File);
    }
}
