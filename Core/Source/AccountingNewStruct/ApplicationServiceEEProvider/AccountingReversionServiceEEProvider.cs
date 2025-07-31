using Sistran.Core.Application.AccountingServices.DTOs;
using Sistran.Core.Application.AccountingServices.EEProvider.DAOs.Accounting;
using Sistran.Core.Framework.BAF;
using System;

namespace Sistran.Core.Application.AccountingServices.EEProvider
{
    public class AccountingReversionServiceEEProvider: IAccountingReversionService
    {
        /// <summary>
        /// Reversion primas Aplicadas 
        /// </summary>
        /// <param name="reversionFilterDTO">The reversion filter dto.</param>
        /// <returns></returns>
        public bool ReversionPremiumByFilterApp(ReversionFilterDTO reversionFilterDTO)
        {
            try
            {
                return ReversionDAO.ReversionPremium(reversionFilterDTO);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        /// <summary>
        /// Reversions the temporary premium.
        /// </summary>
        /// <param name="reversionFilterDTO">The reversion filter dto.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        /// <exception cref="Exception"></exception>
        public bool ReversionTempPremium(ReversionFilterDTO reversionFilterDTO)
        {
            try
            {
                return ReversionDAO.ReversionTempPremium(reversionFilterDTO);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
