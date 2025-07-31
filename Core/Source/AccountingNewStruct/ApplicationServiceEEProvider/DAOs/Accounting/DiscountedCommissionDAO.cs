using System;
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
using SEARCH = Sistran.Core.Application.AccountingServices.DTOs.Search;

namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    public class DiscountedCommissionDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        ///<summary>
        /// SaveDiscountedCommission
        /// </summary>
        /// <param name="discountedCommission"></param>
        /// <returns>bool</returns>
        public bool SaveDiscountedCommission(SEARCH.DiscountedCommissionDTO discountedCommission)
        {
            try
            {
                // Convertir de model a entity
                ACCOUNTINGEN.DiscountedCommission discountedCommissionEntity = EntityAssembler.CreateDiscountedCommission(discountedCommission);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(discountedCommissionEntity);

                return true;
            }
            catch (BusinessException)
            {
                return false;
            }
        }

        /// <summary>
        /// GetDiscountedCommission
        /// </summary>
        /// <param name="discountedCommissionId"></param>
        /// <returns>DiscountedCommission</returns>
        public SEARCH.DiscountedCommissionDTO GetDiscountedCommission(int discountedCommissionId)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.DiscountedCommission.Properties.DiscountedCommissionCode,
                    discountedCommissionId);

                BusinessCollection businessCollection = new BusinessCollection
                    (_dataFacadeManager.GetDataFacade().SelectObjects(
                    typeof(ACCOUNTINGEN.DiscountedCommission), criteriaBuilder.GetPredicate()));

                SEARCH.DiscountedCommissionDTO discountedCommission = new SEARCH.DiscountedCommissionDTO();

                // Asignamos BusinessCollection a una lista
                foreach (ACCOUNTINGEN.DiscountedCommission discountedCommissionEntity in businessCollection.OfType<ACCOUNTINGEN.DiscountedCommission>())
                {
                    discountedCommission.DiscountedCommissionId = discountedCommissionEntity.DiscountedCommissionCode;
                    discountedCommission.ApplicationPremiumId = Convert.ToInt32(discountedCommissionEntity.PremiumReceivableTransCode);
                    discountedCommission.AgentTypeCode = Convert.ToInt32(discountedCommissionEntity.AgentTypeCode);
                    discountedCommission.AgentIndividualId = Convert.ToInt32(discountedCommissionEntity.AgentIndividualId);
                    discountedCommission.CurrencyId = Convert.ToInt32(discountedCommissionEntity.CurrencyCode);
                    discountedCommission.ExchangeRate = Convert.ToDecimal(discountedCommissionEntity.ExchangeRate);
                    discountedCommission.BaseIncomeAmount = Convert.ToDecimal(discountedCommissionEntity.BaseIncomeAmount);
                    discountedCommission.BaseAmount = Convert.ToDecimal(discountedCommissionEntity.BaseAmount);
                    discountedCommission.CommissionPercentage = Convert.ToDecimal(discountedCommissionEntity.CommissionPercentage);
                    discountedCommission.CommissionType = Convert.ToInt32(discountedCommissionEntity.CommissionType);
                    discountedCommission.CommissionDiscountIncomeAmount = Convert.ToDecimal(discountedCommissionEntity.CommissionDiscountIncomeAmount);
                    discountedCommission.CommissionDiscountAmount = Convert.ToDecimal(discountedCommissionEntity.CommissionDiscountAmount);
                }

                return discountedCommission;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

    }
}

