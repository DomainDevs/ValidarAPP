using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UnderwritingServices.Models.Base
{
    /// <summary>
    /// Coberturas por Planes Tecnicos
    /// </summary>
    [DataContract]
    public class BaseVehicleUse : Extension
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
        /// PrefixId
        /// </summary>
        [DataMember]
        public int PrefixId { get; set; }
    }
}
