using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lombiq.DownloadAs
{
    internal static class UrlHelper
    {
        public static bool UrlIsInternal(Uri itemUri, string url, out Uri uri)
        {
            if (Uri.IsWellFormedUriString(url, UriKind.Relative))
            {
                uri = new Uri(itemUri, url);
                return true;
            }
            else
            {
                uri = new Uri(url);
                return uri.Host == itemUri.Host;
            }
        }
    }
}