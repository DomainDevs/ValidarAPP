using System.Runtime.Serialization;

namespace Sistran.Core.Application.ModelServices.Models.UnderwritingParam
{
    /// <summary>
    /// Modelo de servicio de deducible
    /// </summary>
    [DataContract]
    public class DeductibleServiceQueryModel
    {
        /// <summary>
        /// Obtiene o establece el id del deducible
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece la descripcion del deducible
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece si el deducible es obligatorio
        /// </summary>
        [DataMember]
        public bool IsMandatory { get; set; }
    }
}
