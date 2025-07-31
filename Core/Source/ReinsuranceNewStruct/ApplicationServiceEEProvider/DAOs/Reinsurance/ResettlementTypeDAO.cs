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

    internal class ResettlementTypeDAO
    {
        #region Instance variables

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;
        #endregion

        #region Get
        /// <summary>
        /// GetResettlementTypes
        /// </summary>
        /// <returns>List<ResettlementType></returns>
        public List<ResettlementType> GetResettlementTypes()
        {
            // Asignamos BusinessCollection a una Lista
            BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(REINSURANCEEN.ResettlementType)));
            // Return como Lista
            return ModelAssembler.CreateReestablishmentTypes(businessCollection);
        }
        #endregion Get
    }
}