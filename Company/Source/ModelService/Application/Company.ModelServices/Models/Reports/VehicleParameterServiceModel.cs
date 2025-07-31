

using Sistran.Company.Application.ModelServices.Enums;
using Sistran.Company.Application.ModelServices.Models.Param;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.ModelServices.Models.Reports
{
    [DataContract]
    public class VehicleParameterServiceModel: ErrorServiceModel
    {
        [DataMember]
        public int VehicleMakeCode { get; set; }
        [DataMember]
        public int VehicleModelCode { get; set; }
        [DataMember]
        public int VehicleVersionCode { get; set; }
        [DataMember]
        public int VehicleTypeCode { get; set; }
        [DataMember]
        public string MakeDescription { get; set; }
        [DataMember]
        public string ModelDescription { get; set; }
        [DataMember]
        public string VersionDescription { get; set; }
        [DataMember]
        public string VehicleTypeDescription { get; set; }
        [DataMember]
        public int VehicleYear { get; set; }
        [DataMember]
        public decimal VehiclePrice { get; set; }
        [DataMember]
        public string FasecoldaMakeId { get; set; }
        [DataMember]
        public string FasecoldaModelId { get; set; }
        [DataMember]
        public StatusTypeService StatusTypeService { get; set; }
    }
}
