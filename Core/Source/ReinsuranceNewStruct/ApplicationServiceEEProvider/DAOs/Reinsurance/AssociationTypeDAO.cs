using System.Collections.Generic;
//Sistran FWK
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
//Sistran Core
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance;
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Assemblers;
using REINSURANCEEN = Sistran.Core.Application.Reinsurance.Entities;
namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.DAOs.Reinsurance
{
    /// <summary>
    /// </summary>
    internal class AssociationTypeDAO
    {
        #region Instance variables

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region Get


        /// <summary>
        /// GetAssociationTypes
        /// </summary>
        /// <returns>List<AssociationType></returns>
        public List<LineAssociationType> GetAssociationTypes()
        {
            // Asignamos BusinessCollection a una Lista
            BusinessCollection businessCollection = new BusinessCollection
                (_dataFacadeManager.GetDataFacade().SelectObjects(typeof(REINSURANCEEN.LineAssociationType)));

            // Return como Lista
            return ModelAssembler.CreateAssociationTypes(businessCollection);
        }
        #endregion Get
    }
}