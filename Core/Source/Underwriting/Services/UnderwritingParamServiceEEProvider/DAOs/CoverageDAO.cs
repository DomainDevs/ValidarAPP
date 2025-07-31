// -----------------------------------------------------------------------
// <copyright file="CoverageDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Heidy Pinto</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.EEProvider.DAOs
{
    using Sistran.Co.Application.Data;
    using Sistran.Core.Application.UnderwritingParamService.EEProvider.Assemblers;
    using Sistran.Core.Application.UnderwritingParamService.Models;
    using Sistran.Core.Application.UnderwritingParamServices.EEProvider.Assemblers;
    using Sistran.Core.Application.UnderwritingParamServices.EEProvider.DAOs;
    using Sistran.Core.Application.Utilities.DataFacade;
    using Sistran.Core.Framework.DAF;
    using Sistran.Core.Framework.DAF.Engine;
    using Sistran.Core.Framework.Queries;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using COMMEN = Sistran.Core.Application.Common.Entities;
    using ENUMUT = Sistran.Core.Application.Utilities.Enums;
    using QUOEN = Sistran.Core.Application.Quotation.Entities;
    using UPENTV = Sistran.Core.Application.UnderwritingParamService.EEProvider.Entities.Views;
    using UTILEN = Sistran.Core.Services.UtilitiesServices.Enums;
    using UTILMO = Sistran.Core.Services.UtilitiesServices.Models;
    using UTMO = Sistran.Core.Application.Utilities.Error;
    /// <summary>
    /// Acceso a DB de cobertura
    /// </summary>
    public class CoverageDAO
    {
        /// <summary>
        /// Obtener listado de coberturas
        /// </summary>        
        /// <returns>listado de coberturas</returns>
        public UTMO.Result<List<ParamCoverage>, UTMO.ErrorModel> GetParamCoverages()
        {
            try
            {                 
                //ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                //filter.Property(QUOEN.CoCoverage.Properties.IsChild, typeof(QUOEN.CoCoverage).Name);
                //filter.Equal();
                //filter.Constant(0);
                ViewBuilder builder = new ViewBuilder("ParamCoverageView");
                //builder.Filter = filter.GetPredicate();
                UPENTV.ParamCoverageView view = new UPENTV.ParamCoverageView();
                DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
                List<ParamCoverage> paramCoverages = ModelAssembler.CreateParamCoverages(view.Coverage);
                paramCoverages.AsParallel().ForAll(b => b.CoCoverage = (view.CoCoverages.Cast<QUOEN.CoCoverage>().Where(c => c.CoverageId == b.Id).Count() > 0) ? (ModelAssembler.CreateParamCoCoverage(view.CoCoverages.Cast<QUOEN.CoCoverage>().FirstOrDefault(c => c.CoverageId == b.Id))) : null);
                paramCoverages.AsParallel().ForAll(b => b.Peril.Description = view.Perils.Cast<QUOEN.Peril>().FirstOrDefault(c => c.PerilCode == b.Peril.Id).Description);
                paramCoverages.AsParallel().ForAll(b => b.InsuredObjectDesc.Description = view.InsuredObjects.Cast<QUOEN.InsuredObject>().FirstOrDefault(c => c.InsuredObjectId == b.InsuredObjectDesc.Id).Description);
                paramCoverages.AsParallel().ForAll(b => b.LineBusiness.Description = view.LineBusiness.Cast<COMMEN.LineBusiness>().FirstOrDefault(c => c.LineBusinessCode == b.LineBusiness.Id).Description);
                paramCoverages.AsParallel().ForAll(b => b.SubLineBusiness.Description = view.SubLineBusiness.Cast<COMMEN.SubLineBusiness>().FirstOrDefault(c => c.SubLineBusinessCode == b.SubLineBusiness.Id).Description);
                paramCoverages.AsParallel().ForAll(b => b.Homologation2G = ModelAssembler.CreateCoCoverage2G(view.CoEquivalenceCoverage.Cast<Integration.Entities.CoEquivalenceCoverage>().FirstOrDefault(p => p.Coverage3gId == b.Id), b.LineBusiness.Id));
                return new UTMO.ResultValue<List<ParamCoverage>, UTMO.ErrorModel>(paramCoverages);
            }
            catch (System.Exception ex)
            {
                return new UTMO.ResultError<List<ParamCoverage>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(new List<string>() { Resources.Errors.ErrorGetCoveragesDB }, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Obtiene coberturas relacionadas a la descripcion
        /// </summary>
        /// <param name="description">nombre de cobertura</param>
        /// <param name="technicalBranchId">id de ramo tecnico</param>
        /// <returns>listado de coberturas</returns>
        public UTMO.Result<List<ParamCoverage>, UTMO.ErrorModel> GetParamCoveragesByDescription(string description, int? technicalBranchId)
        {
            try
            {
                bool resultcoverageId = int.TryParse(description, out int coverageId);
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();                
                if (technicalBranchId != null)
                {
                    filter.Property(QUOEN.Coverage.Properties.LineBusinessCode, typeof(QUOEN.Coverage).Name);
                    filter.Equal();
                    filter.Constant(technicalBranchId);
                    filter.And();
                }

                if (resultcoverageId)
                {
                    filter.Property(QUOEN.Coverage.Properties.CoverageId, typeof(QUOEN.Coverage).Name);
                    filter.Equal();
                    filter.Constant(coverageId);
                }
                else
                {
                    filter.Property(QUOEN.Coverage.Properties.PrintDescription, typeof(QUOEN.Coverage).Name);
                    filter.Like();
                    filter.Constant("%" + description + "%");
                }

                //filter.And();
                //filter.Property(QUOEN.CoCoverage.Properties.IsChild, typeof(QUOEN.CoCoverage).Name);
                //filter.Equal();
                //filter.Constant(0);
                UPENTV.ParamCoverageView view = new UPENTV.ParamCoverageView();
                ViewBuilder builder = new ViewBuilder("ParamCoverageView");
                builder.Filter = filter.GetPredicate();
                DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
                List<ParamCoverage> paramCoverages = ModelAssembler.CreateParamCoverages(view.Coverage);
                paramCoverages.AsParallel().ForAll(b => b.CoCoverage = ModelAssembler.CreateParamCoCoverage(view.CoCoverages.Cast<QUOEN.CoCoverage>().Where(c => c.CoverageId == b.Id).FirstOrDefault()));
                paramCoverages.AsParallel().ForAll(b => b.CoCoverages = ModelAssembler.CreateParamCoCoverages(view.CoCoverages.Cast<QUOEN.CoCoverage>().Where(c => c.CoverageId == b.Id&& c.CoverageNum!=b.CoCoverage.Id).ToList()));
                paramCoverages.AsParallel().ForAll(b => b.Peril.Description = view.Perils.Cast<QUOEN.Peril>().Where(c => c.PerilCode == b.Peril.Id).FirstOrDefault().Description);
                paramCoverages.AsParallel().ForAll(b => b.InsuredObjectDesc.Description = view.InsuredObjects.Cast<QUOEN.InsuredObject>().Where(c => c.InsuredObjectId == b.InsuredObjectDesc.Id).FirstOrDefault().Description);
                paramCoverages.AsParallel().ForAll(b => b.LineBusiness.Description = view.LineBusiness.Cast<COMMEN.LineBusiness>().Where(c => c.LineBusinessCode == b.LineBusiness.Id).FirstOrDefault().Description);
                paramCoverages.AsParallel().ForAll(b => b.Homologation2G = ModelAssembler.CreateCoCoverage2G(view.CoEquivalenceCoverage.Cast<Integration.Entities.CoEquivalenceCoverage>().FirstOrDefault(p=> p.Coverage3gId == b.Id), b.LineBusiness.Id));
                return new UTMO.ResultValue<List<ParamCoverage>, UTMO.ErrorModel>(paramCoverages);
            }
            catch (System.Exception ex)
            {
                return new UTMO.ResultError<List<ParamCoverage>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(new List<string>() { Resources.Errors.ErrorGetCoveragesDB }, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Consulta de coberturas, busqueda avanzada 
        /// </summary>
        /// <param name="paramCoverage">cobertura con filtros de busqueda</param>
        /// <returns>listado de coberturas</returns>
        public UTMO.Result<List<ParamCoverage>, UTMO.ErrorModel> GetParamCoveragesByParamCoverage(ParamCoverage paramCoverage)
        {
            try
            {                
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                bool bandAnd = false;
                if (paramCoverage.Peril != null && !string.IsNullOrEmpty(paramCoverage.Peril.Description))
                {
                    filter.Property(QUOEN.Peril.Properties.Description, typeof(QUOEN.Peril).Name);
                    filter.Like();                    
                    filter.Constant("%" + paramCoverage.Peril.Description + "%"); 
                    bandAnd = true;
                }

                if (paramCoverage.InsuredObjectDesc != null && paramCoverage.InsuredObjectDesc.Id > 0)
                {
                    if (bandAnd)
                    {
                        filter.And();
                    }

                    filter.Property(QUOEN.Coverage.Properties.InsuredObjectId, typeof(QUOEN.Coverage).Name);
                    filter.Equal();
                    filter.Constant(paramCoverage.InsuredObjectDesc.Id);
                    bandAnd = true;
                }

                if (paramCoverage.LineBusiness != null && paramCoverage.LineBusiness.Id > 0)
                {
                    if (bandAnd)
                    {
                        filter.And();
                    }

                    filter.Property(QUOEN.Coverage.Properties.LineBusinessCode, typeof(QUOEN.Coverage).Name);
                    filter.Equal();
                    filter.Constant(paramCoverage.LineBusiness.Id);
                    bandAnd = true;
                }

                if (!string.IsNullOrEmpty(paramCoverage.Description))
                {
                    if (bandAnd)
                    {
                        filter.And();
                    }

                    filter.Property(QUOEN.Coverage.Properties.PrintDescription, typeof(QUOEN.Coverage).Name);
                    filter.Like();
                    filter.Constant("%" + paramCoverage.Description + "%");
                    bandAnd = true;
                }

                //if (bandAnd)
                //{
                //    filter.And();
                //}
                //filter.Property(QUOEN.CoCoverage.Properties.IsChild, typeof(QUOEN.CoCoverage).Name);
                //filter.Equal();
                //filter.Constant(0);

                UPENTV.ParamCoverageView view = new UPENTV.ParamCoverageView();
                ViewBuilder builder = new ViewBuilder("ParamCoverageView");
                builder.Filter = filter.GetPredicate();
                DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
                List<ParamCoverage> paramCoverages = ModelAssembler.CreateParamCoverages(view.Coverage);
                paramCoverages.AsParallel().ForAll(b => b.CoCoverage = ModelAssembler.CreateParamCoCoverage(view.CoCoverages.Cast<QUOEN.CoCoverage>().Where(c => c.CoverageId == b.Id).FirstOrDefault()));
                paramCoverages.AsParallel().ForAll(b => b.Peril.Description = view.Perils.Cast<QUOEN.Peril>().Where(c => c.PerilCode == b.Peril.Id).FirstOrDefault().Description);
                paramCoverages.AsParallel().ForAll(b => b.InsuredObjectDesc.Description = view.InsuredObjects.Cast<QUOEN.InsuredObject>().Where(c => c.InsuredObjectId == b.InsuredObjectDesc.Id).FirstOrDefault().Description);
                paramCoverages.AsParallel().ForAll(b => b.LineBusiness.Description = view.LineBusiness.Cast<COMMEN.LineBusiness>().Where(c => c.LineBusinessCode == b.LineBusiness.Id).FirstOrDefault().Description);
                paramCoverages.AsParallel().ForAll(b => b.Homologation2G = ModelAssembler.CreateCoCoverage2G(view.CoEquivalenceCoverage.Cast<Integration.Entities.CoEquivalenceCoverage>().FirstOrDefault(p => p.Coverage3gId == b.Id), b.LineBusiness.Id));
                return new UTMO.ResultValue<List<ParamCoverage>, UTMO.ErrorModel>(paramCoverages);
            }
            catch (System.Exception ex)
            {
                return new UTMO.ResultError<List<ParamCoverage>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(new List<string>() { Resources.Errors.ErrorGetCoveragesDB }, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Generar archivo excel de coberturas
        /// </summary>
        /// <param name="paramCoverages">listado de coberturas</param>
        /// <param name="fileName">nombre de archivo</param>
        /// <returns>archivo excel </returns>
        public UTMO.Result<string, UTMO.ErrorModel> GenerateFileToCoverage(List<ParamCoverage> paramCoverages, string fileName)
        {
            try
            {
                UTILMO.FileProcessValue fileProcessValue = new UTILMO.FileProcessValue()
                {
                    Key1 = (int)UTILEN.FileProcessType.ParametrizationCoverage
                };
                FileDAO fileDAO = new FileDAO();
                UTILMO.File file = fileDAO.GetFileByFileProcessValue(fileProcessValue);

                if (file != null && file.IsEnabled)
                {
                    file.Name = fileName;
                    List<UTILMO.Row> rows = new List<UTILMO.Row>();

                    foreach (ParamCoverage item in paramCoverages)
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

                        fields[0].Value = item.Id.ToString();                        
                        fields[1].Value = item.LineBusiness.Description;
                        fields[2].Value = item.SubLineBusiness.Description;
                        fields[3].Value = item.InsuredObjectDesc.Description;
                        fields[4].Value = item.Peril.Description;
                        fields[5].Value = item.Description;
                        fields[6].Value = item.CoCoverage?.Description;
                        fields[7].Value = item.CoCoverage?.ImpressionValue;

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
                    return new UTMO.ResultError<string, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(new List<string>() { Resources.Errors.ErrorDownloadingExcel }, ENUMUT.ErrorType.TechnicalFault, new System.ArgumentException(Resources.Errors.ErrorDownloadingExcel, "original")));
                }
            }
            catch (System.Exception ex)
            {
                return new UTMO.ResultError<string, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(new List<string>() { Resources.Errors.ErrorDownloadingExcel }, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Creacion de cobertura
        /// </summary>
        /// <param name="paramCoverage">cobertura a crear</param>
        /// <returns>cobertura creada</returns>
        public UTMO.Result<ParamCoverage, UTMO.ErrorModel> CreateParamCoverage(ParamCoverage paramCoverage)
        {
            try
            {
                QUOEN.Coverage coverageEntity = EntityAssembler.CreateCoverage(paramCoverage);
                DataFacadeManager.Instance.GetDataFacade().InsertObject(coverageEntity);
                //Ensamblar, insert CoverageOrder
                //QUOEN.CoverageOrder coverageOrder = new QUOEN.CoverageOrder(coverageEntity.LineBusinessCode, coverageEntity.SubLineBusinessCode, coverageEntity.CoverageId, coverageEntity.InsuredObjectId)
                //{
                //    LineBusinessCode = coverageEntity.LineBusinessCode,
                //    SubLineBusinessCode = coverageEntity.SubLineBusinessCode,
                //    CoverageId = coverageEntity.CoverageId,
                //    InsuredObjectId = coverageEntity.InsuredObjectId,
                //    NameOrder = coverageEntity.PrintDescription
                //};
              //  DataFacadeManager.Instance.GetDataFacade().InsertObject(coverageOrder);
                ParamCoverage paramCoverageResult = ModelAssembler.CreateParamCoverage(coverageEntity);
                if (paramCoverage.Homologation2G != null)
                {
                    Coverage2GDAO coverage2GDao = new Coverage2GDAO();
                    UTMO.Result<bool, UTMO.ErrorModel> resultCreateHomologation = coverage2GDao.CreateHomologationCoverage(paramCoverage, paramCoverageResult.Id);
                    if (resultCreateHomologation is UTMO.ResultError<bool, UTMO.ErrorModel>)
                    {
                        UTMO.ResultError<bool, UTMO.ErrorModel> resultErrorCreateHomologation = (UTMO.ResultError<bool, UTMO.ErrorModel>)resultCreateHomologation;
                        return new UTMO.ResultError<ParamCoverage, UTMO.ErrorModel>(resultErrorCreateHomologation.Message);
                    }
                }
                return new UTMO.ResultValue<ParamCoverage, UTMO.ErrorModel>(paramCoverageResult);
            }
            catch (System.Exception ex)
            {
                return new UTMO.ResultError<ParamCoverage, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(new List<string>() { Resources.Errors.ErrorDBCreateCoverage }, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Actualizacion de cobertura
        /// </summary>
        /// <param name="paramCoverage">cobertura a actualizar</param>
        /// <returns>cobertura actualizada</returns>
        public UTMO.Result<ParamCoverage, UTMO.ErrorModel> UpdateParamCoverage(ParamCoverage paramCoverage)
        {
            try
            {
                PrimaryKey key = QUOEN.Coverage.CreatePrimaryKey(paramCoverage.Id);
                QUOEN.Coverage coverageEntity = (QUOEN.Coverage)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                coverageEntity.PrintDescription = paramCoverage.CoCoverage.Description;
                coverageEntity.LineBusinessCode = paramCoverage.LineBusiness.Id;
                coverageEntity.SubLineBusinessCode = paramCoverage.SubLineBusiness.Id;
                coverageEntity.PerilCode = paramCoverage.Peril.Id;
                coverageEntity.InsuredObjectId = paramCoverage.InsuredObjectDesc.Id;
                coverageEntity.IsPrimary = paramCoverage.IsPrincipal;
                coverageEntity.CompositionTypeCode = paramCoverage.CompositionTypeId;
                DataFacadeManager.Instance.GetDataFacade().UpdateObject(coverageEntity);

                //Ensamblar, update CoverageOrder
                //PrimaryKey keyOrder = QUOEN.CoverageOrder.CreatePrimaryKey(coverageEntity.LineBusinessCode, coverageEntity.SubLineBusinessCode, coverageEntity.CoverageId, coverageEntity.InsuredObjectId);
                //QUOEN.CoverageOrder coverageOrderOld = (QUOEN.CoverageOrder)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(keyOrder);
                //first delete
                //DataFacadeManager.Instance.GetDataFacade().DeleteObject(coverageOrderOld);
                //final insert new
                //QUOEN.CoverageOrder coverageOrderNew = new QUOEN.CoverageOrder(coverageEntity.LineBusinessCode, coverageEntity.SubLineBusinessCode, coverageEntity.CoverageId, coverageEntity.InsuredObjectId)
                //{
                //    LineBusinessCode = coverageEntity.LineBusinessCode,
                //    SubLineBusinessCode = coverageEntity.SubLineBusinessCode,
                //    CoverageId = coverageEntity.CoverageId,
                //    InsuredObjectId = coverageEntity.InsuredObjectId,
                //    NameOrder = coverageEntity.PrintDescription
                //};
                //DataFacadeManager.Instance.GetDataFacade().InsertObject(coverageOrderNew);

                if (paramCoverage.Homologation2G != null)
                {
                    Coverage2GDAO coverage2GDAO = new Coverage2GDAO();
                    var resultDeleteHomologation = coverage2GDAO.DeleteHomologationCoverage(paramCoverage);
                    if (resultDeleteHomologation is UTMO.ResultError<bool, UTMO.ErrorModel>)
                    {
                        return new UTMO.ResultError<ParamCoverage, UTMO.ErrorModel>(((UTMO.ResultError<bool, UTMO.ErrorModel>)resultDeleteHomologation).Message);
                    }
                    var resultCreateHomologation = coverage2GDAO.CreateHomologationCoverage(paramCoverage, paramCoverage.Id);
                    if (resultCreateHomologation is UTMO.ResultError<bool, UTMO.ErrorModel>)
                    {
                        return new UTMO.ResultError<ParamCoverage, UTMO.ErrorModel>(((UTMO.ResultError<bool, UTMO.ErrorModel>)resultCreateHomologation).Message);
                    }
                }
                ParamCoverage paramCoverageResult = ModelAssembler.CreateParamCoverage(coverageEntity);
                return new UTMO.ResultValue<ParamCoverage, UTMO.ErrorModel>(paramCoverageResult);
            }
            catch (System.Exception ex)
            {
                return new UTMO.ResultError<ParamCoverage, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(new List<string>() { Resources.Errors.ErrorDBUpdateCoverage }, ENUMUT.ErrorType.TechnicalFault, ex));
            }

        }

        /// <summary>
        /// Eliminacion de cobertura
        /// </summary>
        /// <param name="paramCoverage">cobertura a eliminar</param>
        /// <param name="isPrincipal">eliminacion de padres e hijos</param>
        /// <returns>cobertura eliminada</returns>
        public UTMO.Result<ParamCoverage, UTMO.ErrorModel> DeleteParamCoverage(ParamCoverage paramCoverage, bool isPrincipal)
        {
            try
            {
                DataTable result;
                NameValue[] parameters = new NameValue[2];
                parameters[0] = new NameValue("@COVERAGE_ID", paramCoverage.Id);
                parameters[1] = new NameValue("@PRINCIPAL", isPrincipal);
                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    result = dynamicDataAccess.ExecuteSPDataTable("QUO.DELETE_COVERAGE_PARAMETRIZATION", parameters);
                }

                if (((int)result.Rows[0][0]) == 1)
                {
                    paramCoverage.CoCoverage = null;                     
                    return new UTMO.ResultValue<ParamCoverage, UTMO.ErrorModel>(paramCoverage);
                }

                if (((int)result.Rows[0][0]) == -1)
                {
                    return new UTMO.ResultError<ParamCoverage, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(new List<string>() { Resources.Errors.ErrorUseDeleteCoverage }, ENUMUT.ErrorType.TechnicalFault, null));
                }

                return new UTMO.ResultError<ParamCoverage, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(new List<string>() { Resources.Errors.ErrorDeleteCoverage }, ENUMUT.ErrorType.TechnicalFault, null));
            }
            catch (System.Exception ex)
            {
                return new UTMO.ResultError<ParamCoverage, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(new List<string>() { Resources.Errors.ErrorDeleteCoverage }, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Creacion de cocoverage (datos de impresion de cobertura)
        /// </summary>
        /// <param name="paramCocoverage">cocoverage a crear</param>
        /// <param name="coverageId">id de cobertura</param>
        /// <returns>cocoverage creada</returns>
        public UTMO.Result<ParamCoCoverage, UTMO.ErrorModel> CreateCoCoverage(ParamCoCoverage paramCocoverage, int coverageId)
        {
            try
            {
                QUOEN.CoCoverage cocoverageEntity = EntityAssembler.CreateCoCoverage(paramCocoverage, coverageId);
                DataFacadeManager.Instance.GetDataFacade().InsertObject(cocoverageEntity);
                ParamCoCoverage paramCoCoverageResult = ModelAssembler.CreateParamCoCoverage(cocoverageEntity);
                return new UTMO.ResultValue<ParamCoCoverage, UTMO.ErrorModel>(paramCoCoverageResult);
            }
            catch (System.Exception ex)
            {
                return new UTMO.ResultError<ParamCoCoverage, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(new List<string>() { Resources.Errors.ErrorDBCreateCoCoverage }, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Consulta de coberturas relacionadas 
        /// </summary>
        /// <param name="lineBusinessId">id de ramo tecnico</param>
        /// <param name="subLineBusinessId">id de subramo tecnico</param>
        /// <param name="perilId">id de amparo</param>
        /// <param name="insuredObjectDescId">id objeto del seguro</param>
        /// <returns>listado de coberturas</returns>
        public UTMO.Result<List<ParamCoverage>, UTMO.ErrorModel> GetParamCoveragesByLineBusinessIdSubLineBusinessId(int lineBusinessId, int subLineBusinessId, int perilId, int insuredObjectDescId)
        {
            try
            {                
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(QUOEN.Coverage.Properties.LineBusinessCode, typeof(QUOEN.Coverage).Name);
                filter.Equal();
                filter.Constant(lineBusinessId);
                filter.And();
                filter.Property(QUOEN.Coverage.Properties.SubLineBusinessCode, typeof(QUOEN.Coverage).Name);
                filter.Equal();
                filter.Constant(subLineBusinessId);
                filter.And();
                filter.Property(QUOEN.Coverage.Properties.PerilCode, typeof(QUOEN.Coverage).Name);
                filter.Equal();
                filter.Constant(perilId);
                filter.And();
                filter.Property(QUOEN.Coverage.Properties.InsuredObjectId, typeof(QUOEN.Coverage).Name);
                filter.Equal();
                filter.Constant(insuredObjectDescId);                
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(QUOEN.Coverage), filter.GetPredicate()));                
                List<ParamCoverage> paramCoverages = ModelAssembler.CreateParamCoverages(businessCollection);                
                return new UTMO.ResultValue<List<ParamCoverage>, UTMO.ErrorModel>(paramCoverages);
            }
            catch (System.Exception ex)
            {
                return new UTMO.ResultError<List<ParamCoverage>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(new List<string>() { Resources.Errors.ErrorGetCoveragesDB }, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Consulta de coberturas relacionadas con la descripcion
        /// </summary>
        /// <param name="description">nombre de la cobertura</param>
        /// <returns>Listado de coberturas</returns>
        public UTMO.Result<List<ParamCoverage>, UTMO.ErrorModel> GetParamCoveragesByDescription(string description)
        {
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(QUOEN.Coverage.Properties.PrintDescription, typeof(QUOEN.Coverage).Name);
                filter.Equal();
                filter.Constant(description);
                filter.And();
                filter.Property(QUOEN.CoCoverage.Properties.IsChild, typeof(QUOEN.CoCoverage).Name);
                filter.Equal();
                filter.Constant(0);
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(QUOEN.Coverage), filter.GetPredicate()));
                List<ParamCoverage> paramCoverages = ModelAssembler.CreateParamCoverages(businessCollection);
                return new UTMO.ResultValue<List<ParamCoverage>, UTMO.ErrorModel>(paramCoverages);
            }
            catch (System.Exception ex)
            {
                return new UTMO.ResultError<List<ParamCoverage>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(new List<string>() { Resources.Errors.ErrorGetCoveragesDB }, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Obtiene coberturas relacionadas a la descripcion
        /// </summary>
        /// <param name="description">nombre de cobertura</param>
        /// <param name="technicalBranchId">id de ramo tecnico</param>
        /// <returns>listado de coberturas</returns>
        public UTMO.Result<ParamCoverage, UTMO.ErrorModel> GetParamCoverageById(int coverageId)
        {
            ParamCoverage resultCoverage;
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(QUOEN.Coverage.Properties.CoverageId, typeof(QUOEN.Coverage).Name);
                filter.Equal();
                filter.Constant(coverageId);                
                UPENTV.ParamCoverageView view = new UPENTV.ParamCoverageView();
                ViewBuilder builder = new ViewBuilder("ParamCoverageView");
                builder.Filter = filter.GetPredicate();
                DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
                List<ParamCoverage> paramCoverages = ModelAssembler.CreateParamCoverages(view.Coverage);
                paramCoverages.AsParallel().ForAll(b => b.CoCoverage = ModelAssembler.CreateParamCoCoverage(view.CoCoverages.Cast<QUOEN.CoCoverage>().Where(c => c.CoverageId == b.Id).FirstOrDefault()));
                paramCoverages.AsParallel().ForAll(b => b.Peril.Description = view.Perils.Cast<QUOEN.Peril>().Where(c => c.PerilCode == b.Peril.Id).FirstOrDefault().Description);
                paramCoverages.AsParallel().ForAll(b => b.InsuredObjectDesc.Description = view.InsuredObjects.Cast<QUOEN.InsuredObject>().Where(c => c.InsuredObjectId == b.InsuredObjectDesc.Id).FirstOrDefault().Description);
                paramCoverages.AsParallel().ForAll(b => b.LineBusiness.Description = view.LineBusiness.Cast<COMMEN.LineBusiness>().Where(c => c.LineBusinessCode == b.LineBusiness.Id).FirstOrDefault().Description);
                resultCoverage = paramCoverages.FirstOrDefault();
                return new UTMO.ResultValue<ParamCoverage, UTMO.ErrorModel>(resultCoverage);
            }
            catch (System.Exception ex)
            {
                return new UTMO.ResultError<ParamCoverage, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(new List<string>() { Resources.Errors.ErrorGetCoveragesDB }, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }
    }
}
