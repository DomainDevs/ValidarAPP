using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UnderwritingApplicationService.DTOs
{
    [DataContract]
    public class VehicleUseDTO
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
        public string SmallDescription { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int PrefixId { get; set; }
    }
}
