// -----------------------------------------------------------------------
// <copyright file="AssistanceTypeDAO.cs" company="SISTRAN">
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
    using Sistran.Core.Framework.DAF;
    using Sistran.Core.Framework.DAF.Engine;
    using Sistran.Core.Framework.Queries;
    using Sistran.Core.Framework.Transactions;
    using Sistran.Core.Application.Parameters.Entities;
    using Sistran.Core.Application.Common.Entities;
    using Sistran.Core.Application.Product.Entities;
    using Sistran.Core.Application.UnderwritingParamService.Models;
    using Sistran.Core.Application.UnderwritingParamServices.EEProvider.Assemblers;
    using Sistran.Core.Application.Utilities.Error;
    using Sistran.Core.Application.Utilities.Enums;
    using Sistran.Core.Application.Issuance.Entities;

    /// <summary>
    /// Clase DAO del objeto AssistanceType.
    /// </summary>
    public class AssistanceTypeDAO
    {
        /// <summary>
        /// Obtiene la lista de asistencias.
        /// </summary>
        /// <returns>Lista de asistencias</returns>
        public Result<List<ParamAssistanceType>, ErrorModel> GetAssistanceTypes()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            try
            {
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CptAssistanceType)));
                Result<List<ParamAssistanceType>, ErrorModel> assistanceType = ModelAssembler.CreateAssistanceType(businessCollection);
                if (assistanceType is ResultError<List<ParamAssistanceType>, ErrorModel>)
                {
                    return assistanceType;
                }
                else
                {
                    List<ParamAssistanceType> resultValue = (assistanceType as ResultValue<List<ParamAssistanceType>, ErrorModel>).Value;
                    if (resultValue.Count == 0)
                    {
                        errorModelListDescription.Add("No se encontraron asistencias.");
                        return new ResultError<List<ParamAssistanceType>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.NotFound, null));

                    }
                    else
                    {
                        return assistanceType;
                    }
                }
            }
            catch (Exception ex)
            {
                errorModelListDescription.Add("Ocurrio un error inesperado en la consulta de asistencias. Comuniquese con el administrador");
                return new ResultError<List<ParamAssistanceType>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.TechnicalFault, ex));
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingParamServices.EEProvider.DAOs");
            }
        }
        public Result<List<ParamAssistanceType>, ErrorModel> GetAssistanceTypeByAssistanceTypeCode(int assistanceCode)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<string> errorModelListDescription = new List<string>();
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.PropertyEquals(CptAssistanceType.Properties.AssistanceCode, assistanceCode);
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CptAssistanceType), filter.GetPredicate()));
                Result<List<ParamAssistanceType>, ErrorModel> assistanceType = ModelAssembler.CreateAssistanceType(businessCollection);
                if (assistanceType is ResultError<List<ParamAssistanceType>, ErrorModel>)
                {
                    return assistanceType;
                }
                else
                {
                    List<ParamAssistanceType> resultValue = (assistanceType as ResultValue<List<ParamAssistanceType>, ErrorModel>).Value;
                    if (resultValue.Count == 0)
                    {
                        errorModelListDescription.Add("No se encontraron asistencias.");
                        return new ResultError<List<ParamAssistanceType>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.NotFound, null));

                    }
                    else
                    {
                        return assistanceType;
                    }
                }
            }
            catch (Exception ex)
            {
                errorModelListDescription.Add("Ocurrio un error inesperado en la consulta de asistencias. Comuniquese con el administrador");
                return new ResultError<List<ParamAssistanceType>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.TechnicalFault, ex));
            }
            finally
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingParamServices.EEProvider.DAOs");
            }
        }

        public Result<List<ParamAssistanceType>, ErrorModel> GetAssistanceTypeByProductId(int productId)
        {
            return null;
            //Stopwatch stopWatch = new Stopwatch();
            //stopWatch.Start();
            //List<string> errorModelListDescription = new List<string>();
            //try
            //{
            //    ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            //    filter.PropertyEquals(CptProductBranchAssistanceType.Properties.ProductId, productId);
            //    BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CptProductBranchAssistanceType), filter.GetPredicate()));
            //    if (businessCollection != null && businessCollection.Count > 0)
            //    {
            //        List<ParamAssistanceType> listAssistanceType = new List<ParamAssistanceType>();
            //        foreach (CptProductBranchAssistanceType cptProductBranchAssistanceType in businessCollection)
            //        {
            //            Result<List<ParamAssistanceType>, ErrorModel> item = GetAssistanceTypeByAssistanceTypeCode(cptProductBranchAssistanceType.AssistanceCode);
            //            if (item is ResultError<List<ParamAssistanceType>, ErrorModel>)
            //            {
            //                return item;
            //            }
            //            else
            //            {
            //                List<ParamAssistanceType> resultItem = (item as ResultValue<List<ParamAssistanceType>, ErrorModel>).Value;
            //                foreach (ParamAssistanceType paramAssistance in resultItem)
            //                {
            //                    listAssistanceType.Add(paramAssistance);
            //                }
            //            }
            //        }
            //        if (listAssistanceType.Count==0)
            //        {
            //            errorModelListDescription.Add("No se encontraron asistencias.");
            //            return new ResultError<List<ParamAssistanceType>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.NotFound, null));
            //        }
            //        else
            //        {
            //            return new ResultValue<List<ParamAssistanceType>, ErrorModel>(listAssistanceType);
            //        }
            //    }
            //    else
            //    {
            //        errorModelListDescription.Add("No se encontraron asistencias.");
            //        return new ResultError<List<ParamAssistanceType>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.NotFound, null));
            //    }
            //}
            //catch (Exception ex)
            //{
            //    errorModelListDescription.Add("Ocurrio un error inesperado en la consulta de asistencias. Comuniquese con el administrador");
            //    return new ResultError<List<ParamAssistanceType>, ErrorModel>(ErrorModel.CreateErrorModel(errorModelListDescription, ErrorType.TechnicalFault, ex));
            //}
            //finally
            //{
            //    stopWatch.Stop();
            //    Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingParamServices.EEProvider.DAOs");
            //}
        }
    }
}
