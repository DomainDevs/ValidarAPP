// -----------------------------------------------------------------------
// <copyright file="ProductDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Cristyan Fernando Ballesteros</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.ProductParamServices.EEProvider.DAOs
{
    using Sistran.Core.Application.Common.Entities;
    using Sistran.Core.Application.Utilities.DataFacade;
    using Sistran.Core.Framework.DAF;
    using System.Collections.Generic;
    using PRODEN = Product.Entities;
    /// <summary>
    /// Clase DAO del objeto Product.
    /// </summary>
    public class ProductDAO
    {
        #region Crud Producto
        /// <summary>
        /// crea un producto producto
        /// </summary>
        /// <param name="product">producto a crear</param>
        /// <returns></returns>
        public PRODEN.Product CreateProduct(PRODEN.Product product)
        {
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.InsertObject(product);
            }
            return product;
        }

        /// <summary>
        /// actualiza un producto producto
        /// </summary>
        /// <param name="product">producto a actualizar</param>
        /// <returns></returns>
        public PRODEN.Product UpdateProduct(PRODEN.Product product)
        {
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.UpdateObject(product);
            }
            return product;
        }

        /// <summary>
        /// elimina un producto producto
        /// </summary>
        /// <param name="product">producto a eliminar</param>
        /// <returns></returns>
        public void DeleteProduct(PRODEN.Product product)
        {
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.DeleteObject(product);
            }
        }

        #endregion

        /// <summary>
        /// Habilitar / Deshabilitar Producto
        /// </summary>
        /// <param name="productId">Id Producto</param>
        /// <param name="currentTo">Fecha Fin Habilitado</param>
        /// <returns>true o false</returns>
        //public virtual Boolean EnableDisableProduct(int productId, DateTime? currentTo)
        //{
        //    try
        //    {
        //        PrimaryKey key = PRODEN.Product.CreatePrimaryKey(productId);
        //        PRODEN.Product productEntity = (PRODEN.Product)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

        //        if (productEntity != null)
        //        {
        //            productEntity.CurrentTo = currentTo;
        //            using (var daf = DataFacadeManager.Instance.GetDataFacade())
        //            {
        //                daf.UpdateObject(productEntity);
        //            }
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}
        //    /// <summary>
        //    /// Obtener producto Completo
        //    /// </summary>
        //    /// <param name="productId">Id producto</param>
        //    /// <returns>Producto</returns>
        //    public Model.Product GetFullProductById(int id)
        //    {
        //        Stopwatch stopWatch = new Stopwatch();
        //        stopWatch.Start();

        //        bool IsUse = false;
        //        ProductRelatedEntitiesView view = null;
        //        ProductAgencyCommissionView viewProductAgencyCommission = null;
        //        FinancialPlanUnderwritingView financialPlanView = null;
        //        List<PARAMEN.RiskCommercialClass> riskCommercialClass = null;
        //        List<COMMEN.CoLimitsRc> limitsRc = null;
        //        try
        //        {
        //            List<Task> taskProduct = new List<Task>();
        //            IsUse = ValidatePolicyByProductId(id);
        //            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
        //            filter.Property(PRODEN.Product.Properties.ProductId, typeof(PRODEN.Product).Name).Equal().Constant(id);
        //            view = new ProductRelatedEntitiesView();
        //            ViewBuilder builder = new ViewBuilder("ProductRelatedEntitiesView");
        //            builder.Filter = filter.GetPredicate();
        //            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
        //            // Planes de pago
        //            filter = new ObjectCriteriaBuilder();
        //            filter.Property(PRODEN.ProductFinancialPlan.Properties.ProductId, typeof(PRODEN.ProductFinancialPlan).Name).Equal().Constant(id);
        //            financialPlanView = new FinancialPlanUnderwritingView();
        //            builder = new ViewBuilder("FinancialPlanUnderwritingView");
        //            builder.Filter = filter.GetPredicate();
        //            taskProduct.Add(Task.Run(() =>
        //            {
        //                DataFacadeManager.Instance.GetDataFacade().FillView(builder, financialPlanView);
        //                view.ProductFinancialPlanList = financialPlanView.ProductFinancialPlanList;
        //                view.FinancialPlanList = financialPlanView.FinancialPlanList;
        //                DataFacadeManager.Dispose();
        //            }));

        //            // Comisiones Agente por Producto
        //            filter = new ObjectCriteriaBuilder();
        //            filter.Property(PRODEN.ProductAgent.Properties.ProductId, typeof(PRODEN.ProductAgent).Name).Equal().Constant(id);
        //            viewProductAgencyCommission = new ProductAgencyCommissionView();
        //            ViewBuilder builderProductAgencyCommission = new ViewBuilder("ProductAgencyCommissionView");
        //            builderProductAgencyCommission.Filter = filter.GetPredicate();
        //            taskProduct.Add(Task.Run(() =>
        //            {
        //                DataFacadeManager.Instance.GetDataFacade().FillView(builderProductAgencyCommission, viewProductAgencyCommission);
        //                DataFacadeManager.Dispose();
        //            }));

        //            // Actividades del producto
        //            filter = new ObjectCriteriaBuilder();
        //            filter.Property(COMMEN.ProductRiskCommercialClass.Properties.ProductId, typeof(COMMEN.ProductRiskCommercialClass).Name).Equal().Constant(id);
        //            taskProduct.Add(Task.Run(() =>
        //            {
        //                view.ProductRiskCommercialClassList = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.ProductRiskCommercialClass), filter.GetPredicate()));
        //                riskCommercialClass = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(PARAMEN.RiskCommercialClass))).Cast<PARAMEN.RiskCommercialClass>().ToList();
        //                DataFacadeManager.Dispose();
        //            }));

        //            // deducibles del producto
        //            List<QUOEN.Deductible> deductibles = null;
        //            filter = new ObjectCriteriaBuilder();
        //            filter.Property(COMMEN.DeductibleProduct.Properties.ProductId, typeof(COMMEN.DeductibleProduct).Name).Equal().Constant(id);
        //            taskProduct.Add(Task.Run(() =>
        //            {
        //                view.DeductibleProductList = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.DeductibleProduct), filter.GetPredicate()));
        //                if (view.DeductibleProductList != null && view.DeductibleProductList.Count > 0)
        //                {
        //                    //filtro deducibles
        //                    ObjectCriteriaBuilder filterDeduct = new ObjectCriteriaBuilder();
        //                    filterDeduct.Property(QUOEN.Deductible.Properties.DeductId, typeof(QUOEN.Deductible).Name);
        //                    filterDeduct.In();
        //                    filterDeduct.ListValue();
        //                    Parallel.ForEach(view.DeductibleProductList.Cast<COMMEN.DeductibleProduct>().ToList(), deductible =>
        //                     {
        //                         filterDeduct.Constant(deductible.DeductId);
        //                     });
        //                    filterDeduct.EndList();
        //                    deductibles = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(QUOEN.Deductible), filterDeduct.GetPredicate())).Cast<QUOEN.Deductible>().ToList();
        //                }
        //                DataFacadeManager.Dispose();
        //            }));

        //            // Limites  de RC producto
        //            filter = new ObjectCriteriaBuilder();
        //            filter.Property(COMMEN.CoLimitsRcRel.Properties.ProductId, typeof(COMMEN.CoLimitsRcRel).Name).Equal().Constant(id);
        //            taskProduct.Add(Task.Run(() =>
        //            {
        //                view.CoLimitsRcRelList = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.CoLimitsRcRel), filter.GetPredicate()));
        //                limitsRc = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.CoLimitsRc))).Cast<COMMEN.CoLimitsRc>().ToList();
        //                DataFacadeManager.Dispose();
        //            }));

        //            // Formularios del producto
        //            filter = new ObjectCriteriaBuilder();
        //            filter.Property(PRODEN.ProductForm.Properties.ProductId, typeof(PRODEN.ProductForm).Name).Equal().Constant(id);
        //            taskProduct.Add(Task.Run(() =>
        //            {
        //                view.ProductFormList = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(PRODEN.ProductForm), filter.GetPredicate()));
        //                DataFacadeManager.Dispose();
        //            }));

        //            Task.WaitAll(taskProduct.ToArray());
        //            if (view != null)
        //            {
        //                Model.Product product = ConvertToModelFullProduct(view, viewProductAgencyCommission);
        //                if (product.ProductCommercialClass != null)
        //                {
        //                    Parallel.ForEach(product.ProductCommercialClass, commercialClass =>
        //{
        //    commercialClass.RiskCommercialClass.Description = riskCommercialClass.FirstOrDefault(x => x.RiskCommercialClassCode == commercialClass.RiskCommercialClass.RiskCommercialClassCode).Description;

        //});
        //                }
        //                if (product.LimitRC != null)
        //                {
        //                    Parallel.ForEach(product.LimitRC, limitRc =>
        //                    {
        //                        limitRc.Description = limitsRc.FirstOrDefault(x => x.LimitRcCode == limitRc.Id).Description;
        //                    });
        //                }
        //                product.IsUse = IsUse;
        //                // product.de deductibles                   
        //                if (deductibles != null && deductibles.Count > 0)
        //                {
        //                    ConcurrentBag<Model.DeductibleProduct> deductibleProduct = new ConcurrentBag<Model.DeductibleProduct>();
        //                    Parallel.ForEach(deductibles, deductible =>
        //                    {
        //                        deductibleProduct.Add(new Model.DeductibleProduct { DeductId = deductible.DeductId, Description = deductible.Description });
        //                    });
        //                    product.DeductibleProduct = deductibleProduct.ToList();
        //                }
        //                stopWatch.Stop();
        //                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.ProductServices.EEProvider.DAOs.GetFullProductById");

        //                return product;
        //            }
        //            else
        //            {
        //                stopWatch.Stop();
        //                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.ProductServices.EEProvider.DAOs.GetFullProductById");

        //                return null;
        //            }
        //        }
        //        catch (Exception exc)
        //        {
        //            throw new BusinessException(exc.Message, exc);
        //        }

        //    }

        /// <summary>
        /// Converts to model full product.
        /// </summary>
        /// <param name="productView">The product view.</param>
        /// <param name="productAgentView">The product agent view.</param>
        /// <returns></returns>
        //public Model.Product ConvertToModelFullProduct(ProductRelatedEntitiesView productView, ProductAgencyCommissionView productAgentView)
        //{
        //    List<Model.GroupCoverage> groupCoverages = null;
        //    List<CommonModel.Currency> currencies = null;
        //    List<CommonModel.PolicyType> policyTypes = null;
        //    List<CommonModel.ProductPolicyType> productPolicyTypes = null;
        //    List<Model.GroupCoverage> groupCoverageAllieds = null;
        //    List<Model.ProductAgent> productAgents = null;
        //    List<Model.CoveredRisk> productCoveredRisks = null;
        //    List<Model.FinancialPlan> financialPlans = null;
        //    List<Model.ProductRiskCommercialClass> productRiskCommercialClass = null;
        //    List<Model.DeductibleProduct> deductibleProduct = null;
        //    List<Model.LimitRCRelation> limitRCRelation = null;
        //    List<Model.ProductForm> productForm = null;

        //    PRODEN.Product product = ((PRODEN.Product)productView.ProductList[0]);
        //    if (productView.CurrencyList != null && productView.CurrencyList.Count > 0)
        //    {
        //        currencies = ModelAssembler.CreateCurrencies(productView.CurrencyList);
        //    }
        //    if (productView.CoPolicyTypes != null && productView.CoPolicyTypes.Count > 0)
        //    {
        //        policyTypes = ModelAssembler.CreatePolicyTypes(productView.CoPolicyTypes);
        //    }
        //    if (productView.CoProductPolicyTypes != null && productView.CoProductPolicyTypes.Count > 0)
        //    {
        //        productPolicyTypes = ModelAssembler.CreateProductPolicyTypes(productView.CoProductPolicyTypes);
        //        Parallel.ForEach(policyTypes, policyType =>
        //        {
        //            policyType.IsDefault = productPolicyTypes.FirstOrDefault(x => x.Id == policyType.Id)?.IsDefault ?? false;
        //        });
        //    }
        //    if (productView.ProductGroupCoverageList != null && productView.ProductGroupCoverageList.Count > 0)
        //    {
        //        object obj = new object();
        //        groupCoverages = ModelAssembler.CreateGroupCoveragesByProducts(productView.ProductGroupCoverageList);
        //        Parallel.ForEach(productView.ProductGroupCoverageList.Cast<PRODEN.ProductGroupCover>().ToList(), productGroupCover =>
        //       {
        //           List<Models.Coverage> Coverages = new List<Models.Coverage>();
        //           BusinessCollection groupCoverage = productView.GetGroupCoverageListByProdGroupCoverage(productGroupCover);
        //           Parallel.ForEach(groupCoverage.Cast<PRODEN.GroupCoverage>().ToList(), groupCoverageEntities =>
        //            {
        //                var GroupInsuredObjectList = productView.GroupInsuredObjectList.Cast<PRODEN.GroupInsuredObject>().ToList().Where(x => x.CoverageGroupCode == groupCoverageEntities.CoverGroupId).Select(x => new Model.InsuredObject { Id = x.InsuredObject, IsSelected = x.IsSelected, IsMandatory = x.IsMandatory, Description = "" }).ToList();
        //                var ProdGroupInsuredObjectList =
        //                (from groupInsuredObject in GroupInsuredObjectList
        //                 join insuredObject in productView.InsuredObjectList.Cast<QUOEN.InsuredObject>().ToList()
        //                 on groupInsuredObject.Id equals insuredObject.InsuredObjectId
        //                 select new Model.InsuredObject { Id = groupInsuredObject.Id, IsSelected = groupInsuredObject.IsSelected, IsMandatory = groupInsuredObject.IsMandatory, Description = insuredObject.Description }).ToList();
        //                if (ProdGroupInsuredObjectList != null)
        //                {
        //                    groupCoverages.Where(x => x.Id == groupCoverageEntities.CoverGroupId).FirstOrDefault().InsuredObjects = (List<Model.InsuredObject>)ProdGroupInsuredObjectList;
        //                }
        //                QUOEN.Coverage coverage = productView.GetCoverageByGroupCoverage(groupCoverageEntities);
        //                if (coverage != null)
        //                {
        //                    Models.Coverage cover = ModelAssembler.CreateCoverage(coverage);

        //                    //Linea negocio                        
        //                    cover.PosRuleSetId = groupCoverageEntities.PosRuleSetId;
        //                    cover.RuleSetId = groupCoverageEntities.RuleSetId;
        //                    cover.ScriptId = groupCoverageEntities.ScriptId;
        //                    cover.IsMandatory = groupCoverageEntities.IsMandatory;
        //                    cover.IsSelected = groupCoverageEntities.IsSelected;
        //                    cover.IsSublimit = groupCoverageEntities.IsSublimit;
        //                    cover.MainCoverageId = groupCoverageEntities.MainCoverageId;
        //                    cover.MainCoveragePercentage = groupCoverageEntities.MainCoveragePercentage;
        //                    cover.CoverNum = groupCoverageEntities.CoverNum;

        //                    if (cover.SubLineBusiness != null)
        //                    {
        //                        cover.SubLineBusiness.Description = productView.SubLineBusinessList.Cast<COMMEN.SubLineBusiness>().ToList().Where(x => x.LineBusinessCode == cover.SubLineBusiness.LineBusiness.Id && x.SubLineBusinessCode == cover.SubLineBusiness.Id).Select(x => x.Description).FirstOrDefault();
        //                        if (cover.SubLineBusiness.LineBusiness != null)
        //                        {
        //                            cover.SubLineBusiness.LineBusiness.Description = productView.LineBusinessList.Cast<COMMEN.LineBusiness>().ToList().Where(x => x.LineBusinessCode == cover.SubLineBusiness.LineBusiness.Id).Select(x => x.Description).FirstOrDefault();
        //                        }
        //                    }
        //                    //Objeto del seguro
        //                    cover.InsuredObject.Description = productView.InsuredObjectList.Cast<QUOEN.InsuredObject>().ToList().Where(x => x.InsuredObjectId == cover.InsuredObject.Id).Select(x => x.Description).FirstOrDefault();
        //                    lock (obj)
        //                    {
        //                        Coverages.Add(cover);
        //                    }
        //                }
        //            });
        //           lock (obj)
        //           {
        //               groupCoverages.Where(x => x.Id == productGroupCover.CoverGroupId).FirstOrDefault().Coverages = Coverages;
        //               groupCoverages.Where(x => x.Id == productGroupCover.CoverGroupId).FirstOrDefault().Product = null;
        //           }
        //       });
        //    }
        //    //Planes de pago
        //    if (productView.ProductFinancialPlanList != null && productView.ProductFinancialPlanList.Count > 0)
        //    {
        //        ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

        //        filter.Property("FinancialPlanId", typeof(PRODEN.FinancialPlan).Name);
        //        filter.In();
        //        filter.ListValue();
        //        foreach (PRODEN.ProductFinancialPlan item in productView.ProductFinancialPlanList)
        //        {
        //            filter.Constant(item.FinancialPlanId);
        //        }
        //        filter.EndList();
        //        financialPlans = GetPaymentSchudeleByFilter(filter.GetPredicate());

        //        var financialPlansAll = (from financialPlan in financialPlans
        //                                 join productFinancialPlanList in
        //                                     productView.ProductFinancialPlanList.Cast<PRODEN.ProductFinancialPlan>().ToList()
        //                                     on financialPlan.Id equals productFinancialPlanList.FinancialPlanId
        //                                 select new Model.FinancialPlan { Id = productFinancialPlanList.FinancialPlanId, Currency = financialPlan.Currency, PaymentMethod = financialPlan.PaymentMethod, PaymentSchedule = financialPlan.PaymentSchedule, IsDefault = productFinancialPlanList.IsDefault }).ToList();
        //        financialPlans = financialPlansAll;
        //    }
        //    if (productView.ProductCoveredRiskTypeList != null && productView.ProductCoveredRiskTypeList.Count > 0)
        //    {
        //        productCoveredRisks = ModelAssembler.CreateCoveredRisks(productView.ProductCoveredRiskTypeList);
        //        if (productView.CoveredRiskTypeList != null)
        //        {
        //            Parallel.ForEach(productCoveredRisks, item =>
        //            {
        //                item.Description = productView.CoveredRiskTypeList.Cast<PARAMEN.CoveredRiskType>().ToList().FirstOrDefault(z => z.CoveredRiskTypeCode == (int)item.CoveredRiskType).SmallDescription;
        //            });
        //        }
        //    }

        //    productAgents = AdjustProductsAgent(productAgentView);

        //    //actividades (datos adicionales)
        //    if (productView.ProductRiskCommercialClassList != null)
        //    {
        //        productRiskCommercialClass = new List<Models.ProductRiskCommercialClass>();
        //        ConcurrentBag<Model.ProductRiskCommercialClass> riskCommercialClass = new ConcurrentBag<Models.ProductRiskCommercialClass>();
        //        //COMMEN.ProductRiskCommercialClass item
        //        Parallel.ForEach(productView.ProductRiskCommercialClassList.Cast<COMMEN.ProductRiskCommercialClass>().ToList(), item =>
        //        {
        //            riskCommercialClass.Add(new Models.ProductRiskCommercialClass
        //            {
        //                DefaultRiskCommercial = item.DefaultRiskCommercial,
        //                RiskCommercialClass = new Models.RiskCommercialClass { RiskCommercialClassCode = item.RiskCommercialClassCode }
        //            });
        //        });
        //        productRiskCommercialClass = riskCommercialClass.ToList();
        //    }

        //    //limite Rc (datos adicionales)
        //    if (productView.CoLimitsRcRelList != null)
        //    {
        //        limitRCRelation = new List<Model.LimitRCRelation>();
        //        ConcurrentBag<Model.LimitRCRelation> limitRC = new ConcurrentBag<Models.LimitRCRelation>();

        //        Parallel.ForEach(productView.CoLimitsRcRelList.Cast<COMMEN.CoLimitsRcRel>().ToList(), item =>
        //        {
        //            limitRC.Add(new Model.LimitRCRelation
        //            {
        //                Id = item.LimitRcCode,
        //                IsDefault = item.IsDefault,
        //                PolicyType = new CommonModel.PolicyType { Id = item.PolicyTypeCode }
        //            });
        //        }
        //        );
        //        limitRCRelation = limitRC.ToList();
        //    }

        //    //Formas de impresion (datos adicionales)
        //    if (productView.ProductFormList != null)
        //    {
        //        productForm = new List<Model.ProductForm>();
        //        ConcurrentBag<Model.ProductForm> form = new ConcurrentBag<Models.ProductForm>();

        //        Parallel.ForEach(productView.ProductFormList.Cast<PRODEN.ProductForm>().ToList(), item =>
        //        {
        //            form.Add(new Model.ProductForm
        //            {
        //                CurrentFrom = item.CurrentFrom,
        //                StrCurrentFrom = item.CurrentFrom.ToString(),
        //                FormId = item.FormId,
        //                FormNumber = item.FormNumber,
        //                GroupCoverage = new Models.GroupCoverage { Id = item.CoverGroupId }
        //            });
        //        }
        //        );
        //        productForm = form.ToList();
        //    }

        //    Model.Product productFull = new Model.Product
        //    {
        //        Id = product.ProductId,
        //        Version = product.Version.Value,

        //        //Prefix = new CommonModel.Prefix { Id = product.PrefixCode, Description = productView.PrefixList.Cast<COMMEN.Prefix>().ToList().FirstOrDefault(x => x.PrefixCode == product.PrefixCode)?.Description },
        //        AdditDisCommissPercentage = product.AdditionalCommissionPercentage,
        //        IsGreen = product.IsGreen,
        //        Description = product.Description,
        //        SmallDescription = product.SmallDescription,
        //        IncrementCommisionAdjustFactorPercentage = product.IncCommAdjustFactorPercentage,
        //        DecrementCommisionAdjustFactorPercentage = product.DecCommAdjustFactorPercentage,
        //        PreRuleSetId = product.PreRuleSetId,
        //        RuleSetId = product.RuleSetId,
        //        ScriptId = product.ScriptId,
        //        AdditionalCommissionPercentage = product.AdditCommissPercentage,
        //        StandardCommissionPercentage = product.StandardCommissionPercentage,
        //        StdDiscountCommPercentage = product.StdDiscountCommPercentage,
        //        SurchargeCommissionPercentage = product.SurchargeCommissionPercentage,
        //        IsCollective = (bool)product.IsCollective,
        //        IsRequest = product.IsRequest,
        //        IsFlatRate = product.IsFlatRate,
        //        CurrentFrom = product.CurrentFrom,
        //        CurrentTo = product.CurrentTo,
        //        //Currencies = currencies,
        //        //PolicyTypes = policyTypes,
        //        //GroupCoverages = groupCoverages,
        //        //FinancialPlans = financialPlans,
        //        //GroupCoverageAllieds = groupCoverageAllieds,
        //        //ProductAgents = productAgents,
        //        //ProductCoveredRisks = productCoveredRisks,
        //        //ProductCommercialClass = productRiskCommercialClass,
        //        //DeductibleProduct = deductibleProduct,
        //        //LimitRC = limitRCRelation,
        //        //ProductForm = productForm
        //    };
        //    return productFull;
        //}

        /// <summary>
        /// Converts to entities product.
        /// </summary>
        /// <param name="product">The product.</param>
        /// <returns></returns>
        //public PRODEN.Product ConvertToEntitiesProduct(Model.Product product)
        //{
        //    PRODEN.Product productEntities = new PRODEN.Product
        //    {
        //        //PrefixCode = product.Prefix.Id,
        //        AdditionalCommissionPercentage = product.AdditDisCommissPercentage,
        //        IsGreen = product.IsGreen,
        //        Description = product.Description,
        //        SmallDescription = product.SmallDescription,
        //        IncCommAdjustFactorPercentage = product.IncrementCommisionAdjustFactorPercentage,
        //        DecCommAdjustFactorPercentage = product.DecrementCommisionAdjustFactorPercentage,
        //        PreRuleSetId = product.PreRuleSetId,
        //        RuleSetId = product.RuleSetId,
        //        ScriptId = product.ScriptId,
        //        AdditCommissPercentage = product.AdditionalCommissionPercentage,
        //        StandardCommissionPercentage = product.StandardCommissionPercentage,
        //        StdDiscountCommPercentage = product.StdDiscountCommPercentage,
        //        SurchargeCommissionPercentage = product.SurchargeCommissionPercentage,
        //        IsCollective = product.IsCollective,
        //        IsRequest = product.IsRequest,
        //        IsFlatRate = product.IsFlatRate,
        //        CurrentFrom = product.CurrentFrom,
        //        CurrentTo = product.CurrentTo,
        //        Version = product.Version
        //    };
        //    return productEntities;
        //}
        //#endregion

        /// <summary>
        /// Creates the full product.
        /// </summary>
        /// <param name="product">The product.</param>
        /// <returns></returns>
        //public Model.Product CreateFullProduct(Model.Product product)
        //{
        //    Stopwatch stopWatch = new Stopwatch();
        //    stopWatch.Start();

        //    PrimaryKey key = PRODEN.Product.CreatePrimaryKey(product.Id);
        //    PRODEN.Product ProductEntities = (PRODEN.Product)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
        //    if (ProductEntities != null)
        //    {
        //        return UpdateFullProduct(product, ProductEntities);

        //    }
        //    else
        //    {
        //        ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
        //        filter.PropertyEquals(PRODEN.Product.Properties.Description, product.Description);
        //        ProductEntities = (PRODEN.Product)DataFacadeManager.Instance.GetDataFacade().List(typeof(PRODEN.Product), filter.GetPredicate()).FirstOrDefault();
        //        if (ProductEntities == null)
        //        {
        //            stopWatch.Stop();
        //            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.ProductServices.EEProvider.DAOs.CreateFullProduct");

        //            return SaveFullProduct(product);
        //        }
        //        else
        //        {
        //            stopWatch.Stop();
        //            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.ProductServices.EEProvider.DAOs.CreateFullProduct");

        //            throw new BusinessException("Ya exite un Producto con El nombre " + product.Description);
        //        }
        //    }
        //}

        /// <summary>
        /// guarda el producto completo
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        //private Model.Product SaveFullProduct(Model.Product product)
        //{
        //    Stopwatch stopWatch = new Stopwatch();
        //    stopWatch.Start();

        //    //Guardar con transaccion
        //    if (product == null)
        //    {
        //        throw new BusinessException("Producto Vacio");
        //    }
        //    else
        //    {
        //        using (Context.Current)
        //        {
        //            using (Transaction transaction = new Transaction())
        //            {
        //                try
        //                {
        //                    PRODEN.Product ProductEntities = ConvertToEntitiesProduct(product);
        //                    DataFacadeManager.Instance.GetDataFacade().InsertObject(ProductEntities);
        //                    product.Id = ProductEntities.ProductId;
        //                    /////Agentes Producto
        //                    //if (product.ProductAgents != null && product.ProductAgents.Count > 0)
        //                    //{
        //                    //    foreach (Model.ProductAgent productAgent in product.ProductAgents)
        //                    //    {
        //                    //        productAgent.ProductId = product.Id;
        //                    //        PRODEN.ProductAgent item = EntityAssembler.CreateProductAgent(productAgent);
        //                    //        DataFacadeManager.Instance.GetDataFacade().InsertObject(item);
        //                    //        //Commision Agentes
        //                    //        if (productAgent.ProductAgencyCommiss != null && productAgent.ProductAgencyCommiss.Count > 0)
        //                    //        {
        //                    //            foreach (Model.ProductAgencyCommiss productAgencyCommiss in productAgent.ProductAgencyCommiss)
        //                    //            {
        //                    //                productAgencyCommiss.ProductId = product.Id;
        //                    //                PRODEN.ProductAgencyCommiss itemComission = EntityAssembler.CreateProductAgentCommission(productAgencyCommiss);
        //                    //                DataFacadeManager.Instance.GetDataFacade().InsertObject(itemComission);
        //                    //            }
        //                    //        }

        //                    //    }
        //                    //}
        //                    ////Planes de Pago
        //                    //if (product.FinancialPlans != null && product.FinancialPlans.Count > 0)
        //                    //{
        //                    //    foreach (Model.FinancialPlan productFinancialPlan in product.FinancialPlans)
        //                    //    {
        //                    //        PRODEN.ProductFinancialPlan item = EntityAssembler.CreateProductFinancialPlan(productFinancialPlan, product.Id);
        //                    //        DataFacadeManager.Instance.GetDataFacade().InsertObject(item);
        //                    //    }
        //                    //}
        //                    ////Monedas
        //                    //if (product.Currencies != null && product.Currencies.Count > 0)
        //                    //{
        //                    //    foreach (CommonModel.Currency currency in product.Currencies)
        //                    //    {
        //                    //        PRODEN.ProductCurrency item = EntityAssembler.CreateProductCurrency(currency, product.Id);
        //                    //        DataFacadeManager.Instance.GetDataFacade().InsertObject(item);
        //                    //    }
        //                    //}
        //                    ////Tipos de Poliza
        //                    //if (product.PolicyTypes != null && product.PolicyTypes.Count > 0)
        //                    //{
        //                    //    foreach (CommonModel.PolicyType policyType in product.PolicyTypes)
        //                    //    {
        //                    //        policyType.Prefix = new CommonModel.Prefix { Id = product.Prefix.Id };
        //                    //        PRODEN.CoProductPolicyType item = EntityAssembler.CreateCoProductPolicyType(policyType, product.Id);
        //                    //        DataFacadeManager.Instance.GetDataFacade().InsertObject(item);
        //                    //    }
        //                    //}
        //                    ////Tipos de Riesgo
        //                    //if (product.ProductCoveredRisks != null && product.ProductCoveredRisks.Count > 0)
        //                    //{
        //                    //    foreach (Model.CoveredRisk coveredRisk in product.ProductCoveredRisks)
        //                    //    {
        //                    //        PRODEN.ProductCoverRiskType item = EntityAssembler.CreateProductCoverRiskType(coveredRisk, product.Id);
        //                    //        DataFacadeManager.Instance.GetDataFacade().InsertObject(item);
        //                    //    }
        //                    //}
        //                    ////Grupo de cobertura y objetos del seguro
        //                    //if (product.GroupCoverages != null && product.GroupCoverages.Count > 0)
        //                    //{

        //                    //    foreach (Model.GroupCoverage groupCoverage in product.GroupCoverages)
        //                    //    {
        //                    //        groupCoverage.Product = new Model.Product { Id = product.Id };
        //                    //        groupCoverage.Product.Prefix = new CommonModel.Prefix { Id = product.Prefix.Id };
        //                    //        //Grupo de cobertura tipos de Riesgo
        //                    //        if (groupCoverage.CoveredRiskType != null)
        //                    //        {
        //                    //            PRODEN.ProductGroupCover item = EntityAssembler.CreateProductGroupCover(groupCoverage);
        //                    //            DataFacadeManager.Instance.GetDataFacade().InsertObject(item);
        //                    //        }
        //                    //        //Grupo coberturas coberturas
        //                    //        if (groupCoverage.Coverages != null && groupCoverage.Coverages.Count > 0)
        //                    //        {
        //                    //            foreach (Model.Coverage Coverage in groupCoverage.Coverages)
        //                    //            {
        //                    //                PRODEN.GroupCoverage item = EntityAssembler.CreateGroupCoverageByCoverage(groupCoverage, Coverage);
        //                    //                DataFacadeManager.Instance.GetDataFacade().InsertObject(item);
        //                    //            }
        //                    //        }
        //                    //        //Grupo coberturas Objetos del seguro
        //                    //        if (groupCoverage.InsuredObjects != null && groupCoverage.InsuredObjects.Count > 0)
        //                    //        {
        //                    //            foreach (Model.InsuredObject insuredObject in groupCoverage.InsuredObjects)
        //                    //            {
        //                    //                PRODEN.GroupInsuredObject item = EntityAssembler.CreateProductGroupInsuredObject(insuredObject, groupCoverage, product.Id);
        //                    //                DataFacadeManager.Instance.GetDataFacade().InsertObject(item);
        //                    //            }
        //                    //        }
        //                    //    }

        //                    //}

        //                    ////actividades (datos adicionales)
        //                    //if (product.ProductCommercialClass != null)
        //                    //{
        //                    //    foreach (Model.ProductRiskCommercialClass modelo in product.ProductCommercialClass)
        //                    //    {
        //                    //        COMMEN.ProductRiskCommercialClass item = EntityAssembler.CreateProductCommercialClass(modelo, product.Id);
        //                    //        DataFacadeManager.Instance.GetDataFacade().InsertObject(item);
        //                    //    }
        //                    //}

        //                    ////deducibles (datos adicionales)
        //                    //if (product.DeductibleProduct != null)
        //                    //{
        //                    //    foreach (Model.DeductibleProduct modelo in product.DeductibleProduct)
        //                    //    {
        //                    //        COMMEN.DeductibleProduct item = EntityAssembler.CreateDeductibleProduct(modelo, product.Id);
        //                    //        DataFacadeManager.Instance.GetDataFacade().InsertObject(item);
        //                    //    }
        //                    //}

        //                    ////Limites RC (datos adicionales)
        //                    //if (product.LimitRC != null)
        //                    //{
        //                    //    foreach (Model.LimitRCRelation limitRCRelation in product.LimitRC)
        //                    //    {
        //                    //        COMMEN.CoLimitsRcRel item = EntityAssembler.CreateLimitRCRelation(limitRCRelation, product.Prefix.Id, product.Id);
        //                    //        DataFacadeManager.Instance.GetDataFacade().InsertObject(item);
        //                    //    }
        //                    //}

        //                    ////formas de pago (datos adicionales)
        //                    //if (product.ProductForm != null)
        //                    //{
        //                    //    foreach (Model.ProductForm modelo in product.ProductForm)
        //                    //    {
        //                    //        modelo.Product = new Model.Product { Id = product.Id };
        //                    //        PRODEN.ProductForm item = EntityAssembler.CreateProductForm(modelo);
        //                    //        DataFacadeManager.Instance.GetDataFacade().InsertObject(item);
        //                    //    }
        //                    //}
        //                    transaction.Complete();
        //                    stopWatch.Stop();
        //                    Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.ProductServices.EEProvider.DAOs.SaveFullProduct");

        //                    return product;
        //                }
        //                catch (Exception ex)
        //                {
        //                    stopWatch.Stop();
        //                    Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.ProductServices.EEProvider.DAOs.SaveFullProduct");

        //                    transaction.Dispose();
        //                    throw new BusinessException("Error al guardar producto" + ex.Message);
        //                }

        //            }
        //        }
        //    }

        //}


        /// <summary>
        /// actualiza el producto completo
        /// </summary>
        /// <param name="product"></param>
        /// <param name="ProductEntities"></param>
        /// <returns></returns>
        //private Model.Product UpdateFullProduct(Model.Product product, PRODEN.Product ProductEntities)
        //{
        //    using (Context.Current)
        //    {
        //        using (Transaction transaction = new Transaction())
        //        {

        //            try
        //            {
        //                //ELimina todos los items del producto                     
        //                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
        //                filter.PropertyEquals(PRODEN.ProductAgent.Properties.ProductId, product.Id);
        //                DataFacadeManager.Instance.GetDataFacade().Delete<PRODEN.ProductCurrency>(filter.GetPredicate());
        //                DataFacadeManager.Instance.GetDataFacade().Delete<PRODEN.ProductForm>(filter.GetPredicate());
        //                DataFacadeManager.Instance.GetDataFacade().Delete<COMMEN.ProductRiskCommercialClass>(filter.GetPredicate());
        //                DataFacadeManager.Instance.GetDataFacade().Delete<PRODEN.ProductAgencyCommiss>(filter.GetPredicate());
        //                DataFacadeManager.Instance.GetDataFacade().Delete<PRODEN.ProductAgent>(filter.GetPredicate());
        //                DataFacadeManager.Instance.GetDataFacade().Delete<PRODEN.ProductFinancialPlan>(filter.GetPredicate());
        //                DataFacadeManager.Instance.GetDataFacade().Delete<COMMEN.DeductibleProduct>(filter.GetPredicate());
        //                DataFacadeManager.Instance.GetDataFacade().Delete<COMMEN.CoLimitsRcRel>(filter.GetPredicate());
        //                DataFacadeManager.Instance.GetDataFacade().Delete<PRODEN.GroupCoverage>(filter.GetPredicate());
        //                DataFacadeManager.Instance.GetDataFacade().Delete<PRODEN.ProductGroupCover>(filter.GetPredicate());
        //                DataFacadeManager.Instance.GetDataFacade().Delete<PRODEN.GroupInsuredObject>(filter.GetPredicate());
        //                //Producto
        //                CreateProductEntity(product, ref ProductEntities);
        //                ProductEntities.Version = ProductEntities.Version + 1;
        //                DataFacadeManager.Instance.GetDataFacade().UpdateObject(ProductEntities);
        //                ///Agentes Producto
        //                if (product.ProductAgents != null && product.ProductAgents.Count > 0)
        //                {
        //                    foreach (Model.ProductAgent productAgent in product.ProductAgents)
        //                    {
        //                        productAgent.ProductId = product.Id;
        //                        PRODEN.ProductAgent item = EntityAssembler.CreateProductAgent(productAgent);
        //                        DataFacadeManager.Instance.GetDataFacade().InsertObject(item);
        //                        //Commision Agentes
        //                        if (productAgent.ProductAgencyCommiss != null && productAgent.ProductAgencyCommiss.Count > 0)
        //                        {
        //                            foreach (Model.ProductAgencyCommiss productAgencyCommiss in productAgent.ProductAgencyCommiss)
        //                            {
        //                                productAgencyCommiss.ProductId = product.Id;
        //                                PRODEN.ProductAgencyCommiss itemComission = EntityAssembler.CreateProductAgentCommission(productAgencyCommiss);
        //                                DataFacadeManager.Instance.GetDataFacade().InsertObject(itemComission);
        //                            }
        //                        }

        //                    }
        //                }

        //                //Planes de Pago
        //                if (product.FinancialPlans != null && product.FinancialPlans.Count > 0)
        //                {
        //                    foreach (Model.FinancialPlan productFinancialPlan in product.FinancialPlans)
        //                    {
        //                        PRODEN.ProductFinancialPlan item = EntityAssembler.CreateProductFinancialPlan(productFinancialPlan, product.Id);
        //                        DataFacadeManager.Instance.GetDataFacade().InsertObject(item);
        //                    }
        //                }

        //                //Monedas
        //                if (product.Currencies != null && product.Currencies.Count > 0)
        //                {
        //                    foreach (CommonModel.Currency currency in product.Currencies)
        //                    {
        //                        PRODEN.ProductCurrency item = EntityAssembler.CreateProductCurrency(currency, product.Id);
        //                        DataFacadeManager.Instance.GetDataFacade().InsertObject(item);
        //                    }
        //                }
        //                //Tipos de Poliza
        //                if (product.PolicyTypes != null && product.PolicyTypes.Count > 0)
        //                {
        //                    foreach (CommonModel.PolicyType policyType in product.PolicyTypes)
        //                    {
        //                        policyType.Prefix = new CommonModel.Prefix { Id = product.Prefix.Id };
        //                        PRODEN.CoProductPolicyType item = EntityAssembler.CreateCoProductPolicyType(policyType, product.Id);
        //                        PrimaryKey key = PRODEN.CoProductPolicyType.CreatePrimaryKey(product.Id, policyType.Prefix.Id, policyType.Id);
        //                        PRODEN.CoProductPolicyType coProductPolicyTypeEntity = (PRODEN.CoProductPolicyType)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
        //                        if (coProductPolicyTypeEntity != null)
        //                        {
        //                            coProductPolicyTypeEntity.IsDefault = policyType.IsDefault;
        //                            DataFacadeManager.Instance.GetDataFacade().UpdateObject(coProductPolicyTypeEntity);
        //                        }
        //                        else
        //                        {
        //                            DataFacadeManager.Instance.GetDataFacade().InsertObject(item);
        //                        }

        //                    }
        //                }
        //                //Tipos de Riesgo (reglas y guiones)
        //                if (product.ProductCoveredRisks != null && product.ProductCoveredRisks.Count > 0)
        //                {
        //                    foreach (Model.CoveredRisk coveredRisk in product.ProductCoveredRisks)
        //                    {
        //                        PrimaryKey key = PRODEN.ProductCoverRiskType.CreatePrimaryKey(product.Id, (int)Enum.Parse(typeof(CoveredRiskType), coveredRisk.CoveredRiskType.ToString()));
        //                        PRODEN.ProductCoverRiskType risk = (PRODEN.ProductCoverRiskType)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
        //                        if (risk != null)
        //                        {
        //                            risk.RuleSetId = coveredRisk.RuleSetId;
        //                            risk.PreRuleSetId = coveredRisk.PreRuleSetId;
        //                            risk.ScriptId = coveredRisk.ScriptId;
        //                            risk.MaxRiskQuantity = coveredRisk.MaxRiskQuantity;
        //                            DataFacadeManager.Instance.GetDataFacade().UpdateObject(risk);
        //                        }
        //                        else
        //                        {
        //                            risk = new PRODEN.ProductCoverRiskType(product.Id, (int)coveredRisk.CoveredRiskType);
        //                            risk.RuleSetId = coveredRisk.RuleSetId;
        //                            risk.PreRuleSetId = coveredRisk.PreRuleSetId;
        //                            risk.ScriptId = coveredRisk.ScriptId;
        //                            risk.MaxRiskQuantity = coveredRisk.MaxRiskQuantity;
        //                            DataFacadeManager.Instance.GetDataFacade().InsertObject(risk);
        //                        }
        //                    }
        //                }

        //                //Grupo de cobertura y objetos del seguro
        //                if (product.GroupCoverages != null && product.GroupCoverages.Count > 0)
        //                {

        //                    foreach (Model.GroupCoverage groupCoverage in product.GroupCoverages)
        //                    {
        //                        groupCoverage.Product = new Model.Product { Id = product.Id };
        //                        groupCoverage.Product.Prefix = new CommonModel.Prefix { Id = product.Prefix.Id };
        //                        //Grupo de cobertura tipos de Riesgo
        //                        if (groupCoverage.CoveredRiskType != null)
        //                        {
        //                            PRODEN.ProductGroupCover item = EntityAssembler.CreateProductGroupCover(groupCoverage);
        //                            DataFacadeManager.Instance.GetDataFacade().InsertObject(item);
        //                        }
        //                        //Grupo coberturas coberturas
        //                        if (groupCoverage.Coverages != null && groupCoverage.Coverages.Count > 0)
        //                        {
        //                            foreach (Model.Coverage Coverage in groupCoverage.Coverages)
        //                            {
        //                                PRODEN.GroupCoverage item = EntityAssembler.CreateGroupCoverageByCoverage(groupCoverage, Coverage);
        //                                DataFacadeManager.Instance.GetDataFacade().InsertObject(item);
        //                            }
        //                        }
        //                        //Grupo coberturas Objetos del seguro
        //                        if (groupCoverage.InsuredObjects != null && groupCoverage.InsuredObjects.Count > 0)
        //                        {
        //                            foreach (Model.InsuredObject insuredObject in groupCoverage.InsuredObjects)
        //                            {
        //                                PRODEN.GroupInsuredObject item = EntityAssembler.CreateProductGroupInsuredObject(insuredObject, groupCoverage, product.Id);
        //                                DataFacadeManager.Instance.GetDataFacade().InsertObject(item);
        //                            }
        //                        }
        //                    }

        //                }

        //                //actividades (datos adicionales)
        //                if (product.ProductCommercialClass != null)
        //                {
        //                    foreach (Model.ProductRiskCommercialClass modelo in product.ProductCommercialClass)
        //                    {
        //                        COMMEN.ProductRiskCommercialClass item = EntityAssembler.CreateProductCommercialClass(modelo, product.Id);
        //                        DataFacadeManager.Instance.GetDataFacade().InsertObject(item);
        //                    }
        //                }

        //                //deducibles (datos adicionales)
        //                if (product.DeductibleProduct != null)
        //                {
        //                    foreach (Model.DeductibleProduct modelo in product.DeductibleProduct)
        //                    {
        //                        COMMEN.DeductibleProduct item = EntityAssembler.CreateDeductibleProduct(modelo, product.Id);
        //                        DataFacadeManager.Instance.GetDataFacade().InsertObject(item);
        //                    }
        //                }

        //                //Limites RC (datos adicionales)
        //                if (product.LimitRC != null)
        //                {
        //                    foreach (Model.LimitRCRelation modelo in product.LimitRC)
        //                    {
        //                        COMMEN.CoLimitsRcRel item = EntityAssembler.CreateLimitRCRelation(modelo, product.Prefix.Id, product.Id);
        //                        DataFacadeManager.Instance.GetDataFacade().InsertObject(item);
        //                    }
        //                }

        //                //formas de pago (datos adicionales)
        //                if (product.ProductForm != null)
        //                {
        //                    foreach (Model.ProductForm modelo in product.ProductForm)
        //                    {
        //                        modelo.Product = new Model.Product { Id = product.Id };
        //                        PRODEN.ProductForm item = EntityAssembler.CreateProductForm(modelo);
        //                        DataFacadeManager.Instance.GetDataFacade().InsertObject(item);
        //                    }
        //                }
        //                if (product.TableResult != null && product.TableResult.Count > 0)
        //                {
        //                    foreach (Model.TableResult item in product.TableResult)
        //                    {
        //                        PRODEN.ProductLog productLog = new PRODEN.ProductLog();
        //                        productLog.ProductCode = product.Id;
        //                        productLog.IssueDate = DateTime.Now;
        //                        Mapper.CreateMap(item.GetType(), productLog.GetType()).ForMember(PRODEN.ProductLog.Properties.UserCode, c => c.MapFrom("UserId"));
        //                        Mapper.Map(item, productLog);
        //                        DataFacadeManager.Instance.GetDataFacade().InsertObject(productLog);
        //                    }
        //                }
        //                transaction.Complete();
        //                product = ConvertToModelProduct(ProductEntities);
        //                return product;
        //            }
        //            catch (Exception ex)
        //            {
        //                transaction.Dispose();
        //                throw new BusinessException("Error al actualizar producto", ex);
        //            }
        //        }
        //    }
        //}
        //public List<Models.DeductibleProduct> GetDeductibleProductsByProductId(int productId)
        //{
        //    ProductRelatedEntitiesView view = new ProductRelatedEntitiesView();
        //    List<Models.DeductibleProduct> deductibleProduct = new List<Models.DeductibleProduct>();
        //    ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
        //    filter.Property(COMMEN.DeductibleProduct.Properties.ProductId, typeof(COMMEN.DeductibleProduct).Name).Equal().Constant(productId);
        //    view.DeductibleProductList = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.DeductibleProduct), filter.GetPredicate()));

        //    //deducibles (datos adicionales)
        //    if (view.DeductibleProductList != null)
        //    {
        //        deductibleProduct = new List<Models.DeductibleProduct>();
        //        foreach (COMMEN.DeductibleProduct item in view.DeductibleProductList)
        //        {
        //            deductibleProduct.Add(new Models.DeductibleProduct
        //            {
        //                DeductId = item.DeductId
        //            });
        //        }
        //    }
        //    return deductibleProduct;
        //}

        //public List<Models.ProductRiskCommercialClass> GetProductRiskCommercialClassByProductId(int productId)
        //{
        //    List<Models.ProductRiskCommercialClass> productRiskCommercialClass = new List<Models.ProductRiskCommercialClass>();
        //    ProductRelatedEntitiesView view = new ProductRelatedEntitiesView();
        //    ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
        //    filter.Property(COMMEN.DeductibleProduct.Properties.ProductId, typeof(COMMEN.DeductibleProduct).Name).Equal().Constant(productId);
        //    // Actividades del producto
        //    filter = new ObjectCriteriaBuilder();
        //    filter.Property(COMMEN.ProductRiskCommercialClass.Properties.ProductId, typeof(COMMEN.ProductRiskCommercialClass).Name).Equal().Constant(productId);
        //    view.ProductRiskCommercialClassList = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.ProductRiskCommercialClass), filter.GetPredicate()));

        //    //actividades (datos adicionales)
        //    if (view.ProductRiskCommercialClassList != null)
        //    {
        //        productRiskCommercialClass = new List<Models.ProductRiskCommercialClass>();
        //        foreach (COMMEN.ProductRiskCommercialClass item in view.ProductRiskCommercialClassList)
        //        {
        //            productRiskCommercialClass.Add(new Models.ProductRiskCommercialClass
        //            {
        //                DefaultRiskCommercial = item.DefaultRiskCommercial,
        //                RiskCommercialClass = new Models.RiskCommercialClass { RiskCommercialClassCode = item.RiskCommercialClassCode }
        //            });
        //        }
        //    }
        //    return productRiskCommercialClass;
        //}

        //public List<Model.ProductForm> GetProductFormsByProductId(int productId)
        //{
        //    List<Model.ProductForm> productForm = null;
        //    ProductRelatedEntitiesView view = new ProductRelatedEntitiesView();
        //    ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
        //    // Formularios del producto
        //    filter = new ObjectCriteriaBuilder();
        //    filter.Property(PRODEN.ProductForm.Properties.ProductId, typeof(PRODEN.ProductForm).Name).Equal().Constant(productId);
        //    view.ProductFormList = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(PRODEN.ProductForm), filter.GetPredicate()));

        //    //Formas de impresion (datos adicionales)
        //    if (view.ProductFormList != null)
        //    {
        //        productForm = new List<Model.ProductForm>();
        //        foreach (PRODEN.ProductForm item in view.ProductFormList)
        //        {
        //            productForm.Add(new Model.ProductForm
        //            {
        //                CurrentFrom = item.CurrentFrom,
        //                StrCurrentFrom = item.CurrentFrom.ToString("dd/MM/yyyy"),
        //                FormId = item.FormId,
        //                FormNumber = item.FormNumber,
        //                GroupCoverage = new Models.GroupCoverage { Id = item.CoverGroupId }
        //            });
        //        }
        //    }
        //    return productForm;
        //}

        ///// <summary>
        ///// Consultar productos por ramo comercial y descripcion relacionado con currency
        ///// </summary>
        ///// <param name = "prefixId" > Id ramo comercial</param>      
        ///// <param name = "description" > Descripcion del producto</param>      
        ///// <returns>Lista de productos</returns>
        //public List<Model.Product> GetMainProductByPrefixIdDescriptionProductId(int prefixId, string description, int productId)
        //{
        //    Stopwatch stopWatch = new Stopwatch();
        //    stopWatch.Start();

        //    List<Model.Product> products = GetSingleProductByPrefixIdDescriptionProductId(prefixId, description, productId);
        //    if (products.Count == 1)
        //    {
        //        products[0].Currencies = GetCurrencyByProductId(products[0].Id);
        //        products[0].CoveredRisk = GetProductCoveredRiskTypeListByProductId(products[0].Id);
        //    }
        //    stopWatch.Stop();
        //    Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.ProductServices.EEProvider.DAOs.GetMainProductByPrefixIdDescriptionProductId");

        //    return products;
        //}

        //public Model.Product GetDataAditionalByProductId(int productId)
        //{
        //    Stopwatch stopWatch = new Stopwatch();
        //    stopWatch.Start();

        //    Model.Product product = new Models.Product();
        //    product.DeductibleProduct = GetDeductibleProductsByProductId(productId);
        //    product.ProductCommercialClass = GetProductRiskCommercialClassByProductId(productId);
        //    product.ProductForm = GetProductFormsByProductId(productId);
        //    product.LimitRC = GetLimitRCRelationByProductId(productId);
        //    stopWatch.Stop();
        //    Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.ProductServices.EEProvider.DAOs.GetDataAditionalByProductId");

        //    return product;
        //}

        #region exportar excel
        ///// <summary>
        ///// generar Archivo del Producto
        ///// </summary>
        ///// <param name="id">Identificador del producto</param>
        ///// <param name="fileName">Nombre Archivo</param>
        ///// <returns></returns>
        ///// <exception cref="BusinessException"></exception>
        //public string GenerateFileToProduct(int id, string fileName)
        //{
        //    List<Task> taskRule = new List<Task>();
        //    Model.Product product = GetFullProductById(id);
        //    #region reglas
        //    List<RulesScriptsServices.Models.RuleSet> RuleSetPre = null;
        //    List<RulesScriptsServices.Models.RuleSet> RuleSets = null;
        //    List<RulesScriptsServices.Models.Script> Scripts = null;
        //    List<RulesScriptsServices.Models.RuleSet> RuleSetRiskPre = null;
        //    List<RulesScriptsServices.Models.RuleSet> RuleSetRisk = null;
        //    List<RulesScriptsServices.Models.Script> ScriptRisk = null;
        //    List<RulesScriptsServices.Models.RuleSet> RuleSetCoveragePre = null;
        //    List<RulesScriptsServices.Models.RuleSet> RuleSetCoverage = null;
        //    List<RulesScriptsServices.Models.Script> ScriptCoverage = null;
        //    RulesScriptsServices.Models.ScriptComposite scriptsComp = null;
        //    if (product.RuleSetId != null)
        //    {
        //        taskRule.Add(Task.Run(() =>
        //        {
        //            List<int> lstids = new List<int>();
        //            lstids.Add(product.RuleSetId.Value);
        //            RuleSets = DelegateService.ruleServiceCore.GetRuleSetByIds(lstids);
        //            DataFacadeManager.Dispose();
        //        }));
        //    }
        //    if (product.PreRuleSetId != null)
        //    {
        //        taskRule.Add(Task.Run(() =>
        //        {
        //            List<int> lstids = new List<int>();
        //            lstids.Add(product.PreRuleSetId.Value);
        //            RuleSetPre = DelegateService.ruleServiceCore.GetRuleSetByIds(lstids);
        //            DataFacadeManager.Dispose();
        //        }));
        //    }
        //    if (product.ScriptId != null)
        //    {
        //        taskRule.Add(Task.Run(() =>
        //        {
        //            List<int> lstids = new List<int>();
        //            lstids.Add(product.ScriptId.Value);
        //            Scripts = DelegateService.scriptServiceCore.GetScriptsByIds(lstids);
        //            DataFacadeManager.Dispose();
        //        }));
        //    }
        //    if (product.ProductCoveredRisks?.FirstOrDefault()?.ScriptId != null)
        //    {
        //        taskRule.Add(Task.Run(() =>
        //        {
        //            List<int> lstids = new List<int>();
        //            lstids.Add(product.ProductCoveredRisks.FirstOrDefault().ScriptId.Value);
        //            ScriptRisk = DelegateService.scriptServiceCore.GetScriptsByIds(lstids);
        //            DataFacadeManager.Dispose();
        //        }));
        //    }
        //    if (product.ProductCoveredRisks?.FirstOrDefault()?.PreRuleSetId != null)
        //    {
        //        taskRule.Add(Task.Run(() =>
        //        {
        //            List<int> lstids = new List<int>();
        //            lstids.Add(product.ProductCoveredRisks.FirstOrDefault().PreRuleSetId.Value);
        //            RuleSetRiskPre = DelegateService.ruleServiceCore.GetRuleSetByIds(lstids);
        //            DataFacadeManager.Dispose();
        //        }));
        //    }
        //    if (product.ProductCoveredRisks?.FirstOrDefault()?.RuleSetId != null)
        //    {
        //        taskRule.Add(Task.Run(() =>
        //        {
        //            List<int> lstids = new List<int>();
        //            lstids.Add(product.ProductCoveredRisks.FirstOrDefault().RuleSetId.Value);
        //            RuleSetRisk = DelegateService.ruleServiceCore.GetRuleSetByIds(lstids);
        //            DataFacadeManager.Dispose();
        //        }));
        //    }
        //    if (product.GroupCoverages != null && product.GroupCoverages.Count > 0)
        //    {
        //        ConcurrentBag<int> lstids = new ConcurrentBag<int>();
        //        ConcurrentBag<int> Riskids = new ConcurrentBag<int>();
        //        ConcurrentBag<int> Coverageids = new ConcurrentBag<int>();
        //        Parallel.ForEach(product.GroupCoverages, coveredRisks =>
        //        {
        //            Parallel.ForEach(coveredRisks.Coverages, Coverage =>
        //            {
        //                if (Coverage.ScriptId.HasValue)
        //                {
        //                    lstids.Add(Coverage.ScriptId.Value);
        //                }
        //                if (Coverage.RuleSetId.HasValue)
        //                {
        //                    Riskids.Add(Coverage.RuleSetId.Value);
        //                }
        //                if (Coverage.PosRuleSetId.HasValue)
        //                {
        //                    Coverageids.Add(Coverage.PosRuleSetId.Value);
        //                }
        //            });
        //        });
        //        taskRule.Add(Task.Run(() =>
        //        {
        //            ScriptCoverage = DelegateService.scriptServiceCore.GetScriptsByIds(lstids.ToList());
        //            DataFacadeManager.Dispose();
        //        }));
        //        taskRule.Add(Task.Run(() =>
        //        {
        //            RuleSetCoveragePre = DelegateService.ruleServiceCore.GetRuleSetByIds(Riskids.ToList());
        //            DataFacadeManager.Dispose();
        //        }));
        //        taskRule.Add(Task.Run(() =>
        //        {
        //            RuleSetCoverage = DelegateService.ruleServiceCore.GetRuleSetByIds(Coverageids.ToList());
        //            DataFacadeManager.Dispose();
        //        }));
        //    }
        //    if (product.ProductCoveredRisks?.FirstOrDefault()?.ScriptId != null)
        //    {
        //        int scripId = product.ProductCoveredRisks.First().ScriptId == null ? (int)0 : (int)product.ProductCoveredRisks.First().ScriptId;
        //        taskRule.Add(Task.Run(() =>
        //        {
        //            scriptsComp = DelegateService.scriptServiceCore.GetScriptComposite(scripId);
        //            DataFacadeManager.Dispose();
        //        }));
        //    }
        //    Task.WaitAll(taskRule.ToArray());
        //    #endregion
        //    if (product != null)
        //    {
        //        CommonModel.FileProcessValue fileProcessValue = new CommonModel.FileProcessValue();
        //        fileProcessValue.Key1 = (int)FileProcessType.ParametrizationProduct;
        //        CommonModel.File file = DelegateService.commonServiceCore.GetFileByFileProcessValue(fileProcessValue);

        //        if (file != null && file.IsEnabled)
        //        {
        //            object obj = new object();
        //            file.Name = fileName;
        //            List<CommonModel.Row> rows = new List<CommonModel.Row>();
        //            CommonModel.Row row = new CommonModel.Row();
        //            row.Fields = new List<CommonModel.Field>();
        //            var template = file.Templates.First(x => x.PropertyName == TemplatePropertyName.Product).Rows[0].Fields;
        //            row.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.Producto), FieldPropertyName.Producto, product.Description + FormatString(product.Id.ToString())));
        //            row.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.PrefixName), FieldPropertyName.PrefixName, product.Prefix.Description));
        //            row.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.CurrencyName), FieldPropertyName.CurrencyName, string.Join("; ", product.Currencies.Select(z => z.Description).ToList())));
        //            row.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.ScriptName), FieldPropertyName.ScriptId, Scripts?.FirstOrDefault()?.Description + product.ScriptId?.ToString()));
        //            row.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.RulePreName), FieldPropertyName.RulePreName, product.PreRuleSetId?.ToString()));
        //            row.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.RuleName), FieldPropertyName.RuleId, product.RuleSetId?.ToString()));
        //            row.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.IsFlatRate), FieldPropertyName.IsFlatRate, ConvertBool(product.IsFlatRate)));
        //            row.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.IsGreen), FieldPropertyName.IsGreen, ConvertBool(product.IsGreen)));
        //            row.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.IsCollective), FieldPropertyName.IsCollective, ConvertBool(product.IsCollective)));
        //            row.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.IsRequest), FieldPropertyName.IsRequest, ConvertBool(product.IsRequest)));
        //            row.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.IsUse), FieldPropertyName.IsUse, ConvertBool(product.IsUse)));
        //            row.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.PolicyCurrentFrom), FieldPropertyName.PolicyCurrentFrom, product.CurrentFrom.ToString()));
        //            row.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.PolicyCurrentTo), FieldPropertyName.PolicyCurrentTo, product.CurrentTo?.ToString() ?? ""));
        //            row.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.AgentParticipation), FieldPropertyName.AgentParticipation, product.StandardCommissionPercentage.ToString()));
        //            row.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.MaxRiskQuantity), FieldPropertyName.MaxRiskQuantity, product.ProductCoveredRisks.FirstOrDefault()?.MaxRiskQuantity.ToString()));
        //            row.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.BusinessBranchRiskType), FieldPropertyName.BusinessBranchRiskType, product.ProductCoveredRisks.FirstOrDefault()?.Description));
        //            row.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.ScriptName), FieldPropertyName.ScriptRiskName, ScriptRisk?.FirstOrDefault()?.Description == null ? "" : ScriptRisk?.FirstOrDefault()?.Description == null ? "" : ScriptRisk.First().Description + FormatString(product.ProductCoveredRisks.First().ScriptId.Value.ToString())));
        //            row.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.RulePreName), FieldPropertyName.RuleRiskPreName, RuleSetRiskPre?.FirstOrDefault()?.Description + FormatString(RuleSetRiskPre?.FirstOrDefault()?.RuleSetId.ToString())));
        //            row.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.RuleName), FieldPropertyName.RuleRiskName, RuleSetRisk?.FirstOrDefault()?.Description == null ? "" : RuleSetRisk.First().Description + FormatString(RuleSetRisk?.FirstOrDefault()?.RuleSetId.ToString())));
        //            row.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.PolicyTypeName), FieldPropertyName.PolicyTypeName, product.PolicyTypes?.FirstOrDefault()?.Description?.ToString()));
        //            file.Templates[0].Rows.Add(row);
        //            //Reglas General
        //            if (product.RuleSetId.HasValue)
        //            {
        //                List<int> ruleGeneral = new List<int> { (int)product.RuleSetId };
        //                Task<List<CommonModel.Row>> taskGeneral = Task<List<CommonModel.Row>>.Run(() =>
        //                {
        //                    return GetRuleSets(file.Templates.First(x => x.PropertyName == TemplatePropertyName.RulePakage).Rows[0].Fields, ruleGeneral);
        //                }
        //                );
        //                file.Templates.First(x => x.PropertyName == TemplatePropertyName.RulePakage).Rows.AddRange(taskGeneral.Result);
        //            }
        //            if (RuleSetRisk?.FirstOrDefault()?.RuleSetId != null)
        //            {
        //                List<int> ruleRisk = new List<int> { RuleSetRisk.First().RuleSetId };
        //                Task<List<CommonModel.Row>> taskRisk = Task<List<CommonModel.Row>>.Run(() =>
        //                {
        //                    return GetRuleSets(file.Templates.First(x => x.PropertyName == TemplatePropertyName.RulePakageRisk).Rows[0].Fields, ruleRisk);
        //                }
        //                );
        //                file.Templates.First(x => x.PropertyName == TemplatePropertyName.RulePakageRisk).Rows.AddRange(taskRisk.Result);
        //            }
        //            if (RuleSetRiskPre?.FirstOrDefault()?.RuleSetId != null)
        //            {
        //                List<int> ruleRisk = new List<int> { RuleSetRiskPre.FirstOrDefault().RuleSetId };
        //                Task<List<CommonModel.Row>> taskRisk = Task<List<CommonModel.Row>>.Run(() =>
        //                {
        //                    return GetRuleSets(file.Templates.First(x => x.PropertyName == TemplatePropertyName.RulePakageRisk).Rows[0].Fields, ruleRisk);
        //                }
        //                );
        //                file.Templates.First(x => x.PropertyName == TemplatePropertyName.RulePakageRisk).Rows.AddRange(taskRisk.Result);
        //            }

        //            if (product.GroupCoverages != null && product.GroupCoverages.Count > 0)
        //            {
        //                List<int> ruleCoverage = new List<int>();
        //                ruleCoverage = product.GroupCoverages.Select(x => x.Coverages.Where(a => a.RuleSetId != null).Select(z => (int)z.RuleSetId)).Select(y => new { Id = y }).SelectMany(n => n.Id).Distinct().ToList();
        //                Task<List<CommonModel.Row>> taskRisk = Task<List<CommonModel.Row>>.Run(() =>
        //                {
        //                    return GetRuleSets(file.Templates.First(x => x.PropertyName == TemplatePropertyName.RulePakageCoverage).Rows[0].Fields, ruleCoverage);
        //                }
        //                );
        //                file.Templates.First(x => x.PropertyName == TemplatePropertyName.RulePakageCoverage).Rows.AddRange(taskRisk.Result);
        //            }
        //            if (scriptsComp != null)
        //            {
        //                ConcurrentBag<CommonModel.Row> rowsScript = new ConcurrentBag<CommonModel.Row>();
        //                int maxRow = file.Templates.First(x => x.PropertyName == TemplatePropertyName.Script).Rows.Count() - 1;
        //                var templatescripts = file.Templates.First(x => x.PropertyName == TemplatePropertyName.Script).Rows[maxRow].Fields;
        //                Parallel.ForEach(scriptsComp.Nodes, script =>
        //                {
        //                    Parallel.ForEach(script.Questions, question =>
        //                    {
        //                        CommonModel.Row rowScript = new CommonModel.Row();
        //                        rowScript.Fields = new List<CommonModel.Field>();
        //                        rowScript.Fields.Add(NewField(templatescripts.FirstOrDefault(x => x.PropertyName == FieldPropertyName.ScriptId), FieldPropertyName.ScriptId, scriptsComp.Script.Description));
        //                        rowScript.Fields.Add(NewField(templatescripts.FirstOrDefault(x => x.PropertyName == FieldPropertyName.ScriptName), FieldPropertyName.ScriptName, question.Description));
        //                        rowsScript.Add(rowScript);
        //                    });
        //                });
        //                file.Templates.First(x => x.PropertyName == TemplatePropertyName.Script).Rows.AddRange(rowsScript);
        //            }

        //            if (product.ProductAgents != null && product.ProductAgents.Count > 0)
        //            {
        //                int maxRow = file.Templates.First(x => x.PropertyName == TemplatePropertyName.AdditionalIntermediaries).Rows.Count() - 1;
        //                template = file.Templates.First(x => x.PropertyName == TemplatePropertyName.AdditionalIntermediaries).Rows[maxRow].Fields;
        //                maxRow = file.Templates.First(x => x.PropertyName == TemplatePropertyName.Commissions).Rows.Count() - 1;
        //                var templateCommision = file.Templates.First(x => x.PropertyName == TemplatePropertyName.Commissions).Rows[maxRow].Fields;
        //                Parallel.ForEach(product.ProductAgents, agent =>
        //                 {
        //                     CommonModel.Row rowAgent = new CommonModel.Row();
        //                     rowAgent.Fields = new List<CommonModel.Field>();
        //                     rowAgent.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.AgentCode), FieldPropertyName.AgentCode, agent.IndividualId.ToString()));
        //                     rowAgent.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.AgentType), FieldPropertyName.AgentType, agent?.AgentType.Description));
        //                     rowAgent.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.AgentName), FieldPropertyName.AgentName, agent.FullName));
        //                     lock (obj)
        //                     {
        //                         file.Templates.First(x => x.PropertyName == TemplatePropertyName.AdditionalIntermediaries).Rows.Add(rowAgent);
        //                     }
        //                     if (agent.ProductAgencyCommiss != null && agent.ProductAgencyCommiss.Count > 0)
        //                     {
        //                         Parallel.ForEach(agent.ProductAgencyCommiss, agencyCommiss =>
        //                         {
        //                             CommonModel.Row rowAgencyCommiss = new CommonModel.Row();
        //                             rowAgencyCommiss.Fields = new List<CommonModel.Field>();
        //                             rowAgencyCommiss.Fields.Add(NewField(templateCommision.FirstOrDefault(x => x.PropertyName == FieldPropertyName.AgentCode), FieldPropertyName.AgentCode, agencyCommiss.Code.ToString()));
        //                             rowAgencyCommiss.Fields.Add(NewField(templateCommision.FirstOrDefault(x => x.PropertyName == FieldPropertyName.AgentType), FieldPropertyName.AgentType, agencyCommiss.AgentType.Description.ToString()));
        //                             rowAgencyCommiss.Fields.Add(NewField(templateCommision.FirstOrDefault(x => x.PropertyName == FieldPropertyName.AgentParticipation), FieldPropertyName.AgentParticipation, agencyCommiss.CommissPercentage.ToString()));
        //                             rowAgencyCommiss.Fields.Add(NewField(templateCommision.FirstOrDefault(x => x.PropertyName == FieldPropertyName.AgentName), FieldPropertyName.AgentName, agencyCommiss.AgencyName));
        //                             lock (obj)
        //                             {
        //                                 file.Templates.First(x => x.PropertyName == TemplatePropertyName.Commissions).Rows.Add(rowAgencyCommiss);
        //                             }
        //                         });
        //                     }
        //                 });
        //            }

        //            if (product.FinancialPlans != null && product.FinancialPlans.Count > 0)
        //            {
        //                int maxRow = file.Templates.First(x => x.PropertyName == TemplatePropertyName.PaymentPlans).Rows.Count() - 1;
        //                template = file.Templates.First(x => x.PropertyName == TemplatePropertyName.PaymentPlans).Rows[maxRow].Fields;
        //                Parallel.ForEach(product.FinancialPlans, paymentPlan =>
        //                {
        //                    CommonModel.Row rowPaymentPlan = new CommonModel.Row();
        //                    rowPaymentPlan.Fields = new List<CommonModel.Field>();
        //                    rowPaymentPlan.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.PolicyPaymentPlan), FieldPropertyName.PolicyPaymentPlan, paymentPlan.Id.ToString()));
        //                    rowPaymentPlan.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.CurrencyName), FieldPropertyName.CurrencyName, paymentPlan.Currency.Description.ToString()));
        //                    rowPaymentPlan.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.PaymentMethodName), FieldPropertyName.PaymentMethodName, paymentPlan.PaymentMethod.Description));
        //                    rowPaymentPlan.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.PaymentPlanName), FieldPropertyName.PaymentPlanName, paymentPlan.PaymentSchedule.Description));
        //                    rowPaymentPlan.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.IsUse), FieldPropertyName.IsUse, ConvertBool(paymentPlan.IsDefault)));
        //                    lock (obj)
        //                    {
        //                        file.Templates.First(x => x.PropertyName == TemplatePropertyName.PaymentPlans).Rows.Add(rowPaymentPlan);
        //                    }
        //                });
        //            }
        //            if (product.GroupCoverages != null && product.GroupCoverages.Count > 0)
        //            {

        //                int maxRow = file.Templates.First(x => x.PropertyName == TemplatePropertyName.GroupCoverages).Rows.Count() - 1;
        //                template = file.Templates.First(x => x.PropertyName == TemplatePropertyName.GroupCoverages).Rows[maxRow].Fields;
        //                Parallel.ForEach(product.GroupCoverages, coveredRisks =>
        //                {
        //                    Parallel.ForEach(coveredRisks.Coverages, Coverage =>
        //                    {

        //                        CommonModel.Row rowCoverage = new CommonModel.Row();
        //                        rowCoverage.Fields = new List<CommonModel.Field>();
        //                        rowCoverage.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.GroupCoverageName), FieldPropertyName.GroupCoverageName, coveredRisks.Description + FormatString(coveredRisks.Id.ToString())));
        //                        rowCoverage.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.InsuredObjectName), FieldPropertyName.InsuredObjectName, Coverage.InsuredObject.Description + FormatString(Coverage.InsuredObject.Id.ToString())));
        //                        rowCoverage.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.IsMandatory), FieldPropertyName.IsMandatory, ConvertBool(coveredRisks.InsuredObjects.FirstOrDefault(x => x.Id == Coverage.InsuredObject.Id).IsMandatory)));
        //                        rowCoverage.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.IsSelected), FieldPropertyName.IsSelected, ConvertBool(coveredRisks.InsuredObjects.FirstOrDefault(x => x.Id == Coverage.InsuredObject.Id).IsSelected)));
        //                        rowCoverage.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.PerilDescription), FieldPropertyName.PerilDescription, Coverage.Description + FormatString(Coverage?.Id.ToString())));
        //                        rowCoverage.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.ScriptId), FieldPropertyName.ScriptId, ScriptCoverage?.FirstOrDefault(x => Coverage.ScriptId != null && x.ScriptId == Coverage.ScriptId)?.Description + FormatString(Coverage.ScriptId?.ToString())));
        //                        rowCoverage.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.RulePreId), FieldPropertyName.RulePreId, RuleSetCoveragePre?.FirstOrDefault(x => Coverage.RuleSetId != null && x.RuleSetId == Coverage.RuleSetId)?.Description + FormatString(Coverage.RuleSetId?.ToString())));
        //                        rowCoverage.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.RuleId), FieldPropertyName.RuleId, RuleSetCoverage?.FirstOrDefault(x => Coverage.PosRuleSetId != null && x.RuleSetId == Coverage.PosRuleSetId)?.Description + FormatString(Coverage.PosRuleSetId?.ToString())));
        //                        rowCoverage.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.PrefixName), FieldPropertyName.BusinessBranchDescription, Coverage.SubLineBusiness?.LineBusiness?.Description + FormatString(Coverage.SubLineBusiness?.LineBusiness?.Id.ToString())));
        //                        lock (obj)
        //                        {
        //                            file.Templates.First(x => x.PropertyName == TemplatePropertyName.GroupCoverages).Rows.Add(rowCoverage);
        //                        }
        //                    });
        //                });
        //            }
        //            if (product.DeductibleProduct != null && product.DeductibleProduct.Count > 0)
        //            {
        //                int maxRow = file.Templates.First(x => x.PropertyName == TemplatePropertyName.Deductible).Rows.Count() - 1;
        //                template = file.Templates.First(x => x.PropertyName == TemplatePropertyName.Deductible).Rows[maxRow].Fields;
        //                Parallel.ForEach(product.DeductibleProduct, deductible =>
        //                {
        //                    CommonModel.Row rowDeductible = new CommonModel.Row();
        //                    rowDeductible.Fields = new List<CommonModel.Field>();
        //                    rowDeductible.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.DeductibleDescription), FieldPropertyName.DeductibleDescription, deductible.Description + FormatString(deductible.DeductId.ToString())));
        //                    lock (obj)
        //                    {
        //                        file.Templates.First(x => x.PropertyName == TemplatePropertyName.Deductible).Rows.Add(rowDeductible);
        //                    }
        //                });
        //            }
        //            if (product.ProductCommercialClass != null && product.ProductCommercialClass.Count > 0)
        //            {
        //                int maxRow = file.Templates.First(x => x.PropertyName == TemplatePropertyName.Activities).Rows.Count() - 1;
        //                template = file.Templates.First(x => x.PropertyName == TemplatePropertyName.Activities).Rows[maxRow].Fields;
        //                Parallel.ForEach(product.ProductCommercialClass, activiti =>
        //                {
        //                    CommonModel.Row rowActiviti = new CommonModel.Row();
        //                    rowActiviti.Fields = new List<CommonModel.Field>();
        //                    rowActiviti.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.IsSelected), FieldPropertyName.IsSelected, ConvertBool(activiti.DefaultRiskCommercial)));
        //                    rowActiviti.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.ActivityName), FieldPropertyName.ActivityName, activiti.RiskCommercialClass.Description + FormatString(activiti.RiskCommercialClass.RiskCommercialClassCode.ToString())));
        //                    lock (obj)
        //                    {
        //                        file.Templates.First(x => x.PropertyName == TemplatePropertyName.Activities).Rows.Add(rowActiviti);
        //                    }
        //                });
        //            }
        //            if (product.ProductForm != null && product.ProductForm.Count > 0)
        //            {
        //                int maxRow = file.Templates.First(x => x.PropertyName == TemplatePropertyName.FormsOfPrinting).Rows.Count() - 1;
        //                template = file.Templates.First(x => x.PropertyName == TemplatePropertyName.FormsOfPrinting).Rows[maxRow].Fields;
        //                Parallel.ForEach(product.ProductForm, productForm =>
        //                {
        //                    CommonModel.Row rowProductForm = new CommonModel.Row();
        //                    rowProductForm.Fields = new List<CommonModel.Field>();
        //                    rowProductForm.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.PolicyCurrentFrom), FieldPropertyName.PolicyCurrentFrom, productForm.CurrentFrom.ToString()));
        //                    rowProductForm.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.FormName), FieldPropertyName.FormName, productForm.FormNumber));
        //                    rowProductForm.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.RiskGroupCoverage), FieldPropertyName.RiskGroupCoverage, product.GroupCoverages?.FirstOrDefault(x => x.Id == productForm.GroupCoverage.Id)?.Description + FormatString(productForm.GroupCoverage?.Id.ToString())));
        //                    lock (obj)
        //                    {
        //                        file.Templates.First(x => x.PropertyName == TemplatePropertyName.FormsOfPrinting).Rows.Add(rowProductForm);
        //                    }
        //                });
        //            }
        //            if (product.LimitRC != null && product.LimitRC.Count > 0)
        //            {
        //                int maxRow = file.Templates.First(x => x.PropertyName == TemplatePropertyName.LimitsRC).Rows.Count() - 1;
        //                template = file.Templates.First(x => x.PropertyName == TemplatePropertyName.LimitsRC).Rows[maxRow].Fields;
        //                Parallel.ForEach(product.LimitRC, limitRC =>
        //                {
        //                    CommonModel.Row rowlimitRC = new CommonModel.Row();
        //                    rowlimitRC.Fields = new List<CommonModel.Field>();
        //                    rowlimitRC.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.RiskLimitRcDescription), FieldPropertyName.RiskLimitRcDescription, limitRC.Description + FormatString(limitRC.Id.ToString())));
        //                    rowlimitRC.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.IsSelected), FieldPropertyName.IsSelected, ConvertBool(limitRC.IsDefault)));
        //                    rowlimitRC.Fields.Add(NewField(template.FirstOrDefault(x => x.PropertyName == FieldPropertyName.PolicyType), FieldPropertyName.PolicyType, product.PolicyTypes?.FirstOrDefault(x => x.Id == limitRC.PolicyType.Id)?.Description + FormatString(limitRC.PolicyType.Id.ToString())));
        //                    lock (obj)
        //                    {
        //                        file.Templates.First(x => x.PropertyName == TemplatePropertyName.LimitsRC).Rows.Add(rowlimitRC);
        //                    }
        //                });
        //            }

        //            return DelegateService.commonServiceCore.GenerateFile(file);
        //        }
        //        else
        //        {
        //            return "";
        //        }
        //    }
        //    else
        //    {
        //        return "";
        //    }
        //}
        ///// <summary>
        ///// News the field.
        ///// </summary>
        ///// <param name="fields">The fields.</param>
        ///// <param name="fieldPropertyName">Name of the field property.</param>
        ///// <param name="value">The value.</param>
        ///// <returns></returns>
        //private CommonModel.Field NewField(CommonModel.Field oldField, string fieldPropertyName, string value)
        //{
        //    // CommonModel.Field oldField = fields.FirstOrDefault(x => x.PropertyName == fieldPropertyName);
        //    CommonModel.Field field = new CommonModel.Field();

        //    if (oldField != null)
        //    {
        //        Mapper.CreateMap(oldField.GetType(), field.GetType());
        //        Mapper.Map(oldField, field);
        //        field.Value = value;
        //        return field;
        //    }
        //    return null;
        //}
        //private string ConvertBool(bool item)
        //{
        //    if (item)
        //        return "SI";
        //    else
        //        return "NO";
        //}

        //private string FormatString(string item)
        //{
        //    if (item != null)
        //    {
        //        return " (" + item + ")";
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        //private List<CommonModel.Row> GetRuleSets(List<CommonModel.Field> field, List<int> ruleIds)
        //{
        //    ConcurrentBag<RulesScriptsServices.Models._RuleSet> _RuleSets = new ConcurrentBag<RulesScriptsServices.Models._RuleSet>();
        //    ConcurrentBag<CommonModel.Row> rows = new ConcurrentBag<CommonModel.Row>();
        //    Parallel.ForEach(ruleIds, ruleId =>
        //    {
        //        _RuleSets.Add(DelegateService.ruleServiceCore.GetRuleSetById(ruleId));
        //    });
        //    Parallel.ForEach(_RuleSets, ruleSet =>
        //    {
        //        int cont = 0;
        //        Parallel.ForEach(ruleSet.Rules, rule =>
        //         {
        //             CommonModel.Row row = new CommonModel.Row();
        //             row.Fields = new List<CommonModel.Field>();
        //             row.Fields.Add(NewField(field.FirstOrDefault(x => x.PropertyName == FieldPropertyName.RulePreName), FieldPropertyName.RuleName, " "));
        //             row.Fields.Add(NewField(field.FirstOrDefault(x => x.PropertyName == FieldPropertyName.RuleName), FieldPropertyName.RuleName, rule.Description));
        //             rows.Add(row);
        //         });
        //        rows.ToList()[cont].Fields.FirstOrDefault(x => x.PropertyName == FieldPropertyName.RulePreName).Value = ruleSet.Description + FormatString(ruleSet.RuleSetId.ToString());
        //        cont = rows.Count;
        //    });
        //    return rows.ToList();
        //}
        #endregion
        
    }
}
