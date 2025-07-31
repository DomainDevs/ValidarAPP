using System;
using Sistran.Core.Application.Product.Entities;
using Sistran.Core.Application.Quotation.Entities;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;

namespace Sistran.Company.Application.ProductParamServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class ProductRelatedComCoverageView : BusinessView
    {
        public BusinessCollection CoverGroupRiskType
        {
            get
            {
                return this["CoverGroupRiskType"];
            }
        }

        public BusinessCollection ProductGroupCoverageList
        {
            get
            {
                return this["ProductGroupCover"];
            }
        }

        public BusinessCollection GroupCoverageList
        {
            get
            {
                return this["GroupCoverage"];
            }
        }

        public BusinessCollection CoverageList
        {
            get
            {
                return this["Coverage"];
            }
        }
        public BusinessCollection SubLineBusinessList
        {
            get
            {
                return this["SubLineBusiness"];
            }
        }

        public BusinessCollection LineBusinessList
        {
            get
            {
                return this["LineBusiness"];
            }
        }

        public BusinessCollection PerilList
        {
            get
            {
                return this["Peril"];
            }
        }

        public BusinessCollection InsuredObjectList
        {
            get
            {
                return this["InsuredObject"];
            }
        }
        /// <summary>
        /// Obtne objetos del seguro por producto
        /// </summary>
        /// <value>
        /// Objetos del seguro
        /// </value>
        public BusinessCollection GroupInsuredObjectList
        {
            get
            {
                return this["GroupInsuredObject"];
            }
        }

        public BusinessCollection ProductCoveredRiskTypeList
        {
            get
            {
                return this["ProductCoverRiskType"];
            }
        }
        public BusinessCollection ProductCoveragePrvGroup
        {
            get
            {
                return this["CiaGroupCoverage"];
            }
        }

        public BusinessCollection GetCoveragePrvGroupByCiaGroupCoverage(
            CiaGroupCoverage prvProductGroupCover)
        {
            return this.GetObjectsByRelation(
                "CiaGroupCoverageCoverage", prvProductGroupCover, false);
        }

        public BusinessCollection CoveredRiskTypeList
        {
            get
            {
                return this["CoveredRiskType"];
            }
        }

        /// </summary>
        /// <param name="productCoverage">
        /// Objeto de la entidad groupcoverage a partir del que se desea  
        /// obtener el objeto de la entidad groupcoverage asociado.
        /// </param>
        /// <returns>
        /// Objeto de la entidad Coverage.
        /// </returns>
        public BusinessCollection GetGroupCoverageListByProdGroupCoverage(
            ProductGroupCover productGroupCover)
        {
            return this.GetObjectsByRelation(
                "ProductGroupCoverageGroupCoverage", productGroupCover, false);
        }

        /**********************************************************************
        * RELACIÓN : GroupCoverage - Coverage
        **********************************************************************/
        /// <summary>
        /// Obtiene el objeto de la entidad Coverage relacionado a
        ///  un objeto de la entidad ProductCoverage.
        /// </summary>
        /// <param name="productCoverage">
        /// Objeto de la entidad ProductCoverage a partir del que se desea  
        /// obtener el objeto de la entidad Coverage asociado.
        /// </param>
        /// <returns>
        /// Objeto de la entidad Coverage.
        /// </returns>
        public Coverage GetCoverageByGroupCoverage(
            GroupCoverage groupCoverage)
        {
            return (Coverage)this.GetObjectByRelation(
                "GroupCoverageCoverage", groupCoverage, false);
        }
    }
}
