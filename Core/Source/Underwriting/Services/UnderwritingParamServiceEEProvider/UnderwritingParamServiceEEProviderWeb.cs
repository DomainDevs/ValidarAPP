// -----------------------------------------------------------------------
// <copyright file="UnderwritingParamServiceEEProviderWeb.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Gina Gómez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.EEProvider
{
    using AutoMapper;
    using Sistran.Core.Application.ModelServices.Enums;
    using Sistran.Core.Application.ModelServices.Models.Param;
    using Sistran.Core.Application.ModelServices.Models.Underwriting;
    using Sistran.Core.Application.ModelServices.Models.UnderwritingParam;
    using Sistran.Core.Application.UnderwritingParamService.EEProvider.Assemblers;
    using Sistran.Core.Application.UnderwritingParamService.EEProvider.Business;
    using Sistran.Core.Application.UnderwritingParamService.EEProvider.DAOs;
    using Sistran.Core.Application.UnderwritingParamService.Models;
    using Sistran.Core.Application.UnderwritingParamService.Models.Base;
    using Sistran.Core.Application.UnderwritingParamServices.EEProvider.DAOs;
    using Sistran.Core.Application.Utilities.Error;
    using Sistran.Core.Application.Utilities.Managers;
    using Sistran.Core.Framework.BAF;
    using Sistran.Core.Framework.Transactions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel;
    using COMMO = Sistran.Core.Application.CommonService.Models;
    using ENUMSM = Sistran.Core.Application.ModelServices.Enums;
    using MODCO = Sistran.Core.Application.ModelServices.Models.CommonParam;
    using MODEN = Sistran.Core.Application.ModelServices.Enums;
    using MODPA = Sistran.Core.Application.ModelServices.Models.Param;
    using MODSM = Sistran.Core.Application.ModelServices.Models.Param;
    using MODUD = Sistran.Core.Application.ModelServices.Models.Underwriting;
    using PARUPSM = Sistran.Core.Application.ModelServices.Models.UnderwritingParam;
    using UNDEN = Sistran.Core.Application.UnderwritingServices.Enums;
    using UNDMO = Sistran.Core.Application.UnderwritingServices.Models;
    using UTIER = Sistran.Core.Application.Utilities.Error;
    using UTILMO = Sistran.Core.Services.UtilitiesServices.Models;
    using UTIMO = Sistran.Core.Application.Utilities.Error;
    using UTMO = Sistran.Core.Application.Utilities.Error;

    /// <summary>
    /// Provider para UnderwritingParamService
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class UnderwritingParamServiceEEProviderWeb : IUnderwritingParamServiceWeb
    {
        public MODPA.ErrorServiceModel ErrorTypeSevice { get; private set; }

        /// <summary>
        /// Obtener lista de planes pago
        /// </summary>
        /// <returns>Plan de pago MOD-S resultado de consulta de plan de pago</returns>
        public PaymentPlansServiceModel GetPaymentPlansServiceModel()
        {
            PaymentPlanDAO paymentPlanDAO = new PaymentPlanDAO();
            PaymentPlansServiceModel paymentPlansServiceModels = new PaymentPlansServiceModel();
            UTIER.Result<List<ParametrizationPaymentPlan>, UTIER.ErrorModel> result = paymentPlanDAO.GetParametrizationPaymentPlans();
            if (result is UTIER.ResultError<List<ParametrizationPaymentPlan>, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<List<ParametrizationPaymentPlan>, UTIER.ErrorModel>).Message;
                paymentPlansServiceModels.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                paymentPlansServiceModels.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<List<ParametrizationPaymentPlan>, UTIER.ErrorModel>)
            {
                List<ParametrizationPaymentPlan> parametrizationPaymentPlans = (result as UTIER.ResultValue<List<ParametrizationPaymentPlan>, UTIER.ErrorModel>).Value;
                paymentPlansServiceModels.PaymentPlanServiceModels = ModelsServicesAssembler.CreatePaymentPlanServiceModels(parametrizationPaymentPlans);
            }

            return paymentPlansServiceModels;
        }

        /// <summary>
        /// Obtener lista de planes pago
        /// </summary>
        /// <returns>Plan de pago MOD-S resultado de consulta de plan de pago</returns>
        public PaymentPlansServiceModel GetPaymentPlansByDescription(string description)
        {
            PaymentPlanDAO paymentPlanDAO = new PaymentPlanDAO();
            PaymentPlansServiceModel paymentPlansServiceModels = new PaymentPlansServiceModel();
            UTIER.Result<List<ParametrizationPaymentPlan>, UTIER.ErrorModel> result = paymentPlanDAO.GetPaymentPlansByDescription(description);
            if (result is UTIER.ResultError<List<ParametrizationPaymentPlan>, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<List<ParametrizationPaymentPlan>, UTIER.ErrorModel>).Message;
                paymentPlansServiceModels.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                paymentPlansServiceModels.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<List<ParametrizationPaymentPlan>, UTIER.ErrorModel>)
            {
                List<ParametrizationPaymentPlan> parametrizationPaymentPlans = (result as UTIER.ResultValue<List<ParametrizationPaymentPlan>, UTIER.ErrorModel>).Value;
                paymentPlansServiceModels.PaymentPlanServiceModels = ModelsServicesAssembler.CreatePaymentPlanServiceModels(parametrizationPaymentPlans);
            }

            return paymentPlansServiceModels;
        }

        /// <summary>
        /// Otinen el listado de tipos de riesgo asegurado.
        /// </summary>
        /// <returns>Modelo de servicio de tipo de riesgo asegurado.</returns>
        public MODUD.CoveredRiskTypesServiceModel GetCoveredRiskTypes()
        {
            CoveredRiskTypeDAO coverRiskTypeDao = new CoveredRiskTypeDAO();
            MODUD.CoveredRiskTypesServiceModel coveredRiskTypesServiceModel = new MODUD.CoveredRiskTypesServiceModel();
            UTIER.Result<List<ParamCoveredRiskType>, UTIER.ErrorModel> resultGetCoveredRiskTypes = coverRiskTypeDao.GetCoveredRiskTypes();
            if (resultGetCoveredRiskTypes is UTIER.ResultError<List<ParamCoveredRiskType>, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (resultGetCoveredRiskTypes as UTIER.ResultError<List<ParamCoveredRiskType>, UTIER.ErrorModel>).Message;
                coveredRiskTypesServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                coveredRiskTypesServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
            }
            else
            {
                List<ParamCoveredRiskType> resultValue = (resultGetCoveredRiskTypes as UTIER.ResultValue<List<ParamCoveredRiskType>, UTIER.ErrorModel>).Value;
                coveredRiskTypesServiceModel = ModelsServicesAssembler.MappCoveredRiskTypes(resultValue);
            }

            return coveredRiskTypesServiceModel;
        }

        /// <summary>
        /// Genera archivo excel de tipos de riesgo cubierto.
        /// </summary>
        /// <param name="coveredRiskTypesList">Lista de tipos de riesgo cubierto.</param>
        /// <param name="fileName">Nombre del archivo.</param>
        /// <returns>Modelo de servicio de archivos generados de excel.</returns>
        public MODPA.ExcelFileServiceModel GenerateFileToCoveredRiskTypes(List<MODUD.CoveredRiskTypeServiceModel> coveredRiskTypesList, string fileName)
        {
            List<string> listErrors = new List<string>();
            MODPA.ExcelFileServiceModel covRiskTypeExcelFileServiceModel = new MODPA.ExcelFileServiceModel();
            try
            {
                CoveredRiskTypeDAO fileDAO = new CoveredRiskTypeDAO();
                covRiskTypeExcelFileServiceModel.FileData = fileDAO.GenerateFileToCoveredRiskTypes(coveredRiskTypesList, fileName);
                covRiskTypeExcelFileServiceModel.ErrorTypeService = MODEN.ErrorTypeService.Ok;
                covRiskTypeExcelFileServiceModel.ErrorDescription = listErrors;

                return covRiskTypeExcelFileServiceModel;
            }
            catch (Exception)
            {
                listErrors.Add(Resources.Errors.CoveredRiskTypeErrorGeneratingFile);
                covRiskTypeExcelFileServiceModel.ErrorDescription = listErrors;
                covRiskTypeExcelFileServiceModel.ErrorTypeService = MODEN.ErrorTypeService.TechnicalFault;
                return covRiskTypeExcelFileServiceModel;
            }
        }

        /// <summary>
        /// CRUD de plan de pago
        /// </summary>
        /// <param name="paymentPlanServiceModel">Plan de pago MOD-S</param>
        /// <returns>Listado de plan de pago producto de la operacion del CRUD</returns>
        public List<PaymentPlanServiceModel> ExecuteOperationsPaymentPlanServiceModel(List<PaymentPlanServiceModel> paymentPlanServiceModel)
        {
            PaymentPlanBusiness paymentPlanBusiness = new PaymentPlanBusiness();
            List<PaymentPlanServiceModel> result = new List<PaymentPlanServiceModel>();
            foreach (var itemSM in paymentPlanServiceModel)
            {
                ParametrizationPaymentPlan item = ServicesModelsAssembler.CreateParametrizationPaymentPlan(itemSM);
                item.ParametrizationQuotas = ServicesModelsAssembler.CreateParametrizationQuotas(itemSM.QuotasServiceModel);
                if (itemSM.StatusTypeService == MODEN.StatusTypeService.Delete || itemSM.StatusTypeService == MODEN.StatusTypeService.Create || itemSM.StatusTypeService == MODEN.StatusTypeService.Update/*|| paymentPlanBusiness.ValidateQuotaDistribution(item)*/)
                {
                    PaymentPlanServiceModel itemResult = new PaymentPlanServiceModel();
                    using (Transaction transaction = new Transaction())
                    {
                        itemResult = this.OperationPaymentPlanServiceModel(item, itemSM.StatusTypeService);                        

                        if (itemResult.ErrorServiceModel.ErrorTypeService == MODEN.ErrorTypeService.TechnicalFault)
                        {
                            transaction.Dispose();
                        }
                        else
                        {
                            transaction.Complete();
                        }
                    }

                    result.Add(itemResult);
                }
                else
                {
                    itemSM.ErrorServiceModel = new MODPA.ErrorServiceModel()
                    {
                        ErrorTypeService = MODEN.ErrorTypeService.BusinessFault,
                        ErrorDescription = new List<string>() { Resources.Errors.ErrorSumQuotaEqual100 }
                    };
                    result.Add(itemSM);
                }
            }

            return result;
        }

        /// <summary>
        /// Llamado a DAOS respectivos para operacion del CRUD y operacion con result
        /// </summary>
        /// <param name="parametrizationPaymentPlan">plan de pago</param>
        /// <param name="statusTypeService">enum de proceso a realizar del CRUD</param>
        /// <param name="deleteIsPrincipal">¿se elimina el padre con los hijos?</param>
        /// <returns>item Plan de pago - Resutlado de CRUD </returns>
        public PaymentPlanServiceModel OperationPaymentPlanServiceModel(ParametrizationPaymentPlan parametrizationPaymentPlan, MODEN.StatusTypeService statusTypeService, bool deleteIsPrincipal = true)
        {
            PaymentPlanDAO paymentPlanDAO = new PaymentPlanDAO();
            PaymentPlanServiceModel paymentPlanServiceModelResult = new PaymentPlanServiceModel()
            {
                ErrorServiceModel = new MODPA.ErrorServiceModel()
                {
                    ErrorDescription = new List<string>(),
                    ErrorTypeService = MODEN.ErrorTypeService.Ok
                }
            };
            UTIER.Result<ParametrizationPaymentPlan, UTIER.ErrorModel> result;
            switch (statusTypeService)
            {
                case MODEN.StatusTypeService.Create:
                    result = paymentPlanDAO.CreateParametrizationPaymentPlan(parametrizationPaymentPlan);
                    break;
                case MODEN.StatusTypeService.Update:
                    result = paymentPlanDAO.UpdateParametrizationPaymentPlan(parametrizationPaymentPlan);
                    break;
                case MODEN.StatusTypeService.Delete:
                    // Para el caso de eliminar se utiliza la misma logica, retornando Quotas en 0
                    result = paymentPlanDAO.DeleteParametrizationPaymentPlan(parametrizationPaymentPlan, deleteIsPrincipal);
                    break;
                default:
                    result = paymentPlanDAO.CreateParametrizationPaymentPlan(parametrizationPaymentPlan);
                    break;
            }

            if (result is UTIER.ResultError<ParametrizationPaymentPlan, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<ParametrizationPaymentPlan, UTIER.ErrorModel>).Message;
                paymentPlanServiceModelResult.Description = parametrizationPaymentPlan.Description;
                paymentPlanServiceModelResult.ErrorServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                paymentPlanServiceModelResult.ErrorServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                paymentPlanServiceModelResult.StatusTypeService = statusTypeService;
            }
            else if (result is UTIER.ResultValue<ParametrizationPaymentPlan, UTIER.ErrorModel>)
            {
                ParametrizationPaymentPlan parametrizationPaymentPlanResult = (result as UTIER.ResultValue<ParametrizationPaymentPlan, UTIER.ErrorModel>).Value;
                paymentPlanServiceModelResult = ModelsServicesAssembler.CreatePaymentPlanServiceModel(parametrizationPaymentPlanResult);
                paymentPlanServiceModelResult.StatusTypeService = statusTypeService;

                if (parametrizationPaymentPlan.ParametrizationQuotas.Count > 0)
                {
                    PaymentPlanServiceModel paymentPlanServiceModelQuotas = this.OperationQuotaServiceModel(parametrizationPaymentPlan.ParametrizationQuotas, paymentPlanServiceModelResult.Id);
                    if (paymentPlanServiceModelQuotas.ErrorServiceModel == null || paymentPlanServiceModelQuotas.ErrorServiceModel.ErrorTypeService == MODEN.ErrorTypeService.Ok)
                    {
                        paymentPlanServiceModelResult.QuotasServiceModel = paymentPlanServiceModelQuotas.QuotasServiceModel;
                    }
                    else
                    {
                        paymentPlanServiceModelResult.ErrorServiceModel.ErrorTypeService = paymentPlanServiceModelQuotas.ErrorServiceModel.ErrorTypeService;
                        if (paymentPlanServiceModelResult.ErrorServiceModel.ErrorDescription == null)
                        {
                            paymentPlanServiceModelResult.ErrorServiceModel.ErrorDescription = new List<string>();
                        }

                        paymentPlanServiceModelResult.ErrorServiceModel.ErrorDescription.Add(paymentPlanServiceModelQuotas.ErrorServiceModel.ErrorDescription.ToString());
                    }
                }
            }

            return paymentPlanServiceModelResult;
        }

        /// <summary>
        /// LLamado a DAO para la CRUD de las cuotas  
        /// </summary>
        /// <param name="quotas">Cuotas de plan de pago</param>
        /// <param name="idPaymentPlanServiceModel">Id de plan de pago a insertar cuotas</param>
        /// <returns>Plan de pago MOD-S, resultado de conversion de MOD-B retornado del DAO</returns>
        public PaymentPlanServiceModel OperationQuotaServiceModel(List<ParametrizationQuota> quotas, int idPaymentPlanServiceModel)
        {
            QuotaDAO quotaDAO = new QuotaDAO();
            UTIER.Result<ParametrizationQuota, UTIER.ErrorModel> result;

            UTIER.Result<ParametrizacionQuotaTypeComponent, UTIER.ErrorModel> createComponentQuota;
            ParametrizationPaymentPlan parametrizationPaymentPlanDelete = new ParametrizationPaymentPlan()
            {
                Id = idPaymentPlanServiceModel,
                ParametrizationQuotas = new List<ParametrizationQuota>()
            };
            PaymentPlanServiceModel resultDelete = this.OperationPaymentPlanServiceModel(parametrizationPaymentPlanDelete, MODEN.StatusTypeService.Delete, false);
            PaymentPlanServiceModel paymentPlanServiceModelResult = new PaymentPlanServiceModel();
            if (resultDelete.ErrorServiceModel.ErrorTypeService == MODEN.ErrorTypeService.Ok)
            {

                for(int i = 0; i < quotas.Count; i++)
                {
                    QuotaServiceModel quotaResult = new QuotaServiceModel();
                    quotas[i].Id = idPaymentPlanServiceModel;
                    quotas[i].Number = i + 1;
                    result = quotaDAO.CreateQuota(quotas[i]);

                    foreach (Models.ParametrizacionQuotaTypeComponent quotaComponent in quotas[i].ListQuotaComponent)
                    {
                        quotaComponent.PaymentScheduleId = idPaymentPlanServiceModel;
                        quotaComponent.PaymentNumber = i + 1;
                        quotaComponent.Id = quotaComponent.Id;
                        createComponentQuota = quotaDAO.CreateQuotaComponetType(quotaComponent);
                        
                    }


                    paymentPlanServiceModelResult.QuotasServiceModel = new List<QuotaServiceModel>();

                    if (result is UTIER.ResultError<ParametrizationQuota, UTIER.ErrorModel>)
                    {
                        UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<ParametrizationQuota, UTIER.ErrorModel>).Message;
                        quotaResult.ErrorServiceModel = new MODPA.ErrorServiceModel()
                        {
                            ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType,
                            ErrorDescription = errorModelResult.ErrorDescription
                        };
                    }
                    else if (result is UTIER.ResultValue<ParametrizationQuota, UTIER.ErrorModel>)
                    {
                        ParametrizationQuota quotap = (result as UTIER.ResultValue<ParametrizationQuota, UTIER.ErrorModel>).Value;
                        quotap.ListQuotaComponent = quotas[i].ListQuotaComponent;
                        quotaResult = ModelsServicesAssembler.CreateQuotaServiceModel(quotap);
                    }

                    paymentPlanServiceModelResult.QuotasServiceModel.Add(quotaResult);
                    
                }
            }
            else
            {
                paymentPlanServiceModelResult.ErrorServiceModel = resultDelete.ErrorServiceModel;
            }

            return paymentPlanServiceModelResult;
        }

        /// <summary>
        /// Generar archivo excel de planes de pago
        /// </summary>
        /// <param name="paymentPlans">Listado de planes de pagos</param>
        /// <param name="fileName">Nombre de archivo</param>
        /// <returns>Archivo de excel</returns>
        public MODPA.ExcelFileServiceModel GenerateFileToPaymentPlan(List<PaymentPlanServiceModel> paymentPlans, string fileName)
        {
            PaymentPlanDAO paymentPlanDAO = new PaymentPlanDAO();
            MODPA.ExcelFileServiceModel excelFileServiceModel = new MODPA.ExcelFileServiceModel();

            UTIER.Result<string, UTIER.ErrorModel> result = paymentPlanDAO.GenerateFileToPaymentPlan(ServicesModelsAssembler.CreateParametrizationPaymentPlans(paymentPlans), fileName);
            if (result is UTIER.ResultError<string, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<string, UTIER.ErrorModel>).Message;
                excelFileServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                excelFileServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<string, UTIER.ErrorModel>)
            {
                excelFileServiceModel.FileData = (result as UTIER.ResultValue<string, UTIER.ErrorModel>).Value;
            }

            return excelFileServiceModel;
        }

        /// <summary>
        /// Obtene las unidades de los deducibles
        /// </summary>
        /// <returns>Modelo DeductibleUnitsServiceModel</returns>
        public DeductibleUnitsServiceQueryModel GetDeductibleUnits()
        {
            DeductibleUnitsServiceQueryModel deductibleUnitsServiceModel = new DeductibleUnitsServiceQueryModel();
            DeductibleDAO deductibleDAO = new DeductibleDAO();
            UTIER.Result<List<UNDMO.DeductibleUnit>, UTIER.ErrorModel> result = deductibleDAO.GetDeductibleUnits();
            if (result is UTIER.ResultError<List<UNDMO.DeductibleUnit>, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<List<UNDMO.DeductibleUnit>, UTIER.ErrorModel>).Message;
                deductibleUnitsServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                deductibleUnitsServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<List<UNDMO.DeductibleUnit>, UTIER.ErrorModel>)
            {
                List<UNDMO.DeductibleUnit> deductibles = (result as UTIER.ResultValue<List<UNDMO.DeductibleUnit>, UTIER.ErrorModel>).Value;
                deductibleUnitsServiceModel.DeductibleUnitServiceModels = ModelsServicesAssembler.CreateDeductibleUnitServiceModels(deductibles);
            }

            return deductibleUnitsServiceModel;
        }

        /// <summary>
        /// Obtene el asunto de los deducibles
        /// </summary>
        /// <returns>Modelo DeductibleUnitsServiceModel</returns>
        public DeductibleSubjectsServiceQueryModel GetDeductibleSubjects()
        {
            DeductibleSubjectsServiceQueryModel deductibleSubjectsServiceModel = new DeductibleSubjectsServiceQueryModel();
            DeductibleDAO deductibleDAO = new DeductibleDAO();
            UTIER.Result<List<UNDMO.DeductibleSubject>, UTIER.ErrorModel> result = deductibleDAO.GetDeductibleSubjects();
            if (result is UTIER.ResultError<List<UNDMO.DeductibleSubject>, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<List<UNDMO.DeductibleSubject>, UTIER.ErrorModel>).Message;
                deductibleSubjectsServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                deductibleSubjectsServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<List<UNDMO.DeductibleSubject>, UTIER.ErrorModel>)
            {
                List<UNDMO.DeductibleSubject> deductibles = (result as UTIER.ResultValue<List<UNDMO.DeductibleSubject>, UTIER.ErrorModel>).Value;
                deductibleSubjectsServiceModel.DeductibleSubjectServiceModels = ModelsServicesAssembler.CreateDeductibleSubjectServiceModels(deductibles);
            }

            return deductibleSubjectsServiceModel;
        }

        /// <summary>
        /// Obtene las monedas
        /// </summary>
        /// <returns>Modelo CurrenciesServiceModel</returns>
        public CurrenciesServiceQueryModel GetCurrencies()
        {
            CurrenciesServiceQueryModel currenciesServiceModel = new CurrenciesServiceQueryModel();
            CurrencyDAO currencyDAO = new CurrencyDAO();
            UTIER.Result<List<COMMO.Currency>, UTIER.ErrorModel> result = currencyDAO.GetCurrencies();
            if (result is UTIER.ResultError<List<COMMO.Currency>, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<List<COMMO.Currency>, UTIER.ErrorModel>).Message;
                currenciesServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                currenciesServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<List<COMMO.Currency>, UTIER.ErrorModel>)
            {
                List<COMMO.Currency> currencies = (result as UTIER.ResultValue<List<COMMO.Currency>, UTIER.ErrorModel>).Value;
                currenciesServiceModel.CurrencyServiceModel = ModelsServicesAssembler.CreateCurrencyServiceModels(currencies);
            }

            return currenciesServiceModel;
        }

        /// <summary>
        /// Obtiene los deducibles
        /// </summary>
        /// <returns>Listado de deducible</returns>
        public DeductiblesServiceModel GetDeductibles()
        {
            DeductiblesServiceModel deductiblesServiceModel = new DeductiblesServiceModel();
            DeductibleDAO deductibleDAO = new DeductibleDAO();
            UTIER.Result<List<UNDMO.Deductible>, UTIER.ErrorModel> result = deductibleDAO.GetDeductibles();
            if (result is UTIER.ResultError<List<UNDMO.Deductible>, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<List<UNDMO.Deductible>, UTIER.ErrorModel>).Message;
                deductiblesServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                deductiblesServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<List<UNDMO.Deductible>, UTIER.ErrorModel>)
            {
                List<UNDMO.Deductible> deductibles = (result as UTIER.ResultValue<List<UNDMO.Deductible>, UTIER.ErrorModel>).Value;
                deductiblesServiceModel.DeductibleServiceModel = ModelsServicesAssembler.CreateDeductibleServiceModels(deductibles);
            }

            return deductiblesServiceModel;
        }

        /// <summary>
        /// Obtene las lineas de negocio
        /// </summary>
        /// <returns>Modelo LinesBusinessServiceModel</returns>
        public LinesBusinessServiceQueryModel GetLinesBusiness()
        {
            LinesBusinessServiceQueryModel lineBusinessServiceModel = new LinesBusinessServiceQueryModel();
            LineBusinessDAO lineBusinessDAO = new LineBusinessDAO();
            UTIER.Result<List<ParamLineBusinessModel>, UTIER.ErrorModel> result = lineBusinessDAO.GetLinesBusiness();
            if (result is UTIER.ResultError<List<ParamLineBusinessModel>, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<List<ParamLineBusinessModel>, UTIER.ErrorModel>).Message;
                lineBusinessServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                lineBusinessServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<List<ParamLineBusinessModel>, UTIER.ErrorModel>)
            {
                List<ParamLineBusinessModel> lineBusiness = (result as UTIER.ResultValue<List<ParamLineBusinessModel>, UTIER.ErrorModel>).Value;
                lineBusinessServiceModel.LineBusinessServiceModel = ModelsServicesAssembler.CreateLineBusinessServiceModels(lineBusiness);
            }

            return lineBusinessServiceModel;
        }

        /// <summary>
        /// Validaciones dependencias entidad Deducible
        /// </summary>
        /// <param name="deductibleId">Codigo de deducible</param>
        /// <returns>1: tiene dependencias, 0: sin dependencias</returns>
        public int ValidateDeductible(int deductibleId)
        {
            try
            {
                DeductibleDAO deductibleDAO = new DeductibleDAO();
                return deductibleDAO.ValidateDeductible(deductibleId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Resources.Errors.ErrorGetValidation), ex);
            }
        }

        /// <summary>
        /// Generar archivo excel de deducibles
        /// </summary>
        /// <param name="deductibles">Listado deducibles</param>
        /// <param name="fileName">Nombre archivo</param>
        /// <returns>Modelo ExcelFileServiceModel</returns>
        public MODPA.ExcelFileServiceModel GenerateFileToDeductible(List<DeductibleServiceModel> deductibles, string fileName)
        {
            DeductibleDAO deductibleDAO = new DeductibleDAO();
            MODPA.ExcelFileServiceModel excelFileServiceModel = new MODPA.ExcelFileServiceModel();

            UTIER.Result<string, UTIER.ErrorModel> result = deductibleDAO.GenerateFileToDeductible(ServicesModelsAssembler.CreateDeductibles(deductibles), fileName);
            if (result is UTIER.ResultError<string, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<string, UTIER.ErrorModel>).Message;
                excelFileServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                excelFileServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<string, UTIER.ErrorModel>)
            {
                excelFileServiceModel.FileData = (result as UTIER.ResultValue<string, UTIER.ErrorModel>).Value;
            }

            return excelFileServiceModel;
        }

        #region Amparos
        /// <summary>
        /// Validaciones dependencias entidad Deducible
        /// </summary>
        /// <param name="perilId">Codigo de deducible</param>
        /// <returns>1: tiene dependencias, 0: sin dependencias</returns>
        public int ValidatePeril(int perilId)
        {
            try
            {
                PerilDAO perilDAO = new PerilDAO();
                return perilDAO.ValidatePeril(perilId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Resources.Errors.ErrorGetValidation), ex);
            }
        }

        /// <summary>
        /// Generar archivo excel de amparos
        /// </summary>
        /// <param name="perils">Listado amparos</param>
        /// <param name="fileName">Nombre archivo</param>
        /// <returns>Modelo ExcelFileServiceModel</returns>
        public MODPA.ExcelFileServiceModel GenerateFileToPeril(List<PerilServiceModel> perils, string fileName)
        {
            PerilDAO perilDAO = new PerilDAO();
            MODPA.ExcelFileServiceModel excelFileServiceModel = new MODPA.ExcelFileServiceModel();

            UTIER.Result<string, UTIER.ErrorModel> result = perilDAO.GenerateFileToPeril(ServicesModelsAssembler.CreatePerils(perils), fileName);
            if (result is UTIER.ResultError<string, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<string, UTIER.ErrorModel>).Message;
                excelFileServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                excelFileServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<string, UTIER.ErrorModel>)
            {
                excelFileServiceModel.FileData = (result as UTIER.ResultValue<string, UTIER.ErrorModel>).Value;
            }

            return excelFileServiceModel;
        }

        #endregion

        #region GrupoCobertura
        /// <summary>
        /// Generar archivo excel
        /// </summary>
        /// <param name="coverageGroupRiskTypeServiceModel">Listado grupo de coberturas</param>
        /// <param name="fileName">Nombre archivo</param>
        /// <returns>Modelo ExcelFileServiceModel</returns>
        public MODPA.ExcelFileServiceModel GenerateFileToCoverageGroup(List<CoverageGroupRiskTypeServiceModel> coverageGroupRiskTypeServiceModel, string fileName)
        {
            CoverageGroupDAO coverageGroupDAO = new CoverageGroupDAO();
            MODPA.ExcelFileServiceModel excelFileServiceModel = new MODPA.ExcelFileServiceModel();

            UTIER.Result<string, UTIER.ErrorModel> result = coverageGroupDAO.GenerateFileToCoverageGroup(ServicesModelsAssembler.CreateCoverageGroups(coverageGroupRiskTypeServiceModel), fileName);
            if (result is UTIER.ResultError<string, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<string, UTIER.ErrorModel>).Message;
                excelFileServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                excelFileServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<string, UTIER.ErrorModel>)
            {
                excelFileServiceModel.FileData = (result as UTIER.ResultValue<string, UTIER.ErrorModel>).Value;
            }

            return excelFileServiceModel;
        }
        #endregion

        #region Surcharge
        /// <summary>
        /// CRUD de surcharge
        /// </summary>
        /// <param name="surchargeServiceModel">surcharge MOD-S</param>
        /// <returns>Listado de surcharge producto de la operacion del CRUD</returns>
        public List<SurchargeServiceModel> ExecuteOperationsSurchargeServiceModel(List<SurchargeServiceModel> surchargeServiceModel)
        {
            SurchargeBusiness surchargeBusiness = new SurchargeBusiness();
            List<SurchargeServiceModel> result = new List<SurchargeServiceModel>();
            foreach (var itemSM in surchargeServiceModel)
            {
                ParamSurcharge item = ServicesModelsAssembler.CreateSurcharge(itemSM);

                if (itemSM.StatusTypeService == MODEN.StatusTypeService.Delete || surchargeBusiness.ValidateLengthDescription(item))
                {
                    SurchargeServiceModel itemResult = new SurchargeServiceModel();
                    using (Transaction transaction = new Transaction())
                    {
                        switch (itemSM.StatusTypeService)
                        {
                            case MODEN.StatusTypeService.Create:
                                itemResult = this.ExecuteOperationsSurcharge(item, MODEN.StatusTypeService.Create);
                                break;
                            case MODEN.StatusTypeService.Update:
                                itemResult = this.ExecuteOperationsSurcharge(item, MODEN.StatusTypeService.Update);
                                break;
                            case MODEN.StatusTypeService.Delete:
                                itemResult = this.ExecuteOperationsSurcharge(item, MODEN.StatusTypeService.Delete);
                                break;
                            case MODEN.StatusTypeService.Original:
                                itemResult = new SurchargeServiceModel() { StatusTypeService = MODEN.StatusTypeService.Original, ErrorServiceModel = new MODPA.ErrorServiceModel() { ErrorTypeService = MODEN.ErrorTypeService.Ok } };
                                break;
                            default:
                                break;
                        }

                        if (itemResult.ErrorServiceModel.ErrorTypeService == MODEN.ErrorTypeService.TechnicalFault)
                        {
                            transaction.Dispose();
                        }
                        else
                        {
                            transaction.Complete();
                        }
                    }

                    result.Add(itemResult);
                }
                else
                {
                    itemSM.ErrorServiceModel = new MODPA.ErrorServiceModel()
                    {
                        ErrorTypeService = MODEN.ErrorTypeService.BusinessFault,
                        ErrorDescription = new List<string>() { Resources.Errors.FailedUpdatingSurchargeErrorBD }
                    };
                    result.Add(itemSM);
                }
            }

            return result;
        }

        /// <summary>
        /// CRUD de recargos
        /// </summary>
        /// <param name="surchargeParam">modelo de servicio de recargos</param>
        /// <param name="statusTypeService">estatus de recargos</param>
        /// <returns>si se actualizo correctamente </returns>
        public SurchargeServiceModel ExecuteOperationsSurcharge(ParamSurcharge surchargeParam, MODEN.StatusTypeService statusTypeService)
        {
            SurchargeDAO surchargeDAO = new SurchargeDAO();
            SurchargeServiceModel surchargesServiceModel = new SurchargeServiceModel()
            {
                ErrorServiceModel = new MODPA.ErrorServiceModel()
                {
                    ErrorDescription = new List<string>(),
                    ErrorTypeService = MODEN.ErrorTypeService.Ok
                }
            };

            UTIER.Result<ParamSurcharge, UTIER.ErrorModel> result;
            switch (statusTypeService)
            {
                case MODEN.StatusTypeService.Create:
                    result = surchargeDAO.CreateComponentSurcharge(surchargeParam);
                    break;
                case MODEN.StatusTypeService.Update:
                    result = surchargeDAO.UpdateSurchargeComponent(surchargeParam);
                    break;
                case MODEN.StatusTypeService.Delete:
                    result = surchargeDAO.DeleteSurchargeComponet(surchargeParam);
                    result = surchargeDAO.DeleteComponet(surchargeParam);
                    break;
                default:
                    result = surchargeDAO.CreateComponentSurcharge(surchargeParam);
                    break;
            }

            if (result is UTIER.ResultError<ParamSurcharge, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<ParamSurcharge, UTIER.ErrorModel>).Message;
                surchargesServiceModel.Description = surchargeParam.Description;
                surchargesServiceModel.ErrorServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                surchargesServiceModel.ErrorServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                surchargesServiceModel.StatusTypeService = statusTypeService;
            }
            else if (result is UTIER.ResultValue<ParamSurcharge, UTIER.ErrorModel>)
            {
                ParamSurcharge surcharge = (result as UTIER.ResultValue<ParamSurcharge, UTIER.ErrorModel>).Value;
                surchargesServiceModel = ModelsServicesAssembler.CreateSurchargeServiceModel(surcharge);
                surchargesServiceModel.StatusTypeService = statusTypeService;

                if (surchargesServiceModel.ErrorServiceModel.ErrorDescription == null)
                {
                    surchargesServiceModel.ErrorServiceModel.ErrorDescription = new List<string>();
                }
            }

            return surchargesServiceModel;
        }

        /// <summary>
        /// Obtener lista de recargos
        /// </summary>
        /// <returns>resultado de consulta de recargos</returns>
        public SurchargesServiceModel GetSurchargeServiceModel()
        {
            SurchargesServiceModel surchargesServiceModel = new SurchargesServiceModel();
            SurchargeDAO surchargeDAO = new SurchargeDAO();

            UTIER.Result<List<ParamSurcharge>, UTIER.ErrorModel> result = surchargeDAO.GetSurcharge();
            if (result is UTIER.ResultError<List<ParamSurcharge>, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<List<ParamSurcharge>, UTIER.ErrorModel>).Message;
                surchargesServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                surchargesServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<List<ParamSurcharge>, UTIER.ErrorModel>)
            {
                List<ParamSurcharge> surcharge = (result as UTIER.ResultValue<List<ParamSurcharge>, UTIER.ErrorModel>).Value;
                surchargesServiceModel.SurchargeServiceModel = ModelsServicesAssembler.CreateSurchargesServiceModels(surcharge);
            }

            return surchargesServiceModel;
        }

        /// <summary>
        /// Generar archivo excel de recargos
        /// </summary>
        /// <param name="surcharge">Listado deducibles</param>
        /// <param name="fileName">Nombre archivo</param>
        /// <returns>Modelo ExcelFileServiceModel</returns>
        public MODPA.ExcelFileServiceModel GenerateFileToSurcharge(List<SurchargeServiceModel> surcharge, string fileName)
        {
            SurchargeDAO surchargeDAO = new SurchargeDAO();
            MODPA.ExcelFileServiceModel excelFileServiceModel = new MODPA.ExcelFileServiceModel();

            UTIER.Result<string, UTIER.ErrorModel> result = surchargeDAO.GenerateFileToSurcharge(ServicesModelsAssembler.CreateComponent(surcharge), fileName);
            if (result is UTIER.ResultError<string, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<string, UTIER.ErrorModel>).Message;
                excelFileServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                excelFileServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<string, UTIER.ErrorModel>)
            {
                excelFileServiceModel.FileData = (result as UTIER.ResultValue<string, UTIER.ErrorModel>).Value;
            }

            return excelFileServiceModel;
        }
        #endregion

        #region Discount
        /// <summary>
        /// CRUD de Descuentos
        /// </summary>
        /// <param name="discountServiceModel">Descuentos MOD-S</param>
        /// <returns>Listado de Descuentos producto de la operacion del CRUD</returns>
        public List<DiscountServiceModel> ExecuteOperationsDiscountServiceModel(List<DiscountServiceModel> discountServiceModel)
        {
            DiscountBusiness discounBusiness = new DiscountBusiness();
            List<DiscountServiceModel> result = new List<DiscountServiceModel>();
            foreach (var itemSM in discountServiceModel)
            {
                ParamDiscount item = ServicesModelsAssembler.CreateDiscount(itemSM);

                if (itemSM.StatusTypeService == MODEN.StatusTypeService.Delete || discounBusiness.ValidateLengthDescription(item))
                {
                    DiscountServiceModel itemResult = new DiscountServiceModel();
                    using (Transaction transaction = new Transaction())
                    {
                        switch (itemSM.StatusTypeService)
                        {
                            case MODEN.StatusTypeService.Create:
                                itemResult = this.ExecuteOperationsDiscount(item, MODEN.StatusTypeService.Create);
                                break;
                            case MODEN.StatusTypeService.Update:
                                itemResult = this.ExecuteOperationsDiscount(item, MODEN.StatusTypeService.Update);
                                break;
                            case MODEN.StatusTypeService.Delete:
                                itemResult = this.ExecuteOperationsDiscount(item, MODEN.StatusTypeService.Delete);
                                break;
                            case MODEN.StatusTypeService.Original:
                                itemResult = new DiscountServiceModel() { StatusTypeService = MODEN.StatusTypeService.Original, ErrorServiceModel = new MODPA.ErrorServiceModel() { ErrorTypeService = MODEN.ErrorTypeService.Ok } };
                                break;
                            default:
                                break;
                        }

                        if (itemResult.ErrorServiceModel.ErrorTypeService == MODEN.ErrorTypeService.TechnicalFault)
                        {
                            transaction.Dispose();
                        }
                        else
                        {
                            transaction.Complete();
                        }
                    }

                    result.Add(itemResult);
                }
                else
                {
                    itemSM.ErrorServiceModel = new MODPA.ErrorServiceModel()
                    {
                        ErrorTypeService = MODEN.ErrorTypeService.BusinessFault,
                        ErrorDescription = new List<string>() { Resources.Errors.FailedUpdatingDiscountErrorBD }
                    };
                    result.Add(itemSM);
                }
            }

            return result;
        }

        /// <summary>
        /// CRUD de descuentos
        /// </summary>       
        /// <param name="discountParam">modelo de negocio de descuentos</param>
        /// <param name="statusTypeService">estatus de modelos</param>
        /// <returns>si se actualizo correctamente </returns>
        public DiscountServiceModel ExecuteOperationsDiscount(ParamDiscount discountParam, MODEN.StatusTypeService statusTypeService)
        {
            DiscountDAO discountDAO = new DiscountDAO();
            DiscountServiceModel discountsServiceModel = new DiscountServiceModel()
            {
                ErrorServiceModel = new MODPA.ErrorServiceModel()
                {
                    ErrorDescription = new List<string>(),
                    ErrorTypeService = MODEN.ErrorTypeService.Ok
                }
            };

            UTIER.Result<ParamDiscount, UTIER.ErrorModel> result;
            switch (statusTypeService)
            {
                case MODEN.StatusTypeService.Create:
                    result = discountDAO.CreateComponentDiscount(discountParam);
                    break;
                case MODEN.StatusTypeService.Update:
                    result = discountDAO.UpdateDiscountComponent(discountParam);
                    break;
                case MODEN.StatusTypeService.Delete:
                    result = discountDAO.DeleteDiscountComponet(discountParam);
                    result = discountDAO.DeleteComponet(discountParam);
                    break;
                default:
                    result = discountDAO.CreateComponentDiscount(discountParam);
                    break;
            }

            if (result is UTIER.ResultError<ParamDiscount, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<ParamDiscount, UTIER.ErrorModel>).Message;
                discountsServiceModel.Description = discountParam.Description;
                discountsServiceModel.ErrorServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                discountsServiceModel.ErrorServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                discountsServiceModel.StatusTypeService = statusTypeService;
            }
            else if (result is UTIER.ResultValue<ParamDiscount, UTIER.ErrorModel>)
            {
                ParamDiscount discount = (result as UTIER.ResultValue<ParamDiscount, UTIER.ErrorModel>).Value;
                discountsServiceModel = ModelsServicesAssembler.CreateDiscountServiceModel(discount);
                discountsServiceModel.StatusTypeService = statusTypeService;

                if (discountsServiceModel.ErrorServiceModel.ErrorDescription == null)
                {
                    discountsServiceModel.ErrorServiceModel.ErrorDescription = new List<string>();
                }
            }

            return discountsServiceModel;
        }

        /// <summary>
        /// Obtener lista de descuentos
        /// </summary>
        /// <returns>resultado de consulta de descuentos</returns>
        public DiscountsServiceModel GetDiscountServiceModel()
        {
            DiscountsServiceModel discountsServiceModel = new DiscountsServiceModel();
            DiscountDAO discountDAO = new DiscountDAO();

            UTIER.Result<List<ParamDiscount>, UTIER.ErrorModel> result = discountDAO.GetDiscount();
            if (result is UTIER.ResultError<List<ParamDiscount>, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<List<ParamDiscount>, UTIER.ErrorModel>).Message;
                discountsServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                discountsServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<List<ParamDiscount>, UTIER.ErrorModel>)
            {
                List<ParamDiscount> deductibles = (result as UTIER.ResultValue<List<ParamDiscount>, UTIER.ErrorModel>).Value;
                discountsServiceModel.DiscountServiceModel = ModelsServicesAssembler.CreateDiscountsServiceModels(deductibles);
            }

            return discountsServiceModel;
        }


        /// <summary>
        /// Generar archivo excel de descuentos
        /// </summary>
        /// <param name="discount">Listado descuentos</param>
        /// <param name="fileName">Nombre archivo</param>
        /// <returns>Modelo ExcelFileServiceModel</returns>
        public MODPA.ExcelFileServiceModel GenerateFileToDiscount(List<DiscountServiceModel> discount, string fileName)
        {
            DiscountDAO discountDAO = new DiscountDAO();
            MODPA.ExcelFileServiceModel excelFileServiceModel = new MODPA.ExcelFileServiceModel();

            UTIER.Result<string, UTIER.ErrorModel> result = discountDAO.GenerateFileToDiscount(ServicesModelsAssembler.CreateComponent(discount), fileName);
            if (result is UTIER.ResultError<string, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<string, UTIER.ErrorModel>).Message;
                excelFileServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                excelFileServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<string, UTIER.ErrorModel>)
            {
                excelFileServiceModel.FileData = (result as UTIER.ResultValue<string, UTIER.ErrorModel>).Value;
            }

            return excelFileServiceModel;
        }
        #endregion

        #region SubRamoTecnico
        /// <summary>
        /// Obtiene todos los SubRamoTecnicos
        /// </summary>
        /// <returns>Retorna listado de SubRamoTecnico</returns>
        public SubLineBranchsServiceModel GetSubLinesBusiness()
        {
            SubLineBusinessDAO subLineBusinessDAO = new SubLineBusinessDAO();
            SubLineBranchsServiceModel subLineBranchsServiceModel = new SubLineBranchsServiceModel();
            UTIER.Result<List<COMMO.SubLineBusiness>, UTIER.ErrorModel> result = subLineBusinessDAO.GetSubLinesBusiness();
            if (result is UTIER.ResultError<List<COMMO.SubLineBusiness>, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<List<COMMO.SubLineBusiness>, UTIER.ErrorModel>).Message;
                subLineBranchsServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                subLineBranchsServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<List<COMMO.SubLineBusiness>, UTIER.ErrorModel>)
            {
                List<COMMO.SubLineBusiness> list = (result as UTIER.ResultValue<List<COMMO.SubLineBusiness>, UTIER.ErrorModel>).Value;
                subLineBranchsServiceModel.SubLineBranchService = ModelsServicesAssembler.CreateSubLinesBusinessServiceModel(list);
            }

            return subLineBranchsServiceModel;
        }

        /// <summary>
        /// Obtiene SubRamoTecnico por nombre
        /// </summary>
        /// <param name="name">Recibe Nombre</param>
        /// <returns>Retorna listado por nombre</returns>
        public SubLineBranchsServiceModel GetSubLineBusinessByName(string name)
        {
            SubLineBusinessDAO subLineBusinessDAO = new SubLineBusinessDAO();
            SubLineBranchsServiceModel subLineBusinessServiceModels = new SubLineBranchsServiceModel();
            UTIER.Result<List<COMMO.SubLineBusiness>, UTIER.ErrorModel> result = subLineBusinessDAO.GetSubLineBusinessByNameAndTitle(name);
            if (result is UTIER.ResultError<List<COMMO.SubLineBusiness>, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<List<COMMO.SubLineBusiness>, UTIER.ErrorModel>).Message;
                subLineBusinessServiceModels.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                subLineBusinessServiceModels.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<List<COMMO.SubLineBusiness>, UTIER.ErrorModel>)
            {
                List<COMMO.SubLineBusiness> parametrizationSubLineBusiness = (result as UTIER.ResultValue<List<COMMO.SubLineBusiness>, UTIER.ErrorModel>).Value;
                subLineBusinessServiceModels.SubLineBranchService = ModelsServicesAssembler.CreateSubLinesBusinessServiceModel(parametrizationSubLineBusiness);
            }

            return subLineBusinessServiceModels;
        }

        /// <summary>
        /// Generar archivo excel de SubRamo Tecnico
        /// </summary>
        /// <param name="subLienBusiness">Listado de SubRamo Tecnico</param>
        /// <param name="fileName">Nombre de archivo</param>
        /// <returns>Archivo de excel</returns>
        public MODPA.ExcelFileServiceModel GenerateFileToSubLineBusiness(List<SubLineBranchServiceModel> subLienBusiness, string fileName)
        {
            SubLineBusinessDAO subLineBusinessDAO = new SubLineBusinessDAO();
            MODPA.ExcelFileServiceModel excelFileServiceModel = new MODPA.ExcelFileServiceModel();

            UTIER.Result<string, UTIER.ErrorModel> result = subLineBusinessDAO.GenerateFileToSubLineBusiness(ServicesModelsAssembler.CreateParametrizationSubLinesBusiness(subLienBusiness), fileName);
            if (result is UTIER.ResultError<string, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<string, UTIER.ErrorModel>).Message;
                excelFileServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                excelFileServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<string, UTIER.ErrorModel>)
            {
                excelFileServiceModel.FileData = (result as UTIER.ResultValue<string, UTIER.ErrorModel>).Value;
            }

            return excelFileServiceModel;
        }

        /// <summary>
        /// Obtiene los tipos de detalle
        /// </summary>
        /// <returns>Tipos de detalle</returns>


        /// <summary>
        /// Obtiene el listado de Detalle
        /// </summary>
        /// <returns>Modelo DetailsServiceModel</returns>
        public DetailsServiceModel GetParametrizationDetails()
        {
            DetailsServiceModel detailsServiceModel = new DetailsServiceModel();
            DetailDAO detailDAO = new DetailDAO();
            UTIER.Result<List<ParametrizationDetail>, UTIER.ErrorModel> result = detailDAO.GetParametrizationDetails();
            if (result is UTIER.ResultError<List<ParametrizationDetail>, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<List<ParametrizationDetail>, UTIER.ErrorModel>).Message;
                detailsServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                detailsServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<List<ParametrizationDetail>, UTIER.ErrorModel>)
            {
                List<ParametrizationDetail> parametrizationDetail = (result as UTIER.ResultValue<List<ParametrizationDetail>, UTIER.ErrorModel>).Value;
                detailsServiceModel.DetailServiceModel = ModelsServicesAssembler.CreateDetailsServiceModel(parametrizationDetail);
            }

            return detailsServiceModel;
        }

        /// <summary>
        /// Validación de detalle
        /// </summary>
        /// <param name="detailId">Codigo de detalle</param>
        /// <returns>valor 1: tiene dependencias, 0: no tiene dependencias</returns> 
        public int ValidateDetail(int detailId)
        {
            try
            {
                DetailDAO detailDAO = new DetailDAO();
                return detailDAO.ValidateDetail(detailId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Resources.Errors.ErrorGetValidation), ex);
            }
        }

        /// <summary>
        /// Generar archivo excel de detalles
        /// </summary>
        /// <param name="details">Listado de detalles</param>
        /// <param name="fileName">Nombre de archivo</param>
        /// <returns>Archivo de excel</returns>
        public MODPA.ExcelFileServiceModel GenerateFileToDetail(List<DetailServiceModel> details, string fileName)
        {
            DetailDAO detailDAO = new DetailDAO();
            MODPA.ExcelFileServiceModel excelFileServiceModel = new MODPA.ExcelFileServiceModel();

            UTIER.Result<string, UTIER.ErrorModel> result = detailDAO.GenerateFileToPaymentPlan(ServicesModelsAssembler.CreateParametrizationDetails(details), fileName);
            if (result is UTIER.ResultError<string, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<string, UTIER.ErrorModel>).Message;
                excelFileServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                excelFileServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<string, UTIER.ErrorModel>)
            {
                excelFileServiceModel.FileData = (result as UTIER.ResultValue<string, UTIER.ErrorModel>).Value;
            }

            return excelFileServiceModel;
        }

        /// <summary>
        /// Validaciones descripcion SubRamo Tecnico
        /// </summary>
        /// <param name="description">Recibe descripcion</param>
        /// <returns>Retorna true o false</returns>
        public bool ValidateIfSuBlineBusinessExists(string description)
        {
            try
            {
                SubLineBusinessBusiness subLineBusinessBusiness = new SubLineBusinessBusiness();
                return subLineBusinessBusiness.ValidateIfSuBlineBusinessExists(description);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Resources.Errors.ErrorGetValidation), ex);
            }
        }

        #endregion SubRamoTecnico

        #region AllyCoverage
        
        public List<ParamQueryCoverage> GetAplicationBusinessAllyCoverageByDescription(string data, int num)
        {
            throw new NotImplementedException();
        }

        public List<ParamQueryCoverage> GetBusinessAllyCoverageAvd(ParamQueryCoverage allyCoverage)
        {
            throw new NotImplementedException();
        }

        public string GenerateFileBusinessToAllyCoverage(string fileName)
        {
            try
            {
                var excelFileServiceModel = new ExcelFileServiceModel();
                var allyCoverageValueBusiness = new AllyCoverageBusiness();
                return excelFileServiceModel.FileData = allyCoverageValueBusiness.GenerateFileToAllyCoverage(fileName);//GenerateFileBusinessToCoverageValue(fileName);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        public string GenerateFileBusinessToAllyCoverageList(List<ParamQueryCoverage> li_paramquery, string fileName)
        {
            try
            {
                var excelFileServiceModel = new ExcelFileServiceModel();
                var allyCoverageValueBusiness = new AllyCoverageBusiness();
                return excelFileServiceModel.FileData = allyCoverageValueBusiness.GenerateFileToAllyCoverage(li_paramquery, fileName);//GenerateFileBusinessToCoverageValue(fileName);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<ParamQueryCoverage> GetBusinessAllyCoverage()
        {
            throw new NotImplementedException();
        }

        public ParamQueryCoverage CreateBusinessAllyCoverage(ParamQueryCoverage allyCoverage)
        {
            try
            {
                AllyCoverageBusiness ally_coverage_business = new AllyCoverageBusiness();
                return ally_coverage_business.CreateBusinessAllyCoverage(allyCoverage);//UpdateBusinessAllyCoverage(allyCoverage);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public ParamQueryCoverage UpdateBusinessAllyCoverage(ParamQueryCoverage allyCoverage, ParamQueryCoverage allyCoverageOld)
        {
            try
            {
                AllyCoverageBusiness ally_coverage_business = new AllyCoverageBusiness();
                return ally_coverage_business.UpdateBusinessAllyCoverage(allyCoverage, allyCoverageOld);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public ParamQueryCoverage DeleteBusinessAllyCoverage(ParamQueryCoverage allyCoverage)
        {
            try
            {
                AllyCoverageBusiness ally_coverage_business = new AllyCoverageBusiness();
                return ally_coverage_business.DeleteBusinessAllyCoverage(allyCoverage);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public BaseQueryAllyCoverage GetBusinessCoveragePrincipal()
        {
            throw new NotImplementedException();
        }

        public BaseQueryAllyCoverage GetBusinessCoverageAllyByOnejectinsuredId(int data)
        {
            throw new NotImplementedException();
        }
        
        #endregion

        /// <summary>
        /// Obtiene los tipos de vehiculos
        /// </summary>
        /// <returns>Tipos de vehiculos</returns>
        public VehicleTypesServiceModel GetVehicleTypes()
        {
            VehicleTypesServiceModel vehicleTypesServiceModel = new VehicleTypesServiceModel();
            VehicleTypeDAO dao = new VehicleTypeDAO();
            UTIER.Result<List<ParamVehicleTypeBody>, UTIER.ErrorModel> resultVehicleTypes = dao.GetVehicleTypes();
            return Assemblers.ServicesModelsAssembler.CreateVehicleTypesServiceModel(resultVehicleTypes);
        }

        /// <summary>
        /// Ejecuta las operaciones de creacion, actualizacion y eliminacion
        /// </summary>
        /// <param name="vehicleTypes">Tipos de vehiculos a realizar operacion</param>
        /// <returns>Listado de tipos de vehiculos</returns>
        public List<VehicleTypeServiceModel> ExecuteOperationsVehicleType(List<VehicleTypeServiceModel> vehicleTypes)
        {
            VehicleTypeDAO vehicleTypeDAO = new VehicleTypeDAO();
            foreach (VehicleTypeServiceModel vehicleTypeCreate in vehicleTypes.Where(p => p.StatusTypeService == MODEN.StatusTypeService.Create))
            {
                UTIER.Result<ParamVehicleTypeBody, UTIER.ErrorModel> paramVehicleTypeBody = ModelsServicesAssembler.CreateVehicleTypeBody(vehicleTypeCreate);
                if (paramVehicleTypeBody is UTIER.ResultError<ParamVehicleTypeBody, UTIER.ErrorModel>)
                {
                    vehicleTypeCreate.ErrorServiceModel = new MODPA.ErrorServiceModel()
                    {
                        ErrorDescription = ((UTIER.ResultError<ParamVehicleTypeBody, UTIER.ErrorModel>)paramVehicleTypeBody).Message.ErrorDescription.Select(p => vehicleTypeCreate.ToString() + p).ToList(),
                        ErrorTypeService = (MODEN.ErrorTypeService)((UTIER.ResultError<ParamVehicleTypeBody, UTIER.ErrorModel>)paramVehicleTypeBody).Message.ErrorType
                    };
                }
                else
                {
                    paramVehicleTypeBody = vehicleTypeDAO.Insert((UTIER.ResultValue<ParamVehicleTypeBody, UTIER.ErrorModel>)paramVehicleTypeBody);
                    if (paramVehicleTypeBody is UTIER.ResultError<ParamVehicleTypeBody, UTIER.ErrorModel>)
                    {
                        vehicleTypeCreate.ErrorServiceModel = new MODPA.ErrorServiceModel()
                        {
                            ErrorDescription = ((UTIER.ResultError<ParamVehicleTypeBody, UTIER.ErrorModel>)paramVehicleTypeBody).Message.ErrorDescription.Select(p => vehicleTypeCreate.ToString() + p).ToList(),
                            ErrorTypeService = (MODEN.ErrorTypeService)((UTIER.ResultError<ParamVehicleTypeBody, UTIER.ErrorModel>)paramVehicleTypeBody).Message.ErrorType
                        };
                    }
                }
            }

            foreach (VehicleTypeServiceModel vehicleTypeUpdate in vehicleTypes.Where(p => p.StatusTypeService == MODEN.StatusTypeService.Update))
            {
                UTIER.Result<ParamVehicleTypeBody, UTIER.ErrorModel> paramVehicleTypeBody = ModelsServicesAssembler.CreateVehicleTypeBody(vehicleTypeUpdate);
                if (paramVehicleTypeBody is UTIER.ResultError<ParamVehicleTypeBody, UTIER.ErrorModel>)
                {
                    vehicleTypeUpdate.ErrorServiceModel = new MODPA.ErrorServiceModel()
                    {
                        ErrorDescription = ((UTIER.ResultError<ParamVehicleTypeBody, UTIER.ErrorModel>)paramVehicleTypeBody).Message.ErrorDescription.Select(p => vehicleTypeUpdate.ToString() + p).ToList(),
                        ErrorTypeService = (MODEN.ErrorTypeService)((UTIER.ResultError<ParamVehicleTypeBody, UTIER.ErrorModel>)paramVehicleTypeBody).Message.ErrorType
                    };
                }
                else
                {
                    paramVehicleTypeBody = vehicleTypeDAO.Update((UTIER.ResultValue<ParamVehicleTypeBody, UTIER.ErrorModel>)paramVehicleTypeBody);
                    if (paramVehicleTypeBody is UTIER.ResultError<ParamVehicleTypeBody, UTIER.ErrorModel>)
                    {
                        vehicleTypeUpdate.ErrorServiceModel = new MODPA.ErrorServiceModel()
                        {
                            ErrorDescription = ((UTIER.ResultError<ParamVehicleTypeBody, UTIER.ErrorModel>)paramVehicleTypeBody).Message.ErrorDescription.Select(p => vehicleTypeUpdate.ToString() + p).ToList(),
                            ErrorTypeService = (MODEN.ErrorTypeService)((UTIER.ResultError<ParamVehicleTypeBody, UTIER.ErrorModel>)paramVehicleTypeBody).Message.ErrorType
                        };
                    }
                }
            }

            foreach (VehicleTypeServiceModel vehicleTypeDelete in vehicleTypes.Where(p => p.StatusTypeService == MODEN.StatusTypeService.Delete))
            {
                UTIER.Result<int, UTIER.ErrorModel> resultDelete = vehicleTypeDAO.Delete(vehicleTypeDelete.Id);
                if (resultDelete is UTIER.ResultError<int, UTIER.ErrorModel>)
                {
                    vehicleTypeDelete.ErrorServiceModel = new MODPA.ErrorServiceModel()
                    {
                        ErrorDescription = ((UTIER.ResultError<int, UTIER.ErrorModel>)resultDelete).Message.ErrorDescription.Select(p => vehicleTypeDelete.ToString() + p).ToList(),
                        ErrorTypeService = (MODEN.ErrorTypeService)((UTIER.ResultError<int, UTIER.ErrorModel>)resultDelete).Message.ErrorType
                    };
                }
            }

            return vehicleTypes;
        }

        /// <summary>
        /// Genera el archivo para el tipo de vehiculo
        /// </summary>
        /// <param name="fileName">Nombre de archivo</param>
        /// <returns>Ruta archivo</returns>
        public MODPA.ExcelFileServiceModel GenerateFileToVehicleType(string fileName)
        {
            VehicleTypeDAO vehicleTypeDAO = new VehicleTypeDAO();
            UTIER.Result<string, UTIER.ErrorModel> generatedFile = vehicleTypeDAO.GenerateFileToVehicleType(fileName);
            if (generatedFile is UTIER.ResultError<string, UTIER.ErrorModel>)
            {
                return new MODPA.ExcelFileServiceModel()
                {
                    ErrorTypeService = (MODEN.ErrorTypeService)((UTIER.ResultError<string, UTIER.ErrorModel>)generatedFile).Message.ErrorType,
                    ErrorDescription = ((UTIER.ResultError<string, UTIER.ErrorModel>)generatedFile).Message.ErrorDescription
                };
            }

            return new MODPA.ExcelFileServiceModel()
            {
                ErrorTypeService = MODEN.ErrorTypeService.Ok,
                FileData = ((UTIER.ResultValue<string, UTIER.ErrorModel>)generatedFile).Value
            };
        }

        /// <summary>
        /// Genera el archivo para las carrocerias
        /// </summary>
        /// <param name="vehicleType">Tipo de vehiculo</param>
        /// <param name="fileName">Nombre de archivo</param>
        /// <returns>Ruta de archivo</returns>
        public MODPA.ExcelFileServiceModel GenerateFileToVehicleBody(VehicleTypeServiceModel vehicleType, string fileName)
        {
            VehicleTypeDAO vehicleTypeDAO = new VehicleTypeDAO();
            VehicleBodyDAO vehicleBodyDAO = new VehicleBodyDAO();

            ParamVehicleType vehicleTypeValue = ((UTIER.ResultValue<ParamVehicleType, UTIER.ErrorModel>)ParamVehicleType.GetParamVehicleType(vehicleType.Id, vehicleType.Description, vehicleType.SmallDescription, vehicleType.IsEnable, vehicleType.IsTruck)).Value;

            UTIER.Result<List<ParamVehicleBody>, UTIER.ErrorModel> vehicleBodyResult = vehicleBodyDAO.GetVehicleBodiesByIds(vehicleType.VehicleBodyServiceQueryModel.Select(p => p.Id).ToList());
            if (vehicleBodyResult is UTIER.ResultError<List<ParamVehicleBody>, UTIER.ErrorModel>)
            {
                return new MODPA.ExcelFileServiceModel()
                {
                    ErrorTypeService = (MODEN.ErrorTypeService)((UTIER.ResultError<List<ParamVehicleBody>, UTIER.ErrorModel>)vehicleBodyResult).Message.ErrorType,
                    ErrorDescription = ((UTIER.ResultError<List<ParamVehicleBody>, UTIER.ErrorModel>)vehicleBodyResult).Message.ErrorDescription
                };
            }

            ParamVehicleTypeBody vehicleTypeBody = ((UTIER.ResultValue<ParamVehicleTypeBody, UTIER.ErrorModel>)ParamVehicleTypeBody.GetParamVehicleTypBody(vehicleTypeValue, ((UTIER.ResultValue<List<ParamVehicleBody>, UTIER.ErrorModel>)vehicleBodyResult).Value)).Value;

            UTIER.Result<string, UTIER.ErrorModel> generatedFile = vehicleBodyDAO.GenerateFileToVehicleBody(vehicleTypeBody, fileName);
            if (generatedFile is UTIER.ResultError<string, UTIER.ErrorModel>)
            {
                return new MODPA.ExcelFileServiceModel()
                {
                    ErrorTypeService = (MODEN.ErrorTypeService)((UTIER.ResultError<string, UTIER.ErrorModel>)generatedFile).Message.ErrorType,
                    ErrorDescription = ((UTIER.ResultError<string, UTIER.ErrorModel>)generatedFile).Message.ErrorDescription
                };
            }

            return new MODPA.ExcelFileServiceModel()
            {
                ErrorTypeService = MODEN.ErrorTypeService.Ok,
                FileData = ((UTIER.ResultValue<string, UTIER.ErrorModel>)generatedFile).Value
            };
        }

        /// <summary>
        ///  Obtiene subramos técnicos
        /// </summary>
        /// <param name="lineBusinessId">Id ramo tecnico</param>
        /// <returns>MS-subramos técnicos</returns>
        public MODUD.SubLinesBusinessServiceQueryModel GetSubLinesBusinessByLineBusinessId(int lineBusinessId)
        {
            MODUD.SubLinesBusinessServiceQueryModel sublineBusinessServiceModel = new MODUD.SubLinesBusinessServiceQueryModel();
            LineBusinessDAO lineBusinessDAO = new LineBusinessDAO();

            UTIER.Result<List<COMMO.SubLineBusiness>, UTIER.ErrorModel> result = lineBusinessDAO.GetSubLinesBusinessByLineBusinessId(lineBusinessId);
            if (result is UTIER.ResultError<List<COMMO.SubLineBusiness>, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<List<COMMO.SubLineBusiness>, UTIER.ErrorModel>).Message;
                sublineBusinessServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                sublineBusinessServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<List<COMMO.SubLineBusiness>, UTIER.ErrorModel>)
            {
                List<COMMO.SubLineBusiness> subLineBusiness = (result as UTIER.ResultValue<List<COMMO.SubLineBusiness>, UTIER.ErrorModel>).Value;
                sublineBusinessServiceModel.SubLineBusinessServiceModel = ModelsServicesAssembler.CreateSubLineBusinessServiceQueryModels(subLineBusiness);
            }

            return sublineBusinessServiceModel;
        }

        /// <summary>
        /// Listado de amparos asociados al id del ramo tecnico
        /// </summary>
        /// <param name="lineBusinessId">id del ramo tecnico</param>
        /// <returns>Amparos asociados al id de ramo tecnico - MOD-S</returns>
        public PerilsServiceQueryModel GetPerilsServiceQueryModelByLineBusinessId(int lineBusinessId)
        {
            MODUD.PerilsServiceQueryModel perilsServiceQueryModel = new MODUD.PerilsServiceQueryModel();
            PerilDAO perilDAO = new PerilDAO();

            UTIER.Result<List<ParamPeril>, UTIER.ErrorModel> result = perilDAO.GetPerilsByLineBusinessId(lineBusinessId);
            if (result is UTIER.ResultError<List<ParamPeril>, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<List<ParamPeril>, UTIER.ErrorModel>).Message;
                perilsServiceQueryModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                perilsServiceQueryModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<List<ParamPeril>, UTIER.ErrorModel>)
            {
                List<ParamPeril> perils = (result as UTIER.ResultValue<List<ParamPeril>, UTIER.ErrorModel>).Value;
                perilsServiceQueryModel.PerilServiceQueryModels = ModelsServicesAssembler.CreatePerilsServiceQueryModels(perils);
            }

            return perilsServiceQueryModel;
        }

        /// <summary>
        /// Listado de objetos del seguro por id de ramo tecnico
        /// </summary>
        /// <param name="lineBusinessId">id ramo tecnico</param>
        /// <returns>Objetos del seguro => MOD-S</returns>
        public MODUD.InsuredObjectsServiceQueryModel GetInsuredObjectsServiceQueryModel(int lineBusinessId)
        {
            MODUD.InsuredObjectsServiceQueryModel insuredObjectsServiceQueryModels = new MODUD.InsuredObjectsServiceQueryModel();
            InsuredObjectDAO insuredObjectDAO = new InsuredObjectDAO();

            UTIER.Result<List<ParamInsuredObjectDesc>, UTIER.ErrorModel> result = insuredObjectDAO.GetParamInsuredObjectDescsByLineBusinessId(lineBusinessId);
            if (result is UTIER.ResultError<List<ParamInsuredObjectDesc>, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<List<ParamInsuredObjectDesc>, UTIER.ErrorModel>).Message;
                insuredObjectsServiceQueryModels.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                insuredObjectsServiceQueryModels.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<List<ParamInsuredObjectDesc>, UTIER.ErrorModel>)
            {
                List<ParamInsuredObjectDesc> paramInsuredObjectDescs = (result as UTIER.ResultValue<List<ParamInsuredObjectDesc>, UTIER.ErrorModel>).Value;
                insuredObjectsServiceQueryModels.InsuredObjectServiceQueryModels = ModelsServicesAssembler.CreateInsuredObjectServiceQueryModels(paramInsuredObjectDescs);
            }

            return insuredObjectsServiceQueryModels;
        }

       
        /// <summary>
        /// Listado de clausulas relacionados al conditionlevel
        /// </summary>
        /// <param name="conditionLevelId">id de condicion level</param>
        /// <returns>listado de clausulas</returns>
        public ClausesServiceQueryModel GetClausesSQByConditionLevelId(int conditionLevelId)
        {
            ClausesServiceQueryModel clausesServiceQueryModel = new ClausesServiceQueryModel();
            ClauseDAO clauseDAO = new ClauseDAO();

            UTIER.Result<List<ParamClauseDesc>, UTIER.ErrorModel> result = clauseDAO.GetParamClauseDescsByConditionLevelTypeConditionLevelId(UNDEN.ConditionLevelType.Coverage, conditionLevelId);
            if (result is UTIER.ResultError<List<ParamClauseDesc>, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<List<ParamClauseDesc>, UTIER.ErrorModel>).Message;
                clausesServiceQueryModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                clausesServiceQueryModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<List<ParamClauseDesc>, UTIER.ErrorModel>)
            {
                List<ParamClauseDesc> paramClauseDescs = (result as UTIER.ResultValue<List<ParamClauseDesc>, UTIER.ErrorModel>).Value;
                clausesServiceQueryModel.ClauseServiceModels = ModelsServicesAssembler.CreateClausesServiceQueryModels(paramClauseDescs);
            }

            return clausesServiceQueryModel;
        }

        /// <summary>
        /// Consulta las clausulas relacionadas al tipo de condition level
        /// </summary>
        /// <param name="conditionLevelType">enum tipo condition level</param>
        /// <returns>listado de clausulas</returns>
        public ClausesServiceQueryModel GetClausesSQByConditionLevelType(UNDEN.ConditionLevelType conditionLevelType)
        {
            ClausesServiceQueryModel clausesServiceQueryModel = new ClausesServiceQueryModel();
            ClauseDAO clauseDAO = new ClauseDAO();

            UTIER.Result<List<ParamClauseDesc>, UTIER.ErrorModel> result = clauseDAO.GetParamClauseDescsByConditionLevelType(conditionLevelType);
            if (result is UTIER.ResultError<List<ParamClauseDesc>, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<List<ParamClauseDesc>, UTIER.ErrorModel>).Message;
                clausesServiceQueryModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                clausesServiceQueryModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<List<ParamClauseDesc>, UTIER.ErrorModel>)
            {
                List<ParamClauseDesc> paramClauseDescs = (result as UTIER.ResultValue<List<ParamClauseDesc>, UTIER.ErrorModel>).Value;
                clausesServiceQueryModel.ClauseServiceModels = ModelsServicesAssembler.CreateClausesServiceQueryModels(paramClauseDescs);
            }

            return clausesServiceQueryModel;
        }

        /// <summary>
        /// Consulta listado de deducibles
        /// </summary>
        /// <returns>listado de deducible</returns>
        public DeductiblesServiceQueryModel GetDeductiblesSQM()
        {
            DeductiblesServiceQueryModel deductiblesServiceQueryModel = new DeductiblesServiceQueryModel();
            DeductibleDAO deductibleDAO = new DeductibleDAO();

            UTIER.Result<List<ParamDeductibleDesc>, UTIER.ErrorModel> result = deductibleDAO.GetParamDeductibleDescs();
            if (result is UTIER.ResultError<List<ParamDeductibleDesc>, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<List<ParamDeductibleDesc>, UTIER.ErrorModel>).Message;
                deductiblesServiceQueryModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                deductiblesServiceQueryModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<List<ParamDeductibleDesc>, UTIER.ErrorModel>)
            {
                List<ParamDeductibleDesc> paramDeductibleDesc = (result as UTIER.ResultValue<List<ParamDeductibleDesc>, UTIER.ErrorModel>).Value;
                deductiblesServiceQueryModel.DeductibleServiceQueryModels = ModelsServicesAssembler.CreateDeductibleServiceQueryModels(paramDeductibleDesc);
            }

            return deductiblesServiceQueryModel;
        }

        /// <summary>
        /// Consulta listado de TIPOS DETALLES
        /// REQ_#57
        /// </summary>
        /// <returns>listado de deducible</returns>
        public DetailTypesServiceQueryModel  GetDetailTypeSQM()
        {
            DetailTypesServiceQueryModel DetailTypeServiceQueryModel = new DetailTypesServiceQueryModel();
            
            DetailDAO detailDAO = new DetailDAO();
            UTIER.Result<List<ParamDetailTypeDesc>, UTIER.ErrorModel> result = detailDAO.GetParamDetailDescs();

            
            if (result is UTIER.ResultError<List<ParamDetailTypeDesc>, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<List<ParamDetailTypeDesc>, UTIER.ErrorModel>).Message;
                DetailTypeServiceQueryModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                DetailTypeServiceQueryModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<List<ParamDetailTypeDesc>, UTIER.ErrorModel>)
            {
                List<ParamDetailTypeDesc> paramDetailTypeDesc  = (result as UTIER.ResultValue<List<ParamDetailTypeDesc>, UTIER.ErrorModel>).Value;
                DetailTypeServiceQueryModel.DetailTypeServiceQueryModel = ModelsServicesAssembler.CreateDetailTypesServiceQueryModels(paramDetailTypeDesc);
            }

            return DetailTypeServiceQueryModel;
        }

        /// <summary>
        /// Coberturas asociadas a descripcion y id ramo tecnico
        /// </summary>
        /// <param name="description">descipcion de cobertura, puede ser descripcion o id de cobertura</param>
        /// <param name="technicalBranchId">id de ramo tecnico</param>
        /// <returns>listado de coberturas</returns>
        public CoveragesServiceModel GetCoveragesSMByDescriptionTechnicalBranchId(string description, int? technicalBranchId)
        {
            return this.GetRelationCoveragesServiceModel(this.GetCoveragesAndCoCoverageSMByDescriptionTechnicalBranchId(description, technicalBranchId));
        }

        /// <summary>
        /// Consulta de coberturas busqueda avanzada
        /// </summary>
        /// <param name="coverageServiceModel">parametros de cobertura</param>
        /// <returns>listado de coberturas</returns>
        public CoveragesServiceModel GetCoverageSMBySearchAdv(CoverageServiceModel coverageServiceModel)
        {
            return this.GetRelationCoveragesServiceModel(this.GetCoveragesAndCoCoverageSMByParamCoverage(coverageServiceModel));
        }

        /// <summary>
        /// Obtiene las relaciones asociadas a la cobertura
        /// </summary>
        /// <param name="coveragesSM">coberturas a relacionar</param>
        /// <returns>listado de coberturas con relaciones</returns>
        public CoveragesServiceModel GetRelationCoveragesServiceModel(CoveragesServiceModel coveragesSM)
        {
            if (coveragesSM.CoverageServiceModels != null && coveragesSM.CoverageServiceModels.Count == 1) //Se consulta toda la información, si se trata de un registro
            {
                coveragesSM.CoverageServiceModels.AsParallel().ForAll(b => b.DeductiblesServiceQueryModel = this.GetDeductiblesSQMByCoverageId(b.Id));
                
                coveragesSM.CoverageServiceModels.AsParallel().ForAll(b => b.ClausesServiceQueryModel = this.GetClausesSQByConditionLevelId(b.Id));

                ///// REQ_#57
                coveragesSM.CoverageServiceModels.AsParallel().ForAll(b => b.DetailTypesServiceQueryModel = this.GetDetailtypesSQMByCoverageId(b.Id));
                // Validacion de errores
                if (coveragesSM.CoverageServiceModels.AsParallel().Any(b => b.ClausesServiceQueryModel.ErrorTypeService != MODEN.ErrorTypeService.Ok))
                {
                    coveragesSM.ErrorTypeService = MODEN.ErrorTypeService.TechnicalFault;
                    foreach (var item in coveragesSM.CoverageServiceModels)
                    {
                        if (item.ClausesServiceQueryModel != null)
                        {
                            coveragesSM.ErrorDescription.AddRange(item.ClausesServiceQueryModel.ErrorDescription);
                        }
                    }

                    // validar codigo para mejorar rendimiento=>coveragesSM.ErrorDescription.AddRange(coveragesSM.CoverageServiceModels.AsParallel().Where(b=>b.ClausesServiceQueryModel!=null).SelectMany(z => z.ClausesServiceQueryModel.ErrorDescription).ToList());
                }

                if (coveragesSM.CoverageServiceModels.AsParallel().Any(b => b.DeductiblesServiceQueryModel.ErrorTypeService != MODEN.ErrorTypeService.Ok))
                {
                    coveragesSM.ErrorTypeService = MODEN.ErrorTypeService.TechnicalFault;
                    foreach (var item in coveragesSM.CoverageServiceModels)
                    {
                        if (item.DeductiblesServiceQueryModel != null)
                        {
                            coveragesSM.ErrorDescription.AddRange(item.DeductiblesServiceQueryModel.ErrorDescription);
                        }
                    }
                }

                if (coveragesSM.CoverageServiceModels.AsParallel().Any(b => b.DetailTypesServiceQueryModel.ErrorTypeService != MODEN.ErrorTypeService.Ok))
                {
                    coveragesSM.ErrorTypeService = MODEN.ErrorTypeService.TechnicalFault;
                    foreach (var item in coveragesSM.CoverageServiceModels)
                    {
                        if (item.DetailTypesServiceQueryModel != null)
                        {
                            coveragesSM.ErrorDescription.AddRange(item.DetailTypesServiceQueryModel.ErrorDescription);
                        }
                    }
                }
            }

            return coveragesSM;
        }


        /// <summary>
        /// Busqueda de coberturas y cocoverages
        /// </summary>
        /// <param name="coverageServiceModel">parametros de coberturas</param>
        /// <returns>listado de coberturas y cocoverages</returns>
        public CoveragesServiceModel GetCoveragesAndCoCoverageSMByParamCoverage(CoverageServiceModel coverageServiceModel)
        {
            CoveragesServiceModel coveragesServiceModel = new CoveragesServiceModel();
            CoverageDAO coverageDAO = new CoverageDAO();

            ParamCoverage paramCoverage = ServicesModelsAssembler.CreateParamCoverage(coverageServiceModel);
            UTIER.Result<List<ParamCoverage>, UTIER.ErrorModel> result = coverageDAO.GetParamCoveragesByParamCoverage(paramCoverage);
            if (result is UTIER.ResultError<List<ParamCoverage>, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<List<ParamCoverage>, UTIER.ErrorModel>).Message;
                coveragesServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                coveragesServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<List<ParamCoverage>, UTIER.ErrorModel>)
            {
                List<ParamCoverage> paramCoverages = (result as UTIER.ResultValue<List<ParamCoverage>, UTIER.ErrorModel>).Value;
                coveragesServiceModel.CoverageServiceModels = ModelsServicesAssembler.CreateCoverageServiceModels(paramCoverages);
            }

            return coveragesServiceModel;
        }

        /// <summary>
        /// Consulta de coberturas y cocoverages
        /// </summary>
        /// <param name="description">descripcion de cobertura, puede ser descripcion o id de cobertura</param>
        /// <param name="technicalBranchId">id de ramo tecnico</param>
        /// <returns>listado de coberturas</returns>
        public CoveragesServiceModel GetCoveragesAndCoCoverageSMByDescriptionTechnicalBranchId(string description, int? technicalBranchId)
        {
            CoveragesServiceModel coveragesServiceModel = new CoveragesServiceModel();
            CoverageDAO coverageDAO = new CoverageDAO();

            UTIER.Result<List<ParamCoverage>, UTIER.ErrorModel> result = coverageDAO.GetParamCoveragesByDescription(description, technicalBranchId);
            if (result is UTIER.ResultError<List<ParamCoverage>, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<List<ParamCoverage>, UTIER.ErrorModel>).Message;
                coveragesServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                coveragesServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<List<ParamCoverage>, UTIER.ErrorModel>)
            {
                List<ParamCoverage> paramCoverages = (result as UTIER.ResultValue<List<ParamCoverage>, UTIER.ErrorModel>).Value;
                coveragesServiceModel.CoverageServiceModels = ModelsServicesAssembler.CreateCoverageServiceModels(paramCoverages);
                coveragesServiceModel.ErrorDescription = new List<string>();
                coveragesServiceModel.ErrorTypeService = MODEN.ErrorTypeService.Ok;
            }

            return coveragesServiceModel;
        }

        /// <summary>
        /// Consulta de deducibles relacionados al id de cobertura
        /// </summary>
        /// <param name="coverageId">id de cobertura</param>
        /// <returns>listado de deducibles</returns>
        public DeductiblesServiceQueryModel GetDeductiblesSQMByCoverageId(int coverageId)
        {
            DeductiblesServiceQueryModel deductiblesServiceQueryModel = new DeductiblesServiceQueryModel();
            DeductibleDAO deductibleDAO = new DeductibleDAO();

            UTIER.Result<List<ParamDeductibleDesc>, UTIER.ErrorModel> result = deductibleDAO.GetParamDeductibleDescsByCoverageId(coverageId);
            if (result is UTIER.ResultError<List<ParamDeductibleDesc>, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<List<ParamDeductibleDesc>, UTIER.ErrorModel>).Message;
                deductiblesServiceQueryModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                deductiblesServiceQueryModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<List<ParamDeductibleDesc>, UTIER.ErrorModel>)
            {
                List<ParamDeductibleDesc> paramDeductibleDesc = (result as UTIER.ResultValue<List<ParamDeductibleDesc>, UTIER.ErrorModel>).Value;
                deductiblesServiceQueryModel.DeductibleServiceQueryModels = ModelsServicesAssembler.CreateDeductibleServiceQueryModels(paramDeductibleDesc);
            }

            return deductiblesServiceQueryModel;
        }

        /// <summary>
        /// Obtener lista de niveles de influencia
        /// </summary>
        /// <returns>Niveles de Influencia</returns>
        public MODUD.CompositionTypesServiceQueryModel GetCompositionTypes()
        {
            MODUD.CompositionTypesServiceQueryModel compositionTypesServiceQueryModel = new MODUD.CompositionTypesServiceQueryModel();
            CompositionDAO compositionDAO = new CompositionDAO();
            UTIER.Result<List<ParamComposition>, UTIER.ErrorModel> result = compositionDAO.GetParametrizationCompositions();
            if (result is UTIER.ResultError<List<ParamComposition>, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<List<ParamComposition>, UTIER.ErrorModel>).Message;
                compositionTypesServiceQueryModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                compositionTypesServiceQueryModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<List<ParamComposition>, UTIER.ErrorModel>)
            {
                List<ParamComposition> paramCompositionType = (result as UTIER.ResultValue<List<ParamComposition>, UTIER.ErrorModel>).Value;
                compositionTypesServiceQueryModel.CompositionTypeServiceQueryModels = ModelsServicesAssembler.CreateClausesServiceQueryModels(paramCompositionType);
            }
            return compositionTypesServiceQueryModel;
        }


        /// <summary>
        /// Obtiene los tipos de detalle
        /// </summary>
        /// <returns>Tipos de detalle</returns>
        public DetailTypesServiceQueryModel GetDetailTypes()
        {
            DetailTypesServiceQueryModel detailTypesServiceQueryModel = new DetailTypesServiceQueryModel();
            DetailTypeDAO detailTypeDAO = new DetailTypeDAO();
            UTIER.Result<List<DetailType>, UTIER.ErrorModel> result = detailTypeDAO.GetDetailTypes();
            if (result is UTIER.ResultError<List<DetailType>, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<List<DetailType>, UTIER.ErrorModel>).Message;
                detailTypesServiceQueryModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                detailTypesServiceQueryModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<List<DetailType>, UTIER.ErrorModel>)
            {
                List<DetailType> deductibles = (result as UTIER.ResultValue<List<DetailType>, UTIER.ErrorModel>).Value;
                detailTypesServiceQueryModel.DetailTypeServiceQueryModel = ModelsServicesAssembler.CreateDetailTypesServiceModels(deductibles);
            }

            return detailTypesServiceQueryModel;
        }



        /// <summary>
        /// Consulta de tipos de detalle relacionados al id de cobertura
        /// </summary>
        /// <param name="coverageId">id de cobertura</param>
        /// <returns>listado de deducibles</returns>
        public DetailTypesServiceQueryModel GetDetailtypesSQMByCoverageId(int coverageId)
        {
            DetailTypesServiceQueryModel detailTypesServiceQueryModel = new DetailTypesServiceQueryModel();
            
            DetailDAO detailDAO = new DetailDAO();
            
            UTIER.Result<List<ParamDetailTypeDesc >, UTIER.ErrorModel> result = detailDAO.GetParamDetailTypesDescsByCoverageId(coverageId);
            if (result is UTIER.ResultError<List<ParamDetailTypeDesc>, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<List<ParamDetailTypeDesc>, UTIER.ErrorModel>).Message;
                detailTypesServiceQueryModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                detailTypesServiceQueryModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<List<ParamDetailTypeDesc>, UTIER.ErrorModel>)
            {
                List<ParamDetailTypeDesc> paramDetailTypeDesc = (result as UTIER.ResultValue<List<ParamDetailTypeDesc>, UTIER.ErrorModel>).Value;
                detailTypesServiceQueryModel.DetailTypeServiceQueryModel = ModelsServicesAssembler.CreateDetailTypesServiceQueryModels(paramDetailTypeDesc);
            }

            return detailTypesServiceQueryModel;
        }

        /// <summary>
        /// Genera el archivo de excel de todas las coberturas
        /// </summary>
        /// <param name="fileName">nombre de archivo a generar</param>
        /// <returns></returns>
        public MODPA.ExcelFileServiceModel GenerateFileToCoverage(string fileName)
        {
            CoverageDAO coverageDAO = new CoverageDAO();
            MODPA.ExcelFileServiceModel excelFileServiceModel = new MODPA.ExcelFileServiceModel();
            CoveragesServiceModel coverages = this.GetCoveragesServiceModel();
            UTIER.Result<string, UTIER.ErrorModel> result = coverageDAO.GenerateFileToCoverage(ServicesModelsAssembler.CreateParamCoverages(coverages.CoverageServiceModels), fileName);
            if (result is UTIER.ResultError<string, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<string, UTIER.ErrorModel>).Message;
                excelFileServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                excelFileServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<string, UTIER.ErrorModel>)
            {
                excelFileServiceModel.FileData = (result as UTIER.ResultValue<string, UTIER.ErrorModel>).Value;
            }

            return excelFileServiceModel;
        }

        /// <summary>
        /// Obtiene listado de coberturas
        /// </summary>        
        /// <returns>listado de coberturas</returns>
        public CoveragesServiceModel GetCoveragesServiceModel()
        {
            CoveragesServiceModel coveragesServiceModel = new CoveragesServiceModel();
            CoverageDAO coverageDAO = new CoverageDAO();

            UTIER.Result<List<ParamCoverage>, UTIER.ErrorModel> result = coverageDAO.GetParamCoverages();
            if (result is UTIER.ResultError<List<ParamCoverage>, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<List<ParamCoverage>, UTIER.ErrorModel>).Message;
                coveragesServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                coveragesServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<List<ParamCoverage>, UTIER.ErrorModel>)
            {
                List<ParamCoverage> paramCoverages = (result as UTIER.ResultValue<List<ParamCoverage>, UTIER.ErrorModel>).Value;
                coveragesServiceModel.CoverageServiceModels = ModelsServicesAssembler.CreateCoverageServiceModels(paramCoverages);
            }

            return coveragesServiceModel;
        }

        /// <summary>
        /// CRUD de cobertura
        /// </summary>
        /// <param name="coverage">cobertura a modificar</param>
        /// <returns>cobertura modificada</returns>
        public CoverageServiceModel ExecuteOperationCoverage(CoverageServiceModel coverage)
        {
            CoverageServiceModel result = new CoverageServiceModel();
            result.ErrorServiceModel = new MODPA.ErrorServiceModel()
            {
                ErrorTypeService = MODEN.ErrorTypeService.BusinessFault,
                ErrorDescription = new List<string>(),
            };
            MODPA.ErrorServiceModel errorServiceModel = new MODPA.ErrorServiceModel() { ErrorTypeService = MODEN.ErrorTypeService.Ok };
            if (coverage.StatusTypeService == MODEN.StatusTypeService.Create)
            {
                CoverageBusiness coverageBusiness = new CoverageBusiness();
                CoveragesServiceModel validateRelation = this.ValidateRelation(coverage);
                errorServiceModel = coverageBusiness.ValidateCoverage(validateRelation);
            }

            if (errorServiceModel.ErrorTypeService == MODEN.ErrorTypeService.Ok)
            {
                using (Transaction transaction = new Transaction())
                {
                    result = this.OperationCoverage(coverage, coverage.StatusTypeService);

                    // Validacion de error en el result
                    if ((result.ErrorServiceModel != null && result.ErrorServiceModel.ErrorTypeService == MODEN.ErrorTypeService.TechnicalFault) ||
                        result.CoCoverageServiceModel.ErrorServiceModel.ErrorTypeService == MODEN.ErrorTypeService.TechnicalFault ||
                        result.ClausesServiceQueryModel.ErrorTypeService == MODEN.ErrorTypeService.TechnicalFault)
                    {
                        transaction.Dispose();
                    }
                    else
                    {
                        transaction.Complete();
                    }
                }
            }
            else
            {
                result.ErrorServiceModel = errorServiceModel;
                result.StatusTypeService = MODEN.StatusTypeService.Error;
            }

            return result;
        }

        /// <summary>
        /// Listado de coberturas existentes con parametros -ramo,subramo,amparo,objeto del s
        /// </summary>
        /// <param name="coverage">cobertura con parametros</param>
        /// <returns>listado de coberturas</returns>
        public CoveragesServiceModel ValidateRelation(CoverageServiceModel coverage)
        {
            CoveragesServiceModel result = new CoveragesServiceModel();
            CoverageDAO coverageDAO = new CoverageDAO();
            UTIER.Result<List<ParamCoverage>, UTIER.ErrorModel> resultDAO = coverageDAO.GetParamCoveragesByLineBusinessIdSubLineBusinessId(coverage.LineBusiness.Id, coverage.SubLineBusiness.Id, coverage.Peril.Id, coverage.InsuredObject.Id);
            if (resultDAO is UTIER.ResultError<List<ParamCoverage>, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (resultDAO as UTIER.ResultError<List<ParamCoverage>, UTIER.ErrorModel>).Message;
                result = new CoveragesServiceModel
                {
                    ErrorDescription = errorModelResult.ErrorDescription,
                    ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType
                };
            }
            else if (resultDAO is UTIER.ResultValue<List<ParamCoverage>, UTIER.ErrorModel>)
            {
                List<ParamCoverage> paramCoverageResults = (resultDAO as UTIER.ResultValue<List<ParamCoverage>, UTIER.ErrorModel>).Value;
                result.CoverageServiceModels = ModelsServicesAssembler.CreateCoverageServiceModels(paramCoverageResults);
            }

            return result;
        }

        /// <summary>
        /// Coberturas existentes relacionadas con la descripcion
        /// </summary>
        /// <param name="description">descripcion de cobertura</param>
        /// <returns>listado de coberturas</returns>
        public CoveragesServiceModel ValidateDescriptionCoverage(string description)
        {
            CoveragesServiceModel result = new CoveragesServiceModel();
            CoverageDAO coverageDAO = new CoverageDAO();
            UTIER.Result<List<ParamCoverage>, UTIER.ErrorModel> resultDAO = coverageDAO.GetParamCoveragesByDescription(description);
            if (resultDAO is UTIER.ResultError<List<ParamCoverage>, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (resultDAO as UTIER.ResultError<List<ParamCoverage>, UTIER.ErrorModel>).Message;
                result = new CoveragesServiceModel
                {
                    ErrorDescription = errorModelResult.ErrorDescription,
                    ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType
                };
            }
            else if (resultDAO is UTIER.ResultValue<List<ParamCoverage>, UTIER.ErrorModel>)
            {
                List<ParamCoverage> paramCoverageResults = (resultDAO as UTIER.ResultValue<List<ParamCoverage>, UTIER.ErrorModel>).Value;
                result.CoverageServiceModels = ModelsServicesAssembler.CreateCoverageServiceModels(paramCoverageResults);
            }

            return result;
        }

        /// <summary>
        /// Operacion de cobertura
        /// </summary>
        /// <param name="coverage">cobertura a modificar</param>
        /// <param name="statusTypeService">tipo de operacion a realizar</param>
        /// <returns>cobertura modificada</returns>
        public CoverageServiceModel OperationCoverage(CoverageServiceModel coverage, MODEN.StatusTypeService statusTypeService)
        {
            ParamCoverage paramCoverage = ServicesModelsAssembler.CreateParamCoverage(coverage);
            CoverageDAO coverageDAO = new CoverageDAO();
            UTIER.Result<ParamCoverage, UTIER.ErrorModel> resultDAO;
            switch (statusTypeService)
            {
                case MODEN.StatusTypeService.Create:
                    resultDAO = coverageDAO.CreateParamCoverage(paramCoverage);
                    break;
                case MODEN.StatusTypeService.Update:
                    resultDAO = coverageDAO.UpdateParamCoverage(paramCoverage);
                    break;
                case MODEN.StatusTypeService.Delete:
                default:
                    resultDAO = coverageDAO.DeleteParamCoverage(paramCoverage, true);
                    break;
            }

            CoverageServiceModel result = new CoverageServiceModel();
            if (resultDAO is UTIER.ResultError<ParamCoverage, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (resultDAO as UTIER.ResultError<ParamCoverage, UTIER.ErrorModel>).Message;
                result.ErrorServiceModel = new MODPA.ErrorServiceModel()
                {
                    ErrorDescription = errorModelResult.ErrorDescription,
                    ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType
                };
                result.StatusTypeService = statusTypeService;
            }
            else if (resultDAO is UTIER.ResultValue<ParamCoverage, UTIER.ErrorModel>)
            {
                ParamCoverage paramCoverageResult = (resultDAO as UTIER.ResultValue<ParamCoverage, UTIER.ErrorModel>).Value;
                result = ModelsServicesAssembler.CreateCoverageServiceModel(paramCoverageResult);
                result.StatusTypeService = statusTypeService;
                result.ErrorServiceModel = new MODPA.ErrorServiceModel()
                {
                    ErrorTypeService = MODEN.ErrorTypeService.Ok,
                    ErrorDescription = new List<string>()
                };

                // Eliminacion de hijos
                UTIER.Result<ParamCoverage, UTIER.ErrorModel> resultCoveragesDelete = coverageDAO.DeleteParamCoverage(paramCoverage, false);
                if (resultCoveragesDelete is UTIER.ResultValue<ParamCoverage, UTIER.ErrorModel>)
                {
                    // Creación de hijo: CoCoverage
                    if (coverage.CoCoverageServiceModel != null)
                    {
                        coverage.CoCoverageServiceModel.Id = 1;
                        coverage.CoCoverageServiceModel.IsChild = false;
                        result.CoCoverageServiceModel = this.OperationCoCoverage(coverage.CoCoverageServiceModel, paramCoverageResult.Id);
                    }

                    if (coverage.CoCoverageServiceModels != null)
                    {
                        int coverageId = 2;
                        for (int i = 0; i < coverage.CoCoverageServiceModels.Count; i++)
                        {
                            coverage.CoCoverageServiceModels[i].Id = coverageId;
                            coverage.CoCoverageServiceModels[i].IsChild = true;
                            result.CoCoverageServiceModels.Add(this.OperationCoCoverage(coverage.CoCoverageServiceModels[i], paramCoverageResult.Id));
                            coverageId++;
                        }
                    }

                    // Creación de hijo: Clausulas
                    if (coverage.ClausesServiceQueryModel != null && coverage.ClausesServiceQueryModel.ClauseServiceModels != null)
                    {
                        result.ClausesServiceQueryModel = this.OperationClausesDesc(coverage.ClausesServiceQueryModel, paramCoverageResult.Id);
                    }

                    // Creación de hijo: Deducibles
                    if (coverage.DeductiblesServiceQueryModel != null && coverage.DeductiblesServiceQueryModel.DeductibleServiceQueryModels != null)
                    {
                        result.DeductiblesServiceQueryModel = this.OperationDeductiblesSQM(coverage.DeductiblesServiceQueryModel, paramCoverageResult.Id);
                    }
                    if (coverage.DetailTypesServiceQueryModel !=null && coverage.DetailTypesServiceQueryModel.DetailTypeServiceQueryModel != null)
                    {
                           result.DetailTypesServiceQueryModel = this.OperationDetailTypesSQM(coverage.DetailTypesServiceQueryModel, paramCoverageResult.Id);
                    }

                }
                else
                {
                    UTIER.ErrorModel errorModelResult = (resultCoveragesDelete as UTIER.ResultError<ParamCoverage, UTIER.ErrorModel>).Message;
                    result.ErrorServiceModel.ErrorDescription.AddRange(errorModelResult.ErrorDescription);
                    result.ErrorServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                    result.StatusTypeService = statusTypeService;
                }
            }

            return result;
        }

        /// <summary>
        /// Operacion con la CoCoverage
        /// </summary>
        /// <param name="cocoverageServiceModel">cocoverage a modificar</param>
        /// <param name="coverageId">id de cobertura</param>
        /// <returns>CoCoverage modificada</returns>
        public CoCoverageServiceModel OperationCoCoverage(CoCoverageServiceModel cocoverageServiceModel, int coverageId)
        {
            CoCoverageServiceModel result = new CoCoverageServiceModel();
            CoverageDAO coverageDAO = new CoverageDAO();
            ParamCoCoverage paramCoCoverage = ServicesModelsAssembler.CreateParamCoCoverage(cocoverageServiceModel);
            UTIER.Result<ParamCoCoverage, UTIER.ErrorModel> resultDAO = coverageDAO.CreateCoCoverage(paramCoCoverage, coverageId);
            if (resultDAO is UTIER.ResultError<ParamCoCoverage, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (resultDAO as UTIER.ResultError<ParamCoCoverage, UTIER.ErrorModel>).Message;
                result.ErrorServiceModel = new MODPA.ErrorServiceModel()
                {
                    ErrorDescription = errorModelResult.ErrorDescription
                };
                result.ErrorServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                result.StatusTypeService = MODEN.StatusTypeService.Create;
            }
            else
            {
                ParamCoCoverage paramCoCoverageResult = (resultDAO as UTIER.ResultValue<ParamCoCoverage, UTIER.ErrorModel>).Value;
                result = ModelsServicesAssembler.CreateCoCoverageServiceModel(paramCoCoverageResult);
                result.ErrorServiceModel = new MODPA.ErrorServiceModel() { ErrorTypeService = MODEN.ErrorTypeService.Ok };
                result.StatusTypeService = MODEN.StatusTypeService.Create;
            }

            return result;
        }

        /// <summary>
        /// Operacion con clausula
        /// </summary>
        /// <param name="clausesServiceQueryModel">clausula a modificar</param>
        /// <param name="coverageId">id de cobertura</param>
        /// <returns>clausula modificada</returns>
        public ClausesServiceQueryModel OperationClausesDesc(ClausesServiceQueryModel clausesServiceQueryModel, int coverageId)
        {
            ClausesServiceQueryModel result = new ClausesServiceQueryModel()
            {
                ErrorTypeService = MODEN.ErrorTypeService.Ok,
                ErrorDescription = new List<string>(),
                ClauseServiceModels = new List<ClauseServiceQueryModel>()
            };
            ClauseDAO clauseDAO = new ClauseDAO();
            List<ParamClauseDesc> paramClauseDescs = ServicesModelsAssembler.CreateParamClauseDescs(clausesServiceQueryModel.ClauseServiceModels);

            // Se recorre listado para insercion uno a uno
            foreach (ParamClauseDesc item in paramClauseDescs)
            {
                if (result.ErrorTypeService == MODEN.ErrorTypeService.Ok)
                {
                    UTIER.Result<ParamClauseDesc, UTIER.ErrorModel> resultDAO = clauseDAO.CreateParamClauseDesc(item, coverageId);
                    if (resultDAO is UTIER.ResultError<ParamClauseDesc, UTIER.ErrorModel>)
                    {
                        UTIER.ErrorModel errorModelResult = (resultDAO as UTIER.ResultError<ParamClauseDesc, UTIER.ErrorModel>).Message;
                        result.ErrorDescription.AddRange(errorModelResult.ErrorDescription);
                        result.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                    }
                    else
                    {
                        ParamClauseDesc paramClauseDesc = (resultDAO as UTIER.ResultValue<ParamClauseDesc, UTIER.ErrorModel>).Value;
                        result.ClauseServiceModels.Add(ModelsServicesAssembler.CreateClauseServiceQueryModel(paramClauseDesc));
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Operacion con los deducibles
        /// </summary>
        /// <param name="deductiblesServiceQueryModel">deducibles a modificar</param>
        /// <param name="coverageId">id de cobertura</param>
        /// <returns>deducibles modificados</returns>
        public DeductiblesServiceQueryModel OperationDeductiblesSQM(DeductiblesServiceQueryModel deductiblesServiceQueryModel, int coverageId)
        {
            DeductiblesServiceQueryModel result = new DeductiblesServiceQueryModel()
            {
                ErrorTypeService = MODEN.ErrorTypeService.Ok,
                ErrorDescription = new List<string>(),
                DeductibleServiceQueryModels = new List<DeductibleServiceQueryModel>()
            };
            DeductibleDAO deductibleDAO = new DeductibleDAO();
            List<ParamDeductibleDesc> paramDeductibleDescs = ServicesModelsAssembler.CreateParamDeductibleDescs(deductiblesServiceQueryModel.DeductibleServiceQueryModels);

            // Se recorre listado para insercion uno a uno
            foreach (ParamDeductibleDesc item in paramDeductibleDescs)
            {
                if (result.ErrorTypeService == MODEN.ErrorTypeService.Ok)
                {
                    UTIER.Result<ParamDeductibleDesc, UTIER.ErrorModel> resultDAO = deductibleDAO.CreateParamDeductibleDesc(item, coverageId);
                    if (resultDAO is UTIER.ResultError<ParamDeductibleDesc, UTIER.ErrorModel>)
                    {
                        UTIER.ErrorModel errorModelResult = (resultDAO as UTIER.ResultError<ParamDeductibleDesc, UTIER.ErrorModel>).Message;
                        result.ErrorDescription.AddRange(errorModelResult.ErrorDescription);
                        result.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                    }
                    else
                    {
                        ParamDeductibleDesc paramDeductibleDesc = (resultDAO as UTIER.ResultValue<ParamDeductibleDesc, UTIER.ErrorModel>).Value;
                        result.DeductibleServiceQueryModels.Add(ModelsServicesAssembler.CreateDeductibleServiceQueryModel(paramDeductibleDesc));
                    }
                }
            }

            return result;
        }

        public DetailTypesServiceQueryModel OperationDetailTypesSQM(DetailTypesServiceQueryModel detailTypesServiceQueryModel, int coverageId)
        {
            DetailTypesServiceQueryModel result = new DetailTypesServiceQueryModel()
            {
                ErrorTypeService = MODEN.ErrorTypeService.Ok,
                ErrorDescription = new List<string>(),
                DetailTypeServiceQueryModel = new List<DetailTypeServiceQueryModel>()
            };
            DetailTypeDAO detailTypeDAO = new DetailTypeDAO();
            List<ParamDetailTypeDesc> paramDetailTypeDescs = ServicesModelsAssembler.CreateParamDetailTypeDescs(detailTypesServiceQueryModel.DetailTypeServiceQueryModel);

            // Se recorre listado para insercion uno a uno
            foreach (ParamDetailTypeDesc item in paramDetailTypeDescs)
            {
                if (result.ErrorTypeService == MODEN.ErrorTypeService.Ok)
                {
                    UTIER.Result<ParamDetailTypeDesc, UTIER.ErrorModel> resultDAO = detailTypeDAO.CreateParamDetailTypeDesc(item, coverageId);
                    if (resultDAO is UTIER.ResultError<ParamDetailTypeDesc, UTIER.ErrorModel>)
                    {
                        UTIER.ErrorModel errorModelResult = (resultDAO as UTIER.ResultError<ParamDetailTypeDesc, UTIER.ErrorModel>).Message;
                        result.ErrorDescription.AddRange(errorModelResult.ErrorDescription);
                        result.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                    }
                    else
                    {
                        ParamDetailTypeDesc paramDetailTypeDesc = (resultDAO as UTIER.ResultValue<ParamDetailTypeDesc, UTIER.ErrorModel>).Value;
                        result.DetailTypeServiceQueryModel.Add(ModelsServicesAssembler.CreateDetailTypesServiceQueryModel(paramDetailTypeDesc));
                    }
                }
            }

            return result;
        }


        /// <summary>
        /// Generar archivo excel de objetos del seguro
        /// </summary>
        /// <param name="insuredObjectServiceModel">Listado de objetos del seguro</param>
        /// <param name="fileName">Nombre de archivo</param>
        /// <returns>Archivo de excel</returns>
        public MODPA.ExcelFileServiceModel GenerateFileToInsuredObject(List<InsuredObjectServiceModel> insuredObjectServiceModel, string fileName)
        {
            InsuredObjectDAO insuredObjectDAO = new InsuredObjectDAO();
            MODPA.ExcelFileServiceModel excelFileServiceModel = new MODPA.ExcelFileServiceModel();

            UTIER.Result<string, UTIER.ErrorModel> result = insuredObjectDAO.GenerateFileToInsuredObject(ServicesModelsAssembler.CreateParametrizationInsurancesObjects(insuredObjectServiceModel), fileName);
            if (result is UTIER.ResultError<string, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<string, UTIER.ErrorModel>).Message;
                excelFileServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                excelFileServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<string, UTIER.ErrorModel>)
            {
                excelFileServiceModel.FileData = (result as UTIER.ResultValue<string, UTIER.ErrorModel>).Value;
            }

            return excelFileServiceModel;
        }

        /// <summary>
        /// Obtener lista de objetos del seguro
        /// </summary>
        /// <param name="description">descripcion del objeto de seguro</param>
        /// <returns>Plan de pago MOD-S resultado de consulta de objetos del seguro</returns>
        public InsurancesObjectsServiceModel GetInsuredObjectServiceModelByDescription(string description)
        {
            InsuredObjectBusiness insuredObjectBusiness = new InsuredObjectBusiness();
            InsurancesObjectsServiceModel insurancesObjectsServiceModel = new InsurancesObjectsServiceModel();
            if (insuredObjectBusiness.ValidateLengthDescrition(description))
            {
                InsuredObjectDAO insuredObjectDAO = new InsuredObjectDAO();

                UTIER.Result<List<ParamInsuredObject>, UTIER.ErrorModel> result = insuredObjectDAO.GetInsuredObjectsByDescription(description);
                if (result is UTIER.ResultError<List<ParamInsuredObject>, UTIER.ErrorModel>)
                {
                    UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<List<ParamInsuredObject>, UTIER.ErrorModel>).Message;
                    insurancesObjectsServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                    insurancesObjectsServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                }
                else if (result is UTIER.ResultValue<List<ParamInsuredObject>, UTIER.ErrorModel>)
                {
                    List<ParamInsuredObject> insuredObject = (result as UTIER.ResultValue<List<ParamInsuredObject>, UTIER.ErrorModel>).Value;
                    insurancesObjectsServiceModel.InsuredObjectServiceModel = ModelsServicesAssembler.CreateInsurancesObjectServiceModel(insuredObject);
                }

                return insurancesObjectsServiceModel;
            }
            else
            {
                return insurancesObjectsServiceModel;
            }
        }

        /// <summary>
        /// Obtener lista de objetos del seguro
        /// </summary>
        /// <returns>Plan de pago MOD-S resultado de consulta de objetos del seguro</returns>
        public InsurancesObjectsServiceModel GetInsuredObjectServiceModel()
        {
            InsuredObjectDAO insuredObjectDAO = new InsuredObjectDAO();
            InsurancesObjectsServiceModel insurancesObjectsServiceModel = new InsurancesObjectsServiceModel();
            UTIER.Result<List<ParamInsuredObject>, UTIER.ErrorModel> result = insuredObjectDAO.GetInsuredObjects();
            if (result is UTIER.ResultError<List<ParamInsuredObject>, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<List<ParamInsuredObject>, UTIER.ErrorModel>).Message;
                insurancesObjectsServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                insurancesObjectsServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<List<ParamInsuredObject>, UTIER.ErrorModel>)
            {
                List<ParamInsuredObject> insuredObject = (result as UTIER.ResultValue<List<ParamInsuredObject>, UTIER.ErrorModel>).Value;
                insurancesObjectsServiceModel.InsuredObjectServiceModel = ModelsServicesAssembler.CreateInsurancesObjectServiceModel(insuredObject);
            }

            return insurancesObjectsServiceModel;
        }

        /// <summary>
        /// Validaciones dependencias entidad on¿bjetos de seguro
        /// </summary>
        /// <param name="insuredObjectId">Codigo de objeto de seguro</param>
        /// <returns>1: tiene dependencias, 0: sin dependencias</returns>
        public int ValidateInsuredObject(int insuredObjectId)
        {
            try
            {
                InsuredObjectDAO insuredObjectDAO = new InsuredObjectDAO();
                return insuredObjectDAO.ValidateInsuredObject(insuredObjectId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Resources.Errors.ErrorGetValidation), ex);
            }
        }
        #region Clause
        /// <summary>
        /// Obtener o establece lista de clausulas
        /// </summary>
        /// <returns>Retorna clausulas</returns>
        public MODUD.ClausesServiceModel GetClausesServiceModel()
        {
            ClauseDAO clauseDAO = new ClauseDAO();
            MODUD.ClausesServiceModel clausesServiceModels = new MODUD.ClausesServiceModel();
            UTIER.Result<List<ParamClause>, UTIER.ErrorModel> result = clauseDAO.GetClauseAll();
            UTIER.Result<List<ParamClause>, UTIER.ErrorModel> coverage = clauseDAO.GetClauseForCoverage();
            UTIER.Result<List<ParamClause>, UTIER.ErrorModel> prefix = clauseDAO.GetClauseForCommercialBranch();
            UTIER.Result<List<ParamClause>, UTIER.ErrorModel> riskType = clauseDAO.GetClauseForRiskType();
            UTIER.Result<List<ParamClause>, UTIER.ErrorModel> lineBusiness = clauseDAO.GetClauseForLineBusiness();

            if (result is UTIER.ResultError<List<ParamClause>, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<List<ParamClause>, UTIER.ErrorModel>).Message;
                clausesServiceModels.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                clausesServiceModels.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<List<ParamClause>, UTIER.ErrorModel>)
            {
                List<ParamClause> parametrizationClauses = (result as UTIER.ResultValue<List<ParamClause>, UTIER.ErrorModel>).Value;
                List<ParamClause> parametrizationCoverage = (coverage as UTIER.ResultValue<List<ParamClause>, UTIER.ErrorModel>).Value;
                List<ParamClause> parametrizationPrexis = (prefix as UTIER.ResultValue<List<ParamClause>, UTIER.ErrorModel>).Value;
                List<ParamClause> parametrizationRiskType = (riskType as UTIER.ResultValue<List<ParamClause>, UTIER.ErrorModel>).Value;
                List<ParamClause> parametrizationLineBusiness = (lineBusiness as UTIER.ResultValue<List<ParamClause>, UTIER.ErrorModel>).Value;

                

                foreach (ParamClause clause in parametrizationClauses)
                {
                    if (clause.Clause.ConditionLevel.Id == (int)Enum.Parse(typeof(UnderwritingParamService.Enums.ConditionType), "Coverage"))
                    {
                        List<ParamClause> itemCoverage = parametrizationCoverage.Where(x => x.ClauseLevel.ConditionLevelId == clause.ClauseLevel.ConditionLevelId).ToList();
                        if(itemCoverage.Count > 0)
                        {
                            clause.ParamClauseCoverage = parametrizationCoverage.Where(x => x.ClauseLevel.ClauseId == clause.Clause.Id).FirstOrDefault().ParamClauseCoverage;
                        }
                    }

                    if (clause.Clause.ConditionLevel.Id == (int)Enum.Parse(typeof(UnderwritingParamService.Enums.ConditionType), "Prefix"))
                    {
                        List<ParamClause> itemPrexis = parametrizationPrexis.Where(x => x.ClauseLevel.ConditionLevelId == clause.ClauseLevel.ConditionLevelId).ToList();
                        if (itemPrexis.Count > 0)
                        {
                            clause.ParamClausePrefix = parametrizationPrexis.Where(x => x.ClauseLevel.ClauseId == clause.Clause.Id).FirstOrDefault().ParamClausePrefix;
                        }
                    }

                    if (clause.Clause.ConditionLevel.Id == (int)Enum.Parse(typeof(UnderwritingParamService.Enums.ConditionType), "TypeRisk"))
                    {
                        List<ParamClause> itemLineRiskType = parametrizationRiskType.Where(x => x.ClauseLevel.ConditionLevelId == clause.ClauseLevel.ConditionLevelId).ToList();
                        if (itemLineRiskType.Count > 0)
                            {
                                clause.RiskType = parametrizationRiskType.Where(x => x.ClauseLevel.ClauseId == clause.Clause.Id).FirstOrDefault().RiskType;
                            }
                    }
                    if (clause.Clause.ConditionLevel.Id == (int)Enum.Parse(typeof(UnderwritingParamService.Enums.ConditionType), "TechnicalBranch"))
                    {
                       List<ParamClause> itemLineBusiness = parametrizationLineBusiness.Where(x => x.ClauseLevel.ConditionLevelId == clause.ClauseLevel.ConditionLevelId).ToList();
                        if (itemLineBusiness.Count > 0)
                        {
       
                          clause.ParamClauseLineBusiness = parametrizationLineBusiness.Where(x => x.ClauseLevel.ClauseId == clause.Clause.Id).FirstOrDefault().ParamClauseLineBusiness;
                        }
                            
                    }
                }

                clausesServiceModels.ClauseServiceModels = ModelsServicesAssembler.CreateClauseServiceModelsConsults(parametrizationClauses);
            }

            return clausesServiceModels;
        }

        /// <summary>
        /// Obtiene lista de niveles
        /// </summary>
        /// <returns>Retorna lista de niveles</returns>
        public MODUD.ConditionLevelsServiceModel GetClausesLevelsServiceModel()
        {
            ClauseDAO clauseDAO = new ClauseDAO();
            MODUD.ConditionLevelsServiceModel levelsServiceModels = new MODUD.ConditionLevelsServiceModel();
            UTIER.Result<List<UNDMO.ConditionLevel>, UTIER.ErrorModel> result = clauseDAO.GetParametrizationClausesLevels();
            if (result is UTIER.ResultError<List<UNDMO.ConditionLevel>, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<List<UNDMO.ConditionLevel>, UTIER.ErrorModel>).Message;
                levelsServiceModels.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                levelsServiceModels.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<List<UNDMO.ConditionLevel>, UTIER.ErrorModel>)
            {
                List<UNDMO.ConditionLevel> parametrizationLevels = (result as UTIER.ResultValue<List<UNDMO.ConditionLevel>, UTIER.ErrorModel>).Value;
                levelsServiceModels.ConditionLevelsServiceModels = ModelsServicesAssembler.CreateConditionLevelServiceModels(parametrizationLevels);
                levelsServiceModels.ErrorTypeService = MODEN.ErrorTypeService.Ok;
            }

            return levelsServiceModels;
        }

        /// <summary>
        /// Contiene listado de los ramos comerciales
        /// </summary>
        /// <returns>Retorna listado ramos comerciales</returns>
        public MODUD.PrefixsServiceQueryModel GetCommercialBranch()
        {
            ClauseDAO clauseDAO = new ClauseDAO();
            MODUD.PrefixsServiceQueryModel prefixServiceModels = new MODUD.PrefixsServiceQueryModel();
            UTIER.Result<List<ParamClausePrefix>, UTIER.ErrorModel> result = clauseDAO.GetPrefix();
            if (result is UTIER.ResultError<List<ParamClausePrefix>, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<List<ParamClausePrefix>, UTIER.ErrorModel>).Message;
                prefixServiceModels.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                prefixServiceModels.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<List<ParamClausePrefix>, UTIER.ErrorModel>)
            {
                List<ParamClausePrefix> parametrizationPrefix = (result as UTIER.ResultValue<List<ParamClausePrefix>, UTIER.ErrorModel>).Value;
                prefixServiceModels.PrefixServiceQueryModel = ModelsServicesAssembler.CreateCommercialBranchServiceModels(parametrizationPrefix);
                prefixServiceModels.ErrorTypeService = MODEN.ErrorTypeService.Ok;
            }

            return prefixServiceModels;
        }

        /// <summary>
        /// Contiene listado tipo de riesgo
        /// </summary>
        /// <returns>Retorna listado tipo de riesgo</returns>
        public MODUD.RiskTypesServiceModel GetCoveredRiskType()
        {
            ClauseDAO clauseDAO = new ClauseDAO();
            MODUD.RiskTypesServiceModel riskTypeServiceModels = new MODUD.RiskTypesServiceModel();
            UTIER.Result<List<UNDMO.RiskType>, UTIER.ErrorModel> result = clauseDAO.GetCoveredRiskType();
            if (result is UTIER.ResultError<List<UNDMO.RiskType>, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<List<UNDMO.RiskType>, UTIER.ErrorModel>).Message;
                riskTypeServiceModels.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                riskTypeServiceModels.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<List<UNDMO.RiskType>, UTIER.ErrorModel>)
            {
                List<UNDMO.RiskType> parametrizationRiskType = (result as UTIER.ResultValue<List<UNDMO.RiskType>, UTIER.ErrorModel>).Value;
                riskTypeServiceModels.RiskTypeServiceModels = ModelsServicesAssembler.CreateCoveredRiskTypeServiceModels(parametrizationRiskType);
                riskTypeServiceModels.ErrorTypeService = MODEN.ErrorTypeService.Ok;
            }

            return riskTypeServiceModels;
        }

        /// <summary>
        /// Busqueda textos precatalogados por nombre o id
        /// </summary>
        /// <param name="name">Recibe nombre o Id</param>
        /// <returns>Retorna textos</returns>
        public MODUD.TextsServiceModel GetTextServiceModel(string name)
        {
            ClauseDAO clauseDAO = new ClauseDAO();
            MODUD.TextsServiceModel textsServiceModels = new MODUD.TextsServiceModel();
            UTIER.Result<List<ParamClause>, UTIER.ErrorModel> result = clauseDAO.GetTextsByNameLevelIdParametrization(name);
            if (result is UTIER.ResultError<List<ParamClause>, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<List<ParamClause>, UTIER.ErrorModel>).Message;
                textsServiceModels.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                textsServiceModels.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<List<ParamClause>, UTIER.ErrorModel>)
            {
                List<ParamClause> parametrizationTexts = (result as UTIER.ResultValue<List<ParamClause>, UTIER.ErrorModel>).Value;
                textsServiceModels.TextServiceModels = ModelsServicesAssembler.CreateTextServiceModels(parametrizationTexts);
                textsServiceModels.ErrorTypeService = MODEN.ErrorTypeService.Ok;
            }

            return textsServiceModels;
        }

        /// <summary>
        /// Busqueda de coberturas por nombre o Id
        /// </summary>
        /// <param name="name">Recibe nombre o Id</param>
        /// <returns>Retorna coberturas</returns>
        public MODUD.CoveragesClauseServiceModel GetCoverageByName(string name)
        {
            ClauseDAO clauseDAO = new ClauseDAO();
            MODUD.CoveragesClauseServiceModel clausesCoveragreServiceModels = new MODUD.CoveragesClauseServiceModel();
            UTIER.Result<List<ParamClauseCoverage>, UTIER.ErrorModel> result = clauseDAO.GetCoverageByName(name);
            if (result is UTIER.ResultError<List<ParamClauseCoverage>, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<List<ParamClauseCoverage>, UTIER.ErrorModel>).Message;
                clausesCoveragreServiceModels.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                clausesCoveragreServiceModels.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<List<ParamClauseCoverage>, UTIER.ErrorModel>)
            {
                List<ParamClauseCoverage> parametrizationClauseCoverage = (result as UTIER.ResultValue<List<ParamClauseCoverage>, UTIER.ErrorModel>).Value;
                clausesCoveragreServiceModels.CoverageServiceModels = ModelsServicesAssembler.CreateCoverageServiceModels(parametrizationClauseCoverage);
                clausesCoveragreServiceModels.ErrorTypeService = MODEN.ErrorTypeService.Ok;
            }

            return clausesCoveragreServiceModels;
        }

        /// <summary>
        /// Consulta clausulas por nombre o titulo
        /// </summary>
        /// <param name="name">Recibe nombre o titulo</param>
        /// <returns>Retorna clausesServiceModels</returns>
        public MODUD.ClausesServiceModel GetClauseByNameAndTitle(string name)
        {
            ClauseDAO clauseDAO = new ClauseDAO();
            MODUD.ClausesServiceModel clausesServiceModels = new MODUD.ClausesServiceModel();
            UTIER.Result<List<ParamClause>, UTIER.ErrorModel> result = clauseDAO.GetClauseByNameAndTitle(name);
            UTIER.Result<List<ParamClause>, UTIER.ErrorModel> coverage = clauseDAO.GetClauseForCoverage();
            UTIER.Result<List<ParamClause>, UTIER.ErrorModel> prefix = clauseDAO.GetClauseForCommercialBranch();
            UTIER.Result<List<ParamClause>, UTIER.ErrorModel> riskType = clauseDAO.GetClauseForRiskType();
            UTIER.Result<List<ParamClause>, UTIER.ErrorModel> lineBusiness = clauseDAO.GetClauseForLineBusiness();

            if (result is UTIER.ResultError<List<ParamClause>, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<List<ParamClause>, UTIER.ErrorModel>).Message;
                clausesServiceModels.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                clausesServiceModels.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<List<ParamClause>, UTIER.ErrorModel>)
            {
                List<ParamClause> parametrizationClauses = (result as UTIER.ResultValue<List<ParamClause>, UTIER.ErrorModel>).Value;
                List<ParamClause> parametrizationCoverage = (coverage as UTIER.ResultValue<List<ParamClause>, UTIER.ErrorModel>).Value;
                List<ParamClause> parametrizationPrexis = (prefix as UTIER.ResultValue<List<ParamClause>, UTIER.ErrorModel>).Value;
                List<ParamClause> parametrizationRiskType = (riskType as UTIER.ResultValue<List<ParamClause>, UTIER.ErrorModel>).Value;
                List<ParamClause> parametrizationLineBusiness = (lineBusiness as UTIER.ResultValue<List<ParamClause>, UTIER.ErrorModel>).Value;

                foreach (ParamClause clause in parametrizationClauses)
                {
                    if (clause.Clause.ConditionLevel.Id == (int)Enum.Parse(typeof(UnderwritingParamService.Enums.ConditionType), "Coverage"))
                    {
                        List<ParamClause> itemCoverage = parametrizationCoverage.Where(x => x.ClauseLevel.ConditionLevelId == clause.ClauseLevel.ConditionLevelId).ToList();
                        if(itemCoverage.Count > 0)
                        {
                            clause.ParamClauseCoverage = parametrizationCoverage.Where(x => x.ClauseLevel.ClauseId == clause.Clause.Id).FirstOrDefault().ParamClauseCoverage;
                        }
                    }

                    if (clause.Clause.ConditionLevel.Id == (int)Enum.Parse(typeof(UnderwritingParamService.Enums.ConditionType), "Prefix"))
                    {
                        List<ParamClause> itemPrefix = parametrizationPrexis.Where(x => x.ClauseLevel.ConditionLevelId == clause.ClauseLevel.ConditionLevelId).ToList();
                        if(itemPrefix.Count > 0)
                        {
                            clause.ParamClausePrefix = parametrizationPrexis.Where(x => x.ClauseLevel.ClauseId == clause.Clause.Id).FirstOrDefault().ParamClausePrefix;
                        }
                    }

                    if (clause.Clause.ConditionLevel.Id == (int)Enum.Parse(typeof(UnderwritingParamService.Enums.ConditionType), "TypeRisk"))
                    {
                        List<ParamClause> itemTypeRisk = parametrizationRiskType.Where(x => x.ClauseLevel.ConditionLevelId == clause.ClauseLevel.ConditionLevelId).ToList();
                        if (itemTypeRisk.Count > 0)
                        {
                            clause.RiskType = parametrizationRiskType.Where(x => x.ClauseLevel.ClauseId == clause.Clause.Id).FirstOrDefault().RiskType;
                        }
                    }
                    if(clause.Clause.ConditionLevel.Id == (int)Enum.Parse(typeof(UnderwritingParamService.Enums.ConditionType), "TechnicalBranch"))
                    {
                        List<ParamClause> itemLineBusiness = parametrizationLineBusiness.Where(x => x.ClauseLevel.ConditionLevelId == clause.ClauseLevel.ConditionLevelId).ToList();
                        if(itemLineBusiness.Count > 0)
                        {
                            clause.ParamClauseLineBusiness = parametrizationLineBusiness.Where(x => x.ClauseLevel.ClauseId == clause.Clause.Id).FirstOrDefault().ParamClauseLineBusiness;
                        }
                        
                    }
                }

                clausesServiceModels.ClauseServiceModels = ModelsServicesAssembler.CreateClauseServiceModelsConsults(parametrizationClauses);
                clausesServiceModels.ErrorTypeService = MODEN.ErrorTypeService.Ok;
            }

            return clausesServiceModels;
        }

        /// <summary>
        /// CRUD clausulas
        /// </summary>
        /// <param name="clauseServiceModel">Recibe clausulas</param>
        /// <returns>Retorna resultado</returns>
        public List<MODUD.ClauseServiceModel> ExecuteOperationsClauseServiceModel(List<MODUD.ClauseServiceModel> clauseServiceModel)
        {
            List<MODUD.ClauseServiceModel> result = new List<MODUD.ClauseServiceModel>();
            foreach (var itemSM in clauseServiceModel)
            {
                ParamClause item = ServicesModelsAssembler.CreateParametrizationClause(itemSM);
                item.ClauseLevel = ServicesModelsAssembler.CreateClauseLevel(itemSM.ClauseLevelServiceModel);

                MODUD.ClauseServiceModel itemResult = new MODUD.ClauseServiceModel();
                using (Transaction transaction = new Transaction())
                {
                    switch (itemSM.StatusTypeService)
                    {
                        case MODEN.StatusTypeService.Create:
                            itemResult = this.OperationClauseServiceModel(item, MODEN.StatusTypeService.Create);
                            break;
                        case MODEN.StatusTypeService.Update:
                            itemResult = this.OperationClauseServiceModel(item, MODEN.StatusTypeService.Update);
                            break;
                        case MODEN.StatusTypeService.Delete:
                            itemResult = this.OperationClauseServiceModel(item, MODEN.StatusTypeService.Delete, true);
                            break;
                        default:
                            break;
                    }

                    if (itemResult.ErrorServiceModel.ErrorTypeService == MODEN.ErrorTypeService.TechnicalFault)
                    {
                        transaction.Dispose();
                    }
                    else
                    {
                        transaction.Complete();
                    }
                }

                result.Add(itemResult);
            }

            return result;
        }

        /// <summary>
        /// Metodo que obtiene las operaciones a realizar para las clausulas
        /// </summary>
        /// <param name="parametrizationClause">Recibe parametrizationClause</param>
        /// <param name="statusTypeService">Recibe statusTypeService</param>
        /// <param name="deleteIsPrincipal">Recibe deleteIsPrincipal</param>
        /// <returns>Retorna clauseServiceModelResult</returns>
        public MODUD.ClauseServiceModel OperationClauseServiceModel(ParamClause parametrizationClause, MODEN.StatusTypeService statusTypeService, bool deleteIsPrincipal = false)
        {
            ClauseDAO clauseDAO = new ClauseDAO();
            MODUD.ClauseServiceModel clauseServiceModelResult = new MODUD.ClauseServiceModel();
            UTIER.Result<ParamClause, UTIER.ErrorModel> result;
            switch (statusTypeService)
            {
                case MODEN.StatusTypeService.Create:
                    result = clauseDAO.CreateParametrizationClause(parametrizationClause);
                    break;
                case MODEN.StatusTypeService.Update:
                    result = clauseDAO.UpdateParametrizationClause(parametrizationClause);
                    break;
                case MODEN.StatusTypeService.Delete:
                    // Para el caso de eliminar se utiliza la misma logica
                    result = clauseDAO.DeleteParametrizationClause(parametrizationClause, deleteIsPrincipal);
                    break;
                default:
                    result = clauseDAO.CreateParametrizationClause(parametrizationClause);
                    break;
            }

            if (result is UTIER.ResultError<ParamClause, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<ParamClause, UTIER.ErrorModel>).Message;
                clauseServiceModelResult.ErrorServiceModel = new MODPA.ErrorServiceModel()
                {
                    ErrorDescription = errorModelResult.ErrorDescription,
                    ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType
                };
                clauseServiceModelResult.StatusTypeService = statusTypeService;
            }
            else if (result is UTIER.ResultValue<ParamClause, UTIER.ErrorModel>)
            {
                ParamClause parametrizationClauseResult = (result as UTIER.ResultValue<ParamClause, UTIER.ErrorModel>).Value;
                clauseServiceModelResult = ModelsServicesAssembler.CreateClauseServiceModel(parametrizationClauseResult);

                clauseServiceModelResult.StatusTypeService = statusTypeService;

                //  Upgrade SISE 3G R2 Previsora USP-1008
                //  Se elimina la lógica que genera errores en la actualización de parametrización clausulas, se consulto con Gustavo Cuadra.
                //if (parametrizationClause.ClauseLevel != null)
                //{
                //    MODUD.ClauseServiceModel clauseServiceModelClauseLevel = this.OperationClauseServiceModel(parametrizationClause.ClauseLevel, clauseServiceModelResult.Id, clauseServiceModelResult.StatusTypeService);
                //    if (clauseServiceModelClauseLevel.ErrorServiceModel != null && clauseServiceModelClauseLevel.ErrorServiceModel.ErrorTypeService != MODEN.ErrorTypeService.Ok)
                //        clauseServiceModelResult.ErrorServiceModel = clauseServiceModelClauseLevel.ErrorServiceModel;
                //}
            }
            return clauseServiceModelResult;
        }

        /// <summary>
        /// Metodo que permite crear clauseLevel
        /// </summary>
        /// <param name="levels">Recibe levels</param>
        /// <param name="idClauseServiceModel">Recibe idClauseServiceModel</param>
        /// <returns>Retorna clauseServiceModelResult</returns>
        public MODUD.ClauseServiceModel OperationClauseServiceModel(ParamClauseLevel levels, int idClauseServiceModel, MODEN.StatusTypeService statusTypeService)
        {
            ClauseLevelDAO levelDAO = new ClauseLevelDAO();
            UTIER.Result<ParamClauseLevel, UTIER.ErrorModel> result;
            ParamClause parametrizationClauseDelete = new ParamClause();
            parametrizationClauseDelete.Clause = new UNDMO.Clause()
            {
                Id = idClauseServiceModel
            };
            MODUD.ClauseServiceModel resultDelete = null;
            if (statusTypeService == StatusTypeService.Update)
            {
                resultDelete = this.OperationClauseServiceModel(parametrizationClauseDelete, MODEN.StatusTypeService.Update);
            }
            else
            {
                resultDelete = this.OperationClauseServiceModel(parametrizationClauseDelete, MODEN.StatusTypeService.Delete);
            }
            MODUD.ClauseServiceModel clauseServiceModelResult = new MODUD.ClauseServiceModel();

            if (resultDelete.ErrorServiceModel.ErrorTypeService == MODEN.ErrorTypeService.Ok)
            {
                MODUD.ClauseLevelServiceModel clauseLevelResult = new MODUD.ClauseLevelServiceModel();
                levels.ClauseId = idClauseServiceModel;

                result = levelDAO.CreateLevel(levels);

                if (result is UTIER.ResultError<ParamClauseLevel, UTIER.ErrorModel>)
                {
                    UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<ParamClauseLevel, UTIER.ErrorModel>).Message;
                    clauseServiceModelResult.ErrorServiceModel = new MODPA.ErrorServiceModel()
                    {
                        ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType,
                        ErrorDescription = errorModelResult.ErrorDescription,
                    };                    
                }
                else if (result is UTIER.ResultValue<ParamClauseLevel, UTIER.ErrorModel>)
                {
                    ParamClauseLevel level = (result as UTIER.ResultValue<ParamClauseLevel, UTIER.ErrorModel>).Value;
                    clauseLevelResult = ModelsServicesAssembler.CreateClauseLevelServiceModel(level);
                    clauseServiceModelResult.ClauseLevelServiceModel = clauseLevelResult;
                }
            }
            else
            {
                clauseServiceModelResult.ErrorServiceModel = resultDelete.ErrorServiceModel;
            }

            return clauseServiceModelResult;
        }

        /// <summary>
        /// Generar archivo excel de clausulas
        /// </summary>
        /// <param name="clauses">Listado clausulas</param>
        /// <param name="fileName">Nombre archivo</param>
        /// <returns>Modelo ExcelFileServiceModel</returns>
        public MODPA.ExcelFileServiceModel GenerateFileToClause(List<MODUD.ClauseServiceModel> clauses, string fileName)
        {
            ClauseDAO clauseDAO = new ClauseDAO();
            MODPA.ExcelFileServiceModel excelFileServiceModel = new MODPA.ExcelFileServiceModel();

            UTIER.Result<string, UTIER.ErrorModel> result = clauseDAO.GenerateFileToClause(ServicesModelsAssembler.CreateParametrizationClauses(clauses), fileName);
            if (result is UTIER.ResultError<string, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<string, UTIER.ErrorModel>).Message;
                excelFileServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                excelFileServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<string, UTIER.ErrorModel>)
            {
                excelFileServiceModel.FileData = (result as UTIER.ResultValue<string, UTIER.ErrorModel>).Value;
            }

            return excelFileServiceModel;
        }
        #endregion
        
        #region Metodos_Pago

        public List<MODUD.PaymentMethodServiceModel> ExecuteOperationPaymentMethod(List<MODUD.PaymentMethodServiceModel> listPaymentMethodServiceModel)
        {
            PaymentMethodBusiness paymentPlanBusiness = new PaymentMethodBusiness();
            List<MODUD.PaymentMethodServiceModel> result = new List<MODUD.PaymentMethodServiceModel>();
            foreach (var itemSM in listPaymentMethodServiceModel)
            {
                BmParamPaymentMethod item = ServicesModelsAssembler.CreateParamPaymentMethod(itemSM);
                MODUD.PaymentMethodServiceModel itemResult = new MODUD.PaymentMethodServiceModel();
                using (Transaction transaction = new Transaction())
                {
                    switch (itemSM.StatusTypeService)
                    {
                        case MODEN.StatusTypeService.Create:
                            itemResult = this.OperationPaymentMethodServiceModel(item, MODEN.StatusTypeService.Create);
                            break;
                        case MODEN.StatusTypeService.Update:
                            itemResult = this.OperationPaymentMethodServiceModel(item, MODEN.StatusTypeService.Update);
                            break;
                        case MODEN.StatusTypeService.Delete:
                            itemResult = this.OperationPaymentMethodServiceModel(item, MODEN.StatusTypeService.Delete);
                            break;
                        default:
                            break;
                    }

                    if (itemResult.ErrorServiceModel.ErrorTypeService == MODEN.ErrorTypeService.TechnicalFault)
                    {
                        transaction.Dispose();
                    }
                    else
                    {
                        transaction.Complete();
                    }
                }

                result.Add(itemResult);


            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public MODUD.PaymentMethodsServiceModel GetPaymentMethodByDescription(string description)
        {
            PaymentMethodDAO paymentMethodDAO = new PaymentMethodDAO();
            MODUD.PaymentMethodsServiceModel paymentMethodsServiceModel = new MODUD.PaymentMethodsServiceModel();
            Result<List<BmParamPaymentMethod>, ErrorModel> resultPaymentMethod = paymentMethodDAO.GetParamPaymentMethodByDescription(description);
            if (resultPaymentMethod is ResultError<List<BmParamPaymentMethod>, ErrorModel>)
            {
                ErrorModel errorModelResult = (resultPaymentMethod as ResultError<List<BmParamPaymentMethod>, ErrorModel>).Message;
                paymentMethodsServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                paymentMethodsServiceModel.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
            }
            else
            {
                List<BmParamPaymentMethod> resultValue = (resultPaymentMethod as ResultValue<List<BmParamPaymentMethod>, ErrorModel>).Value;
                paymentMethodsServiceModel = ModelsServicesAssembler.CreatePaymentMethodsServiceModel(resultValue);
            }
            return paymentMethodsServiceModel;
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<MODUD.PaymentMethodTypeServiceQueryModel> GetPaymentMethodType()
        {
            PaymentMethodTypeDAO paymentMethodTypeDAO = new PaymentMethodTypeDAO();
            List<MODUD.PaymentMethodTypeServiceQueryModel> paymentMethodsServiceModel = new List<MODUD.PaymentMethodTypeServiceQueryModel>();
            Result<List<BmParamPaymentMethodType>, ErrorModel> resultGetParamPaymentMethod = paymentMethodTypeDAO.GetParamPaymentMethodTypes();
            if (resultGetParamPaymentMethod is ResultError<List<BmParamPaymentMethodType>, ErrorModel>)
            {
                ErrorModel errorModelResult = (resultGetParamPaymentMethod as ResultError<List<BmParamPaymentMethodType>, ErrorModel>).Message;
            }
            else
            {
                List<BmParamPaymentMethodType> resultValue = (resultGetParamPaymentMethod as ResultValue<List<BmParamPaymentMethodType>, ErrorModel>).Value;
                
                paymentMethodsServiceModel = ModelsServicesAssembler.CreatePaymentMethodTypeServiceQueryModels(resultValue);
            }
            return paymentMethodsServiceModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public MODUD.PaymentMethodsServiceModel GetPaymentMethod ()
        {
            PaymentMethodDAO paymentMethodDAO = new PaymentMethodDAO();
            PaymentMethodTypeDAO paymentMethodTypeDAO = new PaymentMethodTypeDAO();
            List<BmParamPaymentMethodType> listParamPaymentMethodType = (paymentMethodTypeDAO.GetParamPaymentMethodTypes() as ResultValue<List<BmParamPaymentMethodType>, ErrorModel>).Value;
            List<BmParamPaymentMethod> listParamPaymentMethod = new List<BmParamPaymentMethod>();
            MODUD.PaymentMethodsServiceModel paymentMethodsServiceModel = new MODUD.PaymentMethodsServiceModel();
            Result<List<BmParamPaymentMethod>, ErrorModel> resultGetParamPaymentMethod = paymentMethodDAO.GetParamPaymentMethods();
            ResultValue<List<BmParamPaymentMethod>, ErrorModel> resultGetParamPaymentMethodValue = (resultGetParamPaymentMethod as ResultValue<List<BmParamPaymentMethod>, ErrorModel>);
            foreach (BmParamPaymentMethod item in resultGetParamPaymentMethodValue.Value)
            {
                string descriptionType = listParamPaymentMethodType.Where(x => x.Id == item.PaymentMethod.Id).FirstOrDefault().Description;
                BmParamPaymentMethodType paymentMethodType = (BmParamPaymentMethodType.CreateParamPayMethodType(item.PaymentMethod.Id, descriptionType) as ResultValue<BmParamPaymentMethodType, ErrorModel>).Value;
                BmParamPaymentMethod paramPaymentMethod = (BmParamPaymentMethod.CreateParamPaymentMethod(item.Id, item.Description, item.TinyDescription, item.SmallDescription, paymentMethodType) as ResultValue<BmParamPaymentMethod, ErrorModel>).Value;
                listParamPaymentMethod.Add(paramPaymentMethod);
            }
            
            if (resultGetParamPaymentMethod is ResultError<List<BmParamPaymentMethod>, ErrorModel>)
            {
                ErrorModel errorModelResult = (resultGetParamPaymentMethod as ResultError<List<BmParamPaymentMethod>, ErrorModel>).Message;
                paymentMethodsServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                paymentMethodsServiceModel.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
            }
            else
            {
                List<BmParamPaymentMethod> resultValue = (resultGetParamPaymentMethod as ResultValue<List<BmParamPaymentMethod>, ErrorModel>).Value;
                paymentMethodsServiceModel = ModelsServicesAssembler.CreatePaymentMethodsServiceModel(listParamPaymentMethod);
            }
            return paymentMethodsServiceModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paymentMethod"></param>
        /// <param name="statusTypeService"></param>
        /// <returns></returns>
        private MODUD.PaymentMethodServiceModel OperationPaymentMethodServiceModel(BmParamPaymentMethod paymentMethod, MODEN.StatusTypeService statusTypeService)
        {
            PaymentMethodDAO paymentMethodDAO = new PaymentMethodDAO();
            MODUD.PaymentMethodServiceModel paymentMethodServiceModelResult = new MODUD.PaymentMethodServiceModel()
            {
                ErrorServiceModel = new MODPA.ErrorServiceModel()
                {
                    ErrorDescription = new List<string>(),
                    ErrorTypeService = MODEN.ErrorTypeService.Ok
                }
            };
            UTIER.Result<BmParamPaymentMethod, UTIER.ErrorModel> result;
            switch (statusTypeService)
            {
                case MODEN.StatusTypeService.Create:
                    result = paymentMethodDAO.CreateParamPaymentMethod(paymentMethod);
                    break;
                case MODEN.StatusTypeService.Update:
                    result = paymentMethodDAO.UpdateParamPaymentMethod(paymentMethod);
                    break;
                case MODEN.StatusTypeService.Delete:
                    result = paymentMethodDAO.DeleteParamPaymentMethod(paymentMethod);
                    break;
                default:
                    result = paymentMethodDAO.CreateParamPaymentMethod(paymentMethod);
                    break;
            }

            if (result is UTIER.ResultError<BmParamPaymentMethod, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<BmParamPaymentMethod, UTIER.ErrorModel>).Message;
                paymentMethodServiceModelResult.Description = paymentMethod.Description;
                paymentMethodServiceModelResult.ErrorServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                paymentMethodServiceModelResult.ErrorServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                paymentMethodServiceModelResult.StatusTypeService = statusTypeService;
            }
            else if (result is UTIER.ResultValue<BmParamPaymentMethod, UTIER.ErrorModel>)
            {
                BmParamPaymentMethod parametrizationPaymentPlanResult = (result as UTIER.ResultValue<BmParamPaymentMethod, UTIER.ErrorModel>).Value;
                paymentMethodServiceModelResult = ModelsServicesAssembler.CreatePaymentMethodServiceModel(parametrizationPaymentPlanResult);
                paymentMethodServiceModelResult.StatusTypeService = statusTypeService;
                
            }

            return paymentMethodServiceModelResult;
        }

        public MODPA.ExcelFileServiceModel GenerateFileToPaymentMethod(List<MODUD.PaymentMethodServiceModel> paymentMethods, string fileName)
        {
            PaymentMethodDAO paymentMethodDAO = new PaymentMethodDAO();
            MODPA.ExcelFileServiceModel excelFileServiceModel = new MODPA.ExcelFileServiceModel();

            UTIER.Result<string, UTIER.ErrorModel> result = paymentMethodDAO.GenerateFileToPaymentMethod(ServicesModelsAssembler.CreateParamPaymentMethods(paymentMethods), fileName);
            if (result is UTIER.ResultError<string, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<string, UTIER.ErrorModel>).Message;
                excelFileServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                excelFileServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<string, UTIER.ErrorModel>)
            {
                excelFileServiceModel.FileData = (result as UTIER.ResultValue<string, UTIER.ErrorModel>).Value;
            }

            return excelFileServiceModel;
        }

        #endregion Metodos_Pago

        #region Business
        /// <summary>
        /// Obtiene la lista de Ramos comerciales.
        /// </summary>
        /// <returns>Modelo de sevicio para Ramos comerciales.</returns>
        public List<MODUD.PrefixServiceQueryModel> GetPrefixes()
        {
            PrefixDAO prefixDAO = new PrefixDAO();
            List<MODUD.PrefixServiceQueryModel> listPrefixsServiceQueryModel = new List<MODUD.PrefixServiceQueryModel>();

            UTIER.Result<List<ParamPrefix>, UTIER.ErrorModel> resultGetPrefixs = prefixDAO.GetPrefixes();
            if (resultGetPrefixs is UTIER.ResultError<List<ParamPrefix>, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (resultGetPrefixs as UTIER.ResultError<List<ParamPrefix>, UTIER.ErrorModel>).Message;
            }
            else
            {
                List<ParamPrefix> resultValue = (resultGetPrefixs as UTIER.ResultValue<List<ParamPrefix>, UTIER.ErrorModel>).Value;
                foreach (ParamPrefix itemParamPrefix in resultValue)
                {
                    MODUD.PrefixServiceQueryModel prefixServiceQueryModel = ModelsServicesAssembler.MappPrefix(itemParamPrefix);
                    listPrefixsServiceQueryModel.Add(prefixServiceQueryModel);
                }
            }

            return listPrefixsServiceQueryModel;
        }

        /// <summary>
        /// Obtiene la lista de Solicitudes agrupadoras vigentes por ramo comercial.
        /// </summary>
        /// <returns>Modelo de sevicio para Solicitudes agrupadoras vigentes por ramo comercial.</returns>
        public List<MODUD.RequestEndorsementServiceQueryModel> GetCurrentRequestEndorsementByPrefixCode(int prefixCode)
        {
            RequestEndorsementDAO requestEndorsementDAO = new RequestEndorsementDAO();
            List<MODUD.RequestEndorsementServiceQueryModel> listRequestEndorsementServiceQueryModel = new List<MODUD.RequestEndorsementServiceQueryModel>();

            UTIER.Result<List<ParamRequestEndorsement>, UTIER.ErrorModel> resultGetRequestEndorsement = requestEndorsementDAO.GetCurrentRequestEndorsementsByPrefixCode(prefixCode);
            if (resultGetRequestEndorsement is UTIER.ResultError<List<ParamRequestEndorsement>, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (resultGetRequestEndorsement as UTIER.ResultError<List<ParamRequestEndorsement>, UTIER.ErrorModel>).Message;
            }
            else
            {
                List<ParamRequestEndorsement> resultValue = (resultGetRequestEndorsement as UTIER.ResultValue<List<ParamRequestEndorsement>, UTIER.ErrorModel>).Value;
                foreach (ParamRequestEndorsement itemParamRequestEndorsement in resultValue)
                {
                    MODUD.RequestEndorsementServiceQueryModel requestEndorsementServiceQueryModel = ModelsServicesAssembler.MappRequestEndorsement(itemParamRequestEndorsement);
                    listRequestEndorsementServiceQueryModel.Add(requestEndorsementServiceQueryModel);
                }
            }

            return listRequestEndorsementServiceQueryModel;
        }

        /// <summary>
        /// Obtiene la lista de productos vigentes por ramo comercial.
        /// </summary>
        /// <returns>Modelo de sevicio para productos vigentes por ramo comercial.</returns>
        public List<MODUD.ProductServiceQueryModel> GetCurrentProductByPrefixCode(int prefixCode)
        {
            ProductDAO productDAO = new ProductDAO();
            List<MODUD.ProductServiceQueryModel> listProductServiceQueryModel = new List<MODUD.ProductServiceQueryModel>();

            UTIER.Result<List<ParamProduct>, UTIER.ErrorModel> resultGetProduct = productDAO.GetProductsByPrefixCode(prefixCode);
            if (resultGetProduct is UTIER.ResultError<List<ParamProduct>, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (resultGetProduct as UTIER.ResultError<List<ParamProduct>, UTIER.ErrorModel>).Message;
            }
            else
            {
                List<ParamProduct> resultValue = (resultGetProduct as UTIER.ResultValue<List<ParamProduct>, UTIER.ErrorModel>).Value;
                foreach (ParamProduct itemParamProduct in resultValue)
                {
                    if (itemParamProduct.ActiveProduct == true)
                    {
                        MODUD.ProductServiceQueryModel productServiceQueryModel = ModelsServicesAssembler.MappProduct(itemParamProduct);
                        listProductServiceQueryModel.Add(productServiceQueryModel);
                    }

                }
            }

            return listProductServiceQueryModel;
        }

        /// <summary>
        /// Obtiene la lista de coberturas por producto.
        /// </summary>
        /// <returns>Modelo de sevicio para coberturas por producto.</returns>
        public List<MODUD.GroupCoverageServiceQueryModel> GetGroupCoverageByProductCode(int productCode)
        {
            GroupCoverageDAO groupCoverageDAO = new GroupCoverageDAO();
            List<MODUD.GroupCoverageServiceQueryModel> listGroupCoverageServiceQueryModel = new List<MODUD.GroupCoverageServiceQueryModel>();

            UTIER.Result<List<ParamGroupCoverage>, UTIER.ErrorModel> resultGetGroupCoverage = groupCoverageDAO.GetGroupCoverageByProductId(productCode);
            if (resultGetGroupCoverage is UTIER.ResultError<List<ParamGroupCoverage>, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (resultGetGroupCoverage as UTIER.ResultError<List<ParamGroupCoverage>, UTIER.ErrorModel>).Message;
            }
            else
            {
                List<ParamGroupCoverage> resultValue = (resultGetGroupCoverage as UTIER.ResultValue<List<ParamGroupCoverage>, UTIER.ErrorModel>).Value;
                foreach (ParamGroupCoverage itemParamGroupCoverage in resultValue)
                {
                    MODUD.GroupCoverageServiceQueryModel groupCoverageServiceQueryModel = ModelsServicesAssembler.MappGroupCoverage(itemParamGroupCoverage);
                    listGroupCoverageServiceQueryModel.Add(groupCoverageServiceQueryModel);
                }
            }

            return listGroupCoverageServiceQueryModel;
        }

        /// <summary>
        /// Obtiene la lista de asistencias por producto.
        /// </summary>
        /// <returns>Modelo de sevicio para asistencias por producto.</returns>
        public List<MODUD.AssistanceTypeServiceQueryModel> GetAssistanceTypeByProductCode(int productCode)
        {
            AssistanceTypeDAO assistanceTypeDAO = new AssistanceTypeDAO();
            List<MODUD.AssistanceTypeServiceQueryModel> listAssistanceTypeServiceQueryModel = new List<MODUD.AssistanceTypeServiceQueryModel>();

            UTIER.Result<List<ParamAssistanceType>, UTIER.ErrorModel> resultGetAssistanceType = assistanceTypeDAO.GetAssistanceTypeByProductId(productCode);
            if (resultGetAssistanceType is UTIER.ResultError<List<ParamAssistanceType>, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (resultGetAssistanceType as UTIER.ResultError<List<ParamAssistanceType>, UTIER.ErrorModel>).Message;
            }
            else
            {
                List<ParamAssistanceType> resultValue = (resultGetAssistanceType as UTIER.ResultValue<List<ParamAssistanceType>, UTIER.ErrorModel>).Value;
                foreach (ParamAssistanceType itemParamAssistanceType in resultValue)
                {
                    MODUD.AssistanceTypeServiceQueryModel assistanceTypeServiceQueryModel = ModelsServicesAssembler.MappAssistanceType(itemParamAssistanceType);
                    listAssistanceTypeServiceQueryModel.Add(assistanceTypeServiceQueryModel);
                }
            }

            return listAssistanceTypeServiceQueryModel;
        }

        /// <summary>
        /// Obtiene la lista de negocios.
        /// </summary>
        /// <returns>Modelo de sevicio para negocios.</returns>
        public MODUD.BusinessServiceQueryModel GetBusinessConfiguration()
        {
            List<string> listErrors = new List<string>();
            MODUD.BusinessServiceQueryModel result = new MODUD.BusinessServiceQueryModel();
            BusinessDAO businessDAO = new BusinessDAO();
            BusinessConfigurationDAO businessConfigurationDAO = new BusinessConfigurationDAO();
            List<MODUD.BusinessServiceModel> listBusinessServiceModel = new List<MODUD.BusinessServiceModel>();
            UTIER.Result<List<ParamBusiness>, UTIER.ErrorModel> listBusiness = businessDAO.GetBusiness();
            if (listBusiness is UTIER.ResultError<List<ParamBusiness>, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (listBusiness as UTIER.ResultError<List<ParamBusiness>, UTIER.ErrorModel>).Message;
            }
            else
            {
                List<ParamBusiness> resultValue = (listBusiness as UTIER.ResultValue<List<ParamBusiness>, UTIER.ErrorModel>).Value;
                foreach (ParamBusiness itemParamBusiness in resultValue)
                {
                    MODUD.BusinessServiceModel businessServiceModel = new MODUD.BusinessServiceModel();
                    UTIER.Result<List<ParamBusinessConfiguration>, UTIER.ErrorModel> listBusinessConfiguration = businessConfigurationDAO.GetBusinessConfigurationByBusinessCode(itemParamBusiness.BusinessId);
                    if (listBusinessConfiguration is UTIER.ResultError<List<ParamBusinessConfiguration>, UTIER.ErrorModel>)
                    {
                        List<MODUD.BusinessConfigurationServiceModel> listBusinessConfigurationServiceModel = new List<MODUD.BusinessConfigurationServiceModel>();
                        List<ParamBusinessConfiguration> resultBusinessConfigurationValue = new List<ParamBusinessConfiguration>();
                        businessServiceModel.BusinessConfiguration = listBusinessConfigurationServiceModel;
                        businessServiceModel.PrefixCode = ModelsServicesAssembler.MappPrefix(itemParamBusiness.Prefix);
                        businessServiceModel.BusinessId = itemParamBusiness.BusinessId;
                        businessServiceModel.Description = itemParamBusiness.Description;
                        businessServiceModel.IsEnabled = itemParamBusiness.IsEnabled;
                        businessServiceModel.StatusTypeService = MODEN.StatusTypeService.Original;
                        listBusinessServiceModel.Add(businessServiceModel);
                    }
                    else
                    {
                        List<MODUD.BusinessConfigurationServiceModel> listBusinessConfigurationServiceModel = new List<MODUD.BusinessConfigurationServiceModel>();
                        List<ParamBusinessConfiguration> resultBusinessConfigurationValue = (listBusinessConfiguration as UTIER.ResultValue<List<ParamBusinessConfiguration>, UTIER.ErrorModel>).Value;
                        foreach (ParamBusinessConfiguration itemParamBusinessConfiguration in resultBusinessConfigurationValue)
                        {
                            MODUD.BusinessConfigurationServiceModel businessConfigurationServiceModel = ModelsServicesAssembler.MappBusinessConfiguration(itemParamBusinessConfiguration);
                            listBusinessConfigurationServiceModel.Add(businessConfigurationServiceModel);
                        }
                        businessServiceModel.BusinessConfiguration = listBusinessConfigurationServiceModel;
                        businessServiceModel.PrefixCode = ModelsServicesAssembler.MappPrefix(itemParamBusiness.Prefix);
                        businessServiceModel.BusinessId = itemParamBusiness.BusinessId;
                        businessServiceModel.Description = itemParamBusiness.Description;
                        businessServiceModel.IsEnabled = itemParamBusiness.IsEnabled;
                        businessServiceModel.StatusTypeService = MODEN.StatusTypeService.Original;
                        listBusinessServiceModel.Add(businessServiceModel);
                    }
                }
            }
            result.BusinessServiceModel = listBusinessServiceModel;
            result.ErrorTypeService = MODEN.ErrorTypeService.Ok;
            result.ErrorDescription = listErrors;
            return result;
        }

        /// <summary>
        /// Realiza las operaciones CRUD para el negocio y acuerdos de negocio.
        /// </summary>
        /// <param name="listBusiness">lista de negocio y acuerdos de negocio.</param>
        /// <returns>Resumen de las operaciones</returns>
        public UTILMO.ParametrizationResponse<MODUD.BusinessServiceModel> SaveBusiness(MODUD.BusinessServiceQueryModel listBusiness)
        {
            UTILMO.ParametrizationResponse<MODUD.BusinessServiceModel> result = new UTILMO.ParametrizationResponse<MODUD.BusinessServiceModel>();
            List<string> listErrors = new List<string>();
            List<ParamBusinessParamBusinessConfiguration> listParamBusinessParamBusinessConfigurationAdded = new List<ParamBusinessParamBusinessConfiguration>();
            BusinessBusinessConfigurationDAO businessBusinessConfigurationDAO = new BusinessBusinessConfigurationDAO();

            List<ParamBusiness> listParamBusinessAdded = new List<ParamBusiness>();
            List<ParamBusiness> listParamBusinessEdited = new List<ParamBusiness>();
            List<ParamBusiness> listParamBusinessDeleted = new List<ParamBusiness>();
            List<ParamBusinessConfiguration> listParamBusinessConfigurationAdded = new List<ParamBusinessConfiguration>();
            List<ParamBusinessConfiguration> listParamBusinessConfigurationEdited = new List<ParamBusinessConfiguration>();
            List<ParamBusinessConfiguration> listParamBusinessConfigurationDeleted = new List<ParamBusinessConfiguration>();
            BusinessDAO businessDAO = new BusinessDAO();
            BusinessConfigurationDAO businessConfigurationDAO = new BusinessConfigurationDAO();
            List<MODUD.BusinessServiceModel> listBusinessServiceModel = new List<MODUD.BusinessServiceModel>();
            listBusinessServiceModel = listBusiness.BusinessServiceModel;
            foreach (MODUD.BusinessServiceModel itemBusinessServiceModel in listBusinessServiceModel)
            {
                UTIER.Result<ParamBusinessParamBusinessConfiguration, UTIER.ErrorModel> resultParamBusinessParamBusinessConfiguration = ServicesModelsAssembler.MappBusinessBusinessConfiguration(itemBusinessServiceModel);
                UTIER.Result<ParamBusiness, UTIER.ErrorModel> resultParamBusiness = ServicesModelsAssembler.MappBusiness(itemBusinessServiceModel);
                switch (itemBusinessServiceModel.StatusTypeService)
                {
                    case MODEN.StatusTypeService.Create:
                        if (resultParamBusinessParamBusinessConfiguration is UTIER.ResultError<ParamBusinessParamBusinessConfiguration, UTIER.ErrorModel>)
                        {
                            UTIER.ErrorModel errorModelResult = (resultParamBusinessParamBusinessConfiguration as UTIER.ResultError<ParamBusinessParamBusinessConfiguration, UTIER.ErrorModel>).Message;
                            result.ErrorAdded = string.Join(" <br/> ", errorModelResult.ErrorDescription);
                        }
                        else
                        {
                            listParamBusinessParamBusinessConfigurationAdded.Add((resultParamBusinessParamBusinessConfiguration as UTIER.ResultValue<ParamBusinessParamBusinessConfiguration, UTIER.ErrorModel>).Value);
                        }
                        break;
                    case MODEN.StatusTypeService.Update:
                        if (resultParamBusiness is UTIER.ResultError<ParamBusiness, UTIER.ErrorModel>)
                        {
                            UTIER.ErrorModel errorModelResult = (resultParamBusiness as UTIER.ResultError<ParamBusiness, UTIER.ErrorModel>).Message;
                            result.ErrorModify = string.Join(" <br/> ", errorModelResult.ErrorDescription);
                        }
                        else
                        {
                            listParamBusinessEdited.Add((resultParamBusiness as UTIER.ResultValue<ParamBusiness, UTIER.ErrorModel>).Value);
                        }
                        break;
                    case MODEN.StatusTypeService.Delete:
                        if (resultParamBusiness is UTIER.ResultError<ParamBusiness, UTIER.ErrorModel>)
                        {
                            UTIER.ErrorModel errorModelResult = (resultParamBusiness as UTIER.ResultError<ParamBusiness, UTIER.ErrorModel>).Message;
                            result.ErrorDeleted = string.Join(" <br/> ", errorModelResult.ErrorDescription);
                        }
                        else
                        {
                            listParamBusinessDeleted.Add((resultParamBusiness as UTIER.ResultValue<ParamBusiness, UTIER.ErrorModel>).Value);
                        }
                        break;
                    default:
                        break;
                }
                if (itemBusinessServiceModel.StatusTypeService != MODEN.StatusTypeService.Create)
                {
                    if (itemBusinessServiceModel.BusinessConfiguration != null)
                    {
                        foreach (MODUD.BusinessConfigurationServiceModel itemBusinessConfigurationServiceModel in itemBusinessServiceModel.BusinessConfiguration)
                        {
                            UTIER.Result<ParamBusinessConfiguration, UTIER.ErrorModel> resultParamBusinessConfiguration = ServicesModelsAssembler.MappBusinessConfiguration(itemBusinessConfigurationServiceModel);
                            switch (itemBusinessConfigurationServiceModel.StatusTypeService)
                            {
                                case MODEN.StatusTypeService.Create:
                                    if (resultParamBusinessConfiguration is UTIER.ResultError<ParamBusinessConfiguration, UTIER.ErrorModel>)
                                    {
                                        UTIER.ErrorModel errorModelResult = (resultParamBusinessConfiguration as UTIER.ResultError<ParamBusinessConfiguration, UTIER.ErrorModel>).Message;
                                        result.ErrorAdded = string.Join(" <br/> ", errorModelResult.ErrorDescription);
                                    }
                                    else
                                    {
                                        listParamBusinessConfigurationAdded.Add((resultParamBusinessConfiguration as UTIER.ResultValue<ParamBusinessConfiguration, UTIER.ErrorModel>).Value);
                                    }
                                    break;
                                case MODEN.StatusTypeService.Update:
                                    if (resultParamBusinessConfiguration is UTIER.ResultError<ParamBusinessConfiguration, UTIER.ErrorModel>)
                                    {
                                        UTIER.ErrorModel errorModelResult = (resultParamBusinessConfiguration as UTIER.ResultError<ParamBusinessConfiguration, UTIER.ErrorModel>).Message;
                                        result.ErrorModify = string.Join(" <br/> ", errorModelResult.ErrorDescription);
                                    }
                                    else
                                    {
                                        listParamBusinessConfigurationEdited.Add((resultParamBusinessConfiguration as UTIER.ResultValue<ParamBusinessConfiguration, UTIER.ErrorModel>).Value);
                                    }
                                    break;
                                case MODEN.StatusTypeService.Delete:
                                    if (resultParamBusinessConfiguration is UTIER.ResultError<ParamBusinessConfiguration, UTIER.ErrorModel>)
                                    {
                                        UTIER.ErrorModel errorModelResult = (resultParamBusinessConfiguration as UTIER.ResultError<ParamBusinessConfiguration, UTIER.ErrorModel>).Message;
                                        result.ErrorDeleted = string.Join(" <br/> ", errorModelResult.ErrorDescription);
                                    }
                                    else
                                    {
                                        listParamBusinessConfigurationDeleted.Add((resultParamBusinessConfiguration as UTIER.ResultValue<ParamBusinessConfiguration, UTIER.ErrorModel>).Value);
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                    }

                }

            }
            UTILMO.ParametrizationResponse<ParamBusinessParamBusinessConfiguration> resultSaveNewBusiness = businessBusinessConfigurationDAO.SaveNewBusiness(listParamBusinessParamBusinessConfigurationAdded);
            UTILMO.ParametrizationResponse<ParamBusiness> resultSaveBusiness = businessDAO.SaveBusiness(null, listParamBusinessEdited, null);
            UTILMO.ParametrizationResponse<ParamBusinessConfiguration> resultSaveBusinessConfiguration = businessConfigurationDAO.SaveBusinessConfiguration(listParamBusinessConfigurationAdded, listParamBusinessConfigurationEdited, listParamBusinessConfigurationDeleted);
            UTILMO.ParametrizationResponse<ParamBusiness> resultDeleteBusiness = businessDAO.SaveBusiness(null, null, listParamBusinessDeleted);
            if (resultSaveBusiness.TotalAdded == null)
            {
                resultSaveBusiness.TotalAdded = 0;
            }
            if (resultSaveBusinessConfiguration.TotalAdded == null)
            {
                resultSaveBusinessConfiguration.TotalAdded = 0;
            }
            if (resultSaveNewBusiness.TotalAdded == null)
            {
                resultSaveNewBusiness.TotalAdded = 0;
            }
            result.TotalAdded = resultSaveBusiness.TotalAdded + resultSaveBusinessConfiguration.TotalAdded + resultSaveNewBusiness.TotalAdded;
            if (string.IsNullOrEmpty(resultSaveNewBusiness.ErrorAdded))
            {
            }
            else
            {
                result.ErrorAdded = string.Join(" <br/> ", resultSaveNewBusiness.ErrorAdded);
            }
            if (string.IsNullOrEmpty(resultSaveBusiness.ErrorAdded))
            {
            }
            else
            {
                result.ErrorAdded = string.Join(" <br/> ", resultSaveBusiness.ErrorAdded);
            }
            if (string.IsNullOrEmpty(resultSaveBusinessConfiguration.ErrorAdded))
            {
            }
            else
            {
                result.ErrorAdded = string.Join(" <br/> ", resultSaveBusinessConfiguration.ErrorAdded);
            }
            if (resultSaveBusiness.TotalModify == null)
            {
                resultSaveBusiness.TotalModify = 0;
            }
            if (resultSaveBusinessConfiguration.TotalModify == null)
            {
                resultSaveBusinessConfiguration.TotalModify = 0;
            }
            result.TotalModify = resultSaveBusiness.TotalModify + resultSaveBusinessConfiguration.TotalModify;
            if (string.IsNullOrEmpty(resultSaveBusiness.ErrorModify))
            {
            }
            else
            {
                result.ErrorModify = string.Join(" <br/> ", resultSaveBusiness.ErrorModify);
            }
            if (string.IsNullOrEmpty(resultSaveBusinessConfiguration.ErrorModify))
            {
            }
            else
            {
                result.ErrorModify = string.Join(" <br/> ", resultSaveBusinessConfiguration.ErrorModify);
            }
            if (resultDeleteBusiness.TotalDeleted == null)
            {
                resultDeleteBusiness.TotalDeleted = 0;
            }
            if (resultSaveBusinessConfiguration.TotalDeleted == null)
            {
                resultSaveBusinessConfiguration.TotalDeleted = 0;
            }
            result.TotalDeleted = resultDeleteBusiness.TotalDeleted + resultSaveBusinessConfiguration.TotalDeleted;
            if (string.IsNullOrEmpty(resultDeleteBusiness.ErrorDeleted))
            {
            }
            else
            {
                result.ErrorDeleted = string.Join(" <br/> ", resultDeleteBusiness.ErrorDeleted);
            }
            if (string.IsNullOrEmpty(resultSaveBusinessConfiguration.ErrorDeleted))
            {
            }
            else
            {
                result.ErrorDeleted = string.Join(" <br/> ", resultSaveBusinessConfiguration.ErrorDeleted);
            }
            result.ReturnedList = this.GetBusinessConfiguration().BusinessServiceModel.OrderBy(x => x.BusinessId).ToList();
            return result;
        }
        /// <summary>
        /// Generar archivo excel de negocios y acuerdos de negocios
        /// </summary>        
        /// <returns>Archivo de excel</returns>
        public MODPA.ExcelFileServiceModel GenerateFileToBusiness()
        {
                BusinessConfigurationDAO businessConfigurationDAO = new BusinessConfigurationDAO();
                MODPA.ExcelFileServiceModel excelFileServiceModel = new MODPA.ExcelFileServiceModel();
                MODUD.BusinessServiceQueryModel businessServiceQueryModel = new MODUD.BusinessServiceQueryModel();
                businessServiceQueryModel = this.GetBusinessConfiguration();
                UTIER.Result<string, UTIER.ErrorModel> result = businessConfigurationDAO.GenerateFileToBusiness(Resources.Errors.FileBusinessConfigurationName);
                if (result is UTIER.ResultError<string, UTIER.ErrorModel>)
                {
                    UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<string, UTIER.ErrorModel>).Message;
                    excelFileServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                    excelFileServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                }
                else
                {
                    excelFileServiceModel.FileData = (result as UTIER.ResultValue<string, UTIER.ErrorModel>).Value;
                }
                return excelFileServiceModel;       
        }
        #endregion

        #region Quotation Number
        /// <summary>
        /// Consulta los números de cotización por sucursal
        /// </summary>
        /// <param name="branchId">Id de sucrusal</param>
        /// <returns>Lista de números de cotización</returns>
        public QuotationNumbersServiceModel GetParametrizationQuotationNumbersByBranchId(int branchId)
        {
            QuotationNumberDAO quotationNumberDAO = new QuotationNumberDAO();
            QuotationNumbersServiceModel quotationNumbersServiceModel = new QuotationNumbersServiceModel();
            UTIER.Result<List<ParametrizationQuotationNumber>, UTIER.ErrorModel> result = quotationNumberDAO.GetParametrizationQuotationNumbersByBranchId(branchId);
            if (result is UTIER.ResultError<List<ParametrizationQuotationNumber>, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<List<ParametrizationQuotationNumber>, UTIER.ErrorModel>).Message;
                quotationNumbersServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                quotationNumbersServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<List<ParametrizationQuotationNumber>, UTIER.ErrorModel>)
            {
                List<ParametrizationQuotationNumber> parametrizationQuotationNumber = (result as UTIER.ResultValue<List<ParametrizationQuotationNumber>, UTIER.ErrorModel>).Value;
                quotationNumbersServiceModel.QuotationNumberServiceModels = ModelsServicesAssembler.CreateQuotationNumberServiceModel(parametrizationQuotationNumber);
            }

            return quotationNumbersServiceModel;
        }

        /// <summary>
        /// Consulta los números de cotización por sucursal y ramo
        /// </summary>
        /// <param name="branchId">Id de sucrusal</param>
        /// <param name="prefixId">Id de ramo</param>
        /// <returns>Lista de números de cotización</returns>
        public QuotationNumbersServiceModel GetParametrizationQuotationNumbersByBranchIdPrefixId(int branchId, int prefixId)
        {
            QuotationNumberDAO quotationNumberDAO = new QuotationNumberDAO();
            QuotationNumbersServiceModel quotationNumbersServiceModel = new QuotationNumbersServiceModel();
            UTIER.Result<List<ParametrizationQuotationNumber>, UTIER.ErrorModel> result = quotationNumberDAO.GetParametrizationQuotationNumbersByBranchIdPrefixId(branchId, prefixId);
            if (result is UTIER.ResultError<List<ParametrizationQuotationNumber>, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<List<ParametrizationQuotationNumber>, UTIER.ErrorModel>).Message;
                quotationNumbersServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                quotationNumbersServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<List<ParametrizationQuotationNumber>, UTIER.ErrorModel>)
            {
                List<ParametrizationQuotationNumber> parametrizationQuotationNumber = (result as UTIER.ResultValue<List<ParametrizationQuotationNumber>, UTIER.ErrorModel>).Value;
                quotationNumbersServiceModel.QuotationNumberServiceModels = ModelsServicesAssembler.CreateQuotationNumberServiceModel(parametrizationQuotationNumber);
            }

            return quotationNumbersServiceModel;
        }

        /// <summary>
        /// Generar archivo excel de números de cotización
        /// </summary>
        /// <param name="fileName">Nombre de archivo</param>
        /// <returns>Archivo de excel</returns>
        public MODPA.ExcelFileServiceModel GenerateFileToQuotationNumber(string fileName)
        {
            QuotationNumberDAO quotationNumberDAO = new QuotationNumberDAO();
            MODPA.ExcelFileServiceModel excelFileServiceModel = new MODPA.ExcelFileServiceModel();

            UTIER.Result<string, UTIER.ErrorModel> result = quotationNumberDAO.GenerateFileToQuotationNumber(fileName);
            if (result is UTIER.ResultError<string, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<string, UTIER.ErrorModel>).Message;
                excelFileServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                excelFileServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<string, UTIER.ErrorModel>)
            {
                excelFileServiceModel.FileData = (result as UTIER.ResultValue<string, UTIER.ErrorModel>).Value;
            }

            return excelFileServiceModel;
        }

        /// <summary>
        /// Validación de numeración de cotización
        /// </summary>
        /// <param name="branchId">Codigo de sucursal</param>
        /// <param name="prefixId">Codigo de ramo</param>
        /// <returns>valor 1: tiene dependencias, 0: no tiene dependencias</returns> 
        public int ValidateQuotationNumber(int branchId, int prefixId)
        {
            try
            {
                QuotationNumberDAO quotationNumberDAO = new QuotationNumberDAO();
                return quotationNumberDAO.ValidateQuotationNumber(branchId, prefixId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Resources.Errors.ErrorGetValidation), ex);
            }
        }
        #endregion

        #region Policy Number
        /// <summary>
        /// Consulta los números de pólizas por sucursal
        /// </summary>
        /// <param name="branchId">Id de sucrusal</param>
        /// <returns>Lista de números de pólizas</returns>
        public PolicyNumbersServiceModel GetParamPolicyNumbersByBranchId(int branchId)
        {
            PolicyNumberDAO policyNumberDAO = new PolicyNumberDAO();
            PolicyNumbersServiceModel policyNumbersServiceModel = new PolicyNumbersServiceModel();
            UTIER.Result<List<ParamPolicyNumber>, UTIER.ErrorModel> result = policyNumberDAO.GetParamPolicyNumbersByBranchId(branchId);
            if (result is UTIER.ResultError<List<ParamPolicyNumber>, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<List<ParamPolicyNumber>, UTIER.ErrorModel>).Message;
                policyNumbersServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                policyNumbersServiceModel.ErrorDescription =errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<List<ParamPolicyNumber>, UTIER.ErrorModel>)
            {
                List<ParamPolicyNumber> parametrizationPolicyNumber = (result as UTIER.ResultValue<List<ParamPolicyNumber>, UTIER.ErrorModel>).Value;
                policyNumbersServiceModel.PolicyNumberServiceModels = ModelsServicesAssembler.CreatePolicyNumbersServiceModel(parametrizationPolicyNumber);
            }

            return policyNumbersServiceModel;
        }

        /// <summary>
        /// Consulta los números de pólizas por sucursal y ramo
        /// </summary>
        /// <param name="branchId">Id de sucrusal</param>
        /// <param name="prefixId">Id de ramo</param>
        /// <returns>Lista de números de pólizas</returns>
        public PolicyNumbersServiceModel GetParamPolicyNumbersByBranchIdPrefixId(int branchId, int prefixId)
        {
            PolicyNumberDAO policyNumberDAO = new PolicyNumberDAO();
            PolicyNumbersServiceModel policyNumbersServiceModel = new PolicyNumbersServiceModel();
            UTIER.Result<List<ParamPolicyNumber>, UTIER.ErrorModel> result = policyNumberDAO.GetParamPolicyNumbersByBranchIdPrefixId(branchId, prefixId);
            if (result is UTIER.ResultError<List<ParamPolicyNumber>, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<List<ParamPolicyNumber>, UTIER.ErrorModel>).Message;
                policyNumbersServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                policyNumbersServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<List<ParamPolicyNumber>, UTIER.ErrorModel>)
            {
                List<ParamPolicyNumber> parametrizationPolicyNumber = (result as UTIER.ResultValue<List<ParamPolicyNumber>, UTIER.ErrorModel>).Value;
                policyNumbersServiceModel.PolicyNumberServiceModels = ModelsServicesAssembler.CreatePolicyNumbersServiceModel(parametrizationPolicyNumber);
            }

            return policyNumbersServiceModel;
        }

        /// <summary>
        /// Generar archivo excel de números de pólizas
        /// </summary>
        /// <param name="fileName">Nombre de archivo</param>
        /// <returns>Archivo de excel</returns>
        public MODPA.ExcelFileServiceModel GenerateFileToPolicyNumber(string fileName)
        {
            PolicyNumberDAO policyNumberDAO = new PolicyNumberDAO();
            MODPA.ExcelFileServiceModel excelFileServiceModel = new MODPA.ExcelFileServiceModel();

            UTIER.Result<string, UTIER.ErrorModel> result = policyNumberDAO.GenerateFileToPolicyNumber(fileName);
            if (result is UTIER.ResultError<string, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<string, UTIER.ErrorModel>).Message;
                excelFileServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                excelFileServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<string, UTIER.ErrorModel>)
            {
                excelFileServiceModel.FileData = (result as UTIER.ResultValue<string, UTIER.ErrorModel>).Value;
            }

            return excelFileServiceModel;
        }

        /// <summary>
        /// Validación de numeración de pólizas
        /// </summary>
        /// <param name="branchId">Codigo de sucursal</param>
        /// <param name="prefixId">Codigo de ramo</param>
        /// <returns>valor 1: tiene dependencias, 0: no tiene dependencias</returns> 
        public int ValidatePolicyNumber(int branchId, int prefixId)
        {
            try
            {
                PolicyNumberDAO policyNumberDAO = new PolicyNumberDAO();
                return policyNumberDAO.ValidatePolicyNumber(branchId, prefixId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Resources.Errors.ErrorGetValidation), ex);
            }
        }
        #endregion

        #region Parametrización ramo técnico
        public List<ClauseLevelServiceModel> GetClausesByLineBussinessId(int idLineBusiness)
        {
            LineBusinessDAO parametrizationLineBusinessDAO = new LineBusinessDAO();
            List<ParamClauseModel> clausestModelDB = new List<ParamClauseModel>();
            Result<List<ParamClauseModel>, ErrorModel> clausetsDB = parametrizationLineBusinessDAO.GetClausesByLineBussinessId(idLineBusiness);
            List<ClauseLevelServiceModel> clausetsServiceModel = new List<ClauseLevelServiceModel>();

            ClauseLevelServiceModel clausetServiceModelResult = new ClauseLevelServiceModel()
            {
                ErrorServiceModel = new MODPA.ErrorServiceModel()
                {
                    ErrorDescription = new List<string>(),
                    ErrorTypeService = MODEN.ErrorTypeService.Ok
                }
            };

            if (clausetsDB is ResultError<List<ParamClauseModel>, ErrorModel>)
            {
                UTIMO.ErrorModel errorModelResult = (clausetsDB as UTIMO.ResultError<List<ParamClauseModel>, UTIMO.ErrorModel>).Message;
                clausetServiceModelResult.ErrorServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                clausetServiceModelResult.ErrorServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                clausetServiceModelResult.StatusTypeService = StatusTypeService.Error;
                clausetsServiceModel.Add(clausetServiceModelResult);
                return clausetsServiceModel;
            }
            else
            {
                clausestModelDB = (clausetsDB as UTMO.ResultValue<List<ParamClauseModel>, UTMO.ErrorModel>).Value;
            }

            clausetsServiceModel = ModelsServicesAssembler.CreateClauseLevelServiceModels(clausestModelDB);
            return clausetsServiceModel;
        }

        public List<InsuredObjectServiceModel> GetInsuredObjectsByLineBussinessId(int idLineBusiness)
        {
            LineBusinessDAO parametrizationLineBusinessDAO = new LineBusinessDAO();
            List<ParamInsuredObjectModel> insuredObjecstModelDB = new List<ParamInsuredObjectModel>();
            Result<List<ParamInsuredObjectModel>, ErrorModel> insuredObjectsDB = parametrizationLineBusinessDAO.GetInsuredObjectsByLineBussinessId(idLineBusiness);
            List<InsuredObjectServiceModel> insuredObjectsServiceModel = new List<InsuredObjectServiceModel>();

            InsuredObjectServiceModel insuredObjectServiceModelResult = new InsuredObjectServiceModel()
            {
                ErrorServiceModel = new MODPA.ErrorServiceModel()
                {
                    ErrorDescription = new List<string>(),
                    ErrorTypeService = MODEN.ErrorTypeService.Ok
                }
            };

            if (insuredObjectsDB is ResultError<List<ParamInsuredObjectModel>, ErrorModel>)
            {
                UTIMO.ErrorModel errorModelResult = (insuredObjectsDB as UTIMO.ResultError<List<ParamInsuredObjectModel>, UTIMO.ErrorModel>).Message;
                insuredObjectServiceModelResult.ErrorServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                insuredObjectServiceModelResult.ErrorServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                insuredObjectServiceModelResult.StatusTypeService = StatusTypeService.Error;
                insuredObjectsServiceModel.Add(insuredObjectServiceModelResult);
                return insuredObjectsServiceModel;
            }
            else
            {
                insuredObjecstModelDB = (insuredObjectsDB as UTMO.ResultValue<List<ParamInsuredObjectModel>, UTMO.ErrorModel>).Value;
            }

            insuredObjectsServiceModel = ModelsServicesAssembler.CreateInsurancesObjectServiceModel(insuredObjecstModelDB);
            return insuredObjectsServiceModel;
        }

        public List<PerilServiceModel> GetProtectionsByLineBussinessId(int idLineBusiness)
        {
            LineBusinessDAO parametrizationLineBusinessDAO = new LineBusinessDAO();
            List<ParamPerilModel> perilModelDB = new List<ParamPerilModel>();
            List<PerilServiceModel> perilsServiceModel = new List<PerilServiceModel>();
            Result<List<ParamPerilModel>, ErrorModel> perilsDB = parametrizationLineBusinessDAO.GetProtectionsByLineBussinessId(idLineBusiness);

            PerilServiceModel perilServiceModelResult = new PerilServiceModel()
            {
                ErrorServiceModel = new MODPA.ErrorServiceModel()
                {
                    ErrorDescription = new List<string>(),
                    ErrorTypeService = MODEN.ErrorTypeService.Ok
                }
            };

            if (perilsDB is ResultError<List<ParamPerilModel>, ErrorModel>)
            {
                UTIMO.ErrorModel errorModelResult = (perilsDB as UTIMO.ResultError<List<ParamPerilModel>, UTIMO.ErrorModel>).Message;
                perilServiceModelResult.ErrorServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                perilServiceModelResult.ErrorServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                perilServiceModelResult.StatusTypeService = StatusTypeService.Error;
                perilsServiceModel.Add(perilServiceModelResult);
                return perilsServiceModel;
            }
            else
            {
                perilModelDB = (perilsDB as UTMO.ResultValue<List<ParamPerilModel>, UTMO.ErrorModel>).Value;
            }

            perilsServiceModel = ModelsServicesAssembler.CreatePerilsServiceModel(perilModelDB);
            return perilsServiceModel;
        }

        /// <summary>
        /// CRUD de Ramo técnico
        /// </summary>
        /// <param name="businessLineServiceModel">Ramo técnico MOD-S</param>
        /// <returns>Ramo técnico producto de la operacion del CRUD</returns>
        public LineBusinessServiceModel ExecuteOperationsLineBusinessServiceModel(LineBusinessServiceModel businessLineServiceModel)
        {
            ParamLineBusinessModel resultValue;
            List<ParamCoveredRiskType> coveredRiskTypesLst = new List<ParamCoveredRiskType>();
            List<ParamCoveredRiskType> coveredRiskTypes = new List<ParamCoveredRiskType>();
            LineBusinessDAO parametrizationLineBusinessDAO = new LineBusinessDAO();
            
            LineBusinessServiceModel businessLineServiceModelResult = new LineBusinessServiceModel()
            {
                ErrorServiceModel = new MODPA.ErrorServiceModel()
                {
                    ErrorDescription = new List<string>(),
                    ErrorTypeService = MODEN.ErrorTypeService.Ok
                }
            };

            Result<ParamLineBusinessModel, ErrorModel> paramLineBusiness = ModelsServicesAssembler.CreateParamLineBusiness(businessLineServiceModel);

            if (paramLineBusiness is ResultError<ParamLineBusinessModel, ErrorModel>)
            {
                UTIMO.ErrorModel errorModelResult = (paramLineBusiness as UTIMO.ResultError<ParamLineBusinessModel, UTIMO.ErrorModel>).Message;
                businessLineServiceModelResult.ErrorServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                businessLineServiceModelResult.ErrorServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                businessLineServiceModelResult.StatusTypeService = businessLineServiceModel.StatusTypeService;
                return businessLineServiceModelResult;
            }
            else
            {
                resultValue = (paramLineBusiness as UTMO.ResultValue<ParamLineBusinessModel, UTMO.ErrorModel>).Value;
            }


            if (businessLineServiceModel.CoveredRiskTypeServiceModel != null)
            {
                foreach (var item in businessLineServiceModel.CoveredRiskTypeServiceModel)
                {
                    Result<ParamCoveredRiskType, ErrorModel> resultCoveredRiskType = ModelsServicesAssembler.CreateCoveredRiskType(item);
                    if (resultCoveredRiskType is ResultError<ParamCoveredRiskType, ErrorModel>)
                    {
                        UTIMO.ErrorModel errorModelResult = (resultCoveredRiskType as UTIMO.ResultError<ParamCoveredRiskType, UTIMO.ErrorModel>).Message;
                        businessLineServiceModelResult.ErrorServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                        businessLineServiceModelResult.ErrorServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                        businessLineServiceModelResult.StatusTypeService = businessLineServiceModel.StatusTypeService;
                        return businessLineServiceModelResult;
                    }
                    else
                    {
                        ParamCoveredRiskType itemCoveredRiskTypes = (resultCoveredRiskType as UTMO.ResultValue<ParamCoveredRiskType, UTMO.ErrorModel>).Value;
                        coveredRiskTypes.Add(itemCoveredRiskTypes);
                    }
                }
            }

            UTIMO.Result<ParamLineBusinessModel, UTIMO.ErrorModel> result;
            UTIMO.Result<List<ParamCoveredRiskType>, UTIMO.ErrorModel> resultCoveredRiskTypes;

            using (Transaction transaction = new Transaction())
            {
                switch (businessLineServiceModel.StatusTypeService)
                {
                    case MODEN.StatusTypeService.Create:
                        result = parametrizationLineBusinessDAO.CreateLineBussiness(resultValue);

                        if (result is ResultError<ParamLineBusinessModel, ErrorModel>)
                        {
                            UTIMO.ErrorModel errorModelResult = (result as UTIMO.ResultError<ParamLineBusinessModel, UTIMO.ErrorModel>).Message;
                            businessLineServiceModelResult.ErrorServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                            businessLineServiceModelResult.ErrorServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                            businessLineServiceModelResult.StatusTypeService = businessLineServiceModel.StatusTypeService;
                            transaction.Dispose();
                            return businessLineServiceModelResult;
                        }
                        else
                        {
                            resultValue = (result as UTMO.ResultValue<ParamLineBusinessModel, UTMO.ErrorModel>).Value;
                            resultCoveredRiskTypes = ExecuteOperationsCoveredRiskType(resultValue.Id, coveredRiskTypes);

                            if (resultCoveredRiskTypes is ResultError<List<ParamCoveredRiskType>, ErrorModel>)
                            {
                                UTIMO.ErrorModel errorModelResult = (resultCoveredRiskTypes as UTIMO.ResultError<List<ParamCoveredRiskType>, UTIMO.ErrorModel>).Message;
                                businessLineServiceModelResult.ErrorServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                                businessLineServiceModelResult.ErrorServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                                businessLineServiceModelResult.StatusTypeService = businessLineServiceModel.StatusTypeService;
                                transaction.Dispose();
                                return businessLineServiceModelResult;
                            }
                            else
                            {
                                coveredRiskTypesLst = (resultCoveredRiskTypes as UTMO.ResultValue<List<ParamCoveredRiskType>, UTMO.ErrorModel>).Value;
                            }
                        }

                        break;
                    case MODEN.StatusTypeService.Update:
                        result = parametrizationLineBusinessDAO.UpdateLineBussiness(resultValue);

                        if (result is ResultError<ParamLineBusinessModel, ErrorModel>)
                        {
                            UTIMO.ErrorModel errorModelResult = (result as UTIMO.ResultError<ParamLineBusinessModel, UTIMO.ErrorModel>).Message;
                            businessLineServiceModelResult.ErrorServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                            businessLineServiceModelResult.ErrorServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                            businessLineServiceModelResult.StatusTypeService = businessLineServiceModel.StatusTypeService;
                            transaction.Dispose();
                            return businessLineServiceModelResult;
                        }
                        else
                        {
                            resultValue = (result as UTMO.ResultValue<ParamLineBusinessModel, UTMO.ErrorModel>).Value;
                            resultCoveredRiskTypes = ExecuteOperationsCoveredRiskType(resultValue.Id, coveredRiskTypes);

                            if (resultCoveredRiskTypes is ResultError<List<ParamCoveredRiskType>, ErrorModel>)
                            {
                                UTIMO.ErrorModel errorModelResult = (resultCoveredRiskTypes as UTIMO.ResultError<List<ParamCoveredRiskType>, UTIMO.ErrorModel>).Message;
                                businessLineServiceModelResult.ErrorServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                                businessLineServiceModelResult.ErrorServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                                businessLineServiceModelResult.StatusTypeService = businessLineServiceModel.StatusTypeService;
                                transaction.Dispose();
                                return businessLineServiceModelResult;
                            }
                            else
                            {
                                coveredRiskTypesLst = (resultCoveredRiskTypes as UTMO.ResultValue<List<ParamCoveredRiskType>, UTMO.ErrorModel>).Value;
                            }
                        }

                        break;
                    default:
                        result = new UTMO.ResultValue<ParamLineBusinessModel, UTMO.ErrorModel>(resultValue);
                        break;
                }

                if (result is UTIMO.ResultError<ParamLineBusinessModel, UTIMO.ErrorModel>)
                {
                    UTIMO.ErrorModel errorModelResult = (result as UTIMO.ResultError<ParamLineBusinessModel, UTIMO.ErrorModel>).Message;
                    businessLineServiceModelResult.ErrorServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                    businessLineServiceModelResult.ErrorServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                    businessLineServiceModelResult.StatusTypeService = businessLineServiceModel.StatusTypeService;
                }
                else if (result is UTIMO.ResultValue<ParamLineBusinessModel, UTIMO.ErrorModel>)
                {
                    resultValue = (result as UTMO.ResultValue<ParamLineBusinessModel, UTMO.ErrorModel>).Value;
                    businessLineServiceModelResult = ModelsServicesAssembler.CreateLineBusinessServiceModel(resultValue, coveredRiskTypesLst);
                    businessLineServiceModelResult.ErrorServiceModel = new MODPA.ErrorServiceModel() { ErrorTypeService = MODEN.ErrorTypeService.Ok };
                    businessLineServiceModelResult.StatusTypeService = businessLineServiceModel.StatusTypeService;
                }

                if (businessLineServiceModelResult.ErrorServiceModel.ErrorTypeService == MODEN.ErrorTypeService.TechnicalFault)
                {
                    transaction.Dispose();
                }
                else
                {
                    transaction.Complete();
                }
            }
          
            return businessLineServiceModelResult;
        }

        public LineBusinessServiceModel DeleteLineBusiness(LineBusinessServiceModel businessLineSM)
        {
            ParamLineBusinessModel resultValue;
            UTIMO.Result<ParamLineBusinessModel, UTIMO.ErrorModel> result;
            LineBusinessDAO parametrizationLineBusinessDAO = new LineBusinessDAO();

            LineBusinessServiceModel businessLineServiceModelResult = new LineBusinessServiceModel()
            {
                ErrorServiceModel = new MODPA.ErrorServiceModel()
                {
                    ErrorDescription = new List<string>(),
                    ErrorTypeService = MODEN.ErrorTypeService.Ok
                }
            };

            Result<ParamLineBusinessModel, ErrorModel> paramLineBusiness = ModelsServicesAssembler.CreateParamLineBusiness(businessLineSM);

            if (paramLineBusiness is ResultError<ParamLineBusinessModel, ErrorModel>)
            {
                UTIMO.ErrorModel errorModelResult = (paramLineBusiness as UTIMO.ResultError<ParamLineBusinessModel, UTIMO.ErrorModel>).Message;
                businessLineServiceModelResult.ErrorServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                businessLineServiceModelResult.ErrorServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                businessLineServiceModelResult.StatusTypeService = businessLineSM.StatusTypeService;
                return businessLineServiceModelResult;
            }
            else
            {
                resultValue = (paramLineBusiness as UTMO.ResultValue<ParamLineBusinessModel, UTMO.ErrorModel>).Value;
            }

            result = parametrizationLineBusinessDAO.DeleteParamLineBusiness(resultValue);
            
            if (result is UTIMO.ResultError<ParamLineBusinessModel, UTIMO.ErrorModel>)
            {
                UTIMO.ErrorModel errorModelResult = (result as UTIMO.ResultError<ParamLineBusinessModel, UTIMO.ErrorModel>).Message;
                businessLineServiceModelResult.ErrorServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                businessLineServiceModelResult.ErrorServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                businessLineServiceModelResult.StatusTypeService = businessLineSM.StatusTypeService;
            }
            else if (result is UTIMO.ResultValue<ParamLineBusinessModel, UTIMO.ErrorModel>)
            {
                resultValue = (result as UTMO.ResultValue<ParamLineBusinessModel, UTMO.ErrorModel>).Value;
                businessLineServiceModelResult = ModelsServicesAssembler.CreateLineBusinessServiceModel(resultValue, new List<ParamCoveredRiskType>());
                businessLineServiceModelResult.ErrorServiceModel = new MODPA.ErrorServiceModel() { ErrorTypeService = MODEN.ErrorTypeService.Ok };
                businessLineServiceModelResult.StatusTypeService = businessLineSM.StatusTypeService;
            }

            return businessLineServiceModelResult;
        }

        /// <summary>
        /// CRUD de Objeto del seguro
        /// </summary>
        /// <param name="insuredObjectsSM">Objeto del seguro MOD-S</param>
        /// <returns>Lista de Objeto del seguro de la operacion del CRUD</returns>
        public List<InsuredObjectServiceModel> ExecuteOperationsInsuredObject(int idLineBusiness, List<InsuredObjectServiceModel> insuredObjectsSM)
        {
            LineBusinessDAO parametrizationLineBusinessDAO = new LineBusinessDAO();
            List<ParamInsuredObjectModel> insuredObjectsModelDB = new List<ParamInsuredObjectModel>();
            List<ParamInsuredObjectModel> insuredObjectsModelLst = new List<ParamInsuredObjectModel>();
            List<ParamInsuredObjectModel> insuredObjectsDelete = new List<ParamInsuredObjectModel>();
            List<ParamInsuredObjectModel> insuredObjectsCreate = new List<ParamInsuredObjectModel>();
            List<InsuredObjectServiceModel> insuredObjectsServiceModel = new List<InsuredObjectServiceModel>();

            InsuredObjectServiceModel insuredObjectServiceModelResult = new InsuredObjectServiceModel()
            {
                ErrorServiceModel = new MODPA.ErrorServiceModel()
                {
                    ErrorDescription = new List<string>(),
                    ErrorTypeService = MODEN.ErrorTypeService.Ok
                }
            };

            Result<List<ParamInsuredObjectModel>, ErrorModel> insuredObjectsDB = parametrizationLineBusinessDAO.GetInsuredObjectsByLineBussinessId(idLineBusiness);

            if (insuredObjectsDB is ResultError<List<ParamInsuredObjectModel>, ErrorModel>)
            {
                UTIMO.ErrorModel errorModelResult = (insuredObjectsDB as UTIMO.ResultError<List<ParamInsuredObjectModel>, UTIMO.ErrorModel>).Message;
                insuredObjectServiceModelResult.ErrorServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                insuredObjectServiceModelResult.ErrorServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                insuredObjectServiceModelResult.StatusTypeService = StatusTypeService.Error;
                insuredObjectsServiceModel.Add(insuredObjectServiceModelResult);
                return insuredObjectsServiceModel;
            }
            else
            {
                insuredObjectsModelDB = (insuredObjectsDB as UTMO.ResultValue<List<ParamInsuredObjectModel>, UTMO.ErrorModel>).Value;
            }

            foreach (ParamInsuredObjectModel paramInsuredObjectModel in insuredObjectsModelDB)
            {
                InsuredObjectServiceModel insuredObjectItem = insuredObjectsSM.Where(t => t.Id == paramInsuredObjectModel.Id).FirstOrDefault();
                if (insuredObjectItem == null)
                {
                    //Eliminar
                    insuredObjectsDelete.Add(paramInsuredObjectModel);
                }
            }
            
            foreach (InsuredObjectServiceModel insuredObjectItem in insuredObjectsSM)
            {
                ParamInsuredObjectModel paramInsuredObjectModel = insuredObjectsModelDB.Where(t => t.Id == insuredObjectItem.Id).FirstOrDefault();
                if (paramInsuredObjectModel == null)
                {
                    //Crear
                    Result<ParamInsuredObjectModel, ErrorModel> paramInsuredObject = ParamInsuredObjectModel.CreateParamInsuredObjectModel(insuredObjectItem.Id, insuredObjectItem.SmallDescription);

                    if (paramInsuredObject is ResultError<ParamInsuredObjectModel, ErrorModel>)
                    {
                        UTIMO.ErrorModel errorModelResult = (paramInsuredObject as UTIMO.ResultError<ParamInsuredObjectModel, UTIMO.ErrorModel>).Message;
                        insuredObjectServiceModelResult.ErrorServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                        insuredObjectServiceModelResult.ErrorServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                        insuredObjectServiceModelResult.StatusTypeService = StatusTypeService.Error;
                        insuredObjectsServiceModel.Add(insuredObjectServiceModelResult);
                        return insuredObjectsServiceModel;
                    }
                    else
                    {
                        paramInsuredObjectModel = (paramInsuredObject as UTMO.ResultValue<ParamInsuredObjectModel, UTMO.ErrorModel>).Value;
                    }

                    insuredObjectsCreate.Add(paramInsuredObjectModel);
                }
            }

            UTIMO.Result<List<ParamInsuredObjectModel>, UTIMO.ErrorModel> resultInsuredObjecs;

            resultInsuredObjecs = parametrizationLineBusinessDAO.DeleteInsuredObjectsForLineBussiness(idLineBusiness, insuredObjectsDelete);

            if (resultInsuredObjecs is ResultError<List<ParamInsuredObjectModel>, ErrorModel>)
            {
                UTIMO.ErrorModel errorModelResult = (resultInsuredObjecs as UTIMO.ResultError<List<ParamInsuredObjectModel>, UTIMO.ErrorModel>).Message;
                insuredObjectServiceModelResult.ErrorServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                insuredObjectServiceModelResult.ErrorServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                insuredObjectServiceModelResult.StatusTypeService = StatusTypeService.Error;
                insuredObjectsServiceModel.Add(insuredObjectServiceModelResult);
                return insuredObjectsServiceModel;
            }
            else
            {
                insuredObjectsModelLst.AddRange((resultInsuredObjecs as UTMO.ResultValue<List<ParamInsuredObjectModel>, UTMO.ErrorModel>).Value);
            }

            resultInsuredObjecs = parametrizationLineBusinessDAO.CreateInsuredObjectsForLineBusiness(idLineBusiness, insuredObjectsCreate);

            if (resultInsuredObjecs is ResultError<List<ParamInsuredObjectModel>, ErrorModel>)
            {
                UTIMO.ErrorModel errorModelResult = (resultInsuredObjecs as UTIMO.ResultError<List<ParamInsuredObjectModel>, UTIMO.ErrorModel>).Message;
                insuredObjectServiceModelResult.ErrorServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                insuredObjectServiceModelResult.ErrorServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                insuredObjectServiceModelResult.StatusTypeService = StatusTypeService.Error;
                insuredObjectsServiceModel.Add(insuredObjectServiceModelResult);
                return insuredObjectsServiceModel;
            }
            else
            {
                insuredObjectsModelLst.AddRange((resultInsuredObjecs as UTMO.ResultValue<List<ParamInsuredObjectModel>, UTMO.ErrorModel>).Value);
            }

            insuredObjectsServiceModel = ModelsServicesAssembler.CreateInsurancesObjectServiceModel(insuredObjectsModelLst);
            return insuredObjectsServiceModel;
        }

        /// <summary>
        /// CRUD de Amparos
        /// </summary>
        /// <param name="perilsSM">Amparos MOD-S</param>
        /// <returns>Lista de Amparos de la operacion del CRUD</returns>
        public List<PerilServiceModel> ExecuteOperationsPeril(int idLineBusiness, List<PerilServiceModel> perilsSM)
        {
            LineBusinessDAO parametrizationLineBusinessDAO = new LineBusinessDAO();
            List<ParamPerilModel> perilstModelDB = new List<ParamPerilModel>();
            List<ParamPerilModel> perilsModelLst = new List<ParamPerilModel>();
            List<ParamPerilModel> perilsDelete = new List<ParamPerilModel>();
            List<ParamPerilModel> perilsCreate = new List<ParamPerilModel>();
            List<PerilServiceModel> perilsServiceModel = new List<PerilServiceModel>();

            PerilServiceModel perilServiceModelResult = new PerilServiceModel()
            {
                ErrorServiceModel = new MODPA.ErrorServiceModel()
                {
                    ErrorDescription = new List<string>(),
                    ErrorTypeService = MODEN.ErrorTypeService.Ok
                }
            };

            Result<List<ParamPerilModel>, ErrorModel> perilsDB = parametrizationLineBusinessDAO.GetProtectionsByLineBussinessId(idLineBusiness);

            if (perilsDB is ResultError<List<ParamPerilModel>, ErrorModel>)
            {
                UTIMO.ErrorModel errorModelResult = (perilsDB as UTIMO.ResultError<List<ParamPerilModel>, UTIMO.ErrorModel>).Message;
                perilServiceModelResult.ErrorServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                perilServiceModelResult.ErrorServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                perilServiceModelResult.StatusTypeService = StatusTypeService.Error;
                perilsServiceModel.Add(perilServiceModelResult);
                return perilsServiceModel;
            }
            else
            {
                perilstModelDB = (perilsDB as UTMO.ResultValue<List<ParamPerilModel>, UTMO.ErrorModel>).Value;
            }

            foreach (ParamPerilModel paramPerilModel in perilstModelDB)
            {
                PerilServiceModel perilItem = perilsSM.Where(t => t.Id == paramPerilModel.Id).FirstOrDefault();
                if (perilItem == null)
                {
                    //Eliminar
                    perilsDelete.Add(paramPerilModel);
                }
            }

            foreach (PerilServiceModel perilItem in perilsSM)
            {
                ParamPerilModel paramPerilModel = perilstModelDB.Where(t => t.Id == perilItem.Id).FirstOrDefault();
                if (paramPerilModel == null)
                {
                    //Crear
                    Result<ParamPerilModel, ErrorModel> paramInsuredObject = ParamPerilModel.CreateParamPerilModel(perilItem.Id, perilItem.SmallDescription);

                    if (paramInsuredObject is ResultError<ParamPerilModel, ErrorModel>)
                    {
                        UTIMO.ErrorModel errorModelResult = (paramInsuredObject as UTIMO.ResultError<ParamPerilModel, UTIMO.ErrorModel>).Message;
                        perilServiceModelResult.ErrorServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                        perilServiceModelResult.ErrorServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                        perilServiceModelResult.StatusTypeService = StatusTypeService.Error;
                        perilsServiceModel.Add(perilServiceModelResult);
                        return perilsServiceModel;
                    }
                    else
                    {
                        paramPerilModel = (paramInsuredObject as UTMO.ResultValue<ParamPerilModel, UTMO.ErrorModel>).Value;
                    }

                    perilsCreate.Add(paramPerilModel);
                }
            }

            UTIMO.Result<List<ParamPerilModel>, UTIMO.ErrorModel> resultPerils;

            resultPerils = parametrizationLineBusinessDAO.DeleteProtectionsForLineBussiness(idLineBusiness, perilsDelete);

            if (resultPerils is ResultError<List<ParamPerilModel>, ErrorModel>)
            {
                UTIMO.ErrorModel errorModelResult = (resultPerils as UTIMO.ResultError<List<ParamPerilModel>, UTIMO.ErrorModel>).Message;
                perilServiceModelResult.ErrorServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                perilServiceModelResult.ErrorServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                perilServiceModelResult.StatusTypeService = StatusTypeService.Error;
                perilsServiceModel.Add(perilServiceModelResult);
                return perilsServiceModel;
            }
            else
            {
                perilsModelLst.AddRange((resultPerils as UTMO.ResultValue<List<ParamPerilModel>, UTMO.ErrorModel>).Value);
            }

            resultPerils = parametrizationLineBusinessDAO.CreateProtectionsForLineBusiness(idLineBusiness, perilsCreate);

            if (resultPerils is ResultError<List<ParamPerilModel>, ErrorModel>)
            {
                UTIMO.ErrorModel errorModelResult = (resultPerils as UTIMO.ResultError<List<ParamPerilModel>, UTIMO.ErrorModel>).Message;
                perilServiceModelResult.ErrorServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                perilServiceModelResult.ErrorServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                perilServiceModelResult.StatusTypeService = StatusTypeService.Error;
                perilsServiceModel.Add(perilServiceModelResult);
                return perilsServiceModel;
            }
            else
            {
                perilsModelLst.AddRange((resultPerils as UTMO.ResultValue<List<ParamPerilModel>, UTMO.ErrorModel>).Value);
            }

            perilsServiceModel = ModelsServicesAssembler.CreatePerilsServiceModel(perilsModelLst);
            return perilsServiceModel;
        }

        /// <summary>
        /// CRUD de Clausula
        /// </summary>
        /// <param name="clausesSM">Clausula MOD-S</param>
        /// <returns>Lista de Clausula de la operacion del CRUD</returns>
        public List<ClauseLevelServiceModel> ExecuteOperationsClause(int idLineBusiness, List<ClauseLevelServiceModel> clausesSM)
        {
            LineBusinessDAO parametrizationLineBusinessDAO = new LineBusinessDAO();
            List<ParamClauseModel> clausestModelDB = new List<ParamClauseModel>();
            List<ParamClauseModel> clausesModelLst = new List<ParamClauseModel>();
            List<ParamClauseModel> clausesDelete = new List<ParamClauseModel>();
            List<ParamClauseModel> clausesCreate = new List<ParamClauseModel>();
            List<ClauseLevelServiceModel> clausesServiceModel = new List<ClauseLevelServiceModel>();

            ClauseLevelServiceModel clauseServiceModelResult = new ClauseLevelServiceModel()
            {
                ErrorServiceModel = new MODPA.ErrorServiceModel()
                {
                    ErrorDescription = new List<string>(),
                    ErrorTypeService = MODEN.ErrorTypeService.Ok
                }
            };

            Result<List<ParamClauseModel>, ErrorModel> clausesDB = parametrizationLineBusinessDAO.GetClausesByLineBussinessId(idLineBusiness);

            if (clausesDB is ResultError<List<ParamClauseModel>, ErrorModel>)
            {
                UTIMO.ErrorModel errorModelResult = (clausesDB as UTIMO.ResultError<List<ParamClauseModel>, UTIMO.ErrorModel>).Message;
                clauseServiceModelResult.ErrorServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                clauseServiceModelResult.ErrorServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                clauseServiceModelResult.StatusTypeService = StatusTypeService.Error;
                clausesServiceModel.Add(clauseServiceModelResult);
                return clausesServiceModel;
            }
            else
            {
                clausestModelDB = (clausesDB as UTMO.ResultValue<List<ParamClauseModel>, UTMO.ErrorModel>).Value;
            }

            foreach (ParamClauseModel paramClauseModel in clausestModelDB)
            {
                ClauseLevelServiceModel clauseItem = clausesSM.Where(t => t.ClauseId == paramClauseModel.Id).FirstOrDefault();
                if (clauseItem == null)
                {
                    //Eliminar
                    clausesDelete.Add(paramClauseModel);
                }
            }

            foreach (ClauseLevelServiceModel clauseItem in clausesSM)
            {
                ParamClauseModel paramClauseModel = clausestModelDB.Where(t => t.Id == clauseItem.ClauseId).FirstOrDefault();
                if (paramClauseModel == null)
                {
                    //Crear
                    Result<ParamClauseModel, ErrorModel> paramInsuredObject = ParamClauseModel.CreateParamClauseModel(clauseItem.ClauseId, string.Empty, clauseItem.IsMandatory);

                    if (paramInsuredObject is ResultError<ParamClauseModel, ErrorModel>)
                    {
                        UTIMO.ErrorModel errorModelResult = (paramInsuredObject as UTIMO.ResultError<ParamClauseModel, UTIMO.ErrorModel>).Message;
                        clauseServiceModelResult.ErrorServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                        clauseServiceModelResult.ErrorServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                        clauseServiceModelResult.StatusTypeService = StatusTypeService.Error;
                        clausesServiceModel.Add(clauseServiceModelResult);
                        return clausesServiceModel;
                    }
                    else
                    {
                        paramClauseModel = (paramInsuredObject as UTMO.ResultValue<ParamClauseModel, UTMO.ErrorModel>).Value;
                    }

                    clausesCreate.Add(paramClauseModel);
                }
            }

            UTIMO.Result<List<ParamClauseModel>, UTIMO.ErrorModel> resultclauses;

            resultclauses = parametrizationLineBusinessDAO.DeleteClausesForLineBussiness(idLineBusiness, clausesDelete);

            if (resultclauses is ResultError<List<ParamClauseModel>, ErrorModel>)
            {
                UTIMO.ErrorModel errorModelResult = (resultclauses as UTIMO.ResultError<List<ParamClauseModel>, UTIMO.ErrorModel>).Message;
                clauseServiceModelResult.ErrorServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                clauseServiceModelResult.ErrorServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                clauseServiceModelResult.StatusTypeService = StatusTypeService.Error;
                clausesServiceModel.Add(clauseServiceModelResult);
                return clausesServiceModel;
            }
            else
            {
                clausesModelLst.AddRange((resultclauses as UTMO.ResultValue<List<ParamClauseModel>, UTMO.ErrorModel>).Value);
            }

            resultclauses = parametrizationLineBusinessDAO.CreateClausesForLineBusiness(idLineBusiness, clausesCreate);

            if (resultclauses is ResultError<List<ParamClauseModel>, ErrorModel>)
            {
                UTIMO.ErrorModel errorModelResult = (resultclauses as UTIMO.ResultError<List<ParamClauseModel>, UTIMO.ErrorModel>).Message;
                clauseServiceModelResult.ErrorServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                clauseServiceModelResult.ErrorServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                clauseServiceModelResult.StatusTypeService = StatusTypeService.Error;
                clausesServiceModel.Add(clauseServiceModelResult);
                return clausesServiceModel;
            }
            else
            {
                clausesModelLst.AddRange((resultclauses as UTMO.ResultValue<List<ParamClauseModel>, UTMO.ErrorModel>).Value);
            }

            clausesServiceModel = ModelsServicesAssembler.CreateClauseLevelServiceModels(clausesModelLst);
            return clausesServiceModel;
        }

        /// <summary>
        /// CRUD de Amparos
        /// </summary>
        /// <param name="coveredRiskTypesSM">Amparos MOD-S</param>
        /// <returns>Lista de Amparos de la operacion del CRUD</returns>
        public Result<List<ParamCoveredRiskType>, ErrorModel> ExecuteOperationsCoveredRiskType(int idLineBusiness, List<ParamCoveredRiskType> coveredRiskTypesSM)
        {
            LineBusinessDAO parametrizationLineBusinessDAO = new LineBusinessDAO();
            List<ParamCoveredRiskType> coveredRiskTypestModelDB = new List<ParamCoveredRiskType>();
            List<ParamCoveredRiskType> coveredRiskTypesModelLst = new List<ParamCoveredRiskType>();
            List<ParamCoveredRiskType> coveredRiskTypesDelete = new List<ParamCoveredRiskType>();
            List<ParamCoveredRiskType> coveredRiskTypesCreate = new List<ParamCoveredRiskType>();
            
            Result<List<ParamCoveredRiskType>, ErrorModel> coveredRiskTypesDB = parametrizationLineBusinessDAO.GetCoveredRisktypesByLineBussinessId(idLineBusiness);

            if (coveredRiskTypesDB is ResultError<List<ParamCoveredRiskType>, ErrorModel>)
            {
                return coveredRiskTypesDB;
            }
            else
            {
                coveredRiskTypestModelDB = (coveredRiskTypesDB as UTMO.ResultValue<List<ParamCoveredRiskType>, UTMO.ErrorModel>).Value;
            }

            foreach (ParamCoveredRiskType paramCoveredRiskTypeModel in coveredRiskTypestModelDB)
            {
                ParamCoveredRiskType coveredRiskTypeItem = coveredRiskTypesSM.Where(t => t.Id == paramCoveredRiskTypeModel.Id).FirstOrDefault();
                if (coveredRiskTypeItem == null)
                {
                    //Eliminar
                    coveredRiskTypesDelete.Add(paramCoveredRiskTypeModel);
                }
            }

            foreach (ParamCoveredRiskType coveredRiskTypeItem in coveredRiskTypesSM)
            {
                ParamCoveredRiskType paramCoveredRiskTypeModel = coveredRiskTypestModelDB.Where(t => t.Id == coveredRiskTypeItem.Id).FirstOrDefault();
                if (paramCoveredRiskTypeModel == null)
                {
                    //Crear
                    coveredRiskTypesCreate.Add(coveredRiskTypeItem);
                }
            }

            UTIMO.Result<List<ParamCoveredRiskType>, UTIMO.ErrorModel> resultCoveredRiskTypes;

            resultCoveredRiskTypes = parametrizationLineBusinessDAO.DeleteCoveredRisktypesForLineBussiness(idLineBusiness, coveredRiskTypesDelete);

            if (resultCoveredRiskTypes is ResultError<List<ParamCoveredRiskType>, ErrorModel>)
            {
                return resultCoveredRiskTypes;
            }
            else
            {
                coveredRiskTypesModelLst.AddRange((resultCoveredRiskTypes as UTMO.ResultValue<List<ParamCoveredRiskType>, UTMO.ErrorModel>).Value);
            }

            resultCoveredRiskTypes = parametrizationLineBusinessDAO.CreateCoveredRisktypesForLineBussiness(idLineBusiness, coveredRiskTypesCreate);

            if (resultCoveredRiskTypes is ResultError<List<ParamCoveredRiskType>, ErrorModel>)
            {
                return resultCoveredRiskTypes;
            }
            else
            {
                coveredRiskTypesModelLst.AddRange((resultCoveredRiskTypes as UTMO.ResultValue<List<ParamCoveredRiskType>, UTMO.ErrorModel>).Value);
            }

            return new ResultValue<List<ParamCoveredRiskType>, ErrorModel>(coveredRiskTypesModelLst);
        }

        /// <summary>
        /// Obtener ramo técnico por Id
        /// </summary>
        /// <param name="lineBusinessId">Id de ramo técnico</param>
        /// <returns>Ramo técnico</returns>
        public LineBusinessServiceModel GetBusinesLinesServiceModelByLineBusinessId(int lineBusinessId)
        {
            LineBusinessDAO parametrizationLineBusinessDAO = new LineBusinessDAO();
            LineBusinessServiceModel businessLineServiceModel = new LineBusinessServiceModel();
            UTIMO.Result<ParamLineBusiness, UTIMO.ErrorModel> result = parametrizationLineBusinessDAO.GetParametrizationLineBusinessByLineBusinessId(lineBusinessId);
            if (result is UTIMO.ResultError<ParamLineBusiness, UTIMO.ErrorModel>)
            {
                UTIMO.ErrorModel errorModelResult = (result as UTIMO.ResultError<ParamLineBusiness, UTIMO.ErrorModel>).Message;
                businessLineServiceModel.ErrorServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                businessLineServiceModel.ErrorServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIMO.ResultValue<ParamLineBusiness, UTIMO.ErrorModel>)
            {
                ParamLineBusiness paramLineBusiness = (result as UTIMO.ResultValue<ParamLineBusiness, UTIMO.ErrorModel>).Value;
                businessLineServiceModel = ModelsServicesAssembler.CreateLineBusinessServiceModel(paramLineBusiness);
            }

            return businessLineServiceModel;
        }


        /// <summary>
        /// Obtiene la lista de ramos técnicos filtrados por descripción o Id
        /// </summary>
        /// <param name="description">Descripción de ramo técnico</param>
        /// <param name="id">Id de ramo técnico</param>
        /// <returns>Bool de respuesta</returns>
        public bool? GetLineBusinessByDescriptionById(string description, int id)
        {
            LineBusinessDAO parametrizationLineBusinessDAO = new LineBusinessDAO();
            UTIMO.Result<bool?, UTIMO.ErrorModel> result = parametrizationLineBusinessDAO.GetLineBusinessByDescriptionById(description, id);
            bool? respuesta = null;

            if (result is UTIMO.ResultValue<bool?, UTIMO.ErrorModel>)
            {
                respuesta = (result as UTIMO.ResultValue<bool?, UTIMO.ErrorModel>).Value;
            }

            return respuesta;
        }

        /// <summary>
        /// Obtiene la lista de ramos técnicos filtrados por descripción o Id
        /// </summary>
        /// <param name="description">Descripción de ramo técnico</param>
        /// <param name="id">Id de ramo técnico</param>
        /// <returns>Lista de  de ramos técnicos</returns>
        public List<LineBusinessServiceModel> GetLineBusinessServiceModelByDescriptionById(string description, int id)
        {
            LineBusinessDAO parametrizationLineBusinessDAO = new LineBusinessDAO();
            List<LineBusinessServiceModel> businessLineServiceModels = new List<LineBusinessServiceModel>();
            UTIMO.Result<List<ParamLineBusiness>, UTIMO.ErrorModel> result = parametrizationLineBusinessDAO.GetParamLineBusinessByDescriptionById(description, id);
            if (result is UTIMO.ResultError<List<ParamLineBusiness>, UTIMO.ErrorModel>)
            {
                UTIMO.ErrorModel errorModelResult = (result as UTIMO.ResultError<List<ParamLineBusiness>, UTIMO.ErrorModel>).Message;
                businessLineServiceModels.Add(new LineBusinessServiceModel()
                {
                    ErrorServiceModel = new MODPA.ErrorServiceModel()
                    {
                        ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType,
                        ErrorDescription = errorModelResult.ErrorDescription
                    }
                });
            }
            else if (result is UTIMO.ResultValue<List<ParamLineBusiness>, UTIMO.ErrorModel>)
            {
                List<ParamLineBusiness> paramLineBusinesss = (result as UTIMO.ResultValue<List<ParamLineBusiness>, UTIMO.ErrorModel>).Value;
                businessLineServiceModels = ModelsServicesAssembler.CreateLineBusinessServiceModels(paramLineBusinesss);
                businessLineServiceModels.ForEach(x => x.StatusTypeService = MODEN.StatusTypeService.Original);
                businessLineServiceModels.ForEach(x => x.Clauses.ForEach(y => y.StatusTypeService = MODEN.StatusTypeService.Original));
                businessLineServiceModels.ForEach(x => x.InsuredObjects.ForEach(y => y.StatusTypeService = MODEN.StatusTypeService.Original));
                businessLineServiceModels.ForEach(x => x.Perils.ForEach(y => y.StatusTypeService = MODEN.StatusTypeService.Original));
            }

            return businessLineServiceModels;
        }

        /// <summary>
        /// Obtiene la lista de ramos técnicos filtrados por descripción o tipo de riesgo cubierto
        /// </summary>
        /// <param name="description">Descripción de ramo técnico</param>
        /// <param name="coveredRiskType">Tipo de riesgo cubierto</param>
        /// <returns>Lista de  de ramos técnicos</returns>
        public List<LineBusinessServiceModel> GetLineBusinessServiceModelByAdvancedSearch(string description, int coveredRiskType)
        {
            LineBusinessDAO parametrizationLineBusinessDAO = new LineBusinessDAO();
            List<LineBusinessServiceModel> businessLineServiceModels = new List<LineBusinessServiceModel>();
            UTIMO.Result<List<ParamLineBusiness>, UTIMO.ErrorModel> result = parametrizationLineBusinessDAO.GetParamLineBusinessByAdvancedSearch(description, coveredRiskType);
            if (result is UTIMO.ResultError<List<ParamLineBusiness>, UTIMO.ErrorModel>)
            {
                UTIMO.ErrorModel errorModelResult = (result as UTIMO.ResultError<List<ParamLineBusiness>, UTIMO.ErrorModel>).Message;
                businessLineServiceModels.Add(new LineBusinessServiceModel()
                {
                    ErrorServiceModel = new MODPA.ErrorServiceModel()
                    {
                        ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType,
                        ErrorDescription = errorModelResult.ErrorDescription
                    }
                });
            }
            else if (result is UTIMO.ResultValue<List<ParamLineBusiness>, UTIMO.ErrorModel>)
            {
                List<ParamLineBusiness> paramLineBusinesss = (result as UTIMO.ResultValue<List<ParamLineBusiness>, UTIMO.ErrorModel>).Value;
                businessLineServiceModels = ModelsServicesAssembler.CreateLineBusinessServiceModels(paramLineBusinesss);
            }

            return businessLineServiceModels;
        }

        /// <summary>
        /// Genera el reporte de ramos técnicos
        /// </summary>
        /// <param name="fileName">Nombre de archivo</param>
        /// <returns>Ruta del archivo</returns>
        public MODPA.ExcelFileServiceModel GenerateLineBusinessServiceModel(string fileName)
        {
            LineBusinessDAO parametrizationLineBusinessDAO = new LineBusinessDAO();
            MODPA.ExcelFileServiceModel excelFileServiceModel = new MODPA.ExcelFileServiceModel();
            UTIMO.Result<string, UTIMO.ErrorModel> result = parametrizationLineBusinessDAO.GenerateParamLineBusiness(fileName);
            if (result is UTIMO.ResultError<string, UTIMO.ErrorModel>)
            {
                UTIMO.ErrorModel errorModelResult = (result as UTIMO.ResultError<string, UTIMO.ErrorModel>).Message;
                excelFileServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                excelFileServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIMO.ResultValue<string, UTIMO.ErrorModel>)
            {
                excelFileServiceModel.FileData = (result as UTIMO.ResultValue<string, UTIMO.ErrorModel>).Value;
            }

            return excelFileServiceModel;
        }

        /// <summary>
        /// Listado de amparos
        /// </summary>
        /// <returns>Amparos - MOD-S</returns>
        public MODUD.PerilsServiceQueryModel GetPerilsServiceQueryModel()
        {
            MODUD.PerilsServiceQueryModel perilsServiceQueryModel = new MODUD.PerilsServiceQueryModel();
            PerilDAO perilDAO = new PerilDAO();

            UTIMO.Result<List<ParamPeril>, UTIMO.ErrorModel> result = perilDAO.GetPerils();
            if (result is UTIMO.ResultError<List<ParamPeril>, UTIMO.ErrorModel>)
            {
                UTIMO.ErrorModel errorModelResult = (result as UTIMO.ResultError<List<ParamPeril>, UTIMO.ErrorModel>).Message;
                perilsServiceQueryModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                perilsServiceQueryModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIMO.ResultValue<List<ParamPeril>, UTIMO.ErrorModel>)
            {
                List<ParamPeril> perils = (result as UTIMO.ResultValue<List<ParamPeril>, UTIMO.ErrorModel>).Value;
                perilsServiceQueryModel.PerilServiceQueryModels = ModelsServicesAssembler.CreatePerilsServiceQueryModels(perils);
            }

            return perilsServiceQueryModel;
        }

        /// <summary>
        /// Listado de objetos del seguro
        /// </summary>
        /// <returns>Objetos del seguro => MOD-S</returns>
        public MODUD.InsuredObjectsServiceQueryModel GetInsuredObjectsServiceQueryModels()
        {
            MODUD.InsuredObjectsServiceQueryModel insuredObjectsServiceQueryModels = new MODUD.InsuredObjectsServiceQueryModel();
            InsuredObjectDAO insuredObjectDAO = new InsuredObjectDAO();

            UTIMO.Result<List<ParamInsuredObjectDesc>, UTIMO.ErrorModel> result = insuredObjectDAO.GetParamInsuredObjectDescs();
            if (result is UTIMO.ResultError<List<ParamInsuredObjectDesc>, UTIMO.ErrorModel>)
            {
                UTIMO.ErrorModel errorModelResult = (result as UTIMO.ResultError<List<ParamInsuredObjectDesc>, UTIMO.ErrorModel>).Message;
                insuredObjectsServiceQueryModels.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                insuredObjectsServiceQueryModels.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIMO.ResultValue<List<ParamInsuredObjectDesc>, UTIMO.ErrorModel>)
            {
                List<ParamInsuredObjectDesc> paramInsuredObjectDescs = (result as UTIMO.ResultValue<List<ParamInsuredObjectDesc>, UTIMO.ErrorModel>).Value;
                insuredObjectsServiceQueryModels.InsuredObjectServiceQueryModels = ModelsServicesAssembler.CreateInsuredObjectServiceQueryModels(paramInsuredObjectDescs);
            }

            return insuredObjectsServiceQueryModels;
        }

        /// <summary>
        /// Consulta las clausulas relacionadas al level
        /// </summary>
        /// <param name="emissionLevel">Nivel de cláusula</param>
        /// <param name="conditionLevelId">Identificador correspondiente</param>
        /// <returns>Lista de cláusulas</returns>
        public ClausesServiceQueryModel GetClausesSQByEmissionLevelConditionLevelId(UNDEN.EmissionLevel emissionLevel, int conditionLevelId)
        {
            ClausesServiceQueryModel clausesServiceQueryModel = new ClausesServiceQueryModel();
            ClauseDAO clauseDAO = new ClauseDAO();

            UTIMO.Result<List<ParamClauseDesc>, UTIMO.ErrorModel> result = clauseDAO.GetParamClauseDescsByEmissionLevelConditionLevelId(emissionLevel, conditionLevelId);
            if (result is UTIMO.ResultError<List<ParamClauseDesc>, UTIMO.ErrorModel>)
            {
                UTIMO.ErrorModel errorModelResult = (result as UTIMO.ResultError<List<ParamClauseDesc>, UTIMO.ErrorModel>).Message;
                clausesServiceQueryModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                clausesServiceQueryModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIMO.ResultValue<List<ParamClauseDesc>, UTIMO.ErrorModel>)
            {
                List<ParamClauseDesc> paramClauseDescs = (result as UTIMO.ResultValue<List<ParamClauseDesc>, UTIMO.ErrorModel>).Value;
                clausesServiceQueryModel.ClauseServiceModels = ModelsServicesAssembler.CreateClauseServiceQueryModels(paramClauseDescs);
            }

            return clausesServiceQueryModel;
        }

        /// <summary>
        /// Obtener Grupos de Coberturas
        /// </summary>
        /// <returns>Grupos de Coberturas</returns>
        public MODUD.CoveredRiskTypesQueryServiceModel GetAllGroupCoverages()
        {
            MODUD.CoveredRiskTypesQueryServiceModel coveredRiskTypesQueryServiceModel = new MODUD.CoveredRiskTypesQueryServiceModel();
            CoveredRiskTypeDAO coveredRiskTypeDAO = new CoveredRiskTypeDAO();

            UTIMO.Result<List<ParamCoveredRiskTypeDesc>, UTIMO.ErrorModel> result = coveredRiskTypeDAO.GetCoveredRiskTypesDesc();
            if (result is UTIMO.ResultError<List<ParamCoveredRiskTypeDesc>, UTIMO.ErrorModel>)
            {
                UTIMO.ErrorModel errorModelResult = (result as UTIMO.ResultError<List<ParamCoveredRiskTypeDesc>, UTIMO.ErrorModel>).Message;
                coveredRiskTypesQueryServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                coveredRiskTypesQueryServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIMO.ResultValue<List<ParamCoveredRiskTypeDesc>, UTIMO.ErrorModel>)
            {
                List<ParamCoveredRiskTypeDesc> paramCoveredRiskTypeDesc = (result as UTIMO.ResultValue<List<ParamCoveredRiskTypeDesc>, UTIMO.ErrorModel>).Value;
                coveredRiskTypesQueryServiceModel.CoveredRiskTypeQueryServiceModels = ModelsServicesAssembler.CreateCoveredRiskTypeQueryServiceModels(paramCoveredRiskTypeDesc);
            }

            return coveredRiskTypesQueryServiceModel;
        }
        #endregion

        #region RatingZone
        /// <summary>
        /// Obtiene las zonas de tarifacion por el filtro
        /// </summary>
        /// <param name="ratingZoneCode">codigo de la zona de tarifacion</param>
        /// <param name="prefixId">id del ramo</param>
        /// <param name="filter">descripcion a buscar</param>
        /// <returns>Zonas de tarifacion MOD-S</returns>
        public RatingZonesServiceModel GetRatingZoneServiceModel(int? ratingZoneCode, int? prefixId, string filter)
        {
            RatingZoneDAO ratingZoneDao = new RatingZoneDAO();
            RatingZonesServiceModel ratingZoneServiceModel = new RatingZonesServiceModel();
            UTIER.Result<List<ParamRatingZoneCity>, UTIER.ErrorModel> result = ratingZoneDao.GetRatingZoneServiceModel(ratingZoneCode, prefixId, filter);

            if (result is UTIER.ResultError<List<ParamRatingZoneCity>, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<List<ParamRatingZoneCity>, UTIER.ErrorModel>).Message;
                ratingZoneServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                ratingZoneServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<List<ParamRatingZoneCity>, UTIER.ErrorModel>)
            {
                List<ParamRatingZoneCity> paramRatingZoneCities = (result as UTIER.ResultValue<List<ParamRatingZoneCity>, UTIER.ErrorModel>).Value;
                ratingZoneServiceModel.RatingZones = ModelsServicesAssembler.CreateRatingZoneServiceModels(paramRatingZoneCities);
                ratingZoneServiceModel.RatingZones.ForEach(x => x.StatusTypeService = MODEN.StatusTypeService.Original);
            }

            return ratingZoneServiceModel;
        }

        /// <summary>
        /// CRUD de zonas de tarifacion
        /// </summary>
        /// <param name="ratingZoneServiceModels">zonas de tarifacion MOD-S</param>
        /// <returns>Listado de zonas de tarifacion de la operacion del CRUD</returns>
        public List<RatingZoneServiceModel> ExecuteOperationsRatingZoneServiceModel(List<RatingZoneServiceModel> ratingZoneServiceModels)
        {
            List<RatingZoneServiceModel> result = new List<RatingZoneServiceModel>();
            foreach (RatingZoneServiceModel itemSm in ratingZoneServiceModels)
            {
                ParamRatingZoneCity item = ServicesModelsAssembler.CreateParamRatingZoneCity(itemSm);

                RatingZoneServiceModel itemResult = new RatingZoneServiceModel();
                using (Transaction transaction = new Transaction())
                {
                    switch (itemSm.StatusTypeService)
                    {
                        case MODEN.StatusTypeService.Create:
                            itemResult = this.OperationRatingZoneServiceModel(item, MODEN.StatusTypeService.Create);
                            break;
                        case MODEN.StatusTypeService.Update:
                            itemResult = this.OperationRatingZoneServiceModel(item, MODEN.StatusTypeService.Update);
                            break;
                        case MODEN.StatusTypeService.Delete:
                            itemResult = this.OperationRatingZoneServiceModel(item, MODEN.StatusTypeService.Delete);
                            break;
                        default:
                            break;
                    }

                    if (itemResult.ErrorServiceModel.ErrorTypeService == MODEN.ErrorTypeService.TechnicalFault)
                    {
                        transaction.Dispose();
                    }
                    else
                    {
                        transaction.Complete();
                    }
                }

                result.Add(itemResult);
            }

            return result;
        }

        /// <summary>
        /// Llamado a DAOS respectivos para operacion del CRUD y operacion con result
        /// </summary>
        /// <param name="paramRatingZoneCity">zona de tarifacion</param>
        /// <param name="statusTypeService">enum de proceso a realizar del CRUD</param>
        /// <returns> zona de tarifacion - Resultado de CRUD </returns>
        public RatingZoneServiceModel OperationRatingZoneServiceModel(ParamRatingZoneCity paramRatingZoneCity, MODEN.StatusTypeService statusTypeService)
        {
            RatingZoneDAO ratingZoneDao = new RatingZoneDAO();
            RatingZoneServiceModel ratingZoneServiceModelResult = new RatingZoneServiceModel
            {
                ErrorServiceModel = new MODPA.ErrorServiceModel
                {
                    ErrorDescription = new List<string>(),
                    ErrorTypeService = MODEN.ErrorTypeService.Ok
                }
            };
            UTIER.Result<ParamRatingZoneCity, UTIER.ErrorModel> result;
            switch (statusTypeService)
            {
                case MODEN.StatusTypeService.Create:
                    result = ratingZoneDao.CreateParamRatingZoneCity(paramRatingZoneCity);
                    break;
                case MODEN.StatusTypeService.Update:
                    result = ratingZoneDao.UpdateParamRatingZoneCity(paramRatingZoneCity);
                    break;
                case MODEN.StatusTypeService.Delete:
                    result = ratingZoneDao.DeleteParamRatingZoneCity(paramRatingZoneCity);
                    break;
                default:
                    result = ratingZoneDao.CreateParamRatingZoneCity(paramRatingZoneCity);
                    break;
            }

            if (result is UTIER.ResultError<ParamRatingZoneCity, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = ((UTIER.ResultError<ParamRatingZoneCity, UTIER.ErrorModel>)result).Message;
                ratingZoneServiceModelResult.Description = paramRatingZoneCity.RatingZone.Description;
                ratingZoneServiceModelResult.ErrorServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                ratingZoneServiceModelResult.ErrorServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                ratingZoneServiceModelResult.StatusTypeService = MODEN.StatusTypeService.Error;
            }
            else if (result is UTIER.ResultValue<ParamRatingZoneCity, UTIER.ErrorModel>)
            {
                paramRatingZoneCity = ((UTIER.ResultValue<ParamRatingZoneCity, UTIER.ErrorModel>)result).Value;
                ratingZoneServiceModelResult = ModelsServicesAssembler.CreateRatingZoneServiceModel(paramRatingZoneCity);
                ratingZoneServiceModelResult.StatusTypeService = statusTypeService;
                ratingZoneServiceModelResult.ErrorServiceModel = new MODPA.ErrorServiceModel { ErrorTypeService = MODEN.ErrorTypeService.Ok };
            }

            return ratingZoneServiceModelResult;
        }

        /// <summary>
        /// Obtiene los paises
        /// </summary>
        /// <returns>Lista de paises</returns>
        public MODCO.CountriesServiceQueryModel GetCountries()
        {
            RatingZoneDAO ratingZoneDao = new RatingZoneDAO();
            MODCO.CountriesServiceQueryModel countryServiceQueryModels = new MODCO.CountriesServiceQueryModel();
            UTIER.Result<List<COMMO.Country>, UTIER.ErrorModel> result = ratingZoneDao.GetCountries();

            if (result is UTIER.ResultError<List<COMMO.Country>, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<List<COMMO.Country>, UTIER.ErrorModel>).Message;
                countryServiceQueryModels.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                countryServiceQueryModels.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<List<COMMO.Country>, UTIER.ErrorModel>)
            {
                List<COMMO.Country> countries = (result as UTIER.ResultValue<List<COMMO.Country>, UTIER.ErrorModel>).Value;
                countryServiceQueryModels.Counties = ModelsServicesAssembler.CreateCountryServiceQueryModel(countries);
            }

            return countryServiceQueryModels;
        }

        /// <summary>
        /// Obtiene los estados por pais
        /// </summary>
        /// <param name="idCountry">identificador del pais</param>
        /// <returns>lista de estados</returns>
        public MODCO.StatesServiceQueryModel GetStatesByCountry(int idCountry)
        {
            RatingZoneDAO ratingZoneDao = new RatingZoneDAO();
            MODCO.StatesServiceQueryModel statesServiceQueryModel = new MODCO.StatesServiceQueryModel();
            UTIER.Result<List<COMMO.State>, UTIER.ErrorModel> result = ratingZoneDao.GetStatesByCountry(idCountry);

            if (result is UTIER.ResultError<List<COMMO.State>, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<List<COMMO.State>, UTIER.ErrorModel>).Message;
                statesServiceQueryModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                statesServiceQueryModel.ErrorDescription =  errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<List<COMMO.State>, UTIER.ErrorModel>)
            {
                List<COMMO.State> countries = (result as UTIER.ResultValue<List<COMMO.State>, UTIER.ErrorModel>).Value;
                statesServiceQueryModel.States = ModelsServicesAssembler.CreateStatesServiceQueryModel(countries);
            }

            return statesServiceQueryModel;
        }

        /// <summary>
        /// Obtiene las cuidades por pais y estado
        /// </summary>
        /// <param name="idState">Identificador del estado</param>
        /// <param name="idCountry">identificador del pais</param>
        /// <returns>lista de estados</returns>
        public MODCO.CitiesServiceRelationModel GetCitiesByStateCountry(int idState, int idCountry, int PrefixCode)
        {
            RatingZoneDAO ratingZoneDao = new RatingZoneDAO();
            MODCO.CitiesServiceRelationModel citiesServiceRelationModel = new MODCO.CitiesServiceRelationModel();
            UTIER.Result<List<COMMO.City>, UTIER.ErrorModel> result = ratingZoneDao.GetCitiesByStateCountry(idState, idCountry, PrefixCode);

            if (result is UTIER.ResultError<List<COMMO.City>, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<List<COMMO.City>, UTIER.ErrorModel>).Message;
                citiesServiceRelationModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                citiesServiceRelationModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<List<COMMO.City>, UTIER.ErrorModel>)
            {
                List<COMMO.City> cities = (result as UTIER.ResultValue<List<COMMO.City>, UTIER.ErrorModel>).Value;
                citiesServiceRelationModel.Cities = ModelsServicesAssembler.CreateCityServiceRelationModels(cities);
            }

            return citiesServiceRelationModel;
        }

        /// <summary>
        /// Generar archivo excel de planes de pago
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <returns>Archivo de excel</returns>
        public MODPA.ExcelFileServiceModel GenerateFileToRatingZone(string fileName)
        {
            RatingZoneDAO ratingZoneDao = new RatingZoneDAO();
            MODPA.ExcelFileServiceModel excelFileServiceModel = new MODPA.ExcelFileServiceModel();

            UTIER.Result<string, UTIER.ErrorModel> result = ratingZoneDao.GenerateFileToRatingZone(fileName);
            if (result is UTIER.ResultError<string, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<string, UTIER.ErrorModel>).Message;
                excelFileServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                excelFileServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<string, UTIER.ErrorModel>)
            {
                excelFileServiceModel.FileData = (result as UTIER.ResultValue<string, UTIER.ErrorModel>).Value;
            }

            return excelFileServiceModel;
        }





        public List<RatingZoneServiceModel> GetRatingZone(int prefixId, int branchId)
        {
            RatingZoneDAO ratingZoneDao = new RatingZoneDAO();
            List<RatingZoneServiceModel> ratingZoneService = new List<RatingZoneServiceModel>();
            List<ParamRatingZone> ListRatingZone = ratingZoneDao.GetRatingZonesByPrefixIdAndBranchId(prefixId, branchId);
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ParamRatingZone, RatingZoneServiceModel>();
                cfg.CreateMap<ParamPrefix, PrefixsServiceModel>();
            });
            foreach (ParamRatingZone itemRatingZone in ListRatingZone)
            {
                ratingZoneService.Add(config.CreateMapper().Map<ParamRatingZone, RatingZoneServiceModel>(itemRatingZone));
            }
            return ratingZoneService;
        }
        public void ExecuteOperationRatingZone(List<RatingZoneServiceModel> ratingZoneService)
        {
            RatingZoneDAO ratingZoneDao = new RatingZoneDAO();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<RatingZoneServiceModel, ParamRatingZone>();
                cfg.CreateMap<PrefixsServiceModel, ParamPrefix>();
            });
            foreach (RatingZoneServiceModel itemRatinZones in ratingZoneService)
            {

                if (itemRatinZones.StatusTypeService == StatusTypeService.Create)
                {
                    var createRatingZone = config.CreateMapper().Map<RatingZoneServiceModel, ParamRatingZone>(itemRatinZones);
                    ratingZoneDao.CreateRatingZones(createRatingZone);
                }
                if (itemRatinZones.StatusTypeService == StatusTypeService.Update)
                {
                    var createRatingZone = config.CreateMapper().Map<RatingZoneServiceModel, ParamRatingZone>(itemRatinZones);
                    ratingZoneDao.UpdateRatingZones(createRatingZone);
                }
                if (itemRatinZones.StatusTypeService == StatusTypeService.Delete)
                {
                    var createRatingZone = config.CreateMapper().Map<RatingZoneServiceModel, ParamRatingZone>(itemRatinZones);
                    ratingZoneDao.DeleteRatingZones(createRatingZone);
                }
            }
        }
        #endregion

        #region Plan Financiero

        /// <summary>
        /// Obtiene listado de plan financiero
        /// </summary>        
        /// <returns>listado de plan financiero</returns>
        public FinancialPlansServiceModel GetFinancialPlanServiceModel()
        {
            FinancialPlansServiceModel financialPlanServiceModel = new FinancialPlansServiceModel();
            FinancialPlanDAO financialPlanDAO = new FinancialPlanDAO();

            UTIER.Result<List<ParamFinancialPlanComponent>, UTIER.ErrorModel> result = financialPlanDAO.GetParamFinancialPlans();
            if (result is UTIER.ResultError<List<ParamFinancialPlanComponent>, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<List<ParamFinancialPlanComponent>, UTIER.ErrorModel>).Message;
                financialPlanServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                financialPlanServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<List<ParamFinancialPlanComponent>, UTIER.ErrorModel>)
            {
                List<ParamFinancialPlanComponent> paramFinancialPlans = (result as UTIER.ResultValue<List<ParamFinancialPlanComponent>, UTIER.ErrorModel>).Value;
                financialPlanServiceModel.FinancialPlanServiceModels = ModelsServicesAssembler.CreateFinancialPlanServiceModels(paramFinancialPlans);
            }

            return financialPlanServiceModel;
        }

        /// <summary>
        /// Obtener lista de metodo de pago
        /// </summary>
        /// <returns>Metodo de pago MOD-S resultado de consulta de metodo de pago</returns>
        public PARUPSM.PaymentMethodsServiceQueryModel GetMethodPaymentServiceModel()
        {
            PaymentMethodDAO paymentMethodDAO = new PaymentMethodDAO();
            PARUPSM.PaymentMethodsServiceQueryModel paymentMethodServiceModels = new PARUPSM.PaymentMethodsServiceQueryModel();
            UTIER.Result<List<ParamPaymentMethod>, UTIER.ErrorModel> result = paymentMethodDAO.GetPaymentMethod();
            if (result is UTIER.ResultError<List<ParamPaymentMethod>, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<List<ParamPaymentMethod>, UTIER.ErrorModel>).Message;
                paymentMethodServiceModels.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                paymentMethodServiceModels.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<List<ParamPaymentMethod>, UTIER.ErrorModel>)
            {
                List<ParamPaymentMethod> listParamPaymentMethod = (result as UTIER.ResultValue<List<ParamPaymentMethod>, UTIER.ErrorModel>).Value;
                paymentMethodServiceModels.PaymentMethodServiceModels = ModelsServicesAssembler.CreatePaymentMethodsServiceModel(listParamPaymentMethod);
            }

            return paymentMethodServiceModels;
        }

        /// <summary>
        /// Obtener lista de componentes
        /// </summary>
        /// <returns>Componentes MOD-S resultado de consulta de componentes</returns>
        public PARUPSM.ComponentRelationsServiceModel GetComponentRelationServiceModel()
        {
            ComponentRelationDAO componentRelationDAO = new ComponentRelationDAO();
            PARUPSM.ComponentRelationsServiceModel componentRelationServiceModels = new PARUPSM.ComponentRelationsServiceModel();
            UTIER.Result<List<UNDMO.Component>, UTIER.ErrorModel> result = componentRelationDAO.GetComponent();
            if (result is UTIER.ResultError<List<UNDMO.Component>, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<List<UNDMO.Component>, UTIER.ErrorModel>).Message;
                componentRelationServiceModels.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                componentRelationServiceModels.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<List<UNDMO.Component>, UTIER.ErrorModel>)
            {
                List<UNDMO.Component> listParamPaymentMethod = (result as UTIER.ResultValue<List<UNDMO.Component>, UTIER.ErrorModel>).Value;
                componentRelationServiceModels.ComponentRelationServiceModels = ModelsServicesAssembler.CreateComponentRelationsServiceModel(listParamPaymentMethod);
            }

            return componentRelationServiceModels;
        }


        /// <summary>
        /// Generar archivo excel de planes financieros
        /// </summary>
        /// <param name="paymentPlans">Listado de planes de y</param>
        /// <param name="fileName">Nombre de archivo</param>
        /// <returns>Archivo de excel</returns>
        public MODPA.ExcelFileServiceModel GenerateFileToFinancialPlan()
        {
            FinancialPlanDAO financialPlanDAO = new FinancialPlanDAO();
            MODPA.ExcelFileServiceModel excelFileServiceModel = new MODPA.ExcelFileServiceModel();
            FinancialPlansServiceModel financials = this.GetFinancialPlanServiceModel();
            UTIER.Result<string, UTIER.ErrorModel> result = financialPlanDAO.GenerateFileToFinancialPlan(ServicesModelsAssembler.CreateParametrizationFinancialPlans(financials.FinancialPlanServiceModels), Resources.Errors.ListFinancialPlan);
            if (result is UTIER.ResultError<string, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<string, UTIER.ErrorModel>).Message;
                excelFileServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                excelFileServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<string, UTIER.ErrorModel>)
            {
                excelFileServiceModel.FileData = (result as UTIER.ResultValue<string, UTIER.ErrorModel>).Value;
            }

            return excelFileServiceModel;
        }


        public bool GetPaymentMethodForId(int IdMethod)
        {
            PARUPSM.PaymentMethodsServiceQueryModel paymentMethodServiceModels = new PARUPSM.PaymentMethodsServiceQueryModel();
            PaymentMethodDAO paymentMethodDAO = new PaymentMethodDAO();
            UTIER.Result<List<ParamPaymentMethod>, UTIER.ErrorModel> resultPaymentMethod = paymentMethodDAO.GetPaymentMethodForId(IdMethod);

            if (resultPaymentMethod is UTIER.ResultValue<List<ParamPaymentMethod>, UTIER.ErrorModel>)
            {
                List<ParamPaymentMethod> listParamPaymentMethod = (resultPaymentMethod as UTIER.ResultValue<List<ParamPaymentMethod>, UTIER.ErrorModel>).Value;
                paymentMethodServiceModels.PaymentMethodServiceModels = ModelsServicesAssembler.CreatePaymentMethodsServiceModel(listParamPaymentMethod);
                foreach (ParamPaymentMethod item in listParamPaymentMethod)
                {
                    item.Id = IdMethod;
                    if(item.IsInCome == false)
                    {
                        return false;
                    }
             
                }

            }
                return true;
        }

        /// <summary>
        /// Ejecuta operaciones crud plan financiero
        /// </summary>
        /// <param name="financialPlan">Recibe plan financiero</param>
        /// <returns>Retorna plan financiero</returns>
        public FinancialPlanServiceModel ExecuteOperationFinancialPlan(FinancialPlanServiceModel financialPlan)
        {
            FinancialPlanServiceModel result = new FinancialPlanServiceModel();
            result.ErrorServiceModel = new MODPA.ErrorServiceModel()
            {
                ErrorTypeService = MODEN.ErrorTypeService.BusinessFault,
                ErrorDescription = new List<string>(),
            };
            MODPA.ErrorServiceModel errorServiceModel = new MODPA.ErrorServiceModel() { ErrorTypeService = MODEN.ErrorTypeService.Ok };
            if (errorServiceModel.ErrorTypeService == MODEN.ErrorTypeService.Ok)
            {
                using (Transaction transaction = new Transaction())
                {
                    result = this.OperationFinancialPlan(financialPlan, financialPlan.StatusTypeService);

                    // Validacion de error en el result
                    if ((result.ErrorServiceModel != null && result.ErrorServiceModel.ErrorTypeService == MODEN.ErrorTypeService.TechnicalFault))
                    {
                        transaction.Dispose();
                    }
                    else
                    {
                        transaction.Complete();
                    }
                }
            }
            else
            {
                result.ErrorServiceModel = errorServiceModel;
                result.StatusTypeService = MODEN.StatusTypeService.Error;
            }

            return result;
        }

        /// <summary>
        /// CRUD plan financiero
        /// </summary>
        /// <param name="financialPlanServiceModel">Recibe plan financiero</param>
        /// <returns>Retorna resultado</returns>
        public FinancialPlanServiceModel OperationFinancialPlan(FinancialPlanServiceModel financialPlan, MODEN.StatusTypeService statusTypeService)
        {


            this.GetPaymentMethodForId(financialPlan.PaymentMethodServiceQueryModel.Id);
            ParamFinancialPlanComponent paramFinancialPlanComponent = ServicesModelsAssembler.CreateParametrizationFinancialPlan(financialPlan);
            FinancialPlanDAO financialPlanDAO = new FinancialPlanDAO();
            UTIER.Result<ParamFinancialPlanComponent, UTIER.ErrorModel> resultDAO;

                switch (statusTypeService)
                {
                    case MODEN.StatusTypeService.Create:
                        resultDAO = financialPlanDAO.CreateParametrizationFinancialPlan(paramFinancialPlanComponent);
                        break;
                case MODEN.StatusTypeService.Update:
                    resultDAO = financialPlanDAO.UpdateParamCoverage(paramFinancialPlanComponent);
                    break;
                default:
                        resultDAO = financialPlanDAO.CreateParametrizationFinancialPlan(paramFinancialPlanComponent);
                        break;
                }

                FinancialPlanServiceModel result = new FinancialPlanServiceModel();
            if (resultDAO is UTIER.ResultError<ParamFinancialPlanComponent, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (resultDAO as UTIER.ResultError<ParamFinancialPlanComponent, UTIER.ErrorModel>).Message;
                result.ErrorServiceModel = new MODPA.ErrorServiceModel()
                {
                    ErrorDescription = errorModelResult.ErrorDescription,
                    ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType
                };
                result.StatusTypeService = financialPlan.StatusTypeService;
            }
           
            else if (resultDAO is UTIER.ResultValue<ParamFinancialPlanComponent, UTIER.ErrorModel>)
            {
                ParamFinancialPlanComponent paramFinancialPlanResult = (resultDAO as UTIER.ResultValue<ParamFinancialPlanComponent, UTIER.ErrorModel>).Value;
                result = ModelsServicesAssembler.CreateFinancialPlanServiceModel(paramFinancialPlanResult);
                result.StatusTypeService = financialPlan.StatusTypeService;
                result.ErrorServiceModel = new MODPA.ErrorServiceModel()
                {
                    ErrorTypeService = MODEN.ErrorTypeService.Ok,
                    ErrorDescription = new List<string>()
                };
                UTIER.Result<ParamFinancialPlanComponent, UTIER.ErrorModel> resultCoveragesDelete = financialPlanDAO.DeleteParamFinancialPlan(paramFinancialPlanComponent, false);
                // Creación de hijo: componen
                if (financialPlan.FirstPayComponentsServiceModel != null)
                    {
                        result.FirstPayComponentsServiceModel = this.OperationFirtsComponent(financialPlan.FirstPayComponentsServiceModel, paramFinancialPlanResult.ParamFinancialPlan.Id);
                    }
                
                else
                {
                    List<string> listErrors = new List<string>();
                    listErrors.Add("Error");
                    result.ErrorServiceModel.ErrorDescription.AddRange(listErrors);
                    result.StatusTypeService = financialPlan.StatusTypeService;
                }
            }

            return result;
        }

        /// <summary>
        /// Ejecuta operaciones hijo plan financiero
        /// </summary>
        /// <param name="firstComponentServiceQueryModel">Recibe firstComponent</param>
        /// <param name="financialPlanId">Recibe Id FinancialPlan</param>
        /// <returns>Retorna resultado</returns>
        public List<FirstPayComponentServiceModel> OperationFirtsComponent(List<FirstPayComponentServiceModel> firstComponentServiceQueryModel, int financialPlanId)
        {
            FirstPayComponentsServiceModel result = new FirstPayComponentsServiceModel()
            {
                ErrorTypeService = MODEN.ErrorTypeService.Ok,
                ErrorDescription = new List<string>()
            };
            ComponentRelationDAO componentRelationDAO = new ComponentRelationDAO();
            List<ParamFirstPayComponent> paramFirtsComponent = ServicesModelsAssembler.CreateParamFirtsComponents(firstComponentServiceQueryModel);

            // Se recorre listado para insercion uno a uno
            foreach (ParamFirstPayComponent item in paramFirtsComponent)
            {
                if (result.ErrorTypeService == MODEN.ErrorTypeService.Ok)
                {
                    UTIER.Result<ParamFirstPayComponent, UTIER.ErrorModel> resultDAO = componentRelationDAO.CreateFirstComponent(item, financialPlanId);
                    if (resultDAO is UTIER.ResultError<ParamFirstPayComponent, UTIER.ErrorModel>)
                    {
                        UTIER.ErrorModel errorModelResult = (resultDAO as UTIER.ResultError<ParamFirstPayComponent, UTIER.ErrorModel>).Message;
                        result.ErrorDescription.AddRange(errorModelResult.ErrorDescription);
                        result.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                    }
                    else
                    {
                        ParamFirstPayComponent paramFirts = (resultDAO as UTIER.ResultValue<ParamFirstPayComponent, UTIER.ErrorModel>).Value;
                        result.FirstPayComponentServiceModel = (ModelsServicesAssembler.CreateFirtsComponenetServiceModel(paramFirts));
                    }
                }
            }

            return firstComponentServiceQueryModel;
        }

        /// <summary>
        /// Listado de planes financieros
        /// </summary>
        /// <param name="idPaymentPlan">id plan de pago</param>
        /// <param name="idPaymentMethod">id metodo de pago</param>
        /// <param name="idCurrency">id moneda</param>
        /// <returns>listado plan financiero</returns>
        public FinancialPlansServiceModel GetFinancialPlanForItems(int idPaymentPlan, int idPaymentMethod, int idCurrency)
        {
            FinancialPlansServiceModel financialPlanServiceQueryModel = new FinancialPlansServiceModel();
            FinancialPlanDAO financialPlanDAO = new FinancialPlanDAO();

            UTIER.Result<List<ParamFinancialPlanComponent>, UTIER.ErrorModel> result = financialPlanDAO.GetFinancialPlanForItems(idPaymentPlan,idPaymentMethod,idCurrency);
            if (result is UTIER.ResultError<List<ParamFinancialPlanComponent>, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<List<ParamFinancialPlanComponent>, UTIER.ErrorModel>).Message;
                financialPlanServiceQueryModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                financialPlanServiceQueryModel.ErrorDescription = errorModelResult.ErrorDescription;
                
            }
            else if (result is UTIER.ResultValue<List<ParamFinancialPlanComponent>, UTIER.ErrorModel>)
            {
                
                List<ParamFinancialPlanComponent> paramFinancialPlan = (result as UTIER.ResultValue<List<ParamFinancialPlanComponent>, UTIER.ErrorModel>).Value;
                financialPlanServiceQueryModel.FinancialPlanServiceModels = ModelsServicesAssembler.UpdateFinancialPlanServiceModels(paramFinancialPlan);
            }

            return financialPlanServiceQueryModel;
        }

        #endregion
		
		#region Carrocería de vehículo
        /// <summary>
        /// Obtiene las Carrocería
        /// </summary>
        /// <returns>Carrocerías de vehículo</returns>
        public PARUPSM.VehicleBodiesServiceModel GetVehicleBodies()
        {
            PARUPSM.VehicleBodiesServiceModel vehicleBodiesServiceModel = new PARUPSM.VehicleBodiesServiceModel();
            VehicleBodyDAO dao = new VehicleBodyDAO();
            UTIER.Result<List<ParamVehicleBodyUse>, UTIER.ErrorModel> resultVehicleBodies = dao.GetVehicleBodies();
            return Assemblers.ServicesModelsAssembler.CreateVehicleBodiesServiceModel(resultVehicleBodies);
        }

        /// <summary>
        /// Ejecuta las operaciones de creacion, actualizacion y eliminacion
        /// </summary>
        /// <param name="vehicleBodies">Carrocería a realizar operacion</param>
        /// <returns>Listado de Carrocería</returns>
        public List<PARUPSM.VehicleBodyServiceModel> ExecuteOperationsVehicleBody(List<PARUPSM.VehicleBodyServiceModel> vehicleBodies)
        {
            VehicleBodyDAO vehicleBodyDAO = new VehicleBodyDAO();
            foreach (PARUPSM.VehicleBodyServiceModel vehicleBodyCreate in vehicleBodies.Where(p => p.StatusTypeService == MODEN.StatusTypeService.Create))
            {
                UTIER.Result<ParamVehicleBodyUse, UTIER.ErrorModel> paramVehicleBodyUse = ModelsServicesAssembler.CreateVehicleBodyUse(vehicleBodyCreate);
                if (paramVehicleBodyUse is UTIER.ResultError<ParamVehicleBodyUse, UTIER.ErrorModel>)
                {
                    vehicleBodyCreate.ErrorServiceModel = new MODPA.ErrorServiceModel()
                    {
                        ErrorDescription = ((UTIER.ResultError<ParamVehicleBodyUse, UTIER.ErrorModel>)paramVehicleBodyUse).Message.ErrorDescription.Select(p => vehicleBodyCreate.ToString() + p).ToList(),
                        ErrorTypeService = (MODEN.ErrorTypeService)((UTIER.ResultError<ParamVehicleBodyUse, UTIER.ErrorModel>)paramVehicleBodyUse).Message.ErrorType
                    };
                }
                else
                {
                    paramVehicleBodyUse = vehicleBodyDAO.Insert((UTIER.ResultValue<ParamVehicleBodyUse, UTIER.ErrorModel>)paramVehicleBodyUse);
                    if (paramVehicleBodyUse is UTIER.ResultError<ParamVehicleBodyUse, UTIER.ErrorModel>)
                    {
                        vehicleBodyCreate.ErrorServiceModel = new MODPA.ErrorServiceModel()
                        {
                            ErrorDescription = ((UTIER.ResultError<ParamVehicleBodyUse, UTIER.ErrorModel>)paramVehicleBodyUse).Message.ErrorDescription.Select(p => vehicleBodyCreate.ToString() + p).ToList(),
                            ErrorTypeService = (MODEN.ErrorTypeService)((UTIER.ResultError<ParamVehicleBodyUse, UTIER.ErrorModel>)paramVehicleBodyUse).Message.ErrorType
                        };
                    }
                }
            }

            foreach (PARUPSM.VehicleBodyServiceModel vehicleBodyUpdate in vehicleBodies.Where(p => p.StatusTypeService == MODEN.StatusTypeService.Update))
            {
                UTIER.Result<ParamVehicleBodyUse, UTIER.ErrorModel> paramVehicleBodyUse = ModelsServicesAssembler.CreateVehicleBodyUse(vehicleBodyUpdate);
                if (paramVehicleBodyUse is UTIER.ResultError<ParamVehicleBodyUse, UTIER.ErrorModel>)
                {
                    vehicleBodyUpdate.ErrorServiceModel = new MODPA.ErrorServiceModel()
                    {
                        ErrorDescription = ((UTIER.ResultError<ParamVehicleBodyUse, UTIER.ErrorModel>)paramVehicleBodyUse).Message.ErrorDescription.Select(p => vehicleBodyUpdate.ToString() + p).ToList(),
                        ErrorTypeService = (MODEN.ErrorTypeService)((UTIER.ResultError<ParamVehicleBodyUse, UTIER.ErrorModel>)paramVehicleBodyUse).Message.ErrorType
                    };
                }
                else
                {
                    paramVehicleBodyUse = vehicleBodyDAO.Update((UTIER.ResultValue<ParamVehicleBodyUse, UTIER.ErrorModel>)paramVehicleBodyUse);
                    if (paramVehicleBodyUse is UTIER.ResultError<ParamVehicleBodyUse, UTIER.ErrorModel>)
                    {
                        vehicleBodyUpdate.ErrorServiceModel = new MODPA.ErrorServiceModel()
                        {
                            ErrorDescription = ((UTIER.ResultError<ParamVehicleBodyUse, UTIER.ErrorModel>)paramVehicleBodyUse).Message.ErrorDescription.Select(p => vehicleBodyUpdate.ToString() + p).ToList(),
                            ErrorTypeService = (MODEN.ErrorTypeService)((UTIER.ResultError<ParamVehicleBodyUse, UTIER.ErrorModel>)paramVehicleBodyUse).Message.ErrorType
                        };
                    }
                }
            }

            foreach (PARUPSM.VehicleBodyServiceModel vehicleBodyDelete in vehicleBodies.Where(p => p.StatusTypeService == MODEN.StatusTypeService.Delete))
            {
                UTIER.Result<int, UTIER.ErrorModel> resultDelete = vehicleBodyDAO.Delete(vehicleBodyDelete.Id);
                if (resultDelete is UTIER.ResultError<int, UTIER.ErrorModel>)
                {
                    vehicleBodyDelete.ErrorServiceModel = new MODPA.ErrorServiceModel()
                    {
                        ErrorDescription = ((UTIER.ResultError<int, UTIER.ErrorModel>)resultDelete).Message.ErrorDescription.Select(p => vehicleBodyDelete.ToString() + p).ToList(),
                        ErrorTypeService = (MODEN.ErrorTypeService)((UTIER.ResultError<int, UTIER.ErrorModel>)resultDelete).Message.ErrorType
                    };
                }
            }

            return vehicleBodies;
        }

        /// <summary>
        /// Genera el archivo para Carrocería
        /// </summary>
        /// <param name="fileName">Nombre de archivo</param>
        /// <returns>Ruta archivo</returns>
        public MODPA.ExcelFileServiceModel GenerateFileToExportVehicleBody(string fileName)
        {
            VehicleBodyDAO vehicleBodyDAO = new VehicleBodyDAO();
            UTIER.Result<string, UTIER.ErrorModel> generatedFile = vehicleBodyDAO.GenerateFileToExportVehicleBody(fileName);
            if (generatedFile is UTIER.ResultError<string, UTIER.ErrorModel>)
            {
                return new MODPA.ExcelFileServiceModel()
                {
                    ErrorTypeService = (MODEN.ErrorTypeService)((UTIER.ResultError<string, UTIER.ErrorModel>)generatedFile).Message.ErrorType,
                    ErrorDescription = ((UTIER.ResultError<string, UTIER.ErrorModel>)generatedFile).Message.ErrorDescription
                };
            }

            return new MODPA.ExcelFileServiceModel()
            {
                ErrorTypeService = MODEN.ErrorTypeService.Ok,
                FileData = ((UTIER.ResultValue<string, UTIER.ErrorModel>)generatedFile).Value
            };
        }

        /// <summary>
        /// Genera el archivo para Carrocería
        /// </summary>
        /// <param name="vehicleBody">Tipo de vehiculo</param>
        /// <param name="fileName">Nombre de archivo</param>
        /// <returns>Ruta de archivo</returns>
        public MODPA.ExcelFileServiceModel GenerateFileToVehicleUse(PARUPSM.VehicleBodyServiceModel vehicleBody, string fileName)
        {
            VehicleBodyDAO vehicleBodyDAO = new VehicleBodyDAO();
            VehicleUseDAO vehicleUseDAO = new VehicleUseDAO();

            ParamVehicleBody vehicleBodyValue = ((UTIER.ResultValue<ParamVehicleBody, UTIER.ErrorModel>)ParamVehicleBody.GetParamVehicleBody(vehicleBody.Id, vehicleBody.SmallDescription)).Value;

            UTIER.Result<List<ParamVehicleUse>, UTIER.ErrorModel> vehicleUseResult = vehicleUseDAO.GetVehicleUsesByIds(vehicleBody.VehicleUseServiceQueryModel.Select(p => p.Id).ToList());
            if (vehicleUseResult is UTIER.ResultError<List<ParamVehicleUse>, UTIER.ErrorModel>)
            {
                return new MODPA.ExcelFileServiceModel()
                {
                    ErrorTypeService = (MODEN.ErrorTypeService)((UTIER.ResultError<List<ParamVehicleUse>, UTIER.ErrorModel>)vehicleUseResult).Message.ErrorType,
                    ErrorDescription = ((UTIER.ResultError<List<ParamVehicleUse>, UTIER.ErrorModel>)vehicleUseResult).Message.ErrorDescription
                };
            }

            ParamVehicleBodyUse vehicleBodyUse = ((UTIER.ResultValue<ParamVehicleBodyUse, UTIER.ErrorModel>)ParamVehicleBodyUse.GetParamVehicleBodyUse(vehicleBodyValue, ((UTIER.ResultValue<List<ParamVehicleUse>, UTIER.ErrorModel>)vehicleUseResult).Value)).Value;

            UTIER.Result<string, UTIER.ErrorModel> generatedFile = vehicleUseDAO.GenerateFileToVehicleUse(vehicleBodyUse, fileName);
            if (generatedFile is UTIER.ResultError<string, UTIER.ErrorModel>)
            {
                return new MODPA.ExcelFileServiceModel()
                {
                    ErrorTypeService = (MODEN.ErrorTypeService)((UTIER.ResultError<string, UTIER.ErrorModel>)generatedFile).Message.ErrorType,
                    ErrorDescription = ((UTIER.ResultError<string, UTIER.ErrorModel>)generatedFile).Message.ErrorDescription
                };
            }

            return new MODPA.ExcelFileServiceModel()
            {
                ErrorTypeService = MODEN.ErrorTypeService.Ok,
                FileData = ((UTIER.ResultValue<string, UTIER.ErrorModel>)generatedFile).Value
            };
        }
        #endregion Carrocería de vehículo
		
		/// <summary>
        /// Obtiene las coverturas de 2G que hacen referencia a los objeto de seguro de vehiculos
        /// </summary>
        /// <returns>Coverturas 2G</returns>
        public Coverages2GServiceModel GetCoverages2GByVehicleInsuredObject()
        {
            const int insuredObjectVehicule = 3;
            Coverage2GDAO coverage2GDAO = new Coverage2GDAO();
            UTIER.Result<List<ParamCoCoverage2G>, UTIER.ErrorModel> coverageResult = coverage2GDAO.GetCoCoverages2GByInsuredObject(insuredObjectVehicule);
            
            if(coverageResult is UTIER.ResultError<List<ParamCoCoverage2G>, UTIER.ErrorModel>)
            {
                UTIER.ResultError<List<ParamCoCoverage2G>, UTIER.ErrorModel> coverageError = (UTIER.ResultError<List<ParamCoCoverage2G>, UTIER.ErrorModel>)coverageResult;
                return new Coverages2GServiceModel()
                {
                    ErrorDescription = coverageError.Message.ErrorDescription,
                    ErrorTypeService = (MODEN.ErrorTypeService)coverageError.Message.ErrorType
                };
            }
            UTIER.ResultValue<List<ParamCoCoverage2G>, UTIER.ErrorModel> coverageValue = (UTIER.ResultValue<List<ParamCoCoverage2G>, UTIER.ErrorModel>)coverageResult;
            return new Coverages2GServiceModel()
            {
                ErrorTypeService = MODEN.ErrorTypeService.Ok,
                CoverageServiceModels = ModelsServicesAssembler.CreateCoverage2GServiceModels(coverageValue.Value)
            };
        }

        #region Limit Rc

        /// <summary>
        /// Obtener lista de limites rc
        /// </summary>
        /// <returns>Retorna LimitsRcServiceModel</returns>
        public PARUPSM.LimitsRcServiceModel GetLimitsRc()
        {
            PARUPSM.LimitsRcServiceModel limitsRcServiceModel = new PARUPSM.LimitsRcServiceModel();
            LimitRcDAO limitRcDAO = new LimitRcDAO();
            UTMO.Result<List<ParamLimitRc>, UTMO.ErrorModel> result = limitRcDAO.GetLimitsRc();
            if (result is UTMO.ResultError<List<ParamLimitRc>, UTMO.ErrorModel>)
            {
                UTMO.ErrorModel errorModelResult = (result as UTMO.ResultError<List<ParamLimitRc>, UTMO.ErrorModel>).Message;
                limitsRcServiceModel.ErrorTypeService = (ENUMSM.ErrorTypeService)errorModelResult.ErrorType;
                limitsRcServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTMO.ResultValue<List<ParamLimitRc>, UTMO.ErrorModel>)
            {
                List<ParamLimitRc> paramLimitRc = (result as UTMO.ResultValue<List<ParamLimitRc>, UTMO.ErrorModel>).Value;
                limitsRcServiceModel.LimitRcModel = ModelsServicesAssembler.CreateLimitRcServiceModel(paramLimitRc);
            }

            return limitsRcServiceModel;
        }

        /// <summary>
        /// Validación de limite rc
        /// </summary>
        /// <param name="limitRcCode">codigo de limite rc</param>
        /// <returns>valor 1: tiene dependencias, 0: no tiene dependencias</returns>
        public int ValidateLimitc(int limitRcCode)
        {
            try
            {
                LimitRcDAO limitRcDAO = new LimitRcDAO();
                return limitRcDAO.ValidateLimitc(limitRcCode);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Resources.Errors.ErrorGetValidation), ex);
            }
        }

        /// <summary>
        /// Generar archivo excel de limites Rc
        /// </summary>
        /// <param name="limitRcServiceModel">Modelo de LimitRcServiceModel</param>
        /// <param name="fileName">Nombre del archivo</param>
        /// <returns>Retorna Modelo ExcelFileServiceModel</returns>
        public MODSM.ExcelFileServiceModel GenerateFileToLimitRc(List<PARUPSM.LimitRcServiceModel> limitRcServiceModel, string fileName)
        {
            LimitRcDAO limitRcDAO = new LimitRcDAO();
            MODSM.ExcelFileServiceModel excelFileServiceModel = new MODSM.ExcelFileServiceModel();

            UTMO.Result<string, UTMO.ErrorModel> result = limitRcDAO.GenerateFileToLimitRc(ServicesModelsAssembler.CreateLimitsRc(limitRcServiceModel), fileName);
            if (result is UTMO.ResultError<string, UTMO.ErrorModel>)
            {
                UTMO.ErrorModel errorModelResult = (result as UTMO.ResultError<string, UTMO.ErrorModel>).Message;
                excelFileServiceModel.ErrorTypeService = (ENUMSM.ErrorTypeService)errorModelResult.ErrorType;
                excelFileServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTMO.ResultValue<string, UTMO.ErrorModel>)
            {
                excelFileServiceModel.FileData = (result as UTMO.ResultValue<string, UTMO.ErrorModel>).Value;
            }

            return excelFileServiceModel;
        }

        #endregion
        
         #region ExpenseComponent

        /// <summary>
        /// CRUD de Gastos de ejecucion
        /// </summary>       
        /// <param name="expenseComponent">modelo de negocio de descuentos</param>
        /// <param name="statusTypeService">estatus de modelos</param>
        /// <returns>si se actualizo correctamente </returns>
        /// 
        public List<ExpenseServiceModel> ExecuteOperationsExpense(List<ExpenseServiceModel> expenseService)
        {
            ExpenseDAO expenseDAO = new ExpenseDAO();
            List<ExpenseServiceModel> result = new List<ExpenseServiceModel>();
            foreach (ExpenseServiceModel expenseCreate in expenseService.Where(p => p.StatusTypeService == MODEN.StatusTypeService.Create))
            {
                UTIER.Result<ParamExpense, UTIER.ErrorModel> paramExpense = ModelsServicesAssembler.CreateExpenseServiceModel(expenseCreate);

                paramExpense = expenseDAO.CreateExpenseComponent((UTIER.ResultValue<ParamExpense, UTIER.ErrorModel>)paramExpense);
                if (paramExpense is UTIER.ResultError<ParamExpense, UTIER.ErrorModel>)
                {
                    expenseCreate.ErrorServiceModel = new MODPA.ErrorServiceModel()
                    {
                        ErrorDescription = ((UTIER.ResultError<ParamExpense, UTIER.ErrorModel>)paramExpense).Message.ErrorDescription.Select(p => expenseCreate.ToString() + p).ToList(),
                        ErrorTypeService = (MODEN.ErrorTypeService)((UTIER.ResultError<ParamExpense, UTIER.ErrorModel>)paramExpense).Message.ErrorType
                    };
                }
                else
                {
                    expenseCreate.ErrorServiceModel = new MODPA.ErrorServiceModel() { ErrorTypeService = ErrorTypeService.Ok };
                }
                result.Add(expenseCreate);
            }

            foreach (ExpenseServiceModel expenseUpdate in expenseService.Where(p => p.StatusTypeService == MODEN.StatusTypeService.Update))
            {
                UTIER.Result<ParamExpense, UTIER.ErrorModel> paramExpense = ModelsServicesAssembler.CreateExpenseServiceModel(expenseUpdate);
                paramExpense = expenseDAO.UpdateExpenseComponent((UTIER.ResultValue<ParamExpense, UTIER.ErrorModel>)paramExpense);
                if (paramExpense is UTIER.ResultError<ParamExpense, UTIER.ErrorModel>)
                {
                    expenseUpdate.ErrorServiceModel = new MODPA.ErrorServiceModel()
                    {
                        ErrorDescription = ((UTIER.ResultError<ParamExpense, UTIER.ErrorModel>)paramExpense).Message.ErrorDescription.Select(p => expenseUpdate.ToString() + p).ToList(),
                        ErrorTypeService = (MODEN.ErrorTypeService)((UTIER.ResultError<ParamExpense, UTIER.ErrorModel>)paramExpense).Message.ErrorType
                    };
                }
                else
                {
                    expenseUpdate.ErrorServiceModel = new MODPA.ErrorServiceModel() { ErrorTypeService = ErrorTypeService.Ok };
                }
                result.Add(expenseUpdate);
            }

            foreach (ExpenseServiceModel expenseDelete in expenseService.Where(p => p.StatusTypeService == MODEN.StatusTypeService.Delete))
            {
                UTIER.Result<ParamExpense, UTIER.ErrorModel> paramExpense = ModelsServicesAssembler.CreateExpenseServiceModel(expenseDelete);
                paramExpense = expenseDAO.DeleteExpenseComponent((UTIER.ResultValue<ParamExpense, UTIER.ErrorModel>)paramExpense);
                if (paramExpense is UTIER.ResultError<ParamExpense, UTIER.ErrorModel>)
                {
                    expenseDelete.ErrorServiceModel = new MODPA.ErrorServiceModel()
                    {
                        ErrorDescription = ((UTIER.ResultError<ParamExpense, UTIER.ErrorModel>)paramExpense).Message.ErrorDescription.ToList(),
                        ErrorTypeService = (MODEN.ErrorTypeService)((UTIER.ResultError<ParamExpense, UTIER.ErrorModel>)paramExpense).Message.ErrorType
                    };
                }
                else
                {
                    expenseDelete.ErrorServiceModel = new MODPA.ErrorServiceModel() { ErrorTypeService = ErrorTypeService.Ok };
                }
                result.Add(expenseDelete);
            }

            return result;
        }

        /// <summary>
        /// Obtener lista de gastos de ejecucion
        /// </summary>
        /// <returns>resultado de consulta gastos de ejecucion</returns>
        public ExpensesServiceModel GetExpenseServiceModel()
        {
            ExpensesServiceModel expenseServiceModel = new ExpensesServiceModel();
            ExpenseDAO ExpenseDAO = new ExpenseDAO();
            UTIER.Result<List<ParamExpense>, UTIER.ErrorModel> result = ExpenseDAO.GetExpense();
            if (result is UTIER.ResultError<List<ParamExpense>, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<List<ParamExpense>, UTIER.ErrorModel>).Message;
                expenseServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                expenseServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<List<ParamExpense>, UTIER.ErrorModel>)
            {
                List<ParamExpense> Gastos = (result as UTIER.ResultValue<List<ParamExpense>, UTIER.ErrorModel>).Value;
                expenseServiceModel.ComponentServiceModel = ModelsServicesAssembler.CreateExpenseServiceModels(Gastos);
            }
            return expenseServiceModel;
        }

        /// <summary>
        /// Obtiene gastos de suscripcion por nombre
        /// </summary>
        /// <param name="description">Recibe Nombre</param>
        /// <returns>Retorna listado por nombre</returns>
        public ExpensesServiceModel GetExpenseByDescription(string description)
        {
            ExpensesServiceModel expenseServiceModel = new ExpensesServiceModel();

            ExpenseDAO expenseDAO = new ExpenseDAO();

            UTIER.Result<List<ParamExpense>, UTIER.ErrorModel> result = expenseDAO.GetExpenseByDescripcion(description);
            if (result is UTIER.ResultError<List<ParamExpense>, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<List<ParamExpense>, UTIER.ErrorModel>).Message;
                expenseServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                expenseServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<List<ParamExpense>, UTIER.ErrorModel>)
            {
                List<ParamExpense> paramExpense = (result as UTIER.ResultValue<List<ParamExpense>, UTIER.ErrorModel>).Value;
                expenseServiceModel.ComponentServiceModel = ModelsServicesAssembler.CreateExpenseServiceModels(paramExpense);
            }

            return expenseServiceModel;
        }

        /// <summary>
        /// Obtener lista tipo de tasa
        /// </summary>
        /// <returns>Plan de pago MOD-S resultado de consulta de objetos del seguro</returns>
        public RateTypeServicesQueryModel GetRateType()
        {
            RateTypeDAO rateSetDAO = new RateTypeDAO();
            RateTypeServicesQueryModel rateTypeServiceQueryModel = new RateTypeServicesQueryModel();
            UTIER.Result<List<ParamRateType>, UTIER.ErrorModel> result = rateSetDAO.GetRateType();
            if (result is UTIER.ResultError<List<ParamRateType>, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<List<ParamRateType>, UTIER.ErrorModel>).Message;
                rateTypeServiceQueryModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                rateTypeServiceQueryModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<List<ParamRateType>, UTIER.ErrorModel>)
            {
                List<ParamRateType> paramRateType = (result as UTIER.ResultValue<List<ParamRateType>, UTIER.ErrorModel>).Value;
                rateTypeServiceQueryModel.RateTypeServicesModel = ModelsServicesAssembler.CreateRateTypeQueryModels(paramRateType);
            }

            return rateTypeServiceQueryModel;
        }

        /// <summary>
        /// Obtener lista tipo de tasa
        /// </summary>
        /// <returns>Plan de pago MOD-S resultado de consulta de objetos del seguro</returns>
        public RulesSetServiceQueryModel GetRuleSet()
        {
            RuleSetDAO ruleSetDAO = new RuleSetDAO();
            RulesSetServiceQueryModel rulesScriptsServices = new RulesSetServiceQueryModel();
            UTIER.Result<List<ParamRuleSet>, UTIER.ErrorModel> result = ruleSetDAO.GetRuleSet();
            if (result is UTIER.ResultError<List<ParamRuleSet>, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<List<ParamRuleSet>, UTIER.ErrorModel>).Message;
                rulesScriptsServices.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                rulesScriptsServices.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<List<ParamRuleSet>, UTIER.ErrorModel>)
            {
                List<ParamRuleSet> paramRuleSet = (result as UTIER.ResultValue<List<ParamRuleSet>, UTIER.ErrorModel>).Value;
                rulesScriptsServices.RuleSetServiceQueryModel = ModelsServicesAssembler.CreateRuleSetQueryModels(paramRuleSet);
            }

            return rulesScriptsServices;
        }

        /// <summary>
        /// Genera el archivo para el tipo de vehiculo
        /// </summary>
        /// <param name="fileName">Nombre de archivo</param>
        /// <returns>Ruta archivo</returns>
        public MODPA.ExcelFileServiceModel GenerateFileToExpense(string fileName)
        {
            ExpenseDAO expenseDAO = new ExpenseDAO();
            UTIER.Result<string, UTIER.ErrorModel> generatedFile = expenseDAO.GenerateFileToExpense(fileName);
            if (generatedFile is UTIER.ResultError<string, UTIER.ErrorModel>)
            {
                return new MODPA.ExcelFileServiceModel()
                {
                    ErrorTypeService = (MODEN.ErrorTypeService)((UTIER.ResultError<string, UTIER.ErrorModel>)generatedFile).Message.ErrorType,
                    ErrorDescription = ((UTIER.ResultError<string, UTIER.ErrorModel>)generatedFile).Message.ErrorDescription
                };
            }

            return new MODPA.ExcelFileServiceModel()
            {
                ErrorTypeService = MODEN.ErrorTypeService.Ok,
                FileData = ((UTIER.ResultValue<string, UTIER.ErrorModel>)generatedFile).Value
            };
        }

        #endregion

        #region Technical Plan

        /// <summary>
        /// Obtener lista de coberturas por objeto del seguro
        /// </summary>
        /// <param name="insuredObjectId">id del objeto de seguro</param>
        /// <returns>modelo de servicio de las coberturas</returns>
        public CoveragesServiceModel GetCoveragesServiceModelByInsuredObject(int insuredObjectId)
        {
            CoveragesServiceModel coveragesServiceModel = new CoveragesServiceModel();
            TechnicalPlanDAO technicalPlanDAO = new TechnicalPlanDAO();
            UTIER.Result<List<ParamCoverage>, UTIER.ErrorModel> result = technicalPlanDAO.GetCoverageByInsuredObjects(insuredObjectId);
            if (result is UTIER.ResultError<List<ParamCoverage>, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<List<ParamCoverage>, UTIER.ErrorModel>).Message;
                coveragesServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                coveragesServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<List<ParamCoverage>, UTIER.ErrorModel>)
            {
                List<ParamCoverage> paramCoverages = (result as UTIER.ResultValue<List<ParamCoverage>, UTIER.ErrorModel>).Value;
                coveragesServiceModel.CoverageServiceModels = ModelsServicesAssembler.CreateCoverageServiceModels(paramCoverages);
            }

            return coveragesServiceModel;
        }

        /// <summary>
        /// Obtener lista de objeto del seguro por tipo de riesgo cubierto
        /// </summary>
        /// <param name="coveredRiskType">id del tipo de riesgo cubierto</param>
        /// <returns>modelo de servicio de los objetos de seguro</returns>
        public InsurancesObjectsServiceModel GetInsuredObjectsByCoveredRiskType(int coveredRiskType)
        {
            InsurancesObjectsServiceModel insuredObjServiceModel = new InsurancesObjectsServiceModel();
            TechnicalPlanDAO technicalPlanDAO = new TechnicalPlanDAO();
            UTIER.Result<List<ParamInsuredObject>, UTIER.ErrorModel> result = technicalPlanDAO.GetInsuredObjectsByCoveredRiskType(coveredRiskType);

            if (result is UTIER.ResultError<List<ParamInsuredObject>, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<List<ParamInsuredObject>, UTIER.ErrorModel>).Message;
                insuredObjServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                insuredObjServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<List<ParamInsuredObject>, UTIER.ErrorModel>)
            {
                List<ParamInsuredObject> insuredObject = (result as UTIER.ResultValue<List<ParamInsuredObject>, UTIER.ErrorModel>).Value;
                insuredObjServiceModel.InsuredObjectServiceModel = ModelsServicesAssembler.CreateInsurancesObjectServiceModel(insuredObject);
            }
            return insuredObjServiceModel;
        }

        /// <summary>
        /// Obtener lista de coberturas aliadas por cobertura
        /// </summary>
        /// <param name="coverageId">id de la cobertura</param>
        /// <returns>modelo de servicio de las cobertuas aliadas</returns>
        public AllyCoveragesServiceModel GetCoverageAlliedByCoverageId(int coverageId)
        {
            AllyCoveragesServiceModel coveragesServiceModel = new AllyCoveragesServiceModel();
            TechnicalPlanDAO technicalPlanDAO = new TechnicalPlanDAO();

            UTIER.Result<List<ParamAllyCoverage>, UTIER.ErrorModel> result = technicalPlanDAO.GetAlliedCoverages(coverageId);
            if (result is UTIER.ResultError<List<ParamAllyCoverage>, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<List<ParamAllyCoverage>, UTIER.ErrorModel>).Message;
                coveragesServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                coveragesServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<List<ParamAllyCoverage>, UTIER.ErrorModel>)
            {
                List<ParamAllyCoverage> paramCoverages = (result as UTIER.ResultValue<List<ParamAllyCoverage>, UTIER.ErrorModel>).Value;
                coveragesServiceModel.AllyCoverageServiceModels = ModelsServicesAssembler.CreateAllyCoveragesObjectServiceModel(paramCoverages);
            }

            return coveragesServiceModel;
        }

        /// <summary>
        /// Obtener lista de planes técnicos por descripción
        /// </summary>
        /// <param name="description">descripción del plan técnico</param>
        /// <param name="coveredRiskType">id del tipo de riesgo cubierto</param>
        /// <returns>Lista de modelo de planes técnicos</returns>
        public TechnicalPlansServiceQueryModel GetTechnicalPlanByDescriptionOrCoveredRiskType(string description, int coveredRiskType)
        {
            TechnicalPlansServiceQueryModel technicalPlanServiceModel = new TechnicalPlansServiceQueryModel();
            TechnicalPlanDAO technicalPlanDAO = new TechnicalPlanDAO();
            UTIER.Result<List<ParamTechnicalPlan>, UTIER.ErrorModel> result = technicalPlanDAO.GetTechnicalPlan(description, coveredRiskType, true);
            if (result is UTIER.ResultError<List<ParamTechnicalPlan>, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<List<ParamTechnicalPlan>, UTIER.ErrorModel>).Message;
                technicalPlanServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                technicalPlanServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<List<ParamTechnicalPlan>, UTIER.ErrorModel>)
            {
                List<ParamTechnicalPlan> paramCoverages = (result as UTIER.ResultValue<List<ParamTechnicalPlan>, UTIER.ErrorModel>).Value;
                technicalPlanServiceModel.TechnicalPlanServiceQueryModel = ModelsServicesAssembler.CreateTechnicalPlansServiceModelWithoutCoverages(paramCoverages);
            }
            return technicalPlanServiceModel;            
        }

        public TechnicalPlanCoveragesServiceRelationModel GetCoveragesByTechnicalPlanId(int technicalPlanId)
        {
            TechnicalPlanCoveragesServiceRelationModel technicalPlanCoverageServiceModel = new TechnicalPlanCoveragesServiceRelationModel();
            TechnicalPlanDAO technicalPlanDAO = new TechnicalPlanDAO();
            UTIER.Result<List<ParamTechnicalPlansCoverage>, UTIER.ErrorModel> result = technicalPlanDAO.GetCoveragesByTechnicalPlanId(technicalPlanId);
            if (result is UTIER.ResultError<List<ParamTechnicalPlansCoverage>, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<List<ParamTechnicalPlansCoverage>, UTIER.ErrorModel>).Message;
                technicalPlanCoverageServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                technicalPlanCoverageServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<List<ParamTechnicalPlansCoverage>, UTIER.ErrorModel>)
            {
                List<ParamTechnicalPlansCoverage> paramCoverages = (result as UTIER.ResultValue<List<ParamTechnicalPlansCoverage>, UTIER.ErrorModel>).Value;
                technicalPlanCoverageServiceModel.TechnicalPlanCoverageServiceRelationModel = ModelsServicesAssembler.CreateTechnicalPlanCoverages(paramCoverages);
            }
            return technicalPlanCoverageServiceModel;            
        }

        public TechnicalPlanServiceModel ExecuteOperationTechnicalPlan(TechnicalPlanServiceModel technicalPlan)
        {
            TechnicalPlanServiceModel resultData = new TechnicalPlanServiceModel();
            using (Transaction transaction = new Transaction())
            {
                resultData = this.OperationTechnicalPlan(technicalPlan, technicalPlan.StatusTypeService);
                if (resultData.ErrorServiceModel != null)
                {
                    transaction.Dispose();
                    return resultData;
                }
                else
                {
                    transaction.Complete();
                    resultData.ErrorServiceModel = new MODPA.ErrorServiceModel()
                    {
                        ErrorDescription = new List<string>(),
                        ErrorTypeService = MODEN.ErrorTypeService.Ok
                    };
                    return resultData;
                }
            }            
        }

        public TechnicalPlanServiceModel OperationTechnicalPlan(TechnicalPlanServiceModel technicalPlan, MODEN.StatusTypeService statusTypeService)
        {
            Result<ParamTechnicalPlan, ErrorModel> resultTechnicalPlan;
            Result<ParamTechnicalPlan, ErrorModel> resultDAO;
            Result<ParamTechnicalPlanCoverage, ErrorModel> resultCoverageDAO;
            Result<List<ParamTechnicalPlanCoverage>, ErrorModel> resultListCoverageDAO;

            ResultError<ParamTechnicalPlan, ErrorModel> errortechnicalPlan;
            ParamTechnicalPlan technicalPlanObject;
            bool processCoverages = false;

            int technicalPlanId;

            TechnicalPlansServiceModel tp = this.GetTechnicalPlansServiceModel();
            bool countExist = false;
            int cont = 0;
            string vStatusTypeServices = statusTypeService.ToString();
            if (vStatusTypeServices == "Create")
            {
                while (!countExist && tp.TechnicalPlanServiceModel.Count > cont)
                {
                    if (tp.TechnicalPlanServiceModel[cont].Description == technicalPlan.Description)
                    {
                        if (cont > 1)
                        {
                            countExist = true;
                        }

                    }
                    cont++;
                }
            }            
            

            if (!countExist)
            {
                resultTechnicalPlan = ServicesModelsAssembler.CreateParamTechnicalPlan(technicalPlan);
                if (resultTechnicalPlan is ResultError<ParamTechnicalPlan, ErrorModel>)
                {
                    errortechnicalPlan = (resultTechnicalPlan as ResultError<ParamTechnicalPlan, ErrorModel>);
                    return new TechnicalPlanServiceModel() { ErrorServiceModel = new MODPA.ErrorServiceModel() { ErrorDescription = errortechnicalPlan.Message.ErrorDescription, ErrorTypeService = (MODEN.ErrorTypeService)errortechnicalPlan.Message.ErrorType } };
                }
                technicalPlanObject = (resultTechnicalPlan as ResultValue<ParamTechnicalPlan, ErrorModel>).Value;

                TechnicalPlanDAO technicalPlanDAO = new TechnicalPlanDAO();
                technicalPlanId = technicalPlan.Id;
                switch (statusTypeService)
                {
                    case MODEN.StatusTypeService.Original:
                        processCoverages = true;
                        break;
                    case MODEN.StatusTypeService.Create:
                        resultDAO = technicalPlanDAO.CreateParamTechnicalPlan(technicalPlanObject);
                        if (resultDAO is ResultError<ParamTechnicalPlan, ErrorModel>)
                        {
                            ResultError<ParamTechnicalPlan, ErrorModel> errorData = (resultDAO as ResultError<ParamTechnicalPlan, ErrorModel>);
                            return new TechnicalPlanServiceModel() { ErrorServiceModel = new MODPA.ErrorServiceModel() { ErrorDescription = errorData.Message.ErrorDescription, ErrorTypeService = (MODEN.ErrorTypeService)errorData.Message.ErrorType } };
                        }
                        technicalPlanId = (resultDAO as ResultValue<ParamTechnicalPlan, ErrorModel>).Value.Id;
                        processCoverages = true;
                        break;
                    case MODEN.StatusTypeService.Update:
                        resultDAO = technicalPlanDAO.UpdateParamTechnicalPlan(technicalPlanObject);
                        if (resultDAO is ResultError<ParamTechnicalPlan, ErrorModel>)
                        {
                            ResultError<ParamTechnicalPlan, ErrorModel> errorData = (resultDAO as ResultError<ParamTechnicalPlan, ErrorModel>);
                            return new TechnicalPlanServiceModel() { ErrorServiceModel = new MODPA.ErrorServiceModel() { ErrorDescription = errorData.Message.ErrorDescription, ErrorTypeService = (MODEN.ErrorTypeService)errorData.Message.ErrorType } };
                        }
                        processCoverages = true;
                        break;
                    case MODEN.StatusTypeService.Delete:
                        foreach (TechnicalPlanCoverageServiceRelationModel coverage in technicalPlan.TechnicalPlanCoverages)
                        {
                            if (coverage.AlliedCoverages != null)
                            {
                                Result<List<ParamTechnicalPlanCoverage>, ErrorModel> resultAllyCoverage;
                                resultAllyCoverage = ServicesModelsAssembler.CreateParamTechnicalPlanCoverages(coverage, coverage.AlliedCoverages);
                                if (resultAllyCoverage is ResultError<List<ParamTechnicalPlanCoverage>, ErrorModel>)
                                {
                                    ResultError<List<ParamTechnicalPlanCoverage>, ErrorModel> errorAllyCoverage = (resultAllyCoverage as ResultError<List<ParamTechnicalPlanCoverage>, ErrorModel>);
                                    return new TechnicalPlanServiceModel() { ErrorServiceModel = new MODPA.ErrorServiceModel() { ErrorDescription = errorAllyCoverage.Message.ErrorDescription, ErrorTypeService = (MODEN.ErrorTypeService)errorAllyCoverage.Message.ErrorType } };
                                }
                                resultListCoverageDAO = technicalPlanDAO.DeleteParamTechnicalPlanCoverages(technicalPlanId, (resultAllyCoverage as ResultValue<List<ParamTechnicalPlanCoverage>, ErrorModel>).Value);
                                if (resultListCoverageDAO is ResultError<List<ParamTechnicalPlanCoverage>, ErrorModel>)
                                {
                                    ResultError<List<ParamTechnicalPlanCoverage>, ErrorModel> errorListAllyCoverage = (resultListCoverageDAO as ResultError<List<ParamTechnicalPlanCoverage>, ErrorModel>);
                                    return new TechnicalPlanServiceModel() { ErrorServiceModel = new MODPA.ErrorServiceModel() { ErrorDescription = errorListAllyCoverage.Message.ErrorDescription, ErrorTypeService = (MODEN.ErrorTypeService)errorListAllyCoverage.Message.ErrorType } };
                                }
                            }
                            Result<ParamTechnicalPlanCoverage, ErrorModel> resultCoverage;
                            ResultError<ParamTechnicalPlanCoverage, ErrorModel> errorCoverage;
                            resultCoverage = ServicesModelsAssembler.CreateParamTechnicalPlanCoverage(coverage);
                            if (resultCoverage is ResultError<ParamTechnicalPlanCoverage, ErrorModel>)
                            {
                                errorCoverage = (resultCoverage as ResultError<ParamTechnicalPlanCoverage, ErrorModel>);
                                return new TechnicalPlanServiceModel() { ErrorServiceModel = new MODPA.ErrorServiceModel() { ErrorDescription = errorCoverage.Message.ErrorDescription, ErrorTypeService = (MODEN.ErrorTypeService)errorCoverage.Message.ErrorType } };
                            }
                            ParamTechnicalPlanCoverage paramCoverage = (resultCoverage as ResultValue<ParamTechnicalPlanCoverage, ErrorModel>).Value;
                            resultCoverageDAO = technicalPlanDAO.DeleteParamTechnicalPlanCoverage(technicalPlanObject.Id, paramCoverage);
                            if (resultCoverageDAO is ResultError<ParamTechnicalPlanCoverage, ErrorModel>)
                            {
                                errorCoverage = (resultCoverageDAO as ResultError<ParamTechnicalPlanCoverage, ErrorModel>);
                                return new TechnicalPlanServiceModel() { ErrorServiceModel = new MODPA.ErrorServiceModel() { ErrorDescription = errorCoverage.Message.ErrorDescription, ErrorTypeService = (MODEN.ErrorTypeService)errorCoverage.Message.ErrorType } };
                            }
                        }
                        resultDAO = technicalPlanDAO.DeleteParamTechnicalPlan(technicalPlanObject);
                        if (resultDAO is ResultError<ParamTechnicalPlan, ErrorModel>)
                        {
                            ResultError<ParamTechnicalPlan, ErrorModel> errorData = (resultDAO as ResultError<ParamTechnicalPlan, ErrorModel>);
                            return new TechnicalPlanServiceModel() { ErrorServiceModel = new MODPA.ErrorServiceModel() { ErrorDescription = errorData.Message.ErrorDescription, ErrorTypeService = (MODEN.ErrorTypeService)errorData.Message.ErrorType } };
                        }
                        processCoverages = false;
                        break;
                }
                if (processCoverages)
                {
                    foreach (TechnicalPlanCoverageServiceRelationModel coverage in technicalPlan.TechnicalPlanCoverages)
                    {
                        Result<ParamTechnicalPlanCoverage, ErrorModel> resultCoverage;
                        ResultError<ParamTechnicalPlanCoverage, ErrorModel> errorCoverage;
                        resultCoverage = ServicesModelsAssembler.CreateParamTechnicalPlanCoverage(coverage);
                        if (resultCoverage is ResultError<ParamTechnicalPlanCoverage, ErrorModel>)
                        {
                            errorCoverage = (resultCoverage as ResultError<ParamTechnicalPlanCoverage, ErrorModel>);
                            return new TechnicalPlanServiceModel() { ErrorServiceModel = new MODPA.ErrorServiceModel() { ErrorDescription = errorCoverage.Message.ErrorDescription, ErrorTypeService = (MODEN.ErrorTypeService)errorCoverage.Message.ErrorType } };
                        }
                        ParamTechnicalPlanCoverage paramCoverage = (resultCoverage as ResultValue<ParamTechnicalPlanCoverage, ErrorModel>).Value;
                        switch (coverage.StatusTypeService)
                        {
                            case MODEN.StatusTypeService.Create:
                                resultCoverageDAO = technicalPlanDAO.CreateParamTechnicalPlanCoverage(technicalPlanId, paramCoverage);
                                if (resultCoverageDAO is ResultError<ParamTechnicalPlanCoverage, ErrorModel>)
                                {
                                    errorCoverage = (resultCoverageDAO as ResultError<ParamTechnicalPlanCoverage, ErrorModel>);
                                    return new TechnicalPlanServiceModel() { ErrorServiceModel = new MODPA.ErrorServiceModel() { ErrorDescription = errorCoverage.Message.ErrorDescription, ErrorTypeService = (MODEN.ErrorTypeService)errorCoverage.Message.ErrorType } };
                                }
                                break;
                            case MODEN.StatusTypeService.Update:
                                resultCoverageDAO = technicalPlanDAO.UpdateParamTechnicalPlanCoverage(technicalPlanId, paramCoverage);
                                if (resultCoverageDAO is ResultError<ParamTechnicalPlanCoverage, ErrorModel>)
                                {
                                    errorCoverage = (resultCoverageDAO as ResultError<ParamTechnicalPlanCoverage, ErrorModel>);
                                    return new TechnicalPlanServiceModel() { ErrorServiceModel = new MODPA.ErrorServiceModel() { ErrorDescription = errorCoverage.Message.ErrorDescription, ErrorTypeService = (MODEN.ErrorTypeService)errorCoverage.Message.ErrorType } };
                                }
                                break;
                            case MODEN.StatusTypeService.Delete:
                                resultCoverageDAO = technicalPlanDAO.DeleteParamTechnicalPlanCoverage(technicalPlanId, paramCoverage);
                                if (resultCoverageDAO is ResultError<ParamTechnicalPlanCoverage, ErrorModel>)
                                {
                                    errorCoverage = (resultCoverageDAO as ResultError<ParamTechnicalPlanCoverage, ErrorModel>);
                                    return new TechnicalPlanServiceModel() { ErrorServiceModel = new MODPA.ErrorServiceModel() { ErrorDescription = errorCoverage.Message.ErrorDescription, ErrorTypeService = (MODEN.ErrorTypeService)errorCoverage.Message.ErrorType } };
                                }
                                break;
                        }

                        if (coverage.AlliedCoverages != null)
                        {
                            foreach (AllyCoverageServiceModel ally in coverage.AlliedCoverages)
                            {
                                resultCoverage = ServicesModelsAssembler.CreateParamTechnicalPlanCoverage(coverage, ally);
                                if (resultCoverage is ResultError<ParamTechnicalPlanCoverage, ErrorModel>)
                                {
                                    errorCoverage = (resultCoverage as ResultError<ParamTechnicalPlanCoverage, ErrorModel>);
                                    return new TechnicalPlanServiceModel() { ErrorServiceModel = new MODPA.ErrorServiceModel() { ErrorDescription = errorCoverage.Message.ErrorDescription, ErrorTypeService = (MODEN.ErrorTypeService)errorCoverage.Message.ErrorType } };
                                }
                                ParamTechnicalPlanCoverage paramAllyCoverage = (resultCoverage as ResultValue<ParamTechnicalPlanCoverage, ErrorModel>).Value;
                                switch (ally.StatusTypeService)
                                {
                                    case MODEN.StatusTypeService.Create:
                                        resultCoverageDAO = technicalPlanDAO.CreateParamTechnicalPlanCoverage(technicalPlanId, paramAllyCoverage);
                                        if (resultCoverageDAO is ResultError<ParamTechnicalPlanCoverage, ErrorModel>)
                                        {
                                            errorCoverage = (resultCoverageDAO as ResultError<ParamTechnicalPlanCoverage, ErrorModel>);
                                            return new TechnicalPlanServiceModel() { ErrorServiceModel = new MODPA.ErrorServiceModel() { ErrorDescription = errorCoverage.Message.ErrorDescription, ErrorTypeService = (MODEN.ErrorTypeService)errorCoverage.Message.ErrorType } };
                                        }
                                        break;
                                    case MODEN.StatusTypeService.Update:
                                        resultCoverageDAO = technicalPlanDAO.UpdateParamTechnicalPlanCoverage(technicalPlanId, paramAllyCoverage);
                                        if (resultCoverageDAO is ResultError<ParamTechnicalPlanCoverage, ErrorModel>)
                                        {
                                            errorCoverage = (resultCoverageDAO as ResultError<ParamTechnicalPlanCoverage, ErrorModel>);
                                            return new TechnicalPlanServiceModel() { ErrorServiceModel = new MODPA.ErrorServiceModel() { ErrorDescription = errorCoverage.Message.ErrorDescription, ErrorTypeService = (MODEN.ErrorTypeService)errorCoverage.Message.ErrorType } };
                                        }
                                        break;
                                    case MODEN.StatusTypeService.Delete:
                                        resultCoverageDAO = technicalPlanDAO.DeleteParamTechnicalPlanCoverage(technicalPlanId, paramAllyCoverage);
                                        if (resultCoverageDAO is ResultError<ParamTechnicalPlanCoverage, ErrorModel>)
                                        {
                                            errorCoverage = (resultCoverageDAO as ResultError<ParamTechnicalPlanCoverage, ErrorModel>);
                                            return new TechnicalPlanServiceModel() { ErrorServiceModel = new MODPA.ErrorServiceModel() { ErrorDescription = errorCoverage.Message.ErrorDescription, ErrorTypeService = (MODEN.ErrorTypeService)errorCoverage.Message.ErrorType } };
                                        }
                                        break;
                                }
                            }
                        }
                    }
                }
                return technicalPlan;
            }
            else
            {
                TechnicalPlanServiceModel technicalPlanServiceModel = new TechnicalPlanServiceModel();
                ErrorServiceModel errorServiceModel = new ErrorServiceModel();
                List<string> errorLst = new List<string>();
                errorLst.Add(Resources.Errors.ErrorAlreadyExistDescription);
                errorServiceModel.ErrorTypeService = MODEN.ErrorTypeService.BusinessFault;
                errorServiceModel.ErrorDescription = errorLst;
                technicalPlanServiceModel.ErrorServiceModel = errorServiceModel;
                technicalPlanServiceModel.StatusTypeService = StatusTypeService.Error;
                return technicalPlanServiceModel;
                               

            }
            
        }

        public TechnicalPlansServiceModel GetTechnicalPlansServiceModel()
        {
            TechnicalPlanDAO technicalPlanDAO = new TechnicalPlanDAO();
            TechnicalPlansServiceModel technicalPlansServiceModel = new TechnicalPlansServiceModel();
            UTIER.Result<List<ParamTechnicalPlanDTO>, UTIER.ErrorModel> result = technicalPlanDAO.GetParametrizationTechnicalPlans();

            if (result is UTIER.ResultError<List<ParamTechnicalPlanDTO>, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<List<ParamTechnicalPlanDTO>, UTIER.ErrorModel>).Message;
                technicalPlansServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                technicalPlansServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<List<ParamTechnicalPlanDTO>, UTIER.ErrorModel>)
            {
                List<ParamTechnicalPlanDTO> listParamDto = (result as UTIER.ResultValue<List<ParamTechnicalPlanDTO>, UTIER.ErrorModel>).Value;
                technicalPlansServiceModel.TechnicalPlanServiceModel = ModelsServicesAssembler.CreateTechnicalPlansServiceModel(listParamDto);
            }
            return technicalPlansServiceModel;            
        }

        public MODPA.ExcelFileServiceModel GenerateFileToTechnicalPlan(List<TechnicalPlanServiceModel> technicalPlans, string fileName)
        {
            TechnicalPlanDAO technicalPlanDAO = new TechnicalPlanDAO();
            MODPA.ExcelFileServiceModel excelFileServiceModel = new MODPA.ExcelFileServiceModel();

            Result<List<ParamTechnicalPlanDTO>, ErrorModel> resultFile = ServicesModelsAssembler.CreateParamTechnicalPlans(technicalPlans);
            if (resultFile is ResultError<List<ParamTechnicalPlanDTO>, ErrorModel>)
            {
                UTIER.ErrorModel errorFileResult = (resultFile as ResultError<List<ParamTechnicalPlanDTO>, ErrorModel>).Message;
                excelFileServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorFileResult.ErrorType;
                excelFileServiceModel.ErrorDescription = errorFileResult.ErrorDescription;
            }
            UTIER.Result<string, UTIER.ErrorModel> result = technicalPlanDAO.GenerateFileToTechnicalPlan((resultFile as ResultValue<List<ParamTechnicalPlanDTO>, ErrorModel>).Value, fileName);
            if (result is UTIER.ResultError<string, UTIER.ErrorModel>)
            {
                UTIER.ErrorModel errorModelResult = (result as UTIER.ResultError<string, UTIER.ErrorModel>).Message;
                excelFileServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                excelFileServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIER.ResultValue<string, UTIER.ErrorModel>)
            {
                excelFileServiceModel.FileData = (result as UTIER.ResultValue<string, UTIER.ErrorModel>).Value;
            }
            return excelFileServiceModel;            
        }
        #endregion
                
    }
}
