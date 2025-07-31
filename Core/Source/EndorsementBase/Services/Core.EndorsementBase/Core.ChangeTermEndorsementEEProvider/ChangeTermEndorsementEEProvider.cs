using Sistran.Core.Application.BaseEndorsementService.EEProvider;
using Sistran.Core.Application.ChangeTermEndorsement.EEProvider.DAOs;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Core.Application.ChangeTermEndorsement.EEProvider
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class ChangeTermEndorsementEEProvider : BaseEndorsementServiceEEProvider, IChangeTermEndorsement
    {


        /// <summary>
        /// Tarifar Traslado de vigencia de la Póliza
        /// </summary>
        /// <param name="policy">Póliza</param>
        /// <returns>Riesgos</returns>
        public List<Risk> QuotateChangeTerm(Policy policy)
        {
            try
            {
                ChangeTermDAO changeTermDAO = new ChangeTermDAO();
                return changeTermDAO.QuotateChangeTerm(policy);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
    }
}
