using System;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.ExternalProxyServices.Models
{
    [DataContract]
    public class ResponseSinisterInfo
    {
        [DataMember]
        public int CompanyCode { get; set; }

        [DataMember]
        public string CompanyName { get; set; }

        [DataMember]
        public string SinisterNumber { get; set; }

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
        public DateTime SinistereDate { get; set; }

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
        public string InsuredTypeDocument { get; set; }

        [DataMember]
        public string DocumentNumber { get; set; }

        [DataMember]
        public string Insured { get; set; }

        [DataMember]
        public double InsuredAmount { get; set; }

        [DataMember]
        public string TypeCruce { get; set; }

        [DataMember]
        public string Color { get; set; }

        [DataMember]
        public string SinisterSucursal { get; set; }

        [DataMember]
        public string PolicyClass { get; set; }

        [DataMember]
        public string DriverIdentificarion { get; set; }

        [DataMember]
        public string DriverName { get; set; }

        [DataMember]
        public string Plate1 { get; set; }

        [DataMember]
        public string Service { get; set; }

        [DataMember]
        public Sistran.Company.Application.ExternalProxyServices.ConsultaCexper.AmparoCexper[] Amparos { get; set; }
    }
}
