using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Localization;

namespace Lombiq.DownloadAs.Models
{
    public class FileBuildWorkerDescriptor : IFileBuildWorkerDescriptor
    {
        public string SupportedFileExtension { get; set; }
        public LocalizedString DisplayName { get; set; }
    }
}