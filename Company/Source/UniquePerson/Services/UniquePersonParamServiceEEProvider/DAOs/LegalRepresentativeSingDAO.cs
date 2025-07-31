using Sistran.Company.Application.UniquePerson.Entities;
using Sistran.Company.Application.UniquePersonParamService.EEProvider.Assemblers;
using Sistran.Company.Application.UniquePersonParamService.EEProvider.Resources;
using Sistran.Company.Application.UniquePersonParamService.Models;
using Sistran.Core.Application.ModelServices.Enums;
using Sistran.Core.Application.ModelServices.Models.Param;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Enums;
using Sistran.Core.Application.Utilities.Error;
using Sistran.Core.Application.UtilitiesServicesEEProvider.DAOs;
using Sistran.Core.Framework.Contexts;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Transactions;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Sistran.Company.Application.UniquePersonParamService.EEProvider.DAOs
{
    public class LegalRepresentativeSingDAO
    {
        /// <summary>
        /// Obtiene la lista de Firma Representante Legal.
        /// </summary>
        /// <returns>Lista de Firma Representante Legal consultadas</returns>
        public Result<List<ParamLegalRepresentativeSing>, ErrorModel> GetLstCptLegalReprSign()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            try
            {
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CptLegalReprSign)));
                Result<List<ParamLegalRepresentativeSing>, ErrorModel> lstCptLegalReprSign = ModelAssembler.CreateLstCptLegalReprSign(businessCollection);
                if (lstCptLegalReprSign is ResultError<List<ParamLegalRepresentativeSing>, ErrorModel>)
                {
                    return lstCptLegalReprSign;
                }
                else
                {
                    List<ParamLegalRepresentativeSing> resultValue = (lstCptLegalReprSign as ResultValue<List<ParamLegalRepresentativeSing>, ErrorModel>).Value;

                    if (resultValue.Count == 0)
                    {
                        errorModelListDescription.Add(Errors.NoLegalRepresentativeSingWasFound);
                        return new ResultError<List<ParamLegalRepresentativeSing>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.NotFound, null));
                    }
                    else
                    {
                        return lstCptLegalReprSign;
                    }
                }
            }
            catch (Exception ex)
            {
                errorModelListDescription.Add(Errors.ErrorQueryTheLegalRepresentativeSing);
                return new ResultError<List<ParamLegalRepresentativeSing>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.TechnicalFault, ex));
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonParamService.DAOs");
            }
        }

        /// <summary>
        /// Método que obtine una Firma Representante Legal por Id
        /// </summary>
        /// <param name="ciaCode">Id Compañia</param>
        /// <param name="branchTypeCode">Id Sucursal</param>
        /// <param name="currentFrom">Fecha actual</param>
        /// <returns>una Firma Representante Legal consultada</returns>
        public Result<ParamLegalRepresentativeSing, ErrorModel> GetCptLegalReprSignByCiaCodeBranchTypeCodeCurrentFrom(decimal ciaCode, decimal branchTypeCode, DateTime currentFrom)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            try
            {
                PrimaryKey key = CptLegalReprSign.CreatePrimaryKey(ciaCode, branchTypeCode, currentFrom);
                CptLegalReprSign cptLegalReprSign = (CptLegalReprSign)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

                Result<ParamLegalRepresentativeSing, ErrorModel> paramCptLegalReprSign = ModelAssembler.CreateCptLegalReprSign(cptLegalReprSign);
                if (paramCptLegalReprSign is ResultError<ParamLegalRepresentativeSing, ErrorModel>)
                {
                    return paramCptLegalReprSign;
                }
                else
                {
                    ParamLegalRepresentativeSing resultValue = (paramCptLegalReprSign as ResultValue<ParamLegalRepresentativeSing, ErrorModel>).Value;

                    if (resultValue == null)
                    {
                        errorModelListDescription.Add("No se encontro una Firma Representante Legal.");
                        return new ResultError<ParamLegalRepresentativeSing, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.NotFound, null));
                    }
                    else
                    {
                        return paramCptLegalReprSign;
                    }
                }
            }
            catch (Exception ex)
            {
                errorModelListDescription.Add("Ocurrio un error inesperado en la consulta de Firma Representante Legal. Comuniquese con el administrador");
                return new ResultError<ParamLegalRepresentativeSing, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.TechnicalFault, ex));
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonParamService.DAOs");
            }
        }

        /// <summary>
        /// Guarda los registros nuevos y editados
        /// </summary>
        /// <param name="legalRepresentativeSingsAdded">Lista de model business legalRepresentativeSingsAdded</param>
        /// <param name="legalRepresentativeSingsEdited">Lista de model business legalRepresentativeSingsEdited</param>
        /// <returns>Respuesta ParametrizationResponse</returns>
        public ParametrizationResponse<ParamLegalRepresentativeSing> SaveLegalRepresentativeSing(List<ParamLegalRepresentativeSing> legalRepresentativeSingsAdded, List<ParamLegalRepresentativeSing> legalRepresentativeSingsEdited)
        {
            Stopwatch stopWatch = new Stopwatch();
            ParametrizationResponse<ParamLegalRepresentativeSing> returnLegalRepresentativeSings = new ParametrizationResponse<ParamLegalRepresentativeSing>();
            stopWatch.Start();
            using (Context.Current)
            {
                if (legalRepresentativeSingsAdded != null)
                {
                    using (Transaction transaction = new Transaction())
                    {
                        try
                        {
                            foreach (ParamLegalRepresentativeSing item in legalRepresentativeSingsAdded)
                            {
                                CptLegalReprSign entityLegalRepresentativeSing = EntityAssembler.CreateCptLegalReprSign(item);
                                DataFacadeManager.Instance.GetDataFacade().InsertObject(entityLegalRepresentativeSing);
                            }

                            transaction.Complete();
                            returnLegalRepresentativeSings.TotalAdded = legalRepresentativeSingsAdded.Count;
                        }
                        catch (Exception ex)
                        {
                            transaction.Dispose();
                            returnLegalRepresentativeSings.ErrorAdded = Errors.ErrorSaveLegalRepresentativeSingsAdded;
                        }
                    }
                }

                if (legalRepresentativeSingsEdited != null)
                {
                    using (Transaction transaction = new Transaction())
                    {
                        try
                        {
                            foreach (var item in legalRepresentativeSingsEdited)
                            {
                                PrimaryKey key = CptLegalReprSign.CreatePrimaryKey(item.ParamCompanyType.Id, item.ParamBranchType.Id, item.CurrentFrom);
                                CptLegalReprSign cptLegalReprSign = (CptLegalReprSign)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                                cptLegalReprSign.LegalRepresentative = item.LegalRepresentative;
                                cptLegalReprSign.PathSignatureImg = item.PathSignatureImg;
                                cptLegalReprSign.SignatureImg = item.SignatureImg;
                                cptLegalReprSign.UserId = item.UserId;
                                DataFacadeManager.Instance.GetDataFacade().UpdateObject(cptLegalReprSign);
                            }

                            transaction.Complete();
                            returnLegalRepresentativeSings.TotalModify = legalRepresentativeSingsEdited.Count;
                        }
                        catch (Exception ex)
                        {
                            transaction.Dispose();
                            returnLegalRepresentativeSings.ErrorModify = Errors.ErrorSaveLegalRepresentativeSingsModify;
                        }
                    }
                }
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonServices.Providers.SaveInsuredSegments");
            return returnLegalRepresentativeSings;
        }

        /// <summary>
        /// Genera archivo excel de firma representante legal
        /// </summary>
        /// <param name="paramLegalRepresentativeSing">listado de firma representante legal a exportar</param>
        /// <param name="fileName">Nombre de archivo</param>
        /// <returns>lista de firma representante legal</returns>
        public ExcelFileServiceModel GenerateFileToLegalRepresentativeSing(List<ParamLegalRepresentativeSing> paramLegalRepresentativeSing, string fileName)
        {
            List<string> errorModelListDescription = new List<string>();
            try
            {
                FileProcessValue fileProcessValue = new FileProcessValue()
                {
                    Key1 = (int)FileProcessType.ParametrizationLegalReprSign
                };
                FileDAO fileDAO = new FileDAO();
                File file = fileDAO.GetFileByFileProcessValue(fileProcessValue);

                if (file != null && file.IsEnabled)
                {
                    file.Name = fileName;
                    List<Row> rows = new List<Row>();

                    foreach (ParamLegalRepresentativeSing item in paramLegalRepresentativeSing)
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

                        fields[0].Value = item.ParamCompanyType.Description.ToString();
                        fields[1].Value = item.ParamBranchType.Description.ToString();
                        fields[2].Value = item.CurrentFrom.ToString();
                        fields[3].Value = item.LegalRepresentative.ToString();

                        rows.Add(new Row
                        {
                            Fields = fields
                        });
                    }

                    file.Templates[0].Rows = rows;
                    file.Name = fileName + "_" + DateTime.Now.ToString("dd_MM_yyyy");

                    var result = fileDAO.GenerateFile(file);

                    return new ExcelFileServiceModel()
                    {
                        ErrorTypeService = ErrorTypeService.Ok,
                        FileData = result
                    };
                }
                else
                {
                    errorModelListDescription.Add(Errors.ErrorDownloadingExcel);
                    return new ExcelFileServiceModel()
                    {
                        ErrorDescription = errorModelListDescription,
                        ErrorTypeService = ErrorTypeService.TechnicalFault
                    };
                }
            }
            catch (System.Exception)
            {
                errorModelListDescription.Add(Errors.ErrorDownloadingExcel);
                return new ExcelFileServiceModel()
                {
                    ErrorDescription = errorModelListDescription,
                    ErrorTypeService = ErrorTypeService.TechnicalFault
                };
            }
        }
    }
}
