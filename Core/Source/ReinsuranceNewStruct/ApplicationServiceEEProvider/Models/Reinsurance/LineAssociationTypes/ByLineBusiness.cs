using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.Models.LineAssociationTypes
{
    /// <summary>
    /// Tipo de asociación de lineas: por Ramo Técnico
    /// </summary>
    [DataContract]
    public class ByLineBusiness : LineAssociationType
    {
        /// <summary>
        /// Ramo Técnico
        /// </summary>
        [DataMember]
        public List<LineBusiness> LineBusiness { get; set; }
    }
}