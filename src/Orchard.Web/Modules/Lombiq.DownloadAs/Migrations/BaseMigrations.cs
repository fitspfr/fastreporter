using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Data.Migration;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Lombiq.DownloadAs.Models;

namespace Lombiq.DownloadAs.Migrations
{
    public class BaseMigrations : DataMigrationImpl
    {
        public int Create()
        {
            ContentDefinitionManager.AlterPartDefinition(typeof(DownloadLinkPart).Name,
                part => part
                    .Attachable()
                    .WithDescription("Displays a download link for the content item.")
                );

            return 2;
        }

        public int UpdateFrom1()
        {
            SchemaBuilder.DropTable("DownloadAsSettingsPartRecord");

            return 2;
        }
    }
}