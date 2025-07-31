using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.AccountingServices.DTOs.Imputations
{
    public class ApplicationPremiumCommisionDTO
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }
        /// <summary>
        /// AppComponentId
        /// </summary>
        [DataMember]
        public int AppComponentId { get; set; }
        /// <summary>
        /// CommisionType
        /// </summary>
        [DataMember]
        public int CommissionType { get; set; }
        /// <summary>
        /// AgentTypeId
        /// </summary>
        [DataMember]
        public int AgentTypeId { get; set; }
        /// <summary>
        /// AgentId
        /// </summary>
        [DataMember]
        public int AgentId { get; set; }
        /// <summary>
        /// AgenteAgencyId
        /// </summary>
        [DataMember]
        public int AgentAgencyId { get; set; }
        /// <summary>
        /// Currency
        /// </summary>
        [DataMember]
        public int CurrencyId { get; set; }
        /// <summary>
        /// ExchangeRate
        /// </summary>
        [DataMember]
        public decimal ExchangeRate { get; set; }
        /// <summary>
        /// Amount
        /// </summary>
        [DataMember]
        public decimal Amount { get; set; }
        /// <summary>
        /// LocalAmount
        /// </summary>
        [DataMember]
        public decimal LocalAmount { get; set; }
        /// <summary>
        /// LocalAmount
        /// </summary>
        [DataMember]
        public decimal BaseAmount { get; set; }

        [DataMember]
        public decimal ApplicationPremiumId { get; set; }
        [DataMember]
        public int DiscountedCommissionId { get; set; }

    }
}
