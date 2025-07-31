using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.Locations.Models.Base
{

    [DataContract]
    public class BaseApartmentOrOffice:Extension
    {
        /// <summary>
        /// identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// apartamento u oficina
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// apartamento u oficina
        /// </summary>

        [DataMember]
        public string SmallDescription { get; set; }
    }
}
