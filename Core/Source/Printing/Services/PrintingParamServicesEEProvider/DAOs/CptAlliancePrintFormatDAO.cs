// -----------------------------------------------------------------------
// <copyright file="CptAlliancePrintFormat.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Carlos Eduardo Rodríguez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.PrintingParamServices.EEProvider.DAOs
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;    
    using Sistran.Core.Application.PrintingParamServices.EEProvider.Assemblers;
    using Sistran.Core.Application.PrintingParamServices.Models;    
    using Sistran.Core.Application.Utilities.DataFacade;
    using Sistran.Core.Application.Parameters.Entities;
    using Sistran.Core.Application.Utilities.Error;
    using Sistran.Core.Application.Utilities.Enums;
    using Sistran.Core.Application.CommonService.Models;
    using Sistran.Core.Framework.Contexts;
    using Sistran.Core.Framework.DAF;
    using Sistran.Core.Framework.DAF.Engine;
    using Sistran.Core.Framework.Transactions;
    using System.Linq;
    using Sistran.Core.Framework.Queries;
    using Sistran.Core.Services.UtilitiesServices.Enums;
 
    using Sistran.Core.Application.ModelServices.Models.Printing;
    using Sistran.Core.Services.UtilitiesServices.Models;
    using Sistran.Core.Application.UtilitiesServicesEEProvider.DAOs;

    /// <summary>
    /// Clase DAO del objeto CptAlliancePrintFormat.
    /// </summary>
    public class CptAlliancePrintFormatDAO
    {
        /// <summary>
        /// Obtiene la lista lista de Formatos de impresión de aliados
        /// </summary>
        /// <returns>Lista de sucursales consultadas</returns>
        public Result<List<ParamCptAlliancePrintFormat>, ErrorModel> GetCptAlliancePrintFormats()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            try
            {
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CptAlliancePrintFormat)));
                Result<List<ParamCptAlliancePrintFormat>, ErrorModel> cptAlliancePrintFormats = ModelAssembler.MappCptAlliancePrintFormats(businessCollection);
                if (cptAlliancePrintFormats is ResultError<List<ParamCptAlliancePrintFormat>, ErrorModel>)
                {
                    return cptAlliancePrintFormats;
                }
                else
                {
                    List<ParamCptAlliancePrintFormat> resultValue = (cptAlliancePrintFormats as ResultValue<List<ParamCptAlliancePrintFormat>, ErrorModel>).Value;
                    if (resultValue.Count == 0)
                    {
                        errorModelListDescription.Add(Resources.Errors.AllyPrintFormatNotFound);
                        return new ResultError<List<ParamCptAlliancePrintFormat>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.NotFound, null));
                    }
                    else
                    {
                        return cptAlliancePrintFormats;
                    }
                }
            }
            catch (Exception ex)
            {                
                errorModelListDescription.Add(Resources.Errors.AllyPrintFormatThecnicalError);
                return new ResultError<List<ParamCptAlliancePrintFormat>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.TechnicalFault, ex));
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.PrintingParamServices.EEProvider.DAOs");
            }
        }
        
        public ParametrizationResponse<ParamCptAlliancePrintFormat> SaveCptAlliancePrintFormat(List<ParamCptAlliancePrintFormat> cptAlliancePrintFormatsAdded, List<ParamCptAlliancePrintFormat> cptAlliancePrintFormatsEdited, List<ParamCptAlliancePrintFormat> cptAlliancePrintFormatsDeleted)
        {
            Stopwatch stopWatch = new Stopwatch();
            ParametrizationResponse<ParamCptAlliancePrintFormat> returnCptAlliancePrintFormats = new ParametrizationResponse<ParamCptAlliancePrintFormat>();
            stopWatch.Start();
            using (Context.Current)
            {
                if (cptAlliancePrintFormatsAdded.Count != 0)
                {
                    using (Transaction transaction = new Transaction())
                    {
                        try
                        {
                            foreach (ParamCptAlliancePrintFormat item in cptAlliancePrintFormatsAdded)
                            {
                                CptAlliancePrintFormat entityCptAlliancePrintFormat = EntityAssembler.CreateCptAlliancePrintFormat(item);
                                DataFacadeManager.Instance.GetDataFacade().InsertObject(entityCptAlliancePrintFormat);
                            }

                            transaction.Complete();
                            returnCptAlliancePrintFormats.TotalAdded = cptAlliancePrintFormatsAdded.Count;
                        }
                        catch (Exception)
                        {
                            transaction.Dispose();
                            returnCptAlliancePrintFormats.ErrorAdded = Resources.Errors.ErrorSaveCptAlliancePrintFormatsAdded;
                        }
                    }
                }

                if (cptAlliancePrintFormatsEdited.Count != 0)
                {
                    using (Transaction transaction = new Transaction())
                    {
                        try
                        {
                            foreach (var item in cptAlliancePrintFormatsEdited)
                            {
                                PrimaryKey key = CptAlliancePrintFormat.CreatePrimaryKey(item.Id);
                                CptAlliancePrintFormat cptAlliancePrintFormatEntity = new CptAlliancePrintFormat(item.Id);
                                cptAlliancePrintFormatEntity = (CptAlliancePrintFormat)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                                cptAlliancePrintFormatEntity.PrefixCode = item.PrefixCd;
                                cptAlliancePrintFormatEntity.EndoTypeCode = item.EndoTypeCd;
                                cptAlliancePrintFormatEntity.Format = item.Format;
                                cptAlliancePrintFormatEntity.Enable = item.Enable;

                                DataFacadeManager.Instance.GetDataFacade().UpdateObject(cptAlliancePrintFormatEntity);
                            }

                            transaction.Complete();
                            returnCptAlliancePrintFormats.TotalModify = cptAlliancePrintFormatsEdited.Count;
                        }
                        catch (Exception)
                        {
                            transaction.Dispose();
                            returnCptAlliancePrintFormats.ErrorModify = Resources.Errors.ErrorSaveCptAlliancePrintFormatsModify;
                        }
                    }
                }

                if (cptAlliancePrintFormatsDeleted.Count != 0)
                {
                    using (Transaction transaction = new Transaction())
                    {
                        try
                        {
                            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                            filter.Property(CptAlliancePrintFormat.Properties.AlliancePrintFormatId).In().ListValue();
                            cptAlliancePrintFormatsDeleted.ForEach(x => filter.Constant(x.Id));
                            filter.EndList();
                            DataFacadeManager.Instance.GetDataFacade().DeleteObjects(typeof(CptAlliancePrintFormat), filter.GetPredicate());
                            transaction.Complete();
                            returnCptAlliancePrintFormats.TotalDeleted = cptAlliancePrintFormatsDeleted.Count;
                        }
                        catch (ForeignKeyException)
                        {
                            transaction.Dispose();
                            returnCptAlliancePrintFormats.ErrorDeleted = Resources.Errors.ErrorSavecptAlliancePrintFormatsRelated;
                        }
                        catch (RelatedObjectException)
                        {
                            transaction.Dispose();
                            returnCptAlliancePrintFormats.ErrorDeleted = Resources.Errors.ErrorSavecptAlliancePrintFormatsRelated;
                        }
                        catch (Exception)
                        {
                            transaction.Dispose();
                            returnCptAlliancePrintFormats.ErrorDeleted = Resources.Errors.ErrorSavecptAlliancePrintFormatsDeleted;
                        }
                    }
                }                
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonServices.Providers.SaveInsuredSegments");
            return returnCptAlliancePrintFormats;
        }

        /// <summary>
        /// Genera archivo excel de fomratos de impresión de aliado.
        /// </summary>
        /// <param name="cptAlliancePrintFormatList">Lista de fomratos de impresión de aliado</param>
        /// <param name="fileName">Nombre del archivo.</param>
        /// <returns>Url del archivo generado.</returns>
        public string GenerateFileToCptAlliancePrintFormats(List<CptAlliancePrintFormatServiceModel> cptAlliancePrintFormatList, string fileName)
        {
            FileDAO commonFileDAO = new FileDAO();
            FileProcessValue fileProcessValue = new FileProcessValue();
            fileProcessValue.Key1 = (int)FileProcessType.ParametrizationPrintAllyFormat;

            File file = commonFileDAO.GetFileByFileProcessValue(fileProcessValue);

            if (file != null && file.IsEnabled)
            {
                file.Name = fileName;
                List<Row> rows = new List<Row>();

                foreach (CptAlliancePrintFormatServiceModel cptAlliancePrintFormatServiceModel in cptAlliancePrintFormatList)
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

                    fields[0].Value = cptAlliancePrintFormatServiceModel.PrefixServiceQueryModel.PrefixDescription.ToString();
                    fields[1].Value = cptAlliancePrintFormatServiceModel.EndorsementTypeServiceQueryModel.Description.ToString();
                    fields[2].Value = cptAlliancePrintFormatServiceModel.Format.ToString();
                    if (cptAlliancePrintFormatServiceModel.Enable == true)
                    {
                        fields[3].Value = "Si";
                    }
                    else
                    {
                        fields[3].Value = "No";
                    }

                    rows.Add(new Row
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
