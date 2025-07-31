using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UnderwritingServices.Models.Base
{
    [DataContract]
    public class BaseBusinessRuleSet:Extension
    {
        /// <summary>
        /// Identificador de la regla
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Descripcion de la Regla (Nombre)
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}
