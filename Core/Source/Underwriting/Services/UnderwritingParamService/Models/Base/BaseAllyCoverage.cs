using Sistran.Core.Application.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.UnderwritingParamService.Models.Base
{
    [DataContract]
    public class BaseAllyCoverage : Extension
    {
        /// <summary>
        /// Identificador de la Cobertura.
        /// </summary>
        private int _Id;

        /// <summary>
        /// Porcentaje de la cobertura aliada
        /// </summary>
        private decimal _CoveragePercentage;

        /// <summary>
        /// Constructor vacio <see cref=""/>.
        /// </summary>
        public BaseAllyCoverage() {}

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref=""/>.
        /// </summary>
        /// <param name="id">Identificador de la Cobertura.</param>
        /// <param name="coveragePercentage">Porcentaje de la Cobertura Aliada.</param>
        public BaseAllyCoverage(int id, decimal coveragePercentage)
        {
            _Id = id;
            _CoveragePercentage = coveragePercentage;
        }

        /// <summary>
        /// Obtiene el Id de la Cobertura.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Obtiene el Porcentaje de la Cobertura Aliada.
        /// </summary>
        public decimal CoveragePercentage {get; set; }
    }
}
