using Sistran.Company.Application.ChangePolicyHolderEndorsement;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.ChangePolicyHolderEndorsement.EEProvider.DAOs;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.ChangePolicyHolderEndorsement.EEProvider
{
    public class CiaChangePolicyHolderEndorsementEEProvider : ICiaChangePolicyHolderEndorsement
    {
        /// <summary>
        /// Tarifar Cambio de intermediario de la Póliza
        /// </summary>
        /// <param name="policy">Póliza</param>        
        /// <returns>Riesgos</returns>
        public List<CompanyRisk> QuotateChangePolicyHolderCia(CompanyPolicy policy)
        {
            try
            {
                ChangePolicyHolderDAO changePolicyHolderDAO = new ChangePolicyHolderDAO();
                return changePolicyHolderDAO.QuotateChangePolicyHolderCia(policy);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

    }
}
