using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UnderwritingServices.Models.Base
{
    /// <summary>
    /// Coberturas por Planes Tecnicos
    /// </summary>
    [DataContract]
    public class BaseVehicleBody : Extension
    {
        /// <summary>
        /// Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// SmallDescription
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }

        /// <summary>
        /// State
        /// </summary> 
        [DataMember]
        public int? State { get; set; }
    }
}
