using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OnuListRisk.Models
{
    [XmlRoot(ElementName = "ENTITY")]
    public class OnuEntity
    {
        [XmlElement("DATAID")]
        public int DataId { get; set; }

        [XmlElement("VERSIONNUM")]
        public int VersionNum { get; set; }

        [XmlElement("FIRST_NAME")]
        public string FirstName { get; set; }

        [XmlElement("REFERENCE_NUMBER")]
        public string ReferenceNumber { get; set; }

        [XmlElement("NATIONALITY")]
        public Nationality Nationality { get; set; }

        [XmlElement("LIST_TYPE")]
        public ListType ListType { get; set; }

        [XmlElement("LISTED_ON")]
        public DateTime ListedOn { get; set; }

        [XmlElement("COMMENTS1")]
        public string Comments1 { get; set; }

        [XmlElement("LAST_DAY_UPDATED")]
        public List<LastDateUpdated> LastDateUpdated { get; set; }

        [XmlElement("ENTITY_DOCUMENT")]
        public List<OnuDocument> Document { get; set; }

        [XmlElement("ENTITY_ALIAS")]
        public List<OnuAlias> Alias { get; set; }

        [XmlElement("ENTITY_ADDRESS")]
        public List<OnuAdrress> Adress { get; set; }
    }
}
