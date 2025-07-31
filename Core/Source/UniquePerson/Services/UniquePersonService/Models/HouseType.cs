using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniquePersonService.Models
{
    [DataContract]
    public class HouseType : Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }
        /// <summary>
        /// Tipo de vivienda
        /// </summary>
        [DataMember]

        public string Description { get; set; }
        /// <summary>
        /// Abreviatura de tipo de vivienda
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }
    }
}
