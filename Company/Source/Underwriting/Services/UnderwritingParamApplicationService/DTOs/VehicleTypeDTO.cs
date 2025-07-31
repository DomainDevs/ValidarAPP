using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UnderwritingParamApplicationService.DTOs
{
    [DataContract]
    public class VehicleTypeDTO
    {
        [DataMember]
        public int? Id { get; set; }

        /// <summary>
        /// Descripcion del tipo de vehiculo
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Descripcion corta del tipo de vehiculo
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }

        /// <summary>
        /// Indica si el tipo de vehiculo es un camion
        /// </summary>
        [DataMember]
        public bool IsTruck { get; set; }

        /// <summary>
        /// Indica si el tipo de vehiculo se encuentra activo
        /// </summary>
        [DataMember]
        public bool IsActive { get; set; }

        /// <summary>
        /// Estado del registro de parametrizacion
        /// </summary>
        [DataMember]
        public int State { get; set; }

        /// <summary>
        /// Carrocerias asociadas al tipo de vehiculo
        /// </summary>
        [DataMember]
        public List<VehicleBodyDTO> VehicleBodies { get; set; }
    }
}
