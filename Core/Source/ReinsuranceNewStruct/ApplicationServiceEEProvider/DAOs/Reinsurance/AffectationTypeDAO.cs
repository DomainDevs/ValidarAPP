using System.Collections.Generic;

//Sistran FWK
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;

//Sistran Core
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance;
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Assemblers;
using REINSEN = Sistran.Core.Application.Reinsurance.Entities;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.DAOs.Reinsurance
{

    internal class AffectationTypeDAO
    {
        #region Instance variables

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region Get
        /// <summary>
        /// GetAffectationTypes
        /// </summary>
        /// <returns>List<AffectationType></returns>
        public List<AffectationType> GetAffectationTypes()
        {
            // Asignamos BusinessCollection a una Lista
            BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(REINSEN.AffectationType)));
            // Return como Lista
            return ModelAssembler.CreateAffectationTypes(businessCollection);
        }
        #endregion Get
    }
}