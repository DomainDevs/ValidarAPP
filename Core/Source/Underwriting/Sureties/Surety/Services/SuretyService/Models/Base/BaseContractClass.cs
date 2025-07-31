using System.Runtime.Serialization;

namespace Sistran.Core.Application.Sureties.SuretyServices.Models.Base
{
    /// <summary>
    /// Base Clase Contrato
    /// </summary>
    [DataContract]
    public class BaseContractClass
    {
        /// <summary>
        /// identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Descripcion 
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}
