using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.MassiveServices.Models
{
    [DataContract]
    public class MassiveLoadLog
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Descripción
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Hora de Registro
        /// </summary>
        [DataMember]
        public DateTime Time { get; set; }
    }
}