// -----------------------------------------------------------------------
// <copyright file="LimitRcDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Jeison Rodriguez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.EEProvider.DAOs
{
    using Sistran.Co.Application.Data;
    using Sistran.Core.Application.Common.Entities;
    using Sistran.Core.Application.UnderwritingParamService.Models;
    using Sistran.Core.Application.UnderwritingParamServices.EEProvider.Assemblers;
    using Sistran.Core.Application.UnderwritingParamServices.EEProvider.DAOs;
    using Sistran.Core.Application.Utilities.DataFacade;
    using Sistran.Core.Framework.DAF;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using ENUMUT = Sistran.Core.Application.Utilities.Enums;
    using UTILEN = Sistran.Core.Services.UtilitiesServices.Enums;
    using UTILMO = Sistran.Core.Services.UtilitiesServices.Models;
    using UTMO = Sistran.Core.Application.Utilities.Error;
    /// <summary>
    /// Clase pública limites rc
    /// </summary>
    public class LimitRcDAO
    {
        /// <summary>
        /// Obtiene lista de limites rc
        /// </summary>
        /// <returns>Retorna lista de limite rc</returns>
        public UTMO.Result<List<ParamLimitRc>, UTMO.ErrorModel> GetLimitsRc()
        {
            try
            {
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CoLimitsRc)));
                List<ParamLimitRc> paramLimitRc = ModelAssembler.CreateLimitsRc(businessCollection);
                return new UTMO.ResultValue<List<ParamLimitRc>, UTMO.ErrorModel>(paramLimitRc);
            }
            catch (System.Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add("Error");
                return new UTMO.ResultError<List<ParamLimitRc>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Validación de limite rc
        /// </summary>
        /// <param name="limitRcCode">codigo de limite rc</param>
        /// <returns>valor 1: tiene dependencias, 0: no tiene dependencias</returns>
        public int ValidateLimitc(int limitRcCode)
        {
            DataTable result;
            NameValue[] parameters = new NameValue[1];
            parameters[0] = new NameValue("@LIMIT_RC_CD", limitRcCode);
            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                result = dynamicDataAccess.ExecuteSPDataTable("COMM.VALIDATE_LIMITRC", parameters);
            }

            return (int)result.Rows[0][0];
        }


        public UTMO.Result<string, UTMO.ErrorModel> GenerateFileToLimitRc(List<ParamLimitRc> paramLimitRc, string fileName)
        {
            try
            {
                UTILMO.FileProcessValue fileProcessValue = new UTILMO.FileProcessValue()
                {
                    Key1 = (int)UTILEN.FileProcessType.ParametrizationLimitRc
                };
                FileDAO fileDAO = new FileDAO();
                UTILMO.File file = fileDAO.GetFileByFileProcessValue(fileProcessValue);

                if (file != null && file.IsEnabled)
                {
                    file.Name = fileName;
                    List<UTILMO.Row> rows = new List<UTILMO.Row>();

                    foreach (ParamLimitRc item in paramLimitRc)
                    {
                        UTILMO.Row row = new UTILMO.Row();
                        ///<summary>
                        ///Se realiza un ordenamiento de columnas utilizando el código OrderBy(x => x.Order) al momento de estar armando la tabla, 
                        ///para corregir los inconvenientes presentados por encabezados de tabla en desorden al generar el archivo Excel. 
                        ///</summary>
                        ///<author>Diego Leon</author>
                        ///<date>17/07/2018</date>
                        ///<purpose>REQ_#084</purpose>
                        ///<returns></returns>
                        var fields = file.Templates[0].Rows[0].Fields.OrderBy(x => x.Order).Select(x => new UTILMO.Field
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
                        fields[1].Value = item.Limit1.ToString();
                        fields[2].Value = item.Limit2.ToString();
                        fields[3].Value = item.Limit3.ToString();
                        fields[4].Value = item.LimitUnique.ToString();
                        fields[5].Value = item.Description;
                        row.Fields = fields;
                        rows.Add(row);
                    }

                    file.Templates[0].Rows = rows;
                    file.Name = fileName + "_" + DateTime.Now.ToString("dd_MM_yyyy");

                    var result = fileDAO.GenerateFile(file);
                    return new UTMO.ResultValue<string, UTMO.ErrorModel>(result);
                }
                else
                {
                    return new UTMO.ResultValue<string, UTMO.ErrorModel>(string.Empty);
                }
            }
            catch (System.Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add("Error Descargando excel");
                return new UTMO.ResultError<string, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }
    }
}
