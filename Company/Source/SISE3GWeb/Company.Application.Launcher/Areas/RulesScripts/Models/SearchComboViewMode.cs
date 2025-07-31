using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.RulesScripts.Models
{
    public class SearchComboViewMode
    {
        public int PropertyName { get; set; }

        public int SymbolComparator { get; set; }

        public string ValueFilter { get; set; }
    }
}