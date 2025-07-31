using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Integration.CommonServices.DTOs
{
    [DataContract]
    public class BranchDTO
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Sucursal
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
        /// Puntos de venta de la sucursal
        /// </summary>
        [DataMember]
        public List<SalePointDTO> SalePoints { get; set; }

    }
}
