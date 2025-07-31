using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniquePersonService.Models
{
    /// <summary>
    /// Nivel de Ingresos
    /// </summary>
    [DataContract]
    public class IncomeLevel : Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int? Id { get; set; }
        /// <summary>
        /// Nivel De Ingresos
        /// </summary>
        [DataMember]
        public string Description { get; set; }
        /// <summary>
        /// Abreviatura de Nivel De Ingresostipo de Especialidad
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }
    }
}
