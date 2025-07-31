// -----------------------------------------------------------------------
// <copyright file="VehicleTypeDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Stiveen Niño</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.EEProvider.DAOs
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Sistran.Core.Application.Common.Entities;
    using Sistran.Core.Application.Parameters.Entities;
    using Sistran.Core.Application.UnderwritingParamService.EEProvider.Assemblers;
    using Sistran.Core.Application.UnderwritingParamService.EEProvider.Entities.Views;
    using Sistran.Core.Application.UnderwritingParamService.EEProvider.Resources;
    using Sistran.Core.Application.Utilities.DataFacade;
    using Sistran.Core.Application.Utilities.Error;
    using Sistran.Core.Framework.DAF;
    using Sistran.Core.Framework.DAF.Engine;
    using Sistran.Core.Framework.Queries;
    using Sistran.Core.Framework.Transactions;
    using COMENUM = Sistran.Core.Application.CommonService.Enums;
    using COMMOD = Sistran.Core.Application.CommonService.Models;
    using Sistran.Core.Application.UnderwritingParamServices.EEProvider.Assemblers;
    using Sistran.Core.Application.UnderwritingParamService.Models;
    using UTMO = Sistran.Core.Application.Utilities.Error;
    using ENUM = Sistran.Core.Application.UnderwritingParamService.Enums;
    using QUOEN = Sistran.Core.Application.Quotation.Entities;
    using ENUMUT = Sistran.Core.Application.Utilities.Enums;
    using Sistran.Core.Application.Utilities.Enums;
    using System;

    /// <summary>
    /// Dao para Tipo de tasa 
    /// </summary>
    public class RateTypeDAO
    {
        /// <summary>
        /// Obtener Objetos Del Seguro 
        /// </summary>       
        /// <returns>Objetos Del Seguro</returns>
        public UTMO.Result<List<ParamRateType>, UTMO.ErrorModel> GetRateType()
        {
            using (Transaction transaction = new Transaction())
            {
                List<ParamRateType> paramRateType = new List<ParamRateType>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            List<string> errorModel = new List<string>();
            try
            {
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(RateType), filter.GetPredicate()));
            Result<List<ParamRateType>, ErrorModel> lstParamRateType = ModelAssembler.CreateRateTypes(businessCollection);
            if (lstParamRateType is ResultError<List<ParamRateType>, ErrorModel>)
            {
                return lstParamRateType;
            }
            else
            {
                List<ParamRateType> resultValue = (lstParamRateType as ResultValue<List<ParamRateType>, ErrorModel>).Value;

                if (resultValue.Count == 0)
                {
                        errorModel.Add("No se encuentra El metodo de pago.");
                    return new ResultError<List<ParamRateType>, ErrorModel>(ErrorModel.CreateErrorModel(errorModel, ErrorType.NotFound, null));
                }
                else
                {
                    return lstParamRateType;
                }

                }
            }
            catch (Exception ex)
            {
                errorModel.Add("Ocurrio un error inesperado en la consulta de metodos de pago. Comuniquese con el administrador");
                return new ResultError<List<ParamRateType>, ErrorModel>(ErrorModel.CreateErrorModel(errorModel, ErrorType.TechnicalFault, ex));
            }
           }
        }
    }

}

