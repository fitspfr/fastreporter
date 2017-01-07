using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Data;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.ContentManagement.Records;
using Pfr.Cases.Models;

namespace Pfr.Cases.Handlers
{
    public class CasePartHandler : ContentHandler
    {
        public CasePartHandler(IRepository<CasePartRecord> repository)
        {
            Filters.Add(new ActivatingFilter<CasePart>("Case"));
            Filters.Add(StorageFilter.For(repository));

            OnInitializing<CasePart>((context, casePart) => {
                casePart.CaseNumber = casePart.Id;
            });
        }
    }
}