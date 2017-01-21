using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard.Localization;

namespace Lombiq.DownloadAs.Models
{
    public interface IFileBuildWorkerDescriptor
    {
        string SupportedFileExtension { get; }
        LocalizedString DisplayName { get; }
    }
}
