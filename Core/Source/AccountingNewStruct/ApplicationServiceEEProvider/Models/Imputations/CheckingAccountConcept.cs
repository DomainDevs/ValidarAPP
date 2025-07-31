using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations
{
    /// <summary>
    /// CheckingAccountConcept: Concepto Cuenta Corriente
    /// </summary>
    [DataContract]
    public class CheckingAccountConcept 
    {

        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }
        /// <summary>
        /// Description 
        /// </summary> 
        [DataMember]
        public string Description { get; set; }
        /// <summary>
        /// ItemsEnabled
        /// </summary>
        [DataMember]
        public bool ItemsEnabled { get; set; }
    }
}
