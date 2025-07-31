#region Using

using System.Collections.Generic;
using System.Runtime.Serialization;

#endregion

namespace Sistran.Core.Application.GeneralLedgerServices.DTOs
{
    /// <summary>
    ///     Modelo utilizado para el manejo de Asientos Tipo
    /// </summary>
    [DataContract]
    public class EntryTypeDTO
    {
        /// <summary>
        ///     Identificador único del modelo
        /// </summary>
        [DataMember]
        public int EntryTypeId { get; set; }

        /// <summary>
        ///     Descripción Reducida
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }

        /// <summary>
        ///     Descripción Extendida
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        ///     EntryTypeItems
        /// </summary>
        [DataMember]
        public List<EntryTypeItemDTO> EntryTypeItems { get; set; }

        /// <summary>
        ///     Sub Cuentas
        /// </summary>
        [DataMember]
        public List<AccountingAccountDTO> SubAccountingAccounts { get; set; }
    }
}