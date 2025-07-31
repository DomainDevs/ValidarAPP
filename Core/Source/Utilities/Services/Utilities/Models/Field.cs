using Sistran.Core.Application.UtilitiesServices.Models.Base;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System.Runtime.Serialization;

namespace Sistran.Core.Services.UtilitiesServices.Models
{
    [DataContract]
    public class Field : BaseField
    {
        /// <summary>
        /// Tipo De Campo
        /// </summary>
        [DataMember]
        public FieldType FieldType { get; set; }

        /// <summary>
        /// Estado
        /// </summary>
        [DataMember]
        public ParametrizationStatus? parametrizationStatus { get; set; }
    }
}