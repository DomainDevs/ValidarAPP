using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePersonService.V1.Models
{
    /// <summary>
    /// Modelo de las propiedades del Perfil de Asegurado.
    /// </summary>  
    [DataContract]
    public class InsuredSegmentV1
    {
       
     
   
        /// <summary>
        /// Obtiene o establece el Id del perfil de asegurado
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece la Descripción larga la  del perfil de asegurado
        /// </summary>
        [DataMember]
        public string LongDescription { get; set; }

        /// <summary>
        /// Obtiene o establece la Descripción corta del perfil de asegurado.
        /// </summary>
        [DataMember]
        public string ShortDescription { get; set; }
    }
}

