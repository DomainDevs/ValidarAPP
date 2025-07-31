#region Using
using System.Collections.Generic;
using System.Runtime.Serialization;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance;
using Sistran.Core.Application.TempCommonServices.Models;
#endregion

namespace Sistran.Core.Application.ReinsuranceServices.Models.LineAssociationTypes
{
    /// <summary>
    /// Tipo de asociación de lineas por Ramo Tecnico -> Subramo Tecnico -> Cobertura
    /// </summary>
    [DataContract]
    public class ByLineBusinessSubLineBusinessCoverage : LineAssociationType
    {
        /// <summary>
        /// Ramo
        /// </summary>
        [DataMember]
        public LineBusiness LineBusiness { get; set; }

        /// <summary>
        /// SubRamo
        /// </summary>
        [DataMember]
        public SubLineBusiness SubLineBusiness { get; set; }

        /// <summary>
        /// Cobertura
        /// </summary>
        [DataMember]
        public List<Coverage> Coverage { get; set; }
    }
}