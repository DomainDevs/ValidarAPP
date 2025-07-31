using Sistran.Core.Application.UtilitiesServices.Models.Base;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Services.UtilitiesServices.Models
{
    [DataContract]
    public class Template : BaseTemplate
    {
        /// <summary>
        /// Filas
        /// </summary>
        [DataMember]
        public List<Row> Rows { get; set; }

        /// <summary>
        /// Estado
        /// </summary>
        [DataMember]
        public ParametrizationStatus? parametrizationStatus { get; set; }
    }
}