using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.Vehicles.ThirdPartyLiabilityService.Models.Base
{
    [DataContract]
    public class BaseShuttle : Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Descripcion de trayecto
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Esta habilitado
        /// </summary>
        [DataMember]
        public bool Enabled { get; set; }

        /// <summary>
        /// Descripcion corta trayecto
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }
    }
}
