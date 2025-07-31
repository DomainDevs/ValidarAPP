namespace Sistran.Core.Framework.UIF.Web.Areas.Product.Models
{
    using Sistran.Company.Application.ModelServices.Enums;
    using Sistran.Core.Application.UnderwritingServices.Models;
    using System.Collections.Generic;

    /// <summary>
    /// Este modelo es para manipular los datos de la pantalla de deducibles por cobertura
    /// </summary>
    public class DeductByCoverProductModelView
    {
        /// <summary>
        /// Identificador
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Almacena el grupo de coberturas
        /// </summary>
        public int GroupCoverId { get; set; }

        //public int InsuredObjectId { get; set; }

        public int CoverageId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int BeneficiaryTypeId { get; set; }

        /// <summary>
        /// atributo que establece si la cobertura esta asociada
        /// </summary>        
        public bool IsSelected { get; set; }

        /// <summary>
        /// 
        /// </summary>        
        public bool IsDefault { get; set; }

        /// <summary>
        /// Atributo para la propiedad Description
        /// </summary>         
        public string Description { get; set; }

        /// <summary>
        /// Estado del objeto
        /// </summary>        
        public StatusTypeService StatusTypeService { get; set; }

    }
}