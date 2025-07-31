using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UnderwritingServices.Models.Base
{
    /// <summary>
    /// Grupo de Facturacion
    /// </summary>
    [DataContract]
    public class BaseBillingGroup : Extension
    {
        /// <summary>
        /// Identificador Grupo de Factruacion
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
