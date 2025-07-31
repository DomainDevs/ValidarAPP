using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingClosingServices.DTOs
{
    /// <summary>
    ///     Cuenta contable
    /// </summary>
    [DataContract]
    public class AccountingAccountDTO
    {
        /// <summary>
        ///     Identificador único del modelo
        /// </summary>
        [DataMember]
        public int AccountingAccountId { get; set; }

        /// <summary>
        ///     AccountingAccountType: Tipo de Cuenta
        /// </summary>
        [DataMember]
        public AccountingAccountTypeDTO AccountingAccountType { get; set; }

        /// <summary>
        ///     Identificador de la cuenta Padre
        /// </summary>
        [DataMember]
        public int AccountingAccountParentId { get; set; }

        /// <summary>
        ///     Número de cuenta contable
        /// </summary>
        [DataMember]
        public string Number { get; set; }

        /// <summary>
        ///     Descripcion
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        ///     Comments: Observaciones de la cuenta
        /// </summary>
        [DataMember]
        public string Comments { get; set; }

        /// <summary>
        ///     Sucursal
        /// </summary>
        [DataMember]
        public BranchDTO Branch { get; set; }

        /// <summary>
        ///     Naturaleza Contable
        /// </summary>
        [DataMember]
        public int AccountingNature { get; set; }

        /// <summary>
        ///     Moneda
        /// </summary>
        [DataMember]
        public CurrencyDTO Currency { get; set; }

        /// <summary>
        ///     Necesita Analisis
        /// </summary>
        [DataMember]
        public bool RequiresAnalysis { get; set; }

        /// <summary>
        ///     Analisis por defecto para procesos automáticos
        /// </summary>
        [DataMember]
        public AnalysisDTO Analysis { get; set; }

        /// <summary>
        ///     Necesita Centro De Costos
        /// </summary>
        [DataMember]
        public bool RequiresCostCenter { get; set; }

        /// <summary>
        ///     Centros de Costos
        /// </summary>
        [DataMember]
        public List<CostCenterDTO> CostCenters { get; set; }

        /// <summary>
        ///     Listado de Conceptos Contables
        /// </summary>
        [DataMember]
        public List<PaymentConceptDTO> AccountingConcepts { get; set; }

        /// <summary>
        ///     Sub Cuentas
        /// </summary>
        [DataMember]
        public List<AccountingAccountDTO> SubAccountingAccounts { get; set; }

        /// <summary>
        ///     Prefixes : Ramos
        /// </summary>
        [DataMember]
        public List<PrefixDTO> Prefixes { get; set; }

        /// <summary>
        ///     AccountingAccountApplication: Tipo de Aplicacion de Cuenta
        /// </summary>
        [DataMember]
        public int AccountingAccountApplication { get; set; }
    }
}