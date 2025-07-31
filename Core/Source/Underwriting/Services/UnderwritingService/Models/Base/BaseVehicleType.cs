using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UnderwritingServices.Models.Base
{
    /// <summary>
    /// Coberturas por Planes Tecnicos
    /// </summary>
    [DataContract]
    public class BaseVehicleType : Extension
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
        /// Atributo Is Sublimit
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }

        /// <summary>
        /// IsTruck
        /// </summary>
        [DataMember]
        public bool IsTruck { get; set; }

        /// <summary>
        /// IsActive
        /// </summary> 
        [DataMember]
        public bool IsActive { get; set; }

        /// <summary>
        /// State
        /// </summary> 
        [DataMember]
        public int? State { get; set; }
    }
}
