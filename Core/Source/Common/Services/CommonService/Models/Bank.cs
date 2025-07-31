using Sistran.Core.Application.CommonService.Models.Base;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.CommonService.Models
{
    [DataContract]
    public class Bank : BaseBank
    {
        /// <summary>
        /// Listado de sucursales
        /// </summary>
        [DataMember]
        public List<BankBranch> BankBranches { get; set; }
    }
}
