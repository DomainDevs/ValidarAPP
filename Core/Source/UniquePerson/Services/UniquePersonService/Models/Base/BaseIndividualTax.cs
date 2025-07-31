using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniquePersonService.Models.Base
{
    [DataContract]
    public class BaseIndividualTax : Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Id del individuo
        /// </summary>
        [DataMember]
        public int IndividualId { get; set; }
    }
}
