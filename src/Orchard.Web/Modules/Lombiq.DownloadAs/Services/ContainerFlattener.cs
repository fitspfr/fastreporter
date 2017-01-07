using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using HtmlAgilityPack;
using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Aspects;
using Orchard.Core.Common.Models;
using Orchard.DisplayManagement;
using Orchard.Settings;

namespace Lombiq.DownloadAs.Services
{
    public interface IContainerFlattener : IDependency
    {
        IEnumerable<IContent> Flatten(IContent container);
    }


    public class ContainerFlattener : IContainerFlattener
    {
        private readonly IShapeDisplay _shapeDisplay;
        private readonly ISiteService _siteService;


        public ContainerFlattener(
            IShapeDisplay shapeDisplay,
            ISiteService siteService)
        {
            _shapeDisplay = shapeDisplay;
            _siteService = siteService;
        }


        public IEnumerable<IContent> Flatten(IContent container)
        {
            return FlattenOneLevel(container);
        }


        private IEnumerable<IContent> FlattenOneLevel(IContent container)
        {
            var items = new List<IContent>();

            items.Add(container);

            var containedItems = container.ContentItem.ContentManager
                .Query()
                .Where<CommonPartRecord>(record => record.Container.Id == container.ContentItem.Id)
                .OrderBy<CommonPartRecord>(record => record.Id)
                .List<IContent>();

            // If the contained items are linked from the container, take the links' order into account. Also if links 
            // are present only linked items will be processed.
            if (containedItems.Any())
            {
                var aliases = containedItems
                    .Where(content => content.As<IAliasAspect>() != null)
                    .Select(content => new
                    {
                        Content = content,
                        Path = content.As<IAliasAspect>().Path
                    })
                    .ToDictionary(alias => alias.Path);

                var siteUri = new Uri(_siteService.GetSiteSettings().BaseUrl);
                var doc = new HtmlDocument();
                var html = _shapeDisplay.Display(container.ContentItem.ContentManager.BuildDisplay(container, "File-ContainerFlattening"));
                doc.LoadHtml(html);

                var aliasAspect = container.As<IAliasAspect>();
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
                                        items.AddRange(FlattenOneLevel(aliases[alias].Content));
                                    }
                                }
                            }
                        }
                    }
                }

                if (items.Count == 1) // No contained item was linked
                {
                    foreach (var item in containedItems)
                    {
                        items.AddRange(FlattenOneLevel(item));
                    }
                }
            }

            return items;
        }
    }
}