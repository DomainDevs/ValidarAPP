using Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePersonService.V1.Models
{
    /// <summary>
    /// proveedor
    /// </summary>
    [DataContract]
    public class Supplier : BaseSupplier
    {

        /// <summary>
        /// Especialidadad de Proveedor
        /// </summary>
        [DataMember]
        public SupplierProfile Profile { get; set; }

        /// <summary>
        /// Tipo de Origen
        /// </summary>
        [DataMember]
        public SupplierType Type { get; set; }

        /// <summary>
        /// PaymentAccountType
        /// </summary>
        [DataMember]
        public PaymentAccountType PaymentAccountType { get; set; }

        /// <summary>
        /// DeclinedType
        /// </summary>
        [DataMember]
        public SupplierDeclinedType DeclinedType { get; set; }


        ///// <summary>
        ///// Listado Concepto de pago
        ///// </summary>
        //[DataMember]
        public List<AccountingConcept> AccountingConcepts { get; set; }

        /// <summary>
        /// GroupSupplier
        /// </summary>
        [DataMember]
        public List<GroupSupplier> GroupSupplier { get; set; }
    }
}
