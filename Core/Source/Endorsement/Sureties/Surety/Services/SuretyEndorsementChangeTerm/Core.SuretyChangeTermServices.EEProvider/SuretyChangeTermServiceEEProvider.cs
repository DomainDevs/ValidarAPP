using Sistran.Core.Application.ChangeTermEndorsement.EEProvider;
using Sistran.Core.Application.SuretyEndorsementChangeTermService.EEProvider.DAOs;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using System;
using System.Collections.Generic;


namespace Sistran.Core.Application.SuretyEndorsementChangeTermService.EEProvider
{
    public class SuretyChangeTermServiceEEProvider : ISuretyChangeTermService
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
                return DelegateService.changeTermEndorsementService.QuotateChangeTerm(policy);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Resources.Errors.ErrorTariffTransferOfValidityOfThePolicy), ex);
            }
        }


        /// <summary>
        /// Emitir Traslado de Vigencia de la Póliza       
        /// </summary>
        /// <param name="Id">Temporal</param>    
        /// <returns>Numero Endoso</returns>
        public Policy Execute(int Id)
        {
            try
            {
                ChangeTermDAO changeTermDAO = new ChangeTermDAO();
                return changeTermDAO.Execute(Id);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Resources.Errors.ErrorIssueTransferOfValidityOfThePolicy), ex);
            }
        }
    }
}
