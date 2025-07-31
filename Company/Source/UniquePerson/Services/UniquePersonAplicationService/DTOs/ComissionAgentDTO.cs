using Sistran.Company.Application.ModelServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UniquePersonAplicationServices.DTOs
{
    [DataContract]
    public class ComissionAgentDTO
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int AgentId { get; set; }

        /// <summary>
        /// Agencia
        /// </summary>
        [DataMember]
        public int AgencyId { get; set; }

        /// <summary>
        /// Ramo Comercial
        /// </summary>
        [DataMember]
        public int PrefixId { get; set; }

        /// <summary>
        /// Ramo Tecnico
        /// </summary>
        [DataMember]
        public int LineBusinessId { get; set; }

        /// <summary>
        /// Sub Ramo Técnico
        /// </summary>
        [DataMember]
        public int SubLineBusinessId { get; set; }

        /// <summary>
        /// Porcentaje Comision
        /// </summary>
        [DataMember]
        public Decimal PercentageCommission { get; set; }

        /// <summary>
        /// Porcentaje Adicional
        /// </summary>
        [DataMember]
        public Decimal PercentageAdditional { get; set; }

        /// <summary>
        /// Fecha Vigencia
        /// </summary>
        [DataMember]
        public DateTime? DateCommission { get; set; }
        [DataMember]
        public int IndividualId { get; set; }
        [DataMember]
        public StatusTypeService StatusTypeService { get; set; }
        [DataMember]
        public string prefix { get; set; }
        [DataMember]
        public string lineBusiness { get; set; }
        [DataMember]
        public string subLineBusiness { get; set; }
        [DataMember]
        public string agency { get; set; }
    }
}
