#region Using
using System.Collections.Generic;
using System.Runtime.Serialization;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance;


#endregion

namespace Sistran.Core.Application.ReinsuranceServices.Models.LineAssociationTypes
{
    /// <summary>
    /// Tipo de asociación de lineas por Ramo -> Subramo -> Riesgo
    /// </summary>
    [DataContract]
    public class ByLineBusinessSubLineBusinessInsuredObject : LineAssociationType
    {
        /// <summary>
        /// Ramo Tecnico
        /// </summary>
        [DataMember]
        public LineBusiness LineBusiness { get; set; }

        /// <summary>
        /// SubRamo Tecnico
        /// </summary>
        [DataMember]
        public SubLineBusiness SubLineBusiness { get; set; }

        /// <summary>
        /// Objeto de Seguro
        /// </summary>
        [DataMember]
        public List<InsuredObject> InsuredObject { get; set; }
    }
}

