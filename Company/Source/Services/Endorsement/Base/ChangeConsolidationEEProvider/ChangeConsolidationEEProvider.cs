using Sistran.Company.Application.ChangeConsolidationEndorsement;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.ChangeConsolidationEndorsement.EEProvider.DAOs;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.ChangeConsolidationEndorsement.EEProvider
{
    public class CiaChangeConsolidationEndorsementEEProvider : ICiaChangeConsolidationEndorsement
    {
        /// <summary>
        /// Tarifar Cambio de intermediario de la Póliza
        /// </summary>
        /// <param name="policy">Póliza</param>        
        /// <returns>Riesgos</returns>
        public List<CompanyRisk> QuotateChangeConsolidationCia(CompanyPolicy policy)
        {
            try
            {
                ChangeConsolidationDAO changeConsolidationDAO = new ChangeConsolidationDAO();
                return changeConsolidationDAO.QuotateChangeConsolidationCia(policy);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

    }
}
