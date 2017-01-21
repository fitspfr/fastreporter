using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Records;

namespace Lombiq.DownloadAs.Models
{
    public class DownloadAsSettingsPart : ContentPart
    {
        public int CacheTimeoutMinutes
        {
            get { return this.Retrieve(x => x.CacheTimeoutMinutes, 10); }
            set { this.Store(x => x.CacheTimeoutMinutes, value); }
        }

        public TimeSpan CacheTimeout
        {
            get { return new TimeSpan(0, CacheTimeoutMinutes, 0); }
        }
    }
}