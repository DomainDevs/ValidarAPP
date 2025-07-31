using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.ClauseEndorsement.EEProvider.DAOs;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Company.Application.ClauseEndorsement.EEProvider
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class CiaClauseEndorsementEEProvider : ICiaClauseEndorsement
    {

        /// <summary>
        /// Creates the cia texts.
        /// </summary>
        /// <param name="endorsement">The endorsement.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public CompanyPolicy CreateCiaClause(CompanyPolicy endorsement)
        {
            try
            {
                CiaClauseDAO ciaClauseDAO = new CiaClauseDAO();
                return ciaClauseDAO.CreateCiaClause(endorsement);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
    }
}
