using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniquePersonService.V1.Models.Base
{
    [DataContract]
    public class BasePaymentAccount : Extension
    {

        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Numero de cuenta
        /// </summary>
        [DataMember]
        public decimal Number { get; set; }
    }
}
