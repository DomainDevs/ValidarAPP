using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Enums;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models.AccountReclassification
{
    /// <summary>
    /// Resultado de Reclasificacion de Cuentas
    /// </summary>
    [DataContract]
    public class AccountReclassificationResult
    {
        /// <summary>
        /// Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Mes
        /// </summary>
        [DataMember]
        public int Month { get; set; }

        /// <summary>
        /// Año
        /// </summary>
        [DataMember]
        public int Year { get; set; }

        /// <summary>
        /// Sucursal
        /// </summary>
        [DataMember]
        public Branch Branch { get; set; }

        /// <summary>
        ///     Naturaleza Contable
        /// </summary>
        [DataMember]
        public AccountingNatures AccountingNature { get; set; }

        /// <summary>
        ///   Importe
        /// </summary>
        [DataMember]
        public Amount Amount { get; set; }

        /// <summary>
        ///   ExchangeRate
        /// </summary>
        [DataMember]
        public ExchangeRate ExchangeRate { get; set; }

        /// <summary>
        ///   Importe Local
        /// </summary>
        [DataMember]
        public Amount LocalAmount { get; set; }

        /// <summary>
        /// Numero de Cuenta Contable Origen
        /// </summary>
        [DataMember]
        public AccountingAccount SourceAccountingAccount { get; set; }
     
        /// <summary>
        /// Numero de Cuenta Contable Destino
        /// </summary>
        [DataMember]
        public AccountingAccount DestinationAccountingAccount { get; set; }
        
        /// <summary>
        /// Analisis
        /// </summary>
        [DataMember]
        public Analysis Analysis { get; set; }

        /// <summary>
        /// Centro de Costo
        /// </summary>
        [DataMember]
        public CostCenter CostCenter { get; set; }




    }
}
