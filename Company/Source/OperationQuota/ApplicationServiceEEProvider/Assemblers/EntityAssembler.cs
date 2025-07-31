using Sistran.Core.Application.Utilities.RulesEngine;
using Sistran.Core.Framework.Rules;
using System;
using System.Collections.Generic;
using AQENT = Sistran.Company.Application.AutomaticQuota.Entities;
using AQMOD = Sistran.Company.Application.OperationQuotaServices.EEProvider.Models.OperationQuota;

namespace Sistran.Company.Application.OperationQuotaCompanyServices.EEProvider.Assemblers

{
    public static class EntityAssembler
    {
        #region Facades
        public static void CreateFacadeGeneral(Facade facade, AQMOD.AutomaticQuota automaticQuota)
        {
            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.DocumentTypeProspect, automaticQuota.Prospect.DocumentType);
            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.DocumentNumberProspect, automaticQuota.Prospect.DocumentNumber);
            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.BusinessNameProspect, automaticQuota.Prospect.Businessname);
            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.CityIdProspect, automaticQuota.Prospect.City);
            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.AddressProspect, automaticQuota.Prospect.Address);
            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.AddressTypeProspect, automaticQuota.Prospect.AddressTypecd);
            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.PhoneProspect, automaticQuota.Prospect.Phone);
            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.EmailProspect, automaticQuota.Prospect.Email);
            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.ConstitutionDateProspect, automaticQuota.Prospect.ConstitutionDate);
            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.LegalRepresentProspect, automaticQuota.Prospect.LegalRepresentative);
            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.TaxAuditorProspect, automaticQuota.Prospect.FiscalReviewer);
            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.EconomicActivityQuota, automaticQuota.Prospect.EconomicActivity);
            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.ProgramQuota, automaticQuota.AgentProgram);
            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.IndividualTypeProspect, automaticQuota.Prospect.IndividualTypeCd);
            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.AdditionalInfoProspect, automaticQuota.Prospect.AdditionalInfo);
            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.CountryIdProspect, automaticQuota.Prospect.CountryCd);
            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.StateIdProspect, automaticQuota.Prospect.StateCd);
            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.CompanyTypeProspect, automaticQuota.Prospect.CompanytypeCd);
            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.SuggestedQuota, automaticQuota.SuggestedQuota);
            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.QuotaReconsideration, automaticQuota.QuotaReconsideration);
            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.LegalizedQuota, automaticQuota.LegalizedQuota);
            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.CurrentQuota, automaticQuota.CurrentQuota);
            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.CurrentCumulus, automaticQuota.CurrentCluster);
            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.QuotaDate, automaticQuota.QuotaPreparationDate);
            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.SolicitedBy, automaticQuota.RequestedById);
            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.UserElaborator, automaticQuota.ElaboratedId);
            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.GuaranteeTypeQuota, automaticQuota.TypeCollateral);
            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.GuaranteeStatusQuota, automaticQuota.CollateralStatus);
            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.QuotaObservation, automaticQuota.Observations);
            if (automaticQuota.Indicator!= null && automaticQuota.Indicator.Count > 0)
            {
                foreach (AQMOD.Indicator indicator in automaticQuota.Indicator)
                {
                    switch (indicator.ConceptIndicatorcd)
                    {
                        case (int)Enums.EnumIndicatorConcept.CAPITAL_TRABAJO:
                            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.InitialWorkingCapital, indicator.IndicatorIni);
                            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.EndWorkingCapital, indicator.IndicatorFin);
                            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.WorkingCapitalObservation, indicator.Observation);
                            break;
                        case (int)Enums.EnumIndicatorConcept.RAZON_CORRIENTE:
                            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.InitialCurrentRatio, indicator.IndicatorIni);
                            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.EndCurrentRatio, indicator.IndicatorFin);
                            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.CurrentRatioObservation, indicator.Observation);
                            break;
                        case (int)Enums.EnumIndicatorConcept.PRUEBA_ACIDA:
                            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.InitialAcidTest, indicator.IndicatorIni);
                            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.EndAcidTest, indicator.IndicatorFin);
                            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.AcidTestObservation, indicator.Observation);
                            break;
                        case (int)Enums.EnumIndicatorConcept.NIVEL_ENDEUDAMIENTO:
                            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.InitialDebtLevel, indicator.IndicatorIni);
                            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.EndDebtLevel, indicator.IndicatorFin);
                            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.DebtLevelObservation, indicator.Observation);
                            break;
                        case (int)Enums.EnumIndicatorConcept.ENDEUDAMIENTO_FINANCIERO:
                            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.InitialFinancialDebt, indicator.IndicatorIni);
                            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.EndFinancialDebt, indicator.IndicatorFin);
                            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.FinancialDebtObservation, indicator.Observation);
                            break;
                        case (int)Enums.EnumIndicatorConcept.APALANCAMIENTO:
                            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.InitialLeverage, indicator.IndicatorIni);
                            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.EndLeverage, indicator.IndicatorFin);
                            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.LeverageObservation, indicator.Observation);
                            break;
                        case (int)Enums.EnumIndicatorConcept.PATRIMONIO_ROE:
                            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.InitialROE, indicator.IndicatorIni);
                            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.EndROE, indicator.IndicatorFin);
                            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.RoeObservation, indicator.Observation);
                            break;
                        case (int)Enums.EnumIndicatorConcept.ACTIVO_ROA:
                            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.InitialROA, indicator.IndicatorIni);
                            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.EndROA, indicator.IndicatorFin);
                            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.RoaObservation, indicator.Observation);
                            break;
                        case (int)Enums.EnumIndicatorConcept.EBITDA:
                            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.InitialEBITDA, indicator.IndicatorIni);
                            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.EndEBITDA, indicator.IndicatorFin);
                            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.EBITDAObservation, indicator.Observation);
                            break;

                        default:
                            break;
                    }

                }
            }

            //conceptos ocultos
            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.YearsOfConstitution,(DateTime.Now.Year - automaticQuota.Prospect.ConstitutionDate.Year));
            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.RestrictiveListQualification, automaticQuota.RestrictiveList?.Qualification);
            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.RestrictiveListWeighted, automaticQuota.RestrictiveList?.Weighted);
            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.ScoreRestrictiveList, automaticQuota.RestrictiveList?.Score);
            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.CenterListRiskQualification, automaticQuota.RiskCenter?.Qualification);
            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.CenterListRiskWeighted, automaticQuota.RiskCenter?.Weighted);
            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.ScoreCenterListRisk, automaticQuota.RiskCenter?.Score);
            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.SignatureOrLetterQualification, automaticQuota.SignatureOrLetter?.Qualification);
            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.SignatureOrLetterWeighted, automaticQuota.SignatureOrLetter?.Weighted);
            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.ScorePFC, automaticQuota.SignatureOrLetter?.Score);
            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.SignatureOrLetterQualification, automaticQuota.SignatureOrLetter?.Qualification);
            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.SignatureOrLetterWeighted, automaticQuota.SignatureOrLetter?.Weighted);
            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.ScorePFC, automaticQuota.SignatureOrLetter?.Score);
            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.RunningReason, automaticQuota.CurrentReason?.Value);
            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.AcidTest, automaticQuota.AcidTest?.Value);
            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.Indebtedness, automaticQuota.Indebtedness?.Value);
            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.SalesGrowth, automaticQuota.SalesGrowth?.Value);
            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.MarginETIBDA, automaticQuota.Etibda?.Value);
            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.EquityVariation, automaticQuota.EquityVariation?.Value);
            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.NetIncomeVariation, automaticQuota.NetIncomeVariation?.Value);
            facade.SetConcept(RuleConceptAutomaticQuotaGeneral.AverageSales, automaticQuota.AverageSales?.Value);

            if (automaticQuota.DynamicProperties != null && automaticQuota.DynamicProperties.Count > 0)
            {
                facade.SetConcept(RuleConceptAutomaticQuotaGeneral.KnowledgeClient, automaticQuota.DynamicProperties?[0].Value);
                facade.SetConcept(RuleConceptAutomaticQuotaGeneral.CorporateGovernance, automaticQuota.DynamicProperties?[1].Value);
                facade.SetConcept(RuleConceptAutomaticQuotaGeneral.CustomerReputation, automaticQuota.DynamicProperties?[2].Value);
                facade.SetConcept(RuleConceptAutomaticQuotaGeneral.MoralSolvency, automaticQuota.DynamicProperties?[3].Value);
                facade.SetConcept(RuleConceptAutomaticQuotaGeneral.CustomerKnowledgeAccess, automaticQuota.DynamicProperties?[4].Value);
            }
            
        }

        public static void CreateFacadeThird(Facade facade, AQMOD.Third third)
        {
            facade.SetConcept(RuleConceptAutomaticQuotaThird.QueryCIFIN, third.CifinQuery);
            facade.SetConcept(RuleConceptAutomaticQuotaThird.PrincipalDebtor, third.PrincipalDebtor);
            facade.SetConcept(RuleConceptAutomaticQuotaThird.Codebtor, third.Cosigner);
            facade.SetConcept(RuleConceptAutomaticQuotaThird.TotalThird, third.Total);
            facade.SetConcept(RuleConceptAutomaticQuotaThird.RiskCenterListQuota, third.RiskCenter.Id);
            facade.SetConcept(RuleConceptAutomaticQuotaThird.RestrictiveList, third.Restrictive.Id);
            facade.SetConcept(RuleConceptAutomaticQuotaThird.ConsultPromissory, third.PromissoryNoteSignature.Id);
            facade.SetConcept(RuleConceptAutomaticQuotaThird.SISCONCReport, third.ReportListSisconc.Id);

        }
        public static void CreateFacadeBusiness(Facade facade, List<AQMOD.Utility> ListUtility)
        {
            foreach (AQMOD.Utility utility in ListUtility)
            {
                switch (utility.Id)
                {
                    case (int)Enums.EnumUtilityDetails.EFECTIVO_EQUIVALENTES:
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.InitialCashAndEquivalent, utility.Start_Values);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.EffectiveEndAndEquivalents, utility.End_value);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.RelativeCashAndEquivalent, utility.Var_Relativa);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.AbsoluteCashAndEquivalents, utility.Var_Abs);
                        break;
                    case (int)Enums.EnumUtilityDetails.INVENTARIOS:
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.InitialInventories, utility.Start_Values);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.FinalInventories, utility.End_value);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.RelativeInventories, utility.Var_Relativa);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.AbsoluteInventories, utility.Var_Abs);
                        break;
                    case (int)Enums.EnumUtilityDetails.CUENTAS_POR_COBRAR:
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.InitialAccountsReceivable, utility.Start_Values);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.FinalAccountsReceivable, utility.End_value);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.RelativeAccountsReceivable, utility.Var_Relativa);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.AbsoluteAccountsReceivable, utility.Var_Abs);
                        break;
                    case (int)Enums.EnumUtilityDetails.ACTIVO_CORRIENTE:
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.InitialCurrentAssets, utility.Start_Values);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.CurrentActiveEnd, utility.End_value);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.RelativeCurrentAssets, utility.Var_Relativa);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.AbsoluteCurrentAssets, utility.Var_Abs);
                        break;
                    case (int)Enums.EnumUtilityDetails.PROPIEDAD_PLATA_EQUIPO:
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.InitialPropertyPlantAndEquipment, utility.Start_Values);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.FinalPropertyPlantAndEquipment, utility.End_value);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.RelativeOwnershipPlantAndEquipment, utility.Var_Relativa);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.AbsolutePropertyPlantAndEquipment, utility.Var_Abs);
                        break;
                    case (int)Enums.EnumUtilityDetails.ACTIVO_NO_CORRIENTE:
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.InitialNonCurrentActive, utility.Start_Values);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.NonCurrentActiveEnd, utility.End_value);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.RelativeNonCurrentAssets, utility.Var_Relativa);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.AbsoluteNonCurrentAssets, utility.Var_Abs);
                        break;
                    case (int)Enums.EnumUtilityDetails.TOTAL_ACTIVO:
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.InitialTotalAssets, utility.Start_Values);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.FinalTotalActive, utility.End_value);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.RelativeTotalAssets, utility.Var_Relativa);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.AbsoluteTotalAssets, utility.Var_Abs);
                        facade.SetConcept(RuleConceptAutomaticQuotaGeneral.ActiveAverage, decimal.Round((utility.End_value / 100000), 2));


                        break;
                    case (int)Enums.EnumUtilityDetails.OBLIGACIONES_CORTO_PLAZO:
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.InitialShortTermFinancialObligations, utility.Start_Values);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.FinalShortTermFinancialObligations, utility.End_value);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.RelativeShortTermFinancialObligations, utility.Var_Relativa);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.AbsoluteShortTermFinancialObligations, utility.Var_Abs);
                        break;
                    case (int)Enums.EnumUtilityDetails.PROVEEDORES:
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.InitialSuppliers, utility.Start_Values);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.FinalSuppliers, utility.End_value);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.RelativeSuppliers, utility.Var_Relativa);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.AbsoluteSuppliers, utility.Var_Abs);
                        break;
                    case (int)Enums.EnumUtilityDetails.PASIVO_CORRIENTE:
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.InitialCurrentLiabilities, utility.Start_Values);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.EndCurrentLiabilities, utility.End_value);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.RelativeCurrentLiabilities, utility.Var_Relativa);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.AbsoluteCurrentLiabilities, utility.Var_Abs);
                        break;
                    case (int)Enums.EnumUtilityDetails.OBLIGACIONES_LARGO_PLAZO:
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.InitialLongTermFinancialObligations, utility.Start_Values);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.Finallongtermfinancialobligations, utility.End_value);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.RelativeLongTermFinancialObligations, utility.Var_Relativa);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.AbsoluteLongTermFinancialObligations, utility.Var_Abs);
                        break;
                    case (int)Enums.EnumUtilityDetails.PASIVO_NO_CORRIENTE:
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.InitialNonCurrentLiabilities, utility.Start_Values);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.EndOfNonCurrentLiabilities, utility.End_value);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.RelativeNonCurrentLiabilities, utility.Var_Relativa);
                        //facade.SetConcept(RuleConceptAutomaticQuotaBusiness.AbsoluteLongTermFinancialObligations, utility.Var_Abs);
                        break;
                    case (int)Enums.EnumUtilityDetails.TOTAL_PASIVO:
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.InitialTotalLiabilities, utility.Start_Values);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.FinalTotalPassive, utility.End_value);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.TotalRelativeLiabilities, utility.Var_Relativa);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.AbsoluteTotalLiability, utility.Var_Abs);
                        break;
                    case (int)Enums.EnumUtilityDetails.CAPITAL_SOCIAL:
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.InitialShareCapital, utility.Start_Values);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.FinalShareCapital, utility.End_value);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.RelativeShareCapital, utility.Var_Relativa);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.AbsoluteShareCapital, utility.Var_Abs);
                        break;
                    case (int)Enums.EnumUtilityDetails.RESERVAS_CAPITAL:
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.InitialCapitalReserves, utility.Start_Values);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.FinalCapitalReserves, utility.End_value);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.RelativeCapitalBuffers, utility.Var_Relativa);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.AbsoluteCapitalReserves, utility.Var_Abs);
                        break;
                    case (int)Enums.EnumUtilityDetails.RESULTADO_EJERCICIOS_ANTERIORES:
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.InitialResultsOfPreviousYears, utility.Start_Values);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.FinalResultsOfPreviousYears, utility.End_value);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.RelativeResultsOfPreviousYears, utility.Var_Relativa);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.AbsoluteResultsOfPreviousYears, utility.Var_Abs);
                        break;
                    case (int)Enums.EnumUtilityDetails.TOTAL_PATRIMONIO:
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.InitialTotalEquity, utility.Start_Values);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.FinalTotalHeritage, utility.End_value);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.RelativeTotalEquity, utility.Var_Relativa);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.AbsoluteTotalEquity, utility.Var_Abs);
                        facade.SetConcept(RuleConceptAutomaticQuotaGeneral.AverageEquity, decimal.Round((utility.End_value / 100000), 2));

                        break;
                    case (int)Enums.EnumUtilityDetails.INGRESOS_OPERACIONALES:
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.InitialOperatingIncome, utility.Start_Values);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.EndOperatingIncome, utility.End_value);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.RelativeOperatingIncome, utility.Var_Relativa);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.AbsoluteOperatingIncome, utility.Var_Abs);
                        break;
                    case (int)Enums.EnumUtilityDetails.COSTOS:
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.InitialCosts, utility.Start_Values);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.EndCosts, utility.End_value);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.RelativeCosts, utility.Var_Relativa);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.AbsoluteCosts, utility.Var_Abs);
                        break;
                    case (int)Enums.EnumUtilityDetails.UTILIDAD_BRUTA:
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.InitialGrossProfit, utility.Start_Values);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.EndGrossProfit, utility.End_value);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.RelativeGrossProfit, utility.Var_Relativa);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.AbsoluteGrossProfit, utility.Var_Abs);
                        break;
                    case (int)Enums.EnumUtilityDetails.COSTOS_GASTOS_ADMINISTRACION:
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.InitialCostsAndExpenses, utility.Start_Values);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.EndCostsAndExpenses, utility.End_value);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.RelativeCostsAndExpenses, utility.Var_Relativa);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.AbsoluteCostsAndExpenses, utility.Var_Abs);
                        break;
                    case (int)Enums.EnumUtilityDetails.UTILIDAD_OPERACIONAL:
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.InitialOperationalUtility, utility.Start_Values);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.EndOperationalUtility, utility.End_value);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.RelativeOperationalUtility, utility.Var_Relativa);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.AbsoluteOperationalUtility, utility.Var_Abs);
                        facade.SetConcept(RuleConceptAutomaticQuotaGeneral.AverageUtility, decimal.Round((utility.End_value / 100000), 2));

                        break;
                    case (int)Enums.EnumUtilityDetails.INGRESOS_GASTOS_NO_OPERACIONALES:
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.InitialIncomeAndExpensesNoOperating, utility.Start_Values);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.EndIncomeAndExpensesNoOperating, utility.End_value);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.RelativeIncomeAndExpensesNoOperating, utility.Var_Relativa);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.AbsoluteIncomeAndExpensesNoOperating, utility.Var_Abs);
                        break;
                    case (int)Enums.EnumUtilityDetails.UTILIDAD_ANTES_IMPUESTOS:
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.InitialIncomeBeforeTaxes, utility.Start_Values);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.EndIncomeBeforeTaxes, utility.End_value);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.RelativeIncomeBeforeTaxes, utility.Var_Relativa);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.AbsoluteIncomeBeforeTaxes, utility.Var_Abs);
                        break;
                    case (int)Enums.EnumUtilityDetails.PROVISION_IMPUESTOS:
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.InitialTaxProvition, utility.Start_Values);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.EndTaxProvition, utility.End_value);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.RelativeTaxProvition, utility.Var_Relativa);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.AbsoluteTaxProvition, utility.Var_Abs);
                        break;
                    case (int)Enums.EnumUtilityDetails.UTILIDAD_NETA:
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.InitialNetProfit, utility.Start_Values);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.EndNetProfit, utility.End_value);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.RelativeNetProfit, utility.Var_Relativa);
                        facade.SetConcept(RuleConceptAutomaticQuotaBusiness.AbsoluteNetProfit, utility.Var_Abs);
                        facade.SetConcept(RuleConceptAutomaticQuotaGeneral.AverageNetIncome, decimal.Round((utility.End_value / 100000), 2));

                        break;
                    default:
                        break;
                }


            }


        }

        #endregion Facades

        #region Guardado entidad
        public static AQENT.AutomaticQuotaOperation CreateEntityAutomaticQuotaOperation(AQMOD.AutomaticQuotaOperation companyOperation)
        {
            return new AQENT.AutomaticQuotaOperation(companyOperation.Id)
            {
                ParentId = companyOperation.ParentId,
                User = companyOperation.User,
                AutomaticOperationType = companyOperation.AutomaticOperationType,
                CreationDate = companyOperation.CreationDate,
                ModificationDate = companyOperation.ModificationDate,
                Operation = companyOperation.Operation,
            };
        }

        public static AQENT.AutomaticQuota CreateEntityAutomaticQuota(AQMOD.AutomaticQuota companyAutomatic)
        {
            return new AQENT.AutomaticQuota(companyAutomatic.AutomaticQuotaId)
            {
                AutomaticQuotaCode = companyAutomatic.AutomaticQuotaId,
                //IndicatorCode = companyAutomatic.Indicatorid,
                IndividualId = companyAutomatic.IndividualId,
                CustomerTypeCode = companyAutomatic.CustomerTypeId,
                //ThirdCode = companyAutomatic.Third.Id,
                SuggestedQuota = companyAutomatic.SuggestedQuota,
                QuotaReconsideration = companyAutomatic.QuotaReconsideration,
                LegalizedQuota = companyAutomatic.LegalizedQuota,
                CurrentQuota = companyAutomatic.CurrentQuota,
                CurrentCluster = companyAutomatic.CurrentCluster,
                QuotaDate = companyAutomatic.QuotaPreparationDate,
                //RequestedById = companyAutomatic.RequestedById,
                Observation = companyAutomatic.Observations,
                //GuaranteeStatusCode = companyAutomatic.
                //PreparedBy = companyAutomatic.Elaborated,

                // UtilityCode = companyAutomatic.Utility
                CountryCode = companyAutomatic.CountryId,
                CityCode = companyAutomatic.CityId,
                StateCode = companyAutomatic.StateId
            };
        }
        #endregion Guardado entidad
    }
}