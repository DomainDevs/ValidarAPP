using System;
using System.Collections.Generic;
//Sistran FWK
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
//Sistran Core
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Assemblers;
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance;
using REINSURANCEEN = Sistran.Core.Application.Reinsurance.Entities;
namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.DAOs.Reinsurance
{

    internal class LineBusinessReinsuranceDAO
    {
        #region Instance variables

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region Save

        /// <summary>
        /// SaveLineBusinessReinsurance
        /// </summary>
        /// <param name="reinsuranceprefix"></param>
        /// <returns>ReinsurancePrefix</returns>
        public ReinsurancePrefix SaveLineBusinessReinsurance(ReinsurancePrefix reinsuranceprefix)
        {
            // Convertir de model a entity
            REINSURANCEEN.ReinsurancePrefix entityReinsurancePrefix = EntityAssembler.CreateLineBusinessReinsurance(reinsuranceprefix);

            // Realizar las operaciones con los entities utilizando DAF
            _dataFacadeManager.GetDataFacade().InsertObject(entityReinsurancePrefix);

            return ModelAssembler.CreateLineBusinessReinsurance(entityReinsurancePrefix);
        }

        #endregion Save

        #region Update

        /// <summary>
        /// UpdateLineBusinessReinsurance
        /// </summary>
        /// <param name="reinsuranceprefix"></param>
        /// <returns>ReinsurancePrefix</returns>
        public ReinsurancePrefix UpdateLineBusinessReinsurance(ReinsurancePrefix reinsuranceprefix)
        {
            // Crea la Primary key con el código de la entidad
            PrimaryKey primaryKey = REINSURANCEEN.ReinsurancePrefix.CreatePrimaryKey(reinsuranceprefix.Id);

            // Encuentra el objeto en referencia a la llave primaria
            REINSURANCEEN.ReinsurancePrefix entityReinsurancePrefix = (REINSURANCEEN.ReinsurancePrefix)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));
            entityReinsurancePrefix.LineBusinessCode = reinsuranceprefix.Prefix.Id;
            entityReinsurancePrefix.LineBusinessCumulusCode = reinsuranceprefix.PrefixCumulus.Id;
            entityReinsurancePrefix.Location = Convert.ToBoolean(reinsuranceprefix.IsLocation);
            entityReinsurancePrefix.TypeExerciceCode = Convert.ToInt16(reinsuranceprefix.ExerciseType);

            // Realizar las operaciones con los entities utilizando DAF
            _dataFacadeManager.GetDataFacade().UpdateObject(entityReinsurancePrefix);

            return ModelAssembler.CreateLineBusinessReinsurance(entityReinsurancePrefix);
        }

        #endregion Update

        #region Delete

        /// <summary>
        /// DeleteLineBusinessReinsurance
        /// </summary>
        /// <param name="linebusinessreinsuranceId"></param>
        /// <returns>bool</returns>
        public bool DeleteLineBusinessReinsurance(int linebusinessreinsuranceId)
        {
            // Crea la Primary key con el código de la entidad
            PrimaryKey primaryKey = REINSURANCEEN.ReinsurancePrefix.CreatePrimaryKey(linebusinessreinsuranceId);

            // Realizar las operaciones con los entities utilizando DAF
            REINSURANCEEN.ReinsurancePrefix entityReinsurancePrefix = (REINSURANCEEN.ReinsurancePrefix)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

            // Realizar las operaciones con los entities utilizando DAF
            _dataFacadeManager.GetDataFacade().DeleteObject(entityReinsurancePrefix);

            return true;

        }

        #endregion Delete

        #region Get

        /// <summary>
        /// GetLineBusinessReinsurances
        /// </summary>
        /// <returns>List<ReinsurancePrefix></returns>
        public List<ReinsurancePrefix> GetLineBusinessReinsurances()
        {

            // Asignar BusinessCollection a una Lista
            BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(REINSURANCEEN.ReinsurancePrefix)));
            // Retornar el model
            return ModelAssembler.CreateLinesBusinessReinsurance(businessCollection);
        }
        #endregion Get
    }
}