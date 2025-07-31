using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.TextEndorsement.EEProvider.DAOs;
using Sistran.Core.Framework.BAF;
using System;
using System.ServiceModel;

namespace Sistran.Company.Application.TextEndorsement.EEProvider
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class CiaTextEndorsementEEProvider : ICiaTextEndorsement
    {

        /// <summary>
        /// Creates the cia texts.
        /// </summary>
        /// <param name="endorsement">The endorsement.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public CompanyPolicy CreateCiaTexts(CompanyEndorsement endorsement)
        {
            try
            {
                CiaTextDAO ciaTextDAO = new CiaTextDAO();
                return ciaTextDAO.CreateCiaTexts(endorsement);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
    }
}
