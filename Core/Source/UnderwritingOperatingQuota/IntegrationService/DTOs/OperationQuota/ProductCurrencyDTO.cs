using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.UnderwritingOperatingQuotaServices.DTOs.OperationQuota
{
    [DataContract]
    public class ProductCurrencyDTO
    {
        [DataMember]
        public int DecimalQuantity { get; set; }

        [DataMember]
        public int ProductId { get; set; }

        [DataMember]
        public int Id { get; set; }
    }
}
