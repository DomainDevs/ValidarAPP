using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Sistran.Core.Application.Extensions;
namespace Sistran.Core.Application.UniquePerson.IntegrationService.Models.Base
{
    [DataContract]
    public class BaseIntegrationEconomicGroup : Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int EconomicGroupId { get; set; }

        /// <summary>
        /// EconomicGroupName
        /// </summary>
        [DataMember]
        public string EconomicGroupName { get; set; }

        /// <summary>
        /// TributaryIdType
        /// </summary>
        [DataMember]
        public int TributaryIdType { get; set; }

        /// <summary>
        /// TributaryIdNo
        /// </summary>
        [DataMember]
        public string TributaryIdNo { get; set; }

        /// <summary>
        /// VerifyDigit
        /// </summary>
        [DataMember]
        public int VerifyDigit { get; set; }


        /// <summary>
        /// EnteredDate
        /// </summary>
        [DataMember]
        public DateTime EnteredDate { get; set; }

        /// <summary>
        /// OperationQuoteAmount
        /// </summary>
        [DataMember]
        public decimal OperationQuoteAmount { get; set; }

        /// <summary>
        /// Enabled
        /// </summary>
        [DataMember]
        public bool Enabled { get; set; }
        /// <summary>
        /// DeclinedDate
        /// </summary>
        [DataMember]
        public DateTime? DeclinedDate { get; set; }

        /// <summary>
        /// UserId
        /// </summary>
        [DataMember]
        public int UserId { get; set; }


    }
}
