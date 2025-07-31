using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance
{
    [DataContract]
    public class LineContractLimit
    {
        /// <summary>
        /// Identificador de la línea
        /// </summary>
        [DataMember]
        public int LineId { get; set; }

        /// <summary>
        /// Límite del contrato
        /// </summary>
        [DataMember]
        public decimal contractLimit { get; set; }
    }
}
