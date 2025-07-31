//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
using System;


namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    public class LogBankResponseDAO
    {
        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        /// <summary>
        /// SaveLogBankResponse
        /// Guarda un nuevo rejistro en la tabla.
        /// </summary>
        /// <param name="logBankResponse"></param>
        /// <returns>Array</returns>
        public Array SaveLogBankResponse(Array logBankResponse)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.LogBankResponse logBankResponseEntity = EntityAssembler.CreateLogBankResponse(logBankResponse);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(logBankResponseEntity);

                return ModelAssembler.CreateLogBankResponse(logBankResponseEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

    }
}
