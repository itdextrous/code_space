using System;

namespace NaviAir.Core.Model
{
    public class FileUploadModel
    {
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public long FileSizeInBytes { get; set; }
        public long FileSizeInKb => (long)Math.Ceiling((double)FileSizeInBytes / 1024);
    }
}