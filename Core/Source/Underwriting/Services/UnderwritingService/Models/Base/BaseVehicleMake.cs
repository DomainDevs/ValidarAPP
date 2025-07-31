using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UnderwritingServices.Models.Base
{
    [DataContract]
    public class BaseVehicleMake : extension
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
        /// IsFrequently
        /// </summary>
        [DataMember]
        public bool IsFrequently { get; set; }

        /// <summary>
        /// AtVehicleModelCode
        /// </summary>
        [DataMember]
        public int AtVehicleModelCode{ get; set; }

        /// <summary>
        /// IaVehicleModelCode
        /// </summary>
        [DataMember]
        public int IaVehicleModelCode { get; set; }
    }
}
