using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.UnderwritingParamService.Models.Base
{
    [DataContract]
    public class BaseQueryAllyCoverage
    {
        /// <summary>
        /// Id de cobertura
        /// </summary>
        public int _Id;
        /// <summary>
        /// Descripción de la cobertura
        /// </summary>
        public string _Description;

        /// <summary>
        /// Obtiene el Id de la Cobertura.
        /// </summary>

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref=""/>.
        /// </summary>
        /// <param name="id">Identificador de la Cobertura.</param>
        /// <param name="coveragePercentage">Porcentaje de la Cobertura Aliada.</param>
        public BaseQueryAllyCoverage(int id, string description)
        {
            _Id = id;
            _Description = description;
        }
        public BaseQueryAllyCoverage() {}
        public int Id{get; set; }

        /// <summary>
        /// Obtiene el Porcentaje de la Cobertura Aliada.
        /// </summary>
        public string Description{get; set; }
        

    }
}
