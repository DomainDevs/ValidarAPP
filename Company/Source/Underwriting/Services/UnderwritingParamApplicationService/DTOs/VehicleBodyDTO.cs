using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UnderwritingParamApplicationService.DTOs
{
    [DataContract]
    public class VehicleBodyDTO
    {

        /// <summary>
        /// Obtiene o establece Id 
        /// </summary>
        [DataMember]
        public int? Id { get; set; }

        /// <summary>
        /// Obtiene o establece Descripcion corta
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Estado del registro de parametrizacion
        /// </summary>
        [DataMember]
        public int State { get; set; }

        //[DataMember]
        //ErrorDTO error { get; set; }
    }
}
