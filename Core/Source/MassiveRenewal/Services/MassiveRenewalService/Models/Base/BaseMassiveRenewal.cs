using Sistran.Core.Application.MassiveServices.Models;
using System;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.MassiveRenewalServices.Models.Base
{
    [DataContract]
    public class BaseMassiveRenewal : MassiveLoad
    {
        /// <summary>
        /// Número Solicitud Agrupadora
        /// </summary>
        [DataMember]
        public int? RequestId { get; set; }

        /// <summary>
        /// Inicio
        /// </summary>
        [DataMember]
        public DateTime? CurrentFrom { get; set; }

        /// <summary>
        /// Final
        /// </summary>
        [DataMember]
        public DateTime? CurrentTo { get; set; }
    }
}
