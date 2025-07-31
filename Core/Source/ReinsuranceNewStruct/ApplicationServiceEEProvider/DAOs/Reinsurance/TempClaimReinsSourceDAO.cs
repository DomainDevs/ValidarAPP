//Sistran FWK
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using REINSURANCEEN = Sistran.Core.Application.Reinsurance.Entities;
namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.DAOs.Reinsurance
{
    internal class TempClaimReinsSourceDAO
    {
        #region Instance variables

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region Update
        public void UpdateTempClaimReinsSource(int tempClaimReinsSourceId, decimal newAmount)
        {
            // Crea la Primary key con el código de la entidad
            PrimaryKey primaryKey = REINSURANCEEN.TempClaimReinsSource.CreatePrimaryKey(tempClaimReinsSourceId);
            REINSURANCEEN.TempClaimReinsSource entityTempClaimReinsSource = (REINSURANCEEN.TempClaimReinsSource) 
            _dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey);
            entityTempClaimReinsSource.Amount = newAmount;
            _dataFacadeManager.GetDataFacade().UpdateObject(entityTempClaimReinsSource);
        }
        #endregion Update
    }
}
