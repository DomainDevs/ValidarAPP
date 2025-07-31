using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniquePersonService.Models
{
    /// <summary>
    /// Tipo Declinacion Agente
    /// </summary>
    [DataContract]
    public class AgentDeclinedType : Extension
    {
        /// <summary>
        /// Id property attribute.
        /// </summary>
        [DataMember]
        public int? Id { get; set; }
        /// <summary>
        /// Description property attribute.
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// SmallDescription property attribute.
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }


    }
}
