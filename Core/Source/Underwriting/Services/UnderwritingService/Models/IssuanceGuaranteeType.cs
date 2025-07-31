using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UnderwritingServices.Models
{
    /// <summary>
    /// Tipos de Garantias
    /// </summary>
    [DataContract]
    public class IssuanceGuaranteeType : Extension
    {
        /// <summary>
        /// Codigo
        /// </summary>
        [DataMember]
        public int Code { get; set; }

        /// <summary>
        /// Descripcion
        /// </summary>
        [DataMember]
        public string Description { get; set; }


    }
}
