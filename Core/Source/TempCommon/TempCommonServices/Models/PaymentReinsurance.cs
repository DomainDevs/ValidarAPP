using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.GeneralLedgerServices.Models.AccountingConcepts;

namespace Sistran.Core.Application.TempCommonServices.Models
{
    /// <summary>
    /// ReinsurancePayment 
    /// </summary>
    [DataContract]
    public class PaymentReinsurance
    {
              
        
        /// <summary>
        /// Id 
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        
        /// <summary>
        /// PaymentNumber
        /// </summary>
        [DataMember]
        public int PaymentNumber { get; set; }

        /// <summary>
        /// PaymentDate
        /// </summary>
        [DataMember]
        public DateTime PaymentDate { get; set; }


        /// <summary>
        /// MovementType 
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public MovementType MovementType { get; set; }


        /// <summary>
        /// RiskNumber
        /// </summary>
        [DataMember]
        public int RiskNumber { get; set; }

        /// <summary>
        /// CoverageNumber
        /// </summary>
        [DataMember]
        public int CoverageNumber { get; set; }

        /// <summary>
        /// Currency
        /// </summary>
        [DataMember]
        public Currency Currency { get; set; }

        /// <summary>
        /// Amount
        /// </summary>
        [DataMember]
        public Amount Amount { get; set; }

        /// <summary>
        /// ClaimId
        /// </summary>
        [DataMember]
        public int ClaimId { get; set; }

    }
}
