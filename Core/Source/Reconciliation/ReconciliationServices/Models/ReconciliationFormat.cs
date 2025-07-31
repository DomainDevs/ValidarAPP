using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

//Sistran

using Sistran.Core.Application.ReportingServices.Models.Formats;
using Sistran.Core.Application.CommonService.Models;

namespace Sistran.Core.Application.ReconciliationServices.Models
{
    /// <summary>
    /// ReconciliationFormat
    /// </summary>
    [DataContract]
    public class ReconciliationFormat
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Bank
        /// </summary>        
        [DataMember]
        public Bank Bank { get; set; }
        
        /// <summary>
        /// Format
        /// </summary>        
        [DataMember]
        public Format Format { get; set; }

       
    }
}
