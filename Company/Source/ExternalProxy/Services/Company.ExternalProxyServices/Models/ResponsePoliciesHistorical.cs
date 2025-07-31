using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.ExternalProxyServices.Models
{
    [DataContract]
    public class ResponsePoliciesHistorical
    {
        [DataMember]
        public short CompanyCode { get; set; }

        [DataMember]
        public string CompanyName { get; set; }

        [DataMember]
        public string PolicyNumber { get; set; }

        [DataMember]
        public long Order { get; set; }

        [DataMember]
        public string LicensePlate { get; set; }

        [DataMember]
        public string Motor { get; set; }

        [DataMember]
        public string Chassis { get; set; }

        [DataMember]
        public DateTime EffectiveDate { get; set; }

        [DataMember]
        public DateTime EndEffectiveDate { get; set; }

        [DataMember]
        public string Current { get; set; }

        [DataMember]
        public string GuideCode { get; set; }

        [DataMember]
        public string Make { get; set; }

        [DataMember]
        public string Class { get; set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public short Model { get; set; }

        [DataMember]
        public string Service { get; set; }

        [DataMember]
        public string DocumentTypeInsured { get; set; }

        [DataMember]
        public string DocumentNumber { get; set; }

        [DataMember]
        public string Insured { get; set; }

        [DataMember]
        public double InsuredValue { get; set; }

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
        public string TypeCrossing { get; set; }

        [DataMember]
        public string Color { get; set; }

        [DataMember]
        public string ClassPolicy { get; set; }

    }
}
