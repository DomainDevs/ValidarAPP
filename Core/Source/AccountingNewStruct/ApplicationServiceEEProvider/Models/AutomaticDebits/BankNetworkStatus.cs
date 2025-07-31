using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.AutomaticDebits
{
    /// <summary>
    /// BankNetworkStatus: Estado del Cupon de la Red
    /// </summary>
    [DataContract]
    public class BankNetworkStatus
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
        /// RejectedCouponStatus: Estados del Cupon Rechazados
        /// </summary>        
        [DataMember]
        public List<CouponStatus> RejectedCouponStatus { get; set; }  

         /// <summary>
        /// AcceptedCouponStatus: Estados del Cupon Aceptados
        /// </summary>        
        [DataMember]
        public List<CouponStatus> AcceptedCouponStatus { get; set; }  
               
                       
    }
}
