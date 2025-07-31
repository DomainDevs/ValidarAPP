// -----------------------------------------------------------------------
// <copyright file="BusinessDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Cristyan Fernando Ballesteros</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.UnderwritingParamServices.EEProvider.DAOs
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Sistran.Core.Application.Utilities.DataFacade;
    using Sistran.Core.Framework.Contexts;
    using Sistran.Core.Framework.DAF;
    using Sistran.Core.Framework.DAF.Engine;
    using Sistran.Core.Framework.Queries;
    using Sistran.Core.Framework.Transactions;
    using Sistran.Core.Application.Parameters.Entities;
    using Sistran.Core.Application.Common.Entities;
    using Sistran.Core.Application.CommonService.Models;
    using Sistran.Core.Application.UnderwritingParamService.Models;
    using Sistran.Core.Application.UnderwritingParamServices.EEProvider.Assemblers;
    using Sistran.Core.Application.Utilities.Error;
    using Sistran.Core.Application.Utilities.Enums;
    using Sistran.Core.Application.UnderwritingParamService.EEProvider.Assemblers;
    using Sistran.Core.Services.UtilitiesServices.Models;

    /// <summary>
    /// Clase DAO del objeto Business.
    /// </summary>
    public class BusinessDAO
    {
        /// <summary>
        /// Obtiene la lista de negocios.
        /// </summary>
        /// <returns>Lista de negocios</returns>
        public Result<List<ParamBusiness>, ErrorModel> GetBusiness()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            try
            {
                List<ParamBusiness> result = new List<ParamBusiness>();
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(Business)));
                foreach (Business itemBusiness in businessCollection)
                {
                    string prefixDescription = "";
                    string prefixSmallDescription = "";
                    ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                    filter.PropertyEquals(Common.Entities.Prefix.Properties.PrefixCode, itemBusiness.PrefixCode);
                    BusinessCollection prefixResult = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(Common.Entities.Prefix), filter.GetPredicate()));
                    foreach (Common.Entities.Prefix itemPrefix in prefixResult)
                    {
                        prefixDescription = itemPrefix.Description;
                        prefixSmallDescription = itemPrefix.SmallDescription;
                    }
                    Result<ParamBusiness, ErrorModel> itemParamBusiness = ModelAssembler.CreateBusiness(itemBusiness, prefixDescription, prefixSmallDescription);
                    if (itemParamBusiness is ResultError<ParamBusiness, ErrorModel>)
                    {
                        errorModelListDescription.Add("Ocurrio un error mapeando la entidad negocio a modelo de negocio.");
                        return new ResultError<List<ParamBusiness>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.BusinessFault, null));
                    }
                    else
                    {
                        ParamBusiness resultItem = (itemParamBusiness as ResultValue<ParamBusiness, ErrorModel>).Value;
                        result.Add(resultItem);
                    }
                }
                if (result.Count > 0)
                {
                    return new ResultValue<List<ParamBusiness>, ErrorModel>(result);
                }
                else
                {
                    errorModelListDescription.Add("No se encontraron negocios.");
                    return new ResultError<List<ParamBusiness>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.NotFound, null));
                }
            }
            catch (Exception ex)
            {
                errorModelListDescription.Add("Ocurrio un error inesperado en la consulta de negocios. Comuniquese con el administrador");
                return new ResultError<List<ParamBusiness>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.TechnicalFault, ex));
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingParamServices.EEProvider.DAOs");
            }
        }
        public Result<List<ParamBusiness>, ErrorModel> GetBusinessByBusinessCode(int businessCode)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            try
            {
                List<ParamBusiness> result = new List<ParamBusiness>();
                ObjectCriteriaBuilder filterBusiness = new ObjectCriteriaBuilder();
                filterBusiness.PropertyEquals(Business.Properties.BusinessId, businessCode);
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(Business), filterBusiness.GetPredicate()));
                foreach (Business itemBusiness in businessCollection)
                {
                    string prefixDescription = "";
                    string prefixSmallDescription = "";
                    ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                    filter.PropertyEquals(Common.Entities.Prefix.Properties.PrefixCode, itemBusiness.PrefixCode);
                    BusinessCollection prefixResult = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(Common.Entities.Prefix), filter.GetPredicate()));
                    foreach (Common.Entities.Prefix itemPrefix in prefixResult)
                    {
                        prefixDescription = itemPrefix.Description;
                        prefixSmallDescription = itemPrefix.SmallDescription;
                    }
                    Result<ParamBusiness, ErrorModel> itemParamBusiness = ModelAssembler.CreateBusiness(itemBusiness, prefixDescription, prefixSmallDescription);
                    if (itemParamBusiness is ResultError<ParamBusiness, ErrorModel>)
                    {
                        errorModelListDescription.Add("Ocurrio un error mapeando la entidad negocio a modelo de negocio.");
                        return new ResultError<List<ParamBusiness>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.BusinessFault, null));
                    }
                    else
                    {
                        ParamBusiness resultItem = (itemParamBusiness as ResultValue<ParamBusiness, ErrorModel>).Value;
                        result.Add(resultItem);
                    }
                }
                if (result.Count > 0)
                {
                    return new ResultValue<List<ParamBusiness>, ErrorModel>(result);
                }
                else
                {
                    errorModelListDescription.Add("No se encontraron negocios.");
                    return new ResultError<List<ParamBusiness>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.NotFound, null));
                }
            }
            catch (Exception ex)
            {
                errorModelListDescription.Add("Ocurrio un error inesperado en la consulta de negocios. Comuniquese con el administrador");
                return new ResultError<List<ParamBusiness>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.TechnicalFault, ex));
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
        /// <param name="businessAdded">negocios para agregar</param>
        /// <param name="businessEdited">negocios para editar</param>
        /// <param name="businessDeleted">negocios para eliminar</param>
        /// <returns>Resumen de las operaciones</returns>
        public ParametrizationResponse<ParamBusiness> SaveBusiness(List<ParamBusiness> businessAdded, List<ParamBusiness> businessEdited, List<ParamBusiness> businessDeleted)
        {
            Stopwatch stopWatch = new Stopwatch();
            ParametrizationResponse<ParamBusiness> returnBusiness = new ParametrizationResponse<ParamBusiness>();
            stopWatch.Start();
            using (Context.Current)
            {
                // Agregar
                if (businessAdded != null && businessAdded.Count>0)
                {
                    using (Transaction transaction = new Transaction())
                    {
                        try
                        {
                            foreach (ParamBusiness item in businessAdded)
                            {
                                Business entityBusiness = EntityAssembler.CreateBusiness(item);
                                DataFacadeManager.Instance.GetDataFacade().InsertObject(entityBusiness);
                            }

                            transaction.Complete();
                            returnBusiness.TotalAdded = businessAdded.Count;
                        }
                        catch (System.Exception)
                        {
                            transaction.Dispose();
                            returnBusiness.ErrorAdded = "ErrorSaveBusinessAdded";
                        }
                    }


                }

                // Modificar
                if (businessEdited != null && businessEdited.Count > 0)
                {
                    using (Transaction transaction = new Transaction())
                    {
                        try
                        {
                            foreach (ParamBusiness item in businessEdited)
                            {
                                PrimaryKey key = Business.CreatePrimaryKey(item.BusinessId);
                                Business businessEntity = new Business(item.BusinessId);
                                businessEntity = (Business)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                                businessEntity.Description = item.Description;
                                businessEntity.IsEnabled = item.IsEnabled;
                                businessEntity.PrefixCode = item.Prefix.PrefixCode;
                                DataFacadeManager.Instance.GetDataFacade().UpdateObject(businessEntity);
                            }

                            transaction.Complete();
                            returnBusiness.TotalModify = businessEdited.Count;
                        }
                        catch (System.Exception)
                        {
                            transaction.Dispose();
                            returnBusiness.ErrorModify = "ErrorSaveBusinessEdited";
                        }
                    }
   
                }

                // Eliminar
                if (businessDeleted != null && businessDeleted.Count > 0)
                {
                    using (Transaction transaction = new Transaction())
                    {
                        try
                        {
                            foreach (ParamBusiness item in businessDeleted)
                            {
                                PrimaryKey key = Business.CreatePrimaryKey(item.BusinessId);
                                Business businessEntity = new Business(item.BusinessId);
                                businessEntity = (Business)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                                DataFacadeManager.Instance.GetDataFacade().DeleteObject(businessEntity);
                            }

                            transaction.Complete();
                            returnBusiness.TotalDeleted = businessDeleted.Count;
                        }
                        catch (ForeignKeyException)
                        {
                            transaction.Dispose();
                            returnBusiness.ErrorDeleted = "ErrorSaveBusinessRelated";
                        }
                        catch (RelatedObjectException)
                        {
                            transaction.Dispose();
                            returnBusiness.ErrorDeleted = "ErrorSaveBusinessRelated";
                        }
                        catch (System.Exception)
                        {
                            transaction.Dispose();
                            returnBusiness.ErrorDeleted = "ErrorSaveBusinessDeleted";
                        }
                    }

                }
            }
            Result<List<ParamBusiness>, ErrorModel> result = this.GetBusiness();
            returnBusiness.ReturnedList = (result as ResultValue<List<ParamBusiness>, ErrorModel>).Value;

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.SaveBusiness");
            return returnBusiness;
        }
    }
}
