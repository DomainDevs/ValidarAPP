using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AutomaticDebitServices.Models
{
    /// <summary>
    /// CouponStatus: Estado del Cupon 
    /// </summary>
    [DataContract]
    public class CouponStatus
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// GroupDescription: Descripcion de Grupo 
        /// </summary>        
        [DataMember]
        public string GroupDescription { get; set; }

        /// <summary>
        /// SmallDescription: Descripcion Corta, codigo 
        /// </summary>        
        [DataMember]
        public string SmallDescription { get; set; }

        /// <summary>
        /// ExternalDescription: Descripcion  externa del Estado (Banco)
        /// </summary>
        [DataMember]
        public string ExternalDescription { get; set; }

        /// <summary>
        /// Description: Descripción 
        /// </summary>        
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// IsEnabled: Habilitado 
        /// </summary>        
        [DataMember]
        public bool IsEnabled { get; set; }

        /// <summary>
        /// RetriesNumber: Numero de Reintentos
        /// </summary>        
        [DataMember]
        public int RetriesNumber { get; set; }  

        /// <summary>
        /// CouponStatusType: Tipo de Estado del Cupon 
        /// </summary>        
        [DataMember]
        public CouponStatusTypes CouponStatusType { get; set; }  
        
                       
    }
}
