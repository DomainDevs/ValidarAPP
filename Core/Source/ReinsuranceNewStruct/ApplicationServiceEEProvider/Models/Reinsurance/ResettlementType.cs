using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance
{
    /// <summary>
    /// ResettlementType
    /// </summary>
    [DataContract]
    public class ResettlementType
    {
        /// <summary>
        /// Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Descripci�n del tipo de contrato
        /// </summary>
        [DataMember]
        public string Description { get; set; }
        
    }
}