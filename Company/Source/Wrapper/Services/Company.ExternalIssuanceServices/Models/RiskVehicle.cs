using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.ExternalIssuanceServices.Models
{
    [DataContract]
    public class RiskVehicle
    {
        public string LicensePlate { get; set; }
        public string EngineSerialNumber { get; set; }
        public string ChassisSerialNumber { get; set; }
        public Insured Insured { get; set; }
        public bool IsInsuredBeneficiary { get; set; }
        public List<Beneficiary> ListBeneficiary { get; set; }
        public List<Accesory> ListAccesory { get; set; }
        public string Observations { get; set; }

    }
}