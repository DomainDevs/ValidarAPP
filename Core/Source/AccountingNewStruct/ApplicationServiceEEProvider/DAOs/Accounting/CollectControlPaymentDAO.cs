//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Collect;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
   public class CollectControlPaymentDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        /// <summary>
        /// SaveCollectControlPayment
        /// </summary>
        /// <param name="collectControl"></param>
        /// <param name="registerNumber"></param>
        /// <returns>CollectControlPayment</returns>
        public CollectControlPayment SaveCollectControlPayment(CollectControl collectControl, int registerNumber)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.CollectControlPayment collectControlPaymentEntity = EntityAssembler.CreateCollectControlPayment(collectControl, registerNumber);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(collectControlPaymentEntity);

                // Return del model
                return ModelAssembler.CreateCollectControlPayment(collectControlPaymentEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
    }
}
