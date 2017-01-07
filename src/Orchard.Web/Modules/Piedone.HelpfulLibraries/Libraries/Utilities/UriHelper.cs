﻿using System.Linq;
using Orchard.Environment.Extensions;

namespace Piedone.HelpfulLibraries.Utilities
{
    [OrchardFeature("Piedone.HelpfulLibraries.Utilities")]
    public static class UriHelper
    {
        /// <summary>
        /// Combines uri segments with forward slashes (much like Path.Combine() for local paths)
        /// </summary>
        /// <param name="segments">The segments to combine</param>
        public static string Combine(params string[] segments)
        {
            var joined = string.Join("/", segments.Select(f => f.Trim().Trim('/')));
            if (!string.IsNullOrEmpty(segments.Last()) && segments.Last().Last() == '/' && joined.Last() != '/') return joined + "/";
            return joined;
        }
    }
}
