using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Sistran.Core.Application.ReportingServices.Models.Formats;


namespace Sistran.Core.Application.AutomaticDebitServices.Models
{
    /// <summary>
    /// AutomaticDebitFormat
    /// </summary>
    [DataContract]
    public class AutomaticDebitFormat
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// BankNetwork: Red 
        /// </summary>        
        [DataMember]
        public BankNetwork BankNetwork { get; set; }

        /// <summary>
        /// FormatUsingType: Tipo de Uso del Formato
        /// </summary>        
        [DataMember]
        public FormatUsingTypes FormatUsingType { get; set; }

        /// <summary>
        /// Format
        /// </summary>        
        [DataMember]
        public Format Format { get; set; }


    }
}
