using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using System;
using System.Collections.Generic;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations
{
    public class ApplicationAccounting : ApplicationItem
    {
        /// <summary>
        /// Identificador
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Identificador del movimiento contable
        /// </summary>
        public int ApplicationAccountingId { get; set; }

        /// <summary>
        /// Descripción
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Sucursal
        /// </summary>
        public Branch Branch { get; set; }

        /// <summary>
        /// Punto de Venta
        /// </summary>
        public SalePoint SalePoint { get; set; }

        ///// <summary>
        ///// Beneficiario
        ///// </summary>
        //public Individual Beneficiary { get; set; }

        /// <summary>
        /// Naturaleza de Cuenta
        /// </summary>
        public int AccountingNature { get; set; }

        /// <summary>
        /// Descripcion de la naturaleza de Cuenta
        /// </summary>
        public string AccountingNatureDescription { get; set; }

        /// <summary>
        /// Cuenta Contable
        /// </summary>
        public BookAccount BookAccount { get; set; }

        /// <summary>
        /// Número del recibo
        /// </summary>
        public int ReceiptNumber { get; set; }

        /// <summary>
        /// Fecha del recibo
        /// </summary>
        public DateTime? ReceiptDate { get; set; }

        /// <summary>
        /// Identificador del banco
        /// </summary>
        public int BankReconciliationId { get; set; }

        ///// <summary>
        ///// Concepto contable
        ///// </summary>
        //public AccountingConcept AccountingConcept { get; set; }

        /// <summary>
        /// DailyAccountingAnalysisCodes
        /// </summary>
        public List<ApplicationAccountingAnalysis> AccountingAnalysisCodes { get; set; }

        /// <summary>
        /// DailyAccountingCostCenters
        /// </summary>
        public List<ApplicationAccountingCostCenter> AccountingCostCenters { get; set; }
    }
}
