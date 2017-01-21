using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using Lombiq.DownloadAs.Models;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.Settings;
//using RestSharp;
using TheArtOfDev.HtmlRenderer.PdfSharp;
using TheArtOfDev.HtmlRenderer.Core.Entities;

namespace Lombiq.DownloadAs.Services
{
    [OrchardFeature("Lombiq.DownloadAs.Pdf")]
    public class PdfFileBuilderWorker : IFileBuilderWorker
    {
        private readonly IFlattenedHtmlGenerator _htmlGenerator;
        private readonly ISiteService _siteService;

        private IFileBuildWorkerDescriptor _descriptor;
        public IFileBuildWorkerDescriptor Descriptor
        {
            get
            {
                if (_descriptor != null) return _descriptor;

                _descriptor = new FileBuildWorkerDescriptor
                {
                    SupportedFileExtension = "pdf",
                    DisplayName = T("PDF")
                };

                return _descriptor;
            }
        }

        public Localizer T { get; set; }


        public PdfFileBuilderWorker(
            IFlattenedHtmlGenerator htmlGenerator,
            ISiteService siteService)
        {
            _htmlGenerator = htmlGenerator;
            _siteService = siteService;

            T = NullLocalizer.Instance;
        }


        public Stream Build(IEnumerable<IContent> contents)
        {
            string htmlContent = _htmlGenerator.GenerateHtml(contents, Descriptor.SupportedFileExtension);
            PdfGenerateConfig config = new PdfGenerateConfig();
            config.PageSize = PdfSharp.PageSize.A4;
            config.SetMargins(20);

            //var doc = PdfGenerator.GeneratePdf(htmlContent, config, null, OnStylesheetLoad);
            var doc = TheArtOfDev.HtmlRenderer.PdfSharp.PdfGenerator.GeneratePdf(htmlContent, config, null, OnStylesheetLoad);
            
            byte[] fileContents = null;
            using (MemoryStream stream = new MemoryStream())
            {
                doc.Save(stream, true);
                fileContents = stream.ToArray();
            }
            return new MemoryStream(fileContents);

            //using (var htmlStream = new MemoryStream(Encoding.UTF8.GetBytes(_htmlGenerator.GenerateHtml(contents, Descriptor.SupportedFileExtension))))
            //using (var tmpStream = new MemoryStream())
            //{
            //    htmlStream.CopyTo(tmpStream);

            //    var apiKey = _siteService.GetSiteSettings().As<DownloadAsPdfSettingsPart>().CloudConvertApiKey;

            //    var client = new RestClient("https://api.cloudconvert.org/");
            //    var request = new RestRequest("process");
            //    request.AddParameter("apikey", apiKey);
            //    request.AddParameter("inputformat", "html");
            //    request.AddParameter("outputformat", "pdf");
            //    request.AddParameter("converter", "unoconv");
            //    var response = client.Post<InitResponse>(request);
            //    var processUrl = response.Data.Url;
            //    if (response.StatusCode != HttpStatusCode.OK) throw new OrchardException(T("PDF generation failed as the CloudConvert API returned an error when trying to open the converion process. Status code: {0}. CloudConvert error message: {1}.", response.StatusCode, response.Data.Error));

            //    var processClient = new RestClient("https:" + processUrl);

            //    var uploadRequest = new RestRequest();
            //    uploadRequest.AddFile("file", tmpStream.ToArray(), "output.html");
            //    uploadRequest.AddParameter("input", "upload");
            //    uploadRequest.AddParameter("format", "pdf");
            //    var uploadResponse = processClient.Post(uploadRequest);
            //    if (uploadResponse.StatusCode != HttpStatusCode.OK) throw new OrchardException(T("PDF generation failed as the CloudConvert API returned an error when trying to upload the HTML file. Status code: {0}.", uploadResponse.StatusCode));

            //    var processResponse = processClient.Get<ProcessResponse>(new RestRequest());
            //    var tryCount = 0;
            //    while (processResponse.Data.Step != "finished" && tryCount < 20)
            //    {
            //        Thread.Sleep(2000); // Yes, doing this like this is bad. No better idea yet.
            //        processResponse = processClient.Get<ProcessResponse>(new RestRequest());
            //        tryCount++;
            //    }
            //    if (tryCount == 20) throw new OrchardException(T("PDF generation failed as CloudConvert didn't finish the conversion after 40s."));


            //    using (var wc = new WebClient())
            //    {
            //        var fileBytes = wc.DownloadData("https:" + processResponse.Data.Output.Url);
            //        return new MemoryStream(fileBytes);
            //    }
            //}
        }

        public static void OnStylesheetLoad(object sender, HtmlStylesheetLoadEventArgs e)
        {
            var stylesheet = GetStylesheet(e.Src);
            if (stylesheet != null)
                e.SetStyleSheet = stylesheet;
        }

        /// <summary>
        /// Get stylesheet by given key.
        /// </summary>
        public static string GetStylesheet(string src)
        {
            if (src == "StyleSheet")
            {
                return @"h1, h2, h3 { color: navy; font-weight:normal; }
                    h1 { margin-bottom: .47em }
                    h2 { margin-bottom: .3em }
                    h3 { margin-bottom: .4em }
                    ul { margin-top: .5em }
                    ul li {margin: .25em}
                    body { font:10pt Tahoma }
		            pre  { border:solid 1px gray; background-color:#eee; padding:1em }
                    a:link { text-decoration: none; }
                    a:hover { text-decoration: underline; }
                    .gray    { color:gray; }
                    .example { background-color:#efefef; corner-radius:5px; padding:0.5em; }
                    .whitehole { background-color:white; corner-radius:10px; padding:15px; }
                    .caption { font-size: 1.1em }
                    .comment { color: green; margin-bottom: 5px; margin-left: 3px; }
                    .comment2 { color: green; }";
            }
            return null;
        }


        public class InitResponse
        {
            public string Error { get; set; }
            public string Url { get; set; }
        }

        public class ProcessResponse
        {
            public string Step { get; set; }
            public ProcessOutput Output { get; set; }
        }

        public class ProcessOutput
        {
            public string Url { get; set; }
        }
    }
}