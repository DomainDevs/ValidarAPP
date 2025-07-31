using System;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.OperationQuotaServices.EEProvider.Models.OperationQuota
{
    [DataContract]
    public class Prospect
    {
        [DataMember]
        public int DocumentType { get; set; }
        [DataMember]
        public string DocumentNumber { get; set; }
        [DataMember]
        public string Businessname { get; set; }
        [DataMember]
        public int City { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public int Phone { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public DateTime ConstitutionDate { get; set; }
        [DataMember]
        public string LegalRepresentative { get; set; }
        [DataMember]
        public string FiscalReviewer { get; set; }
        [DataMember]
        public int EconomicActivity { get; set; }
        [DataMember]
        public int IndividualTypeCd { get; set; }
        [DataMember]
        public string AdditionalInfo { get; set; }
        [DataMember]
        public int CountryCd { get; set; }
        [DataMember]
        public int StateCd { get; set; }
        [DataMember]
        public int CompanytypeCd { get; set; }
        [DataMember]
        public int AddressTypecd { get; set; }
        [DataMember]
        public int? IndividualId { get; set; }







    }
}
