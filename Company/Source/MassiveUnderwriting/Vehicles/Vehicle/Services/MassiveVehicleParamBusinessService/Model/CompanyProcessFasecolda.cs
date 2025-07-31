using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Sistran.Company.Application.Vehicles.MassiveVehicleParamApplicationService.Enum;

namespace Sistran.Company.Application.MassiveVehicleParamBusinessService.Model
{
    [DataContract]
    public class CompanyProcessFasecolda : CompanyProcessFasecoldaMassiveLoad
    {
        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public DateTime BeginDate { get; set; }

        [DataMember]
        public DateTime? EndDate { get; set; }

        [DataMember]
        public bool Status { get; set; }

        [DataMember]
        public bool HasError { get; set; }

        [DataMember]
        public int IssuanceStatus { get; set; }

        [DataMember]
        public bool Active { get; set; }
        
        [DataMember]
        public string Error_Description { get; set; }

        [DataMember]
        public List<CompanyProcessFasecoldaMassiveLoad> ProcessMassiveLoad { get; set; }

        [DataMember]
        public VehicleFasecoldaStatusEnum ProcessStatus { get; set; }

        [DataMember]
        public CompanyVehicleFasecoldaStatusType ProcessStatusType { get; set; }

    }
}
