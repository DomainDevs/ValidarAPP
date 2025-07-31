using System;
using System.Runtime.Serialization;

namespace Sistran.Company.ExternalIssuanceServices.Models
{
    [DataContract]
    public class Beneficiary
    {
        public int BeneficiaryTypeCode { get; set; }
        public NaturalPerson NaturalPerson { get; set; }
        public LegalPerson LegalPerson { get; set; }
        public int BeneficiaryAddressTypeCode { get; set; }
        public string BeneficiaryAddressStreet { get; set; }
        public int BeneficiaryAddressCountryCode { get; set; }
        public int BeneficiaryAddressStateCode { get; set; }
        public int BeneficiaryAddressCityCode { get; set; }
        public int BeneficiaryEmailTypeCode { get; set; }
        public string BeneficiaryEmail { get; set; }
        public int BeneficiaryPhoneTypeCode { get; set; }
        public long BeneficiaryPhoneNumber { get; set; }
        public decimal BeneficiaryParticipationPct { get; set; }

    }
}