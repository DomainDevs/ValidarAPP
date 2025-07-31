namespace Sistran.Core.Application.CommonParamService.Models
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Tipos de Direccion
    /// </summary>
    [DataContract]
    public class ParamAddressType
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Tipo de dirección
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}
