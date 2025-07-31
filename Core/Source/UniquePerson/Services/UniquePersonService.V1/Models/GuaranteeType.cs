using Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePersonService.V1.Models
{
    /// <summary>
    /// Tipos de Garantias
    /// </summary>
    [DataContract]
    public class GuaranteeType : BaseGuaranteeType
    {
        /// <summary>
        /// Class
        /// </summary>
        [DataMember]
        public GuaranteeClass Class { get; set; }



    }
}
