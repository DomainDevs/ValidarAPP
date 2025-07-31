using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;


namespace Sistran.Core.Application.AccountingServices.DTOs.Imputations
{
    [DataContract]
    public class BrokerCheckingAccountItemDTO
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// TempBrokerCheckingAccountId 
        /// </summary>        
        [DataMember]
        public int TempBrokerCheckingAccountId { get; set; }


        /// <summary>
        /// BroketCheckingAccountId 
        /// </summary>        
        [DataMember]
        public int BrokerCheckingAccountId { get; set; }
    }

}
