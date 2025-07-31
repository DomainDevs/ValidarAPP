using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ModelServices.Models.Common
{
    using Sistran.Core.Application.ModelServices.Models.Param;
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Parametros de Dias de Discontinuidad
    /// </summary>
    public class DiscontinuityLogServiceModel
    {
        /// <summary>
        /// Obtiene o establece Id del log
        /// </summary>
        [DataMember]
        public long daysDiscontinuityLogId { get; set; }

        /// <summary>
        /// Obtiene o establece Dias de Discontinuidad
        /// </summary>
        [DataMember]
        public long daysDiscontinuity { get; set; }

        /// <summary>
        /// Obtiene o establece Fecha de registro
        /// </summary>
        [DataMember]
        public DateTime registrationDate { get; set; }

        /// <summary>
        /// Obtiene o establece id del usuario
        /// </summary>
        [DataMember]
        public int userId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public ParametricServiceModel ParametricServiceModel { get; set; }
    }
}
