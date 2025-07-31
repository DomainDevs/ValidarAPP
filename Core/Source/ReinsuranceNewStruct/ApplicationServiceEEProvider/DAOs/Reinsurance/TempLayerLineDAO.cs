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
    internal class TempLayerLineDAO
    {
        #region Instance variables

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        /// <summary>
        /// UpdateTempLayerLine
        /// </summary>
        /// <param name="reinsuranceLine"></param>
        public void UpdateTempLayerLine(ReinsuranceLine reinsuranceLine)
        {

            // Crea la Primary key con el código de la entidad
            PrimaryKey primaryKey = REINSURANCEEN.TempReinsuranceLine.CreatePrimaryKey(reinsuranceLine.ReinsuranceLineId);

            // Encuentra el objeto en referencia a la llave primaria
            REINSURANCEEN.TempReinsuranceLine entityTempReinsuranceLine = (REINSURANCEEN.TempReinsuranceLine)
                (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

            entityTempReinsuranceLine.LineCode = reinsuranceLine.Line.LineId;

            // Realizar las operaciones con los entities utilizando DAF
            _dataFacadeManager.GetDataFacade().UpdateObject(entityTempReinsuranceLine);

        }

        /// <summary>
        /// GetTempLayerLineById
        /// </summary>
        /// <param name="tmpIssueLayerId"></param>
        /// <returns>ReinsuranceLine</returns>
        public ReinsuranceLine GetTempLayerLineById(int tmpIssueLayerId)
        {

            PrimaryKey primaryKey = REINSURANCEEN.TempReinsuranceLine.CreatePrimaryKey(tmpIssueLayerId);

            // Realizar las operaciones con los entities utilizando DAF
            REINSURANCEEN.TempReinsuranceLine entityTempReinsuranceLine = (REINSURANCEEN.TempReinsuranceLine)
                (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

            // Return del model
            return ModelAssembler.CreateTempLayerLine(entityTempReinsuranceLine);
        }

    }
}