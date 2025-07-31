using Sistran.Core.Application.Product.Entities;
using Sistran.Core.Application.UnderwritingServices.EEProvider.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Diagnostics;
using Model = Sistran.Core.Application.UnderwritingServices.Models;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs
{
    public class GroupCoverageDAO
    {
        /// <summary>
        /// Obtener lista de grupo de coberturas
        /// </summary>
        /// <param name="productId">Id producto</param>
        /// <returns></returns>
        public List<Model.GroupCoverage> GetGroupCoveragesByProductId(int productId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ProductGroupCover.Properties.ProductId, typeof(ProductGroupCover).Name);
            filter.Equal();
            filter.Constant(productId);

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(ProductGroupCover), filter.GetPredicate()));

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs.GetGroupCoveragesByProductId");
            return ModelAssembler.CreateGroupCoveragesByProducts(businessCollection);
        }

        /// <summary>
        /// Obtener lista de grupo de coberturas por ramo comercial
        /// </summary>
        /// <param name="prefixCd">Id ramo comercial</param>
        /// <returns></returns>
        public List<Model.GroupCoverage> GetGroupCoveragesByPrefixCd(int prefixCode)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ProductGroupCover.Properties.PrefixCode, typeof(ProductGroupCover).Name);
            filter.Equal();
            filter.Constant(prefixCode);

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(ProductGroupCover), filter.GetPredicate()));

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs.GetGroupCoveragesByPrefixCd");
            return ModelAssembler.CreateGroupCoverageByPrefixs(businessCollection);
        }

        /// <summary>
        /// obtiene un Model.GroupCoverage a partir del coverageId, productId y coverGroupId
        /// </summary>
        /// <param name="coverageId">id cobertura</param>
        /// <param name="productId">id producto</param>
        /// <param name="coverGroupId">id grupo coberturas</param>
        /// <returns></returns>
        public static Model.GroupCoverage GetGroupCoverageByCovergaIdProductIdCoverGroupId(int coverageId, int productId, int coverGroupId)
        {
            PrimaryKey key = GroupCoverage.CreatePrimaryKey(coverageId, productId, coverGroupId);

            return ModelAssembler.CreateGroupCoverageByCoverage((GroupCoverage)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key));
        }

        /// <summary>
        /// obtiene un Model.GroupCoverage a partir del coverGroupId
        /// </summary>
        /// <param name="coverGroupId">id grupo de coberturas</param>
        /// <returns></returns>
        public static Model.GroupCoverage GetGroupCoverageByCoverGroupId(int coverGroupId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(GroupCoverage.Properties.CoverGroupId);
            filter.Equal();
            filter.Constant(coverGroupId);

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(GroupCoverage), filter.GetPredicate()));

            return ModelAssembler.CreateGroupCoverageByCoverage((GroupCoverage)businessCollection[0]);
        }

        /// <summary>
        /// Obtener grupo de cobertura
        /// </summary>
        /// <param name="coverGroupId">Id grupo de coberturas</param>
        /// <param name="productId">Id producto</param>
        ///<param name="coverageId">Id cobertura</param>
        /// <returns></returns>
        /// 
        public  Model.GroupCoverage GetGroupCoverageByCoverGroupIdProductIdCoverageId(int coverGroupId,int productId,int coverageId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(GroupCoverage.Properties.CoverGroupId);
            filter.Equal();
            filter.Constant(coverGroupId);
            filter.And();
            filter.Property(GroupCoverage.Properties.ProductId);
            filter.Equal();
            filter.Constant(productId);
            filter.And();
            filter.Property(GroupCoverage.Properties.CoverageId);
            filter.Equal();
            filter.Constant(coverageId);

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(GroupCoverage), filter.GetPredicate()));
            if (businessCollection != null)
            {
                return ModelAssembler.CreateGroupCoverageByCoverage((GroupCoverage)businessCollection[0]);
            }
            else {
                return new Model.GroupCoverage();
            }
        }
    }
}
