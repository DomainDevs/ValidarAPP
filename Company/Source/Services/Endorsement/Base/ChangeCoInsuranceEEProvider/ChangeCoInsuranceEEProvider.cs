using Sistran.Company.Application.ChangeCoInsuranceEndorsement;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.ChangeCoInsuranceEndorsement.EEProvider.DAOs;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.ChangeCoInsuranceEndorsement.EEProvider
{
    public class CiaChangeCoinsuranceEndorsementEEProvider : ICiaChangeCoinsuranceEndorsement
    {
        /// <summary>
        /// Tarifar Cambio de intermediario de la Póliza
        /// </summary>
        /// <param name="policy">Póliza</param>        
        /// <returns>Riesgos</returns>
        public List<CompanyRisk> QuotateChangeCoinsuranceCia(CompanyPolicy policy)
        {
            try
            {
                ChangeCoInsuranceDAO changeCoInsuranceDAO = new ChangeCoInsuranceDAO();
                return changeCoInsuranceDAO.QuotateChangeCoinsuranceCia(policy);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

    }
}
