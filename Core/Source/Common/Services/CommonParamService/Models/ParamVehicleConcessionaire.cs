using Sistran.Core.Application.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.CommonParamService.Models
{
    [DataContract]
    public class ParamVehicleConcessionaire
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
