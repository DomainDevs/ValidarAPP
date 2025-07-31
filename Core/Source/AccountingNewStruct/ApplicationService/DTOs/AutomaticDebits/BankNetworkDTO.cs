using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs.AutomaticDebits
{
    /// <summary>
    /// BankNetwork: Red
    /// </summary>
    [DataContract]
    public class BankNetworkDTO
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
        public TaxCategoryDTO TaxCategory { get; set; }
       
        /// <summary>
        /// Commission: Valor de Comision 
        /// </summary>        
        [DataMember]
        public AmountDTO Commission { get; set; }

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
