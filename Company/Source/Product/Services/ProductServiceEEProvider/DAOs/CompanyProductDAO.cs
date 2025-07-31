using Sistran.Company.Application.ProductServices.DTOs;
using Sistran.Company.Application.ProductServices.EEProvider.Resources;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.Queries;
using System;
using System.Data;
using PARAMEN = Sistran.Core.Application.Parameters.Entities;
using PRODEN = Sistran.Core.Application.Product.Entities;

namespace Sistran.Company.Application.ProductServices.EEProvider.DAOs
{
    public class CompanyProductDAO
    {

        #region CptProduct
        //public CompanyProduct GetCompanyProductByCoreProduct(PM.Product coreProduct)
        //{
        //    if (coreProduct == null)
        //    {
        //        throw new ArgumentNullException(nameof(coreProduct));
        //    }

        //    PrimaryKey key = CptProduct.CreatePrimaryKey(coreProduct.Id);
        //    CptProduct cptProduct = (CptProduct)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
        //    return ModelAssembler.CreateCptProduct(cptProduct, coreProduct);
        //}

        //public CompanyProduct GetValidDaysByIdProduct(int idProduct)
        //{
        //    PM.Product coreProduct = DelegateService.productServicecore.GetProductById(idProduct);

        //    PrimaryKey key = CptProduct.CreatePrimaryKey(coreProduct.Id);
        //    CptProduct cptProduct = (CptProduct)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

        //    CompanyProduct companyProduct = ModelAssembler.CreateCptProduct(cptProduct, coreProduct);

        //    return companyProduct;
        //}
        #endregion CptProduct
        public SubCoverageDTO GetCompanySubCoverageRiskTypeByProductIdPrefixId(int productId, int prefixId)
        {
            SelectQuery select = new SelectQuery();
            select.AddSelectValue(new SelectValue(new Column(PARAMEN.HardRiskType.Properties.SubCoveredRiskTypeCode, typeof(PARAMEN.HardRiskType).Name)));
            ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
            where.Property(PRODEN.ProductCoverRiskType.Properties.ProductId, typeof(PRODEN.ProductCoverRiskType).Name).Equal().Constant(productId);
            where.And().Property(PARAMEN.HardRiskType.Properties.LineBusinessCode, typeof(PARAMEN.HardRiskType).Name).Equal().Constant(prefixId);
            select.Distinct = true;
            Join join = new Join(new ClassNameTable(typeof(PRODEN.ProductCoverRiskType), typeof(PRODEN.ProductCoverRiskType).Name), new ClassNameTable(typeof(PARAMEN.HardRiskType), typeof(PARAMEN.HardRiskType).Name), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder().Property(PRODEN.ProductCoverRiskType.Properties.CoveredRiskTypeCode, typeof(PRODEN.ProductCoverRiskType).Name).Equal().Property(PARAMEN.HardRiskType.Properties.CoveredRiskTypeCode, typeof(PARAMEN.HardRiskType).Name).GetPredicate());
            select.Table = join;
            select.Where = where.GetPredicate();
            Int16 subCoverageRiskType = -1;
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {
                while (reader.Read())
                {
                    subCoverageRiskType = Convert.ToInt16((int)reader[PARAMEN.HardRiskType.Properties.SubCoveredRiskTypeCode]);
                    break;
                }
            }
            if (subCoverageRiskType != -1)
            {
                return new SubCoverageDTO { Id = subCoverageRiskType };
            }
            else
            {
                throw new Exception(string.Format(Global.ErrorHardRiskType, prefixId.ToString(), productId.ToString()));
            }

        }
    }
}
