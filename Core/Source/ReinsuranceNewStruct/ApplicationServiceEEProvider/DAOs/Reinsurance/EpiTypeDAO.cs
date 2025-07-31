using System.Collections.Generic;
//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
//Sistran Core
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Assemblers;
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance;
using REINSURANCEEN = Sistran.Core.Application.Reinsurance.Entities;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.DAOs.Reinsurance
{

    internal class EpiTypeDAO
    {
        #region Instance variables

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region Get
        /// <summary>
        /// GetEPITypes
        /// </summary>
        /// <returns>List<EPIType></returns>
        public List<EPIType> GetEPITypes()
        {
            // Asignamos BusinessCollection a una Lista
            BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(REINSURANCEEN.EpiType)));
            // Return como Lista
            return ModelAssembler.CreateEpiTypes(businessCollection);
        }

        #endregion Get
    }
}