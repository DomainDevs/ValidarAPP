using System.Collections.Generic;
using System.Runtime.Serialization;


namespace Sistran.Company.Application.Vehicles.MassiveVehicleParamApplicationService.DTOs
{
    [DataContract]
    public class MassiveVehicleFasecoldaRowDTO
    {
        [DataMember]
        public int ProcessId { get; set; }

        [DataMember]
        public int RowNumber { get; set; }

        [DataMember]
        public bool HasError { get; set; }

        [DataMember]
        public List<string> ListError {get; set;}

        [DataMember]
        public string SerializedRow { get; set; }
    }
}
