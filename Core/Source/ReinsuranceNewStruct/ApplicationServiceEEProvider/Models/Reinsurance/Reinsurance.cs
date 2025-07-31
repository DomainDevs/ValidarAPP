//using Sistran.Core.Application.AccountingServices.Models;
using Sistran.Core.Application.Utilities.Enums;
using System;

using System.Collections.Generic;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance
{
    /// <summary>
    /// Modelo de Reaseguro
    /// </summary>
     [DataContract]
    public class Reinsurance
    {
        /// <summary>
        /// ReinsuranceId
        /// </summary>        
        [DataMember]
        public int ReinsuranceId { get; set; }

        /// <summary>
		/// Number
		/// </summary>
		[DataMember]
        public int Number { get; set; }

        /// <summary>
        /// Movements
        /// </summary>        
        [DataMember]
        public Movements Movements { get; set; }

        /// <summary>
		/// ProcessDate
		/// </summary>
		[DataMember]
        public DateTime ProcessDate { get; set; }

        /// <summary>
		/// IssueDate
		/// </summary>
		[DataMember]
        public DateTime IssueDate { get; set; }

        /// <summary>
		/// ValidityFrom
		/// </summary>
		[DataMember]
        public DateTime ValidityFrom { get; set; }
        /// <summary>
        /// ValidityTo
        /// </summary>
        [DataMember]
        public DateTime ValidityTo { get; set; }

        /// <summary>
        /// IsAutomatic
        /// </summary>        
        [DataMember]
        public bool IsAutomatic { get; set; }

        /// <summary>
        /// UserId
        /// </summary>        
        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        /// ReinsuranceLayers
        /// </summary>        
        [DataMember]
        public List<ReinsuranceLayer> ReinsuranceLayers { get; set; }

        /// <summary>
        /// Status
        /// </summary>        
        [DataMember]
        public bool IsReinsured { get; set; }

    }
}
