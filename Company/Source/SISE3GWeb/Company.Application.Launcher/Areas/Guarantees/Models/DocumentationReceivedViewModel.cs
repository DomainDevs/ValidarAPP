using System.Collections.Generic;

namespace Sistran.Core.Framework.UIF.Web.Areas.Guarantees.Models
{
    public class DocumentationReceivedViewModel
    {
        /// <summary>
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
        /// Documentos parametrizados para el tipo de contragarantía seleccionada
        /// </summary>
        public List<DocumentationReceivedListViewModel> DocumentList { get; set; }
    }
}