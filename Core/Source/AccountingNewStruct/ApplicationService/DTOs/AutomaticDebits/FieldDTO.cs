using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs.AutomaticDebits
{
    /// <summary>
    /// Field: Campo del Formato
    /// </summary>
    [DataContract]
    public class FieldDTO
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
        /// ExternalDescription: Descripción Externa 
        /// </summary>        
        [DataMember]
        public string ExternalDescription { get; set; }

        /// <summary>
        /// Start: Posicion Inicial 
        /// </summary>        
        [DataMember]
        public int Start { get; set; }

        /// <summary>
        /// Length: Longitud
        /// </summary>        
        [DataMember]
        public int Length { get; set; }

        /// <summary>
        /// Value: Valor
        /// </summary>        
        [DataMember]
        public string Value { get; set; }

        /// <summary>
        /// Mask: Cadena de Formato del campo
        /// </summary>        
        [DataMember]
        public string Mask { get; set; }

        /// <summary>
        /// Align: Alineacion
        /// </summary>        
        [DataMember]
        public string Align { get; set; }
        
         /// <summary>
        /// Filled: Relleno
        /// </summary>        
        [DataMember]
        public string Filled { get; set; }

        /// <summary>
        /// IsRequired: Campo Requerido
        /// </summary>        
        [DataMember]
        public bool IsRequired { get; set; }

        /// <summary>
        /// Order: Orden de campo
        /// </summary>        
        [DataMember]
        public int  Order { get; set; }         
    }
}
