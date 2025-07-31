using System.Runtime.Serialization;

namespace Sistran.Core.Application.ModelServices.Models.Underwriting
{
    /// <summary>
    /// MOD-S Tipos de riesgo cubiertos
    /// </summary>
    [DataContract]
    public class CoveredRiskTypeQueryServiceModel
    {
        /// <summary>
        /// Obtiene o establece el id del tipos de riesgo cubiertos
        /// </summary>
        [DataMember]
        public int Id { get; set; }
        /// <summary>
        /// Obtiene o establece la descripción del tipos de riesgo cubiertos
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}
