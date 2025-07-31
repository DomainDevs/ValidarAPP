//Sistran Core
using Sistran.Core.Application.AutomaticDebitServices.EEProvider.Assemblers;

//Sitran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;

using System;
using System.Collections.Generic;

namespace Sistran.Core.Application.AutomaticDebitServices.EEProvider.DAOs
{
    public class AutomaticDebitStatusDAO
    {
        #region Instance Variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region Public Methods

        #region Get

        /// <summary>
        /// GetAutomaticDebitStatus
        /// </summary>
        /// <returns>List<Array></returns>
        public List<Array> GetAutomaticDebitStatus()
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.Property(Entities.AutomaticDebitStatus.Properties.AutomaticDebitStatusId);
                criteriaBuilder.GreaterEqual();
                criteriaBuilder.Constant(0);

                BusinessCollection automaticDebitStatusCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(Entities.AutomaticDebitStatus), criteriaBuilder.GetPredicate()));

                if (automaticDebitStatusCollection.Count > 0)
                {
                    // Return del model
                    return ModelAssembler.CreateAutomaticDebitStatus(automaticDebitStatusCollection);
                }

                return new List<Array>();
            }
            catch (BusinessException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion


        #endregion
    }
}
