using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.Sureties.SuretyServices.EEProvider.DAOs;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using Sistran.Core.Application.Sureties.SuretyServices.Models;
namespace Sistran.Core.Application.Sureties.SuretyServices.EEProvider
{
    /// <summary>
    /// Polizas de Cumplimiento
    /// </summary>
    public class SuretyServiceEEProviderCore : SuretiesEEProvider.SuretiesEEProvider, ISuretyServiceCore
    {

        /// <summary>
        ///  Obtiene el Cupo Operativo y el Cumulo
        /// </summary>
        /// <param name="individualId">>Individual Id Asegurado</param>
        /// <param name="PrefixCd">Ramo del Negocio</param>
        /// <param name="issueDate">Fecha Hasta</param>
        /// <returns>Lista de sumas</returns>
        public List<Amount> GetAvailableAmountByIndividualId(int individualId, int PrefixCd, DateTime issueDate)
        {
            try
            {
                Aggregate aggregate = new Aggregate();
                return aggregate.GetAvailableAmountByIndividualId(individualId, PrefixCd, issueDate);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<Contract> GetSuretiesByEndorsementIdModuleType(int endorsementId, ModuleType moduleType)
        {
            try
            {
                ContactDAO contactDAO = new ContactDAO();
                return contactDAO.GetSuretiesByEndorsementIdModuleType(endorsementId, moduleType);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<Contract> GetRisksSuretyByInsuredId(int insuredId)
        {
            try
            {
                ContactDAO contactDAO = new ContactDAO();
                return contactDAO.GetRisksSuretyByInsuredId(insuredId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<Contract> GetRisksSuretyBySuretyId(int suretyId)
        {
            try
            {
                ContactDAO contactDAO = new ContactDAO();
                return contactDAO.GetRisksSuretyBySuretyId(suretyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public Contract GetSuretyByRiskIdModuleType(int riskId, ModuleType moduleType)
        {
            try
            {
                ContactDAO contactDAO = new ContactDAO();
                return contactDAO.GetSuretyByRiskIdModuleType(riskId, moduleType);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        public List<Contract> GetRisksBySurety(string description)
        {
            try
            {
                ContactDAO contactDAO = new ContactDAO();
                return contactDAO.GetRisksBySurety(description);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

    }
}
