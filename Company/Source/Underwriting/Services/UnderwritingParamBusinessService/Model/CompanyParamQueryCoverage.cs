using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UnderwritingParamBusinessService.Model
{
    public class CompanyParamQueryCoverage
    {
        /// <summary>
        /// Obtiene o establece Id de Cobertura
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece el id del amparo relacionada a la cobertura
        /// </summary>
        public int PerilId { get; set; }

        /// <summary>
        /// Obtiene o establece el id del subramo tecnico relacionado a la cobertura
        /// </summary>
        public int SubLineBusinessId { get; set; }

        /// <summary>
        /// Obtiene o establece el id del ramo tecnico relacionado a la cobertura
        /// </summary>
        public int LineBusinessId { get; set; }

        /// <summary>
        /// Obtiene o establece el id del objeto del seguro relacionado a la cobertura
        /// </summary>
        public int InsuredObjectId { get; set; }

        /// <summary>
        /// Obtiene o establece la descripcion de cobertura
        /// </summary>
       public string PrintDescription { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si la cobertura es obligatoria
        /// </summary>
        public bool IsPrimary { get; set; }

        /// <summary>
        /// Obtiene la fecha de expiración de la cobertura
        /// </summary>
        public DateTime? ExpirationDate { get; set; }

        /// <summary>
        /// Obtiene o establece el id del nivel del influencia
        /// </summary>
        public int? CompositionTypeId { get; set; }

        /// <summary>
        /// Obtiene o estableec el id de reglas
        /// </summary>
        public int? RuleSetId { get; set; }
    }
}
