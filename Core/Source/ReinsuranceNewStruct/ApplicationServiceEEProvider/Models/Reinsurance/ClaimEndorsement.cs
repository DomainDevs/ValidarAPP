using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance
{
    [DataContract]
    public class ClaimEndorsement
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int Number { get; set; }

        /// <summary>
        /// Identificador poliza
        /// </summary>
        [DataMember]
        public int PolicyId { get; set; }

        /// <summary>
        /// Numero Poliza
        /// </summary>
        [DataMember]
        public decimal PolicyNumber { get; set; }

        /// <summary>
        /// Riesgo
        /// </summary>
        [DataMember]
        public int RiskId { get; set; }
    }
}
