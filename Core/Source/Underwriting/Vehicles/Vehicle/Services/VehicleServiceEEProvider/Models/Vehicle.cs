using UnderModels = Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.BAF;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.Utilities.RulesEngine;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Services.UtilitiesServices.Enums;

namespace Sistran.Core.Application.Vehicles.VehicleServices.EEProvider.BusinessModels
{
    public class Vehicle
    {
        private static int? coverageIdAccNoOriginal;
        private static List<int> coverageIdsFromRate;
        private static Parameter _parameter;

        public static int CoverageIdAccNoOriginal
        {
            get
            {
                if (!coverageIdAccNoOriginal.HasValue)
                {
                    Parameter parameter = DelegateService.commonServiceCore.GetExtendedParameterByParameterId((int)ExtendedParametersTypes.NonOriginalAccessories);
                    if (parameter != null)
                    {
                        coverageIdAccNoOriginal = parameter.NumberParameter;
                    }
                }
                return coverageIdAccNoOriginal.GetValueOrDefault();
            }
        }
        public static List<int> CoverageIdsFromRate
        {
            get
            {
                if (coverageIdsFromRate == null)
                {
                    Parameter parameter = DelegateService.commonServiceCore.GetParameterByParameterId((int)ParametersTypes.RateAccessories);
                    if (parameter != null && !string.IsNullOrEmpty(parameter.TextParameter))
                    {
                        try
                        {
                            coverageIdsFromRate = parameter.TextParameter.Split('|').Select(x => int.Parse(x)).ToList();
                        }
                        catch
                        {
                            coverageIdsFromRate = new List<int>();
                        }
                    }
                    else
                    {
                        coverageIdsFromRate = new List<int>();
                    }
                }
                return coverageIdsFromRate;
            }
        }

        public static Parameter parameter
        {
            get
            {
                if (_parameter == null)
                {
                    _parameter = DelegateService.commonServiceCore.GetParameterByParameterId((int)ParametersTypes.CoverageHeritage);
                }
                return _parameter;
            }
        }
        
        Models.Vehicle VehicleModel;

        public Vehicle(Models.Vehicle vehicle)
        {
            this.VehicleModel = vehicle;
        }
        
        public void RunRules(int ruleSetId)
        {
            //if (FacadeGeneral == null)
            //{
            //    throw new BusinessException("FacadeGeneral no esta definido");
            //}
            //if (FacadeVehicle == null)
            //{
            //    FacadeVehicle = new FacadeRiskVehicle();
            //}

            //List<FacadeBasic> facts = new List<FacadeBasic>();
            //facts.Add(FacadeVehicle);
            //facts.Add(FacadeGeneral);
            //RulesEngineDelegate rulesEngineDelegate = new RulesEngineDelegate((IList)facts, ruleSetId);
            //FacadeVehicle = (FacadeRiskVehicle)rulesEngineDelegate.OutFacade;
        }

        public Models.Vehicle Quotate()
        {
            VehicleModel.Risk.Premium = 0;
            BusinessModels.Coverage BusinessCoverage;

            foreach (var coverage in VehicleModel.Risk.Coverages)
            {
                BusinessCoverage = new BusinessModels.Coverage(coverage,0,0);
                
                if (coverage.RuleSetId.GetValueOrDefault(0) > 0)
                {
                    BusinessCoverage.RunRules(coverage.RuleSetId.Value);
                }

                if (coverage.PosRuleSetId.GetValueOrDefault(0) > 0)
                {
                    BusinessCoverage.RunRules(coverage.PosRuleSetId.Value);
                }

                BusinessCoverage.Quotate(2);
                VehicleModel.Risk.Premium += coverage.PremiumAmount;
            }

            #region Coberturas a eliminar
            //foreach (int coverageId in FacadeVehicle.CoveragesDelete)
            //{
            //    UnderModels.Coverage coverdelete = VehicleModel.Risk.Coverages.Where(x => x.Id == coverageId).FirstOrDefault();
            //    if (coverdelete != null)
            //    {
            //        VehicleModel.Risk.Premium -= coverdelete.PremiumAmount;
            //        VehicleModel.Risk.Coverages.Remove(coverdelete);
            //    }
            //}
            #endregion
            //#region Coberturas a agregar
            //foreach (UnderModels.Coverage coverage in FacadeVehicle.CoveragesAdd)
            //{
            //    if (!VehicleModel.Coverages.Exists(x => x.Id == coverage.Id))
            //    {
            //        coverage.Risk = (UnderModels.Risk)this.VehicleModel;
            //        BusinessCoverage = new BusinessModels.Coverage(coverage,0,0);
            //        BusinessCoverage.FacadeGeneral = FacadeGeneral;
            //        BusinessCoverage.FacadeVehicle = FacadeVehicle;
            //        if (coverage.RuleSetId.GetValueOrDefault(0) > 0)
            //        {
            //            BusinessCoverage.RunRules(coverage.RuleSetId.Value);
            //        }

            //        if (coverage.PosRuleSetId.GetValueOrDefault(0) > 0)
            //        {
            //            BusinessCoverage.RunRules(coverage.PosRuleSetId.Value);
            //        }

            //        BusinessCoverage.Quotate();
            //        coverage.Risk = null;

            //        VehicleModel.Premium += coverage.PremiumAmount;
            //        VehicleModel.Coverages.Add(coverage);
            //    }
            //}
            //#endregion

            //Calculo de tasa de cobertura Accesorios no originales(CoverageId=71)
            UnderModels.Coverage coverageAccessory = VehicleModel.Risk.Coverages.Where(x => x.Id == CoverageIdAccNoOriginal).FirstOrDefault();
            if (coverageAccessory != null && VehicleModel.Accesories != null && VehicleModel.Accesories.Count > 0)
            {
                VehicleModel.Risk.Premium -= coverageAccessory.PremiumAmount;
                decimal amountAccessory = VehicleModel.Accesories.Where(x => !x.IsOriginal).Sum(x => x.Amount);

                coverageAccessory.DeclaredAmount = amountAccessory;
                coverageAccessory.LimitAmount = amountAccessory;
                coverageAccessory.SubLimitAmount = amountAccessory;
                coverageAccessory.LimitOccurrenceAmount = amountAccessory;
                coverageAccessory.LimitClaimantAmount = amountAccessory;
                coverageAccessory.EndorsementLimitAmount = amountAccessory;
                coverageAccessory.EndorsementSublimitAmount = amountAccessory;

                if (VehicleModel.Rate > 0)
                {
                    coverageAccessory.Rate = VehicleModel.Rate;
                }
                else
                {
                    coverageAccessory.Rate = VehicleModel.Risk.Coverages.Where(x => CoverageIdsFromRate.Contains(x.Id)).Sum(x => x.Rate.GetValueOrDefault());
                }
                coverageAccessory.RateType = RateType.Percentage;

                BusinessCoverage = new BusinessModels.Coverage(coverageAccessory,0,0);
                BusinessCoverage.Quotate(2);
                VehicleModel.Risk.Premium += coverageAccessory.PremiumAmount;
                foreach (Models.Accessory ma in VehicleModel.Accesories.Where(x => !x.IsOriginal))
                {
                    ma.Rate = coverageAccessory.Rate == null ? 0 : (decimal)coverageAccessory.Rate;
                    if (ma.Amount != 0)
                    {
                        ma.Premium = decimal.Round(ma.Amount * coverageAccessory.PremiumAmount / coverageAccessory.LimitAmount, QuoteManager.DecimalRound);
                    }
                }
            }

            //Prima Proteccion patrimonial
            if (parameter != null && VehicleModel.Risk.Coverages.Exists(x => x.Id == parameter.NumberParameter))
            {
                UnderModels.Coverage coveragePatrimonial = VehicleModel.Risk.Coverages.First(x => x.Id == parameter.NumberParameter);
                VehicleModel.Risk.Premium -= coveragePatrimonial.PremiumAmount;

                decimal premiumPatrimonial = VehicleModel.Risk.Coverages.Where(x => parameter.TextParameter.Split('|').Select(y => int.Parse(y)).Contains(x.Id)).Sum(x => x.PremiumAmount);
                premiumPatrimonial = (premiumPatrimonial * parameter.PercentageParameter.GetValueOrDefault()) / 100;

                coveragePatrimonial.PremiumAmount = decimal.Round(premiumPatrimonial, QuoteManager.DecimalRound);
                coveragePatrimonial.Rate = decimal.Round(premiumPatrimonial, 4);
                coveragePatrimonial.RateType = RateType.FixedValue;

                VehicleModel.Risk.Premium += coveragePatrimonial.PremiumAmount;
            }

            return VehicleModel;
        }


    }
}
