using Core.CancellationEndorsement3GProvider.DAOs;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Core.Application.CancellationEndorsement.EEProvider
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class CancellationEndorsementEEProvider : ICancellationEndorsement
    {


        /// <summary>
        /// Tarifar Cancelación de Póliza
        /// </summary>
        /// <param name="policy">Póliza</param>
        /// <param name="cancellationFactor">Factor de Cancelación</param>
        /// <returns>Riesgos</returns>
        public List<Risk> QuotateCancellation(Policy policy, int cancellationFactor)
        {
            try
            {
                CancellationDAO cancellationDAO = new CancellationDAO();
                return cancellationDAO.QuotateCancellation(policy, cancellationFactor);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
    }
}
