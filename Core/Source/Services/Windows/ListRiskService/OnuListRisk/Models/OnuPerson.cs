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

        [XmlElement("UN_LIST_TYPE")]
        public string UnListType { get; set; }

        [XmlElement("REFERENCE_NUMBER")]
        public string ReferenceNumber { get; set; }

        [XmlElement("GENDER")]
        public string Gender { get; set; }

        [XmlElement("SUBMITTED_BY")]
        public string SubmitedBy { get; set; }

        [XmlElement("NAME_ORIGINAL_SCRIPT")]
        public string NamerOriginalScript { get; set; }

        [XmlElement("NATIONALITY")]
        public Nationality Nationality { get; set; }

        [XmlElement("NATIONALITY2")]
        public string Nationality2 { get; set; }

        [XmlElement("TITLE")]
        public Title Title { get; set; }

        [XmlElement("DESIGNATION")]
        public Designation Designation { get; set; }

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

        [XmlElement("INDIVIDUAL_PLACE_OF_BIRTH")]
        public List<PlaceOfBirth> PlaceOfBirths { get; set; }

        [XmlElement("INDIVIDUAL_DATE_OF_BIRTH")]
        public List<DateOfBirth> DateOfBirths { get; set; }

        public DateTime SiseReistrationDate { get; set; }

        [XmlElement("SORT_KEY")]
        public string SortKey { get; set; }

        [XmlElement("SORT_KEY_LAST_MOD")]
        public string SortKeyLastMod { get; set; }

        [XmlElement("DELISTED_ON")]
        public DateTime  DelistedOn { get; set; }
    }

}
