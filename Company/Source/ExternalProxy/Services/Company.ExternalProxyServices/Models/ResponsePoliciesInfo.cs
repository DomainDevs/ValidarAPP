using System;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.ExternalProxyServices.Models
{
    [DataContract]
    public class ResponsePoliciesInfo
    {
        [DataMember]
        public int CompanyCode { get; set; }

        [DataMember]
        public string CompanyName { get; set; }

        [DataMember]
        public string PolicyNumber { get; set; }

        [DataMember]
        public double Order { get; set; }

        [DataMember]
        public string Plate { get; set; }

        [DataMember]
        public string Engine { get; set; }

        [DataMember]
        public string Chassis { get; set; }

        [DataMember]
        public DateTime EffectiveDate { get; set; }

        [DataMember]
        public DateTime EndEffectiveDate { get; set; }

        [DataMember]
        public string Valid { get; set; }

        [DataMember]
        public string GuiedCode { get; set; }

        [DataMember]
        public string Brand { get; set; }

        [DataMember]
        public string Class { get; set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public int Model { get; set; }

        [DataMember]
        public string Service { get; set; }

        [DataMember]
        public string InsuredTypeDocument { get; set; }

        [DataMember]
        public string DocumentNumber { get; set; }

        [DataMember]
        public string Insured { get; set; }

        [DataMember]
        public double InsuredAmount { get; set; }

        [DataMember]
        public string PTD { get; set; }

        [DataMember]
        public string PPD { get; set; }

        [DataMember]
        public string PH { get; set; }

        [DataMember]
        public string PPH { get; set; }

        [DataMember]
        public string RC { get; set; }

        [DataMember]
        public string InterchangeType { get; set; }

        [DataMember]
        public string HolderTypeDocument { get; set; }

        [DataMember]
        public string Color { get; set; }

        [DataMember]
        public string PolicyClass { get; set; }

        [DataMember]
        public string HolderDocumentNumber { get; set; }

        [DataMember]
        public string HolderName { get; set; }

        [DataMember]
        public string BeneficiaryTypeDocument { get; set; }

        [DataMember]
        public string BeneficiaryName { get; set; }

        [DataMember]
        public string BeneficiaryDocumentNumber { get; set; }
    }
}