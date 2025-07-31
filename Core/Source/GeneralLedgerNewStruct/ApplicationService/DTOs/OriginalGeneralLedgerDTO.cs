using System.Runtime.Serialization;

namespace Sistran.Core.Application.GeneralLedgerServices.DTOs
{
    [DataContract]
    public class OriginalGeneralLedgerDTO
    {
        /// <summary>
        /// Bridge account identifier
        /// </summary>
        [DataMember]
        public int BridgeAccountingId { get; set; }
        
        /// <summary>
        /// Module identifier
        /// </summary>
        [DataMember]
        public int ModuleId { get; set; }

        /// <summary>
        /// Code package rule
        /// </summary>
        [DataMember]
        public string CodePackageRule { get; set; }
    }
}
