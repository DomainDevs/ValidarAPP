//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
using System.Collections.Generic;

namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{

    public class PaymentLogDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        /// <summary>
        /// SavePaymentLog
        /// Graba los items necesarios para hacer un Log de la tabla Payment
        /// </summary>
        /// <param name="itemParam"></param>
        /// <returns></returns>
        public void SavePaymentLog(Dictionary<string, string> itemParam)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.PaymentLog paymentLogEntity = EntityAssembler.CreatePaymentLog(itemParam);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(paymentLogEntity);
            }
            catch (System.ArgumentException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }


    }
}
