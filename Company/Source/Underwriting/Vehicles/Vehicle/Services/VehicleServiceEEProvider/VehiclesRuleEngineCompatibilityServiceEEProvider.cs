using Sistran.Company.Application.UniquePersonServices.V1.Models;
using Sistran.Company.Application.Vehicles.Models;
using Sistran.Company.Application.Vehicles.VehicleServices.EEProvider.DAOs;
using Sistran.Company.Application.Vehicles.VehicleServices.EEProvider.Entities;
using Sistran.Company.Application.Vehicles.VehicleServices.Models;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.Utilities.Cache;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using Type = System.Type;
using Rules = Sistran.Core.Framework.Rules;
using Sistran.Company.Application.Utilities.RulesEngine;
using Sistran.Core.Application.Utilities.RulesEngine;
using Sistran.Core.Framework.DAF;
using COMMON = Sistran.Core.Application.Common.Entities;
using Sistran.Core.Application.Vehicles.VehicleServices.Models;
using Sistran.Core.Application.Vehicles.VehicleServices.EEProvider;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;

namespace Sistran.Company.Application.Vehicles.VehicleServices.EEProvider
{
    public class VehiclesRuleEngineCompatibilityServiceEEProvider
    {
        VehicleServiceEEProviderCore vehiclerulecore = new VehicleServiceEEProviderCore();
        int parameterId = 0;

        #region GetCountBeneficiariesByType


        public IList GetCountBeneficiariesByType(IList parameters)
        {
            Rules.Facade facade = new Rules.Facade();
            parameterId = 2102;
            Parameter parameterOneroso = DelegateService.commonService.GetParameterByParameterId(parameterId);
            if (parameterOneroso != null && parameterOneroso.NumberParameter.HasValue)
            {
                int dynamicConceptId = parameterOneroso.NumberParameter.Value;
                int countBeneficiaries = facade.GetConcept<int>(CompanyRuleConceptRisk.BeneficiariesOnerousCount);
                facade.SetConcept(CompanyRuleConceptRisk.DynamicConcept(dynamicConceptId), countBeneficiaries);
            }
            parameterId = 2103;
            Parameter parameterNoOneroso = DelegateService.commonService.GetParameterByParameterId(parameterId);
            if (parameterNoOneroso != null && parameterNoOneroso.NumberParameter.HasValue)
            {
                int dynamicConceptId = parameterNoOneroso.NumberParameter.Value;
                int countBeneficiaries = facade.GetConcept<int>(CompanyRuleConceptRisk.BeneficiariesNoOnerousCount);
                facade.SetConcept(CompanyRuleConceptRisk.DynamicConcept(dynamicConceptId), countBeneficiaries);
            }
            parameters[0] = facade;
            return parameters;
        }

        #endregion GetCountBeneficiariesByType

        #region GetRenewalHistory

        public IList GetRenewalHistory(IList parameters)
        {
            int paramNewRenovated = 581; //concepto Nuevo/Renovado
            int paramRenewalNumber = 582; //concepto Nro Renovaciones
            int dynamicConceptNewRenovated = 0;
            int dynamicConceptRenewalNumber = 0;
            Rules.Facade facade = new Rules.Facade();
            int indexFacadeRiskVehicle = 0;

            Parameter parameter = DelegateService.commonService.GetParameterByParameterId(paramNewRenovated);

            if (parameter != null)
            {
                dynamicConceptNewRenovated = parameter.NumberParameter.GetValueOrDefault();
            }
            else
            {
                throw new BusinessException("PRPERR_PARAM_CONCEPT_DOES_NOT_EXIST", new string[] { paramNewRenovated.ToString() });
            }

            /*<<TODO AUTOR:FELIPE GRISALES; FECHA:31/03/2017; ASUNTO:SE INCLUYE REGISTRO EN CACHE Y VALIDACIÓN DE LOS PARAMETROS*/
            //parameter = CSPR.ParameterProvider.Find(paramRenewalNumber);
            parameter = DelegateService.commonService.GetParameterByParameterId(paramRenewalNumber);
            /*AUTOR:FELIPE GRISALES; FECHA:31/03/2017 >>*/
            if (parameter != null)
            {
                dynamicConceptRenewalNumber = parameter.NumberParameter.GetValueOrDefault();
            }
            else
            {
                throw new BusinessException("PRPERR_PARAM_CONCEPT_DOES_NOT_EXIST", new string[] { paramRenewalNumber.ToString() });
            }

            int newRenovated = 0;
            int renewallNumEnd = 0;
            RenewallHistory renewallHistory = facade.GetConcept<RenewallHistory>(CompanyRuleConceptGeneral.RenewallHistory);
            decimal distance = renewallHistory.Distance;
            int parameterDaysDiscontinuity = 1011;
            Parameter parameterDays = DelegateService.commonService.FindCoParameter(parameterDaysDiscontinuity);
            int daysDiscontinuity = parameterDays.NumberParameter.HasValue ? parameterDays.NumberParameter.Value : 0;
            decimal renewallNum = renewallHistory.RenewallNum.HasValue ? renewallHistory.RenewallNum.Value : 0;
            renewallNumEnd = Convert.ToInt32(renewallNum);
            if (distance >= daysDiscontinuity)
            {
                newRenovated = 1;
                renewallNumEnd += 1;
            }
            else
            {
                if (renewallHistory.NewRenewall == "RENOVADO")
                {
                    newRenovated = 1;
                }
                else
                {
                    newRenovated = 0;
                }
            }
            facade.SetConcept(RuleConceptGeneral.DynamicConcept(dynamicConceptNewRenovated), newRenovated);
            facade.SetConcept(RuleConceptGeneral.DynamicConcept(dynamicConceptRenewalNumber), renewallNumEnd);
            facade.SetConcept(CompanyRuleConceptGeneral.NewRenovated, newRenovated);
            parameters[indexFacadeRiskVehicle] = facade;


            return parameters;
        }

        #endregion GetRenewalHistory

        #region Eventos
        /// <summary>
        /// EVE.CO_GET_CHANGE_VEHICLE_TYPE
        /// </summary>
        /// <returns></returns>
        public void ValidateChangeVehicleType(Rules.Facade facade)
        {
            int currentType = facade.GetConcept<int>(CompanyRuleConceptRisk.VehicleTypeCode);

            PrimaryKey primaryKey = ISSEN.RiskVehicle.CreatePrimaryKey(facade.GetConcept<int>(CompanyRuleConceptRisk.RiskId));

            ISSEN.RiskVehicle entityRiskVehicle = (ISSEN.RiskVehicle)DataFacadeManager.GetObject(primaryKey);

            if (entityRiskVehicle != null)
            {
                if (entityRiskVehicle.VehicleTypeCode != currentType)
                {
                    facade.SetConcept(RuleConceptPolicies.GenerateEvent, true);
                }
            }
        }

        /// <summary>
        /// Evento:Cambio Codigo Fasecolda
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public void ValidateChangeFasecolda(Rules.Facade facade)
        {
            try
            {
                VehicleDAO vehicleDao = new VehicleDAO();
                CompanyVehicle vehicle = vehicleDao.GetVehicleByRiskId(facade.GetConcept<int>(CompanyRuleConceptRisk.RiskId));
                if (vehicle != null)
                {
                    if (facade.GetConcept<int>(CompanyRuleConceptRisk.VehicleMakeCode) != vehicle?.Make?.Id
                        || facade.GetConcept<int>(CompanyRuleConceptRisk.VehicleModelCode) != vehicle?.Model?.Id
                        || facade.GetConcept<int>(CompanyRuleConceptRisk.VehicleVersionCode) != vehicle?.Version?.Id)
                    {

                        facade.SetConcept(RuleConceptPolicies.GenerateEvent, true);
                    }
                }
                return;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="facade"></param>
        public void ValidateRestrictedAccessories(Rules.Facade facade)
        {
            const int parameterId = 1029;
            Parameter parameter = DelegateService.commonService.GetExtendedParameterByParameterId(parameterId);

            if (parameter != null && parameter.NumberParameter.HasValue)
            {
                List<CompanyAccessory> coaccessories = facade.GetConcept<List<CompanyAccessory>>(CompanyRuleConceptRisk.Accesories);
                if (coaccessories != null)
                {
                    if (coaccessories.Exists(x => x.Id == parameter.NumberParameter.Value))
                    {
                        facade.SetConcept(RuleConceptPolicies.GenerateEvent, true);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="facade"></param>
        public void ValidateAccessoriesCodes(Rules.Facade facade)
        {
            List<Accessory> accessories = vehiclerulecore.GetAccessories();
            List<CompanyAccessory> vehicleAccessories = facade.GetConcept<List<CompanyAccessory>>(CompanyRuleConceptRisk.Accesories);

            if (vehicleAccessories != null && vehicleAccessories.Count > 0)
            {
                foreach (CompanyAccessory vehicleAccessory in vehicleAccessories)
                {
                    if (!accessories.Exists(x => x.Id == vehicleAccessory.Id))
                    {
                        facade.SetConcept(RuleConceptPolicies.GenerateEvent, true);
                        break;
                    }
                }
            }
        }

        #endregion

        //funciones de R1     
        /// <summary>
        /// Código Limite RC
        /// </summary>
        /// <param name="facade"></param>
        public void GetCodLimitRC(Rules.Facade facade)
        {
            const int parameterId = 2116;
            Parameter parameter = DelegateService.commonService.GetParameterByParameterId(parameterId);
            if (parameter != null && parameter.NumberParameter.HasValue)
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(ISSEN.CoRisk.Properties.RiskId).Equal().Constant(facade.GetConcept<int>(CompanyRuleConceptRisk.RiskId));

                BusinessCollection<ISSEN.CoRisk> businessCollection = DataFacadeManager.Instance.GetDataFacade().List<ISSEN.CoRisk>(filter.GetPredicate());
                if (businessCollection != null && businessCollection.Count > 0)
                {
                    int RcCode = businessCollection[0].LimitsRcCode ?? 0;
                    facade.SetConcept(CompanyRuleConceptRisk.DynamicConcept(parameter.NumberParameter.Value), RcCode);
                }
            }
        }

        /// <summary>
        /// Ultimo Deducible PTD
        /// </summary>
        /// <param name="facade"></param>     
        public void GetLastDeductIdOfPTD(Rules.Facade facade)
        {
            const int parameterId = 2122;
            const int coveragePTDId = 5;

            Parameter parameter = DelegateService.commonService.GetParameterByParameterId(parameterId);
            if (parameter != null && parameter.NumberParameter.HasValue)
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.PropertyEquals(ISSEN.RiskCoverDeduct.Properties.RiskCoverId, coveragePTDId);
                BusinessCollection<ISSEN.RiskCoverDeduct> businessCollection = DataFacadeManager.Instance.GetDataFacade().List<ISSEN.RiskCoverDeduct>(filter.GetPredicate());
                decimal deductId = 0;
                if (businessCollection != null && businessCollection.Count > 0)
                {
                    deductId = businessCollection[0].DeductId ?? 0;
                }
                facade.SetConcept(CompanyRuleConceptRisk.DynamicConcept(parameter.NumberParameter.Value), deductId);

            }

        }

        /// <summary>
        /// Asigna Limite RC
        /// </summary>
        /// <param name="facade"></param>
        public void UpdateLimitRcCd(Rules.Facade facade)
        {
            const int parameterId = 2116;
            Parameter parameter = DelegateService.commonService.GetParameterByParameterId(parameterId);

            if (parameter != null && parameter.NumberParameter.HasValue)
            {
                facade.SetConcept(RuleConceptRisk.LimitsRcCode, facade.GetConcept<int>(CompanyRuleConceptRisk.DynamicConcept(parameter.NumberParameter.Value)));
            }
        }

        /// <summary>
        /// Asigna el valor del grupo de coberturas
        /// </summary>
        /// <param name="facade"></param>
        public void UpdateCoverageGroupCd(Rules.Facade facade)
        {
            const int parameterId = 2232;
            Parameter parameter = DelegateService.commonService.GetParameterByParameterId(parameterId);

            if (parameter != null && parameter.NumberParameter.HasValue)
            {
                facade.SetConcept(RuleConceptRisk.CoverageGroupId, facade.GetConcept<int>(CompanyRuleConceptRisk.DynamicConcept(parameter.NumberParameter.Value)));
            }
        }

        /// <summary>
        /// Deducible Automatico
        /// </summary>
        /// <param name="facade"></param>
        public void AssignDeductibleVehicle(Rules.Facade facade)
        {
            const int parameterId = 522;
            const int parameterDeductP = 2213;
            int? deductId = 0;
            Parameter parameter = DelegateService.commonService.GetParameterByParameterId(parameterId);
            if (parameter != null && parameter.NumberParameter.HasValue)
            {
                deductId = facade.GetConcept<int?>(RuleConceptCoverage.DynamicConcept(parameter.NumberParameter.Value));
                facade.SetConcept(RuleConceptCoverage.DeductId, deductId);
            }

            Parameter parameterDeduc = DelegateService.commonService.GetParameterByParameterId(parameterDeductP);
            if (parameterDeduc != null && parameterDeduc.NumberParameter.HasValue)
            {
                facade.SetConcept(CompanyRuleConceptRisk.DeductCoverages, parameterDeduc.TextParameter);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="facade"></param>
        public void CalculatePercentageAccessoriesAmount(Rules.Facade facade)
        {
            decimal accoriginalvalue = 0;
            decimal sumlimitamount = 0;
            decimal porcentaccoriginal = 0;
            const int parameterId = 10027;
            Parameter parameter = DelegateService.commonService.GetParameterByParameterId(parameterId);

            if (parameter != null && parameter.NumberParameter.HasValue)
            {
                List<CompanyCoverage> coverages = facade.GetConcept<List<CompanyCoverage>>(CompanyRuleConceptRisk.Coverages);

                foreach (CompanyCoverage c in coverages)
                {
                    //Se realiza la suma excluyendo los Id 
                    if (c.Id != 1 && c.Id != 72)
                    {
                        //Validar accesorios no originales
                        if (c.Id == 71)
                        {
                            //Almacenar valor de accesorios no originales
                            accoriginalvalue = c.LimitAmount;
                        }
                        else
                        {
                            //suma del Limit Amount
                            sumlimitamount = sumlimitamount + c.LimitAmount;
                        }
                    }
                }

                //Se calcula el porcentaje 
                if (sumlimitamount > 0)
                {
                    porcentaccoriginal = (accoriginalvalue * 100) / sumlimitamount;
                }
                facade.SetConcept(CompanyRuleConceptRisk.DynamicConcept(parameter.NumberParameter.Value), porcentaccoriginal);
            }
        }
    }


}
