using System.Runtime.Serialization;

namespace Sistran.Core.Application.MassiveUnderwritingServices.Models
{
    [DataContract]
    public class MassiveReportAgents
    {
        /// <summary>
        /// Obtiene o establece el ID de riesgo
        /// </summary>
        [DataMember]
        public int RiskId { get; set; }

        /// <summary>
        /// Codigo
        /// </summary>
        [DataMember]
        public string AgentCode { get; set; }

        /// <summary>
        /// Participación del intermediario
        /// </summary>
        [DataMember]
        public string AgentParticipation { get; set; }
    }
}
