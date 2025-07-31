//Sistran Core
using Sistran.Core.Application.AccountingServices.DTOs.Imputations;
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;

using System;
using Sistran.Core.Application.AccountingServices.DTOs.Search;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    class AgentCoinsuranceCheckingAccountDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        ///<summary>
        /// SaveAgentCoinsuranceCheckingAccount
        /// </summary>
        /// <param name="agentCoinsuranceCheckingAccount"></param>
        /// <returns>bool</returns>
        public bool SaveAgentCoinsuranceCheckingAccount(AgentCoinsuranceCheckingAccountDTO agentCoinsuranceCheckingAccount)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.AgentCoinsuranceCheckingAccount agentCoinsuranceCheckingAccountEntity =
                    EntityAssembler.CreateAgentCoinsuranceCheckingAccount(agentCoinsuranceCheckingAccount.CurrencyCode, agentCoinsuranceCheckingAccount.AgentTypeCode,
                    agentCoinsuranceCheckingAccount.AgentCode, agentCoinsuranceCheckingAccount.CommissionAmount, agentCoinsuranceCheckingAccount.IncomeCommissionAmount);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(agentCoinsuranceCheckingAccountEntity);

                return true;
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }
    }
}
