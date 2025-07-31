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
    class AgentCommissionClosureDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        ///<summary>
        /// SaveAgentCommissionClosure
        /// </summary>
        /// <param name="agentCommissionClosure"></param>
        /// <returns>bool</returns>
        public bool SaveAgentCommissionClosure(AgentCommissionClosureDTO agentCommissionClosure)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.AgentCommissionClosure agentCommissionClosureEntity = EntityAssembler.CreateAgentCommissionClosure(agentCommissionClosure.UserId, agentCommissionClosure.StartDate,
                        agentCommissionClosure.EndDate, agentCommissionClosure.RegisterDate, agentCommissionClosure.Status);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(agentCommissionClosureEntity);

                // Return del model
                return true;
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }
    }
}
