using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UnderwritingServices.Models.Base
{
    /// <summary>
    /// Condiciones
    /// </summary>
    /// <seealso cref="Sistran.Core.Application.CommonService.Models.Extension" />
    [DataContract]
    public class BaseConditionLevel : Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Descripción
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public int ConditionValue { get; set; }
    }
}
