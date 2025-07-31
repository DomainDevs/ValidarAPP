using Sistran.Company.Application.ModelServices.Models.VehicleParam;
using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.CommonParamService.Models
{
    [DataContract]
    public class VehicleConcessionaireServiceModel
    {

        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// vehicleMake
        /// </summary>
        [DataMember]
        public VehicleMake vehicleMake { get; set; }

        /// <summary>
        /// IsEnabled
        /// </summary>
        [DataMember]
        public Boolean IsEnabled { get; set; }
    }
}
