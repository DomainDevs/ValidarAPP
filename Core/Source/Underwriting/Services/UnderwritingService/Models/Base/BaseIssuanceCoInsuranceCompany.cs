using Sistran.Core.Application.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.UnderwritingServices.Models.Base
{
    [DataContract]
    public class BaseIssuanceCoInsuranceCompany : Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public decimal Id { get; set; }

        /// <summary>
        /// Compañia coaseguradora
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Porcentaje de participación
        /// </summary>
        [DataMember]
        public decimal ParticipationPercentage { get; set; }

        /// <summary>
        /// Porcentaje de gastos
        /// </summary>
        [DataMember]
        public decimal ExpensesPercentage { get; set; }

        /// <summary>
        /// Porcentaje de participacion propia
        /// </summary>
        [DataMember]
        public decimal ParticipationPercentageOwn { get; set; }

        /// <summary>
        /// Numero de poliza
        /// </summary>
        [DataMember]
        public string PolicyNumber { get; set; }

        /// <summary>
        /// Numero de endoso
        /// </summary>
        [DataMember]
        public string EndorsementNumber { get; set; }
    }
}