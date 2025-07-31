using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Models
{
    public class CancellationViewModel : EndorsementViewModel
    {
        /// <summary>
        /// Tipo de cancelación
        /// </summary>
        public int CancellationTypeId { get; set; }
      
    }
}