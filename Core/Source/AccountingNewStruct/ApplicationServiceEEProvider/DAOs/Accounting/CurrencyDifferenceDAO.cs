using System.Linq;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;

namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    class CurrencyDifferenceDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        ///<summary>
        /// SaveCurrencyDifference
        /// </summary>
        ///<param name="currencyCode"> </param>
        ///<param name="maxDifference"> </param>
        ///<param name="percentageDifference"> </param>
        ///<returns>int</returns>
        public int SaveCurrencyDifference(int currencyCode, decimal maxDifference, decimal percentageDifference)
        {
            try
            {
                ACCOUNTINGEN.CurrencyDifference currencyDifferenceEntity = EntityAssembler.CreateCurrencyDifference(
                    currencyCode, maxDifference, percentageDifference);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(currencyDifferenceEntity);

                // Return 
                return currencyCode;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        ///<summary>
        /// UpdateCurrencyDifference
        /// </summary>
        ///<param name="currencyCode"> </param>
        ///<param name="maxDifference"> </param>
        ///<param name="percentageDifference"> </param>
        ///<returns>int</returns>
        public int UpdateCurrencyDifference(int currencyCode, decimal maxDifference, decimal percentageDifference)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.CurrencyDifference.Properties.CurrencyCode, currencyCode);


                BusinessCollection businessCollection = new BusinessCollection(
                    _dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.CurrencyDifference), criteriaBuilder.GetPredicate()));

                foreach (ACCOUNTINGEN.CurrencyDifference currencyDifferenceEntity in businessCollection.OfType<ACCOUNTINGEN.CurrencyDifference>())
                {
                    currencyDifferenceEntity.MaximumDifference = maxDifference;
                    currencyDifferenceEntity.PercentageDifference = percentageDifference;
                    _dataFacadeManager.GetDataFacade().UpdateObject(currencyDifferenceEntity);
                }

                return currencyCode;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        ///<summary>
        /// DeleteCurrencyDifference
        /// </summary>
        ///<param name="currencyCode"> </param>
        ///<returns>bool</returns>
        public bool DeleteCurrencyDifference(int currencyCode)
        {
            bool isDeleted = false;

            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.CurrencyDifference.Properties.CurrencyCode, currencyCode);

                BusinessCollection businessCollection = new BusinessCollection(
                    _dataFacadeManager.GetDataFacade().SelectObjects(typeof(ACCOUNTINGEN.CurrencyDifference), criteriaBuilder.GetPredicate()));

                foreach (ACCOUNTINGEN.CurrencyDifference currencyDifferenceEntity in businessCollection.OfType<ACCOUNTINGEN.CurrencyDifference>())
                {
                    _dataFacadeManager.GetDataFacade().DeleteObject(currencyDifferenceEntity);
                }

                isDeleted = true;
            }
            catch (BusinessException)
            {
                isDeleted = false;
            }
            return isDeleted;
        }


    }
}
