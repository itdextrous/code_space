using NaviAir.Core.Model;

namespace NaviAir.Core.Service.FileStorage
{
	public interface IFileStorageService
    {
        Task<List<FileUploadModel>> UploadFiles(HttpContent httpContent);
    }
}