//Sistran Core
using Sistran.Core.Application.TaxServices.DAOs;
using Sistran.Core.Application.TaxServices.DTOs;
using Sistran.Core.Application.TaxServices.EEProvider.Assemblers;
using Sistran.Core.Application.TaxServices.EEProvider.Business;
using Sistran.Core.Application.TaxServices.EEProvider.DAOs;
using Sistran.Core.Application.TaxServices.views;
using Sistran.Core.Application.Utilities.DataFacade;
//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using TaxEntities = Sistran.Core.Application.Tax.Entities;
using UniquePersonEntities = Sistran.Core.Application.UniquePerson.Entities;

namespace Sistran.Core.Application.TaxServices.EEProvider
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class TaxServicesEEProvider : ITaxService
    {
        #region Instance Variables

        #region Interfaz

        /// <summary>
        /// Declaración del contexto y del dataFacadeManager
        /// </summary>
        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion Interfaz

        #region DAOs

        #endregion DAOs

        #endregion Instance Viarables

        #region Public Methods

        #region Conditions - Categories

        /// <summary>
        /// GetNumber
        /// </summary>
        public void GetNumber()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// GetTax
        /// </summary>
        /// <returns></returns>
        public List<Models.Tax> GetTax()
        {
            try
            {
                TaxDAO taxDAO = new TaxDAO();
                return taxDAO.GetTax();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// GetTaxCondition
        /// </summary>
        /// <returns></returns>
        public List<Models.TaxCondition> GetTaxCondition()
        {
            try
            {
                TaxDAO taxDAO = new TaxDAO();
                return taxDAO.GetTaxCondition();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// GetTaxConditionByTaxId
        /// </summary>
        /// <param name="taxId"></param>
        /// <returns></returns>
        public List<Models.TaxCondition> GetTaxConditionByTaxId(int taxId)
        {
            try
            {
                TaxDAO taxDAO = new TaxDAO();
                return taxDAO.GetTaxConditionByTaxId(taxId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// GetTaxCategory
        /// </summary>
        /// <returns></returns>
        public List<Models.TaxCategory> GetTaxCategory()
        {
            try
            {
                TaxDAO taxDAO = new TaxDAO();
                return taxDAO.GetTaxCategory();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// GetTaxCategoryByTaxId
        /// </summary>
        /// <param name="taxId"></param>
        /// <returns></returns>
        public List<Models.TaxCategory> GetTaxCategoryByTaxId(int taxId)
        {
            try
            {
                TaxDAO taxDAO = new TaxDAO();
                return taxDAO.GetTaxCategoryByTaxId(taxId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// GetTaxesByIndividualId
        /// </summary>
        /// <param name="IndividualId"></param>
        /// <returns></returns>
        public List<Models.Tax> GetTaxesByIndividualId(int IndividualId)
        {
            try
            {
                TaxDAO taxDAO = new TaxDAO();
                return taxDAO.GetTaxesByIndividualId(IndividualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        #endregion

        #region Tax

        /// <summary>
        /// GetIndividualTaxCategoryCondition
        /// </summary>
        /// <param name="individualId">individualId</param>
        /// <returns>List<IndividualTaxCategoryConditionDTO></returns>
        public List<IndividualTaxCategoryConditionDTO> GetIndividualTaxCategoryCondition(int individualId, int? roleId = 0)
        {
            try
            {

                return TaxBusiness.GetIndividualTaxCategoryCondition(individualId, roleId);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetTotalTax
        /// </summary>
        /// <param name="individualId">individualId</param>
        /// <param name="conditionCode">conditionCode</param>
        /// <param name="categoryCode">categoryCode</param>
        /// <param name="branchCode">branchCode</param>
        /// <param name="lineBusinessCode">lineBusinessCode</param>
        /// <param name="stateCode">stateCode</param>
        /// <param name="countryCode">countryCode</param>
        /// <param name="economicActivity">economicActivity</param>
        /// <param name="exchangeRate">exchangeRate</param>
        /// <param name="amount">amount</param>
        /// <returns>Decimal</returns>
        public decimal GetTotalTax(int individualId, int conditionCode, Dictionary<int, int> categories, int branchCode, int lineBusinessCode,
                             double exchangeRate, double amount)
        {
            try
            {
                decimal totalTax = 0;

                if (lineBusinessCode == -1)
                {
                    totalTax = CalculateRetention(individualId, conditionCode, categories, branchCode, lineBusinessCode, exchangeRate, amount);
                }
                else
                {
                    totalTax = CalculateTax(individualId, conditionCode, categories, branchCode, lineBusinessCode, exchangeRate, amount);
                }

                return totalTax;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// Calculates the payment taxes by individual identifier accounting concept identifier tax attributes amount.
        /// </summary>
        /// <param name="individualId">The individual identifier.</param>
        /// <param name="accontingConceptId">The acconting concept identifier.</param>
        /// <param name="taxAttributes">The tax attributes.</param>
        /// <param name="amount">The amount.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public List<TaxCategoryConditionDTO> CalculatePaymentTaxesByIndividualIdAccountingConceptIdTaxAttributesAmount(int individualId, int accontingConceptId, List<TaxAttributeDTO> taxAttributes, decimal amount)
        {
            try
            {
                return TaxBusiness.CalculatePaymentTaxesByIndividualIdAccountingConceptIdTaxAttributesAmount(individualId, accontingConceptId, taxAttributes, amount);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// Gets the tax attributes by tax identifier.
        /// </summary>
        /// <param name="taxId">The tax identifier.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public List<TaxAttributeDTO> GetTaxAttributesByTaxId(int taxId)
        {
            try
            {
                TaxDAO taxDAO = new TaxDAO();
                return DTOAssembler.CreateTaxAttributes(taxDAO.GetTaxAttributesByTaxId(taxId)).ToList();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// Gets the tax attributes.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public List<TaxAttributeDTO> GetTaxAttributes()
        {
            try
            {
                TaxDAO taxDAO = new TaxDAO();
                return DTOAssembler.CreateTaxAttributes(taxDAO.GetTaxAttributes()).ToList();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// obtener personas exoneradas de impuestos
        /// </summary>
        /// <param name="individualId"></param>
        /// <param name="taxCode"></param>
        /// <param name="currentFrom"></param>
        /// <returns></returns>
        public IndividualTaxExemptionDTO GetIndividualTaxExemptionByIndividualId(int individualId, int taxCode, DateTime currentFrom)
        {
            try
            {
                return TaxBusiness.GetIndividualTaxExemptionByIndividualId(individualId, taxCode, currentFrom);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
        #endregion

        #region AccountingCoceptTax
        /// <summary>
        /// Creates the accounting concept taxes.
        /// </summary>
        /// <param name="accountingConceptTaxes">The accounting concept taxes.</param>
        /// <returns></returns>
        public List<AccountingConceptTaxDTO> CreateAccountingConceptTaxes(List<AccountingConceptTaxDTO> accountingConceptTaxes)
        {
            AccountingConceptTaxDAO accountingConceptTaxDAO = new AccountingConceptTaxDAO();

            accountingConceptTaxes.RemoveAll(x => x.AccountingConceptTaxId != 0);

            return DTOAssembler.CreateAccountingConceptTaxes(accountingConceptTaxDAO.CreateAccountingConceptTaxes(ModelAssembler.CreateAccountingConceptTaxes(accountingConceptTaxes)));
        }

        /// <summary>
        /// Deletes the accounting concept tax.
        /// </summary>
        /// <param name="accountingConceptTaxId">The accounting concept tax identifier.</param>
        public void DeleteAccountingConceptTax(int accountingConceptTaxId)
        {
            AccountingConceptTaxDAO accountingConceptTaxDAO = new AccountingConceptTaxDAO();
            accountingConceptTaxDAO.DeleteAccountingConceptTax(accountingConceptTaxId);
        }

        /// <summary>
        /// Gets the accounting concept taxes by accounting concept identifier branch identifier.
        /// </summary>
        /// <param name="accountingConceptId">The accounting concept identifier.</param>
        /// <param name="branchId">The branch identifier.</param>
        /// <returns></returns>
        public List<AccountingConceptTaxDTO> GetAccountingConceptTaxesByAccountingConceptIdBranchId(int accountingConceptId, int branchId)
        {
            AccountingConceptTaxDAO accountingConceptTaxDAO = new AccountingConceptTaxDAO();
            return DTOAssembler.CreateAccountingConceptTaxes(accountingConceptTaxDAO.GetAccountingConceptTaxByAccountingConceptIdBranchId(accountingConceptId, branchId));
        }
        #endregion

        #endregion

        #region Private Methods

        /// <summary>
        /// CalculateRetention Verificar si se elimina
        /// </summary>
        /// <param name="individualId">The individual identifier.</param>
        /// <param name="conditionCode">The condition code.</param>
        /// <param name="taxCategories">The tax categories.</param>
        /// <param name="branchCode">The branch code.</param>
        /// <param name="lineBusinessCode">The line business code.</param>
        /// <param name="exchangeRate">The exchange rate.</param>
        /// <param name="amount">The amount.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        private decimal CalculateRetention(int individualId, int conditionCode, Dictionary<int, int> taxCategories, int branchCode, int lineBusinessCode, double exchangeRate, double amount)
        {
            try
            {
                int categoryCode = 0;
                DataTable dataTableTax = new DataTable();
                dataTableTax.Columns.Add("TaxConditionCode", typeof(int));
                dataTableTax.Columns.Add("TaxCategoryCode", typeof(int));
                dataTableTax.Columns.Add("TaxCode", typeof(int));
                dataTableTax.Columns.Add("Tax", typeof(string));
                dataTableTax.Columns.Add("Rate", typeof(decimal));
                dataTableTax.Columns.Add("TaxAmountBase", typeof(decimal));
                dataTableTax.Columns.Add("TaxValue", typeof(decimal));

                int stateCode = 0;
                int countryCode = 0;
                int economicActivity = 0;
                int AccountingConceptId = 0;

                #region DataRequest

                List<int> address = GetAddressByIndividualId(individualId);
                if (address.Count > 1)
                {
                    stateCode = address[0];
                    countryCode = address[1];
                }

                economicActivity = GetEconomicActivityByIndividualId(individualId);

                #endregion


                #region SearchTax

                //Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(TaxEntities.IndividualTax.Properties.IndividualId, individualId).And();
                criteriaBuilder.PropertyEquals(TaxEntities.Tax.Properties.Enabled, 1);

                List<IndividualTaxAttributeDTO> taxAttributes = GetAttributeTaxByIndividualId(individualId);
                int auxTaxCode = 0;
                int auxTaxCodeOne = 0;
                int numberTax = 0;

                foreach (IndividualTaxAttributeDTO taxAttribute in taxAttributes)
                {
                    auxTaxCode = taxAttribute.TaxId;
                    if (ValidateIndexCategory(taxCategories, auxTaxCode))
                    {
                        criteriaBuilder.And();
                        criteriaBuilder.OpenParenthesis();
                        categoryCode = taxCategories[auxTaxCode];

                        if (auxTaxCode != auxTaxCodeOne)
                        {
                            if (numberTax > 0)
                            {
                                criteriaBuilder.CloseParenthesis();
                                criteriaBuilder.Or();
                                numberTax = 0;
                            }
                            criteriaBuilder.OpenParenthesis();
                            criteriaBuilder.PropertyEquals(TaxEntities.TaxCondition.Properties.TaxCode, "tx", auxTaxCode);
                            criteriaBuilder.And();
                            numberTax = numberTax + 1;
                        }
                        else
                        {
                            criteriaBuilder.And();
                        }
                        auxTaxCodeOne = auxTaxCode;
                        switch (taxAttribute.TaxAttributeDescription)
                        {
                            case "TAX_CONDITION_CODE":
                                criteriaBuilder.PropertyEquals(TaxEntities.TaxCondition.Properties.TaxConditionCode, "tc", conditionCode);
                                break;
                            case "TAX_CATEGORY_CODE":
                                criteriaBuilder.PropertyEquals(TaxEntities.TaxRate.Properties.TaxCategoryCode, categoryCode);
                                break;
                            case "STATE_CODE":
                                criteriaBuilder.PropertyEquals(TaxEntities.TaxRate.Properties.StateCode, stateCode);
                                break;
                            case "COUNTRY_CODE":
                                criteriaBuilder.PropertyEquals(TaxEntities.TaxRate.Properties.CountryCode, countryCode);
                                break;
                            case "ECONOMIC_ACTIVITY_CODE":
                                criteriaBuilder.PropertyEquals(TaxEntities.TaxRate.Properties.EconomicActivityTaxCode, economicActivity);
                                break;
                            case "BRANCH_CODE":
                                if (branchCode > 0)
                                {
                                    criteriaBuilder.PropertyEquals(TaxEntities.TaxRate.Properties.BranchCode, branchCode);
                                }
                                else
                                {
                                    criteriaBuilder.Property(TaxEntities.TaxRate.Properties.BranchCode);
                                    criteriaBuilder.IsNull();
                                }
                                break;
                            case "LINE_BUSINESS_CODE":
                                if (lineBusinessCode > 0)
                                {
                                    criteriaBuilder.PropertyEquals(TaxEntities.TaxRate.Properties.LineBusinessCode, lineBusinessCode);
                                }
                                else
                                {
                                    criteriaBuilder.Property(TaxEntities.TaxRate.Properties.LineBusinessCode);
                                    criteriaBuilder.IsNull();
                                }
                                break;
                        }
                        criteriaBuilder.CloseParenthesis();
                    }

                }
                if (numberTax > 0)
                {
                    criteriaBuilder.CloseParenthesis();
                    //numberTax = 0;
                }

                //Lista de impuestos
                List<IndividualTaxCategoryConditionDTO> individualTaxes = GetIndividualTaxesByCriteriaBuilder(criteriaBuilder);

                #endregion 

                #region CalculateTax

                foreach (IndividualTaxCategoryConditionDTO individualTax in individualTaxes)
                {
                    int baseConditionTaxCode = 0;

                    //Con retenciones
                    if (Convert.ToBoolean(individualTax.IsRetention))
                    {
                        baseConditionTaxCode = Convert.ToInt32(individualTax.BaseConditionTaxId);

                        // tax con base imponible
                        if (baseConditionTaxCode == 0)
                        {
                            int rateTypeCode = Convert.ToInt32(individualTax.RateTypeId);
                            int taxCode = Convert.ToInt32(individualTax.TaxId);
                            double rate = Convert.ToDouble(individualTax.Rate);
                            int taxConditionCode = Convert.ToInt32(individualTax.TaxConditionId);
                            int enabled = Convert.ToInt32(individualTax.Enabled);
                            DateTime currentFrom = Convert.ToDateTime(individualTax.CurrentFrom);
                            double minBaseAmount = Convert.ToDouble(individualTax.MinBaseAmount);

                            if (enabled == 1 && currentFrom < DateTime.Now)
                            {
                                if (rateTypeCode == (int)RateType.FixedValue)
                                {
                                    rate = rate / exchangeRate;
                                }

                                decimal excemptionPct = GetExemptionPercentage(individualId, taxCode, DateTime.Now, countryCode, stateCode);

                                // calcula si supera la base minima
                                double itemAmount = 0;
                                if (minBaseAmount <= amount && taxConditionCode == 1)
                                {
                                    itemAmount = Calculate(rateTypeCode, rate, amount, excemptionPct);
                                }

                                DataRow dataRowTax = dataTableTax.NewRow();
                                dataRowTax["TaxConditionCode"] = individualTax.TaxConditionId;
                                dataRowTax["TaxCategoryCode"] = individualTax.TaxCategoryId;
                                dataRowTax["TaxCode"] = taxCode;
                                dataRowTax["Tax"] = individualTax.TaxDescription;
                                dataRowTax["Rate"] = rate;
                                dataRowTax["TaxAmountBase"] = amount;
                                dataRowTax["TaxValue"] = itemAmount;
                                dataTableTax.Rows.Add(dataRowTax);
                            }
                        }
                    }
                }

                #endregion

                #region CalculateTaxDep

                foreach (IndividualTaxCategoryConditionDTO individualTax in individualTaxes)
                {
                    //Con retenciones
                    if (!Convert.ToBoolean(individualTax.IsRetention))
                    {
                        int baseConditionTaxCode = Convert.ToInt32(individualTax.BaseConditionTaxId);

                        // tax con base imponible
                        if (baseConditionTaxCode > 0)
                        {
                            int rateTypeCode = Convert.ToInt32(individualTax.RateTypeId);
                            int taxCode = Convert.ToInt32(individualTax.TaxId);
                            double rate = Convert.ToDouble(individualTax.Rate);
                            int taxConditionCode = Convert.ToInt32(individualTax.TaxConditionId);
                            int enabled = Convert.ToInt32(individualTax.Enabled);
                            DateTime currentFrom = Convert.ToDateTime(individualTax.CurrentFrom);
                            double minBaseAmount = Convert.ToDouble(individualTax.MinBaseAmount);

                            if (enabled == 1 && currentFrom < DateTime.Now)
                            {
                                if (rateTypeCode == (int)RateType.FixedValue)
                                {
                                    rate = rate / exchangeRate;
                                }

                                double itemAmount = 0;
                                bool taxBaseCondition = false;
                                foreach (DataRow row in dataTableTax.Rows)
                                {
                                    if (baseConditionTaxCode == Convert.ToInt32(row["TaxCode"]))
                                    {
                                        amount = Convert.ToDouble(row["TaxValue"]);

                                        decimal excemptionPct = GetExemptionPercentage(individualId, taxCode, DateTime.Now, countryCode, stateCode);

                                        // calcula si supera la base minima
                                        itemAmount = 0;
                                        if (minBaseAmount <= amount && taxConditionCode == 1)
                                        {
                                            itemAmount = Calculate(rateTypeCode, rate, amount, excemptionPct);
                                        }
                                        taxBaseCondition = true;
                                    }
                                }

                                if (!taxBaseCondition)
                                {
                                    amount = 0;
                                    itemAmount = 0;
                                }

                                DataRow dataRowTax = dataTableTax.NewRow();
                                dataRowTax["TaxConditionCode"] = individualTax.TaxConditionId;
                                dataRowTax["TaxCategoryCode"] = individualTax.TaxCategoryId;
                                dataRowTax["TaxCode"] = taxCode;
                                dataRowTax["Tax"] = individualTax.TaxDescription;
                                dataRowTax["Rate"] = rate;
                                dataRowTax["TaxAmountBase"] = amount;
                                dataRowTax["TaxValue"] = itemAmount;
                                dataTableTax.Rows.Add(dataRowTax);
                            }
                        }
                    }
                }

                #endregion

                decimal totalTax = 0;

                foreach (DataRow dataRow in dataTableTax.Rows)
                {
                    totalTax = totalTax + Convert.ToDecimal(dataRow["TaxValue"]);
                }

                return totalTax;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetAttributeTaxByIndividualId
        /// </summary>
        /// <param name="individualId">individualId</param>
        /// <returns>DataTable</returns>
        private List<IndividualTaxAttributeDTO> GetAttributeTaxByIndividualId(int individualId)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(TaxEntities.IndividualTax.Properties.IndividualId, individualId).And();
                criteriaBuilder.PropertyEquals(TaxEntities.Tax.Properties.Enabled, 1);

                SelectQuery selectQuery = new SelectQuery();
                selectQuery.AddSelectValue(new SelectValue(new Column(TaxEntities.IndividualTax.Properties.IndividualId, "it"), "IndividualId"));
                //selectQuery.AddSelectValue(new SelectValue(new Column(TaxEntities.IndividualTax.Properties.TaxCode, "it"), "TaxCode"));
                selectQuery.AddSelectValue(new SelectValue(new Column(TaxEntities.Tax.Properties.Enabled, "tx"), "Enabled"));
                selectQuery.AddSelectValue(new SelectValue(new Column(TaxEntities.TaxAttributeType.Properties.Description, "tt"), "TaxAttributeTypeDescription"));
                selectQuery.AddSelectValue(new SelectValue(new Column(TaxEntities.TaxAttributeType.Properties.TaxAttributeTypeCode, "tt"), "TaxAttributeTypeCode"));

                Join join = new Join(new ClassNameTable(typeof(TaxEntities.IndividualTax), "it"),
                                     new ClassNameTable(typeof(TaxEntities.TaxAttribute), "ta"), JoinType.Inner);
                // join.Criteria = (new ObjectCriteriaBuilder()
                //     .Property(TaxEntities.IndividualTax.Properties.TaxCode, "it")
                //     .Equal().Property(TaxEntities.TaxAttribute.Properties.TaxCode, "ta")
                //     .GetPredicate());

                join = new Join(join, new ClassNameTable(typeof(TaxEntities.TaxAttributeType), "tt"), JoinType.Inner);
                join.Criteria = (new ObjectCriteriaBuilder()
                    .Property(TaxEntities.TaxAttributeType.Properties.TaxAttributeTypeCode, "tt")
                    .Equal()
                    .Property(TaxEntities.TaxAttribute.Properties.TaxAttributeTypeCode, "ta")
                    .GetPredicate());

                //join = new Join(join, new ClassNameTable(typeof(TaxEntities.Tax), "tx"), JoinType.Inner);
                //join.Criteria = (new ObjectCriteriaBuilder()
                //    .Property(TaxEntities.Tax.Properties.TaxCode, "tx")
                //    .Equal()
                //    .Property(TaxEntities.IndividualTax.Properties.TaxCode, "it")
                //    .GetPredicate());

                selectQuery.Table = join;
                selectQuery.Where = criteriaBuilder.GetPredicate();

                List<IndividualTaxAttributeDTO> individualTaxAttributes = new List<IndividualTaxAttributeDTO>();

                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
                {
                    while (reader.Read())
                    {
                        individualTaxAttributes.Add(new IndividualTaxAttributeDTO()
                        {
                            Enabled = Convert.ToBoolean(reader["Enabled"]),
                            IndividualId = Convert.ToInt32(reader["IndividualId"]),
                            TaxAttributeDescription = reader["TaxAttributeTypeDescription"] == DBNull.Value ? "" : Convert.ToString(reader["TaxAttributeTypeDescription"]),
                            TaxAttributeId = reader["TaxAttributeTypeCode"] == DBNull.Value ? 0 : Convert.ToInt32(reader["TaxAttributeTypeCode"]),
                            TaxId = reader["TaxCode"] == DBNull.Value ? 0 : Convert.ToInt32(reader["TaxCode"])
                        });
                    }
                }

                var distinctTaxAttributes = individualTaxAttributes
                    .GroupBy(p => new { p.Enabled, p.IndividualId, p.TaxAttributeDescription, p.TaxAttributeId, p.TaxId })
                    .Select(g => g.First())
                    .ToList();

                return distinctTaxAttributes;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetAddressByIndividualId
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        private List<int> GetAddressByIndividualId(int individualId)
        {
            try
            {
                List<int> address = new List<int>();

                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(UniquePersonEntities.Address.Properties.IndividualId, individualId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(UniquePersonEntities.Address.Properties.IsMailingAddress, true);

                BusinessCollection businessCollection =
                   new BusinessCollection(
                       _dataFacadeManager.GetDataFacade().SelectObjects(typeof(UniquePersonEntities.Address), criteriaBuilder.GetPredicate()));

                if (businessCollection.Count > 0)
                {
                    foreach (UniquePersonEntities.Address addressEntity in businessCollection.OfType<UniquePersonEntities.Address>())
                    {
                        address.Add(Convert.ToInt32(addressEntity.StateCode));
                        address.Add(Convert.ToInt32(addressEntity.CountryCode));
                    }
                }

                return address;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetIndividualTaxesByCriteriaBuilder
        /// </summary>
        /// <param name="criteriaBuilder"></param>
        /// <returns></returns>
        private List<IndividualTaxCategoryConditionDTO> GetIndividualTaxesByCriteriaBuilder(ObjectCriteriaBuilder criteriaBuilder)
        {
            try
            {
                SelectQuery selectQuery = new SelectQuery();
                selectQuery.AddSelectValue(new SelectValue(new Column(TaxEntities.IndividualTax.Properties.IndividualId, "it"), "IndividualId"));
                selectQuery.AddSelectValue(new SelectValue(new Column(TaxEntities.Tax.Properties.TaxCode, "tx"), "TaxCode"));
                selectQuery.AddSelectValue(new SelectValue(new Column(TaxEntities.Tax.Properties.Description, "tx"), "TaxDescription"));
                selectQuery.AddSelectValue(new SelectValue(new Column(TaxEntities.Tax.Properties.BaseConditionTaxCode, "tx"), "BaseConditionTaxCode"));
                selectQuery.AddSelectValue(new SelectValue(new Column(TaxEntities.Tax.Properties.Enabled, "tx"), "Enabled"));
                selectQuery.AddSelectValue(new SelectValue(new Column(TaxEntities.Tax.Properties.IsRetention, "tx"), "IsRetention"));
                selectQuery.AddSelectValue(new SelectValue(new Column(TaxEntities.Tax.Properties.RateTypeCode, "tx"), "RateTypeCode"));
                selectQuery.AddSelectValue(new SelectValue(new Column(TaxEntities.TaxCondition.Properties.Description, "tc"), "TaxConditionDescription"));
                selectQuery.AddSelectValue(new SelectValue(new Column(TaxEntities.TaxCondition.Properties.TaxConditionCode, "tc"), "TaxConditionCode"));
                selectQuery.AddSelectValue(new SelectValue(new Column(TaxEntities.TaxPeriodRate.Properties.Rate, "tp"), "Rate"));
                selectQuery.AddSelectValue(new SelectValue(new Column(TaxEntities.TaxPeriodRate.Properties.MinBaseAmount, "tp"), "MinBaseAmount"));
                selectQuery.AddSelectValue(new SelectValue(new Column(TaxEntities.TaxPeriodRate.Properties.CurrentFrom, "tp"), "CurrentFrom"));
                selectQuery.AddSelectValue(new SelectValue(new Column(TaxEntities.TaxPeriodRate.Properties.CurrentTo, "tp"), "CurrentTo"));
                selectQuery.AddSelectValue(new SelectValue(new Column(TaxEntities.TaxRate.Properties.BranchCode, "tr"), "BranchCode"));
                selectQuery.AddSelectValue(new SelectValue(new Column(TaxEntities.TaxRate.Properties.CountryCode, "tr"), "CountryCode"));
                selectQuery.AddSelectValue(new SelectValue(new Column(TaxEntities.TaxRate.Properties.CoverageId, "tr"), "CoverageId"));
                selectQuery.AddSelectValue(new SelectValue(new Column(TaxEntities.TaxRate.Properties.EconomicActivityTaxCode, "tr"), "EconomicActivityCode"));
                selectQuery.AddSelectValue(new SelectValue(new Column(TaxEntities.TaxRate.Properties.StateCode, "tr"), "StateCode"));
                selectQuery.AddSelectValue(new SelectValue(new Column(TaxEntities.TaxRate.Properties.LineBusinessCode, "tr"), "LineBusinessCode"));
                selectQuery.AddSelectValue(new SelectValue(new Column(TaxEntities.TaxRate.Properties.TaxCategoryCode, "tr"), "TaxCategoryCode"));

                Join join = new Join(new ClassNameTable(typeof(TaxEntities.IndividualTax), "it"),
                                     new ClassNameTable(typeof(TaxEntities.TaxCondition), "tc"), JoinType.Inner);

                join = new Join(join, new ClassNameTable(typeof(TaxEntities.TaxPeriodRate), "tp"), JoinType.Inner);
                join.Criteria = (new ObjectCriteriaBuilder()
                    .Property(TaxEntities.TaxPeriodRate.Properties.TaxRateId, "tp")
                    .Equal()
                    .Property(TaxEntities.TaxRate.Properties.TaxRateId, "tr")
                    .GetPredicate());

                selectQuery.Table = join;
                selectQuery.Where = criteriaBuilder.GetPredicate();

                List<IndividualTaxCategoryConditionDTO> individualTaxes = new List<IndividualTaxCategoryConditionDTO>();

                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
                {
                    while (reader.Read())
                    {
                        individualTaxes.Add(new IndividualTaxCategoryConditionDTO()
                        {
                            BaseConditionTaxId = reader["BaseConditionTaxCode"] == DBNull.Value ? 0 : Convert.ToInt32(reader["BaseConditionTaxCode"]),
                            BranchId = reader["BranchCode"] == DBNull.Value ? 0 : Convert.ToInt32(reader["BranchCode"]),
                            CountryId = reader["CountryCode"] == DBNull.Value ? 0 : Convert.ToInt32(reader["CountryCode"]),
                            CoverageId = reader["CoverageId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["CoverageId"]),
                            CurrentFrom = reader["CurrentFrom"] == DBNull.Value ? Convert.ToDateTime("01/01/0001 0:00:00") : Convert.ToDateTime(reader["CurrentFrom"]),
                            CurrentTo = reader["CurrentTo"] == DBNull.Value ? Convert.ToDateTime("01/01/0001 0:00:00") : Convert.ToDateTime(reader["CurrentTo"]),
                            EconomicActivityTaxId = reader["EconomicActivityCode"] == DBNull.Value ? 0 : Convert.ToInt32(reader["EconomicActivityCode"]),
                            Enabled = Convert.ToBoolean(reader["Enabled"]),
                            IndividualId = Convert.ToInt32(reader["IndividualId"]),
                            IsRetention = Convert.ToBoolean(reader["IsRetention"]),
                            LineBusinessId = reader["LineBusinessCode"] == DBNull.Value ? 0 : Convert.ToInt32(reader["LineBusinessCode"]),
                            MinBaseAmount = reader["MinBaseAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["MinBaseAmount"]),
                            Rate = reader["Rate"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["Rate"]),
                            RateTypeDescription = "",
                            RateTypeId = reader["RateTypeCode"] == DBNull.Value ? 0 : Convert.ToInt32(reader["RateTypeCode"]),
                            StateId = reader["StateCode"] == DBNull.Value ? 0 : Convert.ToInt32(reader["StateCode"]),
                            TaxCategoryDescription = "",
                            TaxCategoryId = reader["TaxCategoryCode"] == DBNull.Value ? 0 : Convert.ToInt32(reader["TaxCategoryCode"]),
                            TaxConditionDescription = reader["TaxConditionDescription"] == DBNull.Value ? "" : Convert.ToString(reader["TaxConditionDescription"]),
                            TaxConditionId = reader["TaxConditionCode"] == DBNull.Value ? 0 : Convert.ToInt32(reader["TaxConditionCode"]),
                            TaxDescription = reader["TaxDescription"] == DBNull.Value ? "" : Convert.ToString(reader["TaxDescription"]),
                            TaxId = reader["TaxCode"] == DBNull.Value ? 0 : Convert.ToInt32(reader["TaxCode"])
                        });
                    }
                }

                return individualTaxes;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }


        /// <summary>
        /// GetEconomicActivityByIndividualId
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        private int GetEconomicActivityByIndividualId(int individualId)
        {
            int economicActivityId = 0;

            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(UniquePersonEntities.Individual.Properties.IndividualId, individualId);

                BusinessCollection businessCollection =
                   new BusinessCollection(
                       _dataFacadeManager.GetDataFacade().SelectObjects(typeof(UniquePersonEntities.Individual), criteriaBuilder.GetPredicate()));

                if (businessCollection.Count > 0)
                {
                    foreach (UniquePersonEntities.Individual individual in businessCollection.OfType<UniquePersonEntities.Individual>())
                    {
                        economicActivityId = Convert.ToInt32(individual.EconomicActivityCode);
                    }
                }
                else
                {
                    economicActivityId = 0;
                }

                return economicActivityId;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetExemptionPercentage
        /// </summary>
        /// <param name="individualId"></param>
        /// <param name="taxCode"></param>
        /// <param name="date"></param>
        /// <param name="countryCode"></param>
        /// <param name="stateCode"></param>
        /// <returns></returns>
        private decimal GetExemptionPercentage(int individualId, int taxCode, DateTime date, int countryCode, int stateCode)
        {
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

            #region Filtro

            criteriaBuilder.PropertyEquals(TaxEntities.IndividualTaxExemption.Properties.IndividualId, individualId).And();
            criteriaBuilder.PropertyEquals(TaxEntities.IndividualTaxExemption.Properties.TaxCode, taxCode).And();

            criteriaBuilder.Property(TaxEntities.IndividualTaxExemption.Properties.CurrentFrom);
            criteriaBuilder.LessEqual();
            criteriaBuilder.Constant(date);

            criteriaBuilder.And();
            criteriaBuilder.Property(TaxEntities.IndividualTaxExemption.Properties.CountryCode);

            if (countryCode == 0)
            {
                criteriaBuilder.IsNull();
            }
            else
            {
                criteriaBuilder.Equal();
                criteriaBuilder.Constant(countryCode);
            }

            //Provincia
            criteriaBuilder.And();

            criteriaBuilder.OpenParenthesis();
            criteriaBuilder.Property(TaxEntities.IndividualTaxExemption.Properties.StateCode);
            criteriaBuilder.IsNull();

            if (stateCode != 0)
            {
                criteriaBuilder.Or();
                criteriaBuilder.Property(TaxEntities.IndividualTaxExemption.Properties.StateCode);
                criteriaBuilder.Equal();
                criteriaBuilder.Constant(stateCode);
            }

            criteriaBuilder.CloseParenthesis();

            #endregion

            decimal exemptionPercentage = 0;

            IndividualTaxExemptionView view = new IndividualTaxExemptionView();
            ViewBuilder builder = new ViewBuilder("IndividualTaxExemptionView");
            builder.Filter = criteriaBuilder.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            if (view.TaxExemptions.Count > 0)
            {
                foreach (TaxEntities.IndividualTaxExemption individualTaxExemption in view.TaxExemptions.OfType<TaxEntities.IndividualTaxExemption>())
                {
                    exemptionPercentage = Convert.ToDecimal(individualTaxExemption.ExemptionPercentage);
                }
            }

            return exemptionPercentage;
        }

        /// <summary>
        /// Calcula el impuesto. Multiplica la tasa por la base imponible.
        /// </summary>
        /// <param name="rateTypeCode">Tipo de tasa</param>
        /// <param name="rate">Tasa que se desea aplicar.</param>
        /// <param name="amount">Base imponible</param>
        /// <param name="exemptionPercentage"></param>
        /// <returns>Importe del impuesto</returns>
        private double Calculate(int rateTypeCode, double rate, double amount, decimal exemptionPercentage)
        {
            double factor = Convert.ToDouble(GetFactor(rateTypeCode));
            double tax = 0;

            if (rateTypeCode == (int)RateType.FixedValue)
            {
                tax = rate;
            }
            else
            {
                tax = amount * rate * factor;
            }

            return Math.Round(tax - (tax * (double)exemptionPercentage / 100), 2);
        }

        /// <summary>
        /// ValidateIndexCategory
        /// Valida existencia de indice en categoria
        /// </summary>
        /// <param name="taxCategories">The tax categories.</param>
        /// <param name="TaxId">The tax identifier.</param>
        /// <returns>
        /// bool
        /// </returns>
        private bool ValidateIndexCategory(Dictionary<int, int> taxCategories, int TaxId)
        {
            bool result = false;

            for (int i = 0; i < taxCategories.Count; i++)
            {
                if (taxCategories.Keys.ElementAt(i) == TaxId)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }


        /// <summary>
        /// GetTax
        /// </summary>
        /// <param name="individualId">individualId</param>
        /// <param name="branchCode">branchCode</param>
        /// <param name="lineBusinessCode">lineBusinessCode</param>
        /// <param name="exchangeRate">exchangeRate</param>
        /// <param name="amount">amount</param>
        /// <param name="paymentRequestCode">paymentRequestCode</param>
        /// <param name="voucherConceptCode">voucherConceptCode</param>
        /// <returns>DataTable</returns>
        private decimal CalculateTax(int individualId, int conditionCode, Dictionary<int, int> taxCategories, int branchCode, int lineBusinessCode, double exchangeRate, double amount)
        {
            try
            {
                int categoryCode = 0;
                DataTable dataTableTax = new DataTable();
                dataTableTax.Columns.Add("TaxConditionCode", typeof(int));
                dataTableTax.Columns.Add("TaxCategoryCode", typeof(int));
                dataTableTax.Columns.Add("TaxCode", typeof(int));
                dataTableTax.Columns.Add("Tax", typeof(string));
                dataTableTax.Columns.Add("Rate", typeof(decimal));
                dataTableTax.Columns.Add("TaxAmountBase", typeof(decimal));
                dataTableTax.Columns.Add("TaxValue", typeof(decimal));

                int stateCode = 0;
                int countryCode = 0;
                int economicActivity = 0;
                int AccountingConceptId = 0;
                #region DataRequest

                List<int> address = GetAddressByIndividualId(individualId);
                if (address.Count > 1)
                {
                    stateCode = address[0];
                    countryCode = address[1];
                }

                economicActivity = GetEconomicActivityByIndividualId(individualId);

                #endregion

                #region SearchTax

                //Filtro
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(TaxEntities.IndividualTax.Properties.IndividualId, individualId).And();
                criteriaBuilder.PropertyEquals(TaxEntities.Tax.Properties.Enabled, 1);

                List<IndividualTaxAttributeDTO> taxAttributes = GetAttributeTaxByIndividualId(individualId);
                int auxTaxCode = 0;
                int auxTaxCodeOne = 0;
                int numberTax = 0;

                foreach (IndividualTaxAttributeDTO taxAttribute in taxAttributes)
                {
                    auxTaxCode = taxAttribute.TaxId;

                    if (ValidateIndexCategory(taxCategories, auxTaxCode))
                    {
                        criteriaBuilder.And();
                        criteriaBuilder.OpenParenthesis();

                        categoryCode = taxCategories[auxTaxCode];

                        if (auxTaxCode != auxTaxCodeOne)
                        {
                            if (numberTax > 0)
                            {
                                criteriaBuilder.CloseParenthesis();
                                criteriaBuilder.Or();
                                numberTax = 0;
                            }
                            criteriaBuilder.OpenParenthesis();
                            criteriaBuilder.PropertyEquals(TaxEntities.TaxCondition.Properties.TaxCode, "tx", auxTaxCode);
                            criteriaBuilder.And();
                            numberTax = numberTax + 1;
                        }
                        else
                        {
                            criteriaBuilder.And();
                        }

                        auxTaxCodeOne = auxTaxCode;

                        switch (taxAttribute.TaxAttributeDescription)
                        {
                            case "TAX_CONDITION_CODE":
                                criteriaBuilder.PropertyEquals(TaxEntities.TaxCondition.Properties.TaxConditionCode, "tc", conditionCode);
                                break;
                            case "TAX_CATEGORY_CODE":
                                criteriaBuilder.PropertyEquals(TaxEntities.TaxRate.Properties.TaxCategoryCode, categoryCode);
                                break;
                            case "STATE_CODE":
                                criteriaBuilder.PropertyEquals(TaxEntities.TaxRate.Properties.StateCode, stateCode);
                                break;
                            case "COUNTRY_CODE":
                                criteriaBuilder.PropertyEquals(TaxEntities.TaxRate.Properties.CountryCode, countryCode);
                                break;
                            case "ECONOMIC_ACTIVITY_CODE":
                                criteriaBuilder.PropertyEquals(TaxEntities.TaxRate.Properties.EconomicActivityTaxCode, economicActivity);
                                break;
                            case "BRANCH_CODE":
                                if (branchCode > 0)
                                {
                                    criteriaBuilder.PropertyEquals(TaxEntities.TaxRate.Properties.BranchCode, branchCode);
                                }
                                else
                                {
                                    criteriaBuilder.Property(TaxEntities.TaxRate.Properties.BranchCode);
                                    criteriaBuilder.IsNull();
                                }

                                break;
                            case "LINE_BUSINESS_CODE":
                                if (lineBusinessCode > 0)
                                {
                                    criteriaBuilder.PropertyEquals(TaxEntities.TaxRate.Properties.LineBusinessCode, lineBusinessCode);
                                }
                                else
                                {
                                    criteriaBuilder.Property(TaxEntities.TaxRate.Properties.LineBusinessCode);
                                    criteriaBuilder.IsNull();
                                }
                                break;
                        }

                        criteriaBuilder.CloseParenthesis();
                    }
                }
                if (numberTax > 0)
                {
                    criteriaBuilder.CloseParenthesis();
                    //numberTax = 0;
                }

                //Lista de impuestos
                List<IndividualTaxCategoryConditionDTO> individualTaxes = GetIndividualTaxesByCriteriaBuilder(criteriaBuilder);

                #endregion 


                #region CalculateTax

                foreach (IndividualTaxCategoryConditionDTO individualTax in individualTaxes)
                {
                    int baseConditionTaxCode = 0;

                    //Excepto retenciones
                    if (!Convert.ToBoolean(individualTax.IsRetention))
                    {
                        baseConditionTaxCode = Convert.ToInt32(individualTax.BaseConditionTaxId);

                        // tax con base imponible
                        if (baseConditionTaxCode == 0)
                        {
                            int rateTypeCode = Convert.ToInt32(individualTax.RateTypeId);
                            int taxCode = Convert.ToInt32(individualTax.TaxId);
                            double rate = Convert.ToDouble(individualTax.Rate);
                            int taxConditionCode = Convert.ToInt32(individualTax.TaxConditionId);
                            int enabled = Convert.ToInt32(individualTax.Enabled);
                            DateTime currentFrom = Convert.ToDateTime(individualTax.CurrentFrom);
                            double minBaseAmount = Convert.ToDouble(individualTax.MinBaseAmount);

                            if (enabled == 1 && currentFrom < DateTime.Now)
                            {
                                if (rateTypeCode == (int)RateType.FixedValue)
                                {
                                    rate = rate / exchangeRate;
                                }

                                decimal excemptionPct = GetExemptionPercentage(individualId, taxCode, DateTime.Now, countryCode, stateCode);

                                // calcula si supera la base minima
                                double itemAmount = 0;
                                if (minBaseAmount <= amount && taxConditionCode == 1)
                                {
                                    itemAmount = Calculate(rateTypeCode, rate, amount, excemptionPct);
                                }

                                DataRow dataRowTax = dataTableTax.NewRow();
                                dataRowTax["TaxConditionCode"] = individualTax.TaxConditionId;
                                dataRowTax["TaxCategoryCode"] = individualTax.TaxCategoryId;
                                dataRowTax["TaxCode"] = taxCode;
                                dataRowTax["Tax"] = individualTax.TaxDescription;
                                dataRowTax["Rate"] = rate;
                                dataRowTax["TaxAmountBase"] = amount;
                                dataRowTax["TaxValue"] = itemAmount;
                                dataTableTax.Rows.Add(dataRowTax);
                            }
                        }
                    }
                }

                #endregion

                #region CalculateTaxDep

                foreach (IndividualTaxCategoryConditionDTO individualTax in individualTaxes)
                {
                    //Excepto retenciones
                    if (!Convert.ToBoolean(individualTax.IsRetention))
                    {
                        int baseConditionTaxCode = Convert.ToInt32(individualTax.BaseConditionTaxId);

                        // tax con base imponible
                        if (baseConditionTaxCode > 0)
                        {
                            int rateTypeCode = Convert.ToInt32(individualTax.RateTypeId);
                            int taxCode = Convert.ToInt32(individualTax.TaxId);
                            double rate = Convert.ToDouble(individualTax.Rate);
                            int taxConditionCode = Convert.ToInt32(individualTax.TaxConditionId);
                            int enabled = Convert.ToInt32(individualTax.Enabled);
                            DateTime currentFrom = Convert.ToDateTime(individualTax.CurrentFrom);
                            double minBaseAmount = Convert.ToDouble(individualTax.MinBaseAmount);

                            if (enabled == 1 && currentFrom < DateTime.Now)
                            {
                                if (rateTypeCode == (int)RateType.FixedValue)
                                {
                                    rate = rate / exchangeRate;
                                }

                                double itemAmount = 0;
                                bool taxBaseCondition = false;
                                foreach (DataRow row in dataTableTax.Rows)
                                {
                                    if (baseConditionTaxCode == Convert.ToInt32(row["TaxCode"]))
                                    {
                                        amount = Convert.ToDouble(row["TaxValue"]);

                                        decimal excemptionPct = GetExemptionPercentage(individualId, taxCode, DateTime.Now, countryCode, stateCode);

                                        // calcula si supera la base minima
                                        itemAmount = 0;
                                        if (minBaseAmount <= amount && taxConditionCode == 1)
                                        {
                                            itemAmount = Calculate(rateTypeCode, rate, amount, excemptionPct);
                                        }
                                        taxBaseCondition = true;
                                    }
                                }

                                if (!taxBaseCondition)
                                {
                                    amount = 0;
                                    itemAmount = 0;
                                }

                                DataRow dataRowTax = dataTableTax.NewRow();
                                dataRowTax["TaxConditionCode"] = individualTax.TaxConditionId;
                                dataRowTax["TaxCategoryCode"] = individualTax.TaxCategoryId;
                                dataRowTax["TaxCode"] = taxCode;
                                dataRowTax["Tax"] = individualTax.TaxDescription;
                                dataRowTax["Rate"] = rate;
                                dataRowTax["TaxAmountBase"] = amount;
                                dataRowTax["TaxValue"] = itemAmount;
                                dataTableTax.Rows.Add(dataRowTax);
                            }
                        }
                    }
                }

                #endregion

                decimal totalTax = 0;

                foreach (DataRow dataRow in dataTableTax.Rows)
                {
                    totalTax = totalTax + Convert.ToDecimal(dataRow["TaxValue"]);
                }

                return totalTax;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetFactor
        /// </summary>
        /// <param name="rateTypeCode">The rate type code.</param>
        /// <returns></returns>
        private static decimal GetFactor(int rateTypeCode)
        {
            decimal factor = new decimal(1);
            decimal divider = new decimal(1);
            switch (rateTypeCode)
            {
                case 1:
                    divider = new decimal(100);
                    break;
                case 2:
                    divider = new decimal(1000);
                    break;
                case 3:
                    divider = new decimal(1);
                    break;
            }

            return factor / divider;
        }
        #endregion

        #region Economic Activity Tax

        /// <summary>
        /// Gets the economic activities tax.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public List<Models.EconomicActivityTax> GetEconomicActivitiesTax()
        {
            try
            {
                TaxDAO taxDAO = new TaxDAO();
                return taxDAO.GetEconomicActivitiesTax();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        #endregion
        #region tax components
        /// <summary>
        /// Gets the tax Componentes por id
        /// </summary>
        /// <param name="taxId">The tax identifier.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public TaxComponentDTO GetTaxComponentByTaxId(int taxId)
        {
            try
            {
                return TaxBusiness.GetTaxComponentByTaxId(taxId);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// Gets the tax components by tax ids.
        /// </summary>
        /// <param name="taxIds">The tax ids.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public List<TaxComponentDTO> GetTaxComponentsByTaxIds(List<int> taxIds)
        {
            try
            {
                return TaxBusiness.GetTaxComponentsByTaxIds(taxIds);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

        }

        /// <summary>
        /// Gets the tax components by tax ids.
        /// </summary>
        /// <param name="taxIds">The tax ids.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public List<TaxComponentDTO> GetTaxComponentsByComponentsIds(List<int> ComponentsIds)
        {
            try
            {
                return TaxBusiness.GetTaxComponentsByComponentsIds(ComponentsIds);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

        }

        /// <summary>
        /// Gets the tax payer ids.
        /// </summary>
        /// <param name="taxIds">The tax ids.</param>
        /// <param name="branchId">The branch identifier.</param>
        /// <param name="lineBusinessId">The line business identifier.</param>
        /// <returns></returns>
        public List<TaxPayerDTO> GetTaxPayerIds(List<int> taxIds, Int16 branchId, int lineBusinessId)
        {
            try
            {
                return TaxBusiness.GetTaxPayerIds(taxIds, branchId, lineBusinessId);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
        #endregion tax components
    }
}
