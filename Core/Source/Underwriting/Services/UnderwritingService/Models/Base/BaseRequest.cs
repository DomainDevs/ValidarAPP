using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UnderwritingServices.Models.Base
{
    /// <summary>
    /// Solictud Agrupadora
    /// </summary>
    [DataContract]
    public class BaseRequest : Extension
    {
        /// <summary>
        /// Identificador de la solictud agrupadora
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Id Grupo de facturacion
        /// </summary>
        [DataMember]
        public int BillingGroupId { get; set; }

        /// <summary>
        /// Id Grupo de facturacion
        /// </summary>
        [DataMember]
        public string Descripcion { get; set; }
    }
}
