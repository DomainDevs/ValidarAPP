using System;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.Vehicles.VehicleServices.Models
{
    [DataContract]
    public class CurrentEndorsement
    {
        [DataMember]
        public int IdPv { get; set; }
        [DataMember]
        public string LicencePlate { get; set; }
        [DataMember]
        public string Engine { get; set; }
        [DataMember]
        public string TxtChasis { get; set; }
        [DataMember]
        public decimal NroPol { get; set; }
        [DataMember]
        public int CodSucursal { get; set; }
        [DataMember]
        public DateTime FechaEmision { get; set; }
        [DataMember]
        public DateTime DateValidateSince { get; set; }
        [DataMember]
        public DateTime DateValidateUntil { get; set; }
        [DataMember]
        public int NumeroEndoso { get; set; }
        [DataMember]
        public int CodTipoEndo { get; set; }
        [DataMember]
        public int CodGrupoEndo { get; set; }
    }
}
