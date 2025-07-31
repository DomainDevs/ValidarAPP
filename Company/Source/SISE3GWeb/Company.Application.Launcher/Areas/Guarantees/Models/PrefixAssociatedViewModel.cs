using System.Collections.Generic;

namespace Sistran.Core.Framework.UIF.Web.Areas.Guarantees.Models
{
    public class PrefixAssociatedViewModel
    {
        /// <summary>javascript:try{external.ExecuteSelectionMenuItem('Stop')}catch(err){UpdateState()}
        /// Id del afianzado
        /// </summary>
        public int ContractorId { get; set; }

        /// <summary>
        /// Nombre afianzado
        /// </summary>
        public string SecureName { get; set; }

        /// <summary>
        /// Número del documento (pagaré, CDT, escritura, etc.) del afianzado.
        /// </summary>
        public int NumberDocument { get; set; }

        /// <summary>
        /// Listado de ramos existentes en la BD, marcando aquellos asociados a la contragarantía. 
        /// </summary>
        public List<DocumentationReceivedListViewModel> DocumentList { get; set; }
    }
}