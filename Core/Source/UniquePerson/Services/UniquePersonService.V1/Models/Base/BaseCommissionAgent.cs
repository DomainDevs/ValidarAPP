using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.UniquePersonService.V1.Models.Base
{
    [DataContract]
    public class BaseCommissionAgent : Extension
    {

        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }


        /// <summary>
        /// Individual id
        /// </summary>
        [DataMember]
        public int IndividualId { get; set; }


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
    }
}
