//Sistran Company
using Sistran.Company.Application.CommonServices;

//Sistran Core
using Sistran.Core.Application.GeneralLedgerServices;
using GeneralLedgerModels = Sistran.Core.Application.GeneralLedgerServices.DTOs;

//Sistran FWK
using Sistran.Core.Framework.UIF.Web.Resources;
using Sistran.Core.Framework.UIF2.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using Sistran.Core.Framework.UIF.Web.Services;

namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models.Assemblers
{
    //public class ServiceModelAssembler
    //{
    //    #region Public Methods

    //    #region AccountingNaturesModel

    //    /// <summary>
    //    /// AccountingNatures
    //    /// </summary>
    //    /// <param name="accountingNatures"></param>
    //    /// <returns>List<AccountingNaturesModel></returns>
    //    public static List<AccountingNaturesModel> AccountingNatures(string[] accountingNatures)
    //    {
    //        return accountingNatures.Select((t, i) => new AccountingNaturesModel { AccountingNatureId = i + 1, Description = t }).ToList();
    //    }

    //    #endregion AccountingNaturesModel

    //    #region PostDatedModel

    //    /// <summary>
    //    /// PostDatedTypes
    //    /// </summary>
    //    /// <param name="postDatedType"></param>
    //    /// <returns>List<PostDatedTypeModel></returns>
    //    public static List<PostDatedTypeModel> PostDatedTypes(string[] postDatedType)
    //    {
    //        return postDatedType.Select((t, i) => new PostDatedTypeModel { Id = i + 1, Description = t }).ToList();
    //    }

    //    #endregion PostDatedModel

    //    #region EntryType

    //    /// <summary>
    //    /// GetEntryTypeModel
    //    /// </summary>
    //    /// <param name="entryType"></param>
    //    /// <returns>EntryTypeModel</returns>
    //    public static EntryTypeModel GetEntryTypeModel(GeneralLedgerModels.EntryType entryType)
    //    {
    //        List<EntryTypeAccountingModel> entryTypeAccountingModels = GetEntryTypeAccountings(entryType.EntryTypeItems, entryType.EntryTypeId);
    //        return new EntryTypeModel
    //        {
    //            EntryTypeId = entryType.EntryTypeId,
    //            EntryTypeDescription = entryType.Description,
    //            EntryTypeSmallDescription = entryType.SmallDescription,
    //            EntryTypeAccountingModels = entryTypeAccountingModels
    //        };
    //    }

    //    /// <summary>
    //    /// GetEntryTypeModels
    //    /// </summary>
    //    /// <param name="entryTypes"></param>
    //    /// <returns>List<EntryTypeModel></returns>
    //    public static List<EntryTypeModel> GetEntryTypeModels(List<GeneralLedgerModels.EntryType> entryTypes)
    //    {
    //        return entryTypes.Select(entryType => new EntryTypeModel { EntryTypeId = entryType.EntryTypeId, EntryTypeDescription = entryType.Description, EntryTypeSmallDescription = entryType.SmallDescription }).ToList();
    //    }

    //    /// <summary>
    //    /// GetEntryTypeAccountings
    //    /// </summary>
    //    /// <param name="entryTypeItems"></param>
    //    /// <param name="entryTypeId"></param>
    //    /// <returns>List<EntryTypeAccountingModel></returns>
    //    public static List<EntryTypeAccountingModel> GetEntryTypeAccountings(List<GeneralLedgerModels.EntryTypeItem> entryTypeItems, int entryTypeId)
    //    {
    //        List<EntryTypeAccountingModel> entryTypeAccountingModels = new List<EntryTypeAccountingModel>();

    //        if (entryTypeItems.Count > 0)
    //        {
    //            foreach (GeneralLedgerModels.EntryTypeItem entryTypeItem in entryTypeItems)
    //            {
    //                EntryTypeAccountingModel entryTypeAccountingModel = new EntryTypeAccountingModel();
    //                entryTypeAccountingModel.EntryTypeAccountingId = entryTypeItem.Id;
    //                entryTypeAccountingModel.EntryTypeCd = entryTypeId;
    //                entryTypeAccountingModel.Description = entryTypeItem.Description;
    //                entryTypeAccountingModel.AccountingNatureId = Convert.ToInt32(entryTypeItem.AccountingNature);
    //                entryTypeAccountingModel.AccountingNatureDescription = Convert.ToInt32(entryTypeItem.AccountingNature) == 1 ? @Global.Credits : @Global.Debits;
    //                entryTypeAccountingModel.CurrencyId = entryTypeItem.Currency.Id;
    //                entryTypeAccountingModel.CurrencyDescription = GetCurrencyDescriptionById(entryTypeItem.Currency.Id);
    //                entryTypeAccountingModel.ExchangeRate = DelegateService.commonService.GetExchangeRateByRateDateCurrencyId(DateTime.Now, entryTypeItem.Currency.Id).BuyAmount;
    //                entryTypeAccountingModel.AccountingAccountId = entryTypeItem.AccountingAccount.AccountingAccountId;
    //                entryTypeAccountingModel.AccountingAccountNumber = DelegateServoce.glAccountingApplicationService.GetAccountingAccount(entryTypeItem.AccountingAccount.AccountingAccountId).Number;
    //                entryTypeAccountingModel.AccountingAccountDescription = DelegateServoce.glAccountingApplicationService.GetAccountingAccount(entryTypeItem.AccountingAccount.AccountingAccountId).Description;
    //                entryTypeAccountingModel.AccountingMovementTypeId = entryTypeItem.AccountingMovementType.AccountingMovementTypeId;
    //                entryTypeAccountingModel.AccountingMovementTypeDescription = DelegateServoce.glAccountingApplicationService.GetAccountingMovementType(entryTypeItem.AccountingMovementType).Description;
    //                entryTypeAccountingModel.AnalysisId = entryTypeItem.Analysis.AnalysisId;
    //                entryTypeAccountingModel.AnalysisDescription = DelegateServoce.glAccountingApplicationService.GetAnalysisCode(entryTypeItem.Analysis.AnalysisId).Description;
    //                entryTypeAccountingModel.CostCenterId = entryTypeItem.CostCenter.CostCenterId;
    //                entryTypeAccountingModel.CostCenterDescription = DelegateServoce.glAccountingApplicationService.GetCostCenter(entryTypeItem.CostCenter).Description;
    //                entryTypeAccountingModel.PaymentConceptCd = entryTypeItem.AccountingConcept.Id;
    //                entryTypeAccountingModel.PaymentConceptDescription = DelegateServoce.glAccountingApplicationService.GetAccountingConcept(entryTypeItem.AccountingConcept).Description;

    //                entryTypeAccountingModels.Add(entryTypeAccountingModel);
    //            }
    //        }

    //        return entryTypeAccountingModels;
    //    }

    //    /// <summary>
    //    /// GetCurrencyDescriptionById
    //    /// Obtiene la descripción de la moneda dado su id
    //    /// </summary>
    //    /// <param name="currencyId"></param>
    //    /// <returns>string</returns>
    //    public static string GetCurrencyDescriptionById(int currencyId)
    //    {
    //        var currencies = DelegateService.commonService.GetCurrencies();
    //        var currencyNames = currencies.Where(sl => sl.Id == currencyId).ToList();

    //        return currencyNames[0].Description;
    //    }

    //    #endregion


    //    #endregion
    //}
}