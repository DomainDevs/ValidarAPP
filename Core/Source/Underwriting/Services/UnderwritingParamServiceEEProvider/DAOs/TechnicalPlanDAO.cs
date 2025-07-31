using Sistran.Core.Application.Quotation.Entities;
using Sistran.Core.Application.UnderwritingParamService.Models;
using Sistran.Core.Application.UnderwritingParamServices.EEProvider.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Enums;
using Sistran.Core.Application.Utilities.Error;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using ENUMUT = Sistran.Core.Application.Utilities.Enums;
using PRODEN = Sistran.Core.Application.Product.Entities;
using QUOEN = Sistran.Core.Application.Quotation.Entities;
using UPENTV = Sistran.Core.Application.UnderwritingParamService.EEProvider.Entities.Views;
using UTMO = Sistran.Core.Application.Utilities.Error;
using Sistran.Core.Application.UnderwritingParamService.EEProvider.Assemblers;
using Sistran.Core.Services.UtilitiesServices.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Application.UtilitiesServicesEEProvider.DAOs;
using UNVIEW = Sistran.Core.Application.UnderwritingServices.EEProvider.Entities.Views;

namespace Sistran.Core.Application.UnderwritingParamService.EEProvider.DAOs
{
    public class TechnicalPlanDAO
    {
        public UTMO.Result<List<ParamTechnicalPlan>, UTMO.ErrorModel> GetTechnicalPlan(string description)
        {
            return GetTechnicalPlan(description, 0, false);
        }

        public UTMO.Result<List<ParamTechnicalPlan>, UTMO.ErrorModel> GetTechnicalPlan(string description, int coveredRiskType, bool shortDescription = true)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            Result<ParamCoveredRiskType, ErrorModel> resultCoveredRiskType;
            Result<ParamTechnicalPlan, ErrorModel> resultTechnicalPlan;
            ResultError<ParamTechnicalPlan, ErrorModel> resultTechnicalPlanError;
            List<ParamTechnicalPlan> technicalPlanResult;
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                if (coveredRiskType > 0)
                {
                    filter.PropertyEquals(PRODEN.TechnicalPlan.Properties.CoveredRiskTypeCode, typeof(PRODEN.TechnicalPlan).Name, coveredRiskType);
                    if (!string.IsNullOrEmpty(description))
                    {
                        filter.And();
                        filter.OpenParenthesis();
                        filter.Property(PRODEN.TechnicalPlan.Properties.SmallDescription, typeof(PRODEN.TechnicalPlan).Name).Like().Constant("%" + description + "%");
                        if (!shortDescription)
                        {
                            filter.Or();
                            filter.Property(PRODEN.TechnicalPlan.Properties.Description, typeof(PRODEN.TechnicalPlan).Name).Like().Constant("%" + description + "%");
                        }
                        filter.CloseParenthesis();
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(description))
                    {
                        filter.Property(PRODEN.TechnicalPlan.Properties.SmallDescription, typeof(PRODEN.TechnicalPlan).Name).Like().Constant("%" + description + "%");
                        filter.Or();
                        filter.Property(PRODEN.TechnicalPlan.Properties.Description, typeof(PRODEN.TechnicalPlan).Name).Like().Constant("%" + description + "%");
                    }
                }

                BusinessCollection coveredRiskTypes = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(Parameters.Entities.CoveredRiskType)));
                BusinessCollection technicalPlans = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(PRODEN.TechnicalPlan), filter.GetPredicate()));
               
                if (technicalPlans.Count > 0)
                {
                    var listTechnicalPlans = technicalPlans.Cast<PRODEN.TechnicalPlan>()
                            .Join(coveredRiskTypes.Cast<Parameters.Entities.CoveredRiskType>(), x => x.CoveredRiskTypeCode, y => y.CoveredRiskTypeCode,
                            (tpl, tpcrt) => new {
                                tpl.TechnicalPlanId,
                                tpl.Description,
                                tpl.SmallDescription,
                                tpl.CurrentFrom,
                                tpl.CurrentTo,
                                tpcrt.CoveredRiskTypeCode,
                                CoveredRiskTypeDescription = tpcrt.SmallDescription
                            });

                    technicalPlanResult = new List<ParamTechnicalPlan>();

                    foreach (var item in listTechnicalPlans)
                    {
                        resultCoveredRiskType = ParamCoveredRiskType.CreateParamCoveredRiskType(item.CoveredRiskTypeCode, item.CoveredRiskTypeDescription);
                        if (resultCoveredRiskType is ResultError<ParamCoveredRiskType, ErrorModel>)
                        {
                            ResultError<ParamCoveredRiskType, ErrorModel> resultCoveredRiskTypeError = (resultCoveredRiskType as ResultError<ParamCoveredRiskType, ErrorModel>);
                            return new ResultError<List<ParamTechnicalPlan>, ErrorModel>(ErrorModel.CreateErrorModel(resultCoveredRiskTypeError.Message.ErrorDescription, resultCoveredRiskTypeError.Message.ErrorType, resultCoveredRiskTypeError.Message.Exception));
                        }
                        ParamCoveredRiskType coveredRiskTypeResult = (resultCoveredRiskType as ResultValue<ParamCoveredRiskType, ErrorModel>).Value;
                        resultTechnicalPlan = ParamTechnicalPlan.CreateParamTechnicalPlan(item.TechnicalPlanId, item.Description.ToUpper(), item.SmallDescription.ToUpper(), coveredRiskTypeResult, item.CurrentFrom, item.CurrentTo);
                        if (resultTechnicalPlan is ResultError<ParamTechnicalPlan, ErrorModel>)
                        {
                            resultTechnicalPlanError = (resultTechnicalPlan as ResultError<ParamTechnicalPlan, ErrorModel>);
                            return new ResultError<List<ParamTechnicalPlan>, ErrorModel>(ErrorModel.CreateErrorModel(resultTechnicalPlanError.Message.ErrorDescription, resultTechnicalPlanError.Message.ErrorType, resultTechnicalPlanError.Message.Exception));
                        }
                        ParamTechnicalPlan technicalPlan = (resultTechnicalPlan as ResultValue<ParamTechnicalPlan, ErrorModel>).Value;
                        technicalPlanResult.Add(technicalPlan);
                    }
                    return new UTMO.ResultValue<List<ParamTechnicalPlan>, UTMO.ErrorModel>(technicalPlanResult);
                }
                else
                {
                    return new ResultError<List<ParamTechnicalPlan>, ErrorModel>(ErrorModel.CreateErrorModel(new List<string>() { Resources.Errors.TechnicalPlanNotFound}, ErrorType.NotFound, null));
                }
            }
            catch (Exception ex)
            {
                return new ResultError<List<ParamTechnicalPlan>, ErrorModel>(ErrorModel.CreateErrorModel(new List<string>() { Resources.Errors.TechnicalPlanTechnicalError }, ErrorType.TechnicalFault, ex));
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingParamService.EEProvider.DAOs.GetTechnicalPlan");
            }

        }

        public UTMO.Result<List<ParamInsuredObject>, UTMO.ErrorModel> GetInsuredObjectsByCoveredRiskType(int coveredRiskType)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                if (coveredRiskType > 0)
                {
                    filter.Property(Parameters.Entities.CoveredRiskType.Properties.CoveredRiskTypeCode, typeof(Parameters.Entities.CoveredRiskType).Name);
                    filter.Equal();
                    filter.Constant(coveredRiskType);
                }
                UPENTV.InsuredObjectCoveredRiskTypeView view = new UPENTV.InsuredObjectCoveredRiskTypeView();
                ViewBuilder builder = new ViewBuilder("InsuredObjectCoveredRiskTypeView");

                builder.Filter = filter.GetPredicate();
                DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
                List<ParamInsuredObject> insuredObjects = new List<ParamInsuredObject>();
                if (view.InsuredObjects.Count > 0)
                {
                    insuredObjects = ModelAssembler.CreateCompanyInsuredObjects(view.InsuredObjects);
                    return new ResultValue<List<ParamInsuredObject>, ErrorModel>(insuredObjects);
                }
                else
                {
                    return new ResultError<List<ParamInsuredObject>, ErrorModel>(ErrorModel.CreateErrorModel(new List<string>() { Resources.Errors.InsuredObjectNotFound}, ErrorType.NotFound, null));
                }
            }
            catch (Exception ex)
            {
                return new ResultError<List<ParamInsuredObject>, ErrorModel>(ErrorModel.CreateErrorModel(new List<string>() { Resources.Errors.InsuredObjectTechnicalError}, ErrorType.TechnicalFault, ex));
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingParamService.EEProvider.DAOs.GetInsuredObjectsByCoveredRiskType");
            }      
        }


        public UTMO.Result<List<ParamCoverage>, UTMO.ErrorModel> GetCoverageByInsuredObjects(int insuredObjectId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(Coverage.Properties.InsuredObjectId, typeof(Coverage).Name);
                filter.Equal();
                filter.Constant(insuredObjectId);
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(Coverage), filter.GetPredicate()));
                List<ParamCoverage> paramCoverages = ModelAssembler.CreateParamCoverages(businessCollection);

                if (paramCoverages.Count > 0)
                {
                    return new ResultValue<List<ParamCoverage>, UTMO.ErrorModel>(paramCoverages);
                }
                else
                {
                    return new ResultError<List<ParamCoverage>, ErrorModel>(ErrorModel.CreateErrorModel(new List<string>() { Resources.Errors.TechnicalPlanCoverageNotFound}, ErrorType.NotFound, null));
                }
            }
            catch (Exception ex)
            {
                return new ResultError<List<ParamCoverage>, ErrorModel>(ErrorModel.CreateErrorModel(new List<string>() { Resources.Errors.TechnicalPlanCoverageTechnicalError}, ErrorType.TechnicalFault, ex));
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingParamService.EEProvider.DAOs.GetCoverageByInsuredObjects");
            }            
        }

        public UTMO.Result<List<ParamAllyCoverage>, UTMO.ErrorModel> GetAlliedCoverages(int coverageId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            Result<List<ParamAllyCoverage>, ErrorModel> result;       
            
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(QUOEN.AllyCoverage.Properties.CoverageId, typeof(QUOEN.AllyCoverage).Name);
                filter.Equal();
                filter.Constant(coverageId);
                UNVIEW.CoverageAlliedView view = new UNVIEW.CoverageAlliedView();
                ViewBuilder builder = new ViewBuilder("CoverageAlliedView");
                builder.Filter = filter.GetPredicate();
                DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

                if (view.CoverageAllied.Count > 0)
                {
                    result = ModelAssembler.CreateAllyParamCoverages(view.Coverages, view.CoverageAllied);
                    if (result is ResultError<List<ParamAllyCoverage>, ErrorModel>)
                    {
                        return result;
                    }
                    else
                    {
                        List<ParamAllyCoverage> resultValue = (result as ResultValue<List<ParamAllyCoverage>, ErrorModel>).Value;
                        if (resultValue.Count == 0)
                        {
                            return new ResultError<List<ParamAllyCoverage>, ErrorModel>(ErrorModel.CreateErrorModel(new List<string>() { Resources.Errors.TechnicalPlanAllyCoverageNotFound}, ErrorType.NotFound, null));
                        }
                        else
                        {
                            return result;
                        }
                    }
                }
                else
                {
                    return new ResultError<List<ParamAllyCoverage>, ErrorModel>(ErrorModel.CreateErrorModel(new List<string>() { Resources.Errors.TechnicalPlanAllyCoverageNotFound}, ErrorType.NotFound, null));
                }                                
            }
            catch (Exception ex)
            {
                return new UTMO.ResultError<List<ParamAllyCoverage>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(new List<string>() { Resources.Errors.TechnicalPlanAllyCoverageTechnicalError }, ENUMUT.ErrorType.TechnicalFault, ex));
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingParamService.EEProvider.DAOs.GetAlliedCoverages");
            }   
            


        }

        public UTMO.Result<List<ParamTechnicalPlansCoverage>, UTMO.ErrorModel> GetCoveragesByTechnicalPlanId(int technicalPlanId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorList = new List<string>();
            Result<ParamTechnicalPlanCoverage, ErrorModel> resultCoverage;
            Result<ParamTechnicalPlansCoverage, ErrorModel> resultCoveragesPlan;
            ResultError<ParamTechnicalPlanCoverage, ErrorModel> errorCoverage;
            ResultError<ParamTechnicalPlansCoverage, ErrorModel> errorCoveragesPlan;
            ResultError<ParamAllyCoverage, ErrorModel> errorAlly;
            List<ParamTechnicalPlansCoverage> resultData = new List<ParamTechnicalPlansCoverage>();
            ParamTechnicalPlansCoverage coveragesItem;
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(PRODEN.TechnicalPlanCoverage.Properties.TechnicalPlanId, typeof(PRODEN.TechnicalPlanCoverage).Name);
                filter.Equal();
                filter.Constant(technicalPlanId);

                UPENTV.TechnicalPlanCoveragesView view = new UPENTV.TechnicalPlanCoveragesView();
                ViewBuilder builder = new ViewBuilder("TechnicalPlanCoveragesView");
                builder.Filter = filter.GetPredicate();
                DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

                if (view.TechnicalPlanCoverages.Count > 0)
                {
                    List<PRODEN.TechnicalPlanCoverage> listAllyCoverages = view.TechnicalPlanCoverages.Cast<PRODEN.TechnicalPlanCoverage>()
                        .Join(view.TechnicalPlanRelatedAllyCoverages.Cast<AllyCoverage>(), x => x.CoverageId, y => y.AllyCoverageId, (cov, coval) => new
                        { Coverages = cov }).Select(z => z.Coverages).ToList();

                    List<PRODEN.TechnicalPlanCoverage> listCoverages = view.TechnicalPlanCoverages.Cast<PRODEN.TechnicalPlanCoverage>().Except(listAllyCoverages).ToList(); 



                    foreach (PRODEN.TechnicalPlanCoverage item in listCoverages)
                    {
                        ParamTechnicalPlanCoverage paramTechnicalPlanCoverage;                        
                        Coverage coverage = view.TechnicalPlanRelatedCoverages.Cast<Coverage>().Where(x => x.CoverageId == item.CoverageId).FirstOrDefault();
                        Coverage mainCoverage = null;
                        if (coverage == null)
                        {
                            return new ResultError<List<ParamTechnicalPlansCoverage>, ErrorModel>(ErrorModel.CreateErrorModel(new List<string>() { Resources.Errors.ErrorGetCoverage}, ErrorType.TechnicalFault, null));
                        }
                        InsuredObject insuredObject = view.TechnicalPlanInsuredObjects.Cast<InsuredObject>().Where(x => x.InsuredObjectId == coverage.InsuredObjectId).FirstOrDefault();
                        if (insuredObject == null)
                        {
                            return new ResultError<List<ParamTechnicalPlansCoverage>, ErrorModel>(ErrorModel.CreateErrorModel(new List<string>() { Resources.Errors.ErrorGetInsuredObject}, ErrorType.TechnicalFault, null));
                        }
                        if (item.MainCoverageId > 0)
                        {
                            mainCoverage = view.TechnicalPlanPrincipalCoverages.Cast<Coverage>().Where(x => x.CoverageId == item.MainCoverageId).FirstOrDefault();
                            if (mainCoverage == null)
                            {
                                return new ResultError<List<ParamTechnicalPlansCoverage>, ErrorModel>(ErrorModel.CreateErrorModel(new List<string>() { Resources.Errors.ErrorGetCoverage}, ErrorType.TechnicalFault, null));
                            }
                        }
                        ParamInsuredObject paramInsuredObject = ModelAssembler.CreateParamInsuredObject(insuredObject);
                        ParamCoverage paramCoverage = ModelAssembler.CreateParamCoverage(coverage);
                        ParamCoverage paramMainCoverage = null;
                        if (mainCoverage != null)
                        {
                            paramMainCoverage = ModelAssembler.CreateParamCoverage(mainCoverage);

                        }
                        resultCoverage = ParamTechnicalPlanCoverage.CreateParamTechnicalPlanCoverage(paramInsuredObject, paramCoverage, paramMainCoverage, item.MainCoveragePercentage);

                        if (resultCoverage is ResultError<ParamTechnicalPlanCoverage, ErrorModel>)
                        {
                            errorCoverage = (resultCoverage as ResultError<ParamTechnicalPlanCoverage, ErrorModel>);
                            return new ResultError<List<ParamTechnicalPlansCoverage>, ErrorModel>(ErrorModel.CreateErrorModel(errorCoverage.Message.ErrorDescription, errorCoverage.Message.ErrorType, errorCoverage.Message.Exception));
                        }
                        else
                        {
                            paramTechnicalPlanCoverage = (resultCoverage as ResultValue<ParamTechnicalPlanCoverage, ErrorModel>).Value;
                        }
                        List<ParamAllyCoverage> TechnicalPlanAllyCoverages = new List<ParamAllyCoverage>();
                        List<PRODEN.TechnicalPlanCoverage> allyCoverages = listAllyCoverages.Where(x => x.MainCoverageId == item.CoverageId).ToList();
                        foreach (var ally in allyCoverages)
                        {
                            Coverage allyCoverageRelated = view.TechnicalPlanAllyCoverages.Cast<Coverage>().Where(x => x.CoverageId == ally.CoverageId).FirstOrDefault();
                            Result<ParamAllyCoverage, ErrorModel> resultAlly = ParamAllyCoverage.CreateParamAllyCoverage(ally.CoverageId, allyCoverageRelated.PrintDescription, ally.MainCoveragePercentage);
                            if (resultAlly is ResultError<ParamAllyCoverage, ErrorModel>)
                            {
                                errorAlly = (resultAlly as ResultError<ParamAllyCoverage, ErrorModel>);
                                return new ResultError<List<ParamTechnicalPlansCoverage>, ErrorModel>(ErrorModel.CreateErrorModel(errorAlly.Message.ErrorDescription, errorAlly.Message.ErrorType, errorAlly.Message.Exception));
                            }
                            ParamAllyCoverage paramAllyCoverage = (resultAlly as ResultValue<ParamAllyCoverage, ErrorModel>).Value;
                            TechnicalPlanAllyCoverages.Add(paramAllyCoverage); 
                        }

                        resultCoveragesPlan = ParamTechnicalPlansCoverage.CreateParamTechnicalPlansCoverage(paramTechnicalPlanCoverage, TechnicalPlanAllyCoverages);
                        if (resultCoveragesPlan is ResultError<ParamTechnicalPlansCoverage, ErrorModel>)
                        {
                            errorCoveragesPlan = (resultCoveragesPlan as ResultError<ParamTechnicalPlansCoverage, ErrorModel>);
                            return new ResultError<List<ParamTechnicalPlansCoverage>, ErrorModel>(ErrorModel.CreateErrorModel(errorCoveragesPlan.Message.ErrorDescription, errorCoveragesPlan.Message.ErrorType, errorCoveragesPlan.Message.Exception));
                        }
                        else
                        {
                            coveragesItem = (resultCoveragesPlan as ResultValue<ParamTechnicalPlansCoverage, ErrorModel>).Value;
                        }
                        resultData.Add(coveragesItem);
                    }
                    return new ResultValue<List<ParamTechnicalPlansCoverage>, UTMO.ErrorModel>(resultData);
                }
                else
                {
                    return new ResultError<List<ParamTechnicalPlansCoverage>, ErrorModel>(ErrorModel.CreateErrorModel(new List<string>() { Resources.Errors.TechnicalPlanCoverageNotFound}, ErrorType.NotFound, null));
                }                
            }
            catch (Exception ex)
            {
                return new UTMO.ResultError<List<ParamTechnicalPlansCoverage>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(new List<string>() { Resources.Errors.TechnicalPlanCoverageTechnicalError }, ENUMUT.ErrorType.TechnicalFault, ex));
            }
            finally {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingParamService.EEProvider.DAOs.GetCoveragesByTechnicalPlanId");
            }
        }


        /// <summary>
        /// Creacion de Plan Técnico
        /// </summary>
        /// <param name="paramTechnicalPlan">Plan Técnico a crear</param>
        /// <returns>Plan Técnico Creado</returns>
        public UTMO.Result<ParamTechnicalPlan, UTMO.ErrorModel> CreateParamTechnicalPlan(ParamTechnicalPlan paramTechnicalPlan)
        {            
            try
            {
                PRODEN.TechnicalPlan technicalPlan = EntityAssembler.CreateTechnicalPlan(paramTechnicalPlan);
                DataFacadeManager.Instance.GetDataFacade().InsertObject(technicalPlan);
                return ModelAssembler.CreateParamTechnicalPlan(technicalPlan);                
            }
            catch (System.Exception ex)
            {
                return new UTMO.ResultError<ParamTechnicalPlan, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(new List<string>() { Resources.Errors.ErrorCreateTechnicalPlan }, ENUMUT.ErrorType.TechnicalFault, ex));  //pendiente
            }
        }

        /// <summary>
        /// Actualizacion de Plan Técnico
        /// </summary>
        /// <param name="paramTechnicalPlan">Plan Técnico a actualizar</param>
        /// <returns>Plan Técnico actualizado</returns>
        public UTMO.Result<ParamTechnicalPlan, UTMO.ErrorModel> UpdateParamTechnicalPlan(ParamTechnicalPlan paramTechnicalPlan)
        {
            try
            {
                PrimaryKey key = PRODEN.TechnicalPlan.CreatePrimaryKey(paramTechnicalPlan.Id);
                PRODEN.TechnicalPlan technicalPlanEntity = (PRODEN.TechnicalPlan)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                technicalPlanEntity.Description = paramTechnicalPlan.Description;
                technicalPlanEntity.SmallDescription = paramTechnicalPlan.SmallDescription;
                technicalPlanEntity.CoveredRiskTypeCode = paramTechnicalPlan.CoveredRiskType.Id;
                technicalPlanEntity.CurrentFrom = paramTechnicalPlan.CurrentFrom;
                technicalPlanEntity.CurrentTo = paramTechnicalPlan.CurrentTo;                
                DataFacadeManager.Instance.GetDataFacade().UpdateObject(technicalPlanEntity);
                return ModelAssembler.CreateParamTechnicalPlan(technicalPlanEntity);                
            }
            catch (System.Exception ex)
            {
                return new UTMO.ResultError<ParamTechnicalPlan, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(new List<string>() { Resources.Errors.ErrorUpdateTechnicalPlan }, ENUMUT.ErrorType.TechnicalFault, ex)); //pendiente
            }
        }

        /// <summary>
        /// Eliminacion del Plan Técnico
        /// </summary>
        /// <param name="paramTechnicalPlan">Plan Técnico a eliminar</param>        
        /// <returns>Plan Técnico eliminado</returns>
        public UTMO.Result<ParamTechnicalPlan, UTMO.ErrorModel> DeleteParamTechnicalPlan(ParamTechnicalPlan paramTechnicalPlan)
        {
            try
            {
                PrimaryKey key = PRODEN.TechnicalPlan.CreatePrimaryKey(paramTechnicalPlan.Id);
                PRODEN.TechnicalPlan technicalPlanEntity = (PRODEN.TechnicalPlan)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);               
                DataFacadeManager.Instance.GetDataFacade().DeleteObject(technicalPlanEntity);
                return ModelAssembler.CreateParamTechnicalPlan(technicalPlanEntity);
            }
            catch (System.Exception ex)
            {
                return new UTMO.ResultError<ParamTechnicalPlan, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(new List<string>() { Resources.Errors.ErrorDeleteTechnicalPlan }, ENUMUT.ErrorType.TechnicalFault, ex)); //pendiente
            }
        }

        /// <summary>
        /// Creacion de Cobertura del Plan Técnico
        /// </summary>
        /// <param name="paramTechnicalPlanCoverage">Cobertura del Plan Técnico a crear</param>
        /// <returns>Cobertura del Plan Técnico Creada</returns>
        public UTMO.Result<ParamTechnicalPlanCoverage, UTMO.ErrorModel> CreateParamTechnicalPlanCoverage(int technicalPlanId, ParamTechnicalPlanCoverage paramTechnicalPlanCoverage)
        {
            try
            {
                PRODEN.TechnicalPlanCoverage technicalPlanCoverage = EntityAssembler.CreateTechnicalPlanCoverage(technicalPlanId, paramTechnicalPlanCoverage);                
                DataFacadeManager.Instance.GetDataFacade().InsertObject(technicalPlanCoverage);
                return ModelAssembler.CreateParamTechnicalPlanCoverage(technicalPlanCoverage);
            }
            catch (System.Exception ex)
            {
                return new UTMO.ResultError<ParamTechnicalPlanCoverage, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(new List<string>() { Resources.Errors.ErrorCreateTechnicalPlanCoverage }, ENUMUT.ErrorType.TechnicalFault, ex));  //pendiente
            }
        }

        /// <summary>
        /// Actualizacion de Cobertura Plan Técnico
        /// </summary>
        /// <param name="paramTechnicalPlanCoverage">Cobertura Plan Técnico a actualizar</param>
        /// <returns>Cobertura Plan Técnico actualizada</returns>
        public UTMO.Result<ParamTechnicalPlanCoverage, UTMO.ErrorModel> UpdateParamTechnicalPlanCoverage(int technicalPlanId, ParamTechnicalPlanCoverage paramTechnicalPlanCoverage)
        {
            try
            {
                PrimaryKey key = PRODEN.TechnicalPlanCoverage.CreatePrimaryKey(technicalPlanId, paramTechnicalPlanCoverage.Coverage.Id);
                PRODEN.TechnicalPlanCoverage technicalPlanCoverageEntity = (PRODEN.TechnicalPlanCoverage)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                technicalPlanCoverageEntity.IsSublimit = false;
                technicalPlanCoverageEntity.MainCoveragePercentage = null;
                technicalPlanCoverageEntity.MainCoverageId = null;
                if (paramTechnicalPlanCoverage.PrincipalCoverage != null)
                {
                    technicalPlanCoverageEntity.MainCoveragePercentage = paramTechnicalPlanCoverage.CoveragePercentage < 0 ? 0 : paramTechnicalPlanCoverage.CoveragePercentage;
                    if (paramTechnicalPlanCoverage.PrincipalCoverage.Id > 0)
                    {
                        technicalPlanCoverageEntity.MainCoverageId = paramTechnicalPlanCoverage.PrincipalCoverage.Id;
                    }
                }         
                DataFacadeManager.Instance.GetDataFacade().UpdateObject(technicalPlanCoverageEntity);
                return ModelAssembler.CreateParamTechnicalPlanCoverage(technicalPlanCoverageEntity);
            }
            catch (System.Exception ex)
            {
                return new UTMO.ResultError<ParamTechnicalPlanCoverage, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(new List<string>() { Resources.Errors.ErrorUpdateTechnicalPlanCoverage }, ENUMUT.ErrorType.TechnicalFault, ex)); //pendiente
            }
        }

        /// <summary>
        /// Eliminacion Cobertura del Plan Técnico
        /// </summary>
        /// <param name="paramTechnicalPlanCoverage">Cobertura Plan Técnico a eliminar</param>        
        /// <returns>Cobertura Plan Técnico eliminada</returns>
        public UTMO.Result<ParamTechnicalPlanCoverage, UTMO.ErrorModel> DeleteParamTechnicalPlanCoverage(int technicalPlanId, ParamTechnicalPlanCoverage paramTechnicalPlanCoverage)
        {
            try
            {
                PrimaryKey key = PRODEN.TechnicalPlanCoverage.CreatePrimaryKey(technicalPlanId, paramTechnicalPlanCoverage.Coverage.Id);
                PRODEN.TechnicalPlanCoverage technicalPlanCoverageEntity = (PRODEN.TechnicalPlanCoverage)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                DataFacadeManager.Instance.GetDataFacade().DeleteObject(technicalPlanCoverageEntity);
                return ModelAssembler.CreateParamTechnicalPlanCoverage(technicalPlanCoverageEntity);
            }
            catch (System.Exception ex)
            {
                return new UTMO.ResultError<ParamTechnicalPlanCoverage, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(new List<string>() { Resources.Errors.ErrorDeleteTechnicalPlanCoverage }, ENUMUT.ErrorType.TechnicalFault, ex)); //pendiente
            }
        }

        /// <summary>
        /// Eliminacion Lista de Coberturas del Plan Técnico
        /// </summary>
        /// <param name="technicalPlanId">Identificativo del Plan Técnico padre</param>        
        /// <param name="paramTechnicalPlanCoverages">Lista de Coberturas del Plan Técnico a eliminar</param>        
        /// <returns>Lista de Coberturas del Plan Técnico eliminada</returns>
        public UTMO.Result<List<ParamTechnicalPlanCoverage>, UTMO.ErrorModel> DeleteParamTechnicalPlanCoverages(int technicalPlanId, List<ParamTechnicalPlanCoverage> paramTechnicalPlanCoverages)
        {
            List<ParamTechnicalPlanCoverage> returnList = new List<ParamTechnicalPlanCoverage>();
            UTMO.Result<ParamTechnicalPlanCoverage, UTMO.ErrorModel> resultCoverage;
            UTMO.ResultError<ParamTechnicalPlanCoverage, UTMO.ErrorModel> errorCoverage;
            try
            {
                foreach (ParamTechnicalPlanCoverage coverage in paramTechnicalPlanCoverages)
                {
                    resultCoverage = DeleteParamTechnicalPlanCoverage(technicalPlanId, coverage);
                    if (resultCoverage is UTMO.ResultError<ParamTechnicalPlanCoverage, UTMO.ErrorModel>)
                    {
                        errorCoverage = (resultCoverage as UTMO.ResultError<ParamTechnicalPlanCoverage, UTMO.ErrorModel>);
                        return new UTMO.ResultError<List<ParamTechnicalPlanCoverage>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(errorCoverage.Message.ErrorDescription, errorCoverage.Message.ErrorType, errorCoverage.Message.Exception));
                    }
                    returnList.Add((resultCoverage as UTMO.ResultValue<ParamTechnicalPlanCoverage, UTMO.ErrorModel>).Value);
                }
                return new UTMO.ResultValue<List<ParamTechnicalPlanCoverage>, UTMO.ErrorModel>(returnList);
            }
            catch (System.Exception ex)
            {
                return new UTMO.ResultError<List<ParamTechnicalPlanCoverage>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(new List<string>() { Resources.Errors.ErrorDeleteTechnicalPlanCoverage }, ENUMUT.ErrorType.TechnicalFault, ex)); 
            }
        }

        /// <summary>
        /// Aceso a DB para consultar listado de planes técnicos
        /// </summary>
        /// <returns>Listado Result consulta en DB de Planes técnicoa</returns>
        public Result<List<ParamTechnicalPlanDTO>, ErrorModel> GetParametrizationTechnicalPlans()
        {
            Result<List<ParamTechnicalPlan>, ErrorModel> resultListTechnicalPlans;
            Result<List<ParamTechnicalPlansCoverage>, ErrorModel> resultListTechnicalPlansCoverage;
            Result<ParamTechnicalPlanDTO, ErrorModel> resultDTO;
            List<ParamTechnicalPlan> listTechnicalPlans;
            List<ParamTechnicalPlanDTO> listReturnData = new List<ParamTechnicalPlanDTO>();
            try
            {
                resultListTechnicalPlans = GetTechnicalPlan("");
                if (resultListTechnicalPlans is ResultError<List<ParamTechnicalPlan>, ErrorModel>)
                {
                    ResultError<List<ParamTechnicalPlan>, ErrorModel> errorListTechnicalPlans = (resultListTechnicalPlans as ResultError<List<ParamTechnicalPlan>, ErrorModel>);
                    return new ResultError<List<ParamTechnicalPlanDTO>, ErrorModel>(ErrorModel.CreateErrorModel(errorListTechnicalPlans.Message.ErrorDescription, errorListTechnicalPlans.Message.ErrorType, errorListTechnicalPlans.Message.Exception));
                }
                listTechnicalPlans = (resultListTechnicalPlans as ResultValue<List<ParamTechnicalPlan>, ErrorModel>).Value;

                foreach (ParamTechnicalPlan item in listTechnicalPlans)
                {
                    resultListTechnicalPlansCoverage = GetCoveragesByTechnicalPlanId(item.Id);
                    if (resultListTechnicalPlansCoverage is ResultError<List<ParamTechnicalPlansCoverage>, ErrorModel>)
                    {
                        ResultError<List<ParamTechnicalPlansCoverage>, ErrorModel> errorListTechnicalPlansCoverage = (resultListTechnicalPlansCoverage as ResultError<List<ParamTechnicalPlansCoverage>, ErrorModel>);
                        if (errorListTechnicalPlansCoverage.Message.ErrorType == ErrorType.NotFound)
                        {
                            resultDTO = ParamTechnicalPlanDTO.CreateParamTechnicalPlanDTO(item, new List<ParamTechnicalPlansCoverage>());
                            if (resultDTO is ResultError<ParamTechnicalPlanDTO, ErrorModel>)
                            {
                                ResultError<ParamTechnicalPlanDTO, ErrorModel> errorDTO = (resultDTO as ResultError<ParamTechnicalPlanDTO, ErrorModel>);
                                return new ResultError<List<ParamTechnicalPlanDTO>, ErrorModel>(ErrorModel.CreateErrorModel(errorDTO.Message.ErrorDescription, errorDTO.Message.ErrorType, errorDTO.Message.Exception));
                            }
                            listReturnData.Add((resultDTO as ResultValue<ParamTechnicalPlanDTO, ErrorModel>).Value);
                            continue;
                        }
                        return new ResultError<List<ParamTechnicalPlanDTO>, ErrorModel>(ErrorModel.CreateErrorModel(errorListTechnicalPlansCoverage.Message.ErrorDescription, errorListTechnicalPlansCoverage.Message.ErrorType, errorListTechnicalPlansCoverage.Message.Exception));
                    }
                    List<ParamTechnicalPlansCoverage> paramTechnicalPlanCoverages = (resultListTechnicalPlansCoverage as ResultValue<List<ParamTechnicalPlansCoverage>, ErrorModel>).Value;
                    resultDTO = ParamTechnicalPlanDTO.CreateParamTechnicalPlanDTO(item, paramTechnicalPlanCoverages);

                    if (resultDTO is ResultError<ParamTechnicalPlanDTO, ErrorModel>)
                    {
                        ResultError<ParamTechnicalPlanDTO, ErrorModel> errorDTO = (resultDTO as ResultError<ParamTechnicalPlanDTO, ErrorModel>);
                        return new ResultError<List<ParamTechnicalPlanDTO>, ErrorModel>(ErrorModel.CreateErrorModel(errorDTO.Message.ErrorDescription, errorDTO.Message.ErrorType, errorDTO.Message.Exception));
                    }

                    listReturnData.Add((resultDTO as ResultValue<ParamTechnicalPlanDTO, ErrorModel>).Value);
                }
                return new ResultValue<List<ParamTechnicalPlanDTO>, ErrorModel>(listReturnData);
            }
            catch (Exception ex)
            {
                return new ResultError<List<ParamTechnicalPlanDTO>, ErrorModel>(ErrorModel.CreateErrorModel(new List<string>() { Resources.Errors.TechnicalPlanTechnicalError }, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }


        /// <summary>
        /// Genera archivo excel de planes técnicos
        /// </summary>
        /// <param name="technicalPlans">listado de planes técnicos a exportar</param>
        /// <param name="fileName">Nombre de archivo</param>
        /// <returns>lista de planes técnicos - MOD-B</returns>
        public UTMO.Result<string, UTMO.ErrorModel> GenerateFileToTechnicalPlan(List<ParamTechnicalPlanDTO> technicalPlans, string fileName)
        {
            List<string> listErrors = new List<string>();
            try
            {
                FileProcessValue fileProcessValue = new FileProcessValue()
                {
                    Key1 = (int)FileProcessType.ParametrizationTechnicalPlan
                };
                FileDAO fileDAO = new FileDAO();
                File file = fileDAO.GetFileByFileProcessValue(fileProcessValue);

                if (file != null && file.IsEnabled)
                {
                    file.Name = fileName;
                    List<Row> rows = new List<Row>();

                   

                    foreach (ParamTechnicalPlanDTO item in technicalPlans)
                    {
                        var fields = file.Templates[0].Rows[0].Fields.OrderBy(x => x.Order).Select(x => new Field
                        {
                            ColumnSpan = x.ColumnSpan,
                            Description = x.Description,
                            FieldType = x.FieldType,
                            Id = x.Id,
                            IsEnabled = x.IsEnabled,
                            IsMandatory = x.IsMandatory,
                            Order = x.Order,
                            RowPosition = x.RowPosition,
                            SmallDescription = x.SmallDescription
                        }).ToList();

                        if (item.TechnicalPlanCoverages.Count > 0)
                        {
                            foreach (ParamTechnicalPlansCoverage coverage in item.TechnicalPlanCoverages)
                            {
                                if (coverage.TechnicalPlanAllyCoverages.Count > 0)
                                {
                                    foreach (ParamAllyCoverage ally in coverage.TechnicalPlanAllyCoverages)
                                    {
                                        fields[0].Value = item.TechnicalPlan.Description;
                                        fields[1].Value = item.TechnicalPlan.CoveredRiskType.SmallDescription;
                                        fields[2].Value = coverage.TechnicalPlanCoverage.InsuredObject.SmallDescription;
                                        fields[3].Value = coverage.TechnicalPlanCoverage.Coverage.Description;
                                        fields[4].Value = coverage.TechnicalPlanCoverage.PrincipalCoverage.Id.ToString();
                                        fields[5].Value = coverage.TechnicalPlanCoverage.PrincipalCoverage.Description;
                                        fields[6].Value = coverage.TechnicalPlanCoverage.CoveragePercentage.ToString();
                                        fields[7].Value = ally.Id.ToString();
                                        fields[8].Value = ally.Description;
                                        rows.Add(new Row
                                        {
                                            Fields = fields
                                        });
                                    }
                                }
                                else
                                {
                                    fields[0].Value = item.TechnicalPlan.Description;
                                    fields[1].Value = item.TechnicalPlan.CoveredRiskType.SmallDescription;
                                    fields[2].Value = coverage.TechnicalPlanCoverage.InsuredObject.SmallDescription;
                                    fields[3].Value = coverage.TechnicalPlanCoverage.Coverage.Description;
                                    fields[4].Value = coverage.TechnicalPlanCoverage.PrincipalCoverage.Id.ToString();
                                    fields[5].Value = coverage.TechnicalPlanCoverage.PrincipalCoverage.Description;
                                    fields[6].Value = coverage.TechnicalPlanCoverage.CoveragePercentage.ToString();
                                    fields[7].Value = "";
                                    fields[8].Value = "";
                                    rows.Add(new Row
                                    {
                                        Fields = fields
                                    });
                                }
                            }
                        }
                        else
                        {

                            fields[0].Value = item.TechnicalPlan.Description;
                            fields[1].Value = item.TechnicalPlan.CoveredRiskType.SmallDescription;
                            fields[2].Value = "";
                            fields[3].Value = "";
                            fields[4].Value = "";
                            fields[5].Value = "";
                            fields[6].Value = "";
                            fields[7].Value = "";
                            fields[8].Value = "";
                           rows.Add(new Row
                            {
                                Fields = fields
                            });
                        }
                                                                       
                    }

                    file.Templates[0].Rows = rows;
                    file.Name = fileName + "_" + DateTime.Now.ToString("dd_MM_yyyy");

                    var result = fileDAO.GenerateFile(file);
                    return new UTMO.ResultValue<string, UTMO.ErrorModel>(result);
                }
                else
                {
                    listErrors.Add(Resources.Errors.ErrorDownloadingExcel);
                    return new UTMO.ResultError<string, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, new System.ArgumentException(Resources.Errors.ErrorDownloadingExcel, "original")));
                }
            }
            catch (System.Exception ex)
            {
                listErrors.Add(Resources.Errors.ErrorDownloadingExcel);
                return new UTMO.ResultError<string, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }
    }
}
