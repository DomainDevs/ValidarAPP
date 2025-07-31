using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations
{
    [DataContract]
    public class ReinsuranceCheckingAccountItem
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// TempReinsuranceCheckingAccountId 
        /// </summary>        
        [DataMember]
        public int TempReinsuranceCheckingAccountId { get; set; }


        /// <summary>
        /// ReinsuranceCheckingAccountId 
        /// </summary>        
        [DataMember]
        public int ReinsuranceCheckingAccountId { get; set; }
    }
}
