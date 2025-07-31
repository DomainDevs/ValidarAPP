using Sistran.Core.Application.CommonService.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.CommonService.Models
{
    [DataContract]
    public class BankBranch : BaseBankBranch
    {
        /// <summary>
        /// Banco.
        /// </summary>
        [DataMember]
        public Bank Bank { get; set; }

    }
}
