using Sistran.Core.Application.AccountingServices.DTOs.Search;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs.Imputations
{
    [DataContract]
    public class ApplicationAccountingDTO : ApplicationItemDTO
    {
        /// <summary>
        /// Identificador
        /// </summary>        
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Sucursal
        /// </summary>        
        [DataMember]
        public BranchDTO Branch { get; set; }

        /// <summary>
        /// Punto de Venta
        /// </summary>        
        [DataMember]
        public SalePointDTO SalePoint { get; set; }

        /// <summary>
        /// Naturaleza de Cuenta
        /// </summary>        
        [DataMember]
        public int AccountingNature { get; set; }

        /// <summary>
        /// Descripcion de la naturaleza de Cuenta
        /// </summary>
        [DataMember]
        public string AccountingNatureDescription { get; set; }

        /// <summary>
        /// Cuenta Contable
        /// </summary>        
        [DataMember]
        public BookAccountDTO BookAccount { get; set; }

        /// <summary>
        /// Concepto contable
        /// </summary>
        [DataMember]
        public int AccountingConceptId { get; set; }

        /// <summary>
        /// Fecha de recibo
        /// </summary>
        [DataMember]
        public DateTime? ReceiptDate { get; set; }

        /// <summary>
        /// Identificador del banco
        /// </summary>
        [DataMember]
        public int BankReconciliationId { get; set; }

        /// <summary>
        /// Descripción
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// DailyAccountingAnalysisCodes
        /// </summary>        
        [DataMember]
        public List<ApplicationAccountingAnalysisDTO> AccountingAnalysisCodes { get; set; }

        /// <summary>
        /// DailyAccountingCostCenters
        /// </summary>        
        [DataMember]
        public List<ApplicationAccountingCostCenterDTO> AccountingCostCenters { get; set; }
    }
}
