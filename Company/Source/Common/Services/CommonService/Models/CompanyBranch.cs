using Sistran.Core.Application.CommonService.Models.Base;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.CommonServices.Models
{
    /// <summary>
    /// Sucursales
    /// </summary>
    /// <seealso cref="Sistran.Core.Application.CommonService.Models.Base.BaseLineBusiness" />
    [DataContract]
    public class CompanyBranch : BaseBranch
    {
        [DataMember]
        public List<CompanySalesPoint> SalePoints { get; set; }

        [DataMember]
        public int? GroupBranchId { get; set; }
    }
}
