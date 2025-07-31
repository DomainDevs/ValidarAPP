using Sistran.Core.Application.Utilities.Cache;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Company.Application.UnderwritingServices.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Sistran.Core.Application.RulesScriptsServices.Enums;
using Sistran.Core.Application.Utilities.Enums;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Company.Application.QuotationServices.EEProvider
{

    public static class LoadData
    {
        static LoadData()
        {
        }

        public static void Load()
        {
            try
            {
                DictionaryCache.LoadCache();
                if (DelegateService.rulesService != null)
                    DelegateService.rulesService.LoadScripts();

            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("QuotationCache", ex.Message, EventLogEntryType.Error);
            }


        }

        public static void LoadProduct()
        {
            DelegateService.commonService.GetCountries();
            DataFacadeManager.Dispose();
        }

        public static void LoadDictionaries()
        {
            try
            {
                int intDefault = 1;
                string stringDefatult = "-1";
                object objectDefault = null;
                string stringFasecoldaDefault = "02208047";
                int objAuto = 7;
                int objCumplimiento = 2;



                TP.Task.Run(() =>
                {
                    DelegateService.uniqueUserService.GetUserByIndividualId(intDefault);
                    DataFacadeManager.Dispose();
                });
                TP.Task.Run(() =>
                {
                    DelegateService.vehicleService.GetVehicleByFasecoldaCode(stringFasecoldaDefault, intDefault);
                    DataFacadeManager.Dispose();
                });
                TP.Task.Run(() =>
                {
                    DelegateService.commonService.GetCurrencies();
                    DataFacadeManager.Dispose();
                });
                TP.Task.Run(() =>
                {
                    DelegateService.vehicleService.GetUses();
                    DataFacadeManager.Dispose();
                });
                TP.Task.Run(() =>
                {
                    DelegateService.vehicleService.GetColors();
                    DataFacadeManager.Dispose();
                });
                TP.Task.Run(() =>
                {
                    DelegateService.underwritingService.GetRatingZonesByPrefixId(intDefault);
                    DataFacadeManager.Dispose();
                });
                TP.Task.Run(() =>
                {
                    DelegateService.underwritingService.GetLimitRcById(intDefault);
                    DataFacadeManager.Dispose();
                });
                TP.Task.Run(() =>
                {
                    DelegateService.productService.GetProductByProductIdPrefixId(intDefault, intDefault);
                    DataFacadeManager.Dispose();
                });
                TP.Task.Run(() =>
                {
                    DelegateService.productService.GetProducts(intDefault);
                    DataFacadeManager.Dispose();
                });
                TP.Task.Run(() =>
                {
                    DelegateService.uniquePersonService.GetAgencyByAgentIdAgentAgencyId(intDefault, intDefault);
                    DataFacadeManager.Dispose();
                });
                TP.Task.Run(() =>
                {
                    DelegateService.uniquePersonService.GetAgenciesByAgentIdDescription(intDefault, stringDefatult);
                    DataFacadeManager.Dispose();
                });
                TP.Task.Run(() =>
                {
                    DelegateService.productService.ExistProductAgentByAgentIdPrefixIdProductId(intDefault, intDefault, intDefault);
                    DataFacadeManager.Dispose();
                });
                TP.Task.Run(() =>
                {
                    DelegateService.productService.GetCommissByAgentIdAgencyIdProductId(intDefault, intDefault, intDefault);
                    DataFacadeManager.Dispose();
                });
                TP.Task.Run(() =>
                {
                    DelegateService.commonService.GetCountries();
                    DataFacadeManager.Dispose();
                });
                TP.Task.Run(() =>
                {
                    DelegateService.commonService.GetCities();
                    DataFacadeManager.Dispose();
                });
                TP.Task.Run(() =>
                {
                    DelegateService.commonService.GetStates();
                    DataFacadeManager.Dispose();
                });
                TP.Task.Run(() =>
                {
                    DelegateService.underwritingService.GetFinancialPlanByProductId(intDefault);
                    DataFacadeManager.Dispose();
                });
                TP.Task.Run(() =>
                {
                    DelegateService.underwritingService.GetPaymentPlanByPaymentPlanId(intDefault);
                    DataFacadeManager.Dispose();
                });
                TP.Task.Run(() =>
                {
                    DelegateService.commonService.GetExtendedParameterByParameterId(intDefault);
                    DataFacadeManager.Dispose();
                });
                TP.Task.Run(() =>
                {
                    DelegateService.commonService.GetModuleDateIssue(intDefault, DateTime.Now);
                    DataFacadeManager.Dispose();
                });
                TP.Task.Run(() =>
                {
                    DelegateService.uniquePersonService.GetPaymentMethodByIndividualId(intDefault);
                    DataFacadeManager.Dispose();
                });
                TP.Task.Run(() =>
                {
                    DelegateService.underwritingService.GetCoveragesByProductIdGroupCoverageIdPrefixId(intDefault, intDefault, intDefault);
                    DataFacadeManager.Dispose();
                });
                TP.Task.Run(() =>
                {
                    DelegateService.underwritingService.GetClausesByClauseIds(new List<int> { intDefault });
                    DataFacadeManager.Dispose();
                });
                TP.Task.Run(() =>
                {
                    DelegateService.underwritingService.GetLimitsRcByPrefixIdProductIdPolicyTypeId(intDefault * -1, intDefault * -1, intDefault * -1);
                    DataFacadeManager.Dispose();
                });
                TP.Task.Run(() =>
                {
                    DelegateService.underwritingService.GetPaymentDistributionByPaymentPlanId(intDefault);
                    DataFacadeManager.Dispose();
                });
                TP.Task.Run(() =>
                {
                    DelegateService.underwritingService.GetDeductiblesByCoverageId(intDefault);
                    DataFacadeManager.Dispose();
                });
                TP.Task.Run(() =>
                {
                    DelegateService.uniquePersonService.GetPaymentMethodAccountByIndividualId(intDefault * -1);
                    DataFacadeManager.Dispose();
                });
                TP.Task.Run(() =>
                {
                    DelegateService.commonService.GetPolicyTypesByPrefixId(intDefault);
                    DataFacadeManager.Dispose();
                });
                TP.Task.Run(() =>
                {
                    DelegateService.commonService.GetPolicyTypesByProductId(intDefault);
                    DataFacadeManager.Dispose();
                });
                TP.Task.Run(() =>
                {
                    DelegateService.underwritingService.GetCiaCurrentStatusPolicyByEndorsementIdIsCurrent(intDefault, true);
                    DataFacadeManager.Dispose();
                });
                TP.Task.Run(() =>
                {
                    DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(objAuto, intDefault, objCumplimiento);
                    DataFacadeManager.Dispose();
                });

                TP.Task.Run(() =>
                {
                    CompanyPolicy obj = new CompanyPolicy();
                    DelegateService.underwritingService.CreateFacadeGeneral(obj);
                    DataFacadeManager.Dispose();
                });

                TP.Task.Run(() =>
                {
                    DelegateService.SarlaftApplicationServices.GetSarlaft(0);
                    DataFacadeManager.Dispose();
                });

                TP.Task.Run(() =>
                {
                    DelegateService.underwritingService.GetClausesByEmissionLevelConditionLevelId(Core.Application.UnderwritingServices.Enums.EmissionLevel.General, objCumplimiento);
                    DataFacadeManager.Dispose();
                });

                TP.Task.Run(() =>
                {
                    DelegateService.utilitiesServiceCore.EnableCrossGuarantees();
                    DataFacadeManager.Dispose();
                });
                TP.Task.Run(() =>
                {
                    DelegateService.suretyService.GetAvailableAmountByIndividualId(intDefault, objAuto, DateTime.Now);
                    DataFacadeManager.Dispose();
                });
                Core.Framework.Rules.Facade facade = new Core.Framework.Rules.Facade();
                TP.Task.Run(() =>
                {
                    DelegateService.AuthorizationPoliciesServiceCore.ValidateAuthorizationPolicies(10, "2,7", facade, FacadeType.RULE_FACADE_RISK);
                    DataFacadeManager.Dispose();
                });

                TP.Task.Run(() =>
                {
                    DelegateService.vehicleService.GetCompanyVehiclesByPolicyId(intDefault);
                    DataFacadeManager.Dispose();
                });

                TP.Task.Run(() =>
                {
                    DelegateService.underwritingService.GetCompanyPolicyByEndorsementId(intDefault);
                    DataFacadeManager.Dispose();
                });
                TP.Task.Run(() =>
                {
                    DelegateService.vehicleService.GetPremiumAccesory(intDefault, intDefault, intDefault, false);
                    DataFacadeManager.Dispose();
                });
                TP.Task.Run(() =>
                {
                    DelegateService.utilitiesServiceCore.GetEndorsementControlById(intDefault, intDefault);
                    DataFacadeManager.Dispose();
                });
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", ex.StackTrace + "\n\n" + ex.Message, EventLogEntryType.Error);
            }
            finally
            {
                DataFacadeManager.Dispose();
            }
        }
    }
}
