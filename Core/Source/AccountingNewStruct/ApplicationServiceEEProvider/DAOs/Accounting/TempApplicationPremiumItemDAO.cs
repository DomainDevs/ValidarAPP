//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using Sistran.Core.Application.AccountingServices.EEProvider.DAOs.Accounting.Reversion;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations;
using Sistran.Core.Application.Utilities.DataFacade;
//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.Views;
using System;
using System.Collections.Generic;
using System.Data;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
using SEARCH = Sistran.Core.Application.AccountingServices.DTOs.Search;
using System.Linq;
namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs.Accounting
{
    class TempApplicationPremiumItemDAO
    {
        ///<summary>
        /// SaveTempPremiumRecievableTransactionItem
        /// </summary>
        /// <param name="premiumRecievableTransactionItem"></param>
        /// <param name="imputationId"></param>
        /// <param name="exchangeRate"></param>
        /// <param name="userId"></param>
        /// <param name="registerDate"></param>
        /// <returns>PremiumReceivableTransactionItem</returns>
        public PremiumReceivableTransactionItem SaveTempPremiumRecievableTransactionItem(PremiumReceivableTransactionItem premiumRecievableTransactionItem, int imputationId, decimal exchangeRate, int userId, DateTime registerDate, DateTime accountingDate)
        {
            try
            {
                ACCOUNTINGEN.TempApplicationPremium entityTempApplicationPremium = EntityAssembler.CreateTempApplicationPremium(premiumRecievableTransactionItem, imputationId, exchangeRate, userId, registerDate, accountingDate);
                DataFacadeManager.Insert(entityTempApplicationPremium);
                return ModelAssembler.CreateTempPremiumReceivableTransactionItem(entityTempApplicationPremium);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public PremiumReceivableTransactionItem SaveTempPremiumRecievableTransactionItem(TempApplicationPremium tempApplicationPremium)
        {
            try
            {
                ACCOUNTINGEN.TempApplicationPremium entityTempApplicationPremium = EntityAssembler.CreateTempApplicationPremium(tempApplicationPremium);
                DataFacadeManager.Insert(entityTempApplicationPremium);
                return ModelAssembler.CreateTempPremiumReceivableTransactionItem(entityTempApplicationPremium);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public ApplicationPremium SaveTempApplicationPremium(TempApplicationPremium tempApplicationPremium)
        {
            try
            {
                ACCOUNTINGEN.TempApplicationPremium entityTempApplicationPremium = EntityAssembler.CreateTempApplicationPremium(tempApplicationPremium);
                DataFacadeManager.Insert(entityTempApplicationPremium);
                return ModelAssembler.CreateTempApplicationPremium(entityTempApplicationPremium);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        ///<summary>
        /// UpdateTempPremiumReceivable
        /// </summary>
        /// <param name="premiumReceivableTransactionItem"></param>
        /// <param name="imputationId"></param>
        /// <param name="exchangeRate"></param>
        /// <param name="userId"></param>
        /// <param name="registerDate"></param>
        /// <returns>PremiumReceivableTransactionItem</returns>
        public PremiumReceivableTransactionItem UpdateTempPremiumReceivableTransactionItem(PremiumReceivableTransactionItem premiumReceivableTransactionItem, int imputationId, decimal exchangeRate, int userId, DateTime registerDate, DateTime accountingDate)
        {
            try
            {
                int? statusQuota = 0;

                if (premiumReceivableTransactionItem.Policy.PayerComponents[0].BaseAmount < premiumReceivableTransactionItem.Policy.PayerComponents[0].Amount)
                {
                    statusQuota = AccountingApplicationServiceEEProvider.StatusQuotaPartial; //PARCIAL
                }
                if (premiumReceivableTransactionItem.Policy.PayerComponents[0].Amount >= premiumReceivableTransactionItem.Policy.PayerComponents[0].BaseAmount)
                {
                    statusQuota = AccountingApplicationServiceEEProvider.StatusQuotaTotal; //TOTAL
                }

                // Se Crea el Primary key con el código de la entidad
                PrimaryKey primaryKey = ACCOUNTINGEN.TempApplicationPremium.CreatePrimaryKey(premiumReceivableTransactionItem.Id);

                // Encuentra el objeto en referencia a la llave primaria
                ACCOUNTINGEN.TempApplicationPremium entityTempApplicationPremium = (ACCOUNTINGEN.TempApplicationPremium)
                    DataFacadeManager.GetObject(primaryKey);

                entityTempApplicationPremium.TempAppCode = imputationId;
                entityTempApplicationPremium.EndorsementCode = premiumReceivableTransactionItem.Policy.Endorsement.Id;
                entityTempApplicationPremium.PaymentNum = premiumReceivableTransactionItem.Policy.PaymentPlan.Quotas[0].Number;
                entityTempApplicationPremium.MainAmount = premiumReceivableTransactionItem.Policy.PayerComponents[0].Amount;
                entityTempApplicationPremium.PayerCode = premiumReceivableTransactionItem.Policy.DefaultBeneficiaries[0].IndividualId;
                entityTempApplicationPremium.LocalAmount = premiumReceivableTransactionItem.Policy.PayerComponents[0].BaseAmount;
                entityTempApplicationPremium.CurrencyCode = premiumReceivableTransactionItem.Policy.ExchangeRate.Currency.Id;
                entityTempApplicationPremium.ExchangeRate = exchangeRate;
                entityTempApplicationPremium.Amount = premiumReceivableTransactionItem.Policy.PayerComponents[0].BaseAmount * exchangeRate;
                entityTempApplicationPremium.RegisterDate = registerDate;
                entityTempApplicationPremium.DiscountedCommission = premiumReceivableTransactionItem.DeductCommission.Value;
                entityTempApplicationPremium.PremiumQuotaStatusCode = statusQuota;
                entityTempApplicationPremium.AccountingDate = accountingDate;

                DataFacadeManager.Update(entityTempApplicationPremium);

                // Return del model
                return ModelAssembler.CreateTempPremiumReceivableTransactionItem(entityTempApplicationPremium);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteTempPremiumRecievableTransactionItem
        /// </summary>
        /// <param name="tempTransactionItemId"></param>
        /// <returns>bool</returns>
        public bool DeleteTempPremiumRecievableTransactionItem(int tempTransactionItemId)
        {
            try
            {
                PrimaryKey primaryKey = ACCOUNTINGEN.TempApplicationPremium.CreatePrimaryKey(tempTransactionItemId);
                DataFacadeManager.Delete(primaryKey);
                return true;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public static bool DeleteTempPremiumReversionTransactionItem(int tempTransactionItemId)
        {
            ObjectCriteriaBuilder criteriaBuilder;
            List<int> temps = TempApplicationPremiumReversionDAO.GetTempAppPremiumRevByTempAppId(tempTransactionItemId);
            foreach (int temp in temps)
            {
                criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempApplicationPremiumComRev.Properties.TempAppPremiumRevId, temp);
                DataFacadeManager.Instance.GetDataFacade().Delete<ACCOUNTINGEN.TempApplicationPremiumComRev>(criteriaBuilder.GetPredicate());
            }
            criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempApplicationPremiumRev.Properties.TempAppId, tempTransactionItemId);
            DataFacadeManager.Instance.GetDataFacade().Delete<ACCOUNTINGEN.TempApplicationPremiumRev>(criteriaBuilder.GetPredicate());

            return true;
        }
        public static bool DeleteTempPremiumReversionItem(int tempAppPremiumId)
        {
            PrimaryKey primaryKey = ACCOUNTINGEN.TempApplicationPremiumRev.CreatePrimaryKey(tempAppPremiumId);
            DataFacadeManager.Delete(primaryKey);
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempApplicationPremiumComRev.Properties.TempAppPremiumRevId, tempAppPremiumId);
            return Convert.ToBoolean(DataFacadeManager.Instance.GetDataFacade().Delete<ACCOUNTINGEN.TempApplicationPremiumComRev>(criteriaBuilder.GetPredicate()));
        }
        #region obtener Datos
        public static List<SEARCH.PremiumReceivableItemDTO> GetTempApplicationPremiumByApplicationId(int tempImputationId)
        {

            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.GetAppTempPremiumReceivableItem.Properties.TempAppCode, tempImputationId);

            UIView tempPremiumReceivableItems = DataFacadeManager.Instance.GetDataFacade().GetView("GetAppTempPremiumReceivableItemView", criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out int rows);

            if (tempPremiumReceivableItems.Rows.Count > 0)
            {
                tempPremiumReceivableItems.Columns.Add("rows", typeof(int));
                tempPremiumReceivableItems.Rows[0]["rows"] = rows;
            }

            // Load DTO
            List<SEARCH.PremiumReceivableItemDTO> premiumReceivableItems = new List<SEARCH.PremiumReceivableItemDTO>();
            decimal payableAmount=decimal.Zero;
            foreach (DataRow dataRow in tempPremiumReceivableItems)
            {
                decimal commissionPercentage = 0;
                decimal agentParticipationPercentage = 0;

                SEARCH.PendingCommissionDTO pendingCommission = new SEARCH.PendingCommissionDTO();
                SEARCH.PendingCommissionDTO commission = CommisionDAO.GetPendingCommission(Convert.ToInt32(dataRow["PolicyId"]), Convert.ToInt32(dataRow["EndorsementId"]));

                pendingCommission.PendingCommission = commission.PendingCommission;
                pendingCommission.CommissionPercentage = commission.CommissionPercentage;
                pendingCommission.AgentParticipationPercentage = commission.CommissionPercentage;
                payableAmount = decimal.Zero;
                if (Convert.ToBoolean(dataRow["IsReversion"]))
                {
                    payableAmount = Convert.ToDecimal(dataRow["LocalAmount"]);
                }
                else
                {
                    payableAmount = (Convert.ToDecimal(dataRow["Amount"]) > Convert.ToDecimal(dataRow["PaymentAmount"])) ? Convert.ToDecimal(dataRow["PaymentAmount"]) : Convert.ToDecimal(dataRow["LocalAmount"]);
                }
                premiumReceivableItems.Add(new SEARCH.PremiumReceivableItemDTO()
                {
                    // Campos propios de Item
                    PremiumReceivableItemId = Convert.ToInt32(dataRow["TempAppPremiumCode"]),
                    ImputationId = Convert.ToInt32(dataRow["TempAppCode"]),
                    PayableAmount = payableAmount,
                    // Campos para la grilla
                    BranchPrefixPolicyEndorsement = Convert.ToString(dataRow["BranchDescription"]).Substring(0, 3) + '-' + Convert.ToString(dataRow["PrefixTyniDescription"]) + '-' + Convert.ToString(dataRow["PolicyDocumentNumber"]) + '-' + Convert.ToString(dataRow["EndorsementDocumentNumber"]),
                    InsuredDocumentNumberName = Convert.ToString(dataRow["InsuredDocumentNumber"]) + '-' + Convert.ToString(dataRow["InsuredName"]),
                    PayerDocumentNumberName = Convert.ToString(dataRow["PayerDocumentNumber"]) + '-' + Convert.ToString(dataRow["PayerName"]),
                    //PolicyAgentDocumentNumberName = Convert.ToString(dataRow["PolicyAgentDocumentNumber"]) + '-' + Convert.ToString(dataRow["PolicyAgentName"]),
                    PolicyDocumentNumber = Convert.ToString(dataRow["PolicyDocumentNumber"]),
                    PolicyId = Convert.ToInt32(dataRow["PolicyId"]),
                    BussinessTypeId = Convert.ToInt32(dataRow["BusinessTypeCode"]),
                    BussinessTypeDescription = Convert.ToString(dataRow["BussinessTypeDescription"]),
                    BranchId = Convert.ToInt32(dataRow["BranchCode"]),
                    BranchDescription = Convert.ToString(dataRow["BranchDescription"]),
                    PrefixId = Convert.ToInt32(dataRow["PrefixCode"]),
                    PrefixDescription = Convert.ToString(dataRow["PrefixDescription"]),
                    PrefixTyniDescription = Convert.ToString(dataRow["PrefixTyniDescription"]),
                    EndorsementId = Convert.ToInt32(dataRow["EndorsementId"]),
                    EndorsementDocumentNumber = Convert.ToString(dataRow["EndorsementDocumentNumber"]),
                    EndorsementTypeId = Convert.ToInt32(dataRow["EndoTypeCode"]),
                    EndorsementTypeDescription = Convert.ToString(dataRow["EndorsementTypeDescription"]),
                    CollectGroupId = dataRow["BillingGroupCode"] != DBNull.Value ? Convert.ToInt32(dataRow["BillingGroupCode"]) : 0,
                    CollectGroupDescription = dataRow["BillingGroupDescription"] != DBNull.Value ? Convert.ToString(dataRow["BillingGroupDescription"]) : "",
                    PaymentNumber = Convert.ToInt32(dataRow["PaymentNum"]),
                    PayerId = Convert.ToInt32(dataRow["PayerIndividualId"]),
                    PayerDocumentNumber = Convert.ToString(dataRow["PayerDocumentNumber"]),
                    PayerName = Convert.ToString(dataRow["PayerName"]),
                    PaymentExpirationDate = Convert.ToDateTime(dataRow["PaymentExpirationDate"]),
                    PaymentAmount = Convert.ToDecimal(dataRow["PaymentAmount"]),
                    CurrencyId = Convert.ToInt32(dataRow["CurrencyCode"]),
                    CurrencyDescription = Convert.ToString(dataRow["CurrencyDescription"]),
                    ExchangeRate = Convert.ToDecimal(dataRow["ExchangeRate"]),
                    //PolicyAgentId = Convert.ToInt32(dataRow["PolicyAgentIndividualId"]),
                    //PolicyAgentDocumentNumber = Convert.ToString(dataRow["PolicyAgentDocumentNumber"]),
                    //PolicyAgentName = Convert.ToString(dataRow["PolicyAgentName"]),
                    InsuredId = Convert.ToInt32(dataRow["InsuredIndividualId"]),
                    InsuredDocumentNumber = Convert.ToString(dataRow["InsuredDocumentNumber"]),
                    InsuredName = Convert.ToString(dataRow["InsuredName"]),
                    ExcessPayment = (Convert.ToDecimal(dataRow["Amount"]) - Convert.ToDecimal(dataRow["PaymentAmount"]) > 0) ? Convert.ToDecimal(dataRow["Amount"]) - Convert.ToDecimal(dataRow["PaymentAmount"]) : 0,
                    DiscountedCommission = dataRow["DiscountedCommission"] != DBNull.Value ? Convert.ToDecimal(dataRow["DiscountedCommission"]) : 0,
                    PendingCommission = (Convert.ToDecimal(dataRow["PaymentAmount"]) * (agentParticipationPercentage / 100) * (commissionPercentage / 100)) - (dataRow["DiscountedCommission"] != DBNull.Value ? Convert.ToDecimal(dataRow["DiscountedCommission"]) : 0),
                    Address = dataRow["Street"].ToString() + " " + dataRow["HouseNumber"].ToString(),
                    Upd = HasDepositPrimes(Convert.ToInt32(dataRow["TempAppPremiumCode"])) ? 1 : 0,
                    PaidAmount = Convert.ToDecimal(dataRow["Amount"]),
                    IsReversion = Convert.ToBoolean(dataRow["IsReversion"]),
                    Rows = rows
                });
            }
            return premiumReceivableItems;

        }
        public static bool HasDepositPrimes(int tempPremiumReceivableId)
        {
            TempUsedDepositPremiumDAO _tempUsedDepositPremiumDAO = new TempUsedDepositPremiumDAO();
            bool isDeposit = false;

            List<SEARCH.TempUsedDepositPremiumDTO> tempUsedDepositPremiums = _tempUsedDepositPremiumDAO.GetTempUsedDepositPremiumByTempPremiumReceivableId(tempPremiumReceivableId);

            if (tempUsedDepositPremiums.Count > 0)
            {
                isDeposit = true;
            }

            return isDeposit;
        }

        public List<Models.Imputations.ApplicationPremium> GetTempApplicationsByFilter(List<Sistran.Core.Integration.UndewritingIntegrationServices.DTOs.PremiumSearchPolicyDTO> premiums)
        {

            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.Property(ACCOUNTINGEN.TempApplicationPremium.Properties.EndorsementCode);
            criteriaBuilder.In();
            criteriaBuilder.ListValue();
            premiums.ForEach(x =>
            {
                criteriaBuilder.Constant(Convert.ToInt32(x.EndorsementId));
            });
            criteriaBuilder.EndList();

            List<ACCOUNTINGEN.TempApplicationPremium> entityTempApplicationPremiums = DataFacadeManager.GetObjects(
                typeof(ACCOUNTINGEN.TempApplicationPremium), criteriaBuilder.GetPredicate()).
                Cast<ACCOUNTINGEN.TempApplicationPremium>().ToList();

            return ModelAssembler.CreateTempApplicationPremiums(entityTempApplicationPremiums);
        }


        public List<Models.Imputations.TempApplicationPremium> GetTempApplicationPremiumsByTempApplicationId(int tempApplicationPremium)
        {
            
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.Property(ACCOUNTINGEN.TempApplicationPremium.Properties.TempAppCode);
            criteriaBuilder.Equal();
            criteriaBuilder.Constant(tempApplicationPremium);

            List<ACCOUNTINGEN.TempApplicationPremium> entityTempApplicationPremiums = DataFacadeManager.GetObjects(
                typeof(ACCOUNTINGEN.TempApplicationPremium), criteriaBuilder.GetPredicate()).
                Cast<ACCOUNTINGEN.TempApplicationPremium>().ToList();

            return ModelAssembler.CreateTemporalApplicationPremiums(entityTempApplicationPremiums);
        }
        #endregion

    }
}
