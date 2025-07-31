using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.OperationQuotaServices.EEProvider.Models.OperationQuota
{
    [DataContract]
    public class ProductCurrency
    {
        [DataMember]
        public int DecimalQuantity { get; set; }

        [DataMember]
        public int ProductId { get; set; }

        [DataMember]
        public int Id { get; set; }

    }
}
