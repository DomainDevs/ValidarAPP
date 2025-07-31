using System;
using System.Collections.Generic;
using Sistran.Core.Application.Finances.EEProvider;
using Sistran.Core.Application.Finances.FidelirtyServices.EEProvider.DAO;
using Sistran.Core.Application.Finances.FidelityServices.Models;
using Sistran.Core.Framework.BAF;

namespace Sistran.Core.Application.Finances.FidelityServices.EEProvider
{
    /// <summary>
    /// FidelityServicesEEProvider
    /// </summary>
    public class FidelityServicesEEProviderCore : FinancesEEProviderCore, IFidelityServiceCore
    {
        public List<FidelityRisk> GetRiskFidelitiesByEndorsementId(int endorsementId)
        {
            try
            {
                FidelityDAO fidelityDAO = new FidelityDAO();
                return fidelityDAO.GetRiskFidelitiesByEndorsementId(endorsementId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<FidelityRisk> GetRiskFidelitiesByInsuredId(int insuredId)
        {
            try
            {
                FidelityDAO fidelityDAO = new FidelityDAO();
                return fidelityDAO.GetRiskFidelitiesByInsuredId(insuredId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public FidelityRisk GetRiskFidelityByRiskId(int riskId)
        {
            try
            {
                FidelityDAO fidelityDAO = new FidelityDAO();
                return fidelityDAO.GetRiskFidelityByRiskId(riskId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
    }
}
