// -----------------------------------------------------------------------
// <copyright file="ClauseDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Heidy Pinto</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.EEProvider.DAOs
{
    using System.Collections.Generic;
    using Sistran.Core.Application.UnderwritingParamService.EEProvider.Assemblers;
    using Sistran.Core.Application.UnderwritingParamService.EEProvider.Entities.Views;
    using Sistran.Core.Application.UnderwritingParamService.Models;
    using Sistran.Core.Application.Utilities.DataFacade;
    using Sistran.Core.Framework.DAF;
    using Sistran.Core.Framework.DAF.Engine;
    using Sistran.Core.Framework.Queries;
    using ENUMUD = Sistran.Core.Application.UnderwritingServices.Enums;
    using ENUMUT = Sistran.Core.Application.Utilities.Enums;
    using PAREN = Sistran.Core.Application.Parameters.Entities;
    using QUOEN = Sistran.Core.Application.Quotation.Entities;
    using UTMO = Sistran.Core.Application.Utilities.Error;
    using Sistran.Core.Application.UnderwritingParamServices.EEProvider.Assemblers;
    using System.Diagnostics;
    using UNDMO = Sistran.Core.Application.UnderwritingServices.Models;
    using PARAMEN = Sistran.Core.Application.Parameters.Entities;
    using COMMEN = Sistran.Core.Application.Common.Entities;
    using COMMO = Sistran.Core.Application.CommonService.Models;
    using UTILMO = Sistran.Core.Services.UtilitiesServices.Models;
    using UTILEN = Sistran.Core.Services.UtilitiesServices.Enums;
    using System.Linq;
    using System;
    using System.Data;
    using Sistran.Co.Application.Data;
    using COMENUM = Sistran.Core.Application.CommonService.Enums;
    using Sistran.Core.Application.UnderwritingParamServices.EEProvider.DAOs;
    using Sistran.Core.Application.CommonService.Models;
    using Sistran.Core.Application.UnderwritingServices.EEProvider.Entities.Views;

    /// <summary>
    /// Acceso a DB de la clausula
    /// </summary>
    public class ClauseDAO
    {
        /// <summary>
        /// Obtiene DB las clausulas relacionadas al tipo de condicion level
        /// </summary>
        /// <param name="conditionLevelType">tipo de condicion level</param>
        /// <returns>listado de clausulas</returns>
        public UTMO.Result<List<ParamClauseDesc>, UTMO.ErrorModel> GetParamClauseDescsByConditionLevelType(ENUMUD.ConditionLevelType conditionLevelType)
        {
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(QUOEN.Clause.Properties.ConditionLevelCode, typeof(QUOEN.Clause).Name);
                filter.Equal();
                filter.Constant(conditionLevelType);                
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(QUOEN.Clause), filter.GetPredicate()));
                List<ParamClauseDesc> paramClauseDescs = ModelAssembler.CreateParamClauseDescs(businessCollection);                
                return new UTMO.ResultValue<List<ParamClauseDesc>, UTMO.ErrorModel>(paramClauseDescs);                
            }
            catch (System.Exception ex)
            {                
                return new UTMO.ResultError<List<ParamClauseDesc>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(new List<string>() { Resources.Errors.ErrorGetClausesAssociatedDB }, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Obtiene clausulas relacionadas al tipo de condicion level y al id de condicion level
        /// </summary>
        /// <param name="conditionLevelType">tipo de condicion level</param>
        /// <param name="conditionLevelId">id de condicion level</param>
        /// <returns>listado de clausulas</returns>
        public UTMO.Result<List<ParamClauseDesc>, UTMO.ErrorModel> GetParamClauseDescsByConditionLevelTypeConditionLevelId(ENUMUD.ConditionLevelType conditionLevelType, int conditionLevelId)
        {
            try
            {
                List<ParamClauseDesc> paramClauseDescs = new List<ParamClauseDesc>();
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(QUOEN.ClauseLevel.Properties.ConditionLevelId, typeof(QUOEN.ClauseLevel).Name);
                filter.Equal();
                filter.Constant(conditionLevelId);
                filter.And();
                filter.Property(QUOEN.Clause.Properties.ConditionLevelCode, typeof(QUOEN.Clause).Name);
                filter.Equal();
                filter.Constant(conditionLevelType);

                ParamClauseView view = new ParamClauseView();
                ViewBuilder builder = new ViewBuilder("ClauseView");
                builder.Filter = filter.GetPredicate();

                DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

                if (view.Clauses.Count > 0)
                {
                    paramClauseDescs = ModelAssembler.CreateParamClauseItems(view.ClauseLevels);
                }

                return new UTMO.ResultValue<List<ParamClauseDesc>, UTMO.ErrorModel>(paramClauseDescs);
            }
            catch (System.Exception ex)
            {                
                return new UTMO.ResultError<List<ParamClauseDesc>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(new List<string>() { Resources.Errors.ErrorGetClausesAssociatedDB }, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Crea las clausulas asociadas
        /// </summary>
        /// <param name="paramClauseDesc">clausula modelo negocio</param>
        /// <param name="coverageId">id de cobertura</param>
        /// <returns>clausula creada</returns>
        public UTMO.Result<ParamClauseDesc, UTMO.ErrorModel> CreateParamClauseDesc(ParamClauseDesc paramClauseDesc, int coverageId)
        {
            try
            {
                QUOEN.ClauseLevel clauseLevelEntity = EntityAssembler.CreateClauseLevel(paramClauseDesc, coverageId);
                DataFacadeManager.Instance.GetDataFacade().InsertObject(clauseLevelEntity);
                ParamClauseDesc result = ModelAssembler.CreateParamClauseDesc(clauseLevelEntity);
                return new UTMO.ResultValue<ParamClauseDesc, UTMO.ErrorModel>(result);
            }
            catch (System.Exception ex)
            {
                return new UTMO.ResultError<ParamClauseDesc, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(new List<string>() { Resources.Errors.ErrorCreateClausesAssociatedDB }, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        #region Clauses
        /// <summary>
        /// Metodo consultar clausulas
        /// </summary>
        /// <returns>Retorna lista de clausulas</returns>
        public UTMO.Result<List<ParamClause>, UTMO.ErrorModel> GetParametrizationClauses()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            try
            {
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(QUOEN.Clause)));
                List<ParamClause> parametrizationClauses = ModelAssembler.CreateParametrizationClauses(businessCollection);
                return new UTMO.ResultValue<List<ParamClause>, UTMO.ErrorModel>(parametrizationClauses);
            }
            catch (System.Exception ex)
            {
                errorModelListDescription.Add("Resources.Errors.ErrorGetClause");
                return new UTMO.ResultError<List<ParamClause>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(errorModelListDescription, ENUMUT.ErrorType.TechnicalFault, ex));
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingParamService.EEProviderWeb.DAOs");
            }
        }

        /// <summary>
        /// Consulta los niveles de las clausulas
        /// </summary>
        /// <returns>Retorna lista de clausulas</returns>
        public UTMO.Result<List<UNDMO.ConditionLevel>, UTMO.ErrorModel> GetParametrizationClausesLevels()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            try
            {
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(PARAMEN.ConditionLevel)));
                List<UNDMO.ConditionLevel> parametrizationClauses = ModelAssembler.CreateConditionLevels(businessCollection);
                return new UTMO.ResultValue<List<UNDMO.ConditionLevel>, UTMO.ErrorModel>(parametrizationClauses);
            }
            catch (System.Exception ex)
            {
                errorModelListDescription.Add("Resources.Errors.ErrorGetLevelClause");
                return new UTMO.ResultError<List<UNDMO.ConditionLevel>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(errorModelListDescription, ENUMUT.ErrorType.TechnicalFault, ex));
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingParamService.EEProviderWeb.DAOs");
            }
        }

        /// <summary>
        /// Consulta ramo comercial
        /// </summary>
        /// <returns>Retorna ramo comercial</returns>
        public UTMO.Result<List<ParamClausePrefix>, UTMO.ErrorModel> GetPrefix()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            try
            {
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.Prefix)));
                List<ParamClausePrefix> parametrizationClausesPrefix = ModelAssembler.CreatePrefixs(businessCollection);
                return new UTMO.ResultValue<List<ParamClausePrefix>, UTMO.ErrorModel>(parametrizationClausesPrefix);
            }
            catch (System.Exception ex)
            {
                errorModelListDescription.Add("Resources.Errors.ErrorGetPrefix");
                return new UTMO.ResultError<List<ParamClausePrefix>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(errorModelListDescription, ENUMUT.ErrorType.TechnicalFault, ex));
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingParamService.EEProviderWeb.DAOs");
            }
        }

        /// <summary>
        /// Consulta tipo de riesgo
        /// </summary>
        /// <returns>Retorna tipo de riesgo</returns>
        public UTMO.Result<List<UNDMO.RiskType>, UTMO.ErrorModel> GetCoveredRiskType()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            try
            {
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(PARAMEN.CoveredRiskType)));
                List<UNDMO.RiskType> parametrizationClauses = ModelAssembler.CreateCoveredRiskTypesConsult(businessCollection);
                return new UTMO.ResultValue<List<UNDMO.RiskType>, UTMO.ErrorModel>(parametrizationClauses);
            }
            catch (System.Exception ex)
            {
                errorModelListDescription.Add("Resources.Errors.ErrorGetRiskType");
                return new UTMO.ResultError<List<UNDMO.RiskType>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(errorModelListDescription, ENUMUT.ErrorType.TechnicalFault, ex));
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingParamService.EEProviderWeb.DAOs");
            }
        }

        /// <summary>
        /// Metodo para crear clausula
        /// </summary>
        /// <param name="parametrizationClause">Recibe nueva clausula</param>
        /// <returns>Retorna clausulas creadas</returns>
        public UTMO.Result<ParamClause, UTMO.ErrorModel> CreateParametrizationClause(ParamClause parametrizationClause)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            
            try
            {
                //Parameter paramPayment = DelegateService.commonServiceCore.GetParameterByParameterId(paramClauseId);
                //if (paramPayment != null && paramPayment.NumberParameter.HasValue)
                //{
                //    paramPayment.NumberParameter = paramPayment.NumberParameter.Value + 1;
                //    parametrizationClause.Clause.Id = paramPayment.NumberParameter.Value;
                //}

                QUOEN.Clause clauseEntity = EntityAssembler.CreateClause(parametrizationClause);
                DataFacadeManager.Instance.GetDataFacade().InsertObject(clauseEntity);
                ParamClause parametrizationClauseResult = ModelAssembler.CreateParametrizationClause(clauseEntity);

                if(parametrizationClauseResult != null)
                {
                    QUOEN.ClauseLevel clauseLevelEntity = new QUOEN.ClauseLevel();
                    if(clauseEntity.ConditionLevelCode !=1)
                    clauseLevelEntity.ConditionLevelId = parametrizationClause.ClauseLevel.ConditionLevelId;
                    clauseLevelEntity.ClauseId = parametrizationClauseResult.Clause.Id;
                    clauseLevelEntity.IsMandatory = parametrizationClause.Clause.IsMandatory;
                  
                    DataFacadeManager.Instance.GetDataFacade().InsertObject(clauseLevelEntity);
                }
                
                return new UTMO.ResultValue<ParamClause, UTMO.ErrorModel>(parametrizationClauseResult);                
            }
            catch (System.Exception ex)
            {
                errorModelListDescription.Add(Resources.Errors.ErrorGetClause);
                return new UTMO.ResultError<ParamClause, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(errorModelListDescription, ENUMUT.ErrorType.TechnicalFault, ex));
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingParamService.EEProviderWeb.DAOs");
            }
        }

        /// <summary>
        /// Listado de textos precatalogados por nombre o Id
        /// </summary>
        /// <param name="name">Recimbre nombre o Id</param>
        /// <returns>Retorna lista de textos</returns>
        public UTMO.Result<List<ParamClause>, UTMO.ErrorModel> GetTextsByNameLevelIdParametrization(string name)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            try
            {
                int textId = 0;
                int.TryParse(name, out textId);

                ConditionTextView view = new ConditionTextView();
                ViewBuilder builder = new ViewBuilder("ConditionTextView");

                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                if (textId == 0)
                {
                    filter.Property(QUOEN.ConditionText.Properties.TextTitle, typeof(QUOEN.ConditionText).Name);
                    filter.Like();
                    filter.Constant("%" + name + "%");
                }
                else
                {
                    filter.Property(QUOEN.ConditionText.Properties.ConditionTextId, typeof(QUOEN.ConditionText).Name);
                    filter.Equal();
                    filter.Constant(textId);
                }

                builder.Filter = filter.GetPredicate();
                DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
                List<ParamClause> parametrizationTextClause = ModelAssembler.CreateTexts(view.ConditionText);
                return new UTMO.ResultValue<List<ParamClause>, UTMO.ErrorModel>(parametrizationTextClause);
            }
            catch (System.Exception ex)
            {
                errorModelListDescription.Add("Resources.Errors.ErrorGetText");
                return new UTMO.ResultError<List<ParamClause>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(errorModelListDescription, ENUMUT.ErrorType.TechnicalFault, ex));
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingParamService.EEProviderWeb.DAOs");
            }


        }

        /// <summary>
        /// Metodo que consulta cobertura por Nombre o Id
        /// </summary>
        /// <param name="name">Recibe nombre o Id</param>
        /// <returns>Retorna lista de coberturas</returns>
        public UTMO.Result<List<ParamClauseCoverage>, UTMO.ErrorModel> GetCoverageByName(string name)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            try
            {
                int textId = 0;
                int.TryParse(name, out textId);

                CoverageParametrizationView view = new CoverageParametrizationView();
                ViewBuilder builder = new ViewBuilder("CoverageParametrizationView");
                List<ParamClauseCoverage> coverageSets = new List<ParamClauseCoverage>();
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                if (textId == 0)
                {
                    filter.Property(QUOEN.Coverage.Properties.PrintDescription, typeof(QUOEN.Coverage).Name);
                    filter.Like();
                    filter.Constant("%" + name + "%");
                }
                else
                {
                    filter.Property(QUOEN.Coverage.Properties.CoverageId, typeof(QUOEN.Coverage).Name);
                    filter.Equal();
                    filter.Constant(textId);
                }

                builder.Filter = filter.GetPredicate();
                DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

                if (view.Coverages.Count > 0)
                {
                    List<ParamClauseCoverage> coverages = ModelAssembler.CreateParametrizationCoverages(view.Coverages);
                    List<QUOEN.InsuredObject> entityInsuredObject = view.InsuredObjects.Cast<QUOEN.InsuredObject>().ToList();
                    List<QUOEN.Peril> entityPeriles = view.Perils.Cast<QUOEN.Peril>().ToList();

                    foreach (ParamClauseCoverage coverage in coverages)
                    {
                        coverage.ParamClauseInsuredObject.Description = entityInsuredObject.First(X => X.InsuredObjectId == coverage.ParamClauseInsuredObject.Id).Description;
                        coverage.Peril.Description = entityPeriles.First(X => X.PerilCode == coverage.Peril.Id).Description;
                        coverageSets.Add(coverage);
                    }
                }

                coverageSets.OrderBy(x => x.Description).ToList();

                return new UTMO.ResultValue<List<ParamClauseCoverage>, UTMO.ErrorModel>(coverageSets);
            }
            catch (System.Exception ex)
            {
                errorModelListDescription.Add("Resources.Errors.ErrorGetCoverage");
                return new UTMO.ResultError<List<ParamClauseCoverage>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(errorModelListDescription, ENUMUT.ErrorType.TechnicalFault, ex));
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingParamService.EEProviderWeb.DAOs");
            }

        }

        /// <summary>
        /// Metodo que busca clausula por nombre o titulo
        /// </summary>
        /// <param name="name">Recibe nombre</param>
        /// <returns>Retorna clausulas</returns>
        public UTMO.Result<List<ParamClause>, UTMO.ErrorModel> GetClauseByNameAndTitle(string name)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            try
            {

                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(QUOEN.Clause.Properties.ClauseName, typeof(QUOEN.Clause).Name);
                filter.Like();
                filter.Constant("%" + name + "%");
                filter.Or();
                filter.Property(QUOEN.Clause.Properties.ClauseTitle, typeof(QUOEN.Clause).Name);
                filter.Like();
                filter.Constant("%" + name + "%");


                ClauseParametrizationView view = new ClauseParametrizationView();
                ViewBuilder builder = new ViewBuilder("ClauseParametrizationView");
                builder.Filter = filter.GetPredicate();
                DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

                List<ParamClause> listParametrizationClause = new List<ParamClause>();
                List<ParamClauseLevel> listClauseLevel = ModelAssembler.CreateClauseLevels(view.ClauseLevels);
                listParametrizationClause = ModelAssembler.CreateParametrizationClauses(view.Clauses);

                List<QUOEN.Clause> entityClause = view.Clauses.Cast<QUOEN.Clause>().ToList();
                List<PARAMEN.ConditionLevel> entityConditionLevels = view.ConditionLevels.Cast<PARAMEN.ConditionLevel>().ToList();
                List<QUOEN.ClauseLevel> list = view.ClauseLevels.Cast<QUOEN.ClauseLevel>().ToList();
                if (view.Clauses.Count > 0)
                {

                    foreach (ParamClause clause in listParametrizationClause)
                    {
                        clause.ClauseLevel = listClauseLevel.First(x => x.ClauseId == clause.Clause.Id);
                    }
                }

                return new UTMO.ResultValue<List<ParamClause>, UTMO.ErrorModel>(listParametrizationClause);
            }
            catch (System.Exception ex)
            {
                errorModelListDescription.Add("Resources.Errors.ErrorGetClause");
                return new UTMO.ResultError<List<ParamClause>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(errorModelListDescription, ENUMUT.ErrorType.TechnicalFault, ex));
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingParamService.EEProviderWeb.DAOs");
            }

        }

        /// <summary>
        /// Obtiene consulta clausulas,niveles, etc
        /// </summary>
        /// <returns>Retorna lista de clausulas</returns>
        public UTMO.Result<List<ParamClause>, UTMO.ErrorModel> GetClauseAll()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            try
            {
                ClauseParametrizationView view = new ClauseParametrizationView();
                ViewBuilder builder = new ViewBuilder("ClauseParametrizationView");
                DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
                List<ParamClause> listParametrizationClause = new List<ParamClause>();
                List<UNDMO.ConditionLevel> listConditionLevel = new List<UNDMO.ConditionLevel>();
                List<ParamClauseLevel> listClauseLevel = ModelAssembler.CreateClauseLevels(view.ClauseLevels);

                if (view.Clauses.Count == 0)
                {
                    errorModelListDescription.Add(Resources.Errors.ErrorGetClause);
                    return new UTMO.ResultError<List<ParamClause>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(errorModelListDescription, ENUMUT.ErrorType.NotFound, null));
                }
                else
                {
                    listParametrizationClause = ModelAssembler.CreateParametrizationClauses(view.Clauses);
                    listConditionLevel = ModelAssembler.CreateConditionLevels(view.ConditionLevels);
                    List<QUOEN.ClauseLevel> entityClauseLevels = view.ClauseLevels.Cast<QUOEN.ClauseLevel>().ToList();
                    List<PARAMEN.ConditionLevel> entityConditionLevels = view.ConditionLevels.Cast<PARAMEN.ConditionLevel>().ToList();
                    foreach (ParamClause clause in listParametrizationClause)
                    {
                        clause.Clause.ConditionLevel = listConditionLevel.First(x => x.Id == clause.Clause.ConditionLevel.Id);
                        clause.ClauseLevel = listClauseLevel.First(x => x.ClauseId == clause.Clause.Id);
                    }
                }

                return new UTMO.ResultValue<List<ParamClause>, UTMO.ErrorModel>(listParametrizationClause);
            }
            catch (System.Exception ex)
            {
                errorModelListDescription.Add("Resources.Errors.ErrorGetClause");
                return new UTMO.ResultError<List<ParamClause>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(errorModelListDescription, ENUMUT.ErrorType.TechnicalFault, ex));
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingParamService.EEProviderWeb.DAOs");
            }


        }

        /// <summary>
        /// Metodo que permite consultar las coberturas por clausula
        /// </summary>
        /// <returns>Retorna coberturas</returns>
        public UTMO.Result<List<ParamClause>, UTMO.ErrorModel> GetClauseForCoverage()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            try
            {
                CoverageClauseParametrizationView viewcoverage = new CoverageClauseParametrizationView();
                ViewBuilder builderCoverage = new ViewBuilder("CoverageClauseParametrizationView");

                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.PropertyEquals(QUOEN.Clause.Properties.ConditionLevelCode, "Clause", UnderwritingParamService.Enums.ConditionType.Coverage);
                builderCoverage.Filter = filter.GetPredicate();
                DataFacadeManager.Instance.GetDataFacade().FillView(builderCoverage, viewcoverage);
                List<ParamClauseCoverage> listCoverage = new List<ParamClauseCoverage>();
                List<ParamClause> clauses = new List<ParamClause>();
                if (viewcoverage.Coverages.Count > 0)
                {
                    List<ParamClauseLevel> clausesLevel = ModelAssembler.CreateClauseLevels(viewcoverage.ClauseLevels);
                    listCoverage = ModelAssembler.CreateParametrizationCoverages(viewcoverage.Coverages);
                    List<ParamClauseInsuredObject> clausesInsuredobject = ModelAssembler.CreateClauseInsuredObjects(viewcoverage.InsuredObjects);
                    List<UNDMO.Peril> clausesPeril = ModelAssembler.CreateClausePerils(viewcoverage.Perils);
                    foreach (ParamClauseLevel clauseLevel in clausesLevel)
                    {
                        ParamClause clause = new ParamClause();
                        clause.ClauseLevel = clauseLevel;
                        clause.ParamClauseCoverage = listCoverage.First(x => x.Id == clauseLevel.ConditionLevelId);
                        clause.ParamClauseCoverage.ParamClauseInsuredObject = clausesInsuredobject.First(x => x.Id == clause.ParamClauseCoverage.ParamClauseInsuredObject.Id);
                        clause.ParamClauseCoverage.Peril = clausesPeril.First(x => x.Id == clause.ParamClauseCoverage.Peril.Id);
                        clauses.Add(clause);
                    }
                }

                return new UTMO.ResultValue<List<ParamClause>, UTMO.ErrorModel>(clauses);
            }
            catch (System.Exception ex)
            {
                errorModelListDescription.Add("Resources.Errors.ErrorGetCoverage");
                return new UTMO.ResultError<List<ParamClause>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(errorModelListDescription, ENUMUT.ErrorType.TechnicalFault, ex));
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingParamService.EEProviderWeb.DAOs");
            }

        }

        /// <summary>
        /// Metodo que permite consultar Ramo comercial por clausula
        /// </summary>
        /// <returns>Retorna listado de ramo comercial </returns>
        public UTMO.Result<List<ParamClause>, UTMO.ErrorModel> GetClauseForCommercialBranch()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            try
            {
                ClausePrefixParametrizationView view = new ClausePrefixParametrizationView();
                ViewBuilder builder = new ViewBuilder("ClausePrefixParametrizationView");
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.PropertyEquals(QUOEN.Clause.Properties.ConditionLevelCode, "Clause", UnderwritingParamService.Enums.ConditionType.Prefix);
                builder.Filter = filter.GetPredicate();
                DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
                List<ParamClausePrefix> listPrefix = new List<ParamClausePrefix>();
                List<ParamClause> clauses = new List<ParamClause>();
                if (view.Prefix.Count > 0)
                {
                    List<ParamClauseLevel> clausesLevel = ModelAssembler.CreateClauseLevels(view.ClauseLevels);
                    listPrefix = ModelAssembler.CreatePrefixs(view.Prefix);
                    List<COMMEN.Prefix> entityPrefix = view.Prefix.Cast<COMMEN.Prefix>().ToList();
                    foreach (ParamClauseLevel clauseLevel in clausesLevel)
                    {
                        ParamClause clause = new ParamClause();
                        clause.ParamClausePrefix = listPrefix.First(x => x.Id == clauseLevel.ConditionLevelId);
                        clause.ClauseLevel = clauseLevel;
                        clauses.Add(clause);
                    }
                }

                return new UTMO.ResultValue<List<ParamClause>, UTMO.ErrorModel>(clauses);
            }
            catch (System.Exception ex)
            {
                errorModelListDescription.Add("Resources.Errors.ErrorGetPrefix");
                return new UTMO.ResultError<List<ParamClause>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(errorModelListDescription, ENUMUT.ErrorType.TechnicalFault, ex));
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingParamService.EEProviderWeb.DAOs");
            }

        }

        /// <summary>
        /// Metodo que permite consultar tipo de riesgo por clausula
        /// </summary>
        /// <returns>Retorna listado de tipos de riesgo </returns>
        public UTMO.Result<List<ParamClause>, UTMO.ErrorModel> GetClauseForRiskType()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            try
            {
                RiskTypeParametrizationView view = new RiskTypeParametrizationView();
                ViewBuilder builder = new ViewBuilder("RiskTypeParametrizationView");

                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.PropertyEquals(QUOEN.Clause.Properties.ConditionLevelCode, "Clause", UnderwritingParamService.Enums.ConditionType.TypeRisk);
                builder.Filter = filter.GetPredicate();
                DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
                List<UNDMO.RiskType> listRiskType = new List<UNDMO.RiskType>();
                List<ParamClause> clauses = new List<ParamClause>();
                if (view.RiskType.Count > 0)
                {
                    listRiskType = ModelAssembler.CreateCoveredRiskTypesConsult(view.RiskType);
                    List<PARAMEN.CoveredRiskType> entityRiskType = view.RiskType.Cast<PARAMEN.CoveredRiskType>().ToList();
                    List<ParamClauseLevel> listClauses = ModelAssembler.CreateClauseLevels(view.ClauseLevels);

                    foreach (ParamClauseLevel clauseLevel in listClauses)
                    {
                        ParamClause clause = new ParamClause();
                        clause.ClauseLevel = clauseLevel;
                        clause.RiskType = listRiskType.First(x => x.Id == clauseLevel.ConditionLevelId);
                        clauses.Add(clause);
                    }

                }
                return new UTMO.ResultValue<List<ParamClause>, UTMO.ErrorModel>(clauses);
            }

            catch (Exception ex)
            {
                errorModelListDescription.Add(Resources.Errors.ErrorGetRiskType);   // descripción del error del archivo de recurso
                return new UTMO.ResultError<List<ParamClause>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(errorModelListDescription, ENUMUT.ErrorType.TechnicalFault, ex));
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingParamService.EEProviderWeb.DAOs");
            }


        }

        /// <summary>
        /// Metodo que permite consultar tipo de riesgo por clausula
        /// </summary>
        /// <returns>Retorna listado de tipos de riesgo </returns>
        public UTMO.Result<List<ParamClause>, UTMO.ErrorModel> GetClauseForLineBusiness()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            try
            {
                LineBusinessClauseLevelParametrizationView view = new LineBusinessClauseLevelParametrizationView();
                ViewBuilder builder = new ViewBuilder("LineBusinessClauseLevelParametrizationView");

                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.PropertyEquals(QUOEN.Clause.Properties.ConditionLevelCode, "Clause", UnderwritingParamService.Enums.ConditionType.TechnicalBranch);
                builder.Filter = filter.GetPredicate();
                DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
                List<ParamClauseLineBusiness> listLineBusiness = new List<ParamClauseLineBusiness>();
                List<ParamClause> clauses = new List<ParamClause>();
                if (view.LineBusiness.Count > 0)
                {
                    listLineBusiness = ModelAssembler.CreateLinesBusinessConsult(view.LineBusiness);
                    List<COMMEN.LineBusiness> entityRiskType = view.LineBusiness.Cast<COMMEN.LineBusiness>().ToList();
                    List<ParamClauseLevel> listClauses = ModelAssembler.CreateClauseLevels(view.ClauseLevels);

                    foreach (ParamClauseLevel clauseLevel in listClauses)
                    {
                        ParamClause clause = new ParamClause();
                        clause.ClauseLevel = clauseLevel;
                        clause.ParamClauseLineBusiness = listLineBusiness.First(x => x.Id == clauseLevel.ConditionLevelId);
                        clauses.Add(clause);
                    }

                }
                return new UTMO.ResultValue<List<ParamClause>, UTMO.ErrorModel>(clauses);
            }

            catch (Exception ex)
            {
                errorModelListDescription.Add(Resources.Errors.ErrorGetRiskType);   // descripción del error del archivo de recurso
                return new UTMO.ResultError<List<ParamClause>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(errorModelListDescription, ENUMUT.ErrorType.TechnicalFault, ex));
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingParamService.EEProviderWeb.DAOs");
            }


        }



        /// <summary>
        /// Genera archivo excel de clausulas
        /// </summary>
        /// <param name="clauses">Listado de clausulas</param>
        /// <param name="fileName">Nombre de archivo</param>
        /// <returns>Modelo result</returns>
        public UTMO.Result<string, UTMO.ErrorModel> GenerateFileToClause(List<ParamClause> clauses, string fileName)
        {
            List<string> errorModelListDescription = new List<string>();
            try
            {
                UTILMO.FileProcessValue fileProcessValue = new UTILMO.FileProcessValue()
                {
                    Key1 = (int)UTILEN.FileProcessType.ParametrizationClause
                };
                FileDAO fileDAO = new FileDAO();
                UTILMO.File file = fileDAO.GetFileByFileProcessValue(fileProcessValue);

                if (file != null && file.IsEnabled)
                {
                    file.Description = fileName;
                    List<UTILMO.Row> rows = new List<UTILMO.Row>();

                    foreach (ParamClause item in clauses)
                    {
                        var fields = file.Templates[0].Rows[0].Fields.Select(x => new UTILMO.Field
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

                        fields[0].Value = item.Clause.Id.ToString();
                        fields[1].Value = item.Clause.Name.ToString();
                        fields[2].Value = item.Clause.Title;
                        fields[3].Value = item.Clause.ConditionLevel.Description.ToString();
                        if (item.ParamClausePrefix.Description != null)
                        {
                            fields[4].Value = item.ParamClausePrefix.Description.ToString();
                        }
                        else
                        {
                            fields[4].Value = String.Empty;
                        }
                        fields[5].Value = item.InputStartDate.ToString();
                        fields[6].Value = item.DueDate.ToString();
                        fields[7].Value = item.Clause.IsMandatory.ToString();
                        fields[8].Value = item.Clause.Text.ToString();

                        rows.Add(new UTILMO.Row
                        {
                            Fields = fields
                        });
                    }

                    file.Templates[0].Rows = rows;
                    file.Name = fileName + "_" + DateTime.Now.ToString("dd_MM_yyyy");

                    var result = fileDAO.GenerateFile(file);
                    return new UTMO.ResultValue<string, UTMO.ErrorModel>(result);
                }
                else
                {
                    return new UTMO.ResultValue<string, UTMO.ErrorModel>("Error");
                }
            }
            catch (System.Exception ex)
            {
                errorModelListDescription.Add("Error Descargando Excel");
                return new UTMO.ResultError<string, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(errorModelListDescription, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Metodo para actualizar clausulas
        /// </summary>
        /// <param name="parametrizationClause">Recibe clausulas</param>
        /// <returns>Retorna clausulas actualizadas</returns>
        public UTMO.Result<ParamClause, UTMO.ErrorModel> UpdateParametrizationClause(ParamClause parametrizationClause)
        {
            List<string> errorModelListDescription = new List<string>();
            try
            {
                DataTable result;
                NameValue[] parameters = new NameValue[9];
                parameters[0] = new NameValue("@CLAUSE_ID", parametrizationClause.Clause.Id);
                parameters[1] = new NameValue("@CLAUSE_NAME", parametrizationClause.Clause.Name);
                parameters[2] = new NameValue("@CLAUSE_TITLE", parametrizationClause.Clause.Title);
                parameters[3] = new NameValue("@CLAUSE_TEXT", parametrizationClause.Clause.Text);
                parameters[4] = new NameValue("@STAR_DATE", parametrizationClause.InputStartDate);
                parameters[5] = new NameValue("@IS_MANDATORY", parametrizationClause.ClauseLevel.IsMandatory);
                if(parametrizationClause.ClauseLevel.ConditionLevelId != null)
                    parameters[6] = new NameValue("@CONDITION_LEVEL_ID", parametrizationClause.ClauseLevel.ConditionLevelId);
                else
                    parameters[6] = new NameValue("@CONDITION_LEVEL_ID", 0);

                if(parametrizationClause.Clause.ConditionLevel.Id ==1)
                    parameters[6] = new NameValue("@CONDITION_LEVEL_ID", null);

                if (parametrizationClause.DueDate != null)
                {
                    parameters[7] = new NameValue("@DUEDATE", parametrizationClause.DueDate);
                    
                }
                else
                {
                    parameters[7] = new NameValue("@DUEDATE", DBNull.Value, DbType.DateTime);
                }
                parameters[8] = new NameValue("@CONDITION_LEVEL_CD", parametrizationClause.Clause.ConditionLevel.Id);

                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    result = dynamicDataAccess.ExecuteSPDataTable("[QUO].[UPDATE_CLAUSE_CLAUSE_LEVEL_PARAMETRIZATION]", parameters);
                }

                return new UTMO.ResultValue<ParamClause, UTMO.ErrorModel>(parametrizationClause);
            }
            catch (System.Exception ex)
            {
                errorModelListDescription.Add("Resources.Errors.ErrorUpdateParametrizationClause");
                return new UTMO.ResultError<ParamClause, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(errorModelListDescription, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Metodo para eliminar clausula e hijo
        /// </summary>
        /// <param name="parametrizationClause">Recibe parametrizationClause</param>
        /// <param name="isPrincipal">Recibe isPrincipal</param>
        /// <returns>Retorna clausulas eliminadas</returns>
        public UTMO.Result<ParamClause, UTMO.ErrorModel> DeleteParametrizationClause(ParamClause parametrizationClause, bool isPrincipal)
        {
            List<string> errorModelListDescription = new List<string>();
            try
            {
                DataTable result;
                NameValue[] parameters = new NameValue[2];
                parameters[0] = new NameValue("@CLAUSE_ID", parametrizationClause.Clause.Id);
                parameters[1] = new NameValue("@PRINCIPAL", isPrincipal);
                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    result = dynamicDataAccess.ExecuteSPDataTable("[QUO].[DELETE_CLAUSE_LEVEL_PARAMETRIZATION]", parameters);
                }

                if (((int)result.Rows[0][0]) == 1)
                {
                    parametrizationClause.ClauseLevel = null;
                    return new UTMO.ResultValue<ParamClause, UTMO.ErrorModel>(parametrizationClause);
                }

                if (((int)result.Rows[0][0]) == -1)
                {
                    errorModelListDescription.Add(Resources.Errors.ErrorDeleteParametrizationClause);
                    return new UTMO.ResultError<ParamClause, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(errorModelListDescription, ENUMUT.ErrorType.TechnicalFault, null));
                }
                errorModelListDescription.Add("Resources.Errors.ErrorDeleteParametrizationClause");
                return new UTMO.ResultError<ParamClause, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(errorModelListDescription, ENUMUT.ErrorType.TechnicalFault, null));
            }
            catch (System.Exception ex)
            {
                errorModelListDescription.Add("Resources.Errors.ErrorDeleteParametrizationClause");
                return new UTMO.ResultError<ParamClause, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(errorModelListDescription, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }
        #endregion

        /// <summary>
        /// Consulta las clausulas relacionadas al level
        /// </summary>
        /// <param name="emissionLevel">Nivel de cláusula</param>
        /// <param name="conditionLevelId">Identificador correspondiente</param>
        /// <returns>Lista de cláusulas</returns>
        public UTMO.Result<List<ParamClauseDesc>, UTMO.ErrorModel> GetParamClauseDescsByEmissionLevelConditionLevelId(ENUMUD.EmissionLevel emissionLevel, int conditionLevelId)
        {
            try
            {
                List<ParamClauseDesc> paramClauseDescs = new List<ParamClauseDesc>();
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

                filter.Property(PAREN.ConditionLevel.Properties.LevelId, typeof(PAREN.ConditionLevel).Name).Equal().Constant(emissionLevel);
                filter.And();
                filter.OpenParenthesis();
                filter.Property(QUOEN.Clause.Properties.CurrentTo, typeof(QUOEN.Clause).Name).GreaterEqual().Constant(DateTime.Now);
                filter.Or();
                filter.Property(QUOEN.Clause.Properties.CurrentTo, typeof(QUOEN.Clause).Name).IsNull();
                filter.CloseParenthesis();
                filter.And();
                filter.OpenParenthesis();
                filter.Property(QUOEN.Clause.Properties.ConditionLevelCode, typeof(QUOEN.Clause).Name).Equal().Constant(conditionLevelId);
                filter.Or();
                filter.Property(QUOEN.Clause.Properties.ConditionLevelCode, typeof(QUOEN.Clause).Name).IsNull();
                filter.CloseParenthesis();

                ParamClauseView view = new ParamClauseView();
                ViewBuilder builder = new ViewBuilder("ClauseView");
                builder.Filter = filter.GetPredicate();

                DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

                if (view.Clauses.Count > 0)
                {
                    paramClauseDescs = ModelAssembler.CreateParamClauseDescs(view.Clauses);
                }

                return new UTMO.ResultValue<List<ParamClauseDesc>, UTMO.ErrorModel>(paramClauseDescs);
            }
            catch (System.Exception ex)
            {
                ////ToDo: Mensaje de error en caso de fallo
                return new UTMO.ResultError<List<ParamClauseDesc>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(new List<string>() { Resources.Errors.ErrorGetPerils }, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }
    }
}
