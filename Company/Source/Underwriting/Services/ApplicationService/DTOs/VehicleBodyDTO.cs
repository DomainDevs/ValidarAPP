using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UnderwritingApplicationService.DTOs
{
    [DataContract]
    public class VehicleBodyDTO
    {
        /// <summary>
        /// Obtiene o establece Id de la Carrocería de vehículo
        /// </summary>
        [DataMember]
        public int? Id { get; set; }

        /// <summary>
        /// Obtiene o establece Descripcion corta de Carrocería de vehículo
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }

        /// <summary>
        /// Obtiene o establece Usos asociados ala Carrocería de vehículo
        /// </summary>
        [DataMember]
        public List<VehicleUseDTO> VehicleUses { get; set; }

        /// <summary>
        /// Obtiene o establece Estado del registro de parametrizacion
        /// </summary>
        [DataMember]
        public int State { get; set; }
    }
}
