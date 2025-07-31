using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniqueUserServices.Models.Base
{
    [DataContract]
    public class BaseIndividualRelationApp : Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Id de la persona asociada al usuario
        /// </summary>
        [DataMember]
        public int IndividualId { get; set; }

        /// <summary>
        /// Tipo de relacion
        /// </summary>
        [DataMember]
        public int RelationTypeId { get; set; }
    }
}