using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Pfr.Cases.Models;

namespace Pfr.Cases.Drivers
{
    public class CaseDriver:ContentPartDriver<CasePart>
    {
        protected override DriverResult Display(CasePart part, string displayType, dynamic shapeHelper)
        {
            return ContentShape("Parts_Case", () => shapeHelper.Parts_Case());
        }

        protected override DriverResult Editor(CasePart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_Case_Edit",
              () => shapeHelper.EditorTemplate(
                TemplateName: "Parts/Case",
                Model: part,
                Prefix: Prefix));
        }

        protected override DriverResult Editor(CasePart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part, Prefix, null, null);
            return Editor(part, shapeHelper);
        }
    }
}