using Sistran.Core.Application.AccountingServices.Enums;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs
{
    [DataContract]
    public class HolderDTO
    {
        [DataMember]
        public string Name { get; set; }
        
        [DataMember]
        public int InsuredId { get; set; }

        ///// <summary>
        ///// ImputationType: Tipo de Imputacion 
        ///// </summary>        
        [DataMember]
        public int CustomerType { get; set; }

        ///// <summary>
        ///// ImputationType: Tipo de Imputacion 
        ///// </summary>        
        [DataMember]
        public int IndividualType { get; set; }

        ///// <summary>
        ///// ImputationType: Tipo de Imputacion 
        ///// </summary>        
        [DataMember]
        public int IndividualId { get; set; }
    }
}
