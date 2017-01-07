using Orchard.ContentManagement.MetaData;
using Orchard.Data.Migration;
using Pfr.Cases.Models;

namespace Pfr.Cases {
    public class Migrations : DataMigrationImpl {
        public int Create() {
            ContentDefinitionManager.AlterTypeDefinition("Case",
                cfg => cfg
                    .WithPart(typeof(CasePart).Name)
                );
            
            return 1;
        }
        public int UpdateFrom1()
        {
            SchemaBuilder.CreateTable(typeof(CasePartRecord).Name,
                table => table
                  .ContentPartRecord()
                  .Column<int>("CaseNumber"));

            return 2;
        }
    }
}