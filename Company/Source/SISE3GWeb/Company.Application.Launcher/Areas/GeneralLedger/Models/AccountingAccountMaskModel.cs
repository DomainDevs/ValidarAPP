namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models
{
    public class AccountingAccountMaskModel
    {
        /// <summary>
        /// AccountingAccountMaskId
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// ParameterId
        /// </summary>
        public int ParameterId { get; set; }

        /// <summary>
        /// ParameterDescription
        /// </summary>
        public string ParameterDescription { get; set; }

        /// <summary>
        /// Position
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// Mask
        /// </summary>
        public string Mask { get; set; }

        /// <summary>
        /// ResultId
        /// </summary>
        public int ResultId { get; set; }
    }
}