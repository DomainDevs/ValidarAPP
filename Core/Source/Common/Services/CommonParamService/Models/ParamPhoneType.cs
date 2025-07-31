namespace Sistran.Core.Application.CommonParamService.Models
{
    using System.Runtime.Serialization;
    /// <summary>
    /// Tipos de Telefono
    /// </summary>
    [DataContract]
    public class ParamPhoneType
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Tipo de teléfono
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Abreviatura
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }
    }
}
