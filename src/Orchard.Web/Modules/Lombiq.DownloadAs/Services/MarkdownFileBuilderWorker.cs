using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using Lombiq.DownloadAs.Models;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using Orchard.Localization;

namespace Lombiq.DownloadAs.Services
{
    [OrchardFeature("Lombiq.DownloadAs.Markdown")]
    public class MarkdownFileBuilderWorker : IFileBuilderWorker
    {
        private readonly IFlattenedHtmlGenerator _htmlGenerator;

        private IFileBuildWorkerDescriptor _descriptor;
        public IFileBuildWorkerDescriptor Descriptor
        {
            get
            {
                if (_descriptor != null) return _descriptor;

                _descriptor = new FileBuildWorkerDescriptor
                {
                    SupportedFileExtension = "md",
                    DisplayName = T("Markdown")
                };

                return _descriptor;
            }
        }

        public Localizer T { get; set; }


        public MarkdownFileBuilderWorker(IFlattenedHtmlGenerator htmlGenerator)
        {
            _htmlGenerator = htmlGenerator;

            T = NullLocalizer.Instance;
        }


        public Stream Build(IEnumerable<IContent> contents)
        {
            using (var wc = new WebClient())
            {
                var fields = new NameValueCollection();
                fields.Add("html", _htmlGenerator.GenerateHtml(contents, Descriptor.SupportedFileExtension));
                return new MemoryStream(wc.UploadValues("http://fuckyeahmarkdown.com/go/", fields));
            }
        }
    }
}