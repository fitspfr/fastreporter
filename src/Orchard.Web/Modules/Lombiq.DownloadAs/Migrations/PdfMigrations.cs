using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lombiq.DownloadAs.Models;
using Orchard.Data.Migration;
using Orchard.Environment.Extensions;

namespace Lombiq.DownloadAs.Migrations
{
    [OrchardFeature("Lombiq.DownloadAs.Pdf")]
    public class PdfMigrations : DataMigrationImpl
    {
        public int Create()
        {
            // Only creation of the DownloadAsPdfSettingsPartRecord table was here. 

            return 2;
        }

        public int UpdateFrom1()
        {
            SchemaBuilder.DropTable("DownloadAsPdfSettingsPartRecord");

            return 2;
        }
    }
}