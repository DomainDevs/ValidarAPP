using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.CommonService.Models.Base
{
    [DataContract]
    public class BasePaymentConcept : Extension
    {
        /// <summary>
        /// Identificador del tipo de negocio
        /// </summary>
        [DataMember]
        public int Id { get; set; }
        /// <summary>
        /// Forma de pago
        /// </summary>
        [DataMember]
        public string Description { get; set; }

    }
}
