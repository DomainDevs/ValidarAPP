using Sistran.Core.Application.ModelServices.Models.Param;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ModelServices.Models.Audit
{
    /// <summary>
    /// Cambios Log Auditoria
    /// </summary>
    /// <seealso cref="Sistran.Core.Application.ModelServices.Models.Param.ParametricServiceModel" />
    [DataContract]
    public class AuditChangeServiceModel : ParametricServiceModel
    {
        /// <summary>
        /// Obtiene o setea Nombre de la propiedad Modificada
        /// </summary>
        /// <value>
        ///  Nombre de la propiedad Modificada
        /// </value>
        [DataMember]
        public string Id { get; set; }

        /// <summary>
        ///Obtiene o setea el Valor Anterior del Objeto
        /// </summary>
        /// <value>
        ///Valor Anterior
        /// </value>
        [DataMember]
        public string ValueBefore { get; set; }

        /// <summary>
        /// Obtiene o setea el Valor Nuevo del Objeto
        /// </summary>
        /// <value>
        /// Valor Nuevo
        /// </value>
        [DataMember]
        public string ValueAfter { get; set; }
        /// <summary>
        ///  Obtiene o establece un valor que indica si esta instancia se serializa
        /// </summary>
        /// <value>
        ///true si esta instancia es serializada; de lo contrario, falso.
        /// </value>
        [DataMember]
        public bool IsSerialize { get; set; }

    }
}
