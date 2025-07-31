using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePersonService.Models.Base
{
    [DataContract]
    public class BaseOperatingQuota : Individual
    {
        /// <summary>
        /// Obtiene o Setea Identificador Cupo Operativo
        /// </summary>
        /// <value>
        /// Identificador
        /// </value>
        [DataMember]
        public int OperatingQuotaCd { get; set; }

        /// <summary>
        /// Obtiene o Setea Vigencia hasta Habilitado el Cupo
        /// </summary>
        /// <value>
        /// Vigencia hasta Habilitado el Cupo
        /// </value>
        [DataMember]
        public DateTime CurrentTo { get; set; }

        /// <summary>
        /// Gets or sets the string current to.
        /// </summary>
        /// <value>
        /// The string current to.
        /// </value>
        [DataMember]
        public string StrCurrentTo { get; set; }
    }
}
