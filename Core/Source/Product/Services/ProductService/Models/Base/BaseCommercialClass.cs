namespace Sistran.Core.Application.ProductServices.Models.Base
{
    using Sistran.Core.Application.Extensions;

    /// <summary>
    /// 
    /// </summary>
    public class BaseCommercialClass : Extension
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsSelected { get; set; }
    }
}
