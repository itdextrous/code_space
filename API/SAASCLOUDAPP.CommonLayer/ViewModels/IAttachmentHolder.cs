using System.Collections.Generic;

namespace SAASCLOUDAPP.CommonLayer.ViewModels
{
    public interface IAttachmentHolder
    {
        List<AttachmentVm> attachments { get; }
    }
}
