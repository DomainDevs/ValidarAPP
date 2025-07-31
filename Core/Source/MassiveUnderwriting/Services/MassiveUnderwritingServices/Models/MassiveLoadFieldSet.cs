using System.Runtime.Serialization;

namespace Sistran.Core.Application.MassiveUnderwritingServices.Models
{
    [DataContract]
    public class MassiveLoadFieldSet
    {
        /// <summary>
        /// Obtiene o establece el valor del campo id.
        /// </summary>
        [DataMember]
        public int FieldSetId { get; set; }

        /// <summary>
        /// Obtiene o establece la descripción.
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Obtiene o establece la columna de inicio.
        /// </summary>
        [DataMember]
        public string BeginColumn { get; set; }

        /// <summary>
        /// Obtiene o establece la columna final.
        /// </summary>
        [DataMember]
        public string EndColumn { get; set; }

        /// <summary>
        /// Obtiene o establece el conteo de columnas.
        /// </summary>
        [DataMember]
        public int CountColumn { get; set; }

        /// <summary>
        /// Obtiene o establece si esta habilidado.
        /// </summary>
        [DataMember]
        public bool? IsEnabled { get; set; }

        /// <summary>
        /// Obtiene o establece el codigo del prefijo.
        /// </summary>
        [DataMember]
        public int? PrefixCode { get; set; }

        /// <summary>
        /// Obtiene o establece si la instancias es requerida.
        /// </summary>
        [DataMember]
        public bool IsRequest { get; set; }

        /// <summary>
        /// Obtiene o establece si es colectiva.
        /// </summary>
        [DataMember]
        public bool IsCollective { get; set; }

        /// <summary>
        /// Obtiene o establece si es excluida.
        /// </summary>
        [DataMember]
        public bool? IsExclude { get; set; }

    }
}