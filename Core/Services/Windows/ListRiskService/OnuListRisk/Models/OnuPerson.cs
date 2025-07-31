using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OnuListRisk.Models
{
    [XmlRoot(ElementName = "INDIVIDUAL")]
    public class OnuPerson
    {
        [XmlElement("DATAID")]
        public int DataId { get; set; }

        [XmlElement("VERSIONNUM")]
        public int VersionNum { get; set; }

        [XmlElement("FIRST_NAME")]
        public string FirstName { get; set; }

        [XmlElement("SECOND_NAME")]
        public string SecondName { get; set; }

        [XmlElement("THIRD_NAME")]
        public string ThirdName { get; set; }

        [XmlElement("FOURTH_NAME")]
        public string FourthName { get; set; }

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

        [XmlElement("INDIVIDUAL_DOCUMENT")]
        public List<OnuDocument> Document { get; set; }

        [XmlElement("INDIVIDUAL_ALIAS")]
        public List<OnuAlias> Alias { get; set; }

        [XmlElement("INDIVIDUAL_ADDRESS")]
        public List<OnuAdrress> Adress { get; set; }

    }

}
