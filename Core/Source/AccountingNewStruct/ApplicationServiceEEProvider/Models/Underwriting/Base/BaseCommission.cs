using Sistran.Core.Application.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Underwriting.Base
{
    [DataContract]
    public class BaseCommission : Extension
    {
        /// <summary>
        /// Porcentaje
        /// </summary>
        [DataMember]
        public Decimal Percentage { get; set; }

        /// <summary>
        /// Porcentaje Adicional
        /// </summary>
        [DataMember]
        public Decimal PercentageAdditional { get; set; }

        /// <summary>
        /// Base de Cálculo
        /// </summary>
        [DataMember]
        public Decimal CalculateBase { get; set; }

        /// <summary>
        /// Valor
        /// </summary>
        [DataMember]
        public Decimal Amount { get; set; }
    }
}