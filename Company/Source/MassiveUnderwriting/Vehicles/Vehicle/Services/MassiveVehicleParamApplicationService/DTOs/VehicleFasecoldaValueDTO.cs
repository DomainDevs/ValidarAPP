using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.Vehicles.MassiveVehicleParamApplicationService.DTOs
{
    [DataContract]
    public class VehicleFasecoldaValueDTO
    {
        [DataMember]
        public string Codigo { get; set; }
        
        [DataMember]
        public string Modelo { get; set; }

        [DataMember]
        public decimal Valor { get; set; }
    }
}
