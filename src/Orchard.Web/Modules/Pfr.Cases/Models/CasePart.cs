using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;

namespace Pfr.Cases.Models
{
    public class CasePart:ContentPart<CasePartRecord>
    {
        [DisplayName("Case Number : ")]
        public int CaseNumber
        {
            get { return Retrieve(r => r.CaseNumber); }
            set { Store(r => r.CaseNumber, value); }
        }
    }
}