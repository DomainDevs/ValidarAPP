using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UnderwritingServices.Models.Base
{
    [DataContract]
    public class BaseVehicleFuel : extension
    {
        /// <summary>
        /// Identificador Grupo de Factruacion
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// SmallDescripcion
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }
    }
}
