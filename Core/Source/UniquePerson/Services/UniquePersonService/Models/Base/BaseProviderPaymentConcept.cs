using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniquePersonService.Models.Base
{
    [DataContract]
    public class BaseProviderPaymentConcept : Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Identificador Proveedor
        /// </summary>
        [DataMember]
        public int ProviderId { get; set; }

    }
}
