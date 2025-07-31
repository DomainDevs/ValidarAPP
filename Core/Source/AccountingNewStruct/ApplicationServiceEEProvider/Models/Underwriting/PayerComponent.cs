using Sistran.Core.Application.AccountingServices.EEProvider.Models.Underwriting.Base;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Underwriting
{
    [DataContract]
    public class PayerComponent : BasePayerComponent
    {
        /// <summary>
        /// Componente
        /// </summary>
        [DataMember]
        public Component Component { get; set; }
        
        /// <summary>
        /// Cobertura
        /// </summary>
        [DataMember]
        public Coverage Coverage { get; set; }
    }
}
