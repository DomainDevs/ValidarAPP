using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.Person.Models
{
    public class BindPolicyViewModel
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
        /// Sucursal
        /// </summary>
        [Required]
        public int Branch_Cd { get; set; }

        /// <summary>
        /// Ramo comercial
        /// </summary>
        [Required]
        public int Prefix_Cd { get; set; }

        /// <summary>
        /// Número de póliza
        /// </summary>
        [Required]
        public string NumberPolicy { get; set; }

        /// <summary>
        /// Endoso
        /// </summary>
        [Required]
        public int Endorsement { get; set; }

    }
}