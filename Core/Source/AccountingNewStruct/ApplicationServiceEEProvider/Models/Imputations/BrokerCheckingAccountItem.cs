using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations
{
    [DataContract]
    public class BrokerCheckingAccountItem
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
