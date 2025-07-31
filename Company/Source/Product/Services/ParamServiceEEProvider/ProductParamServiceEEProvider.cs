// -----------------------------------------------------------------------
// <copyright file="EEProviderWeb.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Desconocido</author>
// -----------------------------------------------------------------------
namespace Sistran.Company.Application.ProductParamService.EEProvider
{
    using Sistran.Company.Application.ModelServices.Models.Product;
    using Sistran.Company.Application.ProductParamService;
    using System.Collections.Generic;
    using System.ServiceModel;
    using Sistran.Company.Application.ProductParamServices.EEProvider.DAOs;
    using System.Collections;
    using Sistran.Company.Application.ProductParamService.EEProvider.Assemblers;
    using Sistran.Company.Application.ProductParamService.Models;
    using Sistran.Core.Application.ProductParamService.EEProvider;
    using Sistran.Core.Framework.Queries;
    using System;
    using Sistran.Core.Framework.BAF;
    using Sistran.Core.Framework.Contexts;
    using Sistran.Core.Framework.Transactions;
    using System.Diagnostics;
    using System.Linq;
    using Sistran.Core.Application.Utilities.Managers;
    using Sistran.Core.Application.Common.Entities;
    using Sistran.Core.Application.UniquePerson.Entities;
    using System.Threading.Tasks;
    using Sistran.Core.Application.Utilities.DataFacade;
    using System.Data;
    using TP = Sistran.Core.Application.Utilities.Utility;

    /// <summary>
    /// Provider para ProductParamService
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class ProductParamServiceEEProvider : ProductParamServiceEEProviderCore, IProductParamService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefixId"></param>
        /// <returns></returns>
        public List<CiaParamProductServiceModel> GetCiaProductsByPrefixId(int prefixId)
        {
            CiaProductDAO ciaProductDAO = new CiaProductDAO();
            List<CiaParamProduct> result = ciaProductDAO.GetCiaProductByPrefixId(prefixId);
            return ModelsServicesAssembler.CreateCiaParamProductsServiceModel(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefixId"></param>
        /// <returns></returns>
        public List<CiaParamProductServiceModel> GetCiaProductsByProduct(CiaParamProductServiceModel product)
        {
            CiaProductDAO ciaProductDAO = new CiaProductDAO();
            CiaParamProduct ciaParamProduct = new CiaParamProduct();
            ciaParamProduct = ServicesModelsAssembler.CreateCiaParamProduct(product);
            List<CiaParamProduct> products = ciaProductDAO.GetCiaProductsByProduct(ciaParamProduct);
            return ModelsServicesAssembler.CreateCiaParamProductsServiceModel(products);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefixId"></param>
        /// <returns></returns>
        public List<CiaParamProduct2GServiceModel> GetProduct2gByPrefix(int prefixId)
        {
            CiaProductDAO ciaProductDAO = new CiaProductDAO();
            List<CiaParamProduct2G> result = ciaProductDAO.GetProduct2gByPrefix(prefixId);
            return ModelsServicesAssembler.CreateCiaParamProducts2GServiceModel(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefixId"></param>
        /// <returns></returns>
        //public List<CiaParamAssistanceTypeServiceModel> GetCiaAssistanceTypeByPrefix(int prefixId)
        //{
        //    CiaProductDAO ciaProductDAO = new CiaProductDAO();
        //    List<CiaParamAssistanceType> result = ciaProductDAO.GetCiaAssistanceTypeByPrefix(prefixId);
        //    return ModelsServicesAssembler.CreateCiaParamAssistanceTypesServiceModel(result);
        //}

        /// <summary>
        /// Copiar un producto especificp
        /// </summary>
        /// <param name="copyProduct">Producto.</param>
        /// <returns></returns>
        public int CreateCopyProduct(CiaParamCopyProductServiceModel ciaParamCopyProductServiceModel)
        {
            try
            {
                CiaProductDAO productDAO = new CiaProductDAO();
                return productDAO.CreateCopyProduct(ServicesModelsAssembler.CreateCiaParamCopyProduct(ciaParamCopyProductServiceModel));
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefixId"></param>
        /// <returns></returns>
        public List<CiaParamFinancialPlanServiceModel> GetPaymentScheduleByCurrencies(List<int> currencies)
        {
            CiaProductDAO ciaProductDAO = new CiaProductDAO();
            List<CiaParamFinancialPlan> result = ciaProductDAO.GetPaymentScheduleByCurrencies(currencies);
            return ModelsServicesAssembler.CreateCiaParamFinancialPlansServiceModel(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<CiaParamBeneficiaryTypeServiceModel> GetBeneficiaryType()
        {
            CiaProductDAO ciaProductDAO = new CiaProductDAO();
            List<CiaParamBeneficiaryType> result = ciaProductDAO.GetBeneficiaryType();
            return ModelsServicesAssembler.CreateCiaParamBeneficiaryTypesServiceModel(result);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="coverGroupId"></param>
        /// <param name="coverageId"></param>
        /// <param name="beneficiaryTypeCd"></param>
        /// <param name="lineBusinnessCd"></param>
        /// <returns></returns>
        public List<CiaParamDeductiblesCoverageServiceModel> GetDeductiblesByProductId(int productId, int coverGroupId, int coverageId, int beneficiaryTypeCd, int lineBusinnessCd)
        {
            CiaProductDAO ciaProductDAO = new CiaProductDAO();
            List<CiaParamDeductiblesCoverage> result = ciaProductDAO.GetDeductiblesByProductId(productId, coverGroupId, coverageId, beneficiaryTypeCd, lineBusinnessCd);
            if (result != null)
            {
                return ModelsServicesAssembler.CreateCiaParamDeductiblesCoveragesServiceModel(result);
            }
            else
            {
                return null;
            }

        }


        public CiaParamProductServiceModel SaveProduct(CiaParamProductServiceModel product)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            //Guardar con transaccion
            if (product == null)
            {
                throw new BusinessException("Producto Vacio");
            }
            else
            {
                using (Context.Current)
                {
                    using (Transaction transaction = new Transaction())
                    {
                        try
                        {
                            CiaProductDAO ciaProductDAO = new CiaProductDAO();
                            switch (product.StatusTypeService)
                            {
                                case Core.Application.ModelServices.Enums.StatusTypeService.Original:
                                case Core.Application.ModelServices.Enums.StatusTypeService.Update:
                                    //Core Product
                                    ciaProductDAO.UpdateCoreProduct(ServicesModelsAssembler.CreateCiaParamProduct(product));

                                    //Financial Plan Delete
                                    List<CiaParamFinancialPlanServiceModel> listCiaParamFinancialPlanDeleted = product.FinancialPlan.Where(a => a.StatusTypeService == Core.Application.ModelServices.Enums.StatusTypeService.Delete).ToList();
                                    bool resultDeleteFinancialPlans = ciaProductDAO.DeleteFinancialPlans(ServicesModelsAssembler.CreateCiaParamFinancialPlans(listCiaParamFinancialPlanDeleted), product.Id);
                                    if (!resultDeleteFinancialPlans)
                                    {
                                        stopWatch.Stop();
                                        Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.ProductServices.EEProvider.SaveProduct");

                                        transaction.Dispose();
                                        throw new BusinessException(" Error al guardar planes de pago");
                                    }


                                    //Currency Product Delete
                                    List<CiaParamCurrencyServiceModel> listCiaParamCurrencyServiceModelDeleted = product.Currency.Where(a => a.StatusTypeService == Core.Application.ModelServices.Enums.StatusTypeService.Delete).ToList();
                                    bool resultDeleteCurrencies = ciaProductDAO.DeleteCurrencies(ServicesModelsAssembler.CreateCiaParamCurrencies(listCiaParamCurrencyServiceModelDeleted), product.Id);
                                    if (!resultDeleteCurrencies)
                                    {
                                        stopWatch.Stop();
                                        Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.ProductServices.EEProvider.SaveProduct");

                                        transaction.Dispose();
                                        throw new BusinessException(" Error al guardar moneda");
                                    }

                                    //Currency Save
                                    List<CiaParamCurrencyServiceModel> listCiaParamCurrencyServiceModelAdded = product.Currency.Where(a => a.StatusTypeService == Core.Application.ModelServices.Enums.StatusTypeService.Create).ToList();
                                    List<CiaParamCurrencyServiceModel> listCiaParamCurrencyServiceModelUpdate = product.Currency.Where(a => a.StatusTypeService == Core.Application.ModelServices.Enums.StatusTypeService.Update).ToList();
                                    bool resultSaveCurrencies = ciaProductDAO.SaveCurrencies(ServicesModelsAssembler.CreateCiaParamCurrencies(listCiaParamCurrencyServiceModelAdded), ServicesModelsAssembler.CreateCiaParamCurrencies(listCiaParamCurrencyServiceModelUpdate), product.Id);
                                    if (!resultSaveCurrencies)
                                    {
                                        stopWatch.Stop();
                                        Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.ProductServices.EEProvider.SaveProduct");

                                        transaction.Dispose();
                                        throw new BusinessException(" Error al guardar moneda");
                                    }

                                    //Financial Plan Save
                                    List<CiaParamFinancialPlanServiceModel> listCiaParamFinancialPlanAdded = product.FinancialPlan.Where(a => a.StatusTypeService == Core.Application.ModelServices.Enums.StatusTypeService.Create).ToList();
                                    List<CiaParamFinancialPlanServiceModel> listCiaParamFinancialPlanUpdate = product.FinancialPlan.Where(a => a.StatusTypeService == Core.Application.ModelServices.Enums.StatusTypeService.Update).ToList();
                                    bool resultSaveFinancialPlans = ciaProductDAO.SaveFinancialPlans(ServicesModelsAssembler.CreateCiaParamFinancialPlans(listCiaParamFinancialPlanAdded), ServicesModelsAssembler.CreateCiaParamFinancialPlans(listCiaParamFinancialPlanUpdate), product.Id);
                                    if (!resultDeleteFinancialPlans)
                                    {
                                        stopWatch.Stop();
                                        Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.ProductServices.EEProvider.SaveProduct");

                                        transaction.Dispose();
                                        throw new BusinessException(" Error al guardar planes de pago");
                                    }

                                    //Product Policy Type

                                    List<CiaParamPolicyTypeServiceModel> listCiaParamPolicyTypeDeleted = product.PolicyType.Where(a => a.StatusTypeService == Core.Application.ModelServices.Enums.StatusTypeService.Delete).ToList();
                                    List<CiaParamPolicyTypeServiceModel> listCiaParamPolicyTypeAdded = product.PolicyType.Where(a => a.StatusTypeService == Core.Application.ModelServices.Enums.StatusTypeService.Create).ToList();
                                    List<CiaParamPolicyTypeServiceModel> listCiaParamPolicyTypeUpdate = product.PolicyType.Where(a => a.StatusTypeService == Core.Application.ModelServices.Enums.StatusTypeService.Update).ToList();
                                    bool resultSavePolicyTypes = ciaProductDAO.SavePolicyTypes(ServicesModelsAssembler.CreateCiaParamPolicyTypes(listCiaParamPolicyTypeDeleted), ServicesModelsAssembler.CreateCiaParamPolicyTypes(listCiaParamPolicyTypeAdded), ServicesModelsAssembler.CreateCiaParamPolicyTypes(listCiaParamPolicyTypeUpdate), product.Id, product.Prefix.Id);
                                    if (!resultSavePolicyTypes)
                                    {
                                        stopWatch.Stop();
                                        Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.ProductServices.EEProvider.SaveProduct");

                                        transaction.Dispose();
                                        throw new BusinessException(" Error al guardar tipo de poliza");
                                    }


                                    ////Company Product
                                    //ciaProductDAO.UpdateCiaProduct(ServicesModelsAssembler.CreateCiaParamProduct(product));

                                    ////Tipo Asistencia
                                    //if (product.AssistanceType != null)
                                    //{
                                    //    List<CiaParamAssistanceTypeServiceModel> listCiaParamAssistanceTypeDeleted = product.AssistanceType.Where(a => a.StatusTypeService == Core.Application.ModelServices.Enums.StatusTypeService.Delete).ToList();
                                    //    List<CiaParamAssistanceTypeServiceModel> listCiaParamAssistanceTypeAdded = product.AssistanceType.Where(a => a.StatusTypeService == Core.Application.ModelServices.Enums.StatusTypeService.Create).ToList();
                                    //    List<CiaParamAssistanceTypeServiceModel> listCiaParamAssistanceTypeUpdate = product.AssistanceType.Where(a => a.StatusTypeService == Core.Application.ModelServices.Enums.StatusTypeService.Update).ToList();
                                    //    bool resultSaveAssistanceTypes = ciaProductDAO.SaveAssistanceTypes(ServicesModelsAssembler.CreateCiaParamAssistanceTypes(listCiaParamAssistanceTypeDeleted), ServicesModelsAssembler.CreateCiaParamAssistanceTypes(listCiaParamAssistanceTypeAdded), ServicesModelsAssembler.CreateCiaParamAssistanceTypes(listCiaParamAssistanceTypeUpdate), product.Id, product.Prefix.Id);
                                    //    if (!resultSaveAssistanceTypes)
                                    //    {
                                    //        stopWatch.Stop();
                                    //        Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.ProductServices.EEProvider.SaveProduct");

                                    //        transaction.Dispose();
                                    //        throw new BusinessException(" Error al guardar tipo de asistencia");
                                    //    }
                                    //}

                                    //Product Risk Types - Group Coverages - Insured Objects - Coverages - Deductibles Delete
                                    foreach (CiaParamRiskTypeServiceModel itemCiaParamRiskTypeServiceModel in product.RiskTypes)
                                    {
                                        if (itemCiaParamRiskTypeServiceModel.StatusTypeService == Core.Application.ModelServices.Enums.StatusTypeService.Delete)
                                        {
                                            foreach (CiaParamCoverageServiceModel itemCiaParamCoverageServiceModel in itemCiaParamRiskTypeServiceModel.GroupCoverages)
                                            {
                                                foreach (CiaParamInsuredObjectServiceModel itemCiaParamInsuredObjectServiceModel in itemCiaParamCoverageServiceModel.InsuredObjects)
                                                {
                                                    foreach (CiaParamCoveragesServiceModel itemCiaParamCoveragesServiceModel in itemCiaParamInsuredObjectServiceModel.Coverages)
                                                    {
                                                        bool deleteCoverage = ciaProductDAO.DeleteProductGroupCoverage(product.Id, itemCiaParamCoverageServiceModel.Id, itemCiaParamCoveragesServiceModel.Coverage.Id);
                                                        bool deleteDeductibles = ciaProductDAO.DeleteProductCoverageDeductibles(product.Id, itemCiaParamCoverageServiceModel.Id, itemCiaParamCoveragesServiceModel.Coverage.Id);
                                                        if (!deleteCoverage || !deleteDeductibles)
                                                        {
                                                            stopWatch.Stop();
                                                            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.ProductServices.EEProvider.SaveProduct");

                                                            transaction.Dispose();
                                                            throw new BusinessException(" Error al guardar coberturas");
                                                        }
                                                    }
                                                    bool deleteInsuredObject = ciaProductDAO.DeleteInsuredObject(product.Id, itemCiaParamCoverageServiceModel.Id, itemCiaParamInsuredObjectServiceModel.Id);
                                                    if (!deleteInsuredObject)
                                                    {
                                                        stopWatch.Stop();
                                                        Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.ProductServices.EEProvider.SaveProduct");

                                                        transaction.Dispose();
                                                        throw new BusinessException(" Error al guardar objeto del seguro");
                                                    }
                                                }
                                                bool deleteFormRelatedData = ciaProductDAO.DeleteProductGroupCoverForms(product.Id, itemCiaParamCoverageServiceModel.Id);
                                                bool deleteGroupCoverage = ciaProductDAO.DeleteProductGroupCoverage(product.Id, itemCiaParamCoverageServiceModel.Id);
                                                if (!deleteFormRelatedData || !deleteGroupCoverage)
                                                {
                                                    stopWatch.Stop();
                                                    Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.ProductServices.EEProvider.SaveProduct");

                                                    transaction.Dispose();
                                                    throw new BusinessException(" Error al guardar grupo de coberturas");
                                                }

                                            }
                                            bool deleteRisk = ciaProductDAO.DeleteProductRisk(product.Id, itemCiaParamRiskTypeServiceModel.Id);
                                            if (!deleteRisk)
                                            {
                                                stopWatch.Stop();
                                                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.ProductServices.EEProvider.SaveProduct");

                                                transaction.Dispose();
                                                throw new BusinessException(" Error al guardar tipo de riesgo");
                                            }
                                        }
                                        else
                                        {
                                            foreach (CiaParamCoverageServiceModel itemCiaParamCoverageServiceModel in itemCiaParamRiskTypeServiceModel.GroupCoverages)
                                            {
                                                if (itemCiaParamCoverageServiceModel.StatusTypeService == Core.Application.ModelServices.Enums.StatusTypeService.Delete)
                                                {
                                                    if (itemCiaParamCoverageServiceModel.InsuredObjects != null && itemCiaParamCoverageServiceModel.InsuredObjects.Count > 0 && itemCiaParamCoverageServiceModel.InsuredObjects[0] != null)
                                                    {
                                                        foreach (CiaParamInsuredObjectServiceModel itemCiaParamInsuredObjectServiceModel in itemCiaParamCoverageServiceModel.InsuredObjects)
                                                        {
                                                            foreach (CiaParamCoveragesServiceModel itemCiaParamCoveragesServiceModel in itemCiaParamInsuredObjectServiceModel.Coverages)
                                                            {
                                                                bool deleteCoverage = ciaProductDAO.DeleteProductGroupCoverage(product.Id, itemCiaParamCoverageServiceModel.Id, itemCiaParamCoveragesServiceModel.Coverage.Id);
                                                                bool deleteDeductibles = ciaProductDAO.DeleteProductCoverageDeductibles(product.Id, itemCiaParamCoverageServiceModel.Id, itemCiaParamCoveragesServiceModel.Coverage.Id);
                                                                if (!deleteCoverage || !deleteDeductibles)
                                                                {
                                                                    stopWatch.Stop();
                                                                    Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.ProductServices.EEProvider.SaveProduct");

                                                                    transaction.Dispose();
                                                                    throw new BusinessException(" Error al guardar coberturas");
                                                                }
                                                            }
                                                            bool deleteInsuredObject = ciaProductDAO.DeleteInsuredObject(product.Id, itemCiaParamCoverageServiceModel.Id, itemCiaParamInsuredObjectServiceModel.Id);
                                                            if (!deleteInsuredObject)
                                                            {
                                                                stopWatch.Stop();
                                                                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.ProductServices.EEProvider.SaveProduct");

                                                                transaction.Dispose();
                                                                throw new BusinessException(" Error al guardar objeto del seguro");
                                                            }
                                                        }
                                                    }
                                                    bool deleteFormRelatedData = ciaProductDAO.DeleteProductGroupCoverForms(product.Id, itemCiaParamCoverageServiceModel.Id);
                                                    bool deleteGroupCoverage = ciaProductDAO.DeleteProductGroupCoverage(product.Id, itemCiaParamCoverageServiceModel.Id);
                                                    if (!deleteFormRelatedData || !deleteGroupCoverage)
                                                    {
                                                        stopWatch.Stop();
                                                        Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.ProductServices.EEProvider.SaveProduct");

                                                        transaction.Dispose();
                                                        throw new BusinessException(" Error al guardar grupo de coberturas");
                                                    }
                                                }
                                                else
                                                {
                                                    if (itemCiaParamCoverageServiceModel.InsuredObjects != null && itemCiaParamCoverageServiceModel.InsuredObjects.Count > 0 && itemCiaParamCoverageServiceModel.InsuredObjects[0] != null)
                                                    {
                                                        foreach (CiaParamInsuredObjectServiceModel itemCiaParamInsuredObjectServiceModel in itemCiaParamCoverageServiceModel.InsuredObjects)
                                                        {
                                                            if (itemCiaParamInsuredObjectServiceModel.StatusTypeService == Core.Application.ModelServices.Enums.StatusTypeService.Delete)
                                                            {
                                                                foreach (CiaParamCoveragesServiceModel itemCiaParamCoveragesServiceModel in itemCiaParamInsuredObjectServiceModel.Coverages)
                                                                {
                                                                    bool deleteCoverage = ciaProductDAO.DeleteProductGroupCoverage(product.Id, itemCiaParamCoverageServiceModel.Id, itemCiaParamCoveragesServiceModel.Coverage.Id);
                                                                    bool deleteDeductibles = ciaProductDAO.DeleteProductCoverageDeductibles(product.Id, itemCiaParamCoverageServiceModel.Id, itemCiaParamCoveragesServiceModel.Coverage.Id);
                                                                    if (!deleteCoverage || !deleteDeductibles)
                                                                    {
                                                                        stopWatch.Stop();
                                                                        Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.ProductServices.EEProvider.SaveProduct");

                                                                        transaction.Dispose();
                                                                        throw new BusinessException(" Error al guardar coberturas");
                                                                    }
                                                                }
                                                                bool deleteInsuredObject = ciaProductDAO.DeleteInsuredObject(product.Id, itemCiaParamCoverageServiceModel.Id, itemCiaParamInsuredObjectServiceModel.Id);
                                                                if (!deleteInsuredObject)
                                                                {
                                                                    stopWatch.Stop();
                                                                    Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.ProductServices.EEProvider.SaveProduct");

                                                                    transaction.Dispose();
                                                                    throw new BusinessException(" Error al guardar objeto del seguro");
                                                                }
                                                            }
                                                            else
                                                            {
                                                                foreach (CiaParamCoveragesServiceModel itemCiaParamCoveragesServiceModel in itemCiaParamInsuredObjectServiceModel.Coverages)
                                                                {
                                                                    if (itemCiaParamCoveragesServiceModel.Coverage.StatusTypeService == Core.Application.ModelServices.Enums.StatusTypeService.Delete)
                                                                    {
                                                                        bool deleteCoverage = ciaProductDAO.DeleteProductGroupCoverage(product.Id, itemCiaParamCoverageServiceModel.Id, itemCiaParamCoveragesServiceModel.Coverage.Id);
                                                                        bool deleteDeductibles = ciaProductDAO.DeleteProductCoverageDeductibles(product.Id, itemCiaParamCoverageServiceModel.Id, itemCiaParamCoveragesServiceModel.Coverage.Id);
                                                                        if (!deleteCoverage || !deleteDeductibles)
                                                                        {
                                                                            stopWatch.Stop();
                                                                            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.ProductServices.EEProvider.SaveProduct");

                                                                            transaction.Dispose();
                                                                            throw new BusinessException(" Error al guardar coberturas");
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

                                    //Product Risk Types - Group Coverages - Insured Objects - Coverages - Deductibles Insert and Update
                                    foreach (CiaParamRiskTypeServiceModel itemCiaParamRiskTypeServiceModel in product.RiskTypes)
                                    {
                                        if (itemCiaParamRiskTypeServiceModel.StatusTypeService == Core.Application.ModelServices.Enums.StatusTypeService.Create)
                                        {
                                            bool insertRisk = ciaProductDAO.InsertProductRisk(product.Id, itemCiaParamRiskTypeServiceModel.Id, itemCiaParamRiskTypeServiceModel.MaxRiskQuantity, itemCiaParamRiskTypeServiceModel.RuleSetId, itemCiaParamRiskTypeServiceModel.PreRuleSetId, itemCiaParamRiskTypeServiceModel.ScriptId);
                                            if (!insertRisk)
                                            {
                                                stopWatch.Stop();
                                                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.ProductServices.EEProvider.SaveProduct");

                                                transaction.Dispose();
                                                throw new BusinessException(" Error al guardar tipo de riesgo");
                                            }
                                            foreach (CiaParamCoverageServiceModel itemCiaParamCoverageServiceModel in itemCiaParamRiskTypeServiceModel.GroupCoverages)
                                            {
                                                if (itemCiaParamCoverageServiceModel.StatusTypeService != Core.Application.ModelServices.Enums.StatusTypeService.Delete)
                                                {
                                                    bool insertGroupCoverage = ciaProductDAO.InsertProductGroupCoverage(itemCiaParamCoverageServiceModel.Id, product.Id, product.Prefix.Id, itemCiaParamRiskTypeServiceModel.Id, itemCiaParamCoverageServiceModel.Description);
                                                    if (!insertGroupCoverage)
                                                    {
                                                        stopWatch.Stop();
                                                        Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.ProductServices.EEProvider.SaveProduct");

                                                        transaction.Dispose();
                                                        throw new BusinessException(" Error al guardar grupo de coberturas");
                                                    }
                                                    if (itemCiaParamCoverageServiceModel.InsuredObjects != null && itemCiaParamCoverageServiceModel.InsuredObjects.Count > 0 && itemCiaParamCoverageServiceModel.InsuredObjects[0] != null)
                                                    {
                                                        foreach (CiaParamInsuredObjectServiceModel itemCiaParamInsuredObjectServiceModel in itemCiaParamCoverageServiceModel.InsuredObjects)
                                                        {
                                                            if (itemCiaParamInsuredObjectServiceModel.StatusTypeService != Core.Application.ModelServices.Enums.StatusTypeService.Delete)
                                                            {
                                                                bool insertInsuredObject = ciaProductDAO.InsertProductInsuredObject(itemCiaParamInsuredObjectServiceModel.Id, product.Id, itemCiaParamRiskTypeServiceModel.Id, itemCiaParamCoverageServiceModel.Id, itemCiaParamInsuredObjectServiceModel.IsMandatory, itemCiaParamInsuredObjectServiceModel.IsSelected);
                                                                if (!insertInsuredObject)
                                                                {
                                                                    stopWatch.Stop();
                                                                    Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.ProductServices.EEProvider.SaveProduct");

                                                                    transaction.Dispose();
                                                                    throw new BusinessException(" Error al guardar objeto del seguro");
                                                                }
                                                                foreach (CiaParamCoveragesServiceModel itemCiaParamCoveragesServiceModel in itemCiaParamInsuredObjectServiceModel.Coverages)
                                                                {
                                                                    if (itemCiaParamCoveragesServiceModel.Coverage.StatusTypeService != Core.Application.ModelServices.Enums.StatusTypeService.Delete)
                                                                    {
                                                                        bool insertCoverage = ciaProductDAO.InsertProductCoverage(ServicesModelsAssembler.CreateCiaParamCoverages(itemCiaParamCoveragesServiceModel), product.Id, itemCiaParamRiskTypeServiceModel.Id, itemCiaParamCoverageServiceModel.Id);
                                                                        if (itemCiaParamCoveragesServiceModel.Coverage.DeductiblesCoverage != null && itemCiaParamCoveragesServiceModel.Coverage.DeductiblesCoverage.Count > 0 && itemCiaParamCoveragesServiceModel.Coverage.DeductiblesCoverage[0] != null)
                                                                        {
                                                                            List<CiaParamDeductiblesCoverageServiceModel> listDeductibles = itemCiaParamCoveragesServiceModel.Coverage.DeductiblesCoverage.Where(a => a.IsSelected == true).ToList();
                                                                            foreach (CiaParamDeductiblesCoverageServiceModel itemCiaParamDeductiblesCoverageServiceModel in listDeductibles)
                                                                            {
                                                                                bool insertDeductibles = ciaProductDAO.InsertProductDeductible(
                                                                                    itemCiaParamDeductiblesCoverageServiceModel.Id,
                                                                                    itemCiaParamDeductiblesCoverageServiceModel.BeneficiaryTypeId,
                                                                                    product.Id,
                                                                                    itemCiaParamCoveragesServiceModel.Coverage.Id,
                                                                                    itemCiaParamCoverageServiceModel.Id,
                                                                                    itemCiaParamDeductiblesCoverageServiceModel.IsDefault
                                                                                    );
                                                                                if (!insertDeductibles)
                                                                                {
                                                                                    stopWatch.Stop();
                                                                                    Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.ProductServices.EEProvider.SaveProduct");

                                                                                    transaction.Dispose();
                                                                                    throw new BusinessException(" Error al guardar deducibles");
                                                                                }
                                                                            }
                                                                        }
                                                                        if (!insertCoverage)
                                                                        {
                                                                            stopWatch.Stop();
                                                                            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.ProductServices.EEProvider.SaveProduct");

                                                                            transaction.Dispose();
                                                                            throw new BusinessException(" Error al guardar coberturas");
                                                                        }
                                                                    }
                                                                }

                                                            }
                                                        }
                                                    }
                                                }

                                            }
                                        }
                                        else
                                        {
                                            if (itemCiaParamRiskTypeServiceModel.StatusTypeService == Core.Application.ModelServices.Enums.StatusTypeService.Update)
                                            {
                                                bool updateRisk = ciaProductDAO.UpdateProductRisk(product.Id, itemCiaParamRiskTypeServiceModel.Id, itemCiaParamRiskTypeServiceModel.MaxRiskQuantity, itemCiaParamRiskTypeServiceModel.RuleSetId, itemCiaParamRiskTypeServiceModel.PreRuleSetId, itemCiaParamRiskTypeServiceModel.ScriptId);
                                                if (!updateRisk)
                                                {
                                                    stopWatch.Stop();
                                                    Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.ProductServices.EEProvider.SaveProduct");

                                                    transaction.Dispose();
                                                    throw new BusinessException(" Error al guardar tipo de riesgo");
                                                }
                                            }
                                            foreach (CiaParamCoverageServiceModel itemCiaParamCoverageServiceModel in itemCiaParamRiskTypeServiceModel.GroupCoverages)
                                            {
                                                if (itemCiaParamCoverageServiceModel.StatusTypeService == Core.Application.ModelServices.Enums.StatusTypeService.Create && itemCiaParamRiskTypeServiceModel.StatusTypeService != Core.Application.ModelServices.Enums.StatusTypeService.Delete)
                                                {
                                                    bool insertGroupCoverage = ciaProductDAO.InsertProductGroupCoverage(itemCiaParamCoverageServiceModel.Id, product.Id, product.Prefix.Id, itemCiaParamRiskTypeServiceModel.Id, itemCiaParamCoverageServiceModel.Description);
                                                    if (!insertGroupCoverage)
                                                    {
                                                        stopWatch.Stop();
                                                        Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.ProductServices.EEProvider.SaveProduct");

                                                        transaction.Dispose();
                                                        throw new BusinessException(" Error al guardar grupo de coberturas");
                                                    }
                                                    if (itemCiaParamCoverageServiceModel.InsuredObjects != null && itemCiaParamCoverageServiceModel.InsuredObjects.Count > 0 && itemCiaParamCoverageServiceModel.InsuredObjects[0] != null)
                                                    {
                                                        foreach (CiaParamInsuredObjectServiceModel itemCiaParamInsuredObjectServiceModel in itemCiaParamCoverageServiceModel.InsuredObjects)
                                                        {
                                                            if (itemCiaParamInsuredObjectServiceModel.StatusTypeService == Core.Application.ModelServices.Enums.StatusTypeService.Create && itemCiaParamCoverageServiceModel.StatusTypeService != Core.Application.ModelServices.Enums.StatusTypeService.Delete)
                                                            {
                                                                bool insertInsuredObject = ciaProductDAO.InsertProductInsuredObject(itemCiaParamInsuredObjectServiceModel.Id, product.Id, itemCiaParamRiskTypeServiceModel.Id, itemCiaParamCoverageServiceModel.Id, itemCiaParamInsuredObjectServiceModel.IsMandatory, itemCiaParamInsuredObjectServiceModel.IsSelected);
                                                                if (!insertInsuredObject)
                                                                {
                                                                    stopWatch.Stop();
                                                                    Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.ProductServices.EEProvider.SaveProduct");

                                                                    transaction.Dispose();
                                                                    throw new BusinessException(" Error al guardar objeto del seguro");
                                                                }
                                                                foreach (CiaParamCoveragesServiceModel itemCiaParamCoveragesServiceModel in itemCiaParamInsuredObjectServiceModel.Coverages)
                                                                {
                                                                    if (itemCiaParamCoveragesServiceModel.Coverage.StatusTypeService == Core.Application.ModelServices.Enums.StatusTypeService.Create && itemCiaParamInsuredObjectServiceModel.StatusTypeService != Core.Application.ModelServices.Enums.StatusTypeService.Delete)
                                                                    {
                                                                        bool insertCoverage = ciaProductDAO.InsertProductCoverage(ServicesModelsAssembler.CreateCiaParamCoverages(itemCiaParamCoveragesServiceModel), product.Id, itemCiaParamRiskTypeServiceModel.Id, itemCiaParamCoverageServiceModel.Id);
                                                                        if (!insertCoverage)
                                                                        {
                                                                            stopWatch.Stop();
                                                                            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.ProductServices.EEProvider.SaveProduct");

                                                                            transaction.Dispose();
                                                                            throw new BusinessException(" Error al guardar coberturas");
                                                                        }
                                                                        if (itemCiaParamCoveragesServiceModel.Coverage.DeductiblesCoverage != null && itemCiaParamCoveragesServiceModel.Coverage.DeductiblesCoverage.Count > 0 && itemCiaParamCoveragesServiceModel.Coverage.DeductiblesCoverage[0] != null)
                                                                        {
                                                                            List<CiaParamDeductiblesCoverageServiceModel> listDeductibles = itemCiaParamCoveragesServiceModel.Coverage.DeductiblesCoverage.Where(a => a.IsSelected == true).ToList();
                                                                            foreach (CiaParamDeductiblesCoverageServiceModel itemCiaParamDeductiblesCoverageServiceModel in listDeductibles)
                                                                            {
                                                                                bool insertDeductibles = ciaProductDAO.InsertProductDeductible(
                                                                                    itemCiaParamDeductiblesCoverageServiceModel.Id,
                                                                                    itemCiaParamDeductiblesCoverageServiceModel.BeneficiaryTypeId,
                                                                                    product.Id,
                                                                                    itemCiaParamCoveragesServiceModel.Coverage.Id,
                                                                                    itemCiaParamCoverageServiceModel.Id,
                                                                                    itemCiaParamDeductiblesCoverageServiceModel.IsDefault
                                                                                    );
                                                                                if (!insertDeductibles)
                                                                                {
                                                                                    stopWatch.Stop();
                                                                                    Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.ProductServices.EEProvider.SaveProduct");

                                                                                    transaction.Dispose();
                                                                                    throw new BusinessException(" Error al guardar deducibles");
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }

                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (itemCiaParamCoverageServiceModel.StatusTypeService == Core.Application.ModelServices.Enums.StatusTypeService.Update && itemCiaParamRiskTypeServiceModel.StatusTypeService != Core.Application.ModelServices.Enums.StatusTypeService.Delete)
                                                    {
                                                        bool updateGroupCoverage = ciaProductDAO.UpdateProductGroupCoverage(itemCiaParamCoverageServiceModel.Id, product.Id, product.Prefix.Id, itemCiaParamRiskTypeServiceModel.Id, itemCiaParamCoverageServiceModel.Description);
                                                        if (!updateGroupCoverage)
                                                        {
                                                            stopWatch.Stop();
                                                            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.ProductServices.EEProvider.SaveProduct");

                                                            transaction.Dispose();
                                                            throw new BusinessException(" Error al guardar grupo de coberturas");
                                                        }
                                                    }
                                                    if (itemCiaParamCoverageServiceModel.InsuredObjects != null && itemCiaParamCoverageServiceModel.InsuredObjects.Count > 0 && itemCiaParamCoverageServiceModel.InsuredObjects[0] != null)
                                                    {
                                                        foreach (CiaParamInsuredObjectServiceModel itemCiaParamInsuredObjectServiceModel in itemCiaParamCoverageServiceModel.InsuredObjects)
                                                        {
                                                            if (itemCiaParamInsuredObjectServiceModel.StatusTypeService == Core.Application.ModelServices.Enums.StatusTypeService.Create && itemCiaParamCoverageServiceModel.StatusTypeService != Core.Application.ModelServices.Enums.StatusTypeService.Delete)
                                                            {
                                                                bool insertInsuredObject = ciaProductDAO.InsertProductInsuredObject(itemCiaParamInsuredObjectServiceModel.Id, product.Id, itemCiaParamRiskTypeServiceModel.Id, itemCiaParamCoverageServiceModel.Id, itemCiaParamInsuredObjectServiceModel.IsMandatory, itemCiaParamInsuredObjectServiceModel.IsSelected);
                                                                if (!insertInsuredObject)
                                                                {
                                                                    stopWatch.Stop();
                                                                    Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.ProductServices.EEProvider.SaveProduct");

                                                                    transaction.Dispose();
                                                                    throw new BusinessException(" Error al guardar objeto del seguro");
                                                                }
                                                                foreach (CiaParamCoveragesServiceModel itemCiaParamCoveragesServiceModel in itemCiaParamInsuredObjectServiceModel.Coverages)
                                                                {
                                                                    if (itemCiaParamCoveragesServiceModel.Coverage.StatusTypeService == Core.Application.ModelServices.Enums.StatusTypeService.Create && itemCiaParamInsuredObjectServiceModel.StatusTypeService != Core.Application.ModelServices.Enums.StatusTypeService.Delete)
                                                                    {
                                                                        bool insertCoverage = ciaProductDAO.InsertProductCoverage(ServicesModelsAssembler.CreateCiaParamCoverages(itemCiaParamCoveragesServiceModel), product.Id, itemCiaParamRiskTypeServiceModel.Id, itemCiaParamCoverageServiceModel.Id);
                                                                        if (!insertCoverage)
                                                                        {
                                                                            stopWatch.Stop();
                                                                            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.ProductServices.EEProvider.SaveProduct");

                                                                            transaction.Dispose();
                                                                            throw new BusinessException(" Error al guardar coberturas");
                                                                        }
                                                                        if (itemCiaParamCoveragesServiceModel.Coverage.DeductiblesCoverage != null && itemCiaParamCoveragesServiceModel.Coverage.DeductiblesCoverage.Count > 0 && itemCiaParamCoveragesServiceModel.Coverage.DeductiblesCoverage[0] != null)
                                                                        {
                                                                            List<CiaParamDeductiblesCoverageServiceModel> listDeductibles = itemCiaParamCoveragesServiceModel.Coverage.DeductiblesCoverage.Where(a => a.IsSelected == true).ToList();
                                                                            foreach (CiaParamDeductiblesCoverageServiceModel itemCiaParamDeductiblesCoverageServiceModel in listDeductibles)
                                                                            {
                                                                                bool insertDeductibles = ciaProductDAO.InsertProductDeductible(
                                                                                    itemCiaParamDeductiblesCoverageServiceModel.Id,
                                                                                    itemCiaParamDeductiblesCoverageServiceModel.BeneficiaryTypeId,
                                                                                    product.Id,
                                                                                    itemCiaParamCoveragesServiceModel.Coverage.Id,
                                                                                    itemCiaParamCoverageServiceModel.Id,
                                                                                    itemCiaParamDeductiblesCoverageServiceModel.IsDefault
                                                                                    );
                                                                                if (!insertDeductibles)
                                                                                {
                                                                                    stopWatch.Stop();
                                                                                    Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.ProductServices.EEProvider.SaveProduct");

                                                                                    transaction.Dispose();
                                                                                    throw new BusinessException(" Error al guardar deducibles");
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }

                                                            }
                                                            else
                                                            {
                                                                if (itemCiaParamInsuredObjectServiceModel.StatusTypeService == Core.Application.ModelServices.Enums.StatusTypeService.Update && itemCiaParamCoverageServiceModel.StatusTypeService != Core.Application.ModelServices.Enums.StatusTypeService.Delete)
                                                                {
                                                                    bool updateInsuredObject = ciaProductDAO.UpdateProductInsuredObject(itemCiaParamInsuredObjectServiceModel.Id, product.Id, itemCiaParamRiskTypeServiceModel.Id, itemCiaParamCoverageServiceModel.Id, itemCiaParamInsuredObjectServiceModel.IsMandatory, itemCiaParamInsuredObjectServiceModel.IsSelected);
                                                                    if (!updateInsuredObject)
                                                                    {
                                                                        stopWatch.Stop();
                                                                        Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.ProductServices.EEProvider.SaveProduct");

                                                                        transaction.Dispose();
                                                                        throw new BusinessException(" Error al guardar objeto del seguro");
                                                                    }
                                                                }
                                                                foreach (CiaParamCoveragesServiceModel itemCiaParamCoveragesServiceModel in itemCiaParamInsuredObjectServiceModel.Coverages)
                                                                {
                                                                    if (itemCiaParamCoveragesServiceModel.Coverage.StatusTypeService == Core.Application.ModelServices.Enums.StatusTypeService.Create && itemCiaParamInsuredObjectServiceModel.StatusTypeService != Core.Application.ModelServices.Enums.StatusTypeService.Delete)
                                                                    {
                                                                        bool insertCoverage = ciaProductDAO.InsertProductCoverage(ServicesModelsAssembler.CreateCiaParamCoverages(itemCiaParamCoveragesServiceModel), product.Id, itemCiaParamRiskTypeServiceModel.Id, itemCiaParamCoverageServiceModel.Id);
                                                                        if (!insertCoverage)
                                                                        {
                                                                            stopWatch.Stop();
                                                                            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.ProductServices.EEProvider.SaveProduct");

                                                                            transaction.Dispose();
                                                                            throw new BusinessException(" Error al guardar coberturas");
                                                                        }
                                                                        if (itemCiaParamCoveragesServiceModel.Coverage.DeductiblesCoverage != null && itemCiaParamCoveragesServiceModel.Coverage.DeductiblesCoverage.Count > 0 && itemCiaParamCoveragesServiceModel.Coverage.DeductiblesCoverage[0] != null)
                                                                        {
                                                                            List<CiaParamDeductiblesCoverageServiceModel> listDeductibles = itemCiaParamCoveragesServiceModel.Coverage.DeductiblesCoverage.Where(a => a.IsSelected == true).ToList();
                                                                            foreach (CiaParamDeductiblesCoverageServiceModel itemCiaParamDeductiblesCoverageServiceModel in listDeductibles)
                                                                            {
                                                                                bool insertDeductibles = ciaProductDAO.InsertProductDeductible(
                                                                                    itemCiaParamDeductiblesCoverageServiceModel.Id,
                                                                                    itemCiaParamDeductiblesCoverageServiceModel.BeneficiaryTypeId,
                                                                                    product.Id,
                                                                                    itemCiaParamCoveragesServiceModel.Coverage.Id,
                                                                                    itemCiaParamCoverageServiceModel.Id,
                                                                                    itemCiaParamDeductiblesCoverageServiceModel.IsDefault
                                                                                    );
                                                                                if (!insertDeductibles)
                                                                                {
                                                                                    stopWatch.Stop();
                                                                                    Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.ProductServices.EEProvider.SaveProduct");

                                                                                    transaction.Dispose();
                                                                                    throw new BusinessException(" Error al guardar deducibles");
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (itemCiaParamCoveragesServiceModel.Coverage.StatusTypeService == Core.Application.ModelServices.Enums.StatusTypeService.Update && itemCiaParamInsuredObjectServiceModel.StatusTypeService != Core.Application.ModelServices.Enums.StatusTypeService.Delete)
                                                                        {
                                                                            bool updateCoverage = ciaProductDAO.UpdateProductCoverage(ServicesModelsAssembler.CreateCiaParamCoverages(itemCiaParamCoveragesServiceModel), product.Id, itemCiaParamRiskTypeServiceModel.Id, itemCiaParamCoverageServiceModel.Id);
                                                                            if (!updateCoverage)
                                                                            {
                                                                                stopWatch.Stop();
                                                                                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.ProductServices.EEProvider.SaveProduct");

                                                                                transaction.Dispose();
                                                                                throw new BusinessException(" Error al guardar coberturas");
                                                                            }
                                                                            else
                                                                            {//El registro debe existir en la tabla PROD.PRV_GROUP_COVERAGE, de lo contrario el sistema solo omite este paso
                                                                                bool updCoveragePrim = ciaProductDAO.UpdateProductRiskPremium(itemCiaParamCoveragesServiceModel.Coverage.Id, product.Id, itemCiaParamCoverageServiceModel.Id, itemCiaParamCoveragesServiceModel.Coverage.IsPremiumMin, itemCiaParamCoveragesServiceModel.Coverage.NoCalculate);
                                                                            }
                                                                            if (itemCiaParamCoveragesServiceModel.Coverage.DeductiblesCoverage != null && itemCiaParamCoveragesServiceModel.Coverage.DeductiblesCoverage.Count > 0 && itemCiaParamCoveragesServiceModel.Coverage.DeductiblesCoverage[0] != null)
                                                                            {
                                                                                List<CiaParamDeductiblesCoverageServiceModel> listDeductibles = itemCiaParamCoveragesServiceModel.Coverage.DeductiblesCoverage.Where(a => a.StatusTypeService == Core.Application.ModelServices.Enums.StatusTypeService.Update).ToList();
                                                                                foreach (CiaParamDeductiblesCoverageServiceModel itemCiaParamDeductiblesCoverageServiceModel in listDeductibles)
                                                                                {
                                                                                    bool updateDeductibles = ciaProductDAO.UpdateProductDeductible(
                                                                                        itemCiaParamDeductiblesCoverageServiceModel.Id,
                                                                                        itemCiaParamDeductiblesCoverageServiceModel.BeneficiaryTypeId,
                                                                                        product.Id,
                                                                                        itemCiaParamCoveragesServiceModel.Coverage.Id,
                                                                                        itemCiaParamCoverageServiceModel.Id,
                                                                                        itemCiaParamDeductiblesCoverageServiceModel.IsDefault
                                                                                        );
                                                                                    if (!updateDeductibles)
                                                                                    {
                                                                                        stopWatch.Stop();
                                                                                        Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.ProductServices.EEProvider.SaveProduct");

                                                                                        transaction.Dispose();
                                                                                        throw new BusinessException(" Error al guardar deducibles");
                                                                                    }
                                                                                }

                                                                                listDeductibles = itemCiaParamCoveragesServiceModel.Coverage.DeductiblesCoverage.Where(a => a.StatusTypeService == Core.Application.ModelServices.Enums.StatusTypeService.Create).ToList();
                                                                                foreach (CiaParamDeductiblesCoverageServiceModel itemCiaParamDeductiblesCoverageServiceModel in listDeductibles)
                                                                                {
                                                                                    bool insertDeductibles = ciaProductDAO.InsertProductDeductible(
                                                                                        itemCiaParamDeductiblesCoverageServiceModel.Id,
                                                                                        itemCiaParamDeductiblesCoverageServiceModel.BeneficiaryTypeId,
                                                                                        product.Id,
                                                                                        itemCiaParamCoveragesServiceModel.Coverage.Id,
                                                                                        itemCiaParamCoverageServiceModel.Id,
                                                                                        itemCiaParamDeductiblesCoverageServiceModel.IsDefault
                                                                                        );
                                                                                    if (!insertDeductibles)
                                                                                    {
                                                                                        stopWatch.Stop();
                                                                                        Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.ProductServices.EEProvider.SaveProduct");

                                                                                        transaction.Dispose();
                                                                                        throw new BusinessException(" Error al guardar deducibles");
                                                                                    }
                                                                                }

                                                                                listDeductibles = itemCiaParamCoveragesServiceModel.Coverage.DeductiblesCoverage.Where(a => a.StatusTypeService == Core.Application.ModelServices.Enums.StatusTypeService.Delete).ToList();
                                                                                foreach (CiaParamDeductiblesCoverageServiceModel itemCiaParamDeductiblesCoverageServiceModel in listDeductibles)
                                                                                {
                                                                                    bool deleteDeductible = ciaProductDAO.DeleteProductCoverageDeductible(product.Id,
                                                                                        itemCiaParamCoverageServiceModel.Id,
                                                                                        itemCiaParamCoveragesServiceModel.Coverage.Id,
                                                                                        itemCiaParamDeductiblesCoverageServiceModel.Id,
                                                                                        itemCiaParamDeductiblesCoverageServiceModel.BeneficiaryTypeId
                                                                                        );
                                                                                    if (!deleteDeductible)
                                                                                    {
                                                                                        stopWatch.Stop();
                                                                                        Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.ProductServices.EEProvider.SaveProduct");

                                                                                        transaction.Dispose();
                                                                                        throw new BusinessException(" Error al guardar deducibles");
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

                                            }
                                        }
                                    }

                                    //Product - Product 2G
                                    bool resultSaveProduct2g = ciaProductDAO.SaveProduct2G(product.Id, product.Product2G.Id);
                                    if (!resultSaveProduct2g)
                                    {
                                        stopWatch.Stop();
                                        Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.ProductServices.EEProvider.SaveProduct");

                                        transaction.Dispose();
                                        throw new BusinessException("Error al guardar producto 2G");
                                    }

                                    product.Description = product.Id.ToString();
                                    product = this.GetCiaProductsByProduct(product)[0];
                                    break;
                                case Core.Application.ModelServices.Enums.StatusTypeService.Create:
                                    product = ModelsServicesAssembler.CreateCiaParamProductServiceModel(ciaProductDAO.CreateProduct(ServicesModelsAssembler.CreateCiaParamProduct(product)));
                                    break;
                                default:
                                    stopWatch.Stop();
                                    Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.ProductServices.EEProvider.SaveProduct");

                                    transaction.Dispose();
                                    throw new BusinessException("Error al guardar producto");
                            }
                            transaction.Complete();

                            stopWatch.Stop();
                            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.ProductServices.EEProvider.SaveProduct");
                            return product;
                        }
                        catch (Exception ex)
                        {
                            stopWatch.Stop();
                            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.ProductServices.EEProvider.SaveProduct");

                            transaction.Dispose();
                            throw new BusinessException("Error al guardar producto" + ex.Message);
                        }

                    }
                }
            }
        }

        public string GenerateFileToProducts(string fileName)
        {
            try
            {
                CiaProductDAO ciaProductDAO = new CiaProductDAO();
                return ciaProductDAO.GenerateFileToProducts(fileName);
            }
            catch (Exception ex)
            {
                throw new BusinessException(" Error al generar el archivo " + ex.Message);
            }

        }

        public string GenerateFileToProduct(int productId, string fileName)
        {
            try
            {
                CiaProductDAO ciaProductDAO = new CiaProductDAO();
                return ciaProductDAO.GenerateFileToProduct(productId, fileName);
            }
            catch (Exception ex)
            {
                throw new BusinessException(" Error al generar el archivo " + ex.Message);
            }

        }
        public List<CiaParamLimitsRCServiceModel> GetLimitsRc(List<int> policyTypeIds, int productId, int prefixCd)
        {
            CiaProductDAO ciaProductDAO = new CiaProductDAO();
            return ModelsServicesAssembler.CreateCiaParamLimitsRCsServiceModel(ciaProductDAO.GetLimitsRc(policyTypeIds, productId, prefixCd));
        }

        public List<CiaParamCommercialClassServiceModel> GetRiskCommercialClass(int productId)
        {
            CiaProductDAO ciaProductDAO = new CiaProductDAO();
            return ModelsServicesAssembler.CreateCiaParamCommercialClassServiceModel(ciaProductDAO.GetRiskCommercialClass(productId));
        }

        public List<CiaParamFormServiceModel> GetProductForm(int productId)
        {
            CiaProductDAO ciaProductDAO = new CiaProductDAO();
            return ModelsServicesAssembler.CreateCiaParamFormsServiceModel(ciaProductDAO.GetProductForm(productId));
        }

        public List<CiaParamDeductibleProductServiceModel> GetProductDeductiblesByPrefix(int productId, int prefixCode)
        {
            CiaProductDAO ciaProductDAO = new CiaProductDAO();
            return ModelsServicesAssembler.CreateCiaParamDeductiblesProductServiceModel(ciaProductDAO.GetProductDeductiblesByPrefix(productId, prefixCode));
        }

        public void SaveAdditionalData(List<CiaParamCommercialClassServiceModel> listRiskCommercialClass, List<CiaParamLimitsRCServiceModel> listCiaParamLimitsRCServiceModel, List<CiaParamDeductibleProductServiceModel> listCiaParamDeductibleProductServiceModel, List<CiaParamFormServiceModel> listCiaParamFormServiceModel, int productId)
        {
            using (Context.Current)
            {
                using (Transaction transaction = new Transaction())
                {
                    try
                    {
                        CiaProductDAO ciaProductDAO = new CiaProductDAO();

                        if (listRiskCommercialClass != null && listRiskCommercialClass.Count > 0 && listRiskCommercialClass[0] != null)
                        {
                            foreach (CiaParamCommercialClassServiceModel itemCiaParamCommercialClassServiceModel in listRiskCommercialClass)
                            {
                                if (itemCiaParamCommercialClassServiceModel.StatusTypeService == Core.Application.ModelServices.Enums.StatusTypeService.Create)
                                {
                                    ciaProductDAO.InsertProductRiskCommercialClass(ServicesModelsAssembler.CreateCiaParamCommercialClass(itemCiaParamCommercialClassServiceModel));
                                }
                                else if (itemCiaParamCommercialClassServiceModel.StatusTypeService == Core.Application.ModelServices.Enums.StatusTypeService.Update)
                                {
                                    ciaProductDAO.UpdateProductRiskCommercialClass(ServicesModelsAssembler.CreateCiaParamCommercialClass(itemCiaParamCommercialClassServiceModel));
                                }
                                else if (itemCiaParamCommercialClassServiceModel.StatusTypeService == Core.Application.ModelServices.Enums.StatusTypeService.Delete)
                                {
                                    ciaProductDAO.DeleteProductRiskCommercialClass(ServicesModelsAssembler.CreateCiaParamCommercialClass(itemCiaParamCommercialClassServiceModel));
                                }
                            }
                        }
                        if (listCiaParamLimitsRCServiceModel != null && listCiaParamLimitsRCServiceModel.Count > 0 && listCiaParamLimitsRCServiceModel[0] != null)
                        {
                            foreach (CiaParamLimitsRCServiceModel itemCiaParamLimitsRCServiceModel in listCiaParamLimitsRCServiceModel)
                            {
                                if (itemCiaParamLimitsRCServiceModel.StatusTypeService == Core.Application.ModelServices.Enums.StatusTypeService.Create)
                                {
                                    ciaProductDAO.InsertProductLimitRC(ServicesModelsAssembler.CreateCiaParamLimitsRC(itemCiaParamLimitsRCServiceModel));
                                }
                                else if (itemCiaParamLimitsRCServiceModel.StatusTypeService == Core.Application.ModelServices.Enums.StatusTypeService.Update)
                                {
                                    ciaProductDAO.UpdateProductLimitRC(ServicesModelsAssembler.CreateCiaParamLimitsRC(itemCiaParamLimitsRCServiceModel));
                                }
                                else if (itemCiaParamLimitsRCServiceModel.StatusTypeService == Core.Application.ModelServices.Enums.StatusTypeService.Delete)
                                {
                                    ciaProductDAO.DeleteProductLimitRC(ServicesModelsAssembler.CreateCiaParamLimitsRC(itemCiaParamLimitsRCServiceModel));
                                }
                            }
                        }

                        if (listCiaParamDeductibleProductServiceModel != null && listCiaParamDeductibleProductServiceModel.Count > 0 && listCiaParamDeductibleProductServiceModel[0] != null)
                        {
                            foreach (CiaParamDeductibleProductServiceModel itemCiaParamDeductibleProductServiceModel in listCiaParamDeductibleProductServiceModel)
                            {
                                if (itemCiaParamDeductibleProductServiceModel.StatusTypeService == Core.Application.ModelServices.Enums.StatusTypeService.Create)
                                {
                                    ciaProductDAO.InsertProductDeductibles(ServicesModelsAssembler.CreateCiaParamDeductibleProduct(itemCiaParamDeductibleProductServiceModel), productId);
                                }
                                else if (itemCiaParamDeductibleProductServiceModel.StatusTypeService == Core.Application.ModelServices.Enums.StatusTypeService.Update)
                                {
                                    ciaProductDAO.UpdateProductDeductibles(ServicesModelsAssembler.CreateCiaParamDeductibleProduct(itemCiaParamDeductibleProductServiceModel));
                                }
                                else if (itemCiaParamDeductibleProductServiceModel.StatusTypeService == Core.Application.ModelServices.Enums.StatusTypeService.Delete)
                                {
                                    ciaProductDAO.DeleteProductDeductibles(ServicesModelsAssembler.CreateCiaParamDeductibleProduct(itemCiaParamDeductibleProductServiceModel), productId);
                                }
                            }
                        }

                        if (listCiaParamFormServiceModel != null && listCiaParamFormServiceModel.Count > 0 && listCiaParamFormServiceModel[0] != null)
                        {
                            foreach (CiaParamFormServiceModel itemCiaParamFormServiceModel in listCiaParamFormServiceModel)
                            {
                                if (itemCiaParamFormServiceModel.StatusTypeService == Core.Application.ModelServices.Enums.StatusTypeService.Create)
                                {
                                    ciaProductDAO.InsertProductForm(ServicesModelsAssembler.CreateCiaParamForm(itemCiaParamFormServiceModel), productId);
                                }
                                else if (itemCiaParamFormServiceModel.StatusTypeService == Core.Application.ModelServices.Enums.StatusTypeService.Update)
                                {
                                    ciaProductDAO.UpdateProductForm(ServicesModelsAssembler.CreateCiaParamForm(itemCiaParamFormServiceModel));
                                }
                                else if (itemCiaParamFormServiceModel.StatusTypeService == Core.Application.ModelServices.Enums.StatusTypeService.Delete)
                                {
                                    ciaProductDAO.DeleteProductForm(ServicesModelsAssembler.CreateCiaParamForm(itemCiaParamFormServiceModel));
                                }
                            }
                        }

                        transaction.Complete();
                    }
                    catch (Exception ex)
                    {
                        transaction.Dispose();
                        throw new BusinessException("Error al guardar producto" + ex.Message);
                    }

                }
            }
        }
        public CiaParamSummaryAgentServiceModel GetProductAgentByProductId(int productId, int prefixId)
        {
            CiaProductDAO ciaProductDAO = new CiaProductDAO();
            CiaParamSummaryAgentServiceModel result = ModelsServicesAssembler.CreateCiaParamAgentServiceModel(ciaProductDAO.GetProductAgentByProductId(productId, prefixId));
            return result;
        }

        public List<CiaParamAgentServiceModel> GetProductAgentByProductIdByIndividualId(int productId, int individualId)
        {
            CiaProductDAO ciaProductDAO = new CiaProductDAO();
            List<CiaParamAgentServiceModel> result = ModelsServicesAssembler.CreateCiaParamAgentsServiceModel(ciaProductDAO.GetProductAgentByProductIdByIndividualId(productId, individualId));
            return result;
        }
        //public List<CiaParamAgentServiceModel> GetProductAgentByProductId(int productId)
        //      {
        //          CiaProductDAO ciaProductDAO = new CiaProductDAO();
        //          List<CiaParamAgentServiceModel> result = ModelsServicesAssembler.CreateCiaParamAgentsServiceModel(ciaProductDAO.GetProductAgentByProductId(productId));
        //          return result;
        //      }

        public CiaParamProductServiceModel SaveAgents(List<CiaParamAgentServiceModel> agents, int productId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            if (agents == null)
            {
                throw new BusinessException("Intermediarios Vacio");
            }
            else
            {
                using (Context.Current)
                {
                    using (Transaction transaction = new Transaction())
                    {
                        try
                        {

                            CiaParamProductServiceModel product = new CiaParamProductServiceModel();
                            CiaProductDAO ciaProductDAO = new CiaProductDAO();
                            //Delete
                            List<CiaParamAgentServiceModel> listDelete = agents.Where(x => x.StatusTypeService == Core.Application.ModelServices.Enums.StatusTypeService.Delete).ToList();
                            //if (listDelete != null && listDelete.Count==1 && listDelete[0].IndividualId==0 && listDelete[0].ProductId == 0)
                            //{
                            //    //listDelete = this.GetProductAgentByProductId(productId);
                            //}
                            List<CiaParamAgentServiceModel> listInsert = agents.Where(x => x.StatusTypeService == Core.Application.ModelServices.Enums.StatusTypeService.Create).ToList();
                            if (listDelete.Count > 0)
                            {
                                Parallel.ForEach(listDelete, new ParallelOptions { MaxDegreeOfParallelism = -1 }, itemListDelete =>
                                {
                                    if (itemListDelete.AgencyComiss != null && itemListDelete.AgencyComiss.Count > 0 && itemListDelete.AgencyComiss[0] != null)
                                    {
                                        Parallel.ForEach(itemListDelete.AgencyComiss, new ParallelOptions { MaxDegreeOfParallelism = -1 }, itemComiss =>
                                        {
                                            bool deleteComiss = ciaProductDAO.DeleteAgentComiss(productId, itemListDelete.IndividualId, itemComiss.AgencyId);
                                            if (!deleteComiss)
                                            {
                                                stopWatch.Stop();
                                                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.ProductServices.EEProvider.SaveAgents");

                                                transaction.Dispose();
                                                throw new BusinessException("Error al guardar intermediarios");
                                            }
                                        });
                                    }
                                    bool deleteAgent = ciaProductDAO.DeleteAgent(productId, itemListDelete.IndividualId);
                                    if (!deleteAgent)
                                    {
                                        stopWatch.Stop();
                                        Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.ProductServices.EEProvider.SaveAgents");

                                        transaction.Dispose();
                                        throw new BusinessException("Error al guardar intermediarios");
                                    }
                                });
                            }

                            //Update
                            List<CiaParamAgentServiceModel> listUpdate = agents.Where(x => x.StatusTypeService == Core.Application.ModelServices.Enums.StatusTypeService.Update).ToList();
                            foreach (CiaParamAgentServiceModel itemListUpdate in listUpdate)
                            {
                                if (itemListUpdate.AgencyComiss != null && itemListUpdate.AgencyComiss.Count > 0 && itemListUpdate.AgencyComiss[0] != null)
                                {
                                    foreach (CiaParamAgencyCommissServiceModel itemComiss in itemListUpdate.AgencyComiss)
                                    {
                                        if (itemComiss.StatusTypeService == Core.Application.ModelServices.Enums.StatusTypeService.Delete)
                                        {
                                            bool deleteComiss = ciaProductDAO.DeleteAgentComiss(productId, itemListUpdate.IndividualId, itemComiss.AgencyId);
                                            if (!deleteComiss)
                                            {
                                                stopWatch.Stop();
                                                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.ProductServices.EEProvider.SaveAgents");

                                                transaction.Dispose();
                                                throw new BusinessException("Error al guardar intermediarios");
                                            }
                                        }
                                        if (itemComiss.StatusTypeService == Core.Application.ModelServices.Enums.StatusTypeService.Update)
                                        {
                                            bool updateComiss = ciaProductDAO.UpdateAgentComiss(productId, itemListUpdate.IndividualId, itemComiss.AgencyId, itemComiss.CommissPercentage, itemComiss.AdditionalCommissionPercentage, itemComiss.SchCommissPercentage, itemComiss.StDisCommissPercentage, itemComiss.AdditDisCommissPercentage);
                                            if (!updateComiss)
                                            {
                                                stopWatch.Stop();
                                                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.ProductServices.EEProvider.SaveAgents");

                                                transaction.Dispose();
                                                throw new BusinessException("Error al guardar intermediarios");
                                            }
                                        }
                                        if (itemComiss.StatusTypeService == Core.Application.ModelServices.Enums.StatusTypeService.Create)
                                        {
                                            bool insertComiss = ciaProductDAO.InsertAgentComiss(productId, itemListUpdate.IndividualId, itemComiss.AgencyId, itemComiss.CommissPercentage, itemComiss.AdditionalCommissionPercentage, itemComiss.SchCommissPercentage, itemComiss.StDisCommissPercentage, itemComiss.AdditDisCommissPercentage);
                                            if (!insertComiss)
                                            {
                                                stopWatch.Stop();
                                                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.ProductServices.EEProvider.SaveAgents");

                                                transaction.Dispose();
                                                throw new BusinessException("Error al guardar intermediarios");
                                            }
                                        }
                                    }
                                }
                            }

                            //Insert

                            if (listInsert.Count > 0)
                            {
                                Parallel.ForEach(listInsert, new ParallelOptions { MaxDegreeOfParallelism = -1 }, itemListInsert =>
                                {
                                    bool insertAgent = ciaProductDAO.InsertAgent(productId, itemListInsert.IndividualId);
                                    if (!insertAgent)
                                    {
                                        stopWatch.Stop();
                                        Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.ProductServices.EEProvider.SaveAgents");

                                        transaction.Dispose();
                                        throw new BusinessException("Error al guardar intermediarios");
                                    }
                                    if (itemListInsert.AgencyComiss != null && itemListInsert.AgencyComiss.Count > 0 && itemListInsert.AgencyComiss[0] != null)
                                    {
                                        Parallel.ForEach(itemListInsert.AgencyComiss, new ParallelOptions { MaxDegreeOfParallelism = -1 }, itemComiss =>
                                            {
                                                if (itemComiss.StatusTypeService == Core.Application.ModelServices.Enums.StatusTypeService.Create)
                                                {
                                                    bool insertComiss = ciaProductDAO.InsertAgentComiss(productId, itemListInsert.IndividualId, itemComiss.AgencyId, itemComiss.CommissPercentage, itemComiss.AdditionalCommissionPercentage, itemComiss.SchCommissPercentage, itemComiss.StDisCommissPercentage, itemComiss.AdditDisCommissPercentage);
                                                    if (!insertComiss)
                                                    {
                                                        stopWatch.Stop();
                                                        Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.ProductServices.EEProvider.SaveAgents");

                                                        transaction.Dispose();
                                                        throw new BusinessException("Error al guardar intermediarios");
                                                    }
                                                }
                                            });
                                    }

                                });
                            }

                            product.Description = productId.ToString();
                            product = this.GetCiaProductsByProduct(product)[0];
                            transaction.Complete();
                            stopWatch.Stop();
                            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.ProductServices.EEProvider.SaveAgents");
                            return product;
                        }
                        catch (Exception ex)
                        {
                            stopWatch.Stop();
                            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.ProductServices.EEProvider.SaveAgents");

                            transaction.Dispose();
                            throw new BusinessException("Error al guardar producto " + ex.Message);
                        }

                    }
                }
            }
        }

        public string SaveAllAgents(int prefixId, int productId, bool assigned)
        {
            string result = string.Empty;
            CiaProductDAO ciaProductDAO = new CiaProductDAO();
            result = ciaProductDAO.SaveAllAgents(prefixId, productId, assigned);
            return result;
        }

        public bool ValidatePolicyByProductId(int productId, int riskId, int coverId)
        {
            CiaProductDAO ciaProductDAO = new CiaProductDAO();
            return ciaProductDAO.ValidatePolicyByProductId(productId, riskId, coverId);
        }

        public List<CiaParamAgentServiceModel> GetAgent(int agentId, string description, int prefixId, int idagenttye, int idgroupagent)
        {
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

                filter.Property(Prefix.Properties.PrefixCode, "prefix").Equal().Constant(prefixId);
                filter.And();

                if (agentId > 0)
                {
                    filter.Property(Agent.Properties.IndividualId, "agent").Equal().Constant(agentId);
                    //filter.Equal();
                    //filter.Constant(agentId);
                    filter.And();
                }
                Int32 agencyCode = 0;
                Int32.TryParse(description, out agencyCode);
                if (agencyCode > 0)
                {
                    filter.Property(AgentAgency.Properties.AgentCode, "agentagency").Equal().Constant(agencyCode);
                    //filter.Equal();
                    //filter.Constant(agencyCode);
                }
                else
                {
                    filter.Property(Agent.Properties.CheckPayableTo, "agent").Like().Constant("%" + description + "%");
                    //filter.Like();
                    //filter.Constant("%" + description + "%");
                }
                if (idagenttye > 0)
                {
                    filter.And();
                    filter.Property(Agent.Properties.AgentTypeCode, "agent").Equal().Constant(idagenttye);
                    //filter.Equal();
                    //filter.Constant(idagenttye);
                }
                if (idgroupagent > 0)
                {
                    filter.And();
                    filter.Property(Agent.Properties.AgentGroupCode, "agent").Equal().Constant(idgroupagent);
                    //filter.Equal();
                    //filter.Constant(idgroupagent);
                }
                SelectQuery SelectQuery = new SelectQuery();
                #region Select
                SelectQuery.AddSelectValue(new SelectValue(new Column(Agent.Properties.IndividualId, "agent")));
                SelectQuery.AddSelectValue(new SelectValue(new Column(Agent.Properties.CheckPayableTo, "agent")));
                SelectQuery.AddSelectValue(new SelectValue(new Column(AgentAgency.Properties.AgentCode, "agentagency")));
                #endregion Select

                Join join = new Join(new ClassNameTable(typeof(Prefix), "prefix"), new ClassNameTable(typeof(AgentPrefix), "agentprefix"), JoinType.Inner)
                {
                    Criteria = new ObjectCriteriaBuilder()
                    .Property(Prefix.Properties.PrefixCode, "prefix")
                    .Equal()
                    .Property(AgentPrefix.Properties.PrefixCode, "agentprefix")
                    .GetPredicate()
                };

                join = new Join(join, new ClassNameTable(typeof(Agent), "agent"), JoinType.Inner)
                {
                    Criteria = (new ObjectCriteriaBuilder()
                    .Property(AgentPrefix.Properties.IndividualId, "agentprefix")
                    .Equal()
                    .Property(Agent.Properties.IndividualId, "agent")
                    .GetPredicate())
                };


                join = new Join(join, new ClassNameTable(typeof(AgentAgency), "agentagency"), JoinType.Inner)
                {
                    Criteria = (new ObjectCriteriaBuilder()
                        .Property(Agent.Properties.IndividualId, "agent")
                        .Equal()
                        .Property(AgentAgency.Properties.IndividualId, "agentagency")
                        .GetPredicate())
                };
                SelectQuery.Table = join;
                SelectQuery.Where = filter.GetPredicate();
                List<CiaParamAgentServiceModel> agenciesResult = new List<CiaParamAgentServiceModel>();
                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(SelectQuery))
                {
                    while (reader.Read())
                    {
                        agenciesResult.Add(new CiaParamAgentServiceModel
                        {
                            StatusTypeService = Core.Application.ModelServices.Enums.StatusTypeService.Original,
                            AgencyComiss = new List<CiaParamAgencyCommissServiceModel>(),
                            ErrorServiceModel = new Core.Application.ModelServices.Models.Param.ErrorServiceModel(),
                            IndividualId = (int)reader["IndividualId"],
                            FullName = (string)reader["CheckPayableTo"],
                            LockerId = (int)reader["AgentCode"]
                        });
                    }
                }

                return agenciesResult;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
