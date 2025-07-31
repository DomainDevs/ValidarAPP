using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Enums;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models
{
    /// <summary>
    ///     Cuenta contable
    /// </summary>
    [DataContract]
    public class AccountingAccount
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
        public AccountingAccountType AccountingAccountType { get; set; }

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
        public Branch Branch { get; set; }

        /// <summary>
        ///     Naturaleza Contable
        /// </summary>
        [DataMember]
        public AccountingNatures AccountingNature { get; set; }

        /// <summary>
        ///     Moneda
        /// </summary>
        [DataMember]
        public Currency Currency { get; set; }

        /// <summary>
        ///     Necesita Analisis
        /// </summary>
        [DataMember]
        public bool RequiresAnalysis { get; set; }

        /// <summary>
        ///     Analisis por defecto para procesos automáticos
        /// </summary>
        [DataMember]
        public Analysis Analysis { get; set; }

        /// <summary>
        ///     Necesita Centro De Costos
        /// </summary>
        [DataMember]
        public bool RequiresCostCenter { get; set; }

        /// <summary>
        ///     Centros de Costos
        /// </summary>
        [DataMember]
        public List<CostCenter> CostCenters { get; set; }

        /// <summary>
        ///     Listado de Conceptos Contables
        /// </summary>
        [DataMember]
        public List<PaymentConcept> AccountingConcepts { get; set; }

        /// <summary>
        ///     Sub Cuentas
        /// </summary>
        [DataMember]
        public List<AccountingAccount> SubAccountingAccounts { get; set; }

        /// <summary>
        ///     Prefixes : Ramos
        /// </summary>
        [DataMember]
        public List<Prefix> Prefixes { get; set; }

        /// <summary>
        ///     AccountingAccountApplication: Tipo de Aplicacion de Cuenta
        /// </summary>
        [DataMember]
        public AccountingAccountApplications AccountingAccountApplication { get; set; }

        /// <summary>
        /// Indicates multicurrency
        /// </summary>
        public bool MultiCurrency { get; set; }

        /// <summary>
        /// Multicurrency Identifier
        /// </summary>
        public int CurrencyId { get; set; }


        /// <summary>
        ///     Requiere Reclasificacion
        /// </summary>
        [DataMember]
        public bool IsReclassify { get; set; }


        /// <summary>
        ///     Número de cuenta para reclasificacion de cuentas
        /// </summary>
        [DataMember]
        public string RecAccounting { get; set; }


        /// <summary>
        ///     Requiere Reevaluo
        /// </summary>
        [DataMember]
        public bool IsRevalue { get; set; }

        /// <summary>
        ///     Número de cuenta para reevalúo positivo
        /// </summary>
        [DataMember]
        public string RevAcountingPos { get; set; }


        /// <summary>
        ///     Número de cuenta para reevalúo Negativo
        /// </summary>
        [DataMember]
        public string RevAcountingNeg { get; set; }
    }
}