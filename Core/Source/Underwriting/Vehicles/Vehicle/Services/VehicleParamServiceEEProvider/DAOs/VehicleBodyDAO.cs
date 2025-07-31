// -----------------------------------------------------------------------
// <copyright file="VehicleDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Andres Gomez Hernandez</author>
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
    using System.Diagnostics;
    using Sistran.Core.Framework.Contexts;
    using Sistran.Core.Framework.Transactions;
    using Sistran.Core.Framework.Queries;
    using System.Linq;
    using COMMOD = Sistran.Core.Application.CommonService.Models;
    using COMENUM = Sistran.Core.Application.CommonService.Enums;
    using Sistran.Core.Application.VehicleParamService.EEProvider.Resources;

    /// <summary>
    /// Acceso a base de datos para las carrocerias del vehiculo
    /// </summary>
    public class VehicleBodyDAO
    {
        /// <summary>
        /// Obtiene la lista de Versiones de Carrocerias del vehiculo.
        /// </summary>
        /// <returns>Lista de Versiones de Carrocerias consultadas</returns>
        public Result<List<ParamVehicleBody>, ErrorModel> GetParamVehicleBody()
        {
            List<string> errorModel = new List<string>();
            try
            {
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(VehicleBody)));
                Result<List<ParamVehicleBody>, ErrorModel> listParamVehicleVersion = ModelAssembler.CreateParamVehicleBody(businessCollection);
                if (listParamVehicleVersion is ResultError<List<ParamVehicleBody>, ErrorModel>)
                {
                    return listParamVehicleVersion;
                }
                else
                {
                    List<ParamVehicleBody> resultValue = (listParamVehicleVersion as ResultValue<List<ParamVehicleBody>, ErrorModel>).Value;
                    return listParamVehicleVersion;
                }
            }
            catch (Exception ex)
            {
                errorModel.Add(Errors.ErrorQueryVehicleBody);
                return new ResultError<List<ParamVehicleBody>, ErrorModel>(ErrorModel.CreateErrorModel(errorModel, ErrorType.TechnicalFault, ex));
            }
        }
    }
}
