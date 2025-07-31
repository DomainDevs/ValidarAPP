
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models
{
    public class LineBusinessByCoveredRiskType
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