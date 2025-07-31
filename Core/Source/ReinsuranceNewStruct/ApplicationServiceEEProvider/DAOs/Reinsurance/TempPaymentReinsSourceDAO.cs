//Sistran FWK
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using REINSURANCEEN = Sistran.Core.Application.Reinsurance.Entities;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.DAOs.Reinsurance
{
    internal class TempPaymentReinsSourceDAO
    {
        #region Instance variables

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region Update

        /// <summary>
        /// UpdateTempPaymentReinsSource
        /// </summary>
        /// <param name="tempPaymentReinsSourceId"></param>
        /// <param name="amount"></param>
        public void UpdateTempPaymentReinsSource(int tempPaymentReinsSourceId, decimal amount)
        {
            // Crea la Primary key con el código de la entidad
            PrimaryKey primaryKey = REINSURANCEEN.TempPaymentReinsSource.CreatePrimaryKey(tempPaymentReinsSourceId);

            // Encuentra el objeto en referencia a la llave primaria
            REINSURANCEEN.TempPaymentReinsSource entityTempPaymentReinsSource = (REINSURANCEEN.TempPaymentReinsSource)
                (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));
            entityTempPaymentReinsSource.Amount = amount;

            // Realizar las operaciones con los entities utilizando DAF
            _dataFacadeManager.GetDataFacade().UpdateObject(entityTempPaymentReinsSource);
        }

        #endregion Update
    }
}
