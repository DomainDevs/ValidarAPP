using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models
{
    [KnownType("JournalEntryModel")]
    public class JournalEntryModel : LedgerEntryModel
    {
        /// <summary>
        /// Identificador único del modelo
        /// </summary>
        public int JournalEntryId { get; set; }

        /// <summary>
        /// Listado de ítems
        /// </summary>
        public List<JournalEntryItemModel> JournalEntryItems { get; set; }
    }

    [KnownType("JournalEntryItemModel")]
    public class JournalEntryItemModel : LedgerEntryItemModel
    {
    }
}