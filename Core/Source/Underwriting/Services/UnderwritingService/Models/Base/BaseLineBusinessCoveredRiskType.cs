using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UnderwritingServices.Models.Base
{

    /// <summary>
    /// Ramo tecnico 
    /// </summary>
    /// <seealso cref="Sistran.Core.Application.CommonService.Models.Extension" />
    [DataContract]
    public class BaseLineBusinessCoveredRiskType : Extension
    {
        /// <summary>
        /// Id de LineBusiness(Ramo Tecnico)
        /// </summary>
        [DataMember]
        public int IdLineBusiness { get; set; }

        /// <summary>
        /// Id del tipo de Riesgo
        /// </summary>
        [DataMember]
        public int IdRiskType { get; set; }
    }
}
