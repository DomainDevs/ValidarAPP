using Sistran.Company.Application.Vehicles.MassiveVehicleParamApplicationService.Enum;
using Sistran.Core.Application.UniqueUserServices.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.Vehicles.MassiveVehicleParamApplicationService.DTOs
{
    [DataContract]
    public class ProcessFasecoldaDTO
    {
        [DataMember]
        public int ProcessId { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public DateTime BeginDate { get; set; }

        [DataMember]
        public DateTime EndDate { get; set; }

        [DataMember]
        public User User { get; set; }

        [DataMember]
        public bool Status { get; set; }

        [DataMember]
        public bool HasError { get; set; }

        [DataMember]
        public string ErrorDescription { get; set; }

        [DataMember]
        public int IssuanceStatus { get; set; }

        [DataMember]
        public bool Active { get; set; }

        [DataMember]
        public List<ProcessFasecoldaMassiveLoadDTO> ProcessMassiveLoad { get; set; }

        [DataMember]
        public VehicleFasecoldaStatusEnum ProcessStatus { get; set; }

        [DataMember]
        public VehicleFasecoldaStatusTypeDTO ProcessStatusType { get; set; }

        /*
        [DataMember]
        public List<string>  ListError { get; set; }

        [DataMember]
        public CountRowsProcessFasecoldaDTO CountRows { get; set; }



        [DataMember]
        public List<VehicleFasecoldaStatusTypeDTO> ProcessStatusType { get; set; }

        [DataMember]
        public List<MassiveVehicleFasecoldaRowDTO> Rows { get; set; }
        */
    }
}
