using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.ModelServices.Models.VehicleParam
{
    public class VehicleMake
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
        /// IsEnabled
        /// </summary>
        [DataMember]
        public Boolean IsFrequently { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int? AtVehicleModelCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int? IaVehicleMakeCode { get; set; }
    }
}
