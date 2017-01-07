using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Lombiq.DownloadAs.Services;
using Orchard.ContentManagement;
using Orchard.Security;
using Orchard.Themes;

namespace Lombiq.DownloadAs.Controllers
{
    public class DownloadController : Controller
    {
        private readonly IContentManager _contentManager;
        private readonly IAuthorizer _authorizer;
        private readonly IFileBuilder _fileBuilder;


        public DownloadController(
            IContentManager contentManager,
            IAuthorizer authorizer, 
            IFileBuilder fileBuilder)
        {
            _contentManager = contentManager;
            _authorizer = authorizer;
            _fileBuilder = fileBuilder;
        }


        public ActionResult DownloadAs(int? id, string extension)
        {
            if (id == null) return HttpNotFound();

            if (string.IsNullOrEmpty(extension) || !Regex.IsMatch(extension, @"^\w+$") || !_fileBuilder.HasWorkerFor(extension))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var item = _contentManager.Get(id.Value);

            if (!_authorizer.Authorize(Orchard.Core.Contents.Permissions.ViewContent, item)) return new HttpUnauthorizedResult();

            var result = _fileBuilder.BuildRecursive(item, extension);
            var fileName = _contentManager.GetItemMetadata(item).DisplayText;
            if (string.IsNullOrEmpty(fileName)) fileName = item.Id.ToString();
            return File(result.OpenRead(), result.MimeType, fileName + "." + extension);
        }
    }
}