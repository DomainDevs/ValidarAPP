using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    /// <summary>
    /// Installment
    /// </summary>
    [DataContract]
    public class InstallmentDTO
    {
        /// <summary>
        /// Id 
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// LevelCompany 
        /// </summary>
        [DataMember]
        public LevelCompanyDTO LevelCompany { get; set; }
        
        /// <summary>
        /// InstallmentNumber
        /// </summary>
        [DataMember]
        public int InstallmentNumber { get; set; }
        
        /// <summary>
        /// PaidAmount
        /// </summary>
        [DataMember]
        public AmountDTO PaidAmount { get; set; }

        /// <summary>
        /// PaidDate
        /// </summary>
        [DataMember]
        public DateTime PaidDate { get; set; }
    }
}