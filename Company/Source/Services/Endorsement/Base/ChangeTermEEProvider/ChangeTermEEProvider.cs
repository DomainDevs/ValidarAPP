using Sistran.Company.Application.ChangeTermEndorsement.EEProvider.DAOs;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.ChangeTermEndorsement.EEProvider;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Company.Application.ChangeTermEndorsement.EEProvider
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class CiaChangeTermEndorsementEEProvider : ChangeTermEndorsementEEProvider, IChangeTermEndorsementCompany
    {
        /// <summary>
        /// Tarifar Traslado de vigencia de la Póliza
        /// </summary>
        /// <param name="policy">Póliza</param>
        /// <returns>Riesgos</returns>
        public List<CompanyRisk> QuotateChangeTermCia(CompanyPolicy policy)
        {
            try
            {
                CiaChangeTermDAO changeTermDAO = new CiaChangeTermDAO();
                return changeTermDAO.QuotateChangeTermCia(policy);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
    }
}
