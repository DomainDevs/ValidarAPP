using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs.Retentions
{
    /// <summary>
    /// RetentionBase: Base de Retencion
    /// </summary>
    /// <returns></returns>
    [DataContract]
    public class RetentionBaseDTO
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
       
             
    }
}
