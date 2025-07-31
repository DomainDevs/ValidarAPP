// -----------------------------------------------------------------------
// <copyright file="CurrencyDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Gina Gómez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.EEProvider.DAOs
{
    using System.Collections.Generic;
    using CommonService.Models;
    using Framework.DAF;    
    using Sistran.Core.Application.UnderwritingParamServices.EEProvider.Assemblers;
    using UnderwritingParamService.EEProvider.Assemblers;
    using Utilities.DataFacade;
    using COMMEN = Sistran.Core.Application.Common.Entities;
    using ENUMUT = Sistran.Core.Application.Utilities.Enums;
    using UTMO = Sistran.Core.Application.Utilities.Error;

    /// <summary>
    /// Clase pública monedas
    /// </summary>
    public class CurrencyDAO
    {
        /// <summary>
        /// Obtiene las monedas
        /// </summary>
        /// <returns>Modelo Result</returns>
        public UTMO.Result<List<Currency>, UTMO.ErrorModel> GetCurrencies()
        {
            try
            {
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.Currency)));
                List<Currency> currencies = ModelAssembler.CreateCurrencies(businessCollection);
                return new UTMO.ResultValue<List<Currency>, UTMO.ErrorModel>(currencies);
            }
            catch (System.Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add("Error Creando");
                return new UTMO.ResultError<List<Currency>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }
    }
}
