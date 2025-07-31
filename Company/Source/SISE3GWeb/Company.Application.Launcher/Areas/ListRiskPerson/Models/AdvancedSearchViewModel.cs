using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.ListRiskPerson.Models
{
    public class AdvancedSearchViewModel
    {
        public int DocumentNumber { get; set; }
        public string NickName { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public int ListRiskId { get; set; }

    }
}