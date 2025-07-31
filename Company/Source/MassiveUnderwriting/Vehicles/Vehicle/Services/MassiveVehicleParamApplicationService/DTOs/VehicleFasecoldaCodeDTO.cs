using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.Vehicles.MassiveVehicleParamApplicationService.DTOs
{
    [DataContract]
    public class VehicleFasecoldaCodeDTO
    {
        [DataMember]
        public string Novelty { get; set; }
        
        [DataMember]
        public string Brand { get; set; }
        
        [DataMember]
        public string AutoClass { get; set; }

        [DataMember]
        public string Codigo { get; set; }

        [DataMember]
        public string HomologoCode { get; set; }

        [DataMember]
        public string Reference1 { get; set; }

        [DataMember]
        public string Reference2 { get; set; }

        [DataMember]
        public string Reference3 { get; set; }

        [DataMember]
        public string Weight { get; set; }

        [DataMember]
        public string IdService { get; set; }

        [DataMember]
        public string Service { get; set; }

        [DataMember]
        public string BCPP { get; set; }

        [DataMember]
        public string Import { get; set; }

        [DataMember]
        public string Power { get; set; }

        [DataMember]
        public string TypeCabin { get; set; }

        [DataMember]
        public string Cilindraje { get; set; }

        [DataMember]
        public string Nationality { get; set; }

        [DataMember]
        public string CapacityPayers { get; set; }

        [DataMember]
        public string CapacityCharge { get; set; }

        [DataMember]
        public string Doors { get; set; }

        [DataMember]
        public string AirConditioned { get; set; }

        [DataMember]
        public string Axes { get; set; }

        [DataMember]
        public string State { get; set; }

        [DataMember]
        public string Fuel { get; set; }

        [DataMember]
        public string Transmission { get; set; }

        [DataMember]
        public string UM { get; set; }

        [DataMember]
        public string PesoCategory { get; set; }

    }
}
