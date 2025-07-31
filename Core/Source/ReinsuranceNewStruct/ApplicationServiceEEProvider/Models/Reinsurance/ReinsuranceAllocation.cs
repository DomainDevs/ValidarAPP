using Sistran.Core.Application.CommonService.Models;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance
{
    /// <summary>
    /// Distribucion de Seguros
    /// </summary>
     [DataContract]
    public class ReinsuranceAllocation
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
        public Currency Currency { get; set; }

        /// <summary>
        /// Facultative
        /// </summary>
        [DataMember]
        public bool Facultative { get; set; } 

        /// <summary>
        /// Contract
        /// </summary>
         [DataMember]
        public Contract Contract { get; set; }

         
         /// <summary>
         /// Amount
         /// </summary>
         [DataMember]
         public Amount Amount { get; set; }

        /// <summary>
        /// Premium
        /// </summary>
        [DataMember]
        public  Amount Premium{ get; set; }

        /// <summary>
        /// Commission
        /// </summary>
        [DataMember]
        public Amount Commission { get; set; }

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
