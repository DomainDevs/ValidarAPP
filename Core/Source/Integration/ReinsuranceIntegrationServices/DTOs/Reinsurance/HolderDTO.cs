using System.Runtime.Serialization;

namespace Sistran.Core.Integration.ReinsuranceIntegrationServices.DTOs.Reinsurance
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
