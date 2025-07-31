using System.Runtime.Serialization;
namespace Sistran.Company.Application.UniquePersonServices.V1.Models
{
    /// <summary>
    /// Informacion Social
    /// </summary>
    [DataContract]
    public class CompanySupplierAccountingConcept : Sistran.Core.Application.UniquePersonService.V1.Models.Base.BaseSupplierAccountingConcept
    {
        /// <summary>
        /// Supplier
        /// </summary>
        [DataMember]
        public CompanySupplier Supplier { get; set; }

        /// <summary>
        /// AccountingConcept
        /// </summary>
        [DataMember]
        public CompanyAccountingConcept AccountingConcept { get; set; }


    }
}
