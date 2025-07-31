using System.Runtime.Serialization;

namespace Sistran.Core.Application.GeneralLedgerServices.DTOs
{
    [DataContract]
    public class AccountingAccountFilterDTO
    {
        /// <summary>
        /// User identifier
        /// </summary>
        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        /// Individual Identifier
        /// </summary>
        [DataMember]
        public int IndividualId { get; set; }

        /// <summary>
        /// Branch identifier
        /// </summary>
        [DataMember]
        public int BranchId { get; set; }

        /// <summary>
        /// Accounting concept identifier
        /// </summary>
        [DataMember]
        public int ConceptId { get; set; }

        /// <summary>
        /// Accounting concept description
        /// </summary>
        [DataMember]
        public string ConceptDescription { get; set; }

        /// <summary>
        /// Account accounting Name
        /// </summary>
        [DataMember]
        public string AccountingDescription { get; set; }

        /// <summary>
        /// Account accounting Number
        /// </summary>
        [DataMember]
        public string AccountingNumber { get; set; }
    }
}
