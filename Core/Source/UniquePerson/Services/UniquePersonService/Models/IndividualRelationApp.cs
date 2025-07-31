using Sistran.Core.Application.UniquePersonService.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePersonService.Models
{
    /// <summary>
    /// IndividualRelationApp
    /// </summary>
    [DataContract]
    public class IndividualRelationApp : BaseIndividualRelationApp
    {
        /// <summary>
        /// Obtiene o Setea el ChildIndividualId
        /// </summary>
        [DataMember]
        public Agent ChildIndividual { get; set; }

        /// <summary>
        /// Obtiene o Setea el AgentAgencyId
        /// </summary>
        [DataMember]
        public Agency Agency { get; set; }

    }
}
