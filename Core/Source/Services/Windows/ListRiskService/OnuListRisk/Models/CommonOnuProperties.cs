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

        [XmlElement("TYPE_OF_DOCUMENT2")]
        public string DocumentType2 { get; set; }

        [XmlElement("NUMBER")]
        public string DocumentNumber { get; set; }

        [XmlElement("NOTE")]
        public string Note { get; set; }

        [XmlElement("DATE_OF_ISSUE")]
        public DateTime IssueDate { get; set; }

        [XmlElement("ISSUING_COUNTRY")]
        public string IssueCountry { get; set; }

        [XmlElement("COUNTRY_OF_ISSUE")]
        public string CountryOfIssue { get; set; }

        [XmlElement("CITY_OF_ISSUE")]
        public string CityOfIssue { get; set; }
    }

    [XmlType(TypeName = "INDIVIDUAL_ALIAS")]
    public class OnuAlias
    {
        [XmlElement("QUALITY")]
        public string Quality { get; set; }

        [XmlElement("ALIAS_NAME")]
        public string AliasName { get; set; }

        [XmlElement("DATE_OF_BIRTH")]
        public string DateOfBirth { get; set; }

        [XmlElement("CITY_OF_BIRTH")]
        public string CityOfBirth { get; set; }

        [XmlElement("COUNTRY_OF_BIRTH")]
        public string CountryOfBirth { get; set; }

        [XmlElement("NOTE")]
        public string Note { get; set; }
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

        [XmlElement("STATE_PROVINCE")]
        public string StateProvince { get; set; }

        [XmlElement("ZIP_CODE")]
        public string ZipCode { get; set; }

        [XmlElement("NOTE")]
        public string Note { get; set; }
    }

    [XmlRoot(ElementName = "NATIONALITY")]
    public class Nationality
    {

        [XmlElement("VALUE")]
        public string Value { get; set; }
    }

    [XmlRoot(ElementName = "DESIGNATION")]
    public class Designation
    {

        [XmlElement("VALUE")]
        public string Value { get; set; }
    }

    [XmlRoot(ElementName = "TITLE")]
    public class Title
    {

        [XmlElement("VALUE")]
        public string Value { get; set; }
    }

    [XmlRoot(ElementName = "INDIVIDUAL_DATE_OF_BIRTH")]
    public class DateOfBirth
    {

        [XmlElement("TYPE_OF_DATE")]
        public string TypeOfDate { get; set; }

        [XmlElement("NOTE")]
        public string Note { get; set; }

        [XmlElement("DATE")]
        public string Date { get; set; }

        [XmlElement("YEAR")]
        public int Year { get; set; }

        [XmlElement("FROM_YEAR")]
        public int FromYear { get; set; }

        [XmlElement("TO_YEAR")]
        public int ToYear { get; set; }
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

    [XmlRoot(ElementName = "INDIVIDUAL_PLACE_OF_BIRTH")]
    public class PlaceOfBirth
    {
        [XmlElement("STATE_PROVINCE")]
        public string StateProvince { get; set; }

        [XmlElement("CITY")]
        public string City { get; set; }

        [XmlElement("COUNTRY")]
        public string Country { get; set; }

        [XmlElement("NOTE")]
        public string Note { get; set; }
    }
}
