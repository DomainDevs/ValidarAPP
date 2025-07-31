using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.MassiveUnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.MassiveUnderwritingServices.Models
{
    [DataContract]
    public class MassiveCancellationRow
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Id Cargue Masivo
        /// </summary>
        [DataMember]
        public int MassiveLoadId { get; set; }

        /// <summary>
        /// Número Fila
        /// </summary>
        [DataMember]
        public int RowNumber { get; set; }

        /// <summary>
        /// Riesgo
        /// </summary>
        [DataMember]
        public Risk Risk { get; set; }

        /// <summary>
        /// Estado
        /// </summary>
        [DataMember]
        public MassiveLoadProcessStatus Status { get; set; }

        /// <summary>
        /// Tiene Error?
        /// </summary>
        [DataMember]
        public bool HasError { get; set; }

        /// <summary>
        /// Observaciones
        /// </summary>
        [DataMember]
        public string Observations { get; set; }

        /// <summary>
        /// Fila Serializada
        /// </summary>
        [DataMember]
        public string SerializedRow { get; set; }

        /// <summary>
        /// Comisión
        /// </summary>
        [DataMember]
        public decimal TotalCommission { get; set; }

        /// <summary>
        /// Tiene Eventos?
        /// </summary>
        [DataMember]
        public bool HasEvents { get; set; }

        /// <summary>
        /// Propiedad SubcoveredRiskType
        /// </summary>
        [DataMember]
        public SubCoveredRiskType SubcoveredRiskType { get; set; }

        /// <summary>
        /// Propiedad tempId
        /// </summary>
        [DataMember]
        public int? tempId { get; set; }



    }
}