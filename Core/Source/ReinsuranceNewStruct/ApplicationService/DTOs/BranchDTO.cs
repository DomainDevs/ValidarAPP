using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    [DataContract]
    public class BranchDTO
    {
        /// <summary>
        /// Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Descripcion corta
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }

        /// <summary>
        /// Es por defecto
        /// </summary>
        [DataMember]
        public bool IsDefault { get; set; }


        /// <summary>
        /// Sale Point
        /// </summary>
        [DataMember]
        public SalePointDTO SalePoint { get; set; }

    }
}
