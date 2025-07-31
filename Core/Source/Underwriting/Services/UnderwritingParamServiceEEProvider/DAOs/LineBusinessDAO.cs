// -----------------------------------------------------------------------
// <copyright file="ParamInsuredObjectModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Andres F. Gonzalez R.</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.EEProvider.DAOs
{
    using Framework.DAF;
    using Sistran.Co.Application.Data;
    using Sistran.Core.Application.UnderwritingParamService.EEProvider.Assemblers;
    using Sistran.Core.Application.UnderwritingParamService.EEProvider.Resources;
    using Sistran.Core.Application.UnderwritingParamService.EEProviderWeb.Views;
    using Sistran.Core.Application.UnderwritingParamService.Models;
    using Sistran.Core.Application.UnderwritingParamServices.EEProvider.Assemblers;
    using Sistran.Core.Application.UnderwritingParamServices.EEProvider.DAOs;
    using Sistran.Core.Application.UnderwritingServices.Models;
    using Sistran.Core.Framework;
    using Sistran.Core.Framework.DAF.Engine;
    using Sistran.Core.Framework.Queries;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.Linq;
    using Utilities.DataFacade;
    using COMMEN = Sistran.Core.Application.Common.Entities;
    using COMMO = CommonService.Models;
    using ENUMUT = Sistran.Core.Application.Utilities.Enums;
    using PARAMEN = Sistran.Core.Application.Parameters.Entities;
    using QUOEN = Sistran.Core.Application.Quotation.Entities;
    using UTILEN = Sistran.Core.Services.UtilitiesServices.Enums;
    using UTILMO = Sistran.Core.Services.UtilitiesServices.Models;
    using UTMO = Sistran.Core.Application.Utilities.Error;
    /// <summary>
    /// Clase pública Ramo tecnico
    /// </summary>
    public class LineBusinessDAO
    {
        /// <summary>
        /// Obtiene ramos tecnicos
        /// </summary>
        /// <returns>MS-Ramos tecnicos</returns>
        public UTMO.Result<List<ParamLineBusinessModel>, UTMO.ErrorModel> GetLinesBusiness()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            try
            {
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.LineBusiness)));
                UTMO.Result<List<ParamLineBusinessModel>, UTMO.ErrorModel> lstLineBusiness = ModelAssembler.CreateLinesBusiness(businessCollection);
                if (lstLineBusiness is UTMO.ResultError<List<ParamLineBusinessModel>, UTMO.ErrorModel>)
                {
                    return lstLineBusiness;
                }
                else
                {
                    List<ParamLineBusinessModel> resultValue = (lstLineBusiness as UTMO.ResultValue<List<ParamLineBusinessModel>, UTMO.ErrorModel>).Value;
                    if (resultValue.Count == 0)
                    {
                        errorModelListDescription.Add(UnderwritingParamService.EEProvider.Resources.Errors.LineBusinessNotFound);
                        return new UTMO.ResultError<List<ParamLineBusinessModel>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(errorModelListDescription, ENUMUT.ErrorType.NotFound, null));
                    }
                    else
                    {
                        return lstLineBusiness;
                    }
                }
            }
            catch (Exception ex)
            {
                errorModelListDescription.Add(UnderwritingParamService.EEProvider.Resources.Errors.LineBusinessThecnicalError);
                return new UTMO.ResultError<List<ParamLineBusinessModel>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(errorModelListDescription, ENUMUT.ErrorType.TechnicalFault, ex));
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingParamServices.EEProvider.DAOs");
            }
        }

        /// <summary>
        /// Obtener lista de subramos tecnicos
        /// </summary>
        /// <param name="lineBusinessId">Id ramo tecnico</param>
        /// <returns> RESULT-lista de subramos tecnicos</returns>
        public UTMO.Result<List<COMMO.SubLineBusiness>, UTMO.ErrorModel> GetSubLinesBusinessByLineBusinessId(int lineBusinessId)
        {
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(COMMEN.SubLineBusiness.Properties.LineBusinessCode, typeof(COMMEN.SubLineBusiness).Name);
                filter.Equal();
                filter.Constant(lineBusinessId);
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.SubLineBusiness), filter.GetPredicate()));
                List<COMMO.SubLineBusiness> subLineBusiness = ModelAssembler.CreateSubLinesBusiness(businessCollection);
                return new UTMO.ResultValue<List<COMMO.SubLineBusiness>, UTMO.ErrorModel>(subLineBusiness);
            }
            catch (System.Exception ex)
            {
                return new UTMO.ResultError<List<COMMO.SubLineBusiness>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(new List<string>() { Resources.Errors.ErrorDBGetSubLineBusinessRelation }, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        #region Parametrización ramo técnico
        /// <summary>
        /// Crea el ramo técnico
        /// </summary>
        /// <param name="lineBusiness">Nuevo ramo técnico</param>
        /// <returns>Ramo técnico</returns>
        public UTMO.Result<ParamLineBusinessModel, UTMO.ErrorModel> CreateLineBussiness(ParamLineBusinessModel lineBusiness)
        {
            try
            {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                COMMEN.LineBusiness lineBusinessEntity = EntityAssembler.CreateLineBusiness(lineBusiness);
                DataFacadeManager.Instance.GetDataFacade().InsertObject(lineBusinessEntity);
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.UnderwritingServices.EEProvider.DAOs.SaveLineBusiness");
                return ModelAssembler.CreateLineBusiness(lineBusinessEntity);
            }
            catch (System.Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add(Resources.Errors.ErrorCreateLineBusinessParametrization);
                return new UTMO.ResultError<ParamLineBusinessModel, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Actualiza el ramo técnico
        /// </summary>
        /// <param name="lineBusiness">ramo técnico</param>
        /// <returns>Ramo técnico actualizado</returns>
        public UTMO.Result<ParamLineBusinessModel, UTMO.ErrorModel> UpdateLineBussiness(ParamLineBusinessModel lineBusiness)
        {
            try
            {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                COMMEN.LineBusiness lineBusinessEntity = new COMMEN.LineBusiness(lineBusiness.Id);
                PrimaryKey key = COMMEN.LineBusiness.CreatePrimaryKey(lineBusinessEntity.LineBusinessCode);
                lineBusinessEntity = (COMMEN.LineBusiness)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                lineBusinessEntity.Description = lineBusiness.Description;
                lineBusinessEntity.SmallDescription = lineBusiness.SmallDescription;
                lineBusinessEntity.TinyDescription = lineBusiness.TinyDescription;
                lineBusinessEntity.ReportLineBusinessCode = lineBusiness.Id.ToString();
                DataFacadeManager.Instance.GetDataFacade().UpdateObject(lineBusinessEntity);

                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.UnderwritingServices.EEProvider.DAOs.SaveLineBusiness");
                return new UTMO.ResultValue<ParamLineBusinessModel, UTMO.ErrorModel>(lineBusiness);
            }
            catch (System.Exception ex)
            {
                return new UTMO.ResultError<ParamLineBusinessModel, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(new List<string>() { Errors.ErrorUdateLineBusiness }, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Consulta el ramo técnico
        /// </summary>
        /// <param name="lineBusinessId">Id del ramo técnico</param>
        /// <returns>Ramo técnico</returns>
        public UTMO.Result<ParamLineBusiness, UTMO.ErrorModel> GetParametrizationLineBusinessByLineBusinessId(int lineBusinessId)
        {
            try
            {
                List<ParamLineBusiness> listParametrizationLineBusiness = new List<ParamLineBusiness>();

                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(COMMEN.LineBusiness.Properties.Description, typeof(COMMEN.LineBusiness).Name);
                filter.Equal();
                filter.Constant(lineBusinessId);

                ParametrizationLineBusinessView view = new ParametrizationLineBusinessView();
                ViewBuilder builder = new ViewBuilder("ParametrizationLineBusinessView");
                builder.Filter = filter.GetPredicate();
                DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

                ParamLineBusiness parametrization = ModelAssembler.CreateParamLineBusiness(view.LineBusiness.Cast<COMMEN.LineBusiness>().First());

                List<QUOEN.Peril> entityPeriles = view.Perils.Cast<QUOEN.Peril>().ToList();
                List<PARAMEN.CoveredRiskType> entityCoveredRiskType = view.CoveredRiskTypes.Cast<PARAMEN.CoveredRiskType>().ToList();

                parametrization.CoveredRiskTypes = view.LineBusinessCoveredRiskTypes.Cast<COMMEN.LineBusinessCoveredRiskType>().Where(x => x.LineBusinessCode == parametrization.Id).Select(x => x.CoveredRiskTypeCode).ToList();
                List<QUOEN.InsObjLineBusiness> insuredObjets = view.InsObjLineBusinesses.Cast<QUOEN.InsObjLineBusiness>().Where(x => x.LineBusinessCode == parametrization.Id).ToList();
                parametrization.InsuredObjects = new List<ParamInsuredObjectDesc>();
                foreach (QUOEN.InsObjLineBusiness ins in insuredObjets)
                {
                    parametrization.InsuredObjects.Add(ModelAssembler.CreateParamInsuredObjectDesc(view.InsuredObjects.Cast<QUOEN.InsuredObject>().First(x => x.InsuredObjectId == ins.InsuredObjectId)));
                }

                List<QUOEN.PerilLineBusiness> perilsLB = view.PerilLineBusinesses.Cast<QUOEN.PerilLineBusiness>().Where(x => x.LineBusinessCode == parametrization.Id).ToList();
                parametrization.Perils = new List<Peril>();
                foreach (QUOEN.PerilLineBusiness perilLB in perilsLB)
                {
                    parametrization.Perils.Add(ModelAssembler.CreatePeril(view.Perils.Cast<QUOEN.Peril>().First(x => x.PerilCode == perilLB.PerilCode)));
                }

                return new UTMO.ResultValue<ParamLineBusiness, UTMO.ErrorModel>(parametrization);
            }
            catch (System.Exception ex)
            {
                return new UTMO.ResultError<ParamLineBusiness, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(new List<string>() { Errors.ErrorCreateLineBusinessParametrization }, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Consulta los ramos técnicos que contengan la descripción o el id
        /// </summary>
        /// <param name="description">Description del ramo técnico</param>
        /// <param name="id">Id del ramo técnico</param>
        /// <returns>Bool de respuesta</returns>
        public UTMO.Result<bool?, UTMO.ErrorModel> GetLineBusinessByDescriptionById(string description, int id)
        {
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

                filter.Property(COMMEN.LineBusiness.Properties.Description, typeof(COMMEN.LineBusiness).Name);
                filter.Equal();
                filter.Constant(description);
                filter.Or();
                filter.Property(COMMEN.LineBusiness.Properties.LineBusinessCode, typeof(COMMEN.LineBusiness).Name);
                filter.Equal();
                filter.Constant(id);

                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.LineBusiness), filter.GetPredicate()));

                if (businessCollection.Count > 0)
                {
                    return new UTMO.ResultValue<bool?, UTMO.ErrorModel>(true);
                }
                else
                {
                    return new UTMO.ResultValue<bool?, UTMO.ErrorModel>(false);
                }
            }
            catch (System.Exception ex)
            {
                return new UTMO.ResultError<bool?, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(new List<string>() { Errors.ErrorDBGetLineBusiness }, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Consulta los ramos técnicos que contengan la descripción o el id
        /// </summary>
        /// <param name="description">Description del ramo técnico</param>
        /// <param name="id">Id del ramo técnico</param>
        /// <returns>Lista de ramos técnicos</returns>
        public UTMO.Result<List<ParamLineBusiness>, UTMO.ErrorModel> GetParamLineBusinessByDescriptionById(string description, int id)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            if (id == 0)
            {
                filter.Property(COMMEN.LineBusiness.Properties.Description, typeof(COMMEN.LineBusiness).Name);
                filter.Like();
                filter.Constant("%" + description + "%");
            }
            else
            {
                filter.Property(COMMEN.LineBusiness.Properties.LineBusinessCode, typeof(COMMEN.LineBusiness).Name);
                filter.Equal();
                filter.Constant(id);
            }

            return this.GetParamLineBusinessByPredicate(filter.GetPredicate());
        }

        /// <summary>
        /// Consulta los ramos técnicos que contengan la descripción o el tipo de riesgo cubierto
        /// </summary>
        /// <param name="description">Description del ramo técnico</param>
        /// <param name="coveredRiskType">Tipo de riesgo cubierto</param>
        /// <returns>Lista de ramos técnicos</returns>
        public UTMO.Result<List<ParamLineBusiness>, UTMO.ErrorModel> GetParamLineBusinessByAdvancedSearch(string description, int coveredRiskType)
        {
            List<ParamLineBusiness> listParametrizationLineBusiness = new List<ParamLineBusiness>();
            bool bandAnd = false;
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            if (coveredRiskType != 0)
            {
                filter.Property(COMMEN.LineBusinessCoveredRiskType.Properties.CoveredRiskTypeCode, typeof(COMMEN.LineBusinessCoveredRiskType).Name);
                filter.Equal();
                filter.Constant(coveredRiskType);
                bandAnd = true;
            }
            if (!string.IsNullOrEmpty(description))
            {
                if (bandAnd)
                {
                    filter.And();
                }
                filter.Property(COMMEN.LineBusiness.Properties.Description, typeof(COMMEN.LineBusiness).Name);
                filter.Like();
                filter.Constant("%" + description + "%");
                filter.Or();
                filter.Property(COMMEN.LineBusiness.Properties.SmallDescription, typeof(COMMEN.LineBusiness).Name);
                filter.Like();
                filter.Constant("%" + description + "%");
            }
            if(coveredRiskType == 0 && string.IsNullOrEmpty(description))
            {
                return new UTMO.ResultValue<List<ParamLineBusiness>, UTMO.ErrorModel>(new List<ParamLineBusiness>());
            }

            return this.GetParamLineBusinessByPredicate(filter.GetPredicate());
        }

        /// <summary>
        /// Elimina el ramo técnico
        /// </summary>
        /// <param name="parametrizationLineBusiness">Ramo técnico</param>
        /// <returns>Ramo técnico eliminado</returns>
        public UTMO.Result<ParamLineBusinessModel, UTMO.ErrorModel> DeleteParamLineBusiness(ParamLineBusinessModel lineBusiness)
        {
            try
            {
                NameValue[] parameters = new NameValue[1];
                parameters[0] = new NameValue("@LINEBUSINESSCODE", lineBusiness.Id);

                DataTable result = null;

                using (DynamicDataAccess pdb = new DynamicDataAccess())
                {
                    result = pdb.ExecuteSPDataTable("COMM.DELETE_LINE_BUSINESS", parameters);
                }

                if (result != null && result.Rows.Count > 0)
                {
                    switch ((int)result.Rows[0][0])
                    {
                        case -1:
                            return new UTMO.ResultError<ParamLineBusinessModel, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(new List<string>() { (string)result.Rows[0][1] }, ENUMUT.ErrorType.TechnicalFault, new Exception((string)result.Rows[0][1])));
                            throw new ValidationException((string)Errors.ErrorDeletingLineBusinessParametrization);
                        default:
                            break;
                    }
                }
                else
                {
                    return new UTMO.ResultError<ParamLineBusinessModel, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(new List<string>() { Errors.ErrorDeletingLineBusinessParametrization }, ENUMUT.ErrorType.TechnicalFault, null));
                }

                return new UTMO.ResultValue<ParamLineBusinessModel, UTMO.ErrorModel>(lineBusiness);
            }
            catch (System.Exception ex)
            {
                return new UTMO.ResultError<ParamLineBusinessModel, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(new List<string>() { Errors.ErrorDeletingLineBusinessParametrization }, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Genera el archivo excel de ramos técnicos
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <returns>Ruta del archivo</returns>
        public UTMO.Result<string, UTMO.ErrorModel> GenerateParamLineBusiness(string fileName)
        {
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                ParametrizationLineBusinessView view = new ParametrizationLineBusinessView();
                ViewBuilder builder = new ViewBuilder("ParametrizationLineBusinessView");
                builder.Filter = filter.GetPredicate();
                DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

                UTILMO.FileProcessValue fileProcessValue = new UTILMO.FileProcessValue();
                fileProcessValue.Key1 = (int)UTILEN.FileProcessType.ParametrizationLineBusiness;

                FileDAO fileDAO = new FileDAO();
                UTILMO.File file = fileDAO.GetFileByFileProcessValue(fileProcessValue);

                if (file != null && file.IsEnabled)
                {
                    file.Name = fileName;
                    List<UTILMO.Row> rows = new List<UTILMO.Row>();

                    foreach (COMMEN.LineBusiness lineBusiness in view.LineBusiness.Cast<COMMEN.LineBusiness>())
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

                        fields[0].Value = lineBusiness.LineBusinessCode.ToString();
                        fields[1].Value = lineBusiness.Description;
                        fields[2].Value = lineBusiness.SmallDescription;
                        fields[3].Value = lineBusiness.TinyDescription;
                        fields[4].Value = lineBusiness.ReportLineBusinessCode.ToString();

                        List<int> lineBusinesCoveredRiskTypes = view.LineBusinessCoveredRiskTypes.Cast<COMMEN.LineBusinessCoveredRiskType>().Where(x => x.LineBusinessCode == lineBusiness.LineBusinessCode).Select(x => x.CoveredRiskTypeCode).ToList();
                        string coveredRiskTypes = string.Join(",", view.CoveredRiskTypes.Cast<PARAMEN.CoveredRiskType>().Where(x => lineBusinesCoveredRiskTypes.Contains(x.CoveredRiskTypeCode)).Select(x => x.SmallDescription));
                        fields[5].Value = coveredRiskTypes;

                        rows.Add(new UTILMO.Row
                        {
                            Fields = fields
                        });
                    }

                    file.Templates[0].Rows = rows;
                    List<UTILMO.Row> rowsClause = new List<UTILMO.Row>();

                    foreach (QUOEN.Clause clause in view.Clauses.Cast<QUOEN.Clause>())
                    {
                        QUOEN.ClauseLevel clauseLevel = view.ClauseLevels.Cast<QUOEN.ClauseLevel>().First(x => x.ClauseId == clause.ClauseId);
                        COMMEN.LineBusiness lineBusiness = view.LineBusiness.Cast<COMMEN.LineBusiness>().First(x => x.LineBusinessCode == clauseLevel.ConditionLevelId);
                        var fields = file.Templates[1].Rows[0].Fields.Select(x => new UTILMO.Field
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

                        fields[0].Value = lineBusiness.LineBusinessCode.ToString();
                        fields[1].Value = lineBusiness.Description;
                        fields[2].Value = clause.ClauseId.ToString();
                        fields[3].Value = clause.ClauseName;
                        fields[4].Value = clause.ClauseTitle;

                        rowsClause.Add(new UTILMO.Row
                        {
                            Fields = fields
                        });
                    }

                    file.Templates[1].Rows = rowsClause;
                    List<UTILMO.Row> rowsInsuredObject = new List<UTILMO.Row>();

                    foreach (QUOEN.InsuredObject insuredObject in view.InsuredObjects.Cast<QUOEN.InsuredObject>())
                    {
                        QUOEN.InsObjLineBusiness insObjLineBusiness = view.InsObjLineBusinesses.Cast<QUOEN.InsObjLineBusiness>().First(x => x.InsuredObjectId == insuredObject.InsuredObjectId);
                        COMMEN.LineBusiness lineBusiness = view.LineBusiness.Cast<COMMEN.LineBusiness>().First(x => x.LineBusinessCode == insObjLineBusiness.LineBusinessCode);
                        foreach (QUOEN.Peril p in view.Perils.Cast<QUOEN.Peril>())
                        {
                            var fields = file.Templates[2].Rows[0].Fields.Select(x => new UTILMO.Field
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

                            fields[0].Value = lineBusiness.LineBusinessCode.ToString();
                            fields[1].Value = lineBusiness.Description;
                            fields[2].Value = insuredObject.InsuredObjectId.ToString();
                            fields[3].Value = insuredObject.Description;
                            fields[4].Value = p.PerilCode.ToString();
                            fields[5].Value = p.Description;

                            rowsInsuredObject.Add(new UTILMO.Row
                            {
                                Fields = fields
                            });
                        }
                    }

                    file.Templates[2].Rows = rowsInsuredObject;

                    file.Name = string.Format(fileName + "_" + DateTime.Now.ToString("dd_MM_yyyy"));
                    return new UTMO.ResultValue<string, UTMO.ErrorModel>(fileDAO.GenerateFile(file));
                }
                else
                {
                    return new UTMO.ResultValue<string, UTMO.ErrorModel>(string.Empty);
                }
            }
            catch (System.Exception ex)
            {
                return new UTMO.ResultError<string, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(new List<string>() { Errors.ErrorCreateLineBusinessParametrization }, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Elimina los tipos de riesgo cubiertos del ramo técnico
        /// </summary>
        /// <param name="lineBusinessId">Id del ramo técnico</param>
        public void DeleteCoveredRiskTypeByLineBusinessId(int lineBusinessId)
        {
            HardRiskTypeCoveredRiskType view = new HardRiskTypeCoveredRiskType();
            ViewBuilder builder = new ViewBuilder("HardRiskTypeCoveredRiskType");

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(COMMEN.LineBusinessCoveredRiskType.Properties.LineBusinessCode, typeof(COMMEN.LineBusinessCoveredRiskType).Name);
            filter.Equal();
            filter.Constant(lineBusinessId);
            builder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            List<COMMEN.LineBusinessCoveredRiskType> coveredRiskTypes = view.LineBusinessByCoveredRiskType.Cast<COMMEN.LineBusinessCoveredRiskType>().ToList();
            List<PARAMEN.HardRiskType> hardRiskTypes = view.HardRiskTypes.Cast<PARAMEN.HardRiskType>().ToList();

            foreach (COMMEN.LineBusinessCoveredRiskType item in coveredRiskTypes)
            {
                ////No se eliminan los registros de COMM.LB_COVERED_RISK_TYPE que tengan información en la tabla PARAM.HARD_RISK_TYPE
                PARAMEN.HardRiskType hardRiskType = hardRiskTypes.Where(x => x.CoveredRiskTypeCode == item.CoveredRiskTypeCode).FirstOrDefault();
                if (hardRiskType == null)
                {
                    DataFacadeManager.Instance.GetDataFacade().DeleteObject(item);
                }
            }
        }

        public UTMO.Result<List<ParamCoveredRiskType>, UTMO.ErrorModel> GetCoveredRisktypesByLineBussinessId(int idLineBusiness)
        {
            try
            {
                List<string> errorModelListDescription = new List<string>();
                List<ParamCoveredRiskType> coveredRiskTypes = new List<ParamCoveredRiskType>();

                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(COMMEN.LineBusinessCoveredRiskType.Properties.LineBusinessCode);
                filter.Equal();
                filter.Constant(idLineBusiness);

                SelectQuery selectQuery = new SelectQuery();
                selectQuery.AddSelectValue(new SelectValue(new Column(PARAMEN.CoveredRiskType.Properties.CoveredRiskTypeCode, "i")));
                selectQuery.AddSelectValue(new SelectValue(new Column(PARAMEN.CoveredRiskType.Properties.SmallDescription, "i")));
                #region Join
                Join join = new Join(new ClassNameTable(typeof(COMMEN.LineBusinessCoveredRiskType), "il"), new ClassNameTable(typeof(PARAMEN.CoveredRiskType), "i"), JoinType.Inner);
                join.Criteria = (new ObjectCriteriaBuilder()
                    .Property(COMMEN.LineBusinessCoveredRiskType.Properties.CoveredRiskTypeCode, "il")
                    .Equal()
                    .Property(PARAMEN.CoveredRiskType.Properties.CoveredRiskTypeCode, "i")
                    .GetPredicate());
                #endregion
                selectQuery.Table = join;
                selectQuery.Where = filter.GetPredicate();
                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
                {
                    while (reader.Read())
                    {
                        UTMO.Result<ParamCoveredRiskType, UTMO.ErrorModel> coveredRiskTypeItem = ParamCoveredRiskType.CreateParamCoveredRiskType(Convert.ToInt32(reader["CoveredRiskTypeCode"]), (string)reader["SmallDescription"]);
                        if (coveredRiskTypeItem is UTMO.ResultError<ParamCoveredRiskType, UTMO.ErrorModel>)
                        {
                            errorModelListDescription.Add(UnderwritingParamService.EEProvider.Resources.Errors.LineBusinessMappingEntityError);
                            return new UTMO.ResultError<List<ParamCoveredRiskType>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(errorModelListDescription, ENUMUT.ErrorType.BusinessFault, null));
                        }
                        else
                        {
                            ParamCoveredRiskType resultValue = (coveredRiskTypeItem as UTMO.ResultValue<ParamCoveredRiskType, UTMO.ErrorModel>).Value;
                            coveredRiskTypes.Add(resultValue);
                        }
                    }
                }
                return new UTMO.ResultValue<List<ParamCoveredRiskType>, UTMO.ErrorModel>(coveredRiskTypes);
            }
            catch (System.Exception ex)
            {
                return new UTMO.ResultError<List<ParamCoveredRiskType>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(new List<string>() { Errors.ErrorCreatingRiskForLineBusiness }, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        public UTMO.Result<List<ParamInsuredObjectModel>, UTMO.ErrorModel> GetInsuredObjectsByLineBussinessId(int idLineBusiness)
        {
            try
            {
                List<string> errorModelListDescription = new List<string>();
                List<ParamInsuredObjectModel> insuredObjects = new List<ParamInsuredObjectModel>();

                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(QUOEN.InsObjLineBusiness.Properties.LineBusinessCode);
                filter.Equal();
                filter.Constant(idLineBusiness);

                SelectQuery selectQuery = new SelectQuery();
                selectQuery.AddSelectValue(new SelectValue(new Column(QUOEN.InsuredObject.Properties.InsuredObjectId, "i")));
                selectQuery.AddSelectValue(new SelectValue(new Column(QUOEN.InsuredObject.Properties.Description, "i")));
                #region Join
                Join join = new Join(new ClassNameTable(typeof(QUOEN.InsObjLineBusiness), "il"), new ClassNameTable(typeof(QUOEN.InsuredObject), "i"), JoinType.Inner);
                join.Criteria = (new ObjectCriteriaBuilder()
                    .Property(QUOEN.InsObjLineBusiness.Properties.InsuredObjectId, "il")
                    .Equal()
                    .Property(QUOEN.InsuredObject.Properties.InsuredObjectId, "i")
                    .GetPredicate());
                #endregion
                selectQuery.Table = join;
                selectQuery.Where = filter.GetPredicate();
                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
                {
                    while (reader.Read())
                    {
                        UTMO.Result<ParamInsuredObjectModel, UTMO.ErrorModel> insuredObjectItem = ParamInsuredObjectModel.CreateParamInsuredObjectModel(Convert.ToInt32(reader["InsuredObjectId"]), (string)reader["Description"]);
                        if (insuredObjectItem is UTMO.ResultError<ParamInsuredObjectModel, UTMO.ErrorModel>)
                        {
                            errorModelListDescription.Add(UnderwritingParamService.EEProvider.Resources.Errors.LineBusinessMappingEntityError);
                            return new UTMO.ResultError<List<ParamInsuredObjectModel>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(errorModelListDescription, ENUMUT.ErrorType.BusinessFault, null));
                        }
                        else
                        {
                            ParamInsuredObjectModel resultValue = (insuredObjectItem as UTMO.ResultValue<ParamInsuredObjectModel, UTMO.ErrorModel>).Value;
                            insuredObjects.Add(resultValue);
                        }
                    }
                }
                return new UTMO.ResultValue<List<ParamInsuredObjectModel>, UTMO.ErrorModel>(insuredObjects);
            }
            catch (System.Exception ex)
            {
                return new UTMO.ResultError<List<ParamInsuredObjectModel>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(new List<string>() { Errors.ErrorCreatingRiskForLineBusiness }, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        public UTMO.Result<List<ParamPerilModel>, UTMO.ErrorModel> GetProtectionsByLineBussinessId(int idLineBusiness)
        {
            try
            {
                List<string> errorModelListDescription = new List<string>();
                List<ParamPerilModel> perils = new List<ParamPerilModel>();

                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(QUOEN.PerilLineBusiness.Properties.LineBusinessCode);
                filter.Equal();
                filter.Constant(idLineBusiness);

                SelectQuery selectQuery = new SelectQuery();
                selectQuery.AddSelectValue(new SelectValue(new Column(QUOEN.Peril.Properties.PerilCode, "p")));
                selectQuery.AddSelectValue(new SelectValue(new Column(QUOEN.Peril.Properties.Description, "p")));
                #region Join
                Join join = new Join(new ClassNameTable(typeof(QUOEN.PerilLineBusiness), "pl"), new ClassNameTable(typeof(QUOEN.Peril), "p"), JoinType.Inner);
                join.Criteria = (new ObjectCriteriaBuilder()
                    .Property(QUOEN.PerilLineBusiness.Properties.PerilCode, "pl")
                    .Equal()
                    .Property(QUOEN.Peril.Properties.PerilCode, "p")
                    .GetPredicate());
                #endregion
                selectQuery.Table = join;
                selectQuery.Where = filter.GetPredicate();
                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
                {
                    while (reader.Read())
                    {
                        UTMO.Result<ParamPerilModel, UTMO.ErrorModel> perilItem = ParamPerilModel.CreateParamPerilModel(Convert.ToInt32(reader["PerilCode"]), (string)reader["Description"]);
                        if (perilItem is UTMO.ResultError<ParamPerilModel, UTMO.ErrorModel>)
                        {
                            errorModelListDescription.Add(UnderwritingParamService.EEProvider.Resources.Errors.LineBusinessMappingEntityError);
                            return new UTMO.ResultError<List<ParamPerilModel>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(errorModelListDescription, ENUMUT.ErrorType.BusinessFault, null));
                        }
                        else
                        {
                            ParamPerilModel resultValue = (perilItem as UTMO.ResultValue<ParamPerilModel, UTMO.ErrorModel>).Value;
                            perils.Add(resultValue);
                        }
                    }
                }
                return new UTMO.ResultValue<List<ParamPerilModel>, UTMO.ErrorModel>(perils);
            }
            catch (System.Exception ex)
            {
                return new UTMO.ResultError<List<ParamPerilModel>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(new List<string>() { Errors.ErrorCreatingRiskForLineBusiness }, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        public UTMO.Result<List<ParamClauseModel>, UTMO.ErrorModel> GetClausesByLineBussinessId(int idLineBusiness)
        {
            try
            {
                List<string> errorModelListDescription = new List<string>();
                List<ParamClauseModel> clauses = new List<ParamClauseModel>();

                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(QUOEN.ClauseLevel.Properties.ConditionLevelId);
                filter.Equal();
                filter.Constant(idLineBusiness);

                SelectQuery selectQuery = new SelectQuery();
                selectQuery.AddSelectValue(new SelectValue(new Column(QUOEN.Clause.Properties.ClauseId, "c")));
                selectQuery.AddSelectValue(new SelectValue(new Column(QUOEN.Clause.Properties.ClauseName, "c")));
                selectQuery.AddSelectValue(new SelectValue(new Column(QUOEN.ClauseLevel.Properties.IsMandatory, "cl")));
                #region Join
                Join join = new Join(new ClassNameTable(typeof(QUOEN.ClauseLevel), "cl"), new ClassNameTable(typeof(QUOEN.Clause), "c"), JoinType.Inner);
                join.Criteria = (new ObjectCriteriaBuilder()
                    .Property(QUOEN.ClauseLevel.Properties.ClauseId, "cl")
                    .Equal()
                    .Property(QUOEN.Clause.Properties.ClauseId, "c")
                    .GetPredicate());
                #endregion
                selectQuery.Table = join;
                selectQuery.Where = filter.GetPredicate();
                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
                {
                    while (reader.Read())
                    {
                        UTMO.Result<ParamClauseModel, UTMO.ErrorModel> clauseItem = ParamClauseModel.CreateParamClauseModel(Convert.ToInt32(reader["ClauseId"]), (string)reader["ClauseName"], (bool)reader["IsMandatory"]);
                        if (clauseItem is UTMO.ResultError<ParamClauseModel, UTMO.ErrorModel>)
                        {
                            errorModelListDescription.Add(UnderwritingParamService.EEProvider.Resources.Errors.LineBusinessMappingEntityError);
                            return new UTMO.ResultError<List<ParamClauseModel>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(errorModelListDescription, ENUMUT.ErrorType.BusinessFault, null));
                        }
                        else
                        {
                            ParamClauseModel resultValue = (clauseItem as UTMO.ResultValue<ParamClauseModel, UTMO.ErrorModel>).Value;
                            clauses.Add(resultValue);
                        }
                    }
                }
                return new UTMO.ResultValue<List<ParamClauseModel>, UTMO.ErrorModel>(clauses);
            }
            catch (System.Exception ex)
            {
                return new UTMO.ResultError<List<ParamClauseModel>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(new List<string>() { Errors.ErrorCreatingRiskForLineBusiness }, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Crea los tipos de riesgo cubiertos del ramo técnico
        /// </summary>
        /// <param name="idLineBusiness">Id de ramo técnico</param>
        /// <param name="coveredRiskTypes">Ids de tipos de riesgo cubiertos</param>
        public UTMO.Result<List<ParamCoveredRiskType>, UTMO.ErrorModel> CreateCoveredRisktypesForLineBussiness(int idLineBusiness, List<ParamCoveredRiskType> coveredRiskTypes)
        {
            try
            {
                if (coveredRiskTypes != null)
                {
                    foreach (ParamCoveredRiskType item in coveredRiskTypes)
                    {
                        COMMEN.LineBusinessCoveredRiskType lineByRiskEntity = new COMMEN.LineBusinessCoveredRiskType(idLineBusiness, item.Id);
                        DataFacadeManager.Instance.GetDataFacade().InsertObject(lineByRiskEntity);
                    }
                }
                return new UTMO.ResultValue<List<ParamCoveredRiskType>, UTMO.ErrorModel>(coveredRiskTypes);
            }
            catch (System.Exception ex)
            {
                return new UTMO.ResultError<List<ParamCoveredRiskType>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(new List<string>() { Errors.ErrorCreatingRiskForLineBusiness }, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Crea los objetos del seguro del ramo técnico
        /// </summary>
        /// <param name="idLineBusiness">Id de ramo técnico</param>
        /// <param name="insuredObjects">Objetos del seguro</param>
        public UTMO.Result<List<ParamInsuredObjectModel>, UTMO.ErrorModel> CreateInsuredObjectsForLineBusiness(int idLineBusiness, List<ParamInsuredObjectModel> insuredObjects)
        {
            try
            {
                if (insuredObjects != null)
                {
                    foreach (ParamInsuredObjectModel item in insuredObjects)
                    {
                        QUOEN.InsObjLineBusiness perilLineBusiness = new QUOEN.InsObjLineBusiness(item.Id, idLineBusiness);
                        DataFacadeManager.Instance.GetDataFacade().InsertObject(perilLineBusiness);
                    }
                }
                return new UTMO.ResultValue<List<ParamInsuredObjectModel>, UTMO.ErrorModel>(insuredObjects);
            }
            catch (System.Exception ex)
            {
                return new UTMO.ResultError<List<ParamInsuredObjectModel>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(new List<string>() { Errors.ErrorCreatingInsuranceObjectsLineBusiness }, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Crea los amparos del ramo técnico
        /// </summary>
        /// <param name="idLineBusiness">Id de ramo técnico</param>
        /// <param name="perils">Ampraros del ramo técnico</param>
        public UTMO.Result<List<ParamPerilModel>, UTMO.ErrorModel> CreateProtectionsForLineBusiness(int idLineBusiness, List<ParamPerilModel> perils)
        {
            try
            {
                if (perils != null)
                {
                    foreach (ParamPerilModel item in perils)
                    {
                        QUOEN.PerilLineBusiness perilLineBusiness = new QUOEN.PerilLineBusiness(item.Id, idLineBusiness);
                        DataFacadeManager.Instance.GetDataFacade().InsertObject(perilLineBusiness);
                    }
                }
                return new UTMO.ResultValue<List<ParamPerilModel>, UTMO.ErrorModel>(perils);
            }
            catch (System.Exception ex)
            {
                return new UTMO.ResultError<List<ParamPerilModel>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(new List<string>() { Errors.ErrorCreatingPerilForLineBusiness }, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Crea las cláusulas del ramo técnico
        /// </summary>
        /// <param name="idLineBusiness">Id de ramo técnico</param>
        /// <param name="clauses">Cláusulas del ramo técnico</param>
        public UTMO.Result<List<ParamClauseModel>, UTMO.ErrorModel> CreateClausesForLineBusiness(int idLineBusiness, List<ParamClauseModel> clauses)
        {
            try
            {
                if (clauses != null)
                {
                    int maxClauseLevelId = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade()
                                        .SelectObjects(typeof(QUOEN.ClauseLevel)))
                                        .Cast<QUOEN.ClauseLevel>().Max(x => x.ClauseLevelId);

                    foreach (ParamClauseModel item in clauses)
                    {
                        maxClauseLevelId++;
                        QUOEN.ClauseLevel clauseLevel = new QUOEN.ClauseLevel(maxClauseLevelId);
                        clauseLevel.ClauseId = item.Id;
                        clauseLevel.ConditionLevelId = idLineBusiness;
                        clauseLevel.IsMandatory = item.IsMandatory;

                        DataFacadeManager.Instance.GetDataFacade().InsertObject(clauseLevel);
                    }
                }
                return new UTMO.ResultValue<List<ParamClauseModel>, UTMO.ErrorModel>(clauses);
            }
            catch (System.Exception ex)
            {
                return new UTMO.ResultError<List<ParamClauseModel>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(new List<string>() { Errors.ErrorCreatingClauseLineBusiness }, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        public UTMO.Result<List<ParamCoveredRiskType>, UTMO.ErrorModel> DeleteCoveredRisktypesForLineBussiness(int idLineBusiness, List<ParamCoveredRiskType> coveredRiskTypes)
        {
            try
            {
                if (coveredRiskTypes != null)
                {
                    foreach (ParamCoveredRiskType item in coveredRiskTypes)
                    {
                        COMMEN.LineBusinessCoveredRiskType lineByRiskEntity = new COMMEN.LineBusinessCoveredRiskType(idLineBusiness, item.Id);
                        PrimaryKey key = COMMEN.LineBusinessCoveredRiskType.CreatePrimaryKey(lineByRiskEntity.LineBusinessCode, lineByRiskEntity.CoveredRiskTypeCode);
                        lineByRiskEntity = (COMMEN.LineBusinessCoveredRiskType)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                        DataFacadeManager.Instance.GetDataFacade().DeleteObject(lineByRiskEntity);
                    }
                }
                return new UTMO.ResultValue<List<ParamCoveredRiskType>, UTMO.ErrorModel>(coveredRiskTypes);
            }
            catch (System.Exception ex)
            {
                return new UTMO.ResultError<List<ParamCoveredRiskType>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(new List<string>() { Errors.ErrorDeleteCoveredRiskTypes }, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        public UTMO.Result<List<ParamInsuredObjectModel>, UTMO.ErrorModel> DeleteInsuredObjectsForLineBussiness(int idLineBusiness, List<ParamInsuredObjectModel> insuredObjects)
        {
            try
            {
                if (insuredObjects != null)
                {
                    foreach (ParamInsuredObjectModel item in insuredObjects)
                    {
                        QUOEN.InsObjLineBusiness lineByRiskEntity = new QUOEN.InsObjLineBusiness(item.Id, idLineBusiness);
                        PrimaryKey key = QUOEN.InsObjLineBusiness.CreatePrimaryKey(lineByRiskEntity.InsuredObjectId, lineByRiskEntity.LineBusinessCode);
                        lineByRiskEntity = (QUOEN.InsObjLineBusiness)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                        DataFacadeManager.Instance.GetDataFacade().DeleteObject(lineByRiskEntity);
                    }
                }
                return new UTMO.ResultValue<List<ParamInsuredObjectModel>, UTMO.ErrorModel>(insuredObjects);
            }
            catch (System.Exception ex)
            {
                return new UTMO.ResultError<List<ParamInsuredObjectModel>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(new List<string>() { Errors.ErrorDeleteInsuranceObjects }, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        public UTMO.Result<List<ParamPerilModel>, UTMO.ErrorModel> DeleteProtectionsForLineBussiness(int idLineBusiness, List<ParamPerilModel> perils)
        {
            try
            {
                if (perils != null)
                {
                    foreach (ParamPerilModel item in perils)
                    {
                        QUOEN.PerilLineBusiness lineByRiskEntity = new QUOEN.PerilLineBusiness(item.Id, idLineBusiness);
                        PrimaryKey key = QUOEN.PerilLineBusiness.CreatePrimaryKey(lineByRiskEntity.PerilCode, lineByRiskEntity.LineBusinessCode);
                        lineByRiskEntity = (QUOEN.PerilLineBusiness)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                        DataFacadeManager.Instance.GetDataFacade().DeleteObject(lineByRiskEntity);
                    }
                }
                return new UTMO.ResultValue<List<ParamPerilModel>, UTMO.ErrorModel>(perils);
            }
            catch (System.Exception ex)
            {
                return new UTMO.ResultError<List<ParamPerilModel>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(new List<string>() { Errors.ErrorDeletePerils }, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        public UTMO.Result<List<ParamClauseModel>, UTMO.ErrorModel> DeleteClausesForLineBussiness(int idLineBusiness, List<ParamClauseModel> clauses)
        {
            try
            {
                if (clauses != null)
                {
                    foreach (ParamClauseModel item in clauses)
                    {
                        ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                        filter.Property(QUOEN.ClauseLevel.Properties.ClauseId);
                        filter.Equal();
                        filter.Constant(item.Id);
                        filter.And();
                        filter.Property(QUOEN.ClauseLevel.Properties.ConditionLevelId);
                        filter.Equal();
                        filter.Constant(idLineBusiness);

                        BusinessCollection businessCollection = DataFacadeManager.Instance.GetDataFacade().List(typeof(QUOEN.ClauseLevel), filter.GetPredicate());

                        foreach (QUOEN.ClauseLevel itemClause in businessCollection)
                        {
                            DataFacadeManager.Instance.GetDataFacade().DeleteObject(itemClause);
                        }
                    }
                }
                return new UTMO.ResultValue<List<ParamClauseModel>, UTMO.ErrorModel>(clauses);
            }
            catch (System.Exception ex)
            {
                return new UTMO.ResultError<List<ParamClauseModel>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(new List<string>() { Errors.ErrorDeleteClauses }, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Consulta los ramos técnicos dependiendo del filtro
        /// </summary>
        /// <param name="predicate">Filtro para la consulta</param>
        /// <returns>Lista de ramos técnicos</returns>
        private UTMO.Result<List<ParamLineBusiness>, UTMO.ErrorModel> GetParamLineBusinessByPredicate(Predicate predicate)
        {
            try
            {
                ParametrizationLineBusinessView view = new ParametrizationLineBusinessView();
                ViewBuilder builder = new ViewBuilder("ParametrizationLineBusinessView");
                builder.Filter = predicate;
                DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

                List<ParamLineBusiness> parametrizations = ModelAssembler.CreateParamLineBusinesss(view.LineBusiness);

                foreach (ParamLineBusiness parametrization in parametrizations)
                {
                    ////Tipo de riesgos
                    parametrization.CoveredRiskTypes = view.LineBusinessCoveredRiskTypes.Cast<COMMEN.LineBusinessCoveredRiskType>().Where(x => x.LineBusinessCode == parametrization.Id).Select(x => x.CoveredRiskTypeCode).ToList();

                    ////Objetos del seguro
                    List<QUOEN.InsObjLineBusiness> insuredObjets = view.InsObjLineBusinesses.Cast<QUOEN.InsObjLineBusiness>().Where(x => x.LineBusinessCode == parametrization.Id).ToList();
                    parametrization.InsuredObjects = new List<ParamInsuredObjectDesc>();
                    foreach (QUOEN.InsObjLineBusiness ins in insuredObjets)
                    {
                        parametrization.InsuredObjects.Add(ModelAssembler.CreateParamInsuredObjectDesc(view.InsuredObjects.Cast<QUOEN.InsuredObject>().First(x => x.InsuredObjectId == ins.InsuredObjectId)));
                    }

                    ////Amparos
                    List<QUOEN.PerilLineBusiness> perilsLB = view.PerilLineBusinesses.Cast<QUOEN.PerilLineBusiness>().Where(x => x.LineBusinessCode == parametrization.Id).ToList();
                    parametrization.Perils = new List<Peril>();
                    foreach (QUOEN.PerilLineBusiness perilLB in perilsLB)
                    {
                        parametrization.Perils.Add(ModelAssembler.CreatePeril(view.Perils.Cast<QUOEN.Peril>().First(x => x.PerilCode == perilLB.PerilCode)));
                    }

                    ////Cláusulas
                    List<QUOEN.ClauseLevel> clauseLevels = view.ClauseLevels.Cast<QUOEN.ClauseLevel>().Where(x => x.ConditionLevelId == parametrization.Id).ToList();
                    parametrization.Clauses = new List<Clause>();
                    foreach (QUOEN.ClauseLevel clauseLevel in clauseLevels)
                    {
                        parametrization.Clauses.Add(ModelAssembler.CreateClause(view.Clauses.Cast<QUOEN.Clause>().First(x => x.ClauseId == clauseLevel.ClauseId)));
                    }
                }

                return new UTMO.ResultValue<List<ParamLineBusiness>, UTMO.ErrorModel>(parametrizations);
            }
            catch (System.Exception ex)
            {
                return new UTMO.ResultError<List<ParamLineBusiness>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(new List<string>() { Errors.ErrorCreateLineBusinessParametrization }, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }
        #endregion
    }
}
