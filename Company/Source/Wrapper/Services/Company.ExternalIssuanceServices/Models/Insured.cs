using System;
using System.Runtime.Serialization;

namespace Sistran.Company.ExternalIssuanceServices.Models
{
    [DataContract]
    public class Insured
    {
        public NaturalPerson NaturalPerson { get; set; }
        public LegalPerson LegalPerson { get; set; }
        public int AddressTypeCode { get; set; }
        public string AddressStreet { get; set; }
        public int AddressCountryCode { get; set; }
        public int AddressStateCode { get; set; }
        public int AddressCityCode { get; set; }
        public int EmailTypeCode { get; set; }
        public string Email { get; set; }
        public int PhoneTypeCode { get; set; }
        public long PhoneNumber { get; set; }
        public bool IsCommercialClient { get; set; }
        public bool IsSms { get; set; }
        public bool IsMailAddress { get; set; }
        public int WorkerTypeCode { get; set; }

    }
}