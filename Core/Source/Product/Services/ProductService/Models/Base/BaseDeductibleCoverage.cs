using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.ProductServices.Models.Base
{
    /// <summary>
    /// Deducible producto
    /// </summary>
    /// <seealso cref="Sistran.Core.Application.CommonService.Models.Extension" />
    [DataContract]
    public class BaseDeductibleCoverage : Extension
    {
        /// <summary>
        /// 
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int GroupCoverId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int CoverageId { get; set; }

        /// <summary>
        /// Atributo para la propiedad Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int BeneficiaryTypeId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// Atributo para la propiedad Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///
        /// </summary>
        public bool IsSelected { get; set; }
    }
}
