// -----------------------------------------------------------------------
// <copyright file="BusinessConfigurationDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Cristyan Fernando Ballesteros</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.UnderwritingParamServices.EEProvider.DAOs
{
    using Sistran.Core.Application.Product.Entities;
    using Sistran.Core.Application.Quotation.Entities;
    using Sistran.Core.Application.Request.Entities;
    using Sistran.Core.Application.UnderwritingParamService.EEProvider.Assemblers;
    using Sistran.Core.Application.UnderwritingParamService.Models;
    using Sistran.Core.Application.UnderwritingParamServices.EEProvider.Assemblers;
    using Sistran.Core.Application.Utilities.DataFacade;
    using Sistran.Core.Application.Utilities.Enums;
    using Sistran.Core.Application.Utilities.Error;
    using Sistran.Core.Framework.Contexts;
    using Sistran.Core.Framework.DAF;
    using Sistran.Core.Framework.Queries;
    using Sistran.Core.Framework.Transactions;
    using Sistran.Core.Services.UtilitiesServices.Models;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using UTILENUM = Sistran.Core.Services.UtilitiesServices.Enums;

    /// <summary>
    /// Clase DAO del objeto BusinessConfiguration.
    /// </summary>
    public class BusinessConfigurationDAO
    {
        /// <summary>
        /// Obtiene la lista de acuerdos de negocios.
        /// </summary>
        /// <returns>Lista de acuerdos de negocios</returns>
        public Result<List<ParamBusinessConfiguration>, ErrorModel> GetBusinessConfiguration()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            try
            {
                List<ParamBusinessConfiguration> result = new List<ParamBusinessConfiguration>();
                BusinessCollection businessConfigurationCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(BusinessConfiguration)));
                foreach (BusinessConfiguration itemBusinessConfiguration in businessConfigurationCollection)
                {
                    //RequestEndorsement
                    CoRequestEndorsement requestEndorsementEntity = new CoRequestEndorsement();
                    if (itemBusinessConfiguration.RequestId != null)
                    {
                        requestEndorsementEntity.RequestEndorsementId = 0;
                        requestEndorsementEntity.RequestId = (int)itemBusinessConfiguration.RequestId;
                        requestEndorsementEntity.ProductId = 0;
                        requestEndorsementEntity.PrefixCode = 0;
                    }
                    else
                    {
                        requestEndorsementEntity = null;
                    }

                    //Product
                    Product productEntity = null;
                    ObjectCriteriaBuilder filterProduct = new ObjectCriteriaBuilder();
                    filterProduct.PropertyEquals(Product.Properties.ProductId, itemBusinessConfiguration.ProductId);
                    BusinessCollection productResult = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(Product), filterProduct.GetPredicate()));
                    foreach (Product itemProduct in productResult)
                    {
                        productEntity = itemProduct;
                    }

                    //ProductGroupCover
                    ProductGroupCover productGroupCoverEntity = null;
                    ObjectCriteriaBuilder filterProductGroupCover = new ObjectCriteriaBuilder();
                    filterProductGroupCover.PropertyEquals(ProductGroupCover.Properties.CoverGroupId, itemBusinessConfiguration.GroupCoverageId);
                    filterProductGroupCover.And();
                    filterProductGroupCover.PropertyEquals(ProductGroupCover.Properties.ProductId, itemBusinessConfiguration.ProductId);
                    BusinessCollection productGroupCoverResult = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(ProductGroupCover), filterProductGroupCover.GetPredicate()));
                    foreach (ProductGroupCover itemProductGroupCover in productGroupCoverResult)
                    {
                        productGroupCoverEntity = itemProductGroupCover;
                    }

                    //AssistanceType
                    Common.Entities.CptAssistanceType assistanceTypeEntity = null;
                    ObjectCriteriaBuilder filterAssistanceType = new ObjectCriteriaBuilder();
                    filterAssistanceType.PropertyEquals(Common.Entities.CptAssistanceType.Properties.AssistanceCode, itemBusinessConfiguration.AssistanceCode);
                    BusinessCollection assistanceTypeResult = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(Common.Entities.CptAssistanceType), filterAssistanceType.GetPredicate()));
                    foreach (Common.Entities.CptAssistanceType itemAssistanceType in assistanceTypeResult)
                    {
                        assistanceTypeEntity = itemAssistanceType;
                    }
                    Result<ParamBusinessConfiguration, ErrorModel> itemParamBusinessConfiguration = ModelAssembler.CreateBusinessConfiguration(itemBusinessConfiguration, requestEndorsementEntity,productEntity,productGroupCoverEntity,assistanceTypeEntity);
                    if (itemParamBusinessConfiguration is ResultError<ParamBusinessConfiguration, ErrorModel>)
                    {
                        errorModelListDescription.Add("Ocurrio un error mapeando la entidad acuerdo de negocio a modelo de negocio.");
                        return new ResultError<List<ParamBusinessConfiguration>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.BusinessFault, null));
                    }
                    else
                    {
                        ParamBusinessConfiguration resultItem = (itemParamBusinessConfiguration as ResultValue<ParamBusinessConfiguration, ErrorModel>).Value;
                        result.Add(resultItem);                       
                    }
                }
                if (result.Count > 0)
                {
                    return new ResultValue<List<ParamBusinessConfiguration>, ErrorModel>(result);
                }
                else
                {
                    errorModelListDescription.Add("No se encontraron acuerdos de negocios.");
                    return new ResultError<List<ParamBusinessConfiguration>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.NotFound, null));
                }
            }
            catch (Exception ex)
            {
                errorModelListDescription.Add("Ocurrio un error inesperado en la consulta de acuerdos de negocios. Comuniquese con el administrador");
                return new ResultError<List<ParamBusinessConfiguration>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.TechnicalFault, ex));
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingParamServices.EEProvider.DAOs");
            }
        }
        public Result<List<ParamBusinessConfiguration>, ErrorModel> GetBusinessConfigurationByBusinessConfigurationCode(int businessConfigurationCode)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            try
            {
                List<ParamBusinessConfiguration> result = new List<ParamBusinessConfiguration>();
                ObjectCriteriaBuilder filterBusinessConfiguration = new ObjectCriteriaBuilder();
                filterBusinessConfiguration.PropertyEquals(BusinessConfiguration.Properties.BusinessConfigurationId, businessConfigurationCode);
                BusinessCollection businessConfigurationCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(BusinessConfiguration), filterBusinessConfiguration.GetPredicate()));
                foreach (BusinessConfiguration itemBusinessConfiguration in businessConfigurationCollection)
                {
                    //RequestEndorsement
                    CoRequestEndorsement requestEndorsementEntity = new CoRequestEndorsement();
                    if (itemBusinessConfiguration.RequestId != null)
                    {
                        requestEndorsementEntity.RequestEndorsementId = 0;
                        requestEndorsementEntity.RequestId = (int)itemBusinessConfiguration.RequestId;
                        requestEndorsementEntity.ProductId = 0;
                        requestEndorsementEntity.PrefixCode = 0;
                    }
                    else
                    {
                        requestEndorsementEntity = null;
                    }

                    //Product
                    Product productEntity = null;
                    ObjectCriteriaBuilder filterProduct = new ObjectCriteriaBuilder();
                    filterProduct.PropertyEquals(Product.Properties.ProductId, itemBusinessConfiguration.ProductId);
                    BusinessCollection productResult = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(Product), filterProduct.GetPredicate()));
                    foreach (Product itemProduct in productResult)
                    {
                        productEntity = itemProduct;
                    }

                    //ProductGroupCover
                    ProductGroupCover productGroupCoverEntity = null;
                    ObjectCriteriaBuilder filterProductGroupCover = new ObjectCriteriaBuilder();
                    filterProductGroupCover.PropertyEquals(ProductGroupCover.Properties.CoverGroupId, itemBusinessConfiguration.GroupCoverageId);
                    filterProductGroupCover.And();
                    filterProductGroupCover.PropertyEquals(ProductGroupCover.Properties.ProductId, itemBusinessConfiguration.ProductId);
                    BusinessCollection productGroupCoverResult = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(ProductGroupCover), filterProductGroupCover.GetPredicate()));
                    foreach (ProductGroupCover itemProductGroupCover in productGroupCoverResult)
                    {
                        productGroupCoverEntity = itemProductGroupCover;
                    }

                    //AssistanceType
                    Common.Entities.CptAssistanceType assistanceTypeEntity = null;
                    ObjectCriteriaBuilder filterAssistanceType = new ObjectCriteriaBuilder();
                    filterAssistanceType.PropertyEquals(Common.Entities.CptAssistanceType.Properties.AssistanceCode, itemBusinessConfiguration.AssistanceCode);
                    BusinessCollection assistanceTypeResult = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(Common.Entities.CptAssistanceType), filterAssistanceType.GetPredicate()));
                    foreach (Common.Entities.CptAssistanceType itemAssistanceType in assistanceTypeResult)
                    {
                        assistanceTypeEntity = itemAssistanceType;
                    }
                    Result<ParamBusinessConfiguration, ErrorModel> itemParamBusinessConfiguration = ModelAssembler.CreateBusinessConfiguration(itemBusinessConfiguration, requestEndorsementEntity, productEntity, productGroupCoverEntity, assistanceTypeEntity);
                    if (itemParamBusinessConfiguration is ResultError<ParamBusinessConfiguration, ErrorModel>)
                    {
                        errorModelListDescription.Add("Ocurrio un error mapeando la entidad acuerdo de negocio a modelo de negocio.");
                        return new ResultError<List<ParamBusinessConfiguration>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.BusinessFault, null));
                    }
                    else
                    {
                        ParamBusinessConfiguration resultItem = (itemParamBusinessConfiguration as ResultValue<ParamBusinessConfiguration, ErrorModel>).Value;
                        result.Add(resultItem);
                    }
                }
                if (result.Count > 0)
                {
                    return new ResultValue<List<ParamBusinessConfiguration>, ErrorModel>(result);
                }
                else
                {
                    errorModelListDescription.Add("No se encontraron acuerdos de negocios.");
                    return new ResultError<List<ParamBusinessConfiguration>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.NotFound, null));
                }
            }
            catch (Exception ex)
            {
                errorModelListDescription.Add("Ocurrio un error inesperado en la consulta de acuerdos de negocios. Comuniquese con el administrador");
                return new ResultError<List<ParamBusinessConfiguration>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.TechnicalFault, ex));
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingParamServices.EEProvider.DAOs");
            }
        }

        public Result<List<ParamBusinessConfiguration>, ErrorModel> GetBusinessConfigurationByBusinessCode(int businessCode)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            try
            {
                List<ParamBusinessConfiguration> result = new List<ParamBusinessConfiguration>();
                ObjectCriteriaBuilder filterBusinessConfiguration = new ObjectCriteriaBuilder();
                filterBusinessConfiguration.PropertyEquals(BusinessConfiguration.Properties.BusinessId, businessCode);
                BusinessCollection businessConfigurationCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(BusinessConfiguration), filterBusinessConfiguration.GetPredicate()));
                foreach (BusinessConfiguration itemBusinessConfiguration in businessConfigurationCollection)
                {
                    //RequestEndorsement
                    CoRequestEndorsement requestEndorsementEntity = new CoRequestEndorsement();
                    if (itemBusinessConfiguration.RequestId != null)
                    {
                        requestEndorsementEntity.RequestEndorsementId = 0;
                        requestEndorsementEntity.RequestId = (int)itemBusinessConfiguration.RequestId;
                        requestEndorsementEntity.ProductId = 0;
                        requestEndorsementEntity.PrefixCode = 0;
                    }
                    else
                    {
                        requestEndorsementEntity = null;
                    }

                    //Product
                    Product productEntity = null;
                    ObjectCriteriaBuilder filterProduct = new ObjectCriteriaBuilder();
                    filterProduct.PropertyEquals(Product.Properties.ProductId, itemBusinessConfiguration.ProductId);
                    BusinessCollection productResult = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(Product), filterProduct.GetPredicate()));
                    foreach (Product itemProduct in productResult)
                    {
                        productEntity = itemProduct;
                    }

                    //ProductGroupCover
                    ProductGroupCover productGroupCoverEntity = null;
                    ObjectCriteriaBuilder filterProductGroupCover = new ObjectCriteriaBuilder();
                    filterProductGroupCover.PropertyEquals(ProductGroupCover.Properties.CoverGroupId, itemBusinessConfiguration.GroupCoverageId);
                    filterProductGroupCover.And();
                    filterProductGroupCover.PropertyEquals(ProductGroupCover.Properties.ProductId, itemBusinessConfiguration.ProductId);
                    BusinessCollection productGroupCoverResult = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(ProductGroupCover), filterProductGroupCover.GetPredicate()));
                    foreach (ProductGroupCover itemProductGroupCover in productGroupCoverResult)
                    {
                        productGroupCoverEntity = itemProductGroupCover;
                    }

                    //AssistanceType
                    Common.Entities.CptAssistanceType assistanceTypeEntity = null;
                    ObjectCriteriaBuilder filterAssistanceType = new ObjectCriteriaBuilder();
                    filterAssistanceType.PropertyEquals(Common.Entities.CptAssistanceType.Properties.AssistanceCode, itemBusinessConfiguration.AssistanceCode);
                    BusinessCollection assistanceTypeResult = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(Common.Entities.CptAssistanceType), filterAssistanceType.GetPredicate()));
                    foreach (Common.Entities.CptAssistanceType itemAssistanceType in assistanceTypeResult)
                    {
                        assistanceTypeEntity = itemAssistanceType;
                    }
                    Result<ParamBusinessConfiguration, ErrorModel> itemParamBusinessConfiguration = ModelAssembler.CreateBusinessConfiguration(itemBusinessConfiguration, requestEndorsementEntity, productEntity, productGroupCoverEntity, assistanceTypeEntity);
                    if (itemParamBusinessConfiguration is ResultError<ParamBusinessConfiguration, ErrorModel>)
                    {
                        errorModelListDescription.Add("Ocurrio un error mapeando la entidad acuerdo de negocio a modelo de negocio.");
                        return new ResultError<List<ParamBusinessConfiguration>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.BusinessFault, null));
                    }
                    else
                    {
                        ParamBusinessConfiguration resultItem = (itemParamBusinessConfiguration as ResultValue<ParamBusinessConfiguration, ErrorModel>).Value;
                        result.Add(resultItem);
                    }
                }
                if (result.Count > 0)
                {
                    return new ResultValue<List<ParamBusinessConfiguration>, ErrorModel>(result);
                }
                else
                {
                    errorModelListDescription.Add("No se encontraron acuerdos de negocios.");
                    return new ResultError<List<ParamBusinessConfiguration>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.NotFound, null));
                }
            }
            catch (Exception ex)
            {
                errorModelListDescription.Add("Ocurrio un error inesperado en la consulta de acuerdos de negocios. Comuniquese con el administrador");
                return new ResultError<List<ParamBusinessConfiguration>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.TechnicalFault, ex));
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingParamServices.EEProvider.DAOs");
            }
        }

        /// <summary>
        /// Realiza las operaciones CRUD para el tipo de dirección
        /// </summary>
        /// <param name="businessConfigurationAdded">acuerdos de negocios para agregar</param>
        /// <param name="businessConfigurationEdited">acuerdos de negocios para editar</param>
        /// <param name="businessConfigurationDeleted">acuerdos de negocios para eliminar</param>
        /// <returns>Resumen de las operaciones</returns>
        public ParametrizationResponse<ParamBusinessConfiguration> SaveBusinessConfiguration(List<ParamBusinessConfiguration> businessConfigurationAdded, List<ParamBusinessConfiguration> businessConfigurationEdited, List<ParamBusinessConfiguration> businessConfigurationDeleted)
        {
            Stopwatch stopWatch = new Stopwatch();
            ParametrizationResponse<ParamBusinessConfiguration> returnBusinessConfiguration = new ParametrizationResponse<ParamBusinessConfiguration>();
            stopWatch.Start();
            using (Context.Current)
            {
                // Agregar
                if (businessConfigurationAdded != null && businessConfigurationAdded.Count>0)
                {
                    using (Transaction transaction = new Transaction())
                    {
                        try
                        {
                            foreach (ParamBusinessConfiguration item in businessConfigurationAdded)
                            {
                                BusinessConfiguration entityBusinessConfiguration = EntityAssembler.CreateBusinessConfiguration(item);
                                DataFacadeManager.Instance.GetDataFacade().InsertObject(entityBusinessConfiguration);
                            }

                            transaction.Complete();
                            returnBusinessConfiguration.TotalAdded = businessConfigurationAdded.Count;
                        }
                        catch (System.Exception)
                        {
                            transaction.Dispose();
                            returnBusinessConfiguration.ErrorAdded = "ErrorSaveBusinessConfigurationAdded";
                        }
                    }
                }

                // Modificar
                if (businessConfigurationEdited != null && businessConfigurationEdited.Count > 0)
                {
                    using (Transaction transaction = new Transaction())
                    {
                        try
                        {
                            foreach (ParamBusinessConfiguration item in businessConfigurationEdited)
                            {
                                PrimaryKey key = BusinessConfiguration.CreatePrimaryKey(item.BusinessConfigurationId);
                                BusinessConfiguration businessConfigurationEntity = new BusinessConfiguration(item.BusinessConfigurationId);
                                businessConfigurationEntity = (BusinessConfiguration)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                                if (item.Request==null)
                                {
                                    businessConfigurationEntity.RequestId = null;
                                }
                                else
                                {
                                    businessConfigurationEntity.RequestId = item.Request.RequestId;
                                }                              
                                businessConfigurationEntity.ProductId = item.Product.ProductId;
                                businessConfigurationEntity.GroupCoverageId = item.GroupCoverage.GroupCoverageId;
                                businessConfigurationEntity.AssistanceCode = item.Assistance.AssistanceCode;
                                businessConfigurationEntity.ProductIdResponse = item.ProductIdResponse;
                                DataFacadeManager.Instance.GetDataFacade().UpdateObject(businessConfigurationEntity);
                            }

                            transaction.Complete();
                            returnBusinessConfiguration.TotalModify = businessConfigurationEdited.Count;
                        }
                        catch (System.Exception)
                        {
                            transaction.Dispose();
                            returnBusinessConfiguration.ErrorModify = "ErrorSaveBusinessConfigurationEdited";
                        }
                    }
                }

                // Eliminar
                if (businessConfigurationDeleted != null && businessConfigurationDeleted.Count > 0)
                {
                    using (Transaction transaction = new Transaction())
                    {
                        try
                        {
                            foreach (ParamBusinessConfiguration item in businessConfigurationDeleted)
                            {
                                PrimaryKey key = BusinessConfiguration.CreatePrimaryKey(item.BusinessConfigurationId);
                                BusinessConfiguration businessConfigurationEntity = new BusinessConfiguration(item.BusinessConfigurationId);
                                businessConfigurationEntity = (BusinessConfiguration)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                                DataFacadeManager.Instance.GetDataFacade().DeleteObject(businessConfigurationEntity);
                            }

                            transaction.Complete();
                            returnBusinessConfiguration.TotalDeleted = businessConfigurationDeleted.Count;
                        }
                        catch (ForeignKeyException)
                        {
                            transaction.Dispose();
                            returnBusinessConfiguration.ErrorDeleted = "ErrorSaveBusinessConfigurationRelated";
                        }
                        catch (RelatedObjectException)
                        {
                            transaction.Dispose();
                            returnBusinessConfiguration.ErrorDeleted = "ErrorSaveBusinessConfigurationRelated";
                        }
                        catch (System.Exception)
                        {
                            transaction.Dispose();
                            returnBusinessConfiguration.ErrorDeleted = "ErrorSaveBusinessConfigurationDeleted";
                        }
                    }
                }
                Result<List<ParamBusinessConfiguration>, ErrorModel> result = this.GetBusinessConfiguration();
                returnBusinessConfiguration.ReturnedList = (result as ResultValue<List<ParamBusinessConfiguration>, ErrorModel>).Value;
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.SaveBusinessConfiguration");
            return returnBusinessConfiguration;
        }

        /// <summary>
        /// Generar archivo excel de coberturas
        /// </summary>
        /// <param name="fileName">nombre de archivo</param>
        /// <returns>archivo excel </returns>
        public Result<string, ErrorModel> GenerateFileToBusiness(string fileName)
        {
            try
            {
                FileProcessValue fileProcessValue = new FileProcessValue()
                {
                    Key1 = (int)UTILENUM.FileProcessType.BusinessConfiguration
                };
                FileDAO fileDAO = new FileDAO();
                File file = fileDAO.GetFileByFileProcessValue(fileProcessValue);

                if (file != null && file.IsEnabled)
                {
                    BusinessDAO businessDAO = new BusinessDAO();
                    Result<List<ParamBusiness>, ErrorModel> listBusiness = businessDAO.GetBusiness();
                    List<ParamBusiness> listBusinessResult = new List<ParamBusiness>();
                    Result<List<ParamBusinessConfiguration>, ErrorModel> listBusinessConfiguration = this.GetBusinessConfiguration();
                    List<ParamBusinessConfiguration> listBusinessConfigurationResult = new List<ParamBusinessConfiguration>();
                    if (listBusiness is ResultError<List<ParamBusiness>, ErrorModel>)
                    {
                        return new ResultError<string, ErrorModel>(ErrorModel.CreateErrorModel(new List<string>() { UnderwritingParamService.EEProvider.Resources.Errors.ErrorDownloadingExcel }, ErrorType.TechnicalFault, new System.ArgumentException(UnderwritingParamService.EEProvider.Resources.Errors.ErrorDownloadingExcel, "original")));
                    }
                    else
                    {
                        listBusinessResult = (listBusiness as ResultValue<List<ParamBusiness>, ErrorModel>).Value;
                    }
                    if (listBusinessConfiguration is ResultError<List<ParamBusinessConfiguration>, ErrorModel>)
                    {
                        return new ResultError<string, ErrorModel>(ErrorModel.CreateErrorModel(new List<string>() { UnderwritingParamService.EEProvider.Resources.Errors.ErrorDownloadingExcel }, ErrorType.TechnicalFault, new System.ArgumentException(UnderwritingParamService.EEProvider.Resources.Errors.ErrorDownloadingExcel, "original")));
                    }
                    else
                    {
                        listBusinessConfigurationResult = (listBusinessConfiguration as ResultValue<List<ParamBusinessConfiguration>, ErrorModel>).Value;
                    }
                    file.Name = fileName;
                    List<Row> rows = new List<Row>();

                    foreach (ParamBusiness item in listBusinessResult)
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

                        fields[0].Value = item.BusinessId.ToString();
                        fields[1].Value = item.Description;
                        fields[2].Value = item.Prefix.Description;
                        if (item.IsEnabled==true)
                        {
                            fields[3].Value = "Habilitado";
                        }
                        else
                        {
                            fields[3].Value = "Deshabilitado";
                        }
                        

                        rows.Add(new Row
                        {
                            Fields = fields
                        });
                    }
                    file.Templates[0].Rows = rows;

                    rows = new List<Row>();
                    foreach (ParamBusinessConfiguration item in listBusinessConfigurationResult)
                    {
                        var fields = file.Templates[1].Rows[0].Fields.Select(x => new Field
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

                        fields[0].Value = listBusinessResult.First(i=>i.BusinessId== item.BusinessId).Description + " - " + item.BusinessId.ToString();
                        if (item.Request!=null)
                        {
                            fields[1].Value = item.Request.RequestId.ToString();
                        }
                        else
                        {
                            fields[1].Value = "";
                        }
                        fields[2].Value = item.Product.ProductDescription +" - "+ item.Product.ProductId;
                        fields[3].Value = item.GroupCoverage.GroupCoverageSmallDescription + " - " + item.GroupCoverage.GroupCoverageId;
                        fields[4].Value = item.Assistance.AssistanceDescription + " - " + item.Assistance.AssistanceCode;
                        if (item.ProductIdResponse!=null)
                        {
                            fields[5].Value = item.ProductIdResponse;
                        }
                        else
                        {
                            fields[5].Value = "";
                        }

                        rows.Add(new Row
                        {
                            Fields = fields
                        });
                    }
                    file.Templates[1].Rows = rows;



                    file.Name = fileName + "_" + DateTime.Now.ToString("dd_MM_yyyy");

                    var result = fileDAO.GenerateFile(file);
                    return new ResultValue<string, ErrorModel>(result);
                }
                else
                {
                    return new ResultError<string, ErrorModel>(ErrorModel.CreateErrorModel(new List<string>() { UnderwritingParamService.EEProvider.Resources.Errors.ErrorDownloadingExcel }, ErrorType.TechnicalFault, new System.ArgumentException(UnderwritingParamService.EEProvider.Resources.Errors.ErrorDownloadingExcel, "original")));
                }
            }
            catch (System.Exception ex)
            {
                return new ResultError<string, ErrorModel>(ErrorModel.CreateErrorModel(new List<string>() { UnderwritingParamService.EEProvider.Resources.Errors.ErrorDownloadingExcel }, ErrorType.TechnicalFault, ex));
            }
        }
    }
}
