using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UnderwritingServices.Models.Base
{
    [DataContract]
    public class BaseRiskType : Extension
    {
        /// <summary>
        /// Obtiene o establece el Codigo del tipo de riesgo
        /// </summary>
        [DataMember]
        public int Id { get; set; }
        /// <summary>
        /// Obtiene o establece la cantidad maxima de riegos permitida
        /// </summary>
        [DataMember]
        public int MaxRiskQuantity { get; set; }
        /// <summary>
        /// Obtiene o establece la Descripcion del tipo de riesgo
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}
