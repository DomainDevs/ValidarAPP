using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ListRiskMatchingProcess.Model
{
    public class OnuDocument
    {
        public string DocumentType { get; set; }
        public string DocumentNumber { get; set; }
        public string Note { get; set; }
        public DateTime IssueDate { get; set; }
    }

    public class OnuAlias
    {
        public string Quality { get; set; }
        public string AliasName { get; set; }
    }

    public class OnuAdrress
    {
        public string Country { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
    }

    public class Nationality
    {
        public string Value { get; set; }
    }

    public class ListType
    {
        public string Value { get; set; }
    }

    public class LastDateUpdated
    {
        public string Value { get; set; }
    }
}
