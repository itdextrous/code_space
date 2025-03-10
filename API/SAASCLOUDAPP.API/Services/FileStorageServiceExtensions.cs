using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Workfacta.Shared.Services.Storage;

namespace SAASCLOUDAPP.API.Services
{
    internal static class FileStorageServiceExtensions
    {
        private const int UniqueTagLength = 64;

        public static async Task<Uri> SavePostedFile(this IFileStorageService fileStorageService, HttpPostedFile file, string fileName, int tagLength = UniqueTagLength, CancellationToken cancellationToken = default)
        {
            return await fileStorageService.UploadFile(fileName, file.ContentType, file.InputStream, cancellationToken);
        }
    }
}