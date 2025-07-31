//Sistran Core
using Sistran.Core.Application.AccountingServices.DTOs.Imputations;
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;

//Sistran FWK
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Application.AccountingServices.DTOs.Search;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    class AgentCommissionBalanceItemDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        ///<summary>
        /// SaveAgentCommissionBalanceItem
        /// </summary>
        /// <param name="agentCommissionBalanceItem"></param>
        /// <returns>int</returns>
        public int SaveAgentCommissionBalanceItem(AgentCommissionBalanceItemDTO agentCommissionBalanceItem)
        {
            // Convertir de model a entity
            ACCOUNTINGEN.AgentCommissionBalanceItem agentCommissionBalanceitemEntity = EntityAssembler.CreateAgentCommissionBalanceItem(agentCommissionBalanceItem);

            // Realizar las operaciones con los entities utilizando DAF
            _dataFacadeManager.GetDataFacade().InsertObject(agentCommissionBalanceitemEntity);

            return agentCommissionBalanceitemEntity.AgentCommissionBalanceItemCode;
        }

    }
}
