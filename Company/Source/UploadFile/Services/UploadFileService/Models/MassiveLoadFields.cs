using System.Runtime.Serialization;

namespace Sistran.Company.Application.UploadFileServices.Models
{
    [DataContract]
    public class MassiveLoadFields
    {
        /// <summary>
        /// Obtiene o establece el campo ID del cargue.
        /// </summary>
        [DataMember]
        public int MassiveFieldId { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre de campo cargue.
        /// </summary>
        [DataMember]
        public string MassiveFieldName { get; set; }

        /// <summary>
        /// Obtiene o establece la descripcion del cargue.
        /// </summary>
        [DataMember]
        public string MassiveFieldDescription { get; set; }

        /// <summary>
        /// Obtiene o establece el campo largo.
        /// </summary>
        [DataMember]
        public int? FieldLong { get; set; }

    }
}