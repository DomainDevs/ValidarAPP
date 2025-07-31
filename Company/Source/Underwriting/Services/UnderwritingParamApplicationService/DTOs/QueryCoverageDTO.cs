using Sistran.Company.Application.Utilities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UnderwritingParamApplicationService.DTOs
{
    [DataContract]
    public class QueryCoverageDTO
    {
        /// <summary>
        /// Obtiene o establece Id de Cobertura
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece el id del amparo relacionada a la cobertura
        /// </summary>
        [DataMember]
        public int PerilId { get; set; }

        /// <summary>
        /// Obtiene o establece el id del subramo tecnico relacionado a la cobertura
        /// </summary>
        [DataMember]
        public int SubLineBusinessId { get; set; }

        /// <summary>
        /// Obtiene o establece el id del ramo tecnico relacionado a la cobertura
        /// </summary>
        [DataMember]
        public int LineBusinessId { get; set; }

        /// <summary>
        /// Obtiene o establece el id del objeto del seguro relacionado a la cobertura
        /// </summary>
        [DataMember]
        public int InsuredObjectId { get; set; }

        /// <summary>
        /// Obtiene o establece la descripcion de cobertura
        /// </summary>
        [DataMember]
        public string PrintDescription { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si la cobertura es obligatoria
        /// </summary>
        [DataMember]
        public bool IsPrimary { get; set; }

        /// <summary>
        /// Obtiene la fecha de expiración de la cobertura
        /// </summary>
        [DataMember]
        public DateTime? ExpirationDate { get; set; }

        /// <summary>
        /// Obtiene o establece el id del nivel del influencia
        /// </summary>
        [DataMember]
        public int? CompositionTypeId { get; set; }

        /// <summary>
        /// Obtiene o estableec el id de reglas
        /// </summary>
        [DataMember]
        public int? RuleSetId { get; set; }

        [DataMember]
        public ErrorDTO errorDTO { get; set; }
    }
}
