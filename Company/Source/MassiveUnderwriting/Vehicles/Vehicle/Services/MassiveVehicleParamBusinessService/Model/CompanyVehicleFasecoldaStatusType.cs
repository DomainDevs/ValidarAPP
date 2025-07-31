using System;
using System.Runtime.Serialization;
using Sistran.Company.Application.Vehicles.MassiveVehicleParamApplicationService.Enum;

namespace Sistran.Company.Application.MassiveVehicleParamBusinessService.Model
{
    [DataContract]
    public class CompanyVehicleFasecoldaStatusType
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int ProcessId { get; set; }

        [DataMember]
        public VehicleFasecoldaProcessTypeEnum ProcessType { get; set; }

        [DataMember]
        public VehicleFasecoldaProcessStatusEnum StatusType { get; set; }

        [DataMember]
        public DateTime EndDate { get; set; }
    }
}
