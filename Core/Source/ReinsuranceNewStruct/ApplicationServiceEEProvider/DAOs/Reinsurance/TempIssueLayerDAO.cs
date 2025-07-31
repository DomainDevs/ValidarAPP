using System;
//Sistran FWK
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
//Sistran Core
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Assemblers;
using REINSURANCEEN = Sistran.Core.Application.Reinsurance.Entities;
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance;
using Sistran.Core.Framework.DAF;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.DAOs.Reinsurance
{
    /// <summary>
    /// </summary>
    internal class TempIssueLayerDAO
    {
        #region Instance variables

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region Save

        /// <summary>
        /// SaveTempIssueLayer 
        /// </summary>
        /// <param name="reinsuranceLayerIssuance"></param>
        public void SaveTempIssueLayer(ReinsuranceLayerIssuance reinsuranceLayerIssuance)
        {

            // Convertir de model a entity
            REINSURANCEEN.TempReinsLayerIssuance entityTempReinsLayerIssuance = EntityAssembler.CreateTempIssueLayer(reinsuranceLayerIssuance);

            // Realizar las operaciones con los entities utilizando DAF
            _dataFacadeManager.GetDataFacade().InsertObject(entityTempReinsLayerIssuance);
        }

        #endregion Save

        #region Update

        /// <summary>
        /// UpdateTempIssueLayer
        /// </summary>
        /// <param name="reinsuranceLayerIssuance"></param>

        public void UpdateTempIssueLayer(ReinsuranceLayerIssuance reinsuranceLayerIssuance)
        {
            // Crea la Primary key con el código de la entidad
            PrimaryKey primaryKey = REINSURANCEEN.TempReinsLayerIssuance.CreatePrimaryKey(reinsuranceLayerIssuance.ReinsuranceLayerId);

            // Encuentra el objeto en referencia a la llave primaria
            REINSURANCEEN.TempReinsLayerIssuance entityTempReinsLayerIssuance = (REINSURANCEEN.TempReinsLayerIssuance)
                (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

            entityTempReinsLayerIssuance.LayerPercentage = Convert.ToDecimal(reinsuranceLayerIssuance.LayerPercentage);
            entityTempReinsLayerIssuance.PremiumPercentage = Convert.ToDecimal(reinsuranceLayerIssuance.PremiumPercentage);

            // Realizar las operaciones con los entities utilizando DAF
            _dataFacadeManager.GetDataFacade().UpdateObject(entityTempReinsLayerIssuance);
        }

        #endregion Update

        #region Delete

        /// <summary>
        /// DeleteTempIssueLayer
        /// </summary>
        /// <param name="tempIssueLayerId"></param>
        public void DeleteTempIssueLayer(int tempIssueLayerId)
        {
            // Crea la Primary key con el código de la entidad
            PrimaryKey primaryKey = REINSURANCEEN.TempReinsLayerIssuance.CreatePrimaryKey(tempIssueLayerId);

            // Encuentra el objeto en referencia a la llave primaria
            REINSURANCEEN.TempReinsLayerIssuance entityTempReinsLayerIssuance = (REINSURANCEEN.TempReinsLayerIssuance)
                (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

            // Realizar las operaciones con los entities utilizando DAF
            _dataFacadeManager.GetDataFacade().DeleteObject(entityTempReinsLayerIssuance);
        }

        #endregion Delete

        #region Get


        /// <summary>
        /// GetTempIssueLayerById
        /// </summary>
        /// <param name="tempReinsLayerIssuanceId"></param>
        /// <returns></returns>
        public ReinsuranceLayerIssuance GetTempIssueLayerById(int tempReinsLayerIssuanceId)
        {

            PrimaryKey primaryKey = REINSURANCEEN.TempReinsLayerIssuance.CreatePrimaryKey(tempReinsLayerIssuanceId);

            // Realizar las operaciones con los entities utilizando DAF
            REINSURANCEEN.TempReinsLayerIssuance entityTempReinsLayerIssuance = (REINSURANCEEN.TempReinsLayerIssuance)
                (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

            // Return del model
            return ModelAssembler.CreateTempIssueLayer(entityTempReinsLayerIssuance);
        }
        #endregion Get
    }
}