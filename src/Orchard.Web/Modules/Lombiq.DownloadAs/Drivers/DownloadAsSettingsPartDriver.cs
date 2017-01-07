using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lombiq.DownloadAs.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;

namespace Lombiq.DownloadAs.Drivers
{
    public class DownloadAsSettingsPartDriver : ContentPartDriver<DownloadAsSettingsPart>
    {
        protected override DriverResult Editor(DownloadAsSettingsPart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_DownloadAsSettings_Edit",
                () => shapeHelper.EditorTemplate(
                    TemplateName: "Parts.DownloadAsSettings",
                    Model: part,
                    Prefix: Prefix));
        }

        protected override DriverResult Editor(DownloadAsSettingsPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part, Prefix, null, null);
            return Editor(part, shapeHelper);
        }
    }
}