using Sistran.Core.Application.Locations.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.Locations.Models
{
    [DataContract]
    public class NomenclatureAddress: BaseNomenclatureAddress
    {
        /// <summary>
        /// subfijo2 de 
        /// </summary>
        [DataMember]
        public Suffix Suffix2 { get; set; }

        /// <summary>
        /// apartamento u oficina 
        /// </summary>
        [DataMember]
        public ApartmentOrOffice ApartmentOrOffice { get; set; }

        /// <summary>
        ///  Tipo de calle
        /// </summary>
        [DataMember]
        public RouteType Type { get; set; }
        /// <summary>
        /// subfijo1 
        /// </summary>
        [DataMember]
        public Suffix Suffix1 { get; set; }

    }
}
