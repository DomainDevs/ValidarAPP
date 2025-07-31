using Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.SarlaftBusinessServices.Models
{

    public class CompanyCompany : BaseCompany 
    {
        [DataMember]
        /// <summary>
        /// EconomicActivity
        /// </summary>
        public CompanyEconomicActivity EconomicActivity { get; set; }

        [DataMember]
        /// <summary>
        /// IdentificationDocumentNumber
        /// </summary>
        public int DocumentType { get; set; }

        [DataMember]
        /// <summary>
        /// IdentificationDocumentType
        /// </summary>
        public string DocumentNumber { get; set; }

        [DataMember]
        /// <summary>
        /// BranchId
        /// </summary>
        public int BranchId { get; set; }
        [DataMember]
        /// <summary>
        /// PersonType
        /// </summary>
        public int PersonType { get; set; }

        [DataMember]
        /// <summary>
        /// AssociationType
        /// </summary>
        public int AssociationType { get; set; }
    }
}
