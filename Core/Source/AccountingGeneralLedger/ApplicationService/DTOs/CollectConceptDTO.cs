using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingGeneralLedgerServices.DTOs
{
    [DataContract]
    public class CollectConceptDTO
    {
        /// <summary>
        /// Id 
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Description 
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public string Description { get; set; }
    }
}
