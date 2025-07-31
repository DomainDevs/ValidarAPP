// -----------------------------------------------------------------------
// <copyright file="CoverageGroupDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Gina Gómez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.EEProvider.DAOs
{
    using Sistran.Core.Application.UnderwritingParamService.Models;
    using Sistran.Core.Application.UnderwritingParamServices.EEProvider.DAOs;
    using Sistran.Core.Services.UtilitiesServices.Enums;
    using Sistran.Core.Services.UtilitiesServices.Models;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using ENUMUT = Sistran.Core.Application.Utilities.Enums;
    using UTIMO = Sistran.Core.Application.Utilities.Error;

    /// <summary>
    /// Clase DAO del objeto CoverageGroupDAO.
    /// </summary>
    public class CoverageGroupDAO
    {
        /// <summary>
        /// Genera archivo excel
        /// </summary>
        /// <param name="parametrizationCoverages">Listado de grupos de coberturas</param>
        /// <param name="fileName">Nombre de archivo</param>
        /// <returns>Modelo result</returns>
        public UTIMO.Result<string, UTIMO.ErrorModel> GenerateFileToCoverageGroup(List<ParametrizationCoverageGroupRiskType> parametrizationCoverages, string fileName)
        {
            try
            {
                FileProcessValue fileProcessValue = new FileProcessValue()
                {
                    Key1 = (int)FileProcessType.ParametrizationCoverageGroupRiskType
                };
                FileDAO fileDAO = new FileDAO();
                File file = fileDAO.GetFileByFileProcessValue(fileProcessValue);

                if (file != null && file.IsEnabled)
                {
                    file.Name = fileName;
                    file.Templates[0].Rows = this.AssignValues(parametrizationCoverages, file);
                    file.Name = fileName + "_" + DateTime.Now.ToString("dd_MM_yyyy");

                    var result = fileDAO.GenerateFile(file);
                    return new UTIMO.ResultValue<string, UTIMO.ErrorModel>(result);
                }
                else
                {
                    return new UTIMO.ResultValue<string, UTIMO.ErrorModel>(string.Empty);
                }
            }
            catch (System.Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add("Error Creando");
                return new UTIMO.ResultError<string, UTIMO.ErrorModel>(UTIMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Asigna los valores de las filas
        /// </summary>
        /// <param name="parametrizationCoverages">Listado de parametrizationCoverages</param>
        /// <param name="file">Configuración archivo</param>
        /// <returns>Listado filas</returns>
        private List<Row> AssignValues(List<ParametrizationCoverageGroupRiskType> parametrizationCoverages, File file)
        {
            List<Row> rows = new List<Row>();
            foreach (ParametrizationCoverageGroupRiskType item in parametrizationCoverages)
            {
                var fields = file.Templates[0].Rows[0].Fields.Select(x => new Field
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

                fields[0].Value = item.CoverageRiskType.SmallDescription.ToString();
                fields[1].Value = item.Description.ToString();
                fields[2].Value = item.SmallDescription.ToString();
                if (item.Enabled)
                {
                    fields[3].Value = Resources.Errors.Yes;
                }
                else
                {
                    fields[3].Value = Resources.Errors.No;
                }      
                
                rows.Add(new Row
                {
                    Fields = fields
                });
            }

            return rows;
        }
    }
}
