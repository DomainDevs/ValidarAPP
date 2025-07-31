using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.Vehicles.Models.Base
{
    /// <summary>
    /// Combustible
    /// </summary>
    /// <seealso cref="Sistran.Core.Application.CommonService.Models.Extension" />
    [DataContract]
    public class BaseFuel : Extension
    {
        /// <summary>
        /// Identificador Tipo de Combustible
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Descripcion Tipo de Combustible
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}
