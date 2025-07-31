using System.Runtime.Serialization;

//SISTRAN
using Sistran.Core.Application.CommonService.Models;


namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.CancellationPolicies
{
    /// <summary>
    /// Limite para Cancelacion: 
    /// </summary>
    /// <returns></returns>
    [DataContract]
    public class CancellationLimit
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
        public Branch Branch { get; set; }

        /// <summary>
        /// Limite de Cancelacion en Dias
        /// </summary>        
        [DataMember]
        public int CancellationLimitDays { get; set; }
    }
}
