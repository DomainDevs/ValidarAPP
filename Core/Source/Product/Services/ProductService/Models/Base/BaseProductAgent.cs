using Sistran.Core.Application.Extensions;
using System;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.ProductServices.Models.Base
{
    /// <summary>
    /// Agentes por producto
    /// </summary>
    /// <seealso cref="Sistran.Core.Application.CommonService.Models.Extension" />
    [DataContract]
    public class BaseProductAgent : Extension
    {
        /// <summary>
        /// Id agente
        /// </summary>
        [DataMember]
        public int IndividualId { get; set; }

        /// <summary>
        /// Id producto
        /// </summary>
        [DataMember]
        public int ProductId { get; set; }

        /// <summary>
        /// Nombre Agente
        /// </summary>
        [DataMember]
        public string FullName { get; set; }

        /// <summary>
        /// Código
        /// </summary>
        [DataMember]
        public int LockerId { get; set; }


        [DataMember]
        public DateTime DateDeclined { get; set; }
    }
}
