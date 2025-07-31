using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.UnderwritingServices.Enums;
using System.Runtime.Serialization;
using Sistran.Core.Application.Extensions;
namespace Sistran.Core.Application.UnderwritingServices.Models.Base
{
    /// <summary>
    /// Riesgo Base
    /// </summary>
    /// <seealso cref="Sistran.Core.Application.CommonService.Models.Extension" />
    [DataContract]
    public class BaseRisk : Extension
    {
        /// <summary>
        /// Obtiene o establece el identificador de Riesgo
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece el identificador de Riesgo de la tabla iss
        /// </summary>
        [DataMember]
        public int RiskId { get; set; }

        /// <summary>
        /// Obtiene o establece el valor de Prima
        /// </summary>
        [DataMember]
        public decimal Premium { get; set; }

        /// <summary>
        /// Suma asegurada
        /// </summary>
        [DataMember]
        public decimal AmountInsured { get; set; }

        /// <summary>
        /// Nombre del riesgo
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Es asegurado pagador
        /// </summary>
        [DataMember]
        public bool IsInsuredPayer { get; set; }

        /// <summary>
        ///Numero del riesgo
        /// </summary>
        [DataMember]
        public int Number { get; set; }

        /// <summary>
        /// Esta Persistido
        /// </summary>        
        [DataMember]
        public bool IsPersisted { get; set; }

        /// <summary>
        /// Obtiene o establece el estado del riesgo
        /// </summary>
        [DataMember]
        public RiskStatusType? Status { get; set; }

        /// <summary>
        /// Estado original del riesgo
        /// </summary>
        [DataMember]
        public RiskStatusType? OriginalStatus { get; set; }
            
        /// <summary>
        /// Obtiene o establece el tipo de riesgo cubierto
        /// </summary>
        [DataMember]
        public CoveredRiskType? CoveredRiskType { get; set; }

        /// <summary>
        /// 100% retencion 
        /// </summary>
        [DataMember]
        public bool IsRetention { get; set; }


        /// <summary>
        /// A nivel nacional
        /// </summary>
        [DataMember]
        public bool IsNational { get; set; }

    }
}
