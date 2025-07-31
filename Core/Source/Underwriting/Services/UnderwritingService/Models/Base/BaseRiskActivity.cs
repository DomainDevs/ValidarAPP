using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UnderwritingServices.Models.Base
{

    /// <summary>
    /// Actividad Riesgo
    /// </summary>
    /// <seealso cref="Sistran.Core.Application.CommonService.Models.Extension" />
    [DataContract]
    public class BaseRiskActivity : Extension
    {
        /// <summary>
        /// identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// actividad del riesgo
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Abreviatura de la actividad del riesgo
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }

        /// <summary>
        /// Tipo activiad del riesgo
        /// </summary>
        [DataMember]
        public int RiskActivityTypeId { get; set; }
    }
}
