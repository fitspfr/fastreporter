using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using HtmlAgilityPack;
using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Aspects;
using Orchard.DisplayManagement;
using Orchard.Localization;
using Orchard.Settings;

namespace Lombiq.DownloadAs.Services
{
    public interface IFlattenedHtmlGenerator : IDependency
    {
        string GenerateHtml(IEnumerable<IContent> contents, string extension);
    }


    public class FlattenedHtmlGenerator : IFlattenedHtmlGenerator
    {
        private readonly ISiteService _siteService;
        private readonly dynamic _shapeFactory;
        private readonly IShapeDisplay _shapeDisplay;


        public FlattenedHtmlGenerator(
            ISiteService siteService,
            IShapeFactory shapeFactory,
            IShapeDisplay shapeDisplay)
        {
            _siteService = siteService;
            _shapeFactory = shapeFactory;
            _shapeDisplay = shapeDisplay;
        }


        public string GenerateHtml(IEnumerable<IContent> contents, string extension)
        {
            var firstContent = contents.First();
            var contentManager = firstContent.ContentItem.ContentManager;

            var aliases = contents
                .Where(content => content.As<IAliasAspect>() != null)
                .Select(content => new
                {
                    Id = content.ContentItem.Id,
                    Path = content.As<IAliasAspect>().Path
                })
                .ToDictionary(alias => alias.Path);

            var siteUri = new Uri(_siteService.GetSiteSettings().BaseUrl);
            var contentShapes = contents.Select(content =>
            {
                IShape contentShape = contentManager.BuildDisplay(content, "File-" + extension);

                contentShape.Metadata.OnDisplayed(context =>
                {
                    var doc = new HtmlDocument();
                    doc.LoadHtml(context.ShapeMetadata.ChildContent.ToHtmlString());

                    var aliasAspect = content.As<IAliasAspect>();
                    if (aliasAspect != null)
                    {
                        var itemUri = new Uri(siteUri, aliasAspect.Path);

                        var links = doc.DocumentNode.SelectNodes("//a[@href]");
                        if (links != null) // See: https://htmlagilitypack.codeplex.com/workitem/29175
                        {
                            foreach (var link in links)
                            {
                                var href = link.GetAttributeValue("href", null);
                                if (href != null)
                                {
                                    Uri uri;
                                    if (UrlHelper.UrlIsInternal(itemUri, href, out uri))
                                    {
                                        var alias = uri.LocalPath.TrimStart('/');
                                        if (aliases.ContainsKey(alias))
                                        {
                                            link.SetAttributeValue("href", "#content-item-top-" + aliases[alias].Id);
                                        }
                                        else
                                        {
                                            link.SetAttributeValue("href", uri.ToString());
                                        }
                                    }
                                }
                            }
                        }

                        var srcElements = doc.DocumentNode.SelectNodes("//*[@src]");
                        if (srcElements != null)
                        {
                            foreach (var element in srcElements)
                            {
                                var src = element.GetAttributeValue("src", null);
                                if (src != null)
                                {
                                    Uri uri;
                                    if (UrlHelper.UrlIsInternal(itemUri, src, out uri))
                                    {
                                        element.SetAttributeValue("src", uri.ToString());
                                    }
                                }
                            }
                        }
                    }

                    var stringBuilder = new StringBuilder();
                    using (var stringWriter = new StringWriter(stringBuilder))
                    {
                        doc.Save(stringWriter);
                    }
                    context.ShapeMetadata.ChildContent = new HtmlString(stringBuilder.ToString());
                });

                return contentShape;
            });

            var shape = _shapeFactory.File_ContentsWrapper(
                Title: contentManager.GetItemMetadata(firstContent).DisplayText,
                ContentShapes: contentShapes);
            shape.Metadata.Alternates.Add("File_ContentsWrapper__" + extension);
            shape.Metadata.Alternates.Add("File_ContentsWrapper__" + extension + " __" + firstContent.ContentItem.Id);
            shape.Metadata.Alternates.Add("File_ContentsWrapper__" + firstContent.ContentItem.Id);

            return _shapeDisplay.Display(shape);
        }
    }
}