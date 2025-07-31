using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;


namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class CoinsuranceBalanceDTO 
    {
        [DataMember]
        public int CoinsuranceBalanceId { get; set; }
        [DataMember]
        public int CoinsuredCompanyId { get; set; }
        [DataMember]
        public DateTime BalanceDate { get; set; }
        [DataMember]
        public int CurrencyId { get; set; }
        [DataMember]
        public DateTime LastBalanceDate { get; set; }
        [DataMember]
        public decimal BalanceAmount { get; set; }
        [DataMember]
        public decimal BalanceIncomeAmount { get; set; }
        [DataMember]
        public int NumSheet { get; set; }
    }
}
