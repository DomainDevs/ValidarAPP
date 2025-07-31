#region Using

using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance;
using System.Collections.Generic;
using System.Runtime.Serialization;

#endregion

namespace Sistran.Core.Application.ReinsuranceServices.Models.LineAssociationTypes
{
    /// <summary>
    /// Tipo de asociación de lineas: por Asegurado Ramo
    /// </summary>
    [DataContract]
    public class ByInsuredPrefix : LineAssociationType
    {

        /// <summary>
        /// Asegurado
        /// </summary>
        [DataMember]
        public Individual Insured { get; set; }


        /// <summary>
        /// Ramos 
        /// </summary>
        [DataMember]
        public List<Prefix> Prefixes { get; set; }
    }
}