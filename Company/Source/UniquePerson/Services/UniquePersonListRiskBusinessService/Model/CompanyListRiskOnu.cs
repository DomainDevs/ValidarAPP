using System.Runtime.Serialization;

namespace Sistran.Company.Application.UniquePersonListRiskBusinessService.Model
{
    using System;
    using System.Collections.Generic;

    public class CompanyListRiskOnu
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string DataId { get; set; }

        [DataMember]
        public string CreatedUser { get; set; }

        [DataMember]
        public int VersionNum { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string SecondName { get; set; }

        [DataMember]
        public string ThirdName { get; set; }

        [DataMember]
        public string FourthName { get; set; }

        [DataMember]
        public string UnListType { get; set; }

        [DataMember]
        public string ReferenceNumber { get; set; }

        [DataMember]
        public string Gender { get; set; }

        [DataMember]
        public string SubmitedBy { get; set; }

        [DataMember]
        public string NamerOriginalScript { get; set; }

        [DataMember]
        public Nationality Nationality { get; set; }

        [DataMember]
        public ListType ListType { get; set; }

        [DataMember]
        public DateTime ListedOn { get; set; }

        [DataMember]
        public string Comments1 { get; set; }

        [DataMember]
        public List<LastDateUpdated> LastDateUpdated { get; set; }

        [DataMember]
        public List<OnuDocument> Document { get; set; }

        [DataMember]
        public List<OnuAlias> Alias { get; set; }

        [DataMember]
        public List<OnuAdrress> Adress { get; set; }

        [DataMember]
        public List<PlaceOfBirth> PlaceOfBirths { get; set; }

        [DataMember]
        public DateTime SiseReistrationDate { get; set; }

        [DataMember]
        public string ListRiskDescription { get; set; }
        [DataMember]
        public string ListRiskTypeDescription { get; set; }
        [DataMember]
        public int Event { get; set; }
        [DataMember]
        public int ListRiskId { get; set; }

        [DataMember]
        public int ProcessId { get; set; }

        [DataMember]
        public int ListRiskType { get; set; }

        [DataMember]
        public DateTime AssignmentDate { get; set; }
    }


    public class OnuDocument
    {
        [DataMember]
        public string DocumentType { get; set; }

        [DataMember]
        public string DocumentType2 { get; set; }

        [DataMember]
        public string DocumentNumber { get; set; }

        [DataMember]
        public string Note { get; set; }

        [DataMember]
        public DateTime IssueDate { get; set; }

        [DataMember]
        public string IssueCountry { get; set; }

        [DataMember]
        public string CityOfIssue { get; set; }

    }

    public class OnuAlias
    {
        [DataMember]
        public string Quality { get; set; }

        [DataMember]
        public string AliasName { get; set; }

        [DataMember]
        public string DateOfBirth { get; set; }

        [DataMember]
        public string CityOfBirth { get; set; }

        [DataMember]
        public string CountryOfBirth { get; set; }

        [DataMember]
        public string Note { get; set; }
    }

    public class OnuAdrress
    {
        [DataMember]
        public string Country { get; set; }

        [DataMember]
        public string Street { get; set; }

        [DataMember]
        public string City { get; set; }

        [DataMember]
        public string StateProvince { get; set; }

        [DataMember]
        public string ZipCode { get; set; }

        [DataMember]
        public string Note { get; set; }
    }


    public class Nationality
    {
        [DataMember]
        public string Value { get; set; }
    }

    public class ListType
    {
        [DataMember]
        public string Value { get; set; }
    }

    public class LastDateUpdated
    {
        [DataMember]
        public string Value { get; set; }
    }

    public class PlaceOfBirth
    {
        [DataMember]
        public string StateProvince { get; set; }

        [DataMember]
        public string City { get; set; }

        [DataMember]
        public string Country { get; set; }

        [DataMember]
        public string Note { get; set; }
    }
}
