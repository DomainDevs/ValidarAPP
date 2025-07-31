using Sistran.Core.Application.MassiveServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.MassiveServices.Models
{
    [DataContract]
    public class LoadType
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
        /// Abreviatura
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }

        /// <summary>
        /// Id del Tipo de Proceso
        /// </summary>
        [DataMember]
        public MassiveProcessType? ProcessType { get; set; }
        
        /// <summary>
        /// Descripción del Tipo de Proceso
        /// </summary>
        [DataMember]
        public string ProcessTypeDescription { get; set; }
    }
}