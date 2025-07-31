#region Using

using System.Collections.Generic;
using System.Runtime.Serialization;

#endregion

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance
{
    /// <summary>
    /// Modelo de Líneas de Contrato
    /// </summary>
    [DataContract]
    public class Line
    {
        /// <summary>
        /// Identificador único de la línea
        /// </summary>
        [DataMember]    
        public int LineId { get; set; }

        /// <summary>
        /// Descripción de la línea
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Modelo de Tipo de Cúmulo
        /// </summary>
        [DataMember]
        public CumulusType CumulusType { get; set; }

		///// <summary>
		///// Clave de Cúmulo
		///// </summary>
		//[DataMember]
		//public string CumulusKey { get; set; }

        /// <summary>
        /// Listado de líneas de contrato
        /// </summary>
        [DataMember]
        public List<ContractLine> ContractLines { get; set; }
    }
}