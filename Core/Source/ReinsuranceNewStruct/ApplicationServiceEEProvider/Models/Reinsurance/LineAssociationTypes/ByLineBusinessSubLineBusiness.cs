using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.Models.LineAssociationTypes
{
    /// <summary>
    /// Tipo de asociación de lineas por Ramo Tecnico -> Subramo Tecnico
    /// </summary>
    [DataContract]
    public class ByLineBusinessSubLineBusiness : LineAssociationType
    {
        /// <summary>
        /// Ramo Tecnico
        /// </summary>
        [DataMember]
        public LineBusiness LineBusiness { get; set; }

        /// <summary>
        /// SubRamos Tecmicos
        /// </summary>
        [DataMember]
        public List<SubLineBusiness> SubLineBusiness { get; set; }
    }
}