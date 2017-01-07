using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Lombiq.DownloadAs.Models
{
    public interface IFileResult
    {
        string MimeType { get; }
        Stream OpenRead();
    }
}
