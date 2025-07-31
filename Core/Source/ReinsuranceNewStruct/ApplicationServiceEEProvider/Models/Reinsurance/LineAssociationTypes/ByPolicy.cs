#region Using
using System.Runtime.Serialization;
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance;
using Sistran.Core.Application.TempCommonServices.Models;
#endregion

namespace Sistran.Core.Application.ReinsuranceServices.Models.LineAssociationTypes
{
    /// <summary>
    /// Tipo de asociación de lineas por Póliza
    /// </summary>
    [DataContract]
    public class ByPolicy : LineAssociationType
    {
        /// <summary>
        /// Póliza
        /// </summary>
        [DataMember]
        public Policy Policy { get; set; }
    }
}