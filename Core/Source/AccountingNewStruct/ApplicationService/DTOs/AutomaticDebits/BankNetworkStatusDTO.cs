using Sistran.Core.Application.AccountingServices.DTOs.Search;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs.AutomaticDebits
{
    /// <summary>
    /// BankNetworkStatus: Estado del Cupon de la Red
    /// </summary>
    [DataContract]
    public class BankNetworkStatusDTO
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
        public BankNetworkDTO BankNetwork { get; set; }

        /// <summary>
        /// RejectedCouponStatus: Estados del Cupon Rechazados
        /// </summary>        
        [DataMember]
        public List<CouponStatusDTO> RejectedCouponStatus { get; set; }  

         /// <summary>
        /// AcceptedCouponStatus: Estados del Cupon Aceptados
        /// </summary>        
        [DataMember]
        public List<CouponStatusDTO> AcceptedCouponStatus { get; set; }  
               
                       
    }
}
