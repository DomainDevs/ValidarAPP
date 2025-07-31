using System.Runtime.Serialization;


namespace Sistran.Core.Application.GeneralLedgerServices.DTOs.AccountReclassification
{
    /// <summary>
    /// Resultado de Reclasificacion de Cuentas
    /// </summary>
    [DataContract]
    public class AccountReclassificationResultDTO
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
        public BranchDTO Branch { get; set; }

        /// <summary>
        ///     Naturaleza Contable
        /// </summary>
        [DataMember]
        public int AccountingNature { get; set; }

        /// <summary>
        ///   Importe
        /// </summary>
        [DataMember]
        public AmountDTO Amount { get; set; }

        /// <summary>
        ///   ExchangeRate
        /// </summary>
        [DataMember]
        public ExchangeRateDTO ExchangeRate { get; set; }

        /// <summary>
        ///   Importe Local
        /// </summary>
        [DataMember]
        public AmountDTO LocalAmount { get; set; }

        /// <summary>
        /// Numero de Cuenta Contable Origen
        /// </summary>
        [DataMember]
        public AccountingAccountDTO SourceAccountingAccount { get; set; }
     
        /// <summary>
        /// Numero de Cuenta Contable Destino
        /// </summary>
        [DataMember]
        public AccountingAccountDTO DestinationAccountingAccount { get; set; }
        
        /// <summary>
        /// Analisis
        /// </summary>
        [DataMember]
        public AnalysisDTO Analysis { get; set; }

        /// <summary>
        /// Centro de Costo
        /// </summary>
        [DataMember]
        public CostCenterDTO CostCenter { get; set; }




    }
}
