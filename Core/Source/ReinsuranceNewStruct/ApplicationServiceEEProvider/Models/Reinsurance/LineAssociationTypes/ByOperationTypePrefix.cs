#region Using

using System.Runtime.Serialization;
using System.Collections.Generic;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance;

#endregion

namespace Sistran.Core.Application.ReinsuranceServices.Models.LineAssociationTypes
{
    /// <summary>
    /// Tipo de asociación de lineas por Tipo de Operacion -->  Ramo 
    /// </summary>
    [DataContract]
    public class ByOperationTypePrefix : LineAssociationType
    {
        
        /// <summary>
        /// Tipos de Operacion
        /// </summary>
        [DataMember]        
        public int BusinessType { get; set; } 

        /// <summary>
        /// Ramos
        /// </summary>
        [DataMember]
        public List<Prefix> Prefixes { get; set; }
      
    }
}