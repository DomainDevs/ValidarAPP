namespace Sistran.Company.Application.ProductParamService.Models
{
    using Sistran.Core.Application.ProductParamService.Models.Base;

    /// <summary>
    ///
    /// </summary>
    public class CiaParamCopyProduct: BaseParamCopyProduct
    {
        /// <summary>
        /// propiedad que almacena si al copiar copia los grupos de coberturas
        /// </summary>
        public bool CopyGroupCoverages { get; set; }

        /// <summary>
        /// propiedad que almacena si al copiar copia los planes de pago
        /// </summary>
        public bool CopyPaymentPlan { get; set; }

        /// <summary>
        /// propiedad que almacena si al copiar copia los paquetes de reglas
        /// </summary>
        public bool CopyRuleSet { get; set; }

        /// <summary>
        /// propiedad que almacena si al copiar copia las formas de impresion
        /// </summary>
        public bool CopyPrintingFormes { get; set; }

        /// <summary>
        /// propiedad que almacena si al copiar copia los intermediarios
        /// </summary>
        public bool CopyAgent { get; set; }

        /// <summary>
        /// propiedad que almacena si al copiar copia los Limites RC
        /// </summary>
        public bool CopyLimitRC { get; set; }

        /// <summary>
        /// propiedad que almacena si al copiar copia los Guiones
        /// </summary>
        public bool CopyScript { get; set; }

        /// <summary>
        /// propiedad que almacena si al copiar copia las actividades del riesgo
        /// </summary>
        public bool CopyActivityRisk { get; set; }
    }
}
