using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.ReinsuranceIntegrationServices.DTOs.Reinsurance
{
    /// <summary>
    /// Distribucion de Seguros
    /// </summary>
    [DataContract]
    public class ReinsuranceAllocationDTO
    {
        /// <summary>
        /// Id
        /// </summary>
        [DataMember]
        public int ReinsuranceAllocationId { get; set; }

        /// <summary>
        /// Currency
        /// </summary>
        [DataMember]
        public CurrencyDTO Currency { get; set; }

        /// <summary>
        /// Facultative
        /// </summary>
        [DataMember]
        public bool Facultative { get; set; }

        /// <summary>
        /// Contract
        /// </summary>
        [DataMember]
        public ContractDTO Contract { get; set; }


        /// <summary>
        /// Amount
        /// </summary>
        [DataMember]
        public AmountDTO Amount { get; set; }

        /// <summary>
        /// Premium
        /// </summary>
        [DataMember]
        public AmountDTO Premium { get; set; }

        /// <summary>
        /// Commission
        /// </summary>
        [DataMember]
        public AmountDTO Commission { get; set; }

        /// <summary>
        /// MovementSourceId: Movimiento Origen 
        /// </summary>
        [DataMember]
        public int MovementSourceId { get; set; }

        /// <summary>
        /// Required in GetTotSumPrimeAllocation
        /// </summary>
        [DataMember]
        public decimal TotalSum { get; set; }
        /// <summary>
        /// Required in GetTotSumPrimeAllocation
        /// </summary>
        [DataMember]
        public decimal TotalPremium { get; set; }

        [DataMember]
        public string Sum { get; set; }

        [DataMember]
        public string PremiumAllocation { get; set; }
    }
}
