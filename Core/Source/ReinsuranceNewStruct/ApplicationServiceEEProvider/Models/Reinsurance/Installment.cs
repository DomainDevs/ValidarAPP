using Sistran.Core.Application.CommonService.Models;
using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance
{
    /// <summary>
    /// Installment
    /// </summary>
    [DataContract]
    public class Installment
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
        public LevelCompany LevelCompany { get; set; }
        

        /// <summary>
        /// InstallmentNumber
        /// </summary>
        [DataMember]
        public int InstallmentNumber { get; set; }

       
        /// <summary>
        /// PaidAmount
        /// </summary>
        [DataMember]
        public Amount PaidAmount { get; set; }

        /// <summary>
        /// PaidDate
        /// </summary>
        [DataMember]
        public DateTime PaidDate { get; set; }



    }
}