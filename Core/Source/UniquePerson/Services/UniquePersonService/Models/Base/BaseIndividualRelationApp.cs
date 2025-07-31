using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniquePersonService.Models.Base
{
    [DataContract]
    public class BaseIndividualRelationApp : Extension
    {
        /// <summary>
        /// Obtiene o Setea el IndividualRelationAppId
        /// </summary>
        [DataMember]
        public int IndividualRelationAppId { get; set; }

        /// <summary>
        /// Obtiene o Setea el ParentIndividualId
        /// </summary>
        [DataMember]
        public int ParentIndividualId { get; set; }

        /// <summary>
        /// Obtiene o Setea el RelationTypeCd
        /// </summary>
        [DataMember]
        public int RelationTypeCd { get; set; }

    }
}
