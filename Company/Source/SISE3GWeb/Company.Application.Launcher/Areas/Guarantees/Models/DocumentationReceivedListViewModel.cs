namespace Sistran.Core.Framework.UIF.Web.Areas.Guarantees.Models
{
    public class DocumentationReceivedListViewModel
    {
        /// <summary>
        /// Nombre afianzado
        /// </summary>
        public string SecureName { get; set; }

        /// <summary>
        /// Número del documento (pagaré, CDT, escritura, etc.) del afianzado.
        /// </summary>
        public int NumberDocument { get; set; }

        /// <summary>
        /// Si el documento fue recibido
        /// </summary>
        public bool IsReceived { get; set; }
        /// <summary>
        /// Documento
        /// </summary>
        public string Document { get; set; }

    }
}