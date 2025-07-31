using Sistran.Core.Application.Extensions;
using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePersonService.V1.Models.Base
{
    [DataContract]
    public class BaseOperatingQuota : Extension
    {
        /// <summary>
        /// Obtiene o Setea Suma 
        /// </summary>
        /// <value>
        /// Suma
        /// </value>
        [DataMember]
        public decimal Amount { get; set; }

        /// <summary>
        /// Obtiene o Setea Suma 
        /// </summary>
        /// <value>
        /// Suma
        /// </value>
        [DataMember]
        public int CurrencyId { get; set; }

        /// <summary>
        /// Obtiene o Setea Vigencia hasta Habilitado el Cupo
        /// </summary>
        /// <value>
        /// Vigencia hasta Habilitado el Cupo
        /// </value>
        [DataMember]
        public DateTime CurrentTo { get; set; }

        /// <summary>
        /// Obtiene o Setea Suma 
        /// </summary>
        /// <value>
        /// Suma
        /// </value>
        [DataMember]
        public int LineBusinessId { get; set; }

        /// <summary>
        /// Individual id
        /// </summary>
        [DataMember]
        public int IndividualId { get; set; }


    }
}
