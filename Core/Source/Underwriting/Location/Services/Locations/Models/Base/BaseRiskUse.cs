using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.Locations.Models.Base
{
    [DataContract]
    public class BaseRiskUse: Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// uso del riesgo
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Abreviatura uso del riesgo
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }
    }
}
