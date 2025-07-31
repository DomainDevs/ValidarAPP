using System.Runtime.Serialization;

namespace Sistran.Core.Application.MassiveUnderwritingServices.Models
{
    [DataContract]
    public class MassiveProcessState
    {
        /// <summary>
        /// Obtiene o establece el identificador del estado.
        /// </summary>
        [DataMember]
        public int StateId { get; set; }
        /// <summary>
        /// Obtiene o establece la descripción.
        /// </summary>
        [DataMember]
        public string Description { get; set; }
        /// <summary>
        /// Obtiene o establece el valor que indica si la instancia es visible.
        /// </summary>
        [DataMember]
        public bool IsVisible { get; set; }
    }
}
