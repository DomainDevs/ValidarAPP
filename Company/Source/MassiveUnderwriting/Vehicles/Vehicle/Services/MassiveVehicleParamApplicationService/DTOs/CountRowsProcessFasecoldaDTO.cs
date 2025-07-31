using System.Runtime.Serialization;

namespace Sistran.Company.Application.Vehicles.MassiveVehicleParamApplicationService.DTOs
{
    [DataContract]
    public class CountRowsProcessFasecoldaDTO
    {

        [DataMember]
        public int ProcessId { get; set; }

        [DataMember]
        public int Pendings { get; set; }

        [DataMember]
        public int Processeds { get; set; }

        [DataMember]
        public int WithErrorsProcesseds { get; set; }

        [DataMember]
        public int TotalRowsProcessed { get; set; }

        [DataMember]
        public int TotalRowsLoaded { get; set; }

        [DataMember]
        public int TotalRows { get; set; }
    }
}
