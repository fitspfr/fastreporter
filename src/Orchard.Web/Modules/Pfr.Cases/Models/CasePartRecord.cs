using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement.Records;

namespace Pfr.Cases.Models
{
    public class CasePartRecord:ContentPartRecord
    {
        public virtual int CaseNumber { get; set; }
    }
}