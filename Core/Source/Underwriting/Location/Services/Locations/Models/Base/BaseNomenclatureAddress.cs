using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.Locations.Models.Base
{
    [DataContract]
    public class BaseNomenclatureAddress: Extension
    {

        /// <summary>
        /// nombre 
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// letra
        /// </summary>
        [DataMember]
        public string Letter { get; set; }


        /// <summary>
        /// Nro 1 de 
        /// </summary>
        [DataMember]
        public int Number1 { get; set; }

        /// <summary>
        /// letra2 de 
        /// </summary>
        [DataMember]
        public string Letter2 { get; set; }

        /// <summary>
        /// Nro 2 de 
        /// </summary>
        [DataMember]
        public int Number2 { get; set; }

        /// <summary>
        /// Nro 3 
        /// </summary>
        [DataMember]
        public int? Number3 { get; set; }

    }
}
