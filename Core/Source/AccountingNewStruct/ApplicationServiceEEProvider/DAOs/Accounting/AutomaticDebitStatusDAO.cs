//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;

using System;
using System.Collections.Generic;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
   public class AutomaticDebitStatusDAO
    {
        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        /// <summary>
        /// GetAutomaticDebitStatus
        /// </summary>
        /// <returns>List<Array/></returns>
        public List<Array> GetAutomaticDebitStatus()
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.Property(ACCOUNTINGEN.AutomaticDebitStatus.Properties.AutomaticDebitStatusId);
                criteriaBuilder.GreaterEqual();
                criteriaBuilder.Constant(0);

                BusinessCollection automaticDebitStatusCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(ACCOUNTINGEN.AutomaticDebitStatus), criteriaBuilder.GetPredicate()));

                if (automaticDebitStatusCollection.Count > 0)
                {
                    // Return del model
                    return ModelAssembler.CreateAutomaticDebitStatus(automaticDebitStatusCollection);
                }

                return new List<Array>();
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }
    }
}
