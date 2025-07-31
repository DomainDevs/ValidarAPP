// -----------------------------------------------------------------------
// <copyright file="CoveredRiskTypeDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Carlos Eduardo Rodríguez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamServices.EEProvider.DAOs
{
    using System;
    using System.Collections.Generic;    
    using System.Diagnostics;
    using System.Linq;
    using System.Resources;
    using Sistran.Core.Application.ModelServices.Models.Underwriting;
    using Sistran.Core.Application.Parameters.Entities;
    using Sistran.Core.Application.UnderwritingParamService.Models;
    using Sistran.Core.Application.UnderwritingParamServices.EEProvider.Assemblers;
    using Sistran.Core.Application.Utilities.DataFacade;    
    using Sistran.Core.Application.Utilities.Enums;
    using Sistran.Core.Application.Utilities.Error;
    using Sistran.Core.Framework.DAF;
    using Sistran.Core.Framework.DAF.Engine;
    using Sistran.Core.Framework.Queries;
    using enumsUtilities = Sistran.Core.Services.UtilitiesServices.Enums;
    using modelsUtilities = Sistran.Core.Services.UtilitiesServices.Models;

    /// <summary>
    /// Clase DAO del objeto CoveredRiskTypeDAO.
    /// </summary>
    public class CoveredRiskTypeDAO
    {
        /// <summary>
        /// Obtiene la lista de tipos de riesgo cubierto.
        /// </summary>
        /// <returns>Lista de sucursales consultadas</returns>
        public Result<List<ParamCoveredRiskType>, ErrorModel> GetCoveredRiskTypes()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            try
            {                
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CoveredRiskType)));
                Result<List<ParamCoveredRiskType>, ErrorModel> coveredRiskTypes = ModelAssembler.CreateCoveredRiskTypes(businessCollection);
                if (coveredRiskTypes is ResultError<List<ParamCoveredRiskType>, ErrorModel>)
                {                    
                    return coveredRiskTypes;
                }
                else
                {
                    List<ParamCoveredRiskType> resultValue = (coveredRiskTypes as ResultValue<List<ParamCoveredRiskType>, ErrorModel>).Value;
                    if (resultValue.Count == 0)
                    {
                        errorModelListDescription.Add(UnderwritingParamService.EEProvider.Resources.Errors.CoveredRiskTypeNotFound);
                        return new ResultError<List<ParamCoveredRiskType>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.NotFound, null));
                    }
                    else
                    {
                        return coveredRiskTypes;
                    }
                }                
            }
            catch (Exception ex)
            {
                errorModelListDescription.Add(UnderwritingParamService.EEProvider.Resources.Errors.CoveredRiskTypeThecnicalError);
                return new ResultError<List<ParamCoveredRiskType>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.TechnicalFault, ex));
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingParamServices.EEProvider.DAOs");
            }
        }

        /// <summary>
        /// Obtener tipos de riesgo cubiertos
        /// </summary>
        /// <returns>Grupos de Coberturas</returns>
        public Result<List<ParamCoveredRiskTypeDesc>, ErrorModel> GetCoveredRiskTypesDesc()
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CoveredRiskType)));

            List<ParamCoveredRiskTypeDesc> paramGroupCoverageDesc = ModelAssembler.CreateParamCoveredRiskTypeDescs(businessCollection);
            return new ResultValue<List<ParamCoveredRiskTypeDesc>, ErrorModel>(paramGroupCoverageDesc);
        }

        /// <summary>
        /// Genera archivo excel de tipos de riesgo cubierto.
        /// </summary>
        /// <param name="coveredRiskTypesList">Lista de tipos de riesgo cubierto</param>
        /// <param name="fileName">Url del archivo</param>
        /// <returns>Url del archivo generado.</returns>
        public string GenerateFileToCoveredRiskTypes(List<CoveredRiskTypeServiceModel> coveredRiskTypesList, string fileName)
        {
            FileDAO commonFileDAO = new FileDAO();
            modelsUtilities.FileProcessValue fileProcessValue = new modelsUtilities.FileProcessValue();
            fileProcessValue.Key1 = (int)enumsUtilities.FileProcessType.ParametrizationCeoveredRiskType;

            modelsUtilities.File file = commonFileDAO.GetFileByFileProcessValue(fileProcessValue);

            if (file != null && file.IsEnabled)
            {
                file.Name = fileName;
                List<modelsUtilities.Row> rows = new List<modelsUtilities.Row>();

                foreach (CoveredRiskTypeServiceModel coveredRiskType in coveredRiskTypesList)
                {
                    var fields = file.Templates[0].Rows[0].Fields.Select(x => new modelsUtilities.Field
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

                    fields[0].Value = coveredRiskType.Id.ToString();
                    fields[1].Value = coveredRiskType.SmallDescription;

                    rows.Add(new modelsUtilities.Row
                    {
                        Fields = fields
                    });
                }

                file.Templates[0].Rows = rows;
                file.Name = fileName + "_" + DateTime.Now.ToString("dd_MM_yyyy");
                return commonFileDAO.GenerateFile(file);
            }
            else
            {
                return string.Empty;
            }
        }

    }
}
