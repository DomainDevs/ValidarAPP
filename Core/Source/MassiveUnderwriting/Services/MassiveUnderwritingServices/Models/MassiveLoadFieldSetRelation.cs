using System.Runtime.Serialization;

namespace Sistran.Core.Application.MassiveUnderwritingServices.Models
{
    [DataContract]
    public class MassiveLoadFieldSetRelation
    {
        /// <summary>
        /// Obtiene o establece el Id del campo.
        /// </summary>
        [DataMember]
        public int FieldSetId { get; set; }

        /// <summary>
        ///Obtiene o establece el tipo de endo cd.
        /// </summary>
        [DataMember]
        public int EndoTypeCd { get; set; }

        /// <summary>
        ///Obtiene o establece el Id de relación.
        /// </summary>
        [DataMember]
        public int RelationId { get; set; }
    }
}
