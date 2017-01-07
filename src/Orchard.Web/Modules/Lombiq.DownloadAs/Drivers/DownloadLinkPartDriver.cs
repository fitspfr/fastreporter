using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lombiq.DownloadAs.Models;
using Lombiq.DownloadAs.Services;
using Orchard.ContentManagement.Drivers;

namespace Lombiq.DownloadAs.Drivers
{
    public class DownloadLinkPartDriver : ContentPartDriver<DownloadLinkPart>
    {
        private readonly IFileBuilder _fileBuilder;


        public DownloadLinkPartDriver(IFileBuilder fileBuilder)
        {
            _fileBuilder = fileBuilder;
        }
    
            
        protected override DriverResult Display(DownloadLinkPart part, string displayType, dynamic shapeHelper)
        {
            return ContentShape("Parts_DownloadLink",
                () => shapeHelper.Parts_DownloadLink(Workers: _fileBuilder.GetWorkers()));
        }
    }
}