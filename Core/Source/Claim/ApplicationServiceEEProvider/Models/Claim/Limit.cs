using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim
{
    [DataContract]
    public class Limit
    {
        /// <summary>
        /// InsuredAmount
        /// </summary>
        [DataMember]
        public decimal InsuredAmount { get; set; }
        /// <summary>
        /// Payment
        /// </summary>
        [DataMember]
        public decimal Payment { get; set; }
        /// <summary>
        /// Consumtion
        /// </summary>
        [DataMember]
        public decimal Consumption { get; set; }
        /// <summary>
        /// Reserve
        /// </summary>
        [DataMember]
        public decimal Reserve { get; set; }
        /// <summary>
        /// PaymentConsumtion
        /// </summary>
        [DataMember]
        public decimal PaymentConsumption { get; set; }
    }
}
