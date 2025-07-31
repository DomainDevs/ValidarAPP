using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.CancellationEndorsement.EEProvider.DAOs;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Company.Application.CancellationEndorsement.EEProvider
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class CiaCancellationEndorsementEEProvider : ICiaCancellationEndorsement
    {


        /// <summary>
        /// Tarifar Cancelación de Póliza
        /// </summary>
        /// <param name="policy">Póliza</param>
        /// <param name="cancellationFactor">Factor de Cancelación</param>
        /// <returns>Riesgos</returns>
        public List<CompanyRisk> QuotateCiaCancellation(CompanyPolicy policy, int cancellationFactor)
        {
            try
            {
                CiaCancellationDAO cancellationDAO = new CiaCancellationDAO();
                return cancellationDAO.QuotateCancellation(policy, cancellationFactor);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
    }
}
