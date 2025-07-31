using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance
{
    [DataContract]
    public class LineAssociationType
    {
        /// <summary>
        ///  Id
        /// </summary>
        [DataMember]
        public int LineAssociationTypeId { get; set; }

        /// <summary>
        ///  Descripción
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        ///  Prioridad
        /// </summary>
        [DataMember]
        public int Priority { get; set; }

        /// <summary>
        /// Habilitada?
        /// </summary>
        [DataMember]
        public bool Enabled { get; set; }
    }
}
