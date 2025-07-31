using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.Locations.Models.Base
{
    [DataContract]
    public   class BaseReinsurance: Extension
    {
        /// <summary>
        /// Facultativo
        /// </summary>
        [DataMember]
        public bool IsFacultativo { get; set; }

        /// <summary>
        /// Manzana
        /// </summary>

        [DataMember]
        public string Manzana { get; set; }

        /// <summary>
        /// PML
        /// </summary>
        [DataMember]
        public string PML { get; set; }
    }
}
