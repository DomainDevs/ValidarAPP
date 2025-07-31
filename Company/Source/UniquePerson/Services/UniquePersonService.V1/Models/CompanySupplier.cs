using Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using System.Collections.Generic;

namespace Sistran.Company.Application.UniquePersonServices.V1.Models
{
    public class CompanySupplier : BaseSupplier
    {

        /// <summary>
        /// Especialidadad de Proveedor
        /// </summary>        
        public CompanySupplierProfile Profile { get; set; }

        /// <summary>
        /// Tipo de Origen
        /// </summary>        
        public CompanySupplierType Type { get; set; }

        /// <summary>
        /// PaymentAccountType
        /// </summary>        
        public CompanyPaymentAccountType PaymentAccountType { get; set; }

        /// <summary>
        /// DeclinedType
        /// </summary>        
        public CompanySupplierDeclinedType DeclinedType { get; set; }

        /// <summary>
        /// DeclinedType
        /// </summary>        
        public List <CompanyAccountingConcept> AccountingConcepts { get; set; }

        /// <summary>
        /// GroupSuppliers
        /// </summary>        
        public List<CompanyGroupSupplier> GroupSupplier { get; set; }
    }
}
