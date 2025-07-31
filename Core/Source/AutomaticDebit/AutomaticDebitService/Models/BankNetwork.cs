
using System.Runtime.Serialization;

//Sistran
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.TaxServices.Models;

namespace Sistran.Core.Application.AutomaticDebitServices.Models
{
    /// <summary>
    /// BankNetwork: Red
    /// </summary>
    [DataContract]
    public class BankNetwork
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Description: Descripción 
        /// </summary>        
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// HasTax: Tiene Impuesto
        /// </summary>        
        [DataMember]
        public bool HasTax { get; set; }

        /// <summary>
        /// TaxCategory: Tipo Impuesto
        /// </summary>        
        [DataMember]
        public TaxCategory TaxCategory { get; set; }
       
        /// <summary>
        /// Commission: Valor de Comision 
        /// </summary>        
        [DataMember]
        public Amount Commission { get; set; }

        /// <summary>
        /// RetriesNumber: Numero de Reintentos  
        /// </summary>        
        [DataMember]
        public int RetriesNumber { get; set; }

        /// <summary>
        /// RequiresNotification: Todos los Lotes de la red requiere Notificacion 
        /// </summary>        
        [DataMember]
        public bool RequiresNotification { get; set; }
    
        
    }
}
