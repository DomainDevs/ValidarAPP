using Sistran.Core.Application.Extensions;

namespace Sistran.Core.Application.ProductServices.Models.Base
{
    public class BaseInsuredObject : Extension
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? IsMandatory { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? IsSelected { get; set; }
    }
}
