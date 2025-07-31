using Sistran.Core.Application.CommonService.Models.Base;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.CommonService.Models
{
    /// <summary>
    /// Sucursales
    /// </summary>
    [DataContract]
    public class Branch : BaseBranch
    {
        /// <summary>
        /// Puntos de venta de la sucursal
        /// </summary>
        [DataMember]
        public List<SalePoint> SalePoints { get; set; }

        [DataMember]
        public int? GroupBranchId { get; set; }
    }
}
