using Sistran.Core.Application.TaxServices.DTOs;
using System;
using System.Collections.Generic;
using System.ServiceModel;


namespace Sistran.Core.Application.TaxServices
{
    /// <summary>
    /// 
    /// </summary>
    [ServiceContract]
    public interface ITaxService
    {
        [OperationContract]
        void GetNumber();
        //conjunto de cambios

        /// <summary>
        /// Get impuestos
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Models.Tax> GetTax();


        /// <summary>
        /// Obtiene las condiciones de los impuestos
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Models.TaxCondition> GetTaxCondition();

        /// <summary>
        /// Obtiene las condiciones de los impuestos por id de impuesto
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Models.TaxCondition> GetTaxConditionByTaxId(int taxId);

        ////
        [OperationContract]
        List<Models.TaxCategory> GetTaxCategory();

        /// <summary>
        /// Obtiene las condiciones de los impuestos por id de impuesto
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Models.TaxCategory> GetTaxCategoryByTaxId(int taxId);


        /// <summary>
        /// Obtiene impuestos por individuo
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Models.Tax> GetTaxesByIndividualId(int IndividualId);

        #region Tax

        /// <summary>        
        /// Llena el combo de impuestos, desarrollado para General Ledger
        /// </summary>
        /// <returns></returns>
        //[OperationContract]
        //List<Models.Tax> GetTaxes();
        // Validar si cambiamos por  List<Models.Tax> GetTax();

        /// <summary>
        /// GetIndividualTaxCategoryCondition
        /// </summary>
        /// <param name="individualId">individualId</param>
        /// <param name="roleId">roleId</param>
        /// <returns>List<IndividualTaxCategoryConditionDTO></returns>
        [OperationContract]
        List<IndividualTaxCategoryConditionDTO> GetIndividualTaxCategoryCondition(int individualId, int? roleId = 0);

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
        [OperationContract]
        decimal GetTotalTax(int individualId, int conditionCode, Dictionary<int, int> categories, int branchCode, int lineBusinessCode,
                             double exchangeRate, double amount);

        [OperationContract]
        List<TaxCategoryConditionDTO> CalculatePaymentTaxesByIndividualIdAccountingConceptIdTaxAttributesAmount(int individualId, int accontingConceptId, List<TaxAttributeDTO> taxAttributes, decimal amount);

        [OperationContract]
        List<TaxAttributeDTO> GetTaxAttributesByTaxId(int taxId);

        [OperationContract]
        List<TaxAttributeDTO> GetTaxAttributes();

        [OperationContract]
        IndividualTaxExemptionDTO GetIndividualTaxExemptionByIndividualId(int individualId, int taxCode, DateTime currentFrom);
        #endregion

        #region AccountingCoceptTax
        [OperationContract]
        List<AccountingConceptTaxDTO> CreateAccountingConceptTaxes(List<AccountingConceptTaxDTO> accountingConceptTaxes);

        [OperationContract]
        void DeleteAccountingConceptTax(int accountingConceptTaxId);

        [OperationContract]
        List<AccountingConceptTaxDTO> GetAccountingConceptTaxesByAccountingConceptIdBranchId(int accountingConceptId, int branchId);
        #endregion
        #region Economic activity
        [OperationContract]
        List<Models.EconomicActivityTax> GetEconomicActivitiesTax();
        #endregion
        #region tax components
        /// <summary>
        /// Gets the tax Componentes por id
        /// </summary>
        /// <param name="taxId">The tax identifier.</param>
        /// <returns></returns>
        [OperationContract]
        TaxComponentDTO GetTaxComponentByTaxId(int taxId);

        /// <summary>
        /// Gets the tax components by tax ids.
        /// </summary>
        /// <param name="taxIds">The tax ids.</param>
        /// <returns></returns>
        [OperationContract]
        List<TaxComponentDTO> GetTaxComponentsByTaxIds(List<int> taxIds);

        /// <summary>
        /// Gets the tax components by components ids.
        /// </summary>
        /// <param name="ComponentsIds">The components ids.</param>
        /// <returns></returns>
        [OperationContract]
        List<TaxComponentDTO> GetTaxComponentsByComponentsIds(List<int> ComponentsIds);

        /// <summary>
        /// Gets the tax payer ids.
        /// </summary>
        /// <param name="taxIds">The tax ids.</param>
        /// <param name="branchId">The branch identifier.</param>
        /// <param name="lineBusinessId">The line business identifier.</param>
        /// <returns></returns>
        [OperationContract]
        List<TaxPayerDTO> GetTaxPayerIds(List<int> taxIds, Int16 branchId, int lineBusinessId);
        #endregion tax components
    }
}
