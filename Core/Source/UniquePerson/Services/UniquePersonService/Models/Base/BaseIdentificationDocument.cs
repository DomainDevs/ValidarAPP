using Sistran.Core.Application.Extensions;
using System;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniquePersonService.Models.Base
{
    [DataContract]
    public class BaseIdentificationDocument : Extension
    {
        /// <summary>
        /// Número de documento
        /// </summary>
        [DataMember]
        public string Number { get; set; }

        /// <summary>
        /// Fecha de expedición
        /// </summary>
        [DataMember]
        public DateTime ExpeditionDate { get; set; }
    }
}
