#region Using
using System.Runtime.Serialization;
using Sistran.Core.Application.CommonService.Models;
using System.Collections.Generic;
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance;
#endregion

namespace Sistran.Core.Application.ReinsuranceServices.Models.LineAssociationTypes
{
    /// <summary>
    /// Tipo de asociación de lineas por Ramo -> Riesgo
    /// </summary>
    [DataContract]
    public class ByLineBusinessInsuredObject : LineAssociationType
    {
        /// <summary>
        /// Ramo Tecnico
        /// </summary>
        [DataMember]
        public LineBusiness LineBusiness { get; set; }

        /// <summary>
        /// Objeto de Seguro
        /// </summary>
        [DataMember]
        public List<InsuredObject> InsuredObject { get; set; }
    }
}