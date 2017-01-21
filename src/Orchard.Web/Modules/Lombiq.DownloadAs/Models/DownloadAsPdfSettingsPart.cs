using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Records;
using Orchard.Environment.Extensions;

namespace Lombiq.DownloadAs.Models
{
    [OrchardFeature("Lombiq.DownloadAs.Pdf")]
    public class DownloadAsPdfSettingsPart : ContentPart
    {
        public string CloudConvertApiKey
        {
            get { return this.Retrieve(x => x.CloudConvertApiKey); }
            set { this.Store(x => x.CloudConvertApiKey, value); }
        }
    }
}