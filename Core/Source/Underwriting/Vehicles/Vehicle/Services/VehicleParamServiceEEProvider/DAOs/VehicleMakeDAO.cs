// -----------------------------------------------------------------------
// <copyright file="ParamVehicleMakeDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>John Jairo Peralta</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.VehicleParamService.EEProvider.DAOs
{
    using Utilities.DataFacade;
    using Models;
    using System.Collections.Generic;
    using Utilities.Error;
    using Framework.DAF;
    using Common.Entities;
    using Assemblers;
    using Sistran.Core.Application.Utilities.Enums;
    using System;
    using Sistran.Core.Framework.Queries;
    using System.Data;
    using Sistran.Core.Framework.Transactions;

    public class VehicleMakeDAO
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns>Listo las marcas de vehiculo</returns>
        public Result<List<ParamVehicleMake>, ErrorModel> GetVehicleMakes()
        {
            List<string> errorModel = new List<string>();
            try
            {
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(VehicleMake)));
                Result<List<ParamVehicleMake>, ErrorModel> lstParamVehicleMake = ModelAssembler.CreateVehicleMakes(businessCollection);
                if (lstParamVehicleMake is ResultError<List<ParamVehicleMake>, ErrorModel>)
                {
                    return lstParamVehicleMake;
                }
                else
                {
                    List<ParamVehicleMake> resultValue = (lstParamVehicleMake as ResultValue<List<ParamVehicleMake>, ErrorModel>).Value;

                    if (resultValue.Count == 0)
                    {
                        errorModel.Add("No se encuentra la marca.");
                        return new ResultError<List<ParamVehicleMake>, ErrorModel>(ErrorModel.CreateErrorModel(errorModel, ErrorType.NotFound, null));
                    }
                    else
                    {
                        return lstParamVehicleMake;
                    }
                }
            }
            catch (Exception ex)
            {
                errorModel.Add("Ocurrio un error inesperado en la consulta de marcas. Comuniquese con el administrador");
                return new ResultError<List<ParamVehicleMake>, ErrorModel>(ErrorModel.CreateErrorModel(errorModel, ErrorType.TechnicalFault, ex));
            }
        }
        public Result<ParamVehicleMake, ErrorModel> ParamVehicleMakeCreate(ParamVehicleMake paramVehicleMake)
        {
            using (Transaction transaction = new Transaction())
            {
                try
                {
                    VehicleMake vehicleMake = EntityAssembler.CreateVehicleMake(paramVehicleMake);
                    DataFacadeManager.Instance.GetDataFacade().InsertObject(vehicleMake);
                    Result<ParamVehicleMake, ErrorModel> vehicleMakepeResult = ModelAssembler.CreateVehicleMake(vehicleMake);
                    transaction.Complete();
                    return vehicleMakepeResult;
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    List<string> listErrors = new List<string>();
                    listErrors.Add(/*Resources.Errors.FailedCreatingPaymentPlanErrorBD*/"Failed Creating Make");
                    return new ResultError<ParamVehicleMake, ErrorModel>(ErrorModel.CreateErrorModel(listErrors, ErrorType.TechnicalFault, ex));
                }
            }

        }
        public Result<ParamVehicleMake, ErrorModel> paramVehicleMakeUpdate(ParamVehicleMake paramVehicleUpdate)
        {
            using (Transaction transaction = new Transaction())
            {
                try
                {
                    ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                    filter.Property(VehicleMake.Properties.VehicleMakeCode, typeof(VehicleMake).Name);
                    filter.Equal();
                    filter.Constant(paramVehicleUpdate.Id);

                    BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(VehicleMake), filter.GetPredicate()));
                    VehicleMake VehicleMakeEntity = new VehicleMake(paramVehicleUpdate.Id);
                    foreach (VehicleMake item in businessCollection)
                    {
                        item.VehicleMakeCode = paramVehicleUpdate.Id;
                        item.SmallDescription = paramVehicleUpdate.Description;
                        DataFacadeManager.Instance.GetDataFacade().UpdateObject(item);
                    };
                    transaction.Complete();
                    Result<ParamVehicleMake, ErrorModel> result = ModelAssembler.CreateVehicleMake(VehicleMakeEntity);
                    ResultValue<ParamVehicleMake, ErrorModel> paramVehicleMakeResult = ((ResultValue<ParamVehicleMake, ErrorModel>)result);
                    return paramVehicleMakeResult;
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    List<string> listErrors = new List<string>();
                    listErrors.Add(/*Resources.Errors.FailedUpdatingPaymentPlanErrorBD*/"Failed Updating Infringement");
                    return new ResultError<ParamVehicleMake, ErrorModel>(ErrorModel.CreateErrorModel(listErrors, ErrorType.TechnicalFault, ex));

                }
            }

        }
        public Result<ParamVehicleMake, ErrorModel> paramVehicleMakeDelete(ParamVehicleMake paramVehicleMakeDelete)
        {
            using (Transaction transaction = new Transaction())
            {
                try
                {
                    ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                    filter.Property(VehicleMake.Properties.VehicleMakeCode, typeof(VehicleMake).Name);
                    filter.Equal();
                    filter.Constant(paramVehicleMakeDelete.Id);

                    BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(VehicleMake), filter.GetPredicate()));
                    VehicleMake vehicleMakeEntity = new VehicleMake(paramVehicleMakeDelete.Id);
                    foreach (VehicleMake item in businessCollection)
                    {
                        item.VehicleMakeCode = paramVehicleMakeDelete.Id;
                        item.SmallDescription = paramVehicleMakeDelete.Description;
                        DataFacadeManager.Instance.GetDataFacade().DeleteObject(item);
                    }

                    transaction.Complete();
                    Result<ParamVehicleMake, ErrorModel> result = ModelAssembler.CreateVehicleMake(vehicleMakeEntity);
                    ResultValue<ParamVehicleMake, ErrorModel> paramVehicleMakeResult = ((ResultValue<ParamVehicleMake, ErrorModel>)result);
                    return paramVehicleMakeResult;
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    List<string> listErrors = new List<string>();
                    listErrors.Add(/*Resources.Errors.FailedUpdatingPaymentPlanErrorBD*/"Failed Updating Infringement");
                    return new ResultError<ParamVehicleMake, ErrorModel>(ErrorModel.CreateErrorModel(listErrors, ErrorType.TechnicalFault, ex));

                }
            }

        }
    }
}