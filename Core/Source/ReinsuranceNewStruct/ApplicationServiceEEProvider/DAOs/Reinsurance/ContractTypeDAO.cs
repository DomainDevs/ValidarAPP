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
    /// <summary>
    /// 
    /// </summary>
    internal class ContractTypeDAO
    {
        #region Instance variables

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region Get

        /// <summary>
        /// GetContractTypes
        /// </summary>
        /// <returns>List<ContractType></returns>
        public List<ContractType> GetContractTypes()
        {
            // Asignar BusinessCollection a una Lista
            BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(REINSURANCEEN.ContractType)));
            // Retornar el model como Lista
            return ModelAssembler.CreateContractTypes(businessCollection); 
        }

        #endregion Get
    }
}
