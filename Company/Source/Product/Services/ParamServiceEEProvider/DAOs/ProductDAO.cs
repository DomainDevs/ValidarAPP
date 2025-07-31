// -----------------------------------------------------------------------
// <copyright file="ProductDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Cristyan Fernando Ballesteros</author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.ProductParamServices.EEProvider.DAOs
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Sistran.Co.Application.Data;
    using Sistran.Company.Application.ProductParamService.EEProvider;
    using Sistran.Company.Application.ProductParamService.EEProvider.Assemblers;
    using Sistran.Company.Application.ProductParamService.Models;
    using Sistran.Company.Application.ProductParamServices.EEProvider.Assemblers;
    using Sistran.Company.Application.ProductParamServices.EEProvider.Entities.Views;
    using Sistran.Core.Application.Parameters.Entities;
    using Sistran.Core.Application.Temporary.Entities;
    using Sistran.Core.Application.Utilities.Constants;
    //using COMMPARAMETER = Sistran.Core.Application.CommonServices.EEProvider;
    using Sistran.Core.Application.Utilities.DataFacade;
    using Sistran.Core.Application.UtilitiesServicesEEProvider.DAOs;
    // using Sistran.Core.Application.Utilities.DataFacade;
    using Sistran.Core.Framework.BAF;
    using Sistran.Core.Framework.DAF;
    using Sistran.Core.Framework.DAF.Engine;
    using Sistran.Core.Framework.Queries;
    using Sistran.Core.Services.UtilitiesServices.Enums;
    using Sistran.Core.Services.UtilitiesServices.Models;
    using CIAPRODEN = Sistran.Company.Application.Product.Entities;
    using COMMEN = Sistran.Core.Application.Common.Entities;
    using INTEN = Sistran.Company.Application.Integration.Entities;
    using ISSEN = Sistran.Core.Application.Issuance.Entities;
    using PARAMEN = Sistran.Core.Application.Parameters.Entities;
    using PRODEN = Sistran.Core.Application.Product.Entities;
    using QUOEN = Sistran.Core.Application.Quotation.Entities;
    using TP = Sistran.Core.Application.Utilities.Utility;
    using UPEN = Sistran.Core.Application.UniquePerson.Entities;

    /// <summary>
    /// Clase DAO del objeto Product.
    /// </summary>
    public class CiaProductDAO
    {
        /// <summary>
        /// obtiene la lista de productos a partir de 
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public BusinessCollection ListProduct(Predicate filter, string[] sort)
        {
            return new BusinessCollection(
                DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(PRODEN.Product), filter, sort));
        }

        /// <summary>
        /// Obtiene los productos de acuerdo al ramo.
        /// </summary>
        /// <param name="prefixId">Identificador del ramo</param>
        /// <returns>Listado de productos encontrados.</returns>
        public List<CiaParamProduct> GetCiaProductByPrefixId(int prefixId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(PRODEN.Product.Properties.PrefixCode, "p", prefixId);
            IList productList = ListProduct(filter.GetPredicate(), null);
            return ModelAssembler.CreateCiaParamProducts(productList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public List<CiaParamProduct> GetCiaProductsByProduct(CiaParamProduct product)
        {
            int ProductId = 0;
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            List<CiaParamProduct> products = new List<CiaParamProduct>();
            if (product.Prefix != null && product.Prefix.Id > 0)
            {
                filter.PropertyEquals(PRODEN.Product.Properties.PrefixCode, product.Prefix.Id);
            }
            if (product.Description != null && product.Description.Trim() != "")
            {
                Int32.TryParse(product.Description, out ProductId);
                if (ProductId == 0)
                {
                    filter.And().Property(PRODEN.Product.Properties.Description).Like().Constant("%" + product.Description + "%");
                }
                else
                {
                    if (product.Prefix != null && product.Prefix.Id > 0)
                    {
                        filter.And().Property(PRODEN.Product.Properties.ProductId).Equal().Constant(Convert.ToInt32(product.Description));
                    }
                    else
                    {
                        //filter.PropertyEquals(PRODEN.Product.Properties.ProductId, product.Description);
                        filter.Property(PRODEN.Product.Properties.ProductId).Equal().Constant(Convert.ToInt32(product.Description));
                    }

                }
            }

            if (product.CurrentFrom > DateTime.MinValue)
            {
                filter.And();
                filter.Property(PRODEN.Product.Properties.CurrentFrom);
                filter.GreaterEqual();
                filter.Constant(product.CurrentFrom);
            }

            if (product.CurrentTo != null && product.CurrentTo > DateTime.MinValue)
            {
                filter.And();
                filter.Property(PRODEN.Product.Properties.CurrentTo);
                filter.GreaterEqual();
                filter.Constant(product.CurrentTo);
            }
            IList productList = ListProduct(filter.GetPredicate(), null).Skip(0).Take(20).ToList();
            return this.GetCompanyProductData(productList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="products">Listado de productos core</param>
        /// <returns></returns>
        public List<CiaParamProduct> GetCompanyProductData(IList products)
        {
            List<CiaParamProduct> listCiaParamProduct = new List<CiaParamProduct>();
            foreach (PRODEN.Product itemProduct in products)
            {
                CiaParamProduct ciaParamProduct = new CiaParamProduct();
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                BusinessCollection businessCollection = new BusinessCollection();

                List<PRODEN.ProductCurrency> productCurrency = new List<PRODEN.ProductCurrency>();
                INTEN.CoEquivalenceProduct product2G = null;
                List<PRODEN.CoProductPolicyType> productPolicyType = new List<PRODEN.CoProductPolicyType>();
                //List<CIAPRODEN.CptProductBranchAssistanceType> productAssistanceType = new List<CIAPRODEN.CptProductBranchAssistanceType>();
                List<PRODEN.ProductCoverRiskType> productRiskType = new List<PRODEN.ProductCoverRiskType>();

                //PrimaryKey keyCpt = CIAPRODEN.CptProduct.CreatePrimaryKey(itemProduct.ProductId);
                //CIAPRODEN.CptProduct cptProductEntity = (CIAPRODEN.CptProduct)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(keyCpt);

                PrimaryKey keyPrefix = COMMEN.Prefix.CreatePrimaryKey(itemProduct.PrefixCode);
                COMMEN.Prefix productPrefix = (COMMEN.Prefix)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(keyPrefix);

                if (products.Count == 1)
                {
                    //Product Currency
                    filter.Property(PRODEN.ProductCurrency.Properties.ProductId);
                    filter.Equal();
                    filter.Constant(itemProduct.ProductId);
                    businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(PRODEN.ProductCurrency), filter.GetPredicate()));
                    productCurrency = businessCollection.Cast<PRODEN.ProductCurrency>().ToList();

                    //Product Product2G
                    filter = new ObjectCriteriaBuilder();
                    filter.Property(INTEN.CoEquivalenceProduct.Properties.Product3gId);
                    filter.Equal();
                    filter.Constant(itemProduct.ProductId);
                    businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(INTEN.CoEquivalenceProduct), filter.GetPredicate()));
                    product2G = businessCollection.Cast<INTEN.CoEquivalenceProduct>().FirstOrDefault();

                    //Product PolicyType
                    filter = new ObjectCriteriaBuilder();
                    filter.Property(PRODEN.CoProductPolicyType.Properties.ProductId);
                    filter.Equal();
                    filter.Constant(itemProduct.ProductId);
                    businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(PRODEN.CoProductPolicyType), filter.GetPredicate()));
                    productPolicyType = businessCollection.Cast<PRODEN.CoProductPolicyType>().ToList();

                    //Product AssistanceType
                    //filter = new ObjectCriteriaBuilder();
                    //filter.Property(CIAPRODEN.CptProductBranchAssistanceType.Properties.ProductId);
                    //filter.Equal();
                    //filter.Constant(itemProduct.ProductId);
                    //businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CIAPRODEN.CptProductBranchAssistanceType), filter.GetPredicate()));
                    //productAssistanceType = businessCollection.Cast<CIAPRODEN.CptProductBranchAssistanceType>().ToList();
                }

                ciaParamProduct = ModelAssembler.CreateCiaParamProduct(productPrefix, itemProduct, productCurrency, product2G, productPolicyType, productRiskType);
                if (ciaParamProduct.Currency != null && ciaParamProduct.Currency.Count > 0)
                {
                    ciaParamProduct.Currency.ForEach(x=> x.Description = DelegateService.commonServiceCore.GetCurrencies().FirstOrDefault(y=> y.Id == x.Id).Description) ;
                }
                if (products.Count == 1)
                {
                    if (ciaParamProduct.PolicyType != null)
                    {
                        foreach (CiaParamPolicyType itemCiaParamPolicyType in ciaParamProduct.PolicyType)
                        {
                            itemCiaParamPolicyType.Description = GetPolicyTypeDescription(itemCiaParamPolicyType.Id, ciaParamProduct.Prefix.Id);
                        }
                    }
                    ciaParamProduct.FinancialPlan = GetFinancialPlanByProductId(ciaParamProduct.Id);

                    ciaParamProduct.RiskTypes = GetFullRiskByProductId(ciaParamProduct.Id);

                    ciaParamProduct.IsUse = false;// ValidatePolicyByProductId(ciaParamProduct.Id);
                }
                listCiaParamProduct.Add(ciaParamProduct);
            }
            return listCiaParamProduct;
        }

        /// <summary>
        /// Obtiene los productos de acuerdo al ramo.
        /// </summary>
        /// <param name="prefixId">Identificador del ramo</param>
        /// <returns>Listado de productos encontrados.</returns>
        public List<CiaParamProduct2G> GetProduct2gByPrefix(int prefixId)
        {
            BusinessCollection businessCollection = new BusinessCollection();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(CIAPRODEN.CoProduct2g.Properties.PrefixCode, "p", prefixId);

            businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CIAPRODEN.CoProduct2g), filter.GetPredicate()));
            List<CIAPRODEN.CoProduct2g> productList = businessCollection.Cast<CIAPRODEN.CoProduct2g>().ToList();
            return ModelAssembler.CreateCiaParamProducts2G(productList);
        }

        /// <summary>
        /// Obtiene los productos de acuerdo al ramo.
        /// </summary>
        /// <param name="prefixId">Identificador del ramo</param>
        /// <returns>Listado de productos encontrados.</returns>
        //public List<CiaParamAssistanceType> GetCiaAssistanceTypeByPrefix(int prefixId)
        //{
        //    BusinessCollection businessCollection = new BusinessCollection();
        //    ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
        //    filter.PropertyEquals(CIACOMMEN.CptAssistanceType.Properties.PrefixCode, prefixId);
        //    filter.And();
        //    filter.Property(CIACOMMEN.CptAssistanceType.Properties.Enabled);
        //    filter.Equal();
        //    filter.Constant(true);

        //    businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CIACOMMEN.CptAssistanceType), filter.GetPredicate()));
        //    List<CIACOMMEN.CptAssistanceType> assistanceTypeList = businessCollection.Cast<CIACOMMEN.CptAssistanceType>().ToList();
        //    return ModelAssembler.CreateCiaParamAssistanceTypes(assistanceTypeList);
        //}

        public string GetPolicyTypeDescription(int idPolicyTypeId, int prefixId)
        {
            string result = null;
            BusinessCollection businessCollection = new BusinessCollection();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(COMMEN.CoPolicyType.Properties.PolicyTypeCode, idPolicyTypeId);
            filter.And();
            filter.Property(COMMEN.CoPolicyType.Properties.PrefixCode);
            filter.Equal();
            filter.Constant(prefixId);

            businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.CoPolicyType), filter.GetPredicate()));
            COMMEN.CoPolicyType coPolicyType = businessCollection.Cast<COMMEN.CoPolicyType>().FirstOrDefault();
            if (coPolicyType != null)
            {
                result = coPolicyType.Description;
            }
            return result;
        }

        /// <summary>
        /// Validacion si el producto esta en Uso en tablas reales o temporales
        /// </summary>
        /// <param name="ProductId">Id del Producto</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        /// <exception cref="Exception"></exception>
        public bool ValidatePolicyByProductId(int productId, int riskId, int coverId)
        {
            bool validatePolicyId = false;
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(PRODEN.Product.Properties.ProductId);
            filter.Equal();
            filter.Constant(productId);
            try
            {
                validatePolicyId = CheckPolicy(productId, riskId, coverId);
                if (!validatePolicyId)
                {
                    validatePolicyId = CheckTempSubscription(productId, riskId, coverId);//filter.GetPredicate(), false
                }
                return validatePolicyId;

            }
            catch (Exception ex)
            {
                if (ex is BusinessException)
                {
                    throw new BusinessException("Error in ValidatePolicyByProductId", ex);
                }
                throw new Exception(ex.Message, ex);
            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        private bool CheckPolicy(int productId, int riskId, int coverId)
        {
            int result = 0;

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.Risk.Properties.CoveredRiskTypeCode, "r");
            filter.Equal();
            filter.Constant(riskId);
            //filter.PropertyEquals(ISSEN.Risk.Properties.CoveredRiskTypeCode, riskId);
            filter.And();
            filter.Property(ISSEN.Risk.Properties.CoverGroupId, "r");
            filter.Equal();
            filter.Constant(coverId);
            //filter.PropertyEquals(ISSEN.Risk.Properties.CoverGroupId, coverId);
            filter.And();
            filter.Property(ISSEN.Policy.Properties.ProductId, "p");
            filter.Equal();
            filter.Constant(productId);

            SelectQuery selectQuery = new SelectQuery();
            selectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.Policy.Properties.PolicyId, "p")));
            #region Joins
            Join join = new Join(new ClassNameTable(typeof(ISSEN.Policy), "p"), new ClassNameTable(typeof(ISSEN.Endorsement), "e"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(ISSEN.Policy.Properties.PolicyId, "p")
                .Equal()
                .Property(ISSEN.Endorsement.Properties.PolicyId, "e")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(ISSEN.EndorsementRisk), "er"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(ISSEN.Endorsement.Properties.EndorsementId, "e")
                .Equal()
                .Property(ISSEN.EndorsementRisk.Properties.EndorsementId, "er")
                .And()
                .Property(ISSEN.Endorsement.Properties.PolicyId, "e")
                .Equal()
                .Property(ISSEN.EndorsementRisk.Properties.PolicyId, "er")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(ISSEN.Risk), "r"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(ISSEN.EndorsementRisk.Properties.RiskId, "er")
                .Equal()
                .Property(ISSEN.Risk.Properties.RiskId, "r")
                .GetPredicate());

            #endregion Joins
            selectQuery.Where = filter.GetPredicate();
            selectQuery.Table = join;
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                while (reader.Read())
                {
                    result = result + 1;
                }
            }

            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

            //int policyCount = 0;
            //#region Select
            //Function f = new Function(FunctionType.Count);
            //f.AddParameter(new Constant(1, System.Data.DbType.Int32));
            //SelectQuery policySelectQuery = new SelectQuery();
            //policySelectQuery.AddSelectValue(new SelectValue(f));
            //policySelectQuery.Distinct = true;
            //#endregion
            //policySelectQuery.Table = new ClassNameTable(typeof(ISSEN.Policy), "Policy");
            //policySelectQuery.Where = filter;
            //policySelectQuery.GetFirstSelect();
            //using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(policySelectQuery))
            //{
            //    while (reader.Read())
            //    {
            //        if ((int)reader[0] != 0)
            //        {
            //            policyCount++;
            //            break;
            //        }
            //    }
            //}
            //if (policyCount > 0)
            //{
            //    return true;
            //}
            //return false;
        }

        /// <summary>
        /// Verificar si el producto está siendo usado en una cotización temporal.
        /// </summary>
        /// <param name="filter">
        /// Filtro de ProductId.
        /// </param>
        /// <param name="throwException">
        /// True : Si el producto está siendo usado, dispara una excepción.
        /// </param>
        /// <returns>   
        /// Referencia de la transacción si es que el producto está siendo usado, sino 
        /// devuelve la cadena vacía.
        /// </returns>
        private bool CheckTempSubscription(int productId, int riskId, int coverId)//, bool throwException
        {
            int result = 0;

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(TempRisk.Properties.CoveredRiskTypeCode, "tr");
            filter.Equal();
            filter.Constant(riskId);
            filter.And();
            filter.Property(TempRisk.Properties.CoverageGroupId, "tr");
            filter.Equal();
            filter.Constant(coverId);
            filter.And();
            filter.Property(TempSubscription.Properties.ProductId, "ts");
            filter.Equal();
            filter.Constant(productId);

            SelectQuery selectQuery = new SelectQuery();
            selectQuery.AddSelectValue(new SelectValue(new Column(TempSubscription.Properties.TempId, "ts")));
            #region Joins
            Join joinComiss = new Join(new ClassNameTable(typeof(TempSubscription), "ts"), new ClassNameTable(typeof(TempRisk), "tr"), JoinType.Inner);
            joinComiss.Criteria = (new ObjectCriteriaBuilder()
                .Property(TempSubscription.Properties.TempId, "ts")
                .Equal()
                .Property(TempRisk.Properties.TempId, "tr")
                .GetPredicate());
            #endregion Joins
            selectQuery.Where = filter.GetPredicate();
            selectQuery.Table = joinComiss;
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                while (reader.Read())
                {
                    result = result + 1;
                }
            }

            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

            //TempSubscription tempSubscription = (TempSubscription)DataFacadeManager.Instance.GetDataFacade().List(typeof(TempSubscription), filter).FirstOrDefault();
            //if (tempSubscription != null)
            //{

            //    if (throwException)
            //    {
            //        throw new BusinessException("Error in CheckTempSubscription tempId: " + tempSubscription.TempId.ToString());
            //    }
            //    return true;
            //}
            //return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idRiskType"></param>
        /// <returns></returns>
        public string GetRiskTypeDescription(int idRiskType)
        {
            string result = null;
            BusinessCollection businessCollection = new BusinessCollection();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(PARAMEN.CoveredRiskType.Properties.CoveredRiskTypeCode, idRiskType);

            businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(PARAMEN.CoveredRiskType), filter.GetPredicate()));
            PARAMEN.CoveredRiskType coveredRiskType = businessCollection.Cast<PARAMEN.CoveredRiskType>().FirstOrDefault();
            if (coveredRiskType != null)
            {
                result = coveredRiskType.SmallDescription;
            }
            return result;
        }

        public List<CiaParamFinancialPlan> GetFinancialPlanByProductId(int productId)
        {
            List<CiaParamFinancialPlan> listFinancialPlan = new List<CiaParamFinancialPlan>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(PRODEN.ProductFinancialPlan.Properties.ProductId);
            filter.Equal();
            filter.Constant(productId);

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ProductFinancialPlanRelatedEntitiesView view = null;
            try
            {
                view = new ProductFinancialPlanRelatedEntitiesView();
                ViewBuilder builder = new ViewBuilder("ProductFinancialPlanRelatedEntitiesView");
                if (filter != null)
                {
                    builder.Filter = filter.GetPredicate();
                }
                DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
                listFinancialPlan = ModelAssembler.CreateCiaParamFinancialPlans(view.ProductFinancialPlanList.Cast<PRODEN.ProductFinancialPlan>().ToList());
                TP.Parallel.ForEach(listFinancialPlan, financialPlan =>
                {
                    PRODEN.FinancialPlan financialPlanEntity = view.FinancialPlanList?.Cast<PRODEN.FinancialPlan>().ToList()?.FirstOrDefault(x => x.FinancialPlanId == financialPlan.Id);

                    financialPlan.ProductId = productId;
                    financialPlan.Currency = new CiaParamCurrency { Id = (int)view.CurrencyList?.Cast<COMMEN.Currency>().ToList()?.FirstOrDefault(x => x.CurrencyCode == financialPlanEntity?.CurrencyCode)?.CurrencyCode, Description = view.CurrencyList?.Cast<COMMEN.Currency>().ToList()?.FirstOrDefault(x => x.CurrencyCode == financialPlanEntity?.CurrencyCode)?.Description };
                    financialPlan.PaymentMethod = new CiaParamPaymentMethod { Id = (int)view.PaymentMethodList?.Cast<COMMEN.PaymentMethod>().ToList()?.FirstOrDefault(x => x.PaymentMethodCode == financialPlanEntity?.PaymentMethodCode)?.PaymentMethodCode, Description = view.PaymentMethodList?.Cast<COMMEN.PaymentMethod>().ToList()?.FirstOrDefault(x => x.PaymentMethodCode == financialPlanEntity?.PaymentMethodCode)?.Description };
                    financialPlan.PaymentSchedule = new CiaParamPaymentSchedule { Id = (int)view.PaymentScheduleList?.Cast<PRODEN.PaymentSchedule>().ToList()?.FirstOrDefault(x => x.PaymentScheduleId == financialPlanEntity?.PaymentScheduleId)?.PaymentScheduleId, Description = view.PaymentScheduleList?.Cast<PRODEN.PaymentSchedule>().ToList()?.FirstOrDefault(x => x.PaymentScheduleId == financialPlanEntity?.PaymentScheduleId)?.Description };
                    financialPlan.IsSelected = true;
                });
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.ProductParamServices.EEProvider.DAOs.GetFinancialPlanByCurrencyIdByProductId");
                return listFinancialPlan;
            }
            catch (Exception exc)
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.ProductParamServices.EEProvider.DAOs.GetFinancialPlanByCurrencyIdByProductId");

                throw new BusinessException("excepcion en  GetFinancialPlanByCurrencyIdByProductId", exc);
            }
        }

        public List<CiaParamRiskType> GetFullRiskByProductId(int productId)
        {
            ProductRelatedComCoverageView view = new ProductRelatedComCoverageView();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(PRODEN.ProductGroupCover.Properties.ProductId, typeof(PRODEN.ProductGroupCover).Name);
            filter.Equal();
            filter.Constant(productId);
            ViewBuilder builder = new ViewBuilder("ProductRelatedComCoverageView");
            builder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
            ObjectCriteriaBuilder filterRiskType = new ObjectCriteriaBuilder();
            filterRiskType.Property(PRODEN.ProductCoverRiskType.Properties.ProductId, typeof(PRODEN.ProductCoverRiskType).Name);
            filterRiskType.Equal();
            filterRiskType.Constant(productId);
            List<PRODEN.ProductCoverRiskType> listProductCoverRiskType =
                DataFacadeManager.Instance.GetDataFacade().List<PRODEN.ProductCoverRiskType>(filterRiskType.GetPredicate()).Cast<PRODEN.ProductCoverRiskType>().ToList();

            List<CiaParamRiskType> listCiaParamCoverage = new List<CiaParamRiskType>();
            if (listProductCoverRiskType != null && view.ProductCoveredRiskTypeList.Count < 1)
            {
                listCiaParamCoverage = ModelAssembler.CreateCiaParamRiskTypes(listProductCoverRiskType);
                return listCiaParamCoverage;
            }
            if (view.ProductCoveredRiskTypeList != null && view.ProductCoveredRiskTypeList.Count > 0)
            {
                //List<PRODEN.ProductCoverRiskType> listProductCoverRiskType = view.ProductCoveredRiskTypeList.Cast<PRODEN.ProductCoverRiskType>().ToList();
                List<QUOEN.Coverage> listCoverage = view.CoverageList.Cast<QUOEN.Coverage>().ToList();
                List<PARAMEN.CoveredRiskType> listCoveredRiskType = view.CoveredRiskTypeList.Cast<PARAMEN.CoveredRiskType>().ToList();
                List<PRODEN.GroupCoverage> listGroupCoverage = view.GroupCoverageList.Cast<PRODEN.GroupCoverage>().OrderBy(o => o.CoverNum).ToList();
                List<PRODEN.GroupInsuredObject> listGroupInsuredObject = view.GroupInsuredObjectList.Cast<PRODEN.GroupInsuredObject>().ToList();
                List<QUOEN.InsuredObject> listInsuredObject = view.InsuredObjectList.Cast<QUOEN.InsuredObject>().ToList();
                List<COMMEN.LineBusiness> listLineBusiness = view.LineBusinessList.Cast<COMMEN.LineBusiness>().ToList();
                List<QUOEN.Peril> listPeril = view.PerilList.Cast<QUOEN.Peril>().ToList();
                List<PRODEN.ProductGroupCover> listProductGroupCover = view.ProductGroupCoverageList.Cast<PRODEN.ProductGroupCover>().ToList();
                List<PRODEN.CoverGroupRiskType> coverGroupRiskType = view.CoverGroupRiskType.Cast<PRODEN.CoverGroupRiskType>().ToList();
                List<COMMEN.SubLineBusiness> listSubLineBusiness = view.SubLineBusinessList.Cast<COMMEN.SubLineBusiness>().ToList();
                List<PRODEN.CiaGroupCoverage> listCiaGroupCoverage = view.ProductCoveragePrvGroup.Cast<PRODEN.CiaGroupCoverage>().ToList();//ProductCoveragePrvGroup();//SubLineBusinessList.Cast<COMMEN.SubLineBusiness>().ToList();

                foreach (PRODEN.ProductCoverRiskType itemProductCoverRiskType in listProductCoverRiskType)
                {
                    CiaParamRiskType ciaParamRiskType = ModelAssembler.CreateCiaParamRiskType(itemProductCoverRiskType);
                    ciaParamRiskType.Description = listCoveredRiskType?.FirstOrDefault(x => x.CoveredRiskTypeCode == ciaParamRiskType?.Id)?.SmallDescription;

                    //Grupos de coberturas
                    List<PRODEN.ProductGroupCover> itemProductGroupCovers = listProductGroupCover?.Where(x => x.CoveredRiskTypeCode == ciaParamRiskType.Id)?.ToList();
                    ciaParamRiskType.GroupCoverages = new List<CiaParamCoverage>();
                    ciaParamRiskType.GroupCoverages = ModelAssembler.CreateCiaParamGroupCoverages(itemProductGroupCovers);

                    foreach (CiaParamCoverage itemCiaParamCoverage in ciaParamRiskType.GroupCoverages)
                    {
                        itemCiaParamCoverage.Description = coverGroupRiskType.First(x => x.CoverageGroupCode == itemCiaParamCoverage.Id && x.CoveredRiskTypeCode == itemCiaParamCoverage.RiskTypeId).Description;
                        //Objetos del seguro
                        itemCiaParamCoverage.InsuredObjects = new List<CiaParamInsuredObject>();
                        itemCiaParamCoverage.InsuredObjects = ModelAssembler.CreateCiaParamInsuredObjects(listGroupInsuredObject?.Where(x => x.CoverageGroupCode == itemCiaParamCoverage.Id)?.ToList());
                        List<CiaParamInsuredObject> listInsuredObjectsValidate = new List<CiaParamInsuredObject>();
                        if (itemCiaParamCoverage.InsuredObjects != null && itemCiaParamCoverage.InsuredObjects.Count > 0)
                        {
                            foreach (CiaParamInsuredObject itemCiaParamInsuredObject in itemCiaParamCoverage.InsuredObjects)
                            {
                                var validateInsured = listInsuredObjectsValidate?.Where(x => x.Id == itemCiaParamInsuredObject.Id).FirstOrDefault();
                                if (validateInsured == null)
                                {
                                    listInsuredObjectsValidate.Add(itemCiaParamInsuredObject);
                                }
                            }
                        }
                        itemCiaParamCoverage.InsuredObjects = listInsuredObjectsValidate;

                        foreach (CiaParamInsuredObject itemCiaParamInsuredObject in itemCiaParamCoverage.InsuredObjects)
                        {
                            itemCiaParamInsuredObject.Description = listInsuredObject?.Where(x => x.InsuredObjectId == itemCiaParamInsuredObject.Id)?.FirstOrDefault()?.Description;
                            //Coverturas del objeto del seguro
                            itemCiaParamInsuredObject.Coverages = new List<CiaParamCoverages>();
                            List<QUOEN.Coverage> listInsuredObjectCoverage = listCoverage?.Where(x => x.InsuredObjectId == itemCiaParamInsuredObject.Id)?.ToList();
                            List<PRODEN.GroupCoverage> listCoverages = listGroupCoverage?.Where(x => listInsuredObjectCoverage.Any(y => y.CoverageId == x.CoverageId && x.CoverGroupId == itemCiaParamCoverage.Id))?.ToList();
                            foreach (PRODEN.GroupCoverage itemGroupCoverage in listCoverages)
                            {
                                var thisGroupCoverage = listCiaGroupCoverage;

                                CiaParamCoverages ciaParamCoverages = new CiaParamCoverages();
                                QUOEN.Coverage coverage = view.GetCoverageByGroupCoverage(itemGroupCoverage);
                                ciaParamCoverages.Coverage = new CiaParamGroupCoverage();
                                ciaParamCoverages.Coverage = ModelAssembler.CreateCiaParamFullCoverage(itemGroupCoverage, coverage);
                                ciaParamCoverages.Coverage.LineBusinessDescription = listLineBusiness?.Where(x => x?.LineBusinessCode == ciaParamCoverages?.Coverage?.LineBusinessId)?.FirstOrDefault()?.Description;
                                PRODEN.CiaGroupCoverage element = null;

                                element = thisGroupCoverage.Where(x => x?.CoverageId == itemGroupCoverage.CoverageId && x?.CoverGroupId == itemGroupCoverage.CoverGroupId)?.FirstOrDefault();

                                if (element != null)
                                {
                                    ciaParamCoverages.Coverage.IsPremiumMin = element?.IsPremiumMin == 1;
                                    ciaParamCoverages.Coverage.NoCalculate = element?.NoCalculate == 1;
                                }
                                ciaParamCoverages.Coverage.SubLineBusinessDescription = listSubLineBusiness?.Where(x => x?.LineBusinessCode == ciaParamCoverages?.Coverage?.LineBusinessId && x?.SubLineBusinessCode == ciaParamCoverages?.Coverage?.SubLineBusinessId)?.FirstOrDefault()?.Description;

                                //Coberturas aliadas
                                ciaParamCoverages.CoverageAllied = this.GetCoveragesAlliedByCoverageId(itemGroupCoverage.CoverageId);
                                itemCiaParamInsuredObject.Coverages.Add(ciaParamCoverages);
                            }

                        }
                    }
                    listCiaParamCoverage.Add(ciaParamRiskType);
                }

            }
            return listCiaParamCoverage;
        }

        public List<CiaParamGroupCoverage> GetCoveragesAlliedByCoverageId(int coverageId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(QUOEN.AllyCoverage.Properties.CoverageId, typeof(QUOEN.AllyCoverage).Name);
            filter.Equal();
            filter.Constant(coverageId);

            CompanyCoverageAlliedView view = new CompanyCoverageAlliedView();
            ViewBuilder builder = new ViewBuilder("CompanyCoverageAlliedView");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            List<CiaParamGroupCoverage> coverages = null;
            List<QUOEN.Coverage> listCoverages = view.Coverages.Cast<QUOEN.Coverage>().ToList();
            if (listCoverages.Count > 0)
            {
                coverages = ModelAssembler.CreateCiaParamGroupCoverages(listCoverages);

                List<QUOEN.AllyCoverage> coverageAllied = view.AllyCoverages.Cast<QUOEN.AllyCoverage>().ToList();
                foreach (CiaParamGroupCoverage itemCiaParamGroupCoverage in coverages)
                {
                    itemCiaParamGroupCoverage.SublimitPercentage = coverageAllied.First(x => x.AllyCoverageId == itemCiaParamGroupCoverage.Id).CoveragePercentage;
                }
            }
            return coverages;
        }

        /// <summary>
        /// Creates the copy product.
        /// </summary>
        /// <param name="copyProduct">The copy product.</param>
        /// <returns></returns>
        public int CreateCopyProduct(CiaParamCopyProduct copyProduct)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            NameValue[] parameters = new NameValue[11];
            parameters[0] = new NameValue("PRODUCT_ID", copyProduct.Id);
            parameters[1] = new NameValue("DECRIPTION", copyProduct.Description);
            parameters[2] = new NameValue("DECRIPTION_SMALL", copyProduct.SmallDescription);
            parameters[3] = new NameValue("COPY_GROUP_COVERAGES", copyProduct.CopyGroupCoverages);
            parameters[4] = new NameValue("COPY_PAYMENTPLAN", copyProduct.CopyPaymentPlan);
            parameters[5] = new NameValue("COPY_RULESET", copyProduct.CopyRuleSet);
            parameters[6] = new NameValue("COPY_PRINTINGFORMES", copyProduct.CopyPrintingFormes);
            parameters[7] = new NameValue("COPY_AGENT", copyProduct.CopyAgent);
            parameters[8] = new NameValue("COPY_LIMITRC", copyProduct.CopyLimitRC);
            parameters[9] = new NameValue("COPY_SCRIPT", copyProduct.CopyScript);
            parameters[10] = new NameValue("COPY_ACTIVITYRISK", copyProduct.CopyActivityRisk);

            object result;

            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                result = dynamicDataAccess.ExecuteSPScalar("PROD.COPYPRODUCT", parameters);
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.ProductServices.EEProvider.DAOs.CreateCopyProduct");

            return Convert.ToInt32(result);
        }

        public List<CiaParamFinancialPlan> GetPaymentScheduleByCurrencies(List<int> currencies)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            if (currencies != null && currencies.Count > 0)
            {
                filter.Property("CurrencyCode", typeof(PRODEN.FinancialPlan).Name);
                filter.In();
                filter.ListValue();
                foreach (int item in currencies)
                {
                    filter.Constant(item);
                }
                filter.EndList();
            }

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            CompanyFinancialPlanRelatedEntitiesView view = null;
            try
            {
                view = new CompanyFinancialPlanRelatedEntitiesView();
                ViewBuilder builder = new ViewBuilder("CompanyFinancialPlanRelatedEntitiesView");
                if (filter != null)
                {
                    builder.Filter = filter.GetPredicate();
                }
                DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
                List<CiaParamFinancialPlan> financialPlanList = ModelAssembler.CreateCiaParamListFinancialPlans(view.FinancialPlanList.Cast<PRODEN.FinancialPlan>().ToList());
                TP.Parallel.ForEach(financialPlanList, financialPlan =>
                {
                    financialPlan.Currency.Description = view.CurrencyList?.Cast<COMMEN.Currency>().ToList()?.FirstOrDefault(x => x.CurrencyCode == financialPlan.Currency.Id)?.Description;
                    financialPlan.PaymentMethod.Description = view.PaymentMethodList?.Cast<COMMEN.PaymentMethod>().ToList()?.FirstOrDefault(x => x.PaymentMethodCode == financialPlan?.PaymentMethod?.Id)?.Description;
                    financialPlan.PaymentSchedule.Description = view.PaymentScheduleList?.Cast<PRODEN.PaymentSchedule>().ToList()?.FirstOrDefault(x => x.PaymentScheduleId == financialPlan?.PaymentSchedule?.Id)?.Description;
                });
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.ProductServices.EEProvider.DAOs.GetPaymentSchudeleByFilter");
                return financialPlanList;
            }
            catch (Exception exc)
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.ProductServices.EEProvider.DAOs.GetPaymentSchudeleByFilter");

                throw new BusinessException("excepcion en  GetPaymentSchudeleByFilter", exc);
            }
        }

        public List<CiaParamBeneficiaryType> GetBeneficiaryType()
        {
            BusinessCollection businessCollection = new BusinessCollection();
            businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(QUOEN.BeneficiaryType)));
            List<QUOEN.BeneficiaryType> beneficiaryType = businessCollection.Cast<QUOEN.BeneficiaryType>().ToList();
            return ModelAssembler.CreateCiaParamBeneficiaryTypes(beneficiaryType);
        }

        /// <summary>
        /// Obtener deducibles por producto grupo de cobertura y cobertura (trael loas asignado y no asignados)
        /// </summary>
        /// <param name="productId">Id producto</param>
        /// <param name="coverGroupId">Id cover Group</param>
        /// <param name="coverageId">Id coverage</param>
        /// <returns>Endosos</returns>
        public List<CiaParamDeductiblesCoverage> GetDeductiblesByProductId(int productId, int coverGroupId, int coverageId, int beneficiaryTypeCd, int lineBusinnessCd)
        {
            NameValue[] parameters = new NameValue[5];

            parameters[0] = new NameValue("PRODUCT_ID", productId);
            parameters[1] = new NameValue("COVER_GROUP_ID", coverGroupId);
            parameters[2] = new NameValue("COVERAGE_ID", coverageId);
            parameters[3] = new NameValue("BENEFICIARY_TYPE_CD", beneficiaryTypeCd);
            parameters[4] = new NameValue("LINE_BUSINESS_CD", lineBusinnessCd);

            DataTable result;

            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                result = dynamicDataAccess.ExecuteSPDataTable("PROD.GET_DEDUCTIBLES_BY_PRODUCT_ID", parameters);
            }
            if (result == null || result.Rows.Count == 0)
            {
                parameters[0] = new NameValue("PRODUCT_ID", 0);
                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    result = dynamicDataAccess.ExecuteSPDataTable("PROD.GET_DEDUCTIBLES_BY_PRODUCT_ID", parameters);
                }
            }
            if (result != null && result.Rows.Count > 0)
            {
                List<CiaParamDeductiblesCoverage> deductibles = new List<CiaParamDeductiblesCoverage>();

                foreach (DataRow arrayItem in result.Rows)
                {
                    deductibles.Add(new CiaParamDeductiblesCoverage
                    {
                        IsSelected = Convert.ToBoolean(arrayItem[0]),
                        ProductId = productId,
                        GroupCoverId = Convert.ToInt32(arrayItem[2]),
                        CoverageId = Convert.ToInt32(arrayItem[3]),
                        Id = Convert.ToInt32(arrayItem[4]),
                        BeneficiaryTypeId = Convert.ToInt32(arrayItem[5]),
                        IsDefault = Convert.ToBoolean(arrayItem[6]),
                        Description = arrayItem[7].ToString(),
                    });
                }

                return deductibles;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public CiaParamProduct CreateProduct(CiaParamProduct product)
        {
            product.Id = this.GetIdProduct();

            //Core Product
            PRODEN.Product ProductEntities = EntityAssembler.CreateCoreEntityProduct(product);
            DataFacadeManager.Instance.GetDataFacade().InsertObject(ProductEntities);

            //Asigna el Identificador del producto creado en base de datos
            product.Id = ProductEntities.ProductId;

            //Currency Product
            if (product.Currency != null && product.Currency.Count > 0)
            {
                foreach (CiaParamCurrency currency in product.Currency)
                {
                    PRODEN.ProductCurrency item = new PRODEN.ProductCurrency(product.Id, currency.Id);
                    DataFacadeManager.Instance.GetDataFacade().InsertObject(item);
                }
            }

            //Financial Plan Product
            if (product.FinancialPlan != null && product.FinancialPlan.Count > 0)
            {
                foreach (CiaParamFinancialPlan productFinancialPlan in product.FinancialPlan)
                {
                    PRODEN.ProductFinancialPlan item = EntityAssembler.CreateEntityFinancialPlan(productFinancialPlan, product.Id);
                    DataFacadeManager.Instance.GetDataFacade().InsertObject(item);
                }
            }

            //Porduct Policy Type
            if (product.PolicyType != null && product.PolicyType.Count > 0)
            {
                foreach (CiaParamPolicyType policyType in product.PolicyType)
                {
                    PRODEN.CoProductPolicyType item = new PRODEN.CoProductPolicyType(product.Id, product.Prefix.Id, policyType.Id)
                    {
                        IsDefault = policyType.IsDefault
                    };
                    DataFacadeManager.Instance.GetDataFacade().InsertObject(item);
                }
            }

            //Product Risk Types
            if (product.RiskTypes != null && product.RiskTypes.Count > 0)
            {
                foreach (CiaParamRiskType coveredRisk in product.RiskTypes)
                {
                    PRODEN.ProductCoverRiskType item = EntityAssembler.CreateEntityRiskType(coveredRisk, product.Id);
                    DataFacadeManager.Instance.GetDataFacade().InsertObject(item);

                    //Product Risk Types - Group Coverages
                    if (coveredRisk.GroupCoverages != null && coveredRisk.GroupCoverages.Count > 0)
                    {
                        foreach (CiaParamCoverage itemCiaParamCoverage in coveredRisk.GroupCoverages)
                        {
                            PRODEN.ProductGroupCover itemProductGroupCover = EntityAssembler.CreateEntityGroupCover(itemCiaParamCoverage, product.Id, product.Prefix.Id);
                            DataFacadeManager.Instance.GetDataFacade().InsertObject(itemProductGroupCover);

                            //Product Risk Types - Group Coverages - Insured Objects
                            if (itemCiaParamCoverage.InsuredObjects != null && itemCiaParamCoverage.InsuredObjects.Count > 0)
                            {
                                foreach (CiaParamInsuredObject itemCiaParamInsuredObject in itemCiaParamCoverage.InsuredObjects)
                                {
                                    PRODEN.GroupInsuredObject itemGroupInsuredObject = EntityAssembler.CreateGroupInsuredObject(itemCiaParamInsuredObject, product.Id, coveredRisk.Id, itemCiaParamCoverage.Id);
                                    DataFacadeManager.Instance.GetDataFacade().InsertObject(itemGroupInsuredObject);

                                    //Product Risk Types - Group Coverages - Insured Objects - Coverages

                                    if (itemCiaParamInsuredObject.Coverages != null && itemCiaParamInsuredObject.Coverages.Count > 0)
                                    {
                                        foreach (CiaParamCoverages itemCiaParamCoverages in itemCiaParamInsuredObject.Coverages)
                                        {
                                            PRODEN.GroupCoverage itemGroupCoverage = EntityAssembler.CreateCoverage(itemCiaParamCoverages.Coverage, product.Id, coveredRisk.Id, itemCiaParamCoverage.Id);
                                            DataFacadeManager.Instance.GetDataFacade().InsertObject(itemGroupCoverage);
                                            CIAPRODEN.CiaGroupCoverage coCiaGroupCoverage = EntityAssembler.CreateCiaGroupCoverage(itemCiaParamCoverages.Coverage, product.Id, itemCiaParamCoverage.Id);
                                            DataFacadeManager.Instance.GetDataFacade().InsertObject(coCiaGroupCoverage);

                                            //Product Risk Types - Group Coverages - Insured Objects - Coverages - Deductibles

                                            if (itemCiaParamCoverages.Coverage.DeductiblesCoverage != null && itemCiaParamCoverages.Coverage.DeductiblesCoverage != null && itemCiaParamCoverages.Coverage.DeductiblesCoverage.Count > 0)
                                            {
                                                foreach (CiaParamDeductiblesCoverage itemCiaParamDeductiblesCoverage in itemCiaParamCoverages.Coverage.DeductiblesCoverage)
                                                {
                                                    PRODEN.CoProductCoverageDeductible coProductCoverageDeductible = new PRODEN.CoProductCoverageDeductible(product.Id, itemCiaParamCoverage.Id, itemCiaParamCoverages.Coverage.Id, itemCiaParamDeductiblesCoverage.Id, itemCiaParamDeductiblesCoverage.BeneficiaryTypeId);
                                                    coProductCoverageDeductible.IsDefault = itemCiaParamDeductiblesCoverage.IsDefault;
                                                    DataFacadeManager.Instance.GetDataFacade().InsertObject(coProductCoverageDeductible);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            #region cia product 
            ////Company Product
            //CIAPRODEN.CptProduct cptProduct = EntityAssembler.CreateCiaEntityProduct(product);
            //DataFacadeManager.Instance.GetDataFacade().InsertObject(cptProduct);


            ////Tipo asistencia
            //if (product.AssistanceType != null && product.AssistanceType.Count > 0)
            //{
            //    foreach (CiaParamAssistanceType assistanceType in product.AssistanceType)
            //    {
            //        CIAPRODEN.CptProductBranchAssistanceType item = EntityAssembler.CreateAssistanceType(assistanceType, product.Id, product.Prefix.Id);
            //        DataFacadeManager.Instance.GetDataFacade().InsertObject(item);
            //    }
            //}

            //Product - Product 2G
            INTEN.CoEquivalenceProduct coEquivalenceProduct = new INTEN.CoEquivalenceProduct(product.Id, product.Product2G.Id);
            DataFacadeManager.Instance.GetDataFacade().InsertObject(coEquivalenceProduct);

            #endregion cia product

            product.Description = product.Id.ToString();

            PrimaryKey key = COMMEN.Parameter.CreatePrimaryKey(25);
            COMMEN.Parameter parameter = (COMMEN.Parameter)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
            parameter.NumberParameter = product.Id;
            DataFacadeManager.Instance.GetDataFacade().UpdateObject(parameter);
            return GetCiaProductsByProduct(product).FirstOrDefault();
        }

        /// <summary>
        /// Retorna el valor de la proxima key de la tabla Product
        /// </summary>
        /// <returns>Key dispobible</returns>
        public int GetIdProduct()
        {
            const int parameterId = 25;
            Sistran.Core.Application.CommonService.Models.Parameter parameter = DelegateService.commonServiceCore.GetParameterByParameterId(parameterId);
            int maxProductId = (int)parameter.NumberParameter;

            maxProductId++;
            return maxProductId;
        }

        /// <summary>
        /// Retorna el valor de la proxima key de la tabla ProductForm
        /// </summary>
        /// <returns>Key dispobible</returns>
        public int GetIdProductForm()
        {
            PrimaryKey key = COMMEN.Parameter.CreatePrimaryKey(2134);
            COMMEN.Parameter parameter = (COMMEN.Parameter)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
            int maxProductFormId = (int)parameter.NumberParameter;
            maxProductFormId++;
            return maxProductFormId;
        }



        public int UpdateCoreProduct(CiaParamProduct ciaParamProduct)
        {
            //PrimaryKey key = PRODEN.Product.CreatePrimaryKey(ciaParamProduct.Id);
            //PRODEN.Product ProductEntities = (PRODEN.Product)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(PRODEN.Product.Properties.ProductId, typeof(PRODEN.Product).Name)
            .Equal()
            .Constant(ciaParamProduct.Id);
            PRODEN.Product ProductEntities = DataFacadeManager.Instance.GetDataFacade().List(typeof(PRODEN.Product), filter.GetPredicate()).Cast<PRODEN.Product>().FirstOrDefault();

            ProductEntities.PrefixCode = ciaParamProduct.Prefix.Id;
            ProductEntities.AdditionalCommissionPercentage = ciaParamProduct.AdditDisCommissPercentage;
            ProductEntities.IsGreen = ciaParamProduct.IsGreen;
            ProductEntities.Description = ciaParamProduct.Description;
            ProductEntities.SmallDescription = ciaParamProduct.SmallDescription;
            ProductEntities.IncCommAdjustFactorPercentage = ciaParamProduct.IncrementCommisionAdjustFactorPercentage;
            ProductEntities.DecCommAdjustFactorPercentage = ciaParamProduct.DecrementCommisionAdjustFactorPercentage;
            ProductEntities.PreRuleSetId = ciaParamProduct.PreRuleSetId;
            ProductEntities.RuleSetId = ciaParamProduct.RuleSetId;
            ProductEntities.ScriptId = ciaParamProduct.ScriptId;
            ProductEntities.AdditCommissPercentage = ciaParamProduct.AdditionalCommissionPercentage;
            ProductEntities.StandardCommissionPercentage = ciaParamProduct.StandardCommissionPercentage;
            ProductEntities.StdDiscountCommPercentage = ciaParamProduct.StdDiscountCommPercentage;
            ProductEntities.SurchargeCommissionPercentage = ciaParamProduct.SurchargeCommissionPercentage;
            ProductEntities.IsCollective = ciaParamProduct.IsCollective;
            ProductEntities.IsRequest = ciaParamProduct.IsRequest;
            ProductEntities.IsFlatRate = ciaParamProduct.IsFlatRate;
            ProductEntities.CurrentFrom = ciaParamProduct.CurrentFrom;
            ProductEntities.CurrentTo = ciaParamProduct.CurrentTo;
            ProductEntities.Version = ProductEntities.Version == null ? 1 : ProductEntities.Version + 1;
            ProductEntities.CalculateMinPremium = ciaParamProduct.CalculateMinPremium;
            ProductEntities.IsInteractive = ciaParamProduct.IsInteractive;
            ProductEntities.IsMassive = ciaParamProduct.IsMassive;
            DataFacadeManager.Instance.GetDataFacade().UpdateObject(ProductEntities);
            return ProductEntities.ProductId;
        }

        public bool DeleteFinancialPlans(List<CiaParamFinancialPlan> listCiaParamFinancialPlanDeleted, int productId)
        {
            try
            {
                foreach (CiaParamFinancialPlan itemCiaParamFinancialPlan in listCiaParamFinancialPlanDeleted)
                {
                    PrimaryKey key = PRODEN.ProductFinancialPlan.CreatePrimaryKey(productId, itemCiaParamFinancialPlan.Id);
                    PRODEN.ProductFinancialPlan ProductFinancialPlanEntities = (PRODEN.ProductFinancialPlan)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                    DataFacadeManager.Instance.GetDataFacade().DeleteObject(ProductFinancialPlanEntities);
                }
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool SaveFinancialPlans(List<CiaParamFinancialPlan> listCiaParamFinancialPlanAdded, List<CiaParamFinancialPlan> listCiaParamFinancialPlanUpdate, int productId)
        {
            try
            {
                foreach (CiaParamFinancialPlan itemCiaParamFinancialPlan in listCiaParamFinancialPlanAdded)
                {
                    PRODEN.ProductFinancialPlan item = EntityAssembler.CreateEntityFinancialPlan(itemCiaParamFinancialPlan, productId);
                    DataFacadeManager.Instance.GetDataFacade().InsertObject(item);
                }
                foreach (CiaParamFinancialPlan itemCiaParamFinancialPlan in listCiaParamFinancialPlanUpdate)
                {
                    PrimaryKey key = PRODEN.ProductFinancialPlan.CreatePrimaryKey(productId, itemCiaParamFinancialPlan.Id);
                    PRODEN.ProductFinancialPlan ProductFinancialPlanEntities = (PRODEN.ProductFinancialPlan)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                    ProductFinancialPlanEntities.IsDefault = itemCiaParamFinancialPlan.IsDefault;
                    DataFacadeManager.Instance.GetDataFacade().UpdateObject(ProductFinancialPlanEntities);
                }
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool DeleteCurrencies(List<CiaParamCurrency> listCiaParamCurrencyDeleted, int productId)
        {
            try
            {
                foreach (CiaParamCurrency itemCiaParamCurrency in listCiaParamCurrencyDeleted)
                {
                    PrimaryKey key = PRODEN.ProductCurrency.CreatePrimaryKey(productId, itemCiaParamCurrency.Id);
                    PRODEN.ProductCurrency ProductCurrencyEntities = (PRODEN.ProductCurrency)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                    DataFacadeManager.Instance.GetDataFacade().DeleteObject(ProductCurrencyEntities);
                }
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
        public bool SaveCurrencies(List<CiaParamCurrency> listCiaParamCurrencyAdded, List<CiaParamCurrency> listCiaParamCurrencyUpdate, int productId)
        {
            try
            {
                foreach (CiaParamCurrency itemCiaParamCurrency in listCiaParamCurrencyAdded)
                {
                    PRODEN.ProductCurrency item = new PRODEN.ProductCurrency(productId, itemCiaParamCurrency.Id);
                    item.DecimalQuantity = itemCiaParamCurrency.DecimalQuantity;
                    DataFacadeManager.Instance.GetDataFacade().InsertObject(item);
                }
                foreach (CiaParamCurrency itemCiaParamCurrency in listCiaParamCurrencyUpdate)
                {
                    PrimaryKey key = PRODEN.ProductCurrency.CreatePrimaryKey(productId, itemCiaParamCurrency.Id);
                    PRODEN.ProductCurrency ProductCurrencyEntities = (PRODEN.ProductCurrency)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                    ProductCurrencyEntities.DecimalQuantity = itemCiaParamCurrency.DecimalQuantity;
                    DataFacadeManager.Instance.GetDataFacade().UpdateObject(ProductCurrencyEntities);
                }
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool SavePolicyTypes(List<CiaParamPolicyType> listCiaParamPolicyTypeDelete, List<CiaParamPolicyType> listCiaParamPolicyTypeAdded, List<CiaParamPolicyType> listCiaParamPolicyTypeUpdate, int productId, int prefixId)
        {
            try
            {
                foreach (CiaParamPolicyType itemCiaParamPolicyType in listCiaParamPolicyTypeDelete)
                {
                    PrimaryKey key = PRODEN.CoProductPolicyType.CreatePrimaryKey(productId, prefixId, itemCiaParamPolicyType.Id);
                    PRODEN.CoProductPolicyType CoProductPolicyTypeEntities = (PRODEN.CoProductPolicyType)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                    DataFacadeManager.Instance.GetDataFacade().DeleteObject(CoProductPolicyTypeEntities);
                }
                foreach (CiaParamPolicyType itemCiaParamPolicyType in listCiaParamPolicyTypeAdded)
                {
                    PRODEN.CoProductPolicyType item = new PRODEN.CoProductPolicyType(productId, prefixId, itemCiaParamPolicyType.Id)
                    {
                        IsDefault = itemCiaParamPolicyType.IsDefault
                    };
                    DataFacadeManager.Instance.GetDataFacade().InsertObject(item);
                }
                foreach (CiaParamPolicyType itemCiaParamPolicyType in listCiaParamPolicyTypeUpdate)
                {
                    PrimaryKey key = PRODEN.CoProductPolicyType.CreatePrimaryKey(productId, prefixId, itemCiaParamPolicyType.Id);
                    PRODEN.CoProductPolicyType CoProductPolicyTypeEntities = (PRODEN.CoProductPolicyType)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                    CoProductPolicyTypeEntities.IsDefault = itemCiaParamPolicyType.IsDefault;
                    DataFacadeManager.Instance.GetDataFacade().UpdateObject(CoProductPolicyTypeEntities);
                }
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        //public int UpdateCiaProduct(CiaParamProduct ciaParamProduct)
        //{
        //    PrimaryKey key = CIAPRODEN.CptProduct.CreatePrimaryKey(ciaParamProduct.Id);
        //    CIAPRODEN.CptProduct ProductEntities = (CIAPRODEN.CptProduct)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

        //    ProductEntities.IsPoliticalProduct = ciaParamProduct.IsPolitical;
        //    ProductEntities.IncentiveAmount = ciaParamProduct.IncentiveAmount;
        //    ProductEntities.IsEnabled = ciaParamProduct.IsEnabled;
        //    ProductEntities.IsScore = ciaParamProduct.IsScore;
        //    ProductEntities.IsFine = ciaParamProduct.IsFine;
        //    ProductEntities.IsFasecolda = ciaParamProduct.IsFasecolda;
        //    ProductEntities.ValidDaysTempPolicy = ciaParamProduct.ValidDaysTempPolicy;
        //    ProductEntities.ValidDaysTempQuote = ciaParamProduct.ValidDaysTempQuote;
        //    ProductEntities.IsRcAdditional = ciaParamProduct.IsRcAdditional;

        //    DataFacadeManager.Instance.GetDataFacade().UpdateObject(ProductEntities);
        //    return ProductEntities.ProductId;
        //}

        //public bool SaveAssistanceTypes(List<CiaParamAssistanceType> listCiaParamAssistanceTypeDelete, List<CiaParamAssistanceType> listCiaParamAssistanceTypeAdded, List<CiaParamAssistanceType> listCiaParamAssistanceTypeUpdate, int productId, int prefixId)
        //{
        //    try
        //    {
        //        foreach (CiaParamAssistanceType itemCiaParamAssistanceType in listCiaParamAssistanceTypeDelete)
        //        {
        //            PrimaryKey key = CIAPRODEN.CptProductBranchAssistanceType.CreatePrimaryKey(productId, prefixId, itemCiaParamAssistanceType.AssistanceId);
        //            CIAPRODEN.CptProductBranchAssistanceType CptProductBranchAssistanceTypeEntities = (CIAPRODEN.CptProductBranchAssistanceType)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
        //            DataFacadeManager.Instance.GetDataFacade().DeleteObject(CptProductBranchAssistanceTypeEntities);
        //        }
        //        foreach (CiaParamAssistanceType itemCiaParamAssistanceType in listCiaParamAssistanceTypeAdded)
        //        {
        //            CIAPRODEN.CptProductBranchAssistanceType item = EntityAssembler.CreateAssistanceType(itemCiaParamAssistanceType, productId, prefixId);
        //            DataFacadeManager.Instance.GetDataFacade().InsertObject(item);
        //        }
        //        foreach (CiaParamAssistanceType itemCiaParamAssistanceType in listCiaParamAssistanceTypeUpdate)
        //        {
        //            PrimaryKey key = CIAPRODEN.CptProductBranchAssistanceType.CreatePrimaryKey(productId, prefixId, itemCiaParamAssistanceType.AssistanceId);
        //            CIAPRODEN.CptProductBranchAssistanceType CptProductBranchAssistanceTypeEntities = (CIAPRODEN.CptProductBranchAssistanceType)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
        //            DataFacadeManager.Instance.GetDataFacade().UpdateObject(CptProductBranchAssistanceTypeEntities);
        //        }
        //        return true;
        //    }
        //    catch (Exception)
        //    {

        //        return false;
        //    }
        //}

        public bool SaveProduct2G(int productId, int product2gId)
        {
            try
            {
                BusinessCollection businessCollection = new BusinessCollection();
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.PropertyEquals(INTEN.CoEquivalenceProduct.Properties.Product3gId, productId);

                DataFacadeManager.Instance.GetDataFacade().DeleteObjects(typeof(INTEN.CoEquivalenceProduct), filter.GetPredicate());

                INTEN.CoEquivalenceProduct coEquivalenceProduct = new INTEN.CoEquivalenceProduct(productId, product2gId);
                DataFacadeManager.Instance.GetDataFacade().InsertObject(coEquivalenceProduct);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool DeleteProductRisk(int productId, int riskId)
        {
            try
            {
                PrimaryKey key = PRODEN.ProductCoverRiskType.CreatePrimaryKey(productId, riskId);
                PRODEN.ProductCoverRiskType ObjectEntities = (PRODEN.ProductCoverRiskType)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                DataFacadeManager.Instance.GetDataFacade().DeleteObject(ObjectEntities);

                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool DeleteProductGroupCoverage(int productId, int coverGroupId)
        {
            try
            {
                PrimaryKey key = PRODEN.ProductGroupCover.CreatePrimaryKey(productId, coverGroupId);
                PRODEN.ProductGroupCover ObjectEntities = (PRODEN.ProductGroupCover)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                DataFacadeManager.Instance.GetDataFacade().DeleteObject(ObjectEntities);

                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool DeleteInsuredObject(int productId, int coverGroupId, int insuredObjectId)
        {
            try
            {
                BusinessCollection businessCollection = new BusinessCollection();
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.PropertyEquals(PRODEN.GroupInsuredObject.Properties.ProductId, productId);
                filter.And();
                filter.Property(PRODEN.GroupInsuredObject.Properties.CoverageGroupCode);
                filter.Equal();
                filter.Constant(coverGroupId);
                filter.And();
                filter.Property(PRODEN.GroupInsuredObject.Properties.InsuredObject);
                filter.Equal();
                filter.Constant(insuredObjectId);

                DataFacadeManager.Instance.GetDataFacade().DeleteObjects(typeof(PRODEN.GroupInsuredObject), filter.GetPredicate());

                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool DeleteProductGroupCoverForms(int productId, int coverGroupId)
        {
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.PropertyEquals(PRODEN.ProductForm.Properties.ProductId, productId);
                filter.And();
                filter.PropertyEquals(PRODEN.ProductForm.Properties.CoverGroupId, coverGroupId);
                DataFacadeManager.Instance.GetDataFacade().Delete<PRODEN.ProductForm>(filter.GetPredicate());

                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool DeleteProductGroupCoverage(int productId, int coverGroupId, int coverageId)
        {
            try
            {
                PrimaryKey key = PRODEN.GroupCoverage.CreatePrimaryKey(coverageId, productId, coverGroupId);
                PRODEN.GroupCoverage ObjectEntities = (PRODEN.GroupCoverage)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                DataFacadeManager.Instance.GetDataFacade().DeleteObject(ObjectEntities);

                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool DeleteProductCoverageDeductibles(int productId, int coverGroupId, int coverageId)
        {
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.PropertyEquals(PRODEN.CoProductCoverageDeductible.Properties.ProductId, productId);
                filter.And();
                filter.PropertyEquals(PRODEN.CoProductCoverageDeductible.Properties.CoverGroupId, coverGroupId);
                filter.And();
                filter.PropertyEquals(PRODEN.CoProductCoverageDeductible.Properties.CoverageId, coverageId);
                DataFacadeManager.Instance.GetDataFacade().Delete<PRODEN.CoProductCoverageDeductible>(filter.GetPredicate());

                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool DeleteProductCoverageDeductible(int productId, int coverGroupId, int coverageId, int deductId, int beneficiaryTypeCode)
        {
            try
            {
                PrimaryKey key = PRODEN.CoProductCoverageDeductible.CreatePrimaryKey(productId, coverGroupId, coverageId, deductId, beneficiaryTypeCode);
                PRODEN.CoProductCoverageDeductible ObjectEntities = (PRODEN.CoProductCoverageDeductible)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                DataFacadeManager.Instance.GetDataFacade().DeleteObject(ObjectEntities);

                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool InsertProductRisk(int productId, int riskId, int maxRiskQuantity, int? ruleSetId, int? preRuleSetId, int? scriptId)
        {
            try
            {
                PRODEN.ProductCoverRiskType itemProductCoverRiskType = new PRODEN.ProductCoverRiskType(productId, riskId)
                {
                    MaxRiskQuantity = maxRiskQuantity,
                    RuleSetId = ruleSetId,
                    PreRuleSetId = preRuleSetId,
                    ScriptId = scriptId
                };
                DataFacadeManager.Instance.GetDataFacade().InsertObject(itemProductCoverRiskType);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool InsertProductGroupCoverage(int coverGroupId, int productId, int? prefixId, int riskId, string description)
        {
            try
            {
                PRODEN.ProductGroupCover itemProductGroupCover = new PRODEN.ProductGroupCover(productId, coverGroupId)
                {
                    CoverGroupId = coverGroupId,
                    SmallDescription = description,
                    PrefixCode = prefixId,
                    CoveredRiskTypeCode = riskId
                };
                DataFacadeManager.Instance.GetDataFacade().InsertObject(itemProductGroupCover);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool InsertProductInsuredObject(int insuredObjectId, int productId, int? riskId, int groupCoverageId, bool isMandatory, bool isSelected)
        {
            try
            {
                PRODEN.GroupInsuredObject itemGroupInsuredObject = new PRODEN.GroupInsuredObject(productId, groupCoverageId, insuredObjectId)
                {
                    IsMandatory = isMandatory,
                    IsSelected = isSelected,
                    CoveredRiskTypeCode = riskId
                };
                DataFacadeManager.Instance.GetDataFacade().InsertObject(itemGroupInsuredObject);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool InsertProductCoverage(CiaParamCoverages itemCiaParamCoverages, int productId, int riskId, int groupCoverageId)
        {
            try
            {
                PRODEN.GroupCoverage itemGroupCoverage = EntityAssembler.CreateCoverage(itemCiaParamCoverages.Coverage, productId, riskId, groupCoverageId);
                DataFacadeManager.Instance.GetDataFacade().InsertObject(itemGroupCoverage);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool InsertProductDeductible(int deductId, int beneficiaryTypeId, int productId, int coverageId, int groupCoverageId, bool isDefault)
        {
            try
            {
                PRODEN.CoProductCoverageDeductible coProductCoverageDeductible = new PRODEN.CoProductCoverageDeductible(productId, groupCoverageId, coverageId, deductId, beneficiaryTypeId);
                coProductCoverageDeductible.IsDefault = isDefault;
                DataFacadeManager.Instance.GetDataFacade().InsertObject(coProductCoverageDeductible);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool UpdateProductRisk(int productId, int riskId, int maxRiskQuantity, int? ruleSetId, int? preRuleSetId, int? scriptId)
        {
            try
            {
                PrimaryKey key = PRODEN.ProductCoverRiskType.CreatePrimaryKey(productId, riskId);
                PRODEN.ProductCoverRiskType ObjectEntities = (PRODEN.ProductCoverRiskType)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                ObjectEntities.MaxRiskQuantity = maxRiskQuantity;
                ObjectEntities.RuleSetId = ruleSetId;
                ObjectEntities.PreRuleSetId = preRuleSetId;
                ObjectEntities.ScriptId = scriptId;
                DataFacadeManager.Instance.GetDataFacade().UpdateObject(ObjectEntities);

                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool UpdateProductRiskPremium(int coverageId, int productId, int coverGroupId, bool isPremium, bool noCalculate)
        {
            try
            {
                PrimaryKey key = PRODEN.CiaGroupCoverage.CreatePrimaryKey(coverageId, productId, coverGroupId);//ProductCoverRiskType.CreatePrimaryKey(productId, riskId);
                PRODEN.CiaGroupCoverage ObjectEntities = (PRODEN.CiaGroupCoverage)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                if (ObjectEntities != null)
                {
                    ObjectEntities.IsPremiumMin = !isPremium ? 0 : 1;
                    ObjectEntities.NoCalculate = !noCalculate ? 0 : 1;
                    DataFacadeManager.Instance.GetDataFacade().UpdateObject(ObjectEntities);
                }
                else
                {
                    ObjectEntities = new PRODEN.CiaGroupCoverage(coverageId, productId, coverGroupId)
                    {
                        IsPremiumMin = !isPremium ? 0 : 1,
                        NoCalculate = !noCalculate ? 0 : 1
                    };
                    DataFacadeManager.Instance.GetDataFacade().InsertObject(ObjectEntities);
                }
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool UpdateProductGroupCoverage(int coverGroupId, int productId, int? prefixId, int riskId, string description)
        {
            try
            {
                PrimaryKey key = PRODEN.ProductGroupCover.CreatePrimaryKey(productId, coverGroupId);
                PRODEN.ProductGroupCover ObjectEntities = (PRODEN.ProductGroupCover)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

                ObjectEntities.SmallDescription = description;
                ObjectEntities.PrefixCode = prefixId;
                ObjectEntities.CoveredRiskTypeCode = riskId;

                DataFacadeManager.Instance.GetDataFacade().UpdateObject(ObjectEntities);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool UpdateProductInsuredObject(int insuredObjectId, int productId, int? riskId, int groupCoverageId, bool isMandatory, bool isSelected)
        {
            try
            {
                BusinessCollection businessCollection = new BusinessCollection();
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.PropertyEquals(PRODEN.GroupInsuredObject.Properties.ProductId, productId);
                filter.And();
                filter.Property(PRODEN.GroupInsuredObject.Properties.CoverageGroupCode);
                filter.Equal();
                filter.Constant(groupCoverageId);
                filter.And();
                filter.Property(PRODEN.GroupInsuredObject.Properties.InsuredObject);
                filter.Equal();
                filter.Constant(insuredObjectId);

                businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(PRODEN.GroupInsuredObject), filter.GetPredicate()));
                if (businessCollection.Count > 1)
                {
                    DataFacadeManager.Instance.GetDataFacade().DeleteObjects(typeof(PRODEN.GroupInsuredObject), filter.GetPredicate());
                    this.InsertProductInsuredObject(insuredObjectId, productId, riskId, groupCoverageId, isMandatory, isSelected);
                }
                else
                {
                    PrimaryKey key = PRODEN.GroupInsuredObject.CreatePrimaryKey(productId, groupCoverageId, insuredObjectId);
                    PRODEN.GroupInsuredObject ObjectEntities = (PRODEN.GroupInsuredObject)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

                    ObjectEntities.IsMandatory = isMandatory;
                    ObjectEntities.IsSelected = isSelected;
                    ObjectEntities.CoveredRiskTypeCode = riskId;

                    DataFacadeManager.Instance.GetDataFacade().UpdateObject(ObjectEntities);
                }
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool UpdateProductCoverage(CiaParamCoverages itemCiaParamCoverages, int productId, int riskId, int groupCoverageId)
        {
            try
            {
                PrimaryKey key = PRODEN.GroupCoverage.CreatePrimaryKey(itemCiaParamCoverages.Coverage.Id, productId, groupCoverageId);
                PRODEN.GroupCoverage ObjectEntities = (PRODEN.GroupCoverage)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                ObjectEntities.IsMandatory = itemCiaParamCoverages.Coverage.IsMandatory;
                ObjectEntities.IsSelected = itemCiaParamCoverages.Coverage.IsSelected;
                ObjectEntities.CoverNum = itemCiaParamCoverages.Coverage.Number;
                ObjectEntities.RuleSetId = itemCiaParamCoverages.Coverage.RuleSetId;
                ObjectEntities.PosRuleSetId = itemCiaParamCoverages.Coverage.PosRuleSetId;
                ObjectEntities.ScriptId = itemCiaParamCoverages.Coverage.ScriptId;
                ObjectEntities.MainCoverageId = itemCiaParamCoverages.Coverage.MainCoverageId;
                ObjectEntities.CoveredRiskTypeCode = riskId;

                DataFacadeManager.Instance.GetDataFacade().UpdateObject(ObjectEntities);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool UpdateProductDeductible(int deductId, int beneficiaryTypeId, int productId, int coverageId, int groupCoverageId, bool isDefault)
        {
            try
            {
                PrimaryKey key = PRODEN.CoProductCoverageDeductible.CreatePrimaryKey(productId, groupCoverageId, coverageId, deductId, beneficiaryTypeId);
                PRODEN.CoProductCoverageDeductible ObjectEntities = (PRODEN.CoProductCoverageDeductible)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

                ObjectEntities.IsDefault = isDefault;

                DataFacadeManager.Instance.GetDataFacade().UpdateObject(ObjectEntities);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        /// <summary>
        /// Genera archivo de Excel de los Prod
        /// </summary>
        public String GenerateFileToProducts(string fileName)
        {
            try
            {
                FileDAO companyFileDao = new FileDAO();
                FileProcessValue fileProcessValue = new FileProcessValue();
                fileProcessValue.Key1 = (int)FileProcessType.ParametrizationProducts;

                File file = companyFileDao.GetFileByFileProcessValue(fileProcessValue);
                if (file == null)
                {
                    throw new BusinessException(" La parametrización del archivo no existe");
                }

                if (!file.IsEnabled)
                {
                    throw new BusinessException(" El archivo no esta habilitado");
                }

                file.Name = fileName;
                List<Row> rows = new List<Row>();


                SelectQuery selectQuery = new SelectQuery();
                selectQuery.AddSelectValue(new SelectValue(new Column(PRODEN.Product.Properties.ProductId, "p")));
                selectQuery.AddSelectValue(new SelectValue(new Column(PRODEN.Product.Properties.Description, "p")));
                selectQuery.AddSelectValue(new SelectValue(new Column(PRODEN.Product.Properties.StdDiscountCommPercentage, "p")));
                //selectQuery.AddSelectValue(new SelectValue(new Column(CIAPRODEN.CptProduct.Properties.IsPoliticalProduct, "pc")));
                selectQuery.AddSelectValue(new SelectValue(new Column(COMMEN.Prefix.Properties.Description, "pf")));
                selectQuery.AddSelectValue(new SelectValue(new Column(PRODEN.Product.Properties.CurrentFrom, "p")));
                selectQuery.AddSelectValue(new SelectValue(new Column(PRODEN.Product.Properties.IsRequest, "p")));
                selectQuery.AddSelectValue(new SelectValue(new Column(COMMEN.CoPolicyType.Properties.Description, "cp")));
                #region Joins
                Join join = new Join(new ClassNameTable(typeof(COMMEN.Prefix), "pf"), new ClassNameTable(typeof(PRODEN.Product), "p"), JoinType.Inner);
                join.Criteria = (new ObjectCriteriaBuilder()
                    .Property(COMMEN.Prefix.Properties.PrefixCode, "pf")
                    .Equal()
                    .Property(PRODEN.Product.Properties.PrefixCode, "p")
                    .GetPredicate());

                //join = new Join(join, new ClassNameTable(typeof(CIAPRODEN.CptProduct), "pc"), JoinType.Inner);
                //join.Criteria = (new ObjectCriteriaBuilder()
                //    .Property(PRODEN.Product.Properties.ProductId, "p")
                join = new Join(join, new ClassNameTable(typeof(PRODEN.CoProductPolicyType), "cpt"), JoinType.Inner);
                join.Criteria = (new ObjectCriteriaBuilder()
                    .Property(PRODEN.Product.Properties.ProductId, "p")
                    .Equal()
                    .Property(PRODEN.CoProductPolicyType.Properties.ProductId, "cpt")
                    //.And()
                    //    .Property(PRODEN.CoProductPolicyType.Properties.IsDefault, "cpt")
                    //    .Equal()
                    //    .Constant(1)
                    .GetPredicate());
                join = new Join(join, new ClassNameTable(typeof(COMMEN.CoPolicyType), "cp"), JoinType.Inner);
                join.Criteria = (new ObjectCriteriaBuilder()
                    .Property(PRODEN.CoProductPolicyType.Properties.PrefixCode, "cpt")
                    .Equal()
                    .Property(COMMEN.CoPolicyType.Properties.PrefixCode, "cp")
                    .And()
                        .Property(PRODEN.CoProductPolicyType.Properties.PolicyTypeCode, "cpt")
                        .Equal()
                        .Property(COMMEN.CoPolicyType.Properties.PolicyTypeCode, "cp")
                    .GetPredicate());

                #endregion Joins
                selectQuery.Table = join;
                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
                {
                    while (reader.Read())
                    {
                        List<Field> fields = file.Templates[0].Rows[0].Fields.Select(p => new Field()
                        {
                            ColumnSpan = p.ColumnSpan,
                            Description = p.Description,
                            FieldType = p.FieldType,
                            Id = p.Id,
                            IsEnabled = p.IsEnabled,
                            IsMandatory = p.IsMandatory,
                            Order = p.Order,
                            RowPosition = p.RowPosition,
                            SmallDescription = p.SmallDescription
                        }).ToList();

                        if (fields.Count < 4)
                        {
                            throw new BusinessException(" Error al generar el archivo");
                        }

                        fields[0].Value = Convert.ToInt32(reader["ProductId"]).ToString();
                        fields[1].Value = (string)reader[1];
                        fields[2].Value = (string)reader[3];
                        fields[3].Value = string.Format("{0:0.00}", Convert.ToDecimal(reader[2]));
                        //fields[4].Value = (bool)reader["IsPoliticalProduct"] ? "SI" : "NO";
                        //fields[4].Value = reader["CurrentFrom"].ToString();
                        //fields[5].Value = (bool)reader["IsRequest"] ? "SI" : "NO";

                        rows.Add(new Row
                        {
                            Fields = fields
                        });
                    }
                }

                file.Templates[0].Rows = rows;
                file.Name = fileName + "_" + DateTime.Now.ToString("dd_MM_yyyy");

                return companyFileDao.GenerateFile(file);
            }
            catch (Exception ex)
            {
                throw new BusinessException(" Error al generar el archivo");
            }
        }


        /// <summary>
        /// generar Archivo del Producto
        /// </summary>
        /// <param name="id">Identificador del producto</param>
        /// <param name="fileName">Nombre Archivo</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public string GenerateFileToProduct(int id, string fileName)
        {
            //FileDAO companyFileDao = new FileDAO();
            List<Task> taskRule = new List<Task>();
            CiaParamProduct product = GetCiaProductsByProduct(new CiaParamProduct { Description = id.ToString() }).FirstOrDefault(); ;
            #region reglas
            List<Core.Application.RulesScriptsServices.Models.RuleSet> RuleSetPre = null;
            List<Core.Application.RulesScriptsServices.Models.RuleSet> RuleSets = null;
            List<Core.Application.RulesScriptsServices.Models.Script> Scripts = null;
            List<Core.Application.RulesScriptsServices.Models.RuleSet> RuleSetRiskPre = null;
            List<Core.Application.RulesScriptsServices.Models.RuleSet> RuleSetRisk = null;
            List<Core.Application.RulesScriptsServices.Models.Script> ScriptRisk = null;
            List<Core.Application.RulesScriptsServices.Models.RuleSet> RuleSetCoveragePre = null;
            List<Core.Application.RulesScriptsServices.Models.RuleSet> RuleSetCoverage = null;
            List<Core.Application.RulesScriptsServices.Models.Script> ScriptCoverage = null;
            Core.Application.RulesScriptsServices.Models.ScriptComposite scriptsComp = null;
            if (product.RuleSetId != null)
            {
                taskRule.Add(TP.Task.Run(() =>
                {
                    List<int> lstids = new List<int>();
                    lstids.Add(product.RuleSetId.Value);
                    RuleSets = DelegateService.ruleServiceCore.GetRuleSetByIds(lstids);
                    DataFacadeManager.Dispose();
                }));
            }
            if (product.PreRuleSetId != null)
            {
                taskRule.Add(TP.Task.Run(() =>
                {
                    List<int> lstids = new List<int>();
                    lstids.Add(product.PreRuleSetId.Value);
                    RuleSetPre = DelegateService.ruleServiceCore.GetRuleSetByIds(lstids);
                    DataFacadeManager.Dispose();
                }));
            }
            if (product.ScriptId != null)
            {
                taskRule.Add(TP.Task.Run(() =>
                {
                    List<int> lstids = new List<int>();
                    lstids.Add(product.ScriptId.Value);
                    Scripts = DelegateService.scriptServiceCore.GetScriptsByIds(lstids);
                    DataFacadeManager.Dispose();
                }));
            }
            if (product.RiskTypes?.FirstOrDefault()?.ScriptId != null)
            {
                taskRule.Add(TP.Task.Run(() =>
                {
                    List<int> lstids = new List<int>();
                    lstids.Add(product.RiskTypes.FirstOrDefault().ScriptId.Value);
                    ScriptRisk = DelegateService.scriptServiceCore.GetScriptsByIds(lstids);
                    DataFacadeManager.Dispose();
                }));
            }
            if (product.RiskTypes?.FirstOrDefault()?.PreRuleSetId != null)
            {
                taskRule.Add(TP.Task.Run(() =>
                {
                    List<int> lstids = new List<int>();
                    lstids.Add(product.RiskTypes.FirstOrDefault().PreRuleSetId.Value);
                    RuleSetRiskPre = DelegateService.ruleServiceCore.GetRuleSetByIds(lstids);
                    DataFacadeManager.Dispose();
                }));
            }
            if (product.RiskTypes?.FirstOrDefault()?.RuleSetId != null)
            {
                taskRule.Add(TP.Task.Run(() =>
                {
                    List<int> lstids = new List<int>();
                    lstids.Add(product.RiskTypes.FirstOrDefault().RuleSetId.Value);
                    RuleSetRisk = DelegateService.ruleServiceCore.GetRuleSetByIds(lstids);
                    DataFacadeManager.Dispose();
                }));
            }
            if (product.RiskTypes[0].GroupCoverages != null && product.RiskTypes[0].GroupCoverages.Count > 0)
            {
                ConcurrentBag<int> lstids = new ConcurrentBag<int>();
                ConcurrentBag<int> Riskids = new ConcurrentBag<int>();
                ConcurrentBag<int> Coverageids = new ConcurrentBag<int>();
                TP.Parallel.ForEach(product.RiskTypes[0].GroupCoverages, groupCoverage =>
                {
                    TP.Parallel.ForEach(groupCoverage.InsuredObjects, insuredObject =>
                    {
                        TP.Parallel.ForEach(insuredObject.Coverages, coverage =>
                        {
                            if (coverage.Coverage.ScriptId.HasValue)
                            {
                                lstids.Add(coverage.Coverage.ScriptId.Value);
                            }
                            if (coverage.Coverage.RuleSetId.HasValue)
                            {
                                Riskids.Add(coverage.Coverage.RuleSetId.Value);
                            }
                            if (coverage.Coverage.PosRuleSetId.HasValue)
                            {
                                Coverageids.Add(coverage.Coverage.PosRuleSetId.Value);
                            }
                        });
                    });
                });
                taskRule.Add(TP.Task.Run(() =>
                {
                    ScriptCoverage = DelegateService.scriptServiceCore.GetScriptsByIds(lstids.ToList());
                    DataFacadeManager.Dispose();
                }));
                taskRule.Add(TP.Task.Run(() =>
                {
                    RuleSetCoveragePre = DelegateService.ruleServiceCore.GetRuleSetByIds(Riskids.ToList());
                    DataFacadeManager.Dispose();
                }));
                taskRule.Add(TP.Task.Run(() =>
                {
                    RuleSetCoverage = DelegateService.ruleServiceCore.GetRuleSetByIds(Coverageids.ToList());
                    DataFacadeManager.Dispose();
                }));
            }
            if (product.RiskTypes?.FirstOrDefault()?.ScriptId != null)
            {
                int scripId = product.RiskTypes.First().ScriptId == null ? (int)0 : (int)product.RiskTypes.First().ScriptId;
                taskRule.Add(TP.Task.Run(() =>
                {
                    scriptsComp = DelegateService.scriptServiceCore.GetScriptComposite(scripId);
                    DataFacadeManager.Dispose();
                }));
            }
            Task.WaitAll(taskRule.ToArray());
            #endregion
            if (product != null)
            {
                FileProcessValue fileProcessValue = new FileProcessValue();
                fileProcessValue.Key1 = (int)FileProcessType.ParametrizationProduct;
                File file = DelegateService.utilitiesServiceCore.GetFileByFileProcessValue(fileProcessValue);

                if (file != null && file.IsEnabled)
                {
                    object obj = new object();
                    file.Name = fileName;
                    List<Row> rows = new List<Row>();
                    Row row = new Row();
                    row.Fields = new List<Field>();
                    var template = file.Templates.First(x => x.PropertyName == TemplatePropertyName.Product).Rows[0].Fields;
                    row.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.Producto), FieldPropertyName.Producto, product.Description + FormatString(product.Id.ToString())));
                    row.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.PrefixName), FieldPropertyName.PrefixName, product.Prefix.Description));
                    row.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.CurrencyName), FieldPropertyName.CurrencyName, string.Join("; ", product.Currency.Select(z => z.Description).ToList())));
                    row.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.ScriptName), FieldPropertyName.ScriptId, Scripts?.FirstOrDefault()?.Description + product.ScriptId?.ToString()));
                    row.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.RulePreName), FieldPropertyName.RulePreName, product.PreRuleSetId?.ToString()));
                    row.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.RuleName), FieldPropertyName.RuleId, product.RuleSetId?.ToString()));
                    row.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.IsFlatRate), FieldPropertyName.IsFlatRate, ConvertBool(product.IsFlatRate)));
                    row.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.IsGreen), FieldPropertyName.IsGreen, ConvertBool(product.IsGreen)));
                    row.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.IsCollective), FieldPropertyName.IsCollective, ConvertBool(product.IsCollective)));
                    row.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.IsRequest), FieldPropertyName.IsRequest, ConvertBool(product.IsRequest)));
                    row.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.IsUse), FieldPropertyName.IsUse, ConvertBool(product.IsUse)));
                    row.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.PolicyCurrentFrom), FieldPropertyName.PolicyCurrentFrom, product.CurrentFrom.ToString()));
                    row.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.PolicyCurrentTo), FieldPropertyName.PolicyCurrentTo, product.CurrentTo?.ToString() ?? ""));
                    row.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.AgentParticipation), FieldPropertyName.AgentParticipation, product.StandardCommissionPercentage.ToString()));
                    row.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.MaxRiskQuantity), FieldPropertyName.MaxRiskQuantity, product.RiskTypes.FirstOrDefault()?.MaxRiskQuantity.ToString()));
                    row.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.BusinessBranchRiskType), FieldPropertyName.BusinessBranchRiskType, product.RiskTypes.FirstOrDefault()?.Description));
                    row.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.ScriptName), FieldPropertyName.ScriptRiskName, ScriptRisk?.FirstOrDefault()?.Description == null ? "" : ScriptRisk?.FirstOrDefault()?.Description == null ? "" : ScriptRisk.First().Description + FormatString(product.RiskTypes.First().ScriptId.Value.ToString())));
                    row.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.RulePreName), FieldPropertyName.RuleRiskPreName, RuleSetRiskPre?.FirstOrDefault()?.Description + FormatString(RuleSetRiskPre?.FirstOrDefault()?.RuleSetId.ToString())));
                    row.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.RuleName), FieldPropertyName.RuleRiskName, RuleSetRisk?.FirstOrDefault()?.Description == null ? "" : RuleSetRisk.First().Description + FormatString(RuleSetRisk?.FirstOrDefault()?.RuleSetId.ToString())));
                    row.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.PolicyTypeName), FieldPropertyName.PolicyTypeName, product.PolicyType?.FirstOrDefault()?.Description?.ToString()));
                    file.Templates[0].Rows.Add(row);
                    //Reglas General
                    if (product.RuleSetId.HasValue)
                    {
                        List<int> ruleGeneral = new List<int> { (int)product.RuleSetId };
                        Task<List<Row>> taskGeneral = Task<List<Row>>.Run(() =>
                        {
                            return GetRuleSets(file.Templates.First(x => x.PropertyName == TemplatePropertyName.RulePakage).Rows[0].Fields, ruleGeneral);
                        }
                        );
                        file.Templates.First(x => x.PropertyName == TemplatePropertyName.RulePakage).Rows.AddRange(taskGeneral.Result);
                    }
                    if (RuleSetRisk?.FirstOrDefault()?.RuleSetId != null)
                    {
                        List<int> ruleRisk = new List<int> { RuleSetRisk.First().RuleSetId };
                        Task<List<Row>> taskRisk = Task<List<Row>>.Run(() =>
                        {
                            return GetRuleSets(file.Templates.First(x => x.PropertyName == TemplatePropertyName.RulePakageRisk).Rows[0].Fields, ruleRisk);
                        }
                        );
                        file.Templates.First(x => x.PropertyName == TemplatePropertyName.RulePakageRisk).Rows.AddRange(taskRisk.Result);
                    }
                    if (RuleSetRiskPre?.FirstOrDefault()?.RuleSetId != null)
                    {
                        List<int> ruleRisk = new List<int> { RuleSetRiskPre.FirstOrDefault().RuleSetId };
                        Task<List<Row>> taskRisk = Task<List<Row>>.Run(() =>
                        {
                            return GetRuleSets(file.Templates.First(x => x.PropertyName == TemplatePropertyName.RulePakageRisk).Rows[0].Fields, ruleRisk);
                        }
                        );
                        file.Templates.First(x => x.PropertyName == TemplatePropertyName.RulePakageRisk).Rows.AddRange(taskRisk.Result);
                    }


                    if (product.RiskTypes[0].GroupCoverages != null && product.RiskTypes[0].GroupCoverages.Count > 0)
                    {
                        List<int> ruleCoverage = new List<int>();
                        ruleCoverage = product.RiskTypes[0].GroupCoverages.SelectMany(x => x.InsuredObjects).Select(b => b.Coverages.Where(a => a.Coverage.RuleSetId != null).Select(z => (int)z.Coverage.RuleSetId)).Select(y => new { Id = y }).SelectMany(n => n.Id).Distinct().ToList();
                        Task<List<Row>> taskRisk = Task<List<Row>>.Run(() =>
                        {
                            return GetRuleSets(file.Templates.First(x => x.PropertyName == TemplatePropertyName.RulePakageCoverage).Rows[0].Fields, ruleCoverage);
                        }
                        );
                        file.Templates.First(x => x.PropertyName == TemplatePropertyName.RulePakageCoverage).Rows.AddRange(taskRisk.Result);
                    }

                    if (product.RiskTypes[0].GroupCoverages != null && product.RiskTypes[0].GroupCoverages.Count > 0)
                    {
                        List<int> ruleCoveragePos = new List<int>();
                        ruleCoveragePos = product.RiskTypes[0].GroupCoverages.SelectMany(x => x.InsuredObjects).Select(b => b.Coverages.Where(a => a.Coverage.PosRuleSetId != null).Select(z => (int)z.Coverage.PosRuleSetId)).Select(y => new { Id = y }).SelectMany(n => n.Id).Distinct().ToList();
                        Task<List<Row>> taskRisk = Task<List<Row>>.Run(() =>
                        {
                            return GetRuleSets(file.Templates.First(x => x.PropertyName == TemplatePropertyName.RulePakageCoverage).Rows[0].Fields, ruleCoveragePos);
                        }
                        );
                        file.Templates.First(x => x.PropertyName == TemplatePropertyName.RulePakageCoverage).Rows.AddRange(taskRisk.Result);
                    }

                    int maxRow;
                    if (scriptsComp != null)
                    {
                        ConcurrentBag<Row> rowsScript = new ConcurrentBag<Row>();
                        maxRow = file.Templates.First(x => x.PropertyName == TemplatePropertyName.Script).Rows.Count() - 1;
                        var templatescripts = file.Templates.First(x => x.PropertyName == TemplatePropertyName.Script).Rows[maxRow].Fields;
                        TP.Parallel.ForEach(scriptsComp.Nodes, script =>
                        {
                            TP.Parallel.ForEach(script.Questions, question =>
                            {
                                Row rowScript = new Row();
                                rowScript.Fields = new List<Field>();
                                rowScript.Fields.Add(NewField(templatescripts.FirstOrDefault(x => x.PropertyName == FieldPropertyName.ScriptId), FieldPropertyName.ScriptId, scriptsComp.Script.Description));
                                rowScript.Fields.Add(NewField(templatescripts.FirstOrDefault(x => x.PropertyName == FieldPropertyName.ScriptName), FieldPropertyName.ScriptName, question.Description));
                                rowsScript.Add(rowScript);
                            });
                        });
                        file.Templates.First(x => x.PropertyName == TemplatePropertyName.Script).Rows.AddRange(rowsScript);
                    }

                    maxRow = file.Templates.First(x => x.PropertyName == TemplatePropertyName.AdditionalIntermediaries).Rows.Count() - 1;
                    template = file.Templates.First(x => x.PropertyName == TemplatePropertyName.AdditionalIntermediaries).Rows[maxRow].Fields;
                    ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                    filter.PropertyEquals(PRODEN.ProductAgent.Properties.ProductId, product.Id);
                    SelectQuery selectQuery = new SelectQuery();
                    selectQuery.AddSelectValue(new SelectValue(new Column(PRODEN.ProductAgent.Properties.ProductId, "p")));
                    selectQuery.AddSelectValue(new SelectValue(new Column(PRODEN.ProductAgent.Properties.IndividualId, "p")));
                    selectQuery.AddSelectValue(new SelectValue(new Column(UPEN.Agent.Properties.CheckPayableTo, "pa")));
                    selectQuery.AddSelectValue(new SelectValue(new Column(UPEN.Agent.Properties.Locker, "pa")));
                    selectQuery.AddSelectValue(new SelectValue(new Column(UPEN.Agent.Properties.DeclinedDate, "pa")));
                    selectQuery.AddSelectValue(new SelectValue(new Column(PARAMEN.AgentType.Properties.Description, "pt")));
                    #region Joins
                    Join join = new Join(new ClassNameTable(typeof(PRODEN.ProductAgent), "p"), new ClassNameTable(typeof(UPEN.Agent), "pa"), JoinType.Inner);
                    join.Criteria = (new ObjectCriteriaBuilder()
                        .Property(PRODEN.ProductAgent.Properties.IndividualId, "p")
                        .Equal()
                        .Property(UPEN.Agent.Properties.IndividualId, "pa")
                        .GetPredicate());

                    join = new Join(join, new ClassNameTable(typeof(PARAMEN.AgentType), "pt"), JoinType.Inner);
                    join.Criteria = (new ObjectCriteriaBuilder()
                        .Property(UPEN.Agent.Properties.AgentTypeCode, "pa")
                        .Equal()
                        .Property(PARAMEN.AgentType.Properties.AgentTypeCode, "pt")
                        .GetPredicate());
                    #endregion Joins
                    selectQuery.Where = filter.GetPredicate();
                    selectQuery.Table = join;
                    using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
                    {
                        while (reader.Read())
                        {
                            DateTime? valAgent = reader.IsDBNull(4) ? (DateTime?)null : (DateTime)reader["DeclinedDate"];
                            if (valAgent == null || valAgent > DateTime.Now)
                            {
                                Row rowAgent = new Row();
                                rowAgent.Fields = new List<Field>();
                                rowAgent.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.AgentCode), FieldPropertyName.AgentCode, (string)(reader["IndividualId"].ToString())));
                                rowAgent.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.AgentType), FieldPropertyName.AgentType, (string)(reader["Description"])));
                                rowAgent.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.AgentName), FieldPropertyName.AgentName, (string)(reader["CheckPayableTo"])));
                                lock (obj)
                                {
                                    file.Templates.First(x => x.PropertyName == TemplatePropertyName.AdditionalIntermediaries).Rows.Add(rowAgent);
                                }
                            }
                        }
                    }


                    maxRow = file.Templates.First(x => x.PropertyName == TemplatePropertyName.Commissions).Rows.Count() - 1;
                    var templateCommision = file.Templates.First(x => x.PropertyName == TemplatePropertyName.Commissions).Rows[maxRow].Fields;
                    selectQuery = new SelectQuery();
                    selectQuery.AddSelectValue(new SelectValue(new Column(PRODEN.ProductAgencyCommiss.Properties.ProductId, "p")));
                    selectQuery.AddSelectValue(new SelectValue(new Column(PRODEN.ProductAgencyCommiss.Properties.IndividualId, "p")));
                    selectQuery.AddSelectValue(new SelectValue(new Column(PRODEN.ProductAgencyCommiss.Properties.AgentAgencyId, "p")));
                    selectQuery.AddSelectValue(new SelectValue(new Column(PRODEN.ProductAgencyCommiss.Properties.StCommissPercentage, "p")));
                    selectQuery.AddSelectValue(new SelectValue(new Column(PRODEN.ProductAgencyCommiss.Properties.AdditCommissPercentage, "p")));
                    selectQuery.AddSelectValue(new SelectValue(new Column(UPEN.AgentAgency.Properties.Description, "pa")));
                    selectQuery.AddSelectValue(new SelectValue(new Column(UPEN.AgentAgency.Properties.AgentCode, "pa")));
                    selectQuery.AddSelectValue(new SelectValue(new Column(UPEN.AgentAgency.Properties.DeclinedDate, "pa")));
                    selectQuery.AddSelectValue(new SelectValue(new Column(PARAMEN.AgentType.Properties.Description, "pt")));
                    #region Joins
                    join = new Join(new ClassNameTable(typeof(PRODEN.ProductAgencyCommiss), "p"), new ClassNameTable(typeof(UPEN.AgentAgency), "pa"), JoinType.Inner);
                    join.Criteria = (new ObjectCriteriaBuilder()
                        .Property(PRODEN.ProductAgencyCommiss.Properties.IndividualId, "p")
                        .Equal()
                        .Property(UPEN.AgentAgency.Properties.IndividualId, "pa")
                        .And()
                        .Property(PRODEN.ProductAgencyCommiss.Properties.AgentAgencyId, "p")
                        .Equal()
                        .Property(UPEN.AgentAgency.Properties.AgentAgencyId, "pa")
                        .GetPredicate());

                    join = new Join(join, new ClassNameTable(typeof(PARAMEN.AgentType), "pt"), JoinType.Inner);
                    join.Criteria = (new ObjectCriteriaBuilder()
                        .Property(UPEN.AgentAgency.Properties.AgentTypeCode, "pa")
                        .Equal()
                        .Property(PARAMEN.AgentType.Properties.AgentTypeCode, "pt")
                        .GetPredicate());
                    #endregion Joins
                    selectQuery.Where = filter.GetPredicate();
                    selectQuery.Table = join;
                    using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
                    {
                        while (reader.Read())
                        {
                            DateTime? valAgent = reader.IsDBNull(7) ? (DateTime?)null : (DateTime)reader["DeclinedDate"];
                            if (valAgent == null || valAgent > DateTime.Now)
                            {
                                Row rowAgencyCommiss = new Row();
                                rowAgencyCommiss.Fields = new List<Field>();
                                rowAgencyCommiss.Fields.Add(NewField(templateCommision.FirstOrDefault(x => x.PropertyName == FieldPropertyName.AgentCode), FieldPropertyName.AgentCode, (string)(reader["AgentCode"].ToString())));
                                rowAgencyCommiss.Fields.Add(NewField(templateCommision.FirstOrDefault(x => x.PropertyName == FieldPropertyName.AgentType), FieldPropertyName.AgentType, (string)(reader[8])));
                                rowAgencyCommiss.Fields.Add(NewField(templateCommision.FirstOrDefault(x => x.PropertyName == FieldPropertyName.AgentParticipation), FieldPropertyName.AgentParticipation, (string)(reader["StCommissPercentage"].ToString())));
                                rowAgencyCommiss.Fields.Add(NewField(templateCommision.FirstOrDefault(x => x.PropertyName == FieldPropertyName.AgentName), FieldPropertyName.AgentName, (string)(reader[5])));
                                lock (obj)
                                {
                                    file.Templates.First(x => x.PropertyName == TemplatePropertyName.Commissions).Rows.Add(rowAgencyCommiss);
                                }
                            }
                        }
                    }

                    if (product.FinancialPlan != null && product.FinancialPlan.Count > 0)
                    {
                        maxRow = file.Templates.First(x => x.PropertyName == TemplatePropertyName.PaymentPlans).Rows.Count() - 1;
                        template = file.Templates.First(x => x.PropertyName == TemplatePropertyName.PaymentPlans).Rows[maxRow].Fields;
                        TP.Parallel.ForEach(product.FinancialPlan, paymentPlan =>
                        {
                            Row rowPaymentPlan = new Row();
                            rowPaymentPlan.Fields = new List<Field>();
                            rowPaymentPlan.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.PolicyPaymentPlan), FieldPropertyName.PolicyPaymentPlan, paymentPlan.Id.ToString()));
                            rowPaymentPlan.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.CurrencyName), FieldPropertyName.CurrencyName, paymentPlan.Currency.Description.ToString()));
                            rowPaymentPlan.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.PaymentMethodName), FieldPropertyName.PaymentMethodName, paymentPlan.PaymentMethod.Description));
                            rowPaymentPlan.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.PaymentPlanName), FieldPropertyName.PaymentPlanName, paymentPlan.PaymentSchedule.Description));
                            rowPaymentPlan.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.IsUse), FieldPropertyName.IsUse, ConvertBool(paymentPlan.IsDefault)));
                            lock (obj)
                            {
                                file.Templates.First(x => x.PropertyName == TemplatePropertyName.PaymentPlans).Rows.Add(rowPaymentPlan);
                            }
                        });
                    }
                    if (product.RiskTypes[0].GroupCoverages != null && product.RiskTypes[0].GroupCoverages.Count > 0)
                    {

                        maxRow = file.Templates.First(x => x.PropertyName == TemplatePropertyName.GroupCoverages).Rows.Count() - 1;
                        template = file.Templates.First(x => x.PropertyName == TemplatePropertyName.GroupCoverages).Rows[maxRow].Fields;
                        ConcurrentBag<string> erorrs = new ConcurrentBag<string>();
                        TP.Parallel.ForEach(product.RiskTypes[0].GroupCoverages, groupCoverage =>
                        {
                            if (groupCoverage.InsuredObjects != null && groupCoverage.InsuredObjects.Count > 0)
                            {
                                TP.Parallel.ForEach(groupCoverage.InsuredObjects, insuredObject =>
                                {
                                    if (insuredObject.Coverages != null && insuredObject.Coverages.Count > 0)
                                    {
                                        TP.Parallel.ForEach(insuredObject.Coverages, coverage =>
                                        {
                                            Row rowCoverage = new Row();
                                            rowCoverage.Fields = new List<Field>();
                                            //rowCoverage.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.GroupCoverageInsuredObjectName), FieldPropertyName.GroupCoverageInsuredObjectName, groupCoverage.Description + FormatString(groupCoverage.Id.ToString())));
                                            rowCoverage.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.InsuredObjectName), FieldPropertyName.InsuredObjectName, insuredObject.Description + FormatString(insuredObject.Id.ToString())));
                                            //rowCoverage.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.IsMandatory), FieldPropertyName.IsMandatory, ConvertBool(insuredObject.IsMandatory)));
                                            //rowCoverage.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.IsSelected), FieldPropertyName.IsSelected, ConvertBool(insuredObject.IsSelected)));
                                            rowCoverage.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.PerilDescription), FieldPropertyName.PerilDescription, coverage.Coverage.Description + FormatString(coverage.Coverage?.Id.ToString())));
                                            rowCoverage.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.ScriptId), FieldPropertyName.ScriptId, ScriptCoverage?.FirstOrDefault(x => coverage.Coverage.ScriptId != null && x.ScriptId == coverage.Coverage.ScriptId)?.Description + FormatString(coverage.Coverage.ScriptId?.ToString())));
                                            rowCoverage.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.RulePreId), FieldPropertyName.RulePreId, RuleSetCoveragePre?.FirstOrDefault(x => coverage.Coverage.RuleSetId != null && x.RuleSetId == coverage.Coverage.RuleSetId)?.Description + FormatString(coverage.Coverage.RuleSetId?.ToString())));
                                            rowCoverage.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.RuleId), FieldPropertyName.RuleId, RuleSetCoverage?.FirstOrDefault(x => coverage.Coverage.PosRuleSetId != null && x.RuleSetId == coverage.Coverage.PosRuleSetId)?.Description + FormatString(coverage.Coverage.PosRuleSetId?.ToString())));
                                            rowCoverage.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.PrefixName), FieldPropertyName.BusinessBranchDescription, coverage.Coverage.LineBusinessDescription + FormatString(coverage.Coverage.LineBusinessId.ToString())));
                                            lock (obj)
                                            {
                                                file.Templates.First(x => x.PropertyName == TemplatePropertyName.GroupCoverages).Rows.Add(rowCoverage);
                                            }
                                        });
                                    }

                                });
                            }

                        });
                    }
                    foreach (var item in file.Templates)
                    {
                        foreach (var itemRow in item.Rows)
                        {
                            for (int i = 0; i < itemRow.Fields.Count(); i++)
                            {
                                if (itemRow.Fields[i] == null)
                                {
                                    itemRow.Fields.RemoveAt(i);
                                    i--;
                                }
                            }
                        }
                    }
                    var deductibleProduct = this.GetProductDeductiblesByPrefix(product.Id, product.Prefix.Id);
                    deductibleProduct = deductibleProduct.Where(x => x.IsSelected == true).ToList();
                    if (deductibleProduct != null && deductibleProduct.Count > 0)
                    {
                        maxRow = file.Templates.First(x => x.PropertyName == TemplatePropertyName.Deductible).Rows.Count() - 1;
                        template = file.Templates.First(x => x.PropertyName == TemplatePropertyName.Deductible).Rows[maxRow].Fields;
                        TP.Parallel.ForEach(deductibleProduct, deductible =>
                        {
                            Row rowDeductible = new Row();
                            rowDeductible.Fields = new List<Field>();
                            rowDeductible.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.DeductibleDescription), FieldPropertyName.DeductibleDescription, deductible.Description + FormatString(deductible.DeductId.ToString())));
                            lock (obj)
                            {
                                file.Templates.First(x => x.PropertyName == TemplatePropertyName.Deductible).Rows.Add(rowDeductible);
                            }
                        });
                    }

                    var productCommercialClass = this.GetRiskCommercialClass(product.Id);
                    productCommercialClass = productCommercialClass.Where(x => x.IsSelected == true).ToList();

                    if (productCommercialClass != null && productCommercialClass.Count > 0)
                    {
                        maxRow = file.Templates.First(x => x.PropertyName == TemplatePropertyName.Activities).Rows.Count() - 1;
                        template = file.Templates.First(x => x.PropertyName == TemplatePropertyName.Activities).Rows[maxRow].Fields;
                        TP.Parallel.ForEach(productCommercialClass, activiti =>
                        {
                            Row rowActiviti = new Row();
                            rowActiviti.Fields = new List<Field>();
                            rowActiviti.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.IsSelected), FieldPropertyName.IsSelected, ConvertBool(activiti.IsSelected)));
                            rowActiviti.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.ActivityName), FieldPropertyName.ActivityName, activiti.Description + FormatString(activiti.Id.ToString())));
                            lock (obj)
                            {
                                file.Templates.First(x => x.PropertyName == TemplatePropertyName.Activities).Rows.Add(rowActiviti);
                            }
                        });
                    }

                    var productForms = this.GetProductForm(product.Id);
                    if (productForms != null && productForms.Count > 0)
                    {
                        maxRow = file.Templates.First(x => x.PropertyName == TemplatePropertyName.FormsOfPrinting).Rows.Count() - 1;
                        template = file.Templates.First(x => x.PropertyName == TemplatePropertyName.FormsOfPrinting).Rows[maxRow].Fields;
                        TP.Parallel.ForEach(productForms, productForm =>
                        {
                            Row rowProductForm = new Row();
                            rowProductForm.Fields = new List<Field>();
                            rowProductForm.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.PolicyCurrentFrom), FieldPropertyName.PolicyCurrentFrom, productForm.CurrentFrom.ToString()));
                            rowProductForm.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.FormName), FieldPropertyName.FormName, productForm.FormNumber));
                            rowProductForm.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.RiskGroupCoverage), FieldPropertyName.RiskGroupCoverage, product.RiskTypes[0].GroupCoverages?.FirstOrDefault(x => x.Id == productForm.CoverGroupId)?.Description + FormatString(productForm.CoverGroupId.ToString())));
                            lock (obj)
                            {
                                file.Templates.First(x => x.PropertyName == TemplatePropertyName.FormsOfPrinting).Rows.Add(rowProductForm);
                            }
                        });
                    }
                    List<int> listPolicy = new List<int>();
                    foreach (CiaParamPolicyType itemCiaParamPolicyType in product.PolicyType)
                    {
                        listPolicy.Add(itemCiaParamPolicyType.Id);
                    }
                    var productLimitRC = this.GetLimitsRc(listPolicy, product.Id, product.Prefix.Id);
                    productLimitRC = productLimitRC.Where(x => x.IsSelected == true).ToList();
                    if (productLimitRC != null && productLimitRC.Count > 0)
                    {
                        maxRow = file.Templates.First(x => x.PropertyName == TemplatePropertyName.LimitsRC).Rows.Count() - 1;
                        template = file.Templates.First(x => x.PropertyName == TemplatePropertyName.LimitsRC).Rows[maxRow].Fields;
                        TP.Parallel.ForEach(productLimitRC, limitRC =>
                        {
                            Row rowlimitRC = new Row();
                            rowlimitRC.Fields = new List<Field>();
                            rowlimitRC.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.RiskLimitRcDescription), FieldPropertyName.RiskLimitRcDescription, limitRC.Description + FormatString(limitRC.Id.ToString())));
                            rowlimitRC.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.IsSelected), FieldPropertyName.IsSelected, ConvertBool(limitRC.IsDefault)));
                            rowlimitRC.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.PolicyType), FieldPropertyName.PolicyType, product.PolicyType?.FirstOrDefault(x => x.Id == limitRC.PolicyTypeId)?.Description + FormatString(limitRC.PolicyTypeId.ToString())));
                            lock (obj)
                            {
                                file.Templates.First(x => x.PropertyName == TemplatePropertyName.LimitsRC).Rows.Add(rowlimitRC);
                            }
                        });
                    }

                    return DelegateService.utilitiesServiceCore.GenerateFile(file);
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }

        private Field NewField(Field oldField, string fieldPropertyName, string value)
        {
            Field field = new Field();

            if (oldField != null)
            {
                var config = new MapperConfiguration(cfg =>
                 {
                     cfg.CreateMap(oldField.GetType(), field.GetType());
                 });
                config.CreateMapper().Map(oldField, field);
                field.Value = value;
                return field;
            }
            return null;
        }

        private string ConvertBool(bool item)
        {
            if (item)
                return "SI";
            else
                return "NO";
        }

        private string FormatString(string item)
        {
            if (item != null)
            {
                return " (" + item + ")";
            }
            else
            {
                return null;
            }
        }

        private List<Row> GetRuleSets(List<Field> field, List<int> ruleIds)
        {
            ConcurrentBag<Sistran.Core.Application.RulesScriptsServices.Models._RuleSet> _RuleSets = new ConcurrentBag<Sistran.Core.Application.RulesScriptsServices.Models._RuleSet>();
            ConcurrentDictionary<string, Row> rows = new ConcurrentDictionary<string, Row>();
            TP.Parallel.ForEach(ruleIds, ruleId =>
            {
                _RuleSets.Add(DelegateService.ruleServiceCore.GetRuleSetById(ruleId));
                DataFacadeManager.Dispose();
            });

            TP.Parallel.For(0, _RuleSets.Count, y =>
              {
                  var ruleSet = _RuleSets.ToList()[y];

                  TP.Parallel.For(0, ruleSet.Rules.Count, i =>
               {

                   var rule = ruleSet.Rules[i];
                   Row row = new Row();
                   row.Fields = new List<Field>();

                   if (i == 0)
                   {
                       row.Fields.Add(NewField(field.FirstOrDefault(x => x.PropertyName == FieldPropertyName.RulePreName), FieldPropertyName.RuleName, ruleSet.Description + FormatString(ruleSet.RuleSetId.ToString())));
                   }
                   else
                   {
                       row.Fields.Add(NewField(field.FirstOrDefault(x => x.PropertyName == FieldPropertyName.RulePreName), FieldPropertyName.RuleName, " "));
                   }
                   row.Fields.Add(NewField(field.FirstOrDefault(x => x.PropertyName == FieldPropertyName.RuleName), FieldPropertyName.RuleName, rule.Description));
                   rows.TryAdd(y + "" + i, row);

               });

              });

            return rows.OrderBy(x => x.Key).Select(x => x.Value).ToList();
        }
        #region AdditionalData

        /// <summary>
        /// Obtener lista de limites RC
        /// </summary>
        /// <param name = "" ></ param >
        /// < returns > Lista Limit RC</returns>
        public List<CiaParamLimitsRC> GetLimitsRc(List<int> policyTypeIds, int productId, int prefixCd)
        {
            List<CiaParamLimitsRC> listLimitRC = new List<CiaParamLimitsRC>();
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.CoLimitsRc)));
            listLimitRC = ModelAssembler.MapCiaParamLimitRCs(businessCollection.Cast<COMMEN.CoLimitsRc>().ToList());
            List<CiaParamLimitsRC> result = new List<CiaParamLimitsRC>();

            List<CiaParamLimitsRC> ciaParamLimitsRCs = GetLimitsRcRels(productId, prefixCd);
            foreach (int itemPolicyTypeId in policyTypeIds)
            {
                foreach (CiaParamLimitsRC itemLimitRC in listLimitRC)
                {
                    CiaParamLimitsRC tempItemLimitRC = new CiaParamLimitsRC();
                    tempItemLimitRC.IsDefault = ciaParamLimitsRCs.Where(x => x.Id == itemLimitRC.Id && x.PolicyTypeId == itemPolicyTypeId).Any() ? ciaParamLimitsRCs.Where(x => x.Id == itemLimitRC.Id && x.PolicyTypeId == itemPolicyTypeId).FirstOrDefault().IsDefault : false;
                    tempItemLimitRC.IsSelected = ciaParamLimitsRCs.Where(x => x.Id == itemLimitRC.Id && x.PolicyTypeId == itemPolicyTypeId).Any() ? true : false;
                    tempItemLimitRC.PolicyTypeId = itemPolicyTypeId;
                    tempItemLimitRC.ProductId = productId;
                    tempItemLimitRC.PrefixId = prefixCd;
                    tempItemLimitRC.Description = itemLimitRC.Description;
                    tempItemLimitRC.Id = itemLimitRC.Id;
                    result.Add(tempItemLimitRC);
                }
            }
            return result;
        }

        /// <summary>
        /// Obtener lista de limites RC
        /// </summary>
        /// <param name=""></param>
        /// <returns>Lista Limit RC</returns>
        public List<CiaParamLimitsRC> GetLimitsRcRels(int productId, int prefixCd)
        {
            List<CiaParamLimitsRC> listLimitRCRel = new List<CiaParamLimitsRC>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.PropertyEquals(COMMEN.CoLimitsRcRel.Properties.ProductId, productId);
            filter.And();
            filter.PropertyEquals(COMMEN.CoLimitsRcRel.Properties.PrefixCode, prefixCd);
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.CoLimitsRcRel), filter.GetPredicate()));

            listLimitRCRel = ModelAssembler.MapCiaParamLimitRCRels(businessCollection.Cast<COMMEN.CoLimitsRcRel>().ToList());
            return listLimitRCRel;
        }

        public List<CiaParamCommercialClass> GetRiskCommercialClass(int productId)
        {
            List<CiaParamCommercialClass> listCiaParamCommercialClass = new List<CiaParamCommercialClass>();
            List<CiaParamCommercialClass> result = new List<CiaParamCommercialClass>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.PropertyEquals(RiskCommercialClass.Properties.Enabled, true);
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(RiskCommercialClass), filter.GetPredicate()));

            listCiaParamCommercialClass = ModelAssembler.CreateMapRiskCommercialClass(businessCollection.Cast<RiskCommercialClass>().ToList());
            List<CiaParamCommercialClass> ciaProductRiskCommercialClasses = GetProductRiskCommercialClass(productId);

            foreach (CiaParamCommercialClass itemCiaParamCommercialClass in listCiaParamCommercialClass)
            {
                itemCiaParamCommercialClass.IsDefault = ciaProductRiskCommercialClasses.Where(x => x.Id == itemCiaParamCommercialClass.Id).Any() ? ciaProductRiskCommercialClasses.Where(x => x.Id == itemCiaParamCommercialClass.Id).FirstOrDefault().IsDefault : false;
                itemCiaParamCommercialClass.IsSelected = ciaProductRiskCommercialClasses.Where(x => x.Id == itemCiaParamCommercialClass.Id).Any() ? true : false;
                itemCiaParamCommercialClass.ProductId = productId;

                result.Add(itemCiaParamCommercialClass);
            }
            return result;
        }

        public List<CiaParamCommercialClass> GetProductRiskCommercialClass(int productId)
        {
            List<CiaParamCommercialClass> result = new List<CiaParamCommercialClass>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.PropertyEquals(COMMEN.ProductRiskCommercialClass.Properties.ProductId, productId);
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.ProductRiskCommercialClass), filter.GetPredicate()));
            result = ModelAssembler.CreateMapProductRiskCommercialClass(businessCollection.Cast<COMMEN.ProductRiskCommercialClass>().ToList());
            return result;
        }

        public List<CiaParamForm> GetProductForm(int productId)
        {
            List<CiaParamForm> result = new List<CiaParamForm>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.PropertyEquals(PRODEN.ProductForm.Properties.ProductId, productId);
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(PRODEN.ProductForm), filter.GetPredicate()));
            result = ModelAssembler.CreateMapProductForm(businessCollection.Cast<PRODEN.ProductForm>().ToList());
            return result;
        }



        public List<CiaParamDeductibleProduct> GetProductDeductiblesByPrefix(int productId, int prefixCode)
        {
            List<CiaParamDeductibleProduct> result = new List<CiaParamDeductibleProduct>();
            List<CiaParamDeductibleProduct> ciaParamDeductibleProducts = new List<CiaParamDeductibleProduct>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            List<CiaParamPrefixLineBusiness> listCiaParamPrefixLineBusiness = GetPrefixLineBusiness(prefixCode);

            ObjectCriteriaBuilder filterDeductible = new ObjectCriteriaBuilder();
            filter.Property(QUOEN.Deductible.Properties.LineBusinessCode, typeof(QUOEN.Deductible).Name).In().ListValue();
            foreach (CiaParamPrefixLineBusiness ciaParamPrefixLineBusiness in listCiaParamPrefixLineBusiness)
            {
                filter.Constant(ciaParamPrefixLineBusiness.LineBusinessCode);
            }

            filter.EndList();

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(QUOEN.Deductible), filter.GetPredicate()));
            ciaParamDeductibleProducts = ModelAssembler.CreateMapDeductibleProduct(businessCollection.Cast<QUOEN.Deductible>().ToList());

            List<CiaParamDeductibleProduct> lstCiaParamDeductibleByProduct = GetProductByDeductibles(productId);

            foreach (CiaParamDeductibleProduct itemCiaParamDeductibleProduct in ciaParamDeductibleProducts)
            {
                itemCiaParamDeductibleProduct.IsSelected = lstCiaParamDeductibleByProduct.Where(x => x.DeductId == itemCiaParamDeductibleProduct.DeductId).Any() ? true : false;

                result.Add(itemCiaParamDeductibleProduct);
            }

            return result;
        }

        public List<CiaParamPrefixLineBusiness> GetPrefixLineBusiness(int prefixCode)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.PropertyEquals(COMMEN.PrefixLineBusiness.Properties.PrefixCode, prefixCode);
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.PrefixLineBusiness), filter.GetPredicate()));
            return ModelAssembler.CreateMapPrefixLineBusiness(businessCollection.Cast<COMMEN.PrefixLineBusiness>().ToList());
        }

        public List<CiaParamDeductibleProduct> GetProductByDeductibles(int prodcutId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.PropertyEquals(COMMEN.DeductibleProduct.Properties.ProductId, prodcutId);
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.DeductibleProduct), filter.GetPredicate()));
            return ModelAssembler.CreateMapDeductibleByProduct(businessCollection.Cast<COMMEN.DeductibleProduct>().ToList());
        }

        public void InsertProductDeductibles(CiaParamDeductibleProduct ciaParamDeductibleProduct, int productId)
        {
            COMMEN.DeductibleProduct deductibleProductEntity = EntityAssembler.CreateDeductibleProduct(ciaParamDeductibleProduct);
            deductibleProductEntity.ProductId = productId;
            DataFacadeManager.Instance.GetDataFacade().InsertObject(deductibleProductEntity);
        }

        public void InsertProductRiskCommercialClass(CiaParamCommercialClass ciaParamCommercialClass)
        {
            COMMEN.ProductRiskCommercialClass productRiskCommercialClassEntity = EntityAssembler.CreateProductRiskCommercialClass(ciaParamCommercialClass);
            DataFacadeManager.Instance.GetDataFacade().InsertObject(productRiskCommercialClassEntity);
        }

        public void InsertProductForm(CiaParamForm ciaParamForm, int productId)
        {
            ciaParamForm.CurrentFrom = DateTime.Parse(ciaParamForm.StrCurrentFrom);
            PRODEN.ProductForm productFormEntity = EntityAssembler.CreateProductForm(ciaParamForm);
            productFormEntity.ProductId = productId;
            productFormEntity.FormId = GetIdProductForm();
            DataFacadeManager.Instance.GetDataFacade().InsertObject(productFormEntity);
            PrimaryKey key = COMMEN.Parameter.CreatePrimaryKey(2134);
            COMMEN.Parameter parameter = (COMMEN.Parameter)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
            parameter.NumberParameter = productFormEntity.FormId;
            DataFacadeManager.Instance.GetDataFacade().UpdateObject(parameter);

        }

        public void InsertProductLimitRC(CiaParamLimitsRC ciaParamLimitsRC)
        {
            COMMEN.CoLimitsRcRel coLimitsRcRelEntity = EntityAssembler.CreateCoLimitsRcRel(ciaParamLimitsRC);
            DataFacadeManager.Instance.GetDataFacade().InsertObject(coLimitsRcRelEntity);
        }

        public void UpdateProductDeductibles(CiaParamDeductibleProduct ciaParamDeductibleProduct)
        {
            PrimaryKey key = COMMEN.DeductibleProduct.CreatePrimaryKey(ciaParamDeductibleProduct.DeductId, ciaParamDeductibleProduct.ProductId);
            COMMEN.DeductibleProduct deductibleProduct = (COMMEN.DeductibleProduct)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
            DataFacadeManager.Instance.GetDataFacade().UpdateObject(deductibleProduct);

        }

        public void UpdateProductRiskCommercialClass(CiaParamCommercialClass ciaParamCommercialClass)
        {
            PrimaryKey key = COMMEN.ProductRiskCommercialClass.CreatePrimaryKey(ciaParamCommercialClass.ProductId, ciaParamCommercialClass.Id);
            COMMEN.ProductRiskCommercialClass productCommercialClass = (COMMEN.ProductRiskCommercialClass)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
            productCommercialClass.DefaultRiskCommercial = ciaParamCommercialClass.IsDefault;
            DataFacadeManager.Instance.GetDataFacade().UpdateObject(productCommercialClass);
        }

        public void UpdateProductForm(CiaParamForm ciaParamForm)
        {
            PrimaryKey key = PRODEN.ProductForm.CreatePrimaryKey(ciaParamForm.FormId);
            PRODEN.ProductForm productForm = (PRODEN.ProductForm)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
            productForm.CurrentFrom = DateTime.Parse(ciaParamForm.StrCurrentFrom);
            productForm.FormNumber = ciaParamForm.FormNumber;
            productForm.CoverGroupId = ciaParamForm.CoverGroupId;

            DataFacadeManager.Instance.GetDataFacade().UpdateObject(productForm);
        }

        public void UpdateProductLimitRC(CiaParamLimitsRC ciaParamLimitsRC)
        {
            PrimaryKey key = COMMEN.CoLimitsRcRel.CreatePrimaryKey(ciaParamLimitsRC.PrefixId, ciaParamLimitsRC.PolicyTypeId, ciaParamLimitsRC.ProductId, ciaParamLimitsRC.Id);
            COMMEN.CoLimitsRcRel coLimitsRcRel = (COMMEN.CoLimitsRcRel)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
            coLimitsRcRel.IsDefault = ciaParamLimitsRC.IsDefault;

            DataFacadeManager.Instance.GetDataFacade().UpdateObject(coLimitsRcRel);
        }

        public void DeleteProductDeductibles(CiaParamDeductibleProduct ciaParamDeductibleProduct, int productId)
        {
            PrimaryKey key = COMMEN.DeductibleProduct.CreatePrimaryKey(ciaParamDeductibleProduct.DeductId, productId);
            COMMEN.DeductibleProduct deductibleProduct = (COMMEN.DeductibleProduct)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
            DataFacadeManager.Instance.GetDataFacade().DeleteObject(deductibleProduct);
        }

        public void DeleteProductRiskCommercialClass(CiaParamCommercialClass ciaParamCommercialClass)
        {
            PrimaryKey key = COMMEN.ProductRiskCommercialClass.CreatePrimaryKey(ciaParamCommercialClass.ProductId, ciaParamCommercialClass.Id);
            COMMEN.ProductRiskCommercialClass productCommercialClass = (COMMEN.ProductRiskCommercialClass)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
            DataFacadeManager.Instance.GetDataFacade().DeleteObject(productCommercialClass);
        }

        public void DeleteProductForm(CiaParamForm ciaParamForm)
        {
            PrimaryKey key = PRODEN.ProductForm.CreatePrimaryKey(ciaParamForm.FormId);
            PRODEN.ProductForm productForm = (PRODEN.ProductForm)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
            DataFacadeManager.Instance.GetDataFacade().DeleteObject(productForm);
        }

        public void DeleteProductLimitRC(CiaParamLimitsRC ciaParamLimitsRC)
        {
            PrimaryKey key = COMMEN.CoLimitsRcRel.CreatePrimaryKey(ciaParamLimitsRC.PrefixId, ciaParamLimitsRC.PolicyTypeId, ciaParamLimitsRC.ProductId, ciaParamLimitsRC.Id);
            COMMEN.CoLimitsRcRel coLimitsRcRel = (COMMEN.CoLimitsRcRel)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
            if (coLimitsRcRel != null)
            {
                DataFacadeManager.Instance.GetDataFacade().DeleteObject(coLimitsRcRel);
            }
        }
        #endregion AdditionalData

        #region Agents

        public CiaParamSummaryAgent GetProductAgentByProductId(int productId, int prefixId)
        {
            CiaParamSummaryAgent result = new CiaParamSummaryAgent();
            List<CiaParamAgencyCommiss> listCiaParamAgencyCommiss = new List<CiaParamAgencyCommiss>();
            List<CiaParamIncentive> listCiaParamIncentive = new List<CiaParamIncentive>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(PRODEN.ProductAgent.Properties.ProductId, productId);

            //Comisiones
            SelectQuery selectQueryComiss = new SelectQuery();
            selectQueryComiss.AddSelectValue(new SelectValue(new Column(PRODEN.ProductAgencyCommiss.Properties.ProductId, "p")));
            selectQueryComiss.AddSelectValue(new SelectValue(new Column(PRODEN.ProductAgencyCommiss.Properties.IndividualId, "p")));
            selectQueryComiss.AddSelectValue(new SelectValue(new Column(PRODEN.ProductAgencyCommiss.Properties.AgentAgencyId, "p")));
            selectQueryComiss.AddSelectValue(new SelectValue(new Column(PRODEN.ProductAgencyCommiss.Properties.StCommissPercentage, "p")));
            selectQueryComiss.AddSelectValue(new SelectValue(new Column(PRODEN.ProductAgencyCommiss.Properties.AdditCommissPercentage, "p")));
            selectQueryComiss.AddSelectValue(new SelectValue(new Column(UPEN.AgentAgency.Properties.Description, "pa")));
            selectQueryComiss.AddSelectValue(new SelectValue(new Column(UPEN.AgentAgency.Properties.AgentCode, "pa")));
            selectQueryComiss.AddSelectValue(new SelectValue(new Column(UPEN.AgentAgency.Properties.DeclinedDate, "pa")));
            #region Joins
            Join joinComiss = new Join(new ClassNameTable(typeof(PRODEN.ProductAgencyCommiss), "p"), new ClassNameTable(typeof(UPEN.AgentAgency), "pa"), JoinType.Inner);
            joinComiss.Criteria = (new ObjectCriteriaBuilder()
                .Property(PRODEN.ProductAgencyCommiss.Properties.IndividualId, "p")
                .Equal()
                .Property(UPEN.AgentAgency.Properties.IndividualId, "pa")
                .And()
                .Property(PRODEN.ProductAgencyCommiss.Properties.AgentAgencyId, "p")
                .Equal()
                .Property(UPEN.AgentAgency.Properties.AgentAgencyId, "pa")

                .GetPredicate());
            #endregion Joins
            selectQueryComiss.Where = filter.GetPredicate();
            selectQueryComiss.Table = joinComiss;
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQueryComiss))
            {
                while (reader.Read())
                {
                    DateTime? valAgent = reader.IsDBNull(7) ? (DateTime?)null : (DateTime)reader["DeclinedDate"];
                    if (valAgent == null || valAgent > DateTime.Now)
                    {
                        CiaParamAgencyCommiss ciaParamAgencyCommiss = new CiaParamAgencyCommiss();
                        ciaParamAgencyCommiss.IndividualId = Convert.ToInt32(reader["IndividualId"]);
                        ciaParamAgencyCommiss.AgencyName = (string)reader["Description"];
                        ciaParamAgencyCommiss.AgencyId = Convert.ToInt32(reader["AgentAgencyId"]);
                        ciaParamAgencyCommiss.CommissPercentage = Convert.ToDecimal(reader["StCommissPercentage"]);
                        ciaParamAgencyCommiss.AdditionalCommissionPercentage = reader.IsDBNull(4) ? (Decimal?)null : Convert.ToDecimal(reader["AdditCommissPercentage"]);
                        ciaParamAgencyCommiss.ProductId = productId;


                        listCiaParamAgencyCommiss.Add(ciaParamAgencyCommiss);
                    }
                }
            }






            //Agentes
            SelectQuery selectQuery = new SelectQuery();
            selectQuery.AddSelectValue(new SelectValue(new Column(PRODEN.ProductAgent.Properties.ProductId, "p")));
            selectQuery.AddSelectValue(new SelectValue(new Column(PRODEN.ProductAgent.Properties.IndividualId, "p")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UPEN.Agent.Properties.CheckPayableTo, "pa")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UPEN.Agent.Properties.Locker, "pa")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UPEN.Agent.Properties.DeclinedDate, "pa")));
            #region Joins
            Join join = new Join(new ClassNameTable(typeof(PRODEN.ProductAgent), "p"), new ClassNameTable(typeof(UPEN.Agent), "pa"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(PRODEN.ProductAgent.Properties.IndividualId, "p")
                .Equal()
                .Property(UPEN.Agent.Properties.IndividualId, "pa")
                .GetPredicate());
            #endregion Joins
            selectQuery.Where = filter.GetPredicate();
            selectQuery.Table = join;
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                while (reader.Read())
                {
                    DateTime? valAgent = reader.IsDBNull(4) ? (DateTime?)null : (DateTime)reader["DeclinedDate"];
                    if (valAgent == null || valAgent > DateTime.Now)
                    {
                        result.AssignedAgents = result.AssignedAgents + 1;
                        //int number;
                        //CiaParamAgent ciaParamAgent = new CiaParamAgent();
                        //ciaParamAgent.IndividualId = Convert.ToInt32(reader["IndividualId"]);
                        //ciaParamAgent.FullName = (string)reader["CheckPayableTo"];
                        //ciaParamAgent.LockerId = Int32.TryParse((string)reader["Locker"], out number) ? Convert.ToInt32(reader["Locker"]) : 0;
                        //ciaParamAgent.ProductId = productId;
                        int agencyCommiss = listCiaParamAgencyCommiss.Where(x => x.IndividualId == Convert.ToInt32(reader["IndividualId"])).Count();
                        if (agencyCommiss > 0)
                        {
                            result.AgentsCommission = result.AgentsCommission + agencyCommiss;
                        }



                    }
                }
            }

            NameValue[] parameters = new NameValue[2];

            parameters[0] = new NameValue("PREFIXCD", prefixId);
            parameters[1] = new NameValue("PRODUCTID", productId);

            DataTable resultSP;

            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                resultSP = dynamicDataAccess.ExecuteSPDataTable("UP.GET_AGENT_PRODUCT", parameters);
            }

            if (resultSP != null && resultSP.Rows.Count > 0)
            {
                foreach (DataRow arrayItem in resultSP.Rows)
                {
                    result.UnassignedAgents = Convert.ToInt32(arrayItem[0]);
                }
            }

            return result;
        }

        public List<CiaParamAgent> GetProductAgentByProductIdByIndividualId(int productId, int individualId)
        {
            List<CiaParamAgent> result = new List<CiaParamAgent>();
            List<CiaParamAgencyCommiss> listCiaParamAgencyCommiss = new List<CiaParamAgencyCommiss>();
            List<CiaParamIncentive> listCiaParamIncentive = new List<CiaParamIncentive>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(PRODEN.ProductAgent.Properties.ProductId, productId);

            //Comisiones
            SelectQuery selectQueryComiss = new SelectQuery();
            selectQueryComiss.AddSelectValue(new SelectValue(new Column(PRODEN.ProductAgencyCommiss.Properties.ProductId, "p")));
            selectQueryComiss.AddSelectValue(new SelectValue(new Column(PRODEN.ProductAgencyCommiss.Properties.IndividualId, "p")));
            selectQueryComiss.AddSelectValue(new SelectValue(new Column(PRODEN.ProductAgencyCommiss.Properties.AgentAgencyId, "p")));
            selectQueryComiss.AddSelectValue(new SelectValue(new Column(PRODEN.ProductAgencyCommiss.Properties.StCommissPercentage, "p")));
            selectQueryComiss.AddSelectValue(new SelectValue(new Column(PRODEN.ProductAgencyCommiss.Properties.AdditCommissPercentage, "p")));
            selectQueryComiss.AddSelectValue(new SelectValue(new Column(UPEN.AgentAgency.Properties.Description, "pa")));
            selectQueryComiss.AddSelectValue(new SelectValue(new Column(UPEN.AgentAgency.Properties.AgentCode, "pa")));
            selectQueryComiss.AddSelectValue(new SelectValue(new Column(UPEN.AgentAgency.Properties.DeclinedDate, "pa")));
            #region Joins
            Join joinComiss = new Join(new ClassNameTable(typeof(PRODEN.ProductAgencyCommiss), "p"), new ClassNameTable(typeof(UPEN.AgentAgency), "pa"), JoinType.Inner);
            joinComiss.Criteria = (new ObjectCriteriaBuilder()
                .Property(PRODEN.ProductAgencyCommiss.Properties.IndividualId, "p")
                .Equal()
                .Property(UPEN.AgentAgency.Properties.IndividualId, "pa")
                .And()
                .Property(PRODEN.ProductAgencyCommiss.Properties.AgentAgencyId, "p")
                .Equal()
                .Property(UPEN.AgentAgency.Properties.AgentAgencyId, "pa")

                .GetPredicate());
            #endregion Joins
            selectQueryComiss.Where = filter.GetPredicate();
            selectQueryComiss.Table = joinComiss;
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQueryComiss))
            {
                while (reader.Read())
                {
                    //DateTime? valAgent = reader.IsDBNull(7) ? (DateTime?)null : (DateTime)reader["DeclinedDate"];
                    //if (valAgent == null || valAgent > DateTime.Now)
                    //{
                    CiaParamAgencyCommiss ciaParamAgencyCommiss = new CiaParamAgencyCommiss();
                    ciaParamAgencyCommiss.IndividualId = Convert.ToInt32(reader["IndividualId"]);
                    ciaParamAgencyCommiss.AgencyName = (string)reader["Description"];
                    ciaParamAgencyCommiss.AgencyId = Convert.ToInt32(reader["AgentAgencyId"]);
                    ciaParamAgencyCommiss.CommissPercentage = Convert.ToDecimal(reader["StCommissPercentage"]);
                    ciaParamAgencyCommiss.AdditionalCommissionPercentage = reader.IsDBNull(4) ? (Decimal?)null : Convert.ToDecimal(reader["AdditCommissPercentage"]);
                    ciaParamAgencyCommiss.ProductId = productId;


                    listCiaParamAgencyCommiss.Add(ciaParamAgencyCommiss);
                    //}
                }
            }






            //Agentes
            ObjectCriteriaBuilder filterP = new ObjectCriteriaBuilder();
            filterP.PropertyEquals(PRODEN.ProductAgent.Properties.ProductId, productId);
            filterP.And();
            filterP.Property(PRODEN.ProductAgent.Properties.IndividualId, "p");
            filterP.Equal();
            filterP.Constant(individualId);
            //filterP.And();
            //filterP.PropertyEquals(PRODEN.ProductAgent.Properties.IndividualId, individualId);

            SelectQuery selectQuery = new SelectQuery();
            selectQuery.AddSelectValue(new SelectValue(new Column(PRODEN.ProductAgent.Properties.ProductId, "p")));
            selectQuery.AddSelectValue(new SelectValue(new Column(PRODEN.ProductAgent.Properties.IndividualId, "p")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UPEN.Agent.Properties.CheckPayableTo, "pa")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UPEN.Agent.Properties.Locker, "pa")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UPEN.Agent.Properties.DeclinedDate, "pa")));
            #region Joins
            Join join = new Join(new ClassNameTable(typeof(PRODEN.ProductAgent), "p"), new ClassNameTable(typeof(UPEN.Agent), "pa"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(PRODEN.ProductAgent.Properties.IndividualId, "p")
                .Equal()
                .Property(UPEN.Agent.Properties.IndividualId, "pa")
                .GetPredicate());
            #endregion Joins
            selectQuery.Where = filterP.GetPredicate();
            selectQuery.Table = join;
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                while (reader.Read())
                {
                    DateTime? valAgent = reader.IsDBNull(4) ? (DateTime?)null : (DateTime)reader["DeclinedDate"];
                    if (valAgent == null || valAgent > DateTime.Now)
                    {
                        int number;
                        CiaParamAgent ciaParamAgent = new CiaParamAgent();
                        ciaParamAgent.IndividualId = Convert.ToInt32(reader["IndividualId"]);
                        ciaParamAgent.FullName = (string)reader["CheckPayableTo"];
                        ciaParamAgent.LockerId = Int32.TryParse((string)reader["Locker"], out number) ? Convert.ToInt32(reader["Locker"]) : 0;
                        ciaParamAgent.ProductId = productId;
                        ciaParamAgent.AgencyComiss = listCiaParamAgencyCommiss.Where(x => x.IndividualId == ciaParamAgent.IndividualId).ToList();

                        result.Add(ciaParamAgent);
                    }
                }
            }
            return result;
        }

        [Obsolete("Obsolet Method")]
        public List<CiaParamAgent> GetProductAgentByProductId(int productId)
        {
            List<CiaParamAgent> result = new List<CiaParamAgent>();
            List<CiaParamAgencyCommiss> listCiaParamAgencyCommiss = new List<CiaParamAgencyCommiss>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(PRODEN.ProductAgent.Properties.ProductId, productId);

            //Comisiones
            SelectQuery selectQueryComiss = new SelectQuery();
            selectQueryComiss.AddSelectValue(new SelectValue(new Column(PRODEN.ProductAgencyCommiss.Properties.ProductId, "p")));
            selectQueryComiss.AddSelectValue(new SelectValue(new Column(PRODEN.ProductAgencyCommiss.Properties.IndividualId, "p")));
            selectQueryComiss.AddSelectValue(new SelectValue(new Column(PRODEN.ProductAgencyCommiss.Properties.AgentAgencyId, "p")));
            selectQueryComiss.AddSelectValue(new SelectValue(new Column(PRODEN.ProductAgencyCommiss.Properties.StCommissPercentage, "p")));
            selectQueryComiss.AddSelectValue(new SelectValue(new Column(PRODEN.ProductAgencyCommiss.Properties.AdditCommissPercentage, "p")));
            selectQueryComiss.AddSelectValue(new SelectValue(new Column(UPEN.AgentAgency.Properties.Description, "pa")));
            selectQueryComiss.AddSelectValue(new SelectValue(new Column(UPEN.AgentAgency.Properties.AgentCode, "pa")));
            selectQueryComiss.AddSelectValue(new SelectValue(new Column(UPEN.AgentAgency.Properties.DeclinedDate, "pa")));
            #region Joins
            Join joinComiss = new Join(new ClassNameTable(typeof(PRODEN.ProductAgencyCommiss), "p"), new ClassNameTable(typeof(UPEN.AgentAgency), "pa"), JoinType.Inner);
            joinComiss.Criteria = (new ObjectCriteriaBuilder()
                .Property(PRODEN.ProductAgencyCommiss.Properties.IndividualId, "p")
                .Equal()
                .Property(UPEN.AgentAgency.Properties.IndividualId, "pa")
                .And()
                .Property(PRODEN.ProductAgencyCommiss.Properties.AgentAgencyId, "p")
                .Equal()
                .Property(UPEN.AgentAgency.Properties.AgentAgencyId, "pa")

                .GetPredicate());
            #endregion Joins
            selectQueryComiss.Where = filter.GetPredicate();
            selectQueryComiss.Table = joinComiss;
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQueryComiss))
            {
                while (reader.Read())
                {
                    //DateTime? valAgent = reader.IsDBNull(7) ? (DateTime?)null : (DateTime)reader["DeclinedDate"];
                    //if (valAgent == null || valAgent > DateTime.Now)
                    //{
                    CiaParamAgencyCommiss ciaParamAgencyCommiss = new CiaParamAgencyCommiss();
                    ciaParamAgencyCommiss.IndividualId = Convert.ToInt32(reader["IndividualId"]);
                    ciaParamAgencyCommiss.AgencyName = (string)reader["Description"];
                    ciaParamAgencyCommiss.AgencyId = Convert.ToInt32(reader["AgentAgencyId"]);
                    ciaParamAgencyCommiss.CommissPercentage = Convert.ToDecimal(reader["StCommissPercentage"]);
                    ciaParamAgencyCommiss.AdditionalCommissionPercentage = reader.IsDBNull(4) ? (Decimal?)null : Convert.ToDecimal(reader["AdditCommissPercentage"]);
                    ciaParamAgencyCommiss.ProductId = productId;


                    listCiaParamAgencyCommiss.Add(ciaParamAgencyCommiss);
                    //}
                }
            }






            //Agentes
            SelectQuery selectQuery = new SelectQuery();
            selectQuery.AddSelectValue(new SelectValue(new Column(PRODEN.ProductAgent.Properties.ProductId, "p")));
            selectQuery.AddSelectValue(new SelectValue(new Column(PRODEN.ProductAgent.Properties.IndividualId, "p")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UPEN.Agent.Properties.CheckPayableTo, "pa")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UPEN.Agent.Properties.Locker, "pa")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UPEN.Agent.Properties.DeclinedDate, "pa")));
            #region Joins
            Join join = new Join(new ClassNameTable(typeof(PRODEN.ProductAgent), "p"), new ClassNameTable(typeof(UPEN.Agent), "pa"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(PRODEN.ProductAgent.Properties.IndividualId, "p")
                .Equal()
                .Property(UPEN.Agent.Properties.IndividualId, "pa")
                .GetPredicate());
            #endregion Joins
            selectQuery.Where = filter.GetPredicate();
            selectQuery.Table = join;
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                while (reader.Read())
                {
                    DateTime? valAgent = reader.IsDBNull(4) ? (DateTime?)null : (DateTime)reader["DeclinedDate"];
                    if (valAgent == null || valAgent > DateTime.Now)
                    {
                        CiaParamAgent ciaParamAgent = new CiaParamAgent();
                        ciaParamAgent.IndividualId = Convert.ToInt32(reader["IndividualId"]);
                        ciaParamAgent.FullName = (string)reader["CheckPayableTo"];
                        ciaParamAgent.LockerId = Convert.ToInt32(reader["Locker"]);
                        ciaParamAgent.ProductId = productId;
                        ciaParamAgent.AgencyComiss = listCiaParamAgencyCommiss.Where(x => x.IndividualId == ciaParamAgent.IndividualId).ToList();
                        result.Add(ciaParamAgent);
                    }
                }
            }
            return result;
        }

        public bool DeleteAgent(int productId, int individualId)
        {
            try
            {
                PrimaryKey key = PRODEN.ProductAgent.CreatePrimaryKey(productId, individualId);
                PRODEN.ProductAgent ObjectEntities = (PRODEN.ProductAgent)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                DataFacadeManager.Instance.GetDataFacade().DeleteObject(ObjectEntities);

                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool DeleteAgents(List<CiaParamAgent> listCiaParamAgent, List<CiaParamAgent> listCiaParamAgentInsert)
        {
            try
            {
                //Daf
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                if (listCiaParamAgent != null && listCiaParamAgent.Count > 0)
                {
                    DeleteQuery deleteQuery = new DeleteQuery();
                    deleteQuery.Table = new ClassNameTable(typeof(PRODEN.ProductAgent));
                    bool deleteAgents = false;
                    int threadAgent = 0;
                    filter.Property(PRODEN.ProductAgent.Properties.IndividualId);
                    filter.In();
                    filter.ListValue();
                    foreach (CiaParamAgent item in listCiaParamAgent)
                    {
                        bool containsItem = listCiaParamAgentInsert.Any(x => x.IndividualId == item.IndividualId);
                        if (!containsItem)
                        {
                            filter.Constant(item.IndividualId);
                            deleteAgents = true;
                            threadAgent++;
                            if (threadAgent % 2000 == 0)
                            {
                                filter.EndList();
                                filter.And();
                                filter.PropertyEquals(PRODEN.ProductAgent.Properties.ProductId, listCiaParamAgent[0].ProductId);
                                deleteQuery.Where = filter.GetPredicate();
                                using (IDataFacade dataFacade = DataFacadeManager.Instance.GetDataFacade())
                                {
                                    dataFacade.Execute(deleteQuery);
                                }
                                //DataFacadeManager.Instance.GetDataFacade().Execute(deleteQuery);
                                //DataFacadeManager.Instance.GetDataFacade().Dispose();
                                deleteAgents = false;
                                filter = new ObjectCriteriaBuilder();
                                filter.Property(PRODEN.ProductAgent.Properties.IndividualId);
                                filter.In();
                                filter.ListValue();
                            }
                        }
                    }
                    filter.EndList();
                    filter.And();
                    filter.PropertyEquals(PRODEN.ProductAgent.Properties.ProductId, listCiaParamAgent[0].ProductId);
                    if (deleteAgents)
                    {
                        deleteQuery.Where = filter.GetPredicate();
                        using (IDataFacade dataFacade = DataFacadeManager.Instance.GetDataFacade())
                        {
                            dataFacade.Execute(deleteQuery);
                        }
                        //DataFacadeManager.Instance.GetDataFacade().Execute(deleteQuery);
                        //DataFacadeManager.Instance.GetDataFacade().DeleteObjects(typeof(PRODEN.ProductAgent), filter.GetPredicate());
                    }
                }

                ////SP
                //if (listCiaParamAgent != null && listCiaParamAgent[0] != null)
                //{
                //    string strIndividualId = (string.Join(",", listCiaParamAgent.Select(x => x.IndividualId.ToString()).ToArray()));
                //    NameValue[] parameters = new NameValue[2];
                //    parameters[0] = new NameValue("PRODUCT_ID", listCiaParamAgent[0].ProductId);
                //    parameters[1] = new NameValue("TXT_INDIVIDUAL_ID", strIndividualId);

                //    object result;

                //    using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                //    {
                //        result = dynamicDataAccess.ExecuteSPScalar("PROD.DELETE_PRODUCT_AGENT_PERIFERIC", parameters);
                //    }
                //}

                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool DeleteAgentComiss(int productId, int individualId, int agencyId)
        {
            try
            {
                PrimaryKey key = PRODEN.ProductAgencyCommiss.CreatePrimaryKey(individualId, agencyId, productId);
                PRODEN.ProductAgencyCommiss ObjectEntities = (PRODEN.ProductAgencyCommiss)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                DataFacadeManager.Instance.GetDataFacade().DeleteObject(ObjectEntities);

                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool UpdateAgentComiss(int productId, int individualId, int agencyId, decimal percentage, decimal? percentageAdd, decimal? schCommissPercentage, decimal? stDisCommissPercentage, decimal? additDisCommissPercentage)
        {
            try
            {
                PrimaryKey key = PRODEN.ProductAgencyCommiss.CreatePrimaryKey(individualId, agencyId, productId);
                PRODEN.ProductAgencyCommiss ObjectEntities = (PRODEN.ProductAgencyCommiss)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                ObjectEntities.StCommissPercentage = percentage;
                ObjectEntities.AdditCommissPercentage = percentageAdd;
                ObjectEntities.SchCommissPercentage = schCommissPercentage;
                ObjectEntities.StDisCommissPercentage = stDisCommissPercentage;
                ObjectEntities.AdditDisCommissPercentage = additDisCommissPercentage;
                DataFacadeManager.Instance.GetDataFacade().UpdateObject(ObjectEntities);

                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool InsertAgentComiss(int productId, int individualId, int agencyId, decimal percentage, decimal? percentageAdd, decimal? schCommissPercentage, decimal? stDisCommissPercentage, decimal? additDisCommissPercentage)
        {
            try
            {
                PRODEN.ProductAgencyCommiss itemProductAgencyCommiss = new PRODEN.ProductAgencyCommiss(individualId, agencyId, productId)
                {
                    StCommissPercentage = percentage,
                    AdditCommissPercentage = percentageAdd,
                    SchCommissPercentage = schCommissPercentage,
                    StDisCommissPercentage = stDisCommissPercentage,
                    AdditDisCommissPercentage = additDisCommissPercentage
                };
                DataFacadeManager.Instance.GetDataFacade().InsertObject(itemProductAgencyCommiss);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool InsertAgents(List<CiaParamAgent> listCiaParamAgent, List<CiaParamAgent> listCiaParamAgentDelete)
        {
            try
            {
                bool insertAgents = false;
                //Daf
                if (listCiaParamAgent != null && listCiaParamAgent.Count > 0)
                {
                    BusinessCollection busC = new BusinessCollection();
                    ConcurrentBag<BusinessObject> bo = new ConcurrentBag<BusinessObject>();
                    TP.Parallel.ForEach(listCiaParamAgent, x =>
                    {
                        bool containsItem = listCiaParamAgentDelete.Any(y => y.IndividualId == x.IndividualId);
                        if (!containsItem)
                        {
                            insertAgents = true;
                            PRODEN.ProductAgent bc = new PRODEN.ProductAgent(x.IndividualId, x.ProductId)
                            {
                                IndividualId = x.IndividualId,
                                ProductId = x.ProductId
                            };
                            bo.Add(bc);
                            //DataFacadeManager.Instance.GetDataFacade().InsertObject(bc);
                            //DataFacadeManager.Instance.GetDataFacade().Dispose();
                        }

                    });
                    busC.AddRange(bo.ToList());
                    if (insertAgents)
                    {
                        BusinessCollection threadInsert = new BusinessCollection();
                        for (int i = 0; i < bo.Count; i = i + 2000)
                        {

                            var items = bo.Skip(i).Take(2000).ToList();
                            threadInsert = new BusinessCollection();
                            threadInsert.AddRange(items);
                            //using (IDataFacade dataFacade = DataFacadeManager.Instance.GetDataFacade())
                            //{
                            //	dataFacade.InsertObjects(threadInsert);
                            //}
                            DataFacadeManager.Instance.GetDataFacade().InsertObjects(threadInsert);
                        }
                        //using (IDataFacade dataFacade = DataFacadeManager.Instance.GetDataFacade())
                        //{
                        //	dataFacade.BulkInsertObjects<PRODEN.ProductAgent>(busC);
                        //}
                        //DataFacadeManager.Instance.GetDataFacade().BulkInsertObjects<PRODEN.ProductAgent>(busC);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool InsertAgent(int productId, int individualId)
        {
            try
            {
                PRODEN.ProductAgent itemProductAgency = new PRODEN.ProductAgent(individualId, productId)
                {
                    IndividualId = individualId,
                    ProductId = productId
                };
                DataFacadeManager.Instance.GetDataFacade().InsertObject(itemProductAgency);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public string SaveAllAgents(int prefixId, int productId, bool assigned)
        {
            try
            {
                NameValue[] parameters = new NameValue[3];

                parameters[0] = new NameValue("PREFIXCD", prefixId);
                parameters[1] = new NameValue("PRODUCTID", productId);
                parameters[2] = new NameValue("ASSIGNED", assigned);

                DataTable resultSP;
                string result = string.Empty;

                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    resultSP = dynamicDataAccess.ExecuteSPDataTable("PROD.ASSIGNED_ALL_AGENTS", parameters);
                }

                if (resultSP != null && resultSP.Rows.Count > 0)
                {
                    foreach (DataRow arrayItem in resultSP.Rows)
                    {
                        result = arrayItem[0].ToString();
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion Agents
    }
}
