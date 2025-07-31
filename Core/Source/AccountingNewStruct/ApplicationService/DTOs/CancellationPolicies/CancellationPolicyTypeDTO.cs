using System.Runtime.Serialization;



namespace Sistran.Core.Application.AccountingServices.DTOs.CancellationPolicies
{
    /// <summary>
    /// Tipo de Cancelacion de Poliza
    /// </summary>
    /// <returns></returns>
    [DataContract]
    public class CancellationPolicyTypeDTO
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Descripcion
        /// </summary>        
        [DataMember]
        public string Description { get; set; }

    }
}
