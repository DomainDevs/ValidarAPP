using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OnuListRisk.Models
{
    [XmlRoot(ElementName = "INDIVIDUAL_DOCUMENT")]
    public class OnuDocument
    {
        [XmlElement("TYPE_OF_DOCUMENT")]
        public string DocumentType { get; set; }

        [XmlElement("NUMBER")]
        public string DocumentNumber { get; set; }

        [XmlElement("NOTE")]
        public string Note { get; set; }

        [XmlElement("DATE_OF_ISSUE")]
        public DateTime IssueDate { get; set; }

    }

    [XmlRoot(ElementName = "INDIVIDUAL_ALIAS")]
    public class OnuAlias
    {
        [XmlElement("QUALITY")]
        public string Quality { get; set; }

        [XmlElement("ALIAS_NAME")]
        public string AliasName { get; set; }
    }

    [XmlRoot(ElementName = "INDIVIDUAL_ADDRESS")]
    public class OnuAdrress
    {
        [XmlElement("COUNTRY")]
        public string Country { get; set; }

        [XmlElement("STREET")]
        public string Street { get; set; }

        [XmlElement("CITY")]
        public string City { get; set; }
    }

    [XmlRoot(ElementName = "NATIONALITY")]
    public class Nationality
    {

        [XmlElement("VALUE")]
        public string Value { get; set; }
    }

    [XmlRoot(ElementName = "LIST_TYPE")]
    public class ListType
    {

        [XmlElement("VALUE")]
        public string Value { get; set; }
    }

    [XmlRoot(ElementName = "LAST_DAY_UPDATED")]
    public class LastDateUpdated
    {

        [XmlElement("VALUE")]
        public string Value { get; set; }
    }
}
