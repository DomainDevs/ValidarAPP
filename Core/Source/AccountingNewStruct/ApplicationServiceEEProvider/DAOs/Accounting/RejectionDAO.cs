
//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using Sistran.Core.Application.AccountingServices.EEProvider.Models;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
using System.Collections.Generic;

namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    class RejectionDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        /// <summary>
        /// GetRejections
        /// </summary>
        /// <returns>List<Rejection></returns>
        public List<Rejection> GetRejections()
        {
            try
            {
                // Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollection = new BusinessCollection(
                    _dataFacadeManager.GetDataFacade().SelectObjects(
                        typeof(ACCOUNTINGEN.Rejection)));

                // Return como lista
                return ModelAssembler.CreateRejections(businessCollection);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
    }
}
