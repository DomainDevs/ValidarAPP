using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models.Base;
using Sistran.Core.Application.Utilities.Enums;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.UnderwritingUtilities
{
    public class ModelAssembler
    {
        #region TempRiesgo


        public static DataTable GetDataTableTempRISK(CompanyRisk risk)
        {
            IDynamicPropertiesSerializer dynamicPropertiesSerializer =
               new DynamicPropertiesSerializer();
            DataTable dataTable = new DataTable("INSERT_TEMP_RISK");
            #region FirsParms

            #region Columns
            dataTable.Columns.Add("OPERATION_ID", typeof(int));
            dataTable.Columns.Add("TEMP_ID", typeof(int));
            dataTable.Columns.Add("INSURED_ID", typeof(int));
            dataTable.Columns.Add("CUSTOMER_TYPE_CD", typeof(int));
            dataTable.Columns.Add("COVERED_RISK_TYPE_CD", typeof(int));
            dataTable.Columns.Add("RISK_NUM", typeof(int));
            dataTable.Columns.Add("ENDORSEMENT_ID", typeof(int));
            dataTable.Columns.Add("POLICY_ID", typeof(int));
            dataTable.Columns.Add("RISK_STATUS_CD", typeof(int));
            dataTable.Columns.Add("RISK_ORIGINAL_STATUS_CD", typeof(int));
            dataTable.Columns.Add("RISK_INSP_TYPE_CD", typeof(int));
            dataTable.Columns.Add("INSPECTION_ID", typeof(int));
            dataTable.Columns.Add("CONDITION_TEXT", typeof(string));
            dataTable.Columns.Add("RATING_ZONE_CD", typeof(int));
            dataTable.Columns.Add("COVER_GROUP_ID", typeof(int));
            dataTable.Columns.Add("PREFIX_CD", typeof(int));
            dataTable.Columns.Add("IS_FACULTATIVE", typeof(bool));
            dataTable.Columns.Add("NAME_NUM", typeof(int));
            dataTable.Columns.Add("ADDRESS_ID", typeof(int));
            dataTable.Columns.Add("PHONE_ID", typeof(int));
            dataTable.Columns.Add("RISK_COMMERCIAL_CLASS_CD", typeof(int));
            dataTable.Columns.Add("RISK_COMMERCIAL_TYPE_CD", typeof(int));
            dataTable.Columns.Add("DYNAMIC_PROPERTIES", typeof(byte[]));
            dataTable.Columns.Add("SECONDARY_INSURED_ID", typeof(int));
            #endregion

            DataRow rows = dataTable.NewRow();

            #region DataRows
            rows["OPERATION_ID"] = risk.Id;
            rows["TEMP_ID"] = risk.Policy?.Endorsement?.TemporalId;
            rows["INSURED_ID"] = risk.MainInsured.IndividualId;
            rows["CUSTOMER_TYPE_CD"] = risk.MainInsured.CustomerType;
            rows["COVERED_RISK_TYPE_CD"] = risk.CoveredRiskType;
            rows["RISK_NUM"] = risk.Number;
            if (risk.Policy.Endorsement.Id > 0)
            {
                rows["ENDORSEMENT_ID"] = risk.Policy.Endorsement.Id;
            }
            rows["POLICY_ID"] = risk.Policy.Endorsement.PolicyId > 0 ? risk.Policy.Endorsement.PolicyId : rows["POLICY_ID"];
            rows["RISK_STATUS_CD"] = risk.Status;
            if (risk.OriginalStatus != null)
            {
                rows["RISK_ORIGINAL_STATUS_CD"] = risk.OriginalStatus;
            }
            rows["RISK_INSP_TYPE_CD"] = 1;

            rows["INSPECTION_ID"] = DBNull.Value;
            if (risk.Text != null)
            {
                if (risk.Text.TextBody != null)
                {
                    while (risk.Text.TextBody.IndexOf("'") > 0)
                    {
                        int posicion = risk.Text.TextBody.IndexOf("'");
                        string parte1 = risk.Text.TextBody.Substring(0, posicion);
                        parte1 = parte1 + " ";
                        string parte2 = risk.Text.TextBody.Substring(posicion + 1);
                        risk.Text.TextBody = parte1 + parte2;
                    }
                }
                rows["CONDITION_TEXT"] = risk.Text.TextBody;
            }
            if (risk.RatingZone == null)
            {
                rows["RATING_ZONE_CD"] = DBNull.Value;
            }
            else
            {
                rows["RATING_ZONE_CD"] = risk.RatingZone.Id;
            }


            rows["COVER_GROUP_ID"] = risk.GroupCoverage.Id;
            rows["PREFIX_CD"] = risk.Policy.Prefix.Id;
            if (risk.IsFacultative != null)
            {
                rows["IS_FACULTATIVE"] = risk.IsFacultative;
            }
            else
            {
                rows["IS_FACULTATIVE"] = false;
            }

           
            if (risk.MainInsured.CompanyName != null)
            {
                if (risk.MainInsured.CompanyName.NameNum > 0)
                {
                    rows["NAME_NUM"] = risk.MainInsured.CompanyName.NameNum;
                }
                if (risk.MainInsured.CompanyName.Address != null && risk.MainInsured.CompanyName.Address.Id > 0)
                {
                    rows["ADDRESS_ID"] = risk.MainInsured.CompanyName.Address.Id;
                }
                if (risk.MainInsured.CompanyName.Phone != null && risk.MainInsured.CompanyName.Phone.Id > 0)
                {
                    rows["PHONE_ID"] = risk.MainInsured.CompanyName.Phone.Id;
                }
            }
            rows["RISK_COMMERCIAL_CLASS_CD"] = DBNull.Value;
            rows["RISK_COMMERCIAL_TYPE_CD"] = DBNull.Value;
            if (risk.DynamicProperties != null && risk.DynamicProperties?.Count > 0)
            {
                DynamicPropertiesCollection dynamicCollectionPolicy = new DynamicPropertiesCollection();

                for (int i = 0; i < risk.DynamicProperties?.Count; i++)
                {
                    DynamicProperty dinamycProperty = new DynamicProperty();
                    dinamycProperty.Id = risk.DynamicProperties[i].Id;
                    dinamycProperty.Value = risk.DynamicProperties[i].Value;
                    dynamicCollectionPolicy[i] = dinamycProperty;
                }

                rows["DYNAMIC_PROPERTIES"] = dynamicPropertiesSerializer.Serialize(dynamicCollectionPolicy);//--Serialize;
            }
            if (risk.SecondInsured != null)
            {
                rows["SECONDARY_INSURED_ID"] = risk.SecondInsured.IndividualId;
            }

            #endregion

            dataTable.Rows.Add(rows);
            #endregion
            return dataTable;
        }

        public static DataTable GetDataTableCOTempRisk(CompanyRisk risk)
        {
            DataTable dataTable = new DataTable("INSERT_CO_TEMP_RISK");
            #region twoParams

            #region Columns
            dataTable.Columns.Add("LIMITS_RC_CD", typeof(int));
            dataTable.Columns.Add("LIMIT_RC_SUM", typeof(decimal));
            dataTable.Columns.Add("100_RETENTION", typeof(bool));
            dataTable.Columns.Add("SINISTER_PCT", typeof(decimal));
            dataTable.Columns.Add("HAS_SINISTER", typeof(bool));
            dataTable.Columns.Add("ASSISTANCE_CD", typeof(int));
            dataTable.Columns.Add("SINISTER_QTY", typeof(int));
            dataTable.Columns.Add("ACTUAL_DATE_MOVEMENT", typeof(DateTime));
            #endregion

            DataRow rows = dataTable.NewRow();

            #region Rows
            if (risk.LimitRc == null)
            {
                rows["LIMITS_RC_CD"] = DBNull.Value;
                rows["LIMIT_RC_SUM"] = DBNull.Value;

            }
            else
            {
                rows["LIMITS_RC_CD"] = risk.LimitRc.Id;
                rows["LIMIT_RC_SUM"] = risk.LimitRc.LimitSum;
            }
            rows["100_RETENTION"] = Convert.ToBoolean(risk.IsRetention);
            rows["SINISTER_PCT"] = DBNull.Value;
            rows["HAS_SINISTER"] = risk.HasSinister;
            if (risk?.AssistanceType?.Id == null)
            {
                rows["ASSISTANCE_CD"] = DBNull.Value;
            }
            else
            {
                rows["ASSISTANCE_CD"] = risk?.AssistanceType?.Id;
            }
            rows["SINISTER_QTY"] = DBNull.Value;
            rows["ACTUAL_DATE_MOVEMENT"] = (risk.ActualDateMovement == DateTime.MinValue) ? DateTime.Now : risk.ActualDateMovement;
            dataTable.Rows.Add(rows);
            #endregion

            #endregion
            return dataTable;
        }


        public static DataTable GetDataTableRiskBeneficiary(CompanyRisk risk)
        {
            DataTable dataTable = new DataTable("INSERT_TEMP_RISK_BENEFICIARY_ADD");
            #region BENEFICARY


            dataTable.Columns.Add("CUSTOMER_TYPE_CD", typeof(int));
            dataTable.Columns.Add("BENEFICIARY_ID", typeof(int));
            dataTable.Columns.Add("BENEFICIARY_TYPE_CD", typeof(int));
            dataTable.Columns.Add("BENEFICT_PCT", typeof(decimal));
            dataTable.Columns.Add("ADDRESS_ID", typeof(int));
            dataTable.Columns.Add("NAME_NUM", typeof(int));

            if (risk.Beneficiaries != null)
            {
                foreach (CompanyBeneficiary companyBeneficiary in risk.Beneficiaries)
                {
                    DataRow rows = dataTable.NewRow();

                    rows["CUSTOMER_TYPE_CD"] = companyBeneficiary.CustomerType;
                    rows["BENEFICIARY_ID"] = companyBeneficiary.IndividualId;
                    rows["BENEFICIARY_TYPE_CD"] = companyBeneficiary.BeneficiaryType.Id;
                    rows["BENEFICT_PCT"] = companyBeneficiary.Participation;
                    if (companyBeneficiary.CompanyName != null && companyBeneficiary.CompanyName.Address != null)
                    {
                        rows["ADDRESS_ID"] = companyBeneficiary.CompanyName.Address.Id;
                        rows["NAME_NUM"] = companyBeneficiary.CompanyName.NameNum;
                    }

                    dataTable.Rows.Add(rows);
                }
            }
            #endregion
            return dataTable;
        }

        public static DataTable GetDataTableRiskPayer(CompanyRisk risk)
        {
            DataTable dataTable = new DataTable("INSERT_TEMP_RISK_PAYER");
            #region PAYER
            dataTable.Columns.Add("PAYER_ID", typeof(int));
            dataTable.Columns.Add("CUSTOMER_TYPE_CD", typeof(int));
            dataTable.Columns.Add("PAYER_NUM", typeof(int));
            dataTable.Columns.Add("PREMIUM_PART_PCT", typeof(decimal));
            dataTable.Columns.Add("ENDORSEMENT_ID", typeof(int));

            DataRow rows = dataTable.NewRow();
            rows["PAYER_ID"] = risk.Policy.Holder.IndividualId;
            rows["CUSTOMER_TYPE_CD"] = risk.Policy.Holder.CustomerType;
            rows["PAYER_NUM"] = 1;
            rows["PREMIUM_PART_PCT"] = 100;
            if (risk.Policy.Endorsement.Id > 0)
            {
                rows["ENDORSEMENT_ID"] = risk.Policy.Endorsement.Id;
            }

            dataTable.Rows.Add(rows);
            #endregion
            return dataTable;
        }

        public static DataTable GetDataTableRiskClause(CompanyRisk risk)
        {
            DataTable dataTable = new DataTable("INSERT_TEMP_CLAUSE");
            dataTable.Columns.Add("CLAUSE_ID", typeof(int));
            dataTable.Columns.Add("ENDORSEMENT_ID", typeof(int));
            dataTable.Columns.Add("CLAUSE_STATUS_CD", typeof(int));
            dataTable.Columns.Add("CLAUSE_ORIG_STATUS_CD", typeof(int));

            if (risk.Clauses != null && risk.Clauses.Count > 0)
            {
                foreach (CompanyClause companyClause in risk.Clauses)
                {
                    DataRow rows = dataTable.NewRow();

                    rows["CLAUSE_ID"] = companyClause.Id;
                    if (risk.Policy.Endorsement?.Id > 0)
                    {
                        rows["ENDORSEMENT_ID"] = risk.Policy.Endorsement.Id;
                    }
                    rows["CLAUSE_STATUS_CD"] = DBNull.Value;
                    rows["CLAUSE_ORIG_STATUS_CD"] = DBNull.Value;

                    dataTable.Rows.Add(rows);
                }
            }
            return dataTable;
        }

        public static DataTable GetDataTableRiskCoverage(CompanyRisk risk)
        {
            IDynamicPropertiesSerializer dynamicPropertiesSerializer =
                new DynamicPropertiesSerializer();
            DataTable dataTable = new DataTable("INSERT_TEMP_RISK_COVERAGE_ALL");

            dataTable.Columns.Add("COVERAGE_ID", typeof(int));
            dataTable.Columns.Add("IS_DECLARATIVE", typeof(bool));
            dataTable.Columns.Add("IS_MIN_PREMIUM_DEPOSIT", typeof(bool));
            dataTable.Columns.Add("FIRST_RISK_TYPE_CD", typeof(int));
            dataTable.Columns.Add("CALCULATION_TYPE_CD", typeof(int));
            dataTable.Columns.Add("DECLARED_AMT", typeof(decimal));
            dataTable.Columns.Add("PREMIUM_AMT", typeof(decimal));
            dataTable.Columns.Add("LIMIT_AMT", typeof(decimal));
            dataTable.Columns.Add("SUBLIMIT_AMT", typeof(decimal));
            dataTable.Columns.Add("LIMIT_IN_EXCESS", typeof(decimal));
            dataTable.Columns.Add("LIMIT_OCCURRENCE_AMT", typeof(decimal));
            dataTable.Columns.Add("LIMIT_CLAIMANT_AMT", typeof(decimal));
            dataTable.Columns.Add("ACC_PREMIUM_AMT", typeof(decimal));
            dataTable.Columns.Add("ACC_LIMIT_AMT", typeof(decimal));
            dataTable.Columns.Add("ACC_SUBLIMIT_AMT", typeof(decimal));
            dataTable.Columns.Add("CURRENT_FROM", typeof(DateTime));
            dataTable.Columns.Add("RATE_TYPE_CD", typeof(int));
            dataTable.Columns.Add("RATE", typeof(decimal));
            dataTable.Columns.Add("CURRENT_TO", typeof(DateTime));
            dataTable.Columns.Add("COVER_NUM", typeof(int));
            dataTable.Columns.Add("RISK_COVER_ID", typeof(int));
            dataTable.Columns.Add("COVER_STATUS_CD", typeof(int));
            dataTable.Columns.Add("COVER_ORIGINAL_STATUS_CD", typeof(int));
            dataTable.Columns.Add("CONDITION_TEXT", typeof(string));
            dataTable.Columns.Add("ENDORSEMENT_LIMIT_AMT", typeof(decimal));
            dataTable.Columns.Add("ENDORSEMENT_SUBLIMIT_AMT", typeof(decimal));

            dataTable.Columns.Add("FLAT_RATE_PCT", typeof(decimal));
            dataTable.Columns.Add("CONTRACT_AMOUNT_PCT", typeof(decimal));
            dataTable.Columns.Add("DYNAMIC_PROPERTIES", typeof(byte[]));
            dataTable.Columns.Add("SHORT_TERM_PCT", typeof(decimal));
            dataTable.Columns.Add("MAIN_COVERAGE_ID", typeof(int));
            dataTable.Columns.Add("MAIN_COVERAGE_PCT", typeof(decimal));
            dataTable.Columns.Add("DIFF_MIN_PREMIUM_AMT", typeof(decimal));


            if (risk.Coverages != null)
            {
                foreach (CompanyCoverage companyCoverage in risk.Coverages)
                {
                    DataRow rows = dataTable.NewRow();

                    rows["COVERAGE_ID"] = companyCoverage.Id;
                    rows["IS_DECLARATIVE"] = companyCoverage.IsDeclarative;
                    rows["IS_MIN_PREMIUM_DEPOSIT"] = companyCoverage.IsMinPremiumDeposit;
                    rows["FIRST_RISK_TYPE_CD"] = (int)Sistran.Core.Application.UnderwritingServices.Enums.FirstRiskType.None;
                    rows["CALCULATION_TYPE_CD"] = companyCoverage.CalculationType.Value;
                    rows["DECLARED_AMT"] = companyCoverage.DeclaredAmount;
                    rows["PREMIUM_AMT"] = Math.Round(companyCoverage.PremiumAmount, 2);
                    rows["LIMIT_AMT"] = companyCoverage.LimitAmount;
                    rows["SUBLIMIT_AMT"] = companyCoverage.SubLimitAmount;
                    rows["LIMIT_IN_EXCESS"] = companyCoverage.ExcessLimit;
                    rows["LIMIT_OCCURRENCE_AMT"] = companyCoverage.LimitOccurrenceAmount;
                    rows["LIMIT_CLAIMANT_AMT"] = companyCoverage.LimitClaimantAmount;
                    rows["ACC_PREMIUM_AMT"] = companyCoverage.AccumulatedPremiumAmount;
                    rows["ACC_LIMIT_AMT"] = companyCoverage.AccumulatedLimitAmount;
                    rows["ACC_SUBLIMIT_AMT"] = companyCoverage.AccumulatedSubLimitAmount;
                    rows["CURRENT_FROM"] = companyCoverage.CurrentFrom;
                    rows["RATE_TYPE_CD"] = companyCoverage.RateType;
                    //rows["RATE"] = (object)companyCoverage.Rate ?? DBNull.Value;
                    if (companyCoverage.Rate == null)
                    {
                        rows["RATE"] = DBNull.Value;
                    }
                    else
                    {
                        rows["RATE"] = Math.Round((double)companyCoverage.Rate, 2);

                    }

                    if (companyCoverage.CurrentTo.GetValueOrDefault() != null && companyCoverage.CurrentTo.GetValueOrDefault() != DateTime.MinValue)
                    {
                        rows["CURRENT_TO"] = companyCoverage.CurrentTo;
                    }
                    else
                    {
                        rows["CURRENT_TO"] = DBNull.Value;
                    }
                    rows["COVER_NUM"] = companyCoverage.Number;
                    if (companyCoverage.RiskCoverageId > 0)
                    {
                        rows["RISK_COVER_ID"] = companyCoverage.RiskCoverageId;
                    }

                    if (companyCoverage.CoverStatus.HasValue)
                    {
                        rows["COVER_STATUS_CD"] = companyCoverage.CoverStatus.Value;
                    }
                    else
                    {
                        rows["COVER_STATUS_CD"] = CoverageStatusType.Original;
                    }
                    rows["COVER_ORIGINAL_STATUS_CD"] = DBNull.Value;
                    if (companyCoverage.Text != null)
                    {
                        rows["CONDITION_TEXT"] = companyCoverage.Text.TextBody;
                    }
                    else
                    {
                        rows["CONDITION_TEXT"] = DBNull.Value;
                    }
                    rows["ENDORSEMENT_LIMIT_AMT"] = companyCoverage.EndorsementLimitAmount;
                    rows["ENDORSEMENT_SUBLIMIT_AMT"] = companyCoverage.EndorsementSublimitAmount;
                    if (companyCoverage.FlatRatePorcentage > 0)
                    {
                        rows["FLAT_RATE_PCT"] = companyCoverage.FlatRatePorcentage;
                    }
                    rows["CONTRACT_AMOUNT_PCT"] = companyCoverage.ContractAmountPercentage;
                    if (companyCoverage.DynamicProperties != null && companyCoverage.DynamicProperties.Count > 0)
                    {
                        DynamicPropertiesCollection dynamicCollectionCoverage = new DynamicPropertiesCollection();
                        for (int i = 0; i < companyCoverage.DynamicProperties.Count(); i++)
                        {
                            DynamicProperty dinamycProperty = new DynamicProperty();
                            dinamycProperty.Id = companyCoverage.DynamicProperties[i].Id;
                            dinamycProperty.Value = companyCoverage.DynamicProperties[i].Value;
                            dynamicCollectionCoverage[i] = dinamycProperty;
                        }

                        byte[] serializedValuesCoverage = dynamicPropertiesSerializer.Serialize(dynamicCollectionCoverage);
                        rows["DYNAMIC_PROPERTIES"] = serializedValuesCoverage;
                    }
                    rows["SHORT_TERM_PCT"] = companyCoverage.ShortTermPercentage;
                    if (companyCoverage.MainCoverageId != null)
                    {
                        rows["MAIN_COVERAGE_ID"] = companyCoverage.MainCoverageId;
                    }
                    else
                    {
                        rows["MAIN_COVERAGE_ID"] = 0;
                    }

                    rows["MAIN_COVERAGE_PCT"] = companyCoverage.MainCoveragePercentage.GetValueOrDefault();
                    rows["DIFF_MIN_PREMIUM_AMT"] = companyCoverage.DiffMinPremiumAmount.GetValueOrDefault();
                    dataTable.Rows.Add(rows);
                }
            }

            return dataTable;
        }

        public static DataTable GetDataTableDeduct(CompanyRisk risk)
        {
            DataTable dtTableDeduct = new DataTable("INSERT_TEMP_RISK_COVER_DEDUCT");

            dtTableDeduct.Columns.Add("COVERAGE_ID", typeof(int));
            dtTableDeduct.Columns.Add("RATE_TYPE_CD", typeof(int));
            dtTableDeduct.Columns.Add("RATE", typeof(decimal));
            dtTableDeduct.Columns.Add("DEDUCT_PREMIUM_AMT", typeof(decimal));
            dtTableDeduct.Columns.Add("DEDUCT_VALUE", typeof(decimal));
            dtTableDeduct.Columns.Add("DEDUCT_UNIT_CD", typeof(int));
            dtTableDeduct.Columns.Add("DEDUCT_SUBJECT_CD", typeof(int));
            dtTableDeduct.Columns.Add("MIN_DEDUCT_VALUE", typeof(decimal));
            dtTableDeduct.Columns.Add("MIN_DEDUCT_UNIT_CD", typeof(int));
            dtTableDeduct.Columns.Add("MIN_DEDUCT_SUBJECT_CD", typeof(int));
            dtTableDeduct.Columns.Add("MAX_DEDUCT_VALUE", typeof(decimal));
            dtTableDeduct.Columns.Add("MAX_DEDUCT_UNIT_CD", typeof(int));
            dtTableDeduct.Columns.Add("MAX_DEDUCT_SUBJECT_CD", typeof(int));
            dtTableDeduct.Columns.Add("CURRENCY_CD", typeof(int));
            dtTableDeduct.Columns.Add("ACC_DEDUCT_AMT", typeof(decimal));
            dtTableDeduct.Columns.Add("DEDUCT_ID", typeof(int));

            if (risk.Coverages != null)
            {
                foreach (CompanyCoverage companyCoverage in risk.Coverages)
                {
                    DataRow rowDeduct = dtTableDeduct.NewRow();

                    if (companyCoverage.Deductible != null)
                    {
                        rowDeduct["COVERAGE_ID"] = companyCoverage.Id;
                        rowDeduct["RATE_TYPE_CD"] = companyCoverage.Deductible.RateType;
                        if (companyCoverage.Deductible.Rate == null)
                        {
                            rowDeduct["RATE"] = DBNull.Value;
                        }
                        else
                        {
                            rowDeduct["RATE"] = Math.Round((double)companyCoverage.Deductible.Rate, 2);

                        }
                        if (companyCoverage.Deductible != null)
                        {
                            rowDeduct["DEDUCT_PREMIUM_AMT"] = companyCoverage.Deductible.DeductPremiumAmount;
                            rowDeduct["DEDUCT_VALUE"] = companyCoverage.Deductible.DeductValue;
                        }
                        if (companyCoverage.Deductible.DeductibleUnit != null && companyCoverage.Deductible.DeductibleUnit.Id > -1)
                        {
                            rowDeduct["DEDUCT_UNIT_CD"] = companyCoverage.Deductible.DeductibleUnit.Id;
                        }
                        if (companyCoverage.Deductible.DeductibleSubject != null)
                        {
                            rowDeduct["DEDUCT_SUBJECT_CD"] = companyCoverage.Deductible.DeductibleSubject.Id;
                        }
                        if (companyCoverage.Deductible.MinDeductValue.HasValue)
                        {
                            rowDeduct["MIN_DEDUCT_VALUE"] = companyCoverage.Deductible.MinDeductValue.Value;
                        }
                        if (companyCoverage.Deductible.MinDeductibleUnit != null && companyCoverage.Deductible.MinDeductibleUnit.Id >-1)
                        {
                            rowDeduct["MIN_DEDUCT_UNIT_CD"] = companyCoverage.Deductible.MinDeductibleUnit.Id;
                        }
                        if (companyCoverage.Deductible.MinDeductibleSubject != null )
                        {
                            rowDeduct["MIN_DEDUCT_SUBJECT_CD"] = companyCoverage.Deductible.MinDeductibleSubject.Id;
                        }
                        if (companyCoverage.Deductible.MaxDeductValue.HasValue)
                        {
                            rowDeduct["MAX_DEDUCT_VALUE"] = companyCoverage.Deductible.MaxDeductValue.Value;
                        }
                        if (companyCoverage.Deductible.MaxDeductibleUnit != null && companyCoverage.Deductible.MaxDeductibleUnit.Id >-1)
                        {
                            rowDeduct["MAX_DEDUCT_UNIT_CD"] = companyCoverage.Deductible.MaxDeductibleUnit.Id;
                        }
                        if (companyCoverage.Deductible.MaxDeductibleSubject != null )
                        {
                            rowDeduct["MAX_DEDUCT_SUBJECT_CD"] = companyCoverage.Deductible.MaxDeductibleSubject.Id;
                        }
                        if (companyCoverage.Deductible.Currency != null)
                        {
                            rowDeduct["CURRENCY_CD"] = companyCoverage.Deductible.Currency.Id;
                        }
                        rowDeduct["ACC_DEDUCT_AMT"] = companyCoverage.Deductible.AccDeductAmt;
                        rowDeduct["DEDUCT_ID"] = companyCoverage.Deductible.Id;
                        dtTableDeduct.Rows.Add(rowDeduct);
                    }
                }
            }
            return dtTableDeduct;
        }

        public static DataTable GetDataTableCoverClause(CompanyRisk risk)
        {
            DataTable dataTable = new DataTable("INSERT_TEMP_RISK_COVER_CLAUSE");

            dataTable.Columns.Add("COVERAGE_ID", typeof(int));
            dataTable.Columns.Add("CLAUSE_ID", typeof(int));
            dataTable.Columns.Add("CLAUSE_STATUS_CD", typeof(int));
            dataTable.Columns.Add("CLAUSE_ORIG_STATUS_CD", typeof(int));

            if (risk.Coverages != null)
            {
                foreach (CompanyCoverage companyCoverage in risk.Coverages)
                {
                    if (companyCoverage.Clauses != null)
                    {
                        foreach (CompanyClause companyClause in companyCoverage.Clauses)
                        {
                            DataRow rowCoverClause = dataTable.NewRow();

                            rowCoverClause["COVERAGE_ID"] = companyCoverage.Id;
                            rowCoverClause["CLAUSE_ID"] = companyClause.Id;
                            rowCoverClause["CLAUSE_STATUS_CD"] = DBNull.Value;
                            rowCoverClause["CLAUSE_ORIG_STATUS_CD"] = DBNull.Value;

                            dataTable.Rows.Add(rowCoverClause);
                        }
                    }
                }
            }
            return dataTable;
        }

        public static DataTable GetDataTableDynamic(CompanyRisk risk)
        {
            DataTable dtTableDynamicProperties = new DataTable("PARAM_TEMP_DYNAMIC_PROPERTIES_GENERAL");


            dtTableDynamicProperties.Columns.Add("DYNAMIC_ID", typeof(int));
            dtTableDynamicProperties.Columns.Add("ENTITY_ID", typeof(int));
            dtTableDynamicProperties.Columns.Add("CONCEPT_VALUE", typeof(string));
            dtTableDynamicProperties.Columns.Add("QUESTION_ID", typeof(int));

            if (risk.DynamicProperties != null)
            {
                foreach (DynamicConcept item in risk.DynamicProperties)
                {
                    if (item.Value != null)
                    {
                        DataRow dataRow = dtTableDynamicProperties.NewRow();
                        dataRow["DYNAMIC_ID"] = item.Id;
                        dataRow["ENTITY_ID"] = item.EntityId;
                        dataRow["CONCEPT_VALUE"] = item.Value;

                        if (item.QuestionId.HasValue)
                        {
                            dataRow["QUESTION_ID"] = item.QuestionId;
                        }
                        else
                        {
                            dataRow["QUESTION_ID"] = DBNull.Value;
                        }

                        dtTableDynamicProperties.Rows.Add(dataRow);
                    }

                }
            }

            return dtTableDynamicProperties;
        }

        public static DataTable GetDataTableDynamicCoverage(CompanyRisk risk)
        {
            DataTable dataTable = new DataTable("PARAM_TEMP_DYNAMIC_PROPERTIES_COVERGE");

            dataTable.Columns.Add("DYNAMIC_ID", typeof(int));
            dataTable.Columns.Add("COVERAGE_ID", typeof(int));
            dataTable.Columns.Add("ENTITY_ID", typeof(int));
            dataTable.Columns.Add("CONCEPT_VALUE", typeof(string));
            dataTable.Columns.Add("QUESTION_ID", typeof(int));

            if (risk.Coverages != null)
            {
                foreach (CompanyCoverage coverage in risk.Coverages)
                {
                    if (coverage.DynamicProperties != null)
                    {
                        foreach (DynamicConcept dynamicConcept in coverage.DynamicProperties)
                        {
                            if (dynamicConcept.Value != null)
                            {
                                DataRow dataRow = dataTable.NewRow();
                                dataRow["DYNAMIC_ID"] = dynamicConcept.Id;
                                dataRow["COVERAGE_ID"] = coverage.Id;
                                dataRow["ENTITY_ID"] = dynamicConcept.EntityId;
                                dataRow["CONCEPT_VALUE"] = dynamicConcept.Value;

                                if (dynamicConcept.QuestionId.HasValue)
                                {
                                    dataRow["QUESTION_ID"] = dynamicConcept.QuestionId;
                                }
                                else
                                {
                                    dataRow["QUESTION_ID"] = DBNull.Value;
                                }

                                dataTable.Rows.Add(dataRow);
                            }

                        }
                    }
                }
            }

            return dataTable;
        }

        #endregion
    }
}
