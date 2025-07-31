using System.Runtime.Serialization;
using Sistran.Core.Application.Extensions;
namespace Sistran.Core.Application.UniquePersonService.V1.Models.Base
{
    [DataContract]
    public class BaseAssetType : Extension
    {
        /// <summary>
        /// Código
        /// </summary>
        [DataMember]
        public int Code { get; set; }

        /// <summary>
        /// Descripción
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Si es vehículo
        /// </summary>
        [DataMember]
        public bool IsVehicle { get; set; }
    }
}
