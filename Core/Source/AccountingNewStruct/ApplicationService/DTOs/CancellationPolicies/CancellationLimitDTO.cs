using Sistran.Core.Application.AccountingServices.DTOs.Search;
using System.Runtime.Serialization;




namespace Sistran.Core.Application.AccountingServices.DTOs.CancellationPolicies
{
    /// <summary>
    /// Limite para Cancelacion: 
    /// </summary>
    /// <returns></returns>
    [DataContract]
    public class CancellationLimitDTO
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Sucursal
        /// </summary>        
        [DataMember]
        public BranchDTO Branch { get; set; }

        /// <summary>
        /// Limite de Cancelacion en Dias
        /// </summary>        
        [DataMember]
        public int CancellationLimitDays { get; set; }
    }
}
