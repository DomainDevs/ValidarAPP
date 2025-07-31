using Sistran.Company.Application.ChangeAgentEndorsement.EEProvider.DAOs;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.ChangeAgentEndorsement.EEProvider;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Company.Application.ChangeAgentEndorsement.EEProvider
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class CiaChangeAgentEndorsementEEProvider : ChangeAgentEndorsementEEProvider, ICiaChangeAgentEndorsement
    {
          /// <summary>
        /// Tarifar Cambio de intermediario de la Póliza
        /// </summary>
        /// <param name="policy">Póliza</param>        
        /// <returns>Riesgos</returns>
        public List<CompanyRisk> QuotateChangeAgentCia(CompanyPolicy policy)
        {
            try
            {
                CiaChangeAgentDAO changeAgentDAO = new CiaChangeAgentDAO();
                return changeAgentDAO.QuotateChangeAgentCia(policy);
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
        public CompanyPolicy ExecuteCia(int Id)
        {
            try
            {
                CiaChangeAgentDAO changeAgentDAO = new CiaChangeAgentDAO();
                return changeAgentDAO.ExecuteCia(Id);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
    }
}
