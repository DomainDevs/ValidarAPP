using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;


namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class ExcessPaymentDTO 
    {
        [DataMember]
        public int ExcessPaymentId { get; set; }
        [DataMember]
        public int CollectId { get; set; }
        [DataMember]
        public int PayerId { get; set; }
        [DataMember]
        public DateTime RegisterDate { get; set; }
        [DataMember]
        public int CurrencyId { get; set; }
        [DataMember]
        public string CurrencyDescription { get; set; }
        [DataMember]
        public decimal Amount { get; set; } 
        [DataMember]
        public decimal UsedAmount { get; set; }

        //indicador de filas para paginación.
        [DataMember]
        public int Rows { get; set; }
    }
}
