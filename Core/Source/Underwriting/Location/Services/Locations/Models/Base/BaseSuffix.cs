using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.Locations.Models.Base
{
    [DataContract]
    public class BaseSuffix: Extension
    {
        /// <summary>
        /// Identificador subfijo
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// subfijo
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Abreviatura del subfijo
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }
    }
}
