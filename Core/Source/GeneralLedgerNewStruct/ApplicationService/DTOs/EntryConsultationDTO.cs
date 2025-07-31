using System;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.GeneralLedgerServices.DTOs
{
    /// <summary>
    ///     DTO para consulta de asientos
    /// </summary>
    [DataContract]
    public class EntryConsultationDTO
    {
        /// <summary>
        ///     Id de asiento
        /// </summary>
        [DataMember]
        public int EntryId { get; set; }

        /// <summary>
        ///     Id de cuenta contable
        /// </summary>
        [DataMember]
        public int AccountingAccountId { get; set; }

        /// <summary>
        ///     Número de cuenta contable
        /// </summary>
        [DataMember]
        public string AccountingAccountNumber { get; set; }

        /// <summary>
        ///     Nombre de cuenta contable
        /// </summary>
        [DataMember]
        public string AccountingAccountName { get; set; }

        /// <summary>
        ///     Id de Moneda
        /// </summary>
        [DataMember]
        public int CurrencyId { get; set; }

        /// <summary>
        ///     Descripción de moneda
        /// </summary>
        [DataMember]
        public string CurrencyDescription { get; set; }

        /// <summary>
        ///     Naturaleza
        /// </summary>
        [DataMember]
        public int AccountingNature { get; set; }

        /// <summary>
        ///     Descripción de la naturaleza
        /// </summary>
        [DataMember]
        public string AccountingNatureDescription { get; set; }

        /// <summary>
        ///     Valor de débito en moneda de emisión
        /// </summary>
        [DataMember]
        public decimal DebitsAmountValue { get; set; }

        /// <summary>
        ///     Valor de débito en moneda local
        /// </summary>
        [DataMember]
        public decimal DebitsAmountLocalValue { get; set; }

        /// <summary>
        ///     Valor de créditos en moneda de emisión
        /// </summary>
        [DataMember]
        public decimal CreditsAmountValue { get; set; }

        /// <summary>
        ///     Valor de créditos en moneda local
        /// </summary>
        [DataMember]
        public decimal CreditsAmountLocalValue { get; set; }

        /// <summary>
        ///     Fecha
        /// </summary>
        [DataMember]
        public string Date { get; set; }

        /// <summary>
        ///     Descripción de cabecera (solo para asiento de diario)
        /// </summary>
        [DataMember]
        public string EntryHeaderDescription { get; set; }

        /// <summary>
        ///     Descripción del asiento
        /// </summary>
        [DataMember]
        public string EntryDescription { get; set; }

        /// <summary>
        ///     Número de asiento
        /// </summary>
        [DataMember]
        public int EntryNumber { get; set; }

        /// <summary>
        ///     Id de sucursal
        /// </summary>
        [DataMember]
        public int BranchId { get; set; }

        /// <summary>
        ///     Descripción de Sucursal
        /// </summary>
        [DataMember]
        public string BranchDescription { get; set; }

        /// <summary>
        ///     Id de usuario
        /// </summary>
        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        ///     Nombre de Usuario
        /// </summary>
        [DataMember]
        public string UserName { get; set; }

        /// <summary>
        ///     Id de cabecera de Asiento, dato usado para asientos de diario
        /// </summary>
        [DataMember]
        public int DailyEntryHeaderId { get; set; }

        /// <summary>
        ///     Estado del asiento
        /// </summary>
        [DataMember]
        public AccountingEntryStatus Status { get; set; }

        /// <summary>
        ///     Descripción del estado
        /// </summary>
        [DataMember]
        public string StatusDescription { get; set; }

        /// <summary>
        ///     Id de destino
        ///     Se utiliza en modal para filtrar registros
        /// </summary>
        [DataMember]
        public int EntryDestinationId { get; set; }

        /// <summary>
        ///     descripción de destino
        ///     Se utiliza en modal para filtrar registros
        /// </summary>
        [DataMember]
        public string EntryDestinationDescription { get; set; }

        /// <summary>
        ///     Id de tipo de comprobante
        ///     Se utiliza en modal para filtrar registros
        /// </summary>
        [DataMember]
        public int AccountingMovementTypeId { get; set; }

        /// <summary>
        ///     descripción de tipo de comprobante
        ///     Se utiliza en modal para filtrar registros
        /// </summary>
        [DataMember]
        public string AccountingMovementTypeDescription { get; set; }
    }

    /// <summary>
    ///     Enum de estado del asiento
    /// </summary>
    [DataContract]
    [Flags]
    public enum AccountingEntryStatus
    {
        /// <summary>
        /// </summary>
        [EnumMember] Active = 1,

        /// <summary>
        /// </summary>
        [EnumMember] Reverted = 2
    }
}