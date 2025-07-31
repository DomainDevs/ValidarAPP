using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniquePersonService.Models.Base
{
    [DataContract]
    public class BaseCompanyName : Extension
    {
        /// <summary>
        /// Id Individuo
        /// </summary>
        [DataMember]
        public int IndividualId { get; set; }

        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int NameNum { get; set; }

        /// <summary>
        /// Descripción
        /// </summary>
        [DataMember]
        public string TradeName { get; set; }

        /// <summary>
        /// Es Principal
        /// </summary>
        [DataMember]
        public bool IsMain { get; set; }
    }
}
