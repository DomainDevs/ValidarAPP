using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Sistran.Core.Application.CommonService.Models;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.AccountsPayables
{
    [DataContract]
    public class PaymentRequestNumber
    {
        /// <summary>
        /// Id 
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public int Number { get; set; }

        /// <summary>
        /// Branches 
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public Branch Branch { get; set; }
    }
}
