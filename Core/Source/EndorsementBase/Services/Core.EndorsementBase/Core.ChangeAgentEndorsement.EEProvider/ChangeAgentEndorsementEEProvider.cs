using Sistran.Core.Application.BaseEndorsementService.EEProvider;
using Sistran.Core.Application.ChangeAgentEndorsement.EEProvider.DAOs;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Core.Application.ChangeAgentEndorsement.EEProvider
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class ChangeAgentEndorsementEEProvider : BaseEndorsementServiceEEProvider, IChangeAgentEndorsement
    {
        internal static IDataFacadeManager dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        /// <summary>
        /// Tarifar Cambio de intermediario de la Póliza
        /// </summary>
        /// <param name="policy">Póliza</param>        
        /// <returns>Riesgos</returns>
        public List<Risk> QuotateChangeAgent(Policy policy)
        {
            try
            {
                ChangeAgentDAO changeAgentDAO = new ChangeAgentDAO();
                return changeAgentDAO.QuotateChangeAgent(policy);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Emitir Cambio de intermediario de la Póliza       
        /// </summary>
        /// <param name="Id">Temporal</param>    
        /// <returns>Numero Endoso</returns>
        public Policy Execute(int Id)
        {
            try
            {
                ChangeAgentDAO changeAgentDAO = new ChangeAgentDAO();
                return changeAgentDAO.Execute(Id);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
    }
}
