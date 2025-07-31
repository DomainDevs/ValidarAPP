namespace Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Models
{
    public class ChangeTermViewModel : EndorsementViewModel
    {
        /// <summary>
        /// Dias de Vigencia de la poliza
        /// </summary>
        public int DaysPolicy { get; set; }

        /// <summary>
        /// Vigencia desde
        /// </summary>
        public string CancelationFrom { get; set; }
    }
}