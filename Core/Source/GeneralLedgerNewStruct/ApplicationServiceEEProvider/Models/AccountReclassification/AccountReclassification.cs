using System.Runtime.Serialization;


namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models.AccountReclassification
{
    /// <summary>
    /// Parametrizacion de Reclasificacion de Cuentas
    /// </summary>
    [DataContract]
    public class AccountReclassification
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
        /// Apertura de Ramo
        /// </summary>
        [DataMember]
        public bool OpeningPrefix { get; set; }

        /// <summary>
        /// Apertura de Sucursal
        /// </summary>
        [DataMember]
        public bool OpeningBranch { get; set; }



    }
}
