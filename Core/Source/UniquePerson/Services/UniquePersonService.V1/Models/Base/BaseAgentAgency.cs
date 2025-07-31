using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePersonService.V1.Models.Base
{
    /// <summary>
    /// Filtros Agencia
    /// </summary>
    [DataContract]
    public class BaseAgentAgency
    {
        /// <summary>
        /// Indivual Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Identificador agencia
        /// </summary>
        [DataMember]
        public int AgencyId { get; set; }

        /// <summary>
        /// Codigo Agente
        /// </summary>
        [DataMember]
        public int LockerId { get; set; }
    }
}
