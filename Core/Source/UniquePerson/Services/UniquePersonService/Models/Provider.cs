using Sistran.Core.Application.UniquePersonService.Models.Base;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePersonService.Models
{
    /// <summary>
    /// Proveedor
    /// </summary>
    [DataContract]
    public class Provider : BaseProvider
    {
        /// <summary>
        /// Listado Concepto de pago
        /// </summary>
        [DataMember]
        public List<ProviderPaymentConcept> ProviderPaymentConcept { get; set; }

        /// <summary>
        /// Especialidad de Proveedor
        /// </summary>
        [DataMember]
        public List<ProviderSpeciality> ProviderSpeciality { get; set; }

    }
}
