using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UnderwritingServices.Models.Base
{
    /// <summary>
    /// Coberturas
    /// </summary>
    [DataContract]
    public class BaseCoverDetailType:Extension
    {
        /// <summary>
        /// Cobertura 
        /// </summary>
        [DataMember]
        public int CoverageId { get; set; }

        /// <summary>
        /// Code Detail Type
        /// </summary>
        [DataMember]
        public int Code { get; set; }

        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public bool IsMandatory { get; set; }
    }
}
