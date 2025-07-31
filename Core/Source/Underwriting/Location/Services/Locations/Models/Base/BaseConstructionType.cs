using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.Locations.Models.Base
{
    [DataContract]
    public class BaseConstructionType: Extension
    {
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// tipo de construccion
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Abreviatura de tipo de construccion
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }
    }
}
