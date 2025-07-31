using System.Collections.Generic;
using System.Runtime.Serialization;
using Sistran.Company.Application.Vehicles.MassiveVehicleParamApplicationService.Enum;

namespace Sistran.Company.Application.Vehicles.MassiveVehicleParamApplicationService.DTOs
{
    [DataContract]
    public class ProcessFasecoldaMassiveLoadDTO
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int ProcessId { get; set; }

        [DataMember]
        public int Pendings { get; set; }

        [DataMember]
        public int Processeds { get; set; }

        [DataMember]
        public int WithErrorsProcesseds { get; set; }

        [DataMember]
        public int TotalRowsProcesseds { get; set; }

        [DataMember]
        public int TotalRowsLoaded { get; set; }

        [DataMember]
        public int TotalRows { get; set; }
        
        [DataMember]
        public int? Status { get; set; }

        [DataMember]
        public int EnableProcessing { get; set; }

        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public FileTypeFasecoldaEnum TypeFile { get; set; }

        [DataMember]
        public List<MassiveVehicleFasecoldaRowDTO> Row { get; set; }
    }
}

