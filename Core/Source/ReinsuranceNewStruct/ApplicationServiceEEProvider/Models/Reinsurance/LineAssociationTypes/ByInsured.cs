#region Using
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance;
using System.Runtime.Serialization;
#endregion

namespace Sistran.Core.Application.ReinsuranceServices.Models.LineAssociationTypes
{
    /// <summary>
    /// Tipo de asociación de lineas: por Asegurado
    /// </summary>
    [DataContract]
    public class ByInsured : LineAssociationType
    {

        /// <summary>
        /// Asegurado
        /// </summary>
        [DataMember]
        public Individual Insured { get; set; }

       
    }
}