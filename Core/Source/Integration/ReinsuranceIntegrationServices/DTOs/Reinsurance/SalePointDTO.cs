using System.Runtime.Serialization;

namespace Sistran.Core.Integration.ReinsuranceIntegrationServices.DTOs.Reinsurance
{
    [DataContract]
    public class SalePointDTO
    {
        /// <summary>
        /// Indetificador
        /// </summary>        
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Punto de Venta
        /// </summary>      
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Abreviatura
        /// </summary>    
        [DataMember]
        public string SmallDescription { get; set; }

        /// <summary>
        /// Es por defecto
        /// </summary>
        [DataMember]
        public bool IsDefault { get; set; }

        /// <summary>
        /// Es por defecto
        /// </summary>
        [DataMember]
        public bool IsEnabled { get; set; }
    }
}
