using System;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.Transports.Endorsement.Adjustment.ApplicationServices.DTOs
{
    /// <summary>
    /// modelo para la informacion resumen de los riesgos asociados al endoso seleccionado.
    /// </summary>
    [DataContract]
    public class SummaryDTO
    {
        /// <summary>
        /// sumatoria de las sumas aseguradas de todos los riesgos que se encuentren vigentes
        /// </summary>
        [DataMember]
        public Decimal Sum { get; set; }

        /// <summary>
        /// sumatoria de las primas de los riesgos que se encuentren vigentes
        /// </summary>
        [DataMember]
        public Decimal Premiun { get; set; }

        /// <summary>
        /// valor de los gastos de emisión que se generaron
        /// </summary>
        [DataMember]
        public Decimal Expense { get; set; }

        /// <summary>
        ///  valor de los recargos que tenga el negocio
        /// </summary>
        [DataMember]
        public Decimal Surcharge { get; set; }

        /// <summary>
        /// valor de los descuentos que tenga el negocio
        /// </summary>
        [DataMember]
        public Decimal Discount { get; set; }

        /// <summary>
        /// el valor de los impuestos que se generaron, para el endoso o póliza
        /// </summary>
        [DataMember]
        public Decimal Tax { get; set; }

        /// <summary>
        /// valor corresponde a la sumatoria de Prima + Gastos + Recargos - Descuentos + Impuestos
        /// </summary>
        [DataMember]
        public Decimal TotalPremiun { get; set; }

        /// <summary>
        /// valor corresponde a la sumatoria de Prima + Gastos + Recargos - Descuentos + Impuestos
        /// </summary>
        [DataMember]
        public int  RiskCount { get; set; }
    }
}
