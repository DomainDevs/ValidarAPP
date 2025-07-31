using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.Locations.Models.Base
{
    [DataContract]
    public class BaseRouteType: Extension
    {
        /// <summary>
        /// Identificador Tipo de Calle
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Descripcion de calle
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        ///Descripcion de calle
        /// </summary>

        [DataMember]
        public string SmallDescription { get; set; }

        /// <summary>
        /// Identificador similar Tipo de Calle
        /// </summary>
        [DataMember]
        public int? SimilarStreetTypeCd { get; set; }

    }
}
