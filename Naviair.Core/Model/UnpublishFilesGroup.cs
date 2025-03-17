using System;
using System.Collections.Generic;

namespace NaviAir.Core.Model
{
    public class UnpublishFilesGroup
    {
        public DateTime? PublishAt { get; set; }
        public IEnumerable<UnpublishedFilesSubListModel> UnpublishedFilesSubListModels { get; set; }
    }
}