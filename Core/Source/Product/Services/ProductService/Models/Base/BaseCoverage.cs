using Sistran.Core.Application.Extensions;

namespace Sistran.Core.Application.ProductServices.Models.Base
{
    public class BaseCoverage : Extension
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Atributo para la propiedad Script de Reglas
        /// </summary>
        public int? RiskTypeId { get; set; }

        /// <summary>
        /// Atributo para la propiedad Reglas pre
        /// </summary>
        public int? PrefixId { get; set; }
    }
}
