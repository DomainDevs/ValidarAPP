using System;
using System.Collections.Generic;

namespace Domain.Models.Entities
{
    public interface IEntityBasic
    {
        void Load(System.Data.DataRow row);
    }
    public class EntityBasicClass : EntityBasic { }
    public abstract class EntityBasic : IEntityBasic
    {
        public string TableName { get; set; }
        public void Load(System.Data.DataRow row)
        {
            foreach (System.Data.DataColumn c in row.Table.Columns)
            {
                // find the property for the column
                System.Reflection.PropertyInfo p = this.GetType().GetProperty(c.ColumnName);

                // if exists, set the value
                if (p != null && row[c] != DBNull.Value)
                {
                    try
                    {
                        p.SetValue(this, row[c], null);
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e);
                    }
                }
            }
        }
    }
    public class COMMBRANCH : EntityBasic
    {
        public string SUCURSAL { get; set; }
        public byte BRANCH_CD { get; set; }
        public string DESCRIPTION { get; set; }
        public string SMALL_DESCRIPTION { get; set; }
        public decimal BRANCH_TYPE_CD { get; set; }
    }
    public class COMMPREFIX : EntityBasic
    {
        public string RAMO { get; set; }
        public Int16 PREFIX_CD { get; set; }
        public string DESCRIPTION { get; set; }
        public string SMALL_DESCRIPTION { get; set; }
        public string TINY_DESCRIPTION { get; set; }
        public bool HAS_DETAIL_COMMISS { get; set; }
        public byte PROFIT_CENTER_CD { get; set; }
        public byte PREFIX_TYPE_CD { get; set; }
        public Int16? RULE_SET_ID { get; set; }
    }

    public class ISSPOLICY : ISSPolicy { }
    public class ISSPolicy : EntityBasic
    {
        public string QueryName { get; set; }
        public Int32 POLICY_ID { get; set; }
        public decimal DOCUMENT_NUM { get; set; }
        public byte BRANCH_CD { get; set; }
        public Int16 PREFIX_CD { get; set; }
        public byte CURRENCY_CD { get; set; }
        public DateTime ISSUE_DATE { get; set; }
        public bool IS_POLICYHOLDER_BILL { get; set; }
        public Int32? POLICYHOLDER_ID { get; set; }
        public DateTime CURRENT_FROM { get; set; }
        public DateTime? CURRENT_TO { get; set; }
        public Int32? PREV_POLICY_ID { get; set; }
        public Int32? NEXT_POLICY_ID { get; set; }
        public Int16? MAIL_ADDRESS_ID { get; set; }
        public decimal SALE_POINT_CD { get; set; }
        public Int32? PRODUCT_ID { get; set; }
        public byte? BILLING_PERIOD_CD { get; set; }
        public Int32? BILLING_GROUP_CD { get; set; }
        public Int32? POLICY_FOLDER_ID { get; set; }
        public Int32? SUBSCRIPTION_REQ_ID { get; set; }
        public Int16? MAIL_AGENT_ADDRESS_ID { get; set; }
        public Int32? MAIL_AGENT_IND_ID { get; set; }
        public Int32? POLICY_NUM_1G { get; set; }
        public bool CALCULATE_MIN_PREMIUM { get; set; }
        public byte BUSINESS_TYPE_CD { get; set; }
        public decimal? COISSUE_PCT { get; set; }
        public Int16? NAME_NUM { get; set; }
    }
    public class ISSCOPOLICY : ISSCOPolicy { }
    public class ISSCOPolicy : EntityBasic
    {
        public string QueryName { get; set; }
        public Int32 POLICY_ID { get; set; }
        public Int16 PREFIX_CD { get; set; }
        public Int16 POLICY_TYPE_CD { get; set; }
        public Int32? REQUEST_ID { get; set; }
        public decimal? CORRELATIVE_POLICY { get; set; }
        public int? REQUEST_ENDORSEMENT_ID { get; set; }
        public int? REQUEST_BENEF_HOLDER_ID { get; set; }
        public bool? IS_REQUEST { get; set; }
        public int? BUSINESS_ID { get; set; }
        public int? GROUP_QUOTATION_ID { get; set; }
        public bool? ACTIVE_QUOTATION { get; set; }
        public byte? JUSTIFICATION_REASON_CD { get; set; }
    }
    public class ISSENDORSEMENT : EntityBasic
    {
        public string QueryName { get; set; }
        public int ENDORSEMENT_ID { get; set; }
        public int POLICY_ID { get; set; }
        public int DOCUMENT_NUM { get; set; }
        public DateTime ISSUE_DATE { get; set; }
        public decimal EXCHANGE_RATE { get; set; }
        public Int16 ENDO_TYPE_CD { get; set; }
        public int USER_ID { get; set; }
        public DateTime BEGIN_DATE { get; set; }
        public DateTime CURRENT_FROM { get; set; }
        public DateTime? CURRENT_TO { get; set; }
        public DateTime? COMMIT_DATE { get; set; }
        public DateTime? PRINTED_DATE { get; set; }
        public int? QUOTATION_ID { get; set; }
        public string CONDITION_TEXT { get; set; }
        public Int16? ENDO_REASON_CD { get; set; }
        public int? SUBSCRIPTION_REQ_ID { get; set; }
        public string ANNOTATIONS { get; set; }
        public Int16? CAPACITY_OF_CD { get; set; }
        public byte?[] DYNAMIC_PROPERTIES { get; set; }
        public bool IS_MASSIVE { get; set; }
    }
    public class ISSCO_ENDORSEMENT : EntityBasic
    {
        public string QueryName { get; set; }
        public int ENDORSEMENT_ID { get; set; }
        public int POLICY_ID { get; set; }
        public Int16 PROFILE_ID { get; set; }
        public int? REF_ENDORSEMENT_ID { get; set; }
    }

    public class ISSCPT_ENDORSEMENT : EntityBasic
    {
        public string QueryName { get; set; }
        public int POLICY_ID { get; set; }
        public int ENDORSEMENT_ID { get; set; }
        public string EXTERNAL_POLICY_NUMBER { get; set; }
        public int ALLIANCE_ID { get; set; }
        public int BRANCH_ID { get; set; }
        public int SALE_POINT_ID { get; set; }
        public decimal INCENTIVE_AMT { get; set; }
        public int POLICY_2G { get; set; }
    }

    public class ISSGROUP_ENDORSEMENT : EntityBasic
    {
        public string QueryName { get; set; }
        public int ENDO_GROUP_ID { get; set; }
        public int ENDORSEMENT_ID { get; set; }
        public int POLICY_ID { get; set; }
        public Int16 ENDO_TYPE_CD { get; set; }
        public int? REF_ENDORSEMENT_ID { get; set; }
        public string TEXT_REASON { get; set; }
    }
    public class ISSCOINSURANCE_ACCEPT_ACCEPTED : EntityBasic
    {
        public string QueryName { get; set; }
        public int ENDORSEMENT_ID { get; set; }
        public int POLICY_ID { get; set; }
        public decimal INSURANCE_COMPANY_ID { get; set; }
        public decimal PART_CIA_PCT { get; set; }
        public decimal EXPENSES_PCT { get; set; }
        public decimal PART_MAIN_PCT { get; set; }
        public string ANNEX_NUM_MAIN { get; set; }
        public string POLICY_NUM_MAIN { get; set; }
    }

    public class ISSCOINSURANCE_ASSIGNED : EntityBasic
    {
        public string QueryName { get; set; }
        public int ENDORSEMENT_ID { get; set; }
        public int POLICY_ID { get; set; }
        public decimal INSURANCE_COMPANY_ID { get; set; }
        public decimal PART_CIA_PCT { get; set; }
        public decimal EXPENSES_PCT { get; set; }
        public Int16 COMPANY_NUM { get; set; }
    }
    public class ISSPOLICY_AGENT : EntityBasic
    {
        public string QueryName { get; set; }
        public int INDIVIDUAL_ID { get; set; }
        public Int16 AGENT_AGENCY_ID { get; set; }
        public int ENDORSEMENT_ID { get; set; }
        public int POLICY_ID { get; set; }
        public bool IS_PRIMARY { get; set; }
        public bool IS_ORG_AGENT { get; set; }
    }
    public class ISSCOMMISS_AGENT : EntityBasic
    {
        public string QueryName { get; set; }
        public int COMMISS_AGENT_ID { get; set; }
        public int INDIVIDUAL_ID { get; set; }
        public Int16 AGENT_AGENCY_ID { get; set; }
        public int POLICY_ID { get; set; }
        public int ENDORSEMENT_ID { get; set; }
        public byte COMMISS_NUM { get; set; }
        public decimal AGENT_PART_PCT { get; set; }
        public decimal ST_COMISSION_PCT { get; set; }
        public decimal ADDIT_COMMISS_PCT { get; set; }
        public decimal ST_DISC_COMMISS_PCT { get; set; }
        public decimal ADDIT_DISC_COMMISS_PCT { get; set; }
        public Int16? LINE_BUSINESS_CD { get; set; }
        public Int16? SUB_LINE_BUSINESS_CD { get; set; }
        public bool IN_UNITS { get; set; }
        public decimal? INC_COMMISS_AD_FAC_PCT { get; set; }
        public decimal? DIM_COMMISS_AD_FAC_PCT { get; set; }
        public decimal? SCH_COMMISS_PCT { get; set; }
    }

    public class ISSPOLICY_CLAUSE : EntityBasic
    {
        public string QueryName { get; set; }
        public int POLICY_ID { get; set; }
        public int ENDORSEMENT_ID { get; set; }
        public int CLAUSE_ID { get; set; }
        public Int16 CLAUSE_STATUS_CD { get; set; }
        public bool IS_CURRENT { get; set; }
    }

    public class ISSENDORSEMENT_PAYER : EntityBasic
    {
        public string QueryName { get; set; }
        public int ENDORSEMENT_ID { get; set; }
        public int POLICY_ID { get; set; }
        public int PAYER_ID { get; set; }
        public decimal SURCHARGE_PCT { get; set; }
        public int PAYMENT_SCHEDULE_ID { get; set; }
        public byte PAYMENT_METHOD_CD { get; set; }
        public Int16? MAIL_ADDRESS_ID { get; set; }
        public Int16? PAYMENT_ID { get; set; }
        public DateTime? AGT_PAY_EXP_DATE { get; set; }
        public bool IS_BY_PAYMENT_UPDATE { get; set; }
    }
    public class ISSFIRST_PAY_COMP : EntityBasic
    {
        public string QueryName { get; set; }
        public int ENDORSEMENT_ID { get; set; }
        public int POLICY_ID { get; set; }
        public byte COMPONENT_CD { get; set; }
        public int PAYER_ID { get; set; }
    }
    public class ISSPAYER_PAYMENT : EntityBasic
    {
        public string QueryName { get; set; }
        public int ENDORSEMENT_ID { get; set; }
        public int POLICY_ID { get; set; }
        public byte PAYMENT_NUM { get; set; }
        public int PAYER_ID { get; set; }
        public DateTime PAY_EXP_DATE { get; set; }
        public decimal? PAYMENT_PCT { get; set; }
        public decimal AMOUNT { get; set; }
        public DateTime? AGT_PAY_EXP_DATE { get; set; }
    }

    public class ISSCO_PAYER_PAYMENT_COMP : EntityBasic
    {
        public string QueryName { get; set; }
        public int ENDORSEMENT_ID { get; set; }
        public int POLICY_ID { get; set; }
        public int PAYER_ID { get; set; }
        public byte PAYMENT_NUM { get; set; }
        public byte COMPONENT_CD { get; set; }
        public decimal PAYMENT_COMP_PCT { get; set; }
        public decimal COMPONENT_COMP_AMT { get; set; }
        public DateTime? DATE_PAYMENT { get; set; }
    }

    public class ISSCO_PAYER_PAYMENT_COMPSum : EntityBasic
    {
        public string QueryName { get; set; }
        public int POLICY_ID { get; set; }
        public int ENDORSEMENT_ID { get; set; }
        public byte COMPONENT_CD { get; set; }
        public int SumResul { get; set; }
    }
    public class ISSRISK : EntityBasic
    {
        public string QueryName { get; set; }
        public int RISK_ID { get; set; }
        public int INSURED_ID { get; set; }
        public byte COVERED_RISK_TYPE_CD { get; set; }
        public string CONDITION_TEXT { get; set; }
        public byte? RATING_ZONE_CD { get; set; }
        public byte? COVER_GROUP_ID { get; set; }
        public bool IS_FACULTATIVE { get; set; }
        public Int16? NAME_NUM { get; set; }
        public Int16? ADDRESS_ID { get; set; }
        public Int16? PHONE_ID { get; set; }
        public Int16? RISK_COMMERCIAL_CLASS_CD { get; set; }
        public Int16? RISK_COMMERCIAL_TYPE_CD { get; set; }
        public byte?[] DYNAMIC_PROPERTIES { get; set; }
        public int? SECONDARY_INSURED_ID { get; set; }
    }
    //ver referencia del campo RETENTION_100 100_RETENTION
    public class ISSCO_RISK : EntityBasic
    {
        public string QueryName { get; set; }
        public int RISK_ID { get; set; }
        public int? LIMITS_RC_CD { get; set; }
        public decimal? LIMIT_RC_SUM { get; set; }
        public bool? RETENTION_100 { get; set; }
        public decimal? SINISTER_PCT { get; set; }
        public bool? HAS_SINISTER { get; set; }
        public int? ASSISTANCE_CD { get; set; }
        public int? SINISTER_QTY { get; set; }
        public DateTime? ACTUAL_DATE_MOVEMENT { get; set; }
    }

    public class ISSCPT_RISK : EntityBasic
    {
        public string QueryName { get; set; }
        public int RISK_ID { get; set; }
        public DateTime? FINE_DATE { get; set; }
        public bool? IS_FINE { get; set; }
        public DateTime? SCORE_DATE { get; set; }
        public bool? IS_SCORE { get; set; }
        public string SCORE { get; set; }
        public int? POLICY_2G { get; set; }
        public int? GROUP_FINE_ID { get; set; }
        public int? NEW_RENOVATED { get; set; }
        public int? RENEWAL_NUMBER { get; set; }
        public byte? MICRO_ZONE_CD { get; set; }
    }

    public class ISSCO_CPT_RISK_INFRINGEMENT : EntityBasic
    {
        public string QueryName { get; set; }
        public int RISK_ID { get; set; }
        public int GROUP_INFRINGEMENT_CD { get; set; }
        public int LAST_YEARS_INFRINGEMENT { get; set; }
        public int INFRINGEMENT_ONE_YEAR { get; set; }
        public int INFRINGEMENT_THREE_YEARS { get; set; }
    }

    public class ISSRISK_VEHICLE : EntityBasic
    {
        public string QueryName { get; set; }
        public int RISK_ID { get; set; }
        public Int16 VEHICLE_VERSION_CD { get; set; }
        public Int16 VEHICLE_MODEL_CD { get; set; }
        public Int16 VEHICLE_MAKE_CD { get; set; }
        public Int16 VEHICLE_YEAR { get; set; }
        public Int16 VEHICLE_TYPE_CD { get; set; }
        public Int16 VEHICLE_USE_CD { get; set; }
        public byte VEHICLE_BODY_CD { get; set; }
        public decimal VEHICLE_PRICE { get; set; }
        public bool IS_NEW { get; set; }
        public string LICENSE_PLATE { get; set; }
        public string ENGINE_SER_NO { get; set; }
        public string CHASSIS_SER_NO { get; set; }
        public Int16? VEHICLE_COLOR_CD { get; set; }
        public Int16? LOAD_TYPE_CD { get; set; }
        public byte? TRAILERS_QTY { get; set; }
        public Int16? PASSENGER_QTY { get; set; }
        public decimal? NEW_VEHICLE_PRICE { get; set; }
        public byte? VEHICLE_FUEL_CD { get; set; }
        public decimal? STD_VEHICLE_PRICE { get; set; }
    }
    public class ISSCO_RISK_VEHICLE : EntityBasic
    {
        public string QueryName { get; set; }
        public int RISK_ID { get; set; }
        public decimal? FLAT_RATE_PCT { get; set; }
        public Int16? SHUTTLE_CD { get; set; }
        public int? DEDUCT_ID { get; set; }
        public Int16? SERVICE_TYPE_CD { get; set; }
        public string MOBILE_NUM { get; set; }
        public int? ENDORSEMENT_ID { get; set; }
        public int? POLICY_ID { get; set; }
        public Int16? TONS_QTY { get; set; }
        public bool EXCESS { get; set; }
        public byte? RATE_TYPE_CD { get; set; }
        public int? GOOD_EXPERIENCE_NUM { get; set; }
        public int? WORKER_TYPE { get; set; }
        public DateTime? BIRTH_DATE { get; set; }
        public bool? IS_NEW_RATE { get; set; }
        public string GOOD_EXP_NUM_RATE { get; set; }
        public int? GOOD_EXP_NUM_PRINTER { get; set; }
    }

    public class ISSRISK_VEHICLE_DRIVE : EntityBasic
    {
        public string QueryName { get; set; }
        public int RISK_ID { get; set; }
        public int DRIVER_ID { get; set; }
        public Int16 DRIVER_NUM { get; set; }
        public string LICENSE_NO { get; set; }
        public decimal? DRIVING_PCT { get; set; }
        public byte? YEARS_DRIVING { get; set; }
        public DateTime? LICENSE_EXP_DATE { get; set; }
        public string LICENSE_EXP_ORG { get; set; }
        public string LICENSE_CATEGORY { get; set; }
    }
    public class ISSRISK_BENEFICIARY : EntityBasic
    {
        public string QueryName { get; set; }
        public int BENEFICIARY_ID { get; set; }
        public int RISK_ID { get; set; }
        public byte BENEFICIARY_TYPE_CD { get; set; }
        public decimal BENEFIT_PCT { get; set; }
        public Int16 ADDRESS_ID { get; set; }
        public Int16? NAME_NUM { get; set; }
    }
    public class ISSRISK_CLAUSE : EntityBasic
    {
        public string QueryName { get; set; }
        public int RISK_ID { get; set; }
        public int CLAUSE_ID { get; set; }
        public byte CLAUSE_STATUS_CD { get; set; }
        public bool IS_CURRENT { get; set; }
    }

    public class ISSRISK_SURETY : EntityBasic
    {
        public string QueryName { get; set; }
        public int RISK_ID { get; set; }
        public int INDIVIDUAL_ID { get; set; }
        public int SURETY_CONTRACT_TYPE_CD { get; set; }
        public int? SURETY_CONTRACT_CATEGORIES_CD { get; set; }
        public string BID_NUMBER { get; set; }
        public string PROYECT_NAME { get; set; }
        public string FUNDED_BY { get; set; }
        public string CONTRACT_ADDRESS { get; set; }
        public decimal CONTRACT_AMT { get; set; }
        public decimal? PILE_AMT { get; set; }
        public bool? IS_FACULTATIVE { get; set; }
    }

    public class ISSCO_RISK_SURETY : EntityBasic
    {
        public string QueryName { get; set; }
        public Int16? RISK_ID { get; set; }
        public int NAME_NUM { get; set; }
        public int INSURED_ID { get; set; }
        public Int16? ADDRESS_ID { get; set; }
        public Int16? PHONE_ID { get; set; }
        public string PROFESSIONAL_CARD_NUM { get; set; }
        public Int16? ARTICLE_CD { get; set; }
        public Int16? COURT_CD { get; set; }
        public Int16? CAPACITY_OF_CD { get; set; }
        public Int16? COUNTRY_CD { get; set; }
        public Int16? STATE_CD { get; set; }
        public Int16? CITY_CD { get; set; }
        public string INSURED_CAUTION { get; set; }
        public bool? IS_RETENTION { get; set; }
        public string COURT_NUM { get; set; }
        public Int16? ID_CARD_TYPE_CD { get; set; }
        public string ID_CARD_NO { get; set; }
    }

    public class ISSRISK_LOCATION : EntityBasic
    {
        public string QueryName { get; set; }
        public int RISK_ID { get; set; }
        public byte CONSTRUCTION_CATEGORY_CD { get; set; }
        public byte RISK_DANGEROUSNESS_CD { get; set; }
        public decimal EML_PCT { get; set; }
        public byte ADDRESS_TYPE_CD { get; set; }
        public byte STREET_TYPE_CD { get; set; }
        public Int16 COUNTRY_CD { get; set; }
        public Int16 STATE_CD { get; set; }
        public string STREET { get; set; }
        public Int16? CITY_CD { get; set; }
        public int? HOUSE_NUMBER { get; set; }
        public string FLOOR { get; set; }
        public string APARTMENT { get; set; }
        public string ZIP_CODE { get; set; }
        public string URBANIZATION { get; set; }
        public byte? CRESTA_ZONE_CD { get; set; }
        public bool IS_MAIN { get; set; }
        public Int16? ECONOMIC_ACTIVITY_CD { get; set; }
        public byte? HOUSING_TYPE_CD { get; set; }
        public byte? OCCUPATION_TYPE_CD { get; set; }
        public int? COMM_RISK_CLASS_CD { get; set; }
        public byte? RISK_COMMERCIAL_TYPE_CD { get; set; }
        public byte? RISK_COMM_SUBTYPE_CD { get; set; }
        public string ADDITIONAL_STREET { get; set; }
        public string BLOCK { get; set; }
        public byte? LOCATION_CD { get; set; }
        public byte? DECLARATIVE_PERIOD_CD { get; set; }
        public byte? PREMIUM_ADJUSTMENT_PERIOD_CD { get; set; }
        public int RISK_TYPE_CD { get; set; }
        public Int16? RISK_AGE { get; set; }
        public bool? IS_RETENTION { get; set; }
        public bool? INSPECTION_RECOMENDATION { get; set; }
        public byte? INSURANCE_MODE_CD { get; set; }
        public int? BUILD_YEAR { get; set; }
        public int? LEVEL_ZONE { get; set; }
        public int? CURRENT_PRIVATE_ZONE { get; set; }
        public bool? IS_RESIDENTIAL { get; set; }
        public bool? IS_OUT_COMMUNITY { get; set; }
        public int? DISTRICT { get; set; }
        public decimal? RISK_TYPE_EQ_CD { get; set; }
        public decimal? INDEMNIFICATION_LIMIT { get; set; }
        public decimal? CONSTRUCTION_YEAR_EARTHQUAKE { get; set; }
        public decimal? LONGITUDE_EARTHQUAKE { get; set; }
        public decimal? LATITUDE_EARTHQUAKE { get; set; }
        public decimal? FLOOR_NUMBER_EARTHQUAKE { get; set; }
        public decimal? RISK_USE_CD { get; set; }
        public decimal? STRUCTURE_CD { get; set; }
        public decimal? IRREGULAR_CD { get; set; }
        public decimal? IRREGULAR_HEIGHT_CD { get; set; }
        public decimal? PREVIOUS_DAMAGE_CD { get; set; }
        public decimal? REPAIRED_CD { get; set; }
        public decimal? REINFORCED_STRUCTURE_TYPE_CD { get; set; }
    }

    public class ISSRISK_SURETY_GUARANTEE : EntityBasic
    {
        public string QueryName { get; set; }
        public int RISK_ID { get; set; }
        public int GUARANTEE_ID { get; set; }
    }

    public class ISSENDO_RISK_COVERAGE : EntityBasic
    {
        public string QueryName { get; set; }
        public Int16 RISK_NUM { get; set; }
        public int ENDORSEMENT_ID { get; set; }
        public int POLICY_ID { get; set; }
        public int COVER_NUM { get; set; }
        public int RISK_COVER_ID { get; set; }
        public byte COVER_STATUS_CD { get; set; }
        public bool? ENABLED_CALCULATE { get; set; }
        public int RISK_ID { get; set; }
    }
    public class ISSRISK_COVERAGESum : EntityBasic
    {
        public string QueryName { get; set; }
        public decimal? PRIMA { get; set; }
    }
    public class ISSRISK_COVERAGE : EntityBasic
    {
        public string QueryName { get; set; }
        public int RISK_COVER_ID { get; set; }
        public int COVERAGE_ID { get; set; }
        public bool IS_DECLARATIVE { get; set; }
        public bool IS_MIN_PREMIUM_DEPOSIT { get; set; }
        public byte FIRST_RISK_TYPE_CD { get; set; }
        public Int16 CALCULATION_TYPE_CD { get; set; }
        public decimal DECLARED_AMT { get; set; }
        public decimal PREMIUM_AMT { get; set; }
        public decimal LIMIT_AMT { get; set; }
        public decimal SUBLIMIT_AMT { get; set; }
        public decimal LIMIT_IN_EXCESS { get; set; }
        public decimal LIMIT_OCCURRENCE_AMT { get; set; }
        public decimal LIMIT_CLAIMANT_AMT { get; set; }
        public decimal ACC_PREMIUM_AMT { get; set; }
        public decimal ACC_LIMIT_AMT { get; set; }
        public decimal ACC_SUBLIMIT_AMT { get; set; }
        public byte RATE_TYPE_CD { get; set; }
        public decimal? RATE { get; set; }
        public int? MAIN_COVERAGE_ID { get; set; }
        public decimal? MAIN_COVERAGE_PCT { get; set; }
        public byte? SHORT_TERM_CD { get; set; }
        public decimal? SHORT_TERM_PCT { get; set; }
        public DateTime? CURRENT_FROM { get; set; }
        public DateTime? CURRENT_TO { get; set; }
        public string CONDITION_string { get; set; }
        public bool IS_AUTO_REINST_SUM { get; set; }
        public bool IS_FACULTATIVE { get; set; }
        public decimal? SURCHARGE_AMT { get; set; }
        public decimal? DISCOUNT_AMT { get; set; }
        public decimal? SURCHARGE_RATE { get; set; }
        public decimal? DISCOUNT_RATE { get; set; }
        public byte? SURCHARGE_RATE_TYPE_CD { get; set; }
        public byte? DISCOUNT_RATE_TYPE_CD { get; set; }
        public decimal? DIFF_MIN_PREMIUM_AMT { get; set; }
        public decimal? ENDORSEMENT_LIMIT_AMT { get; set; }
        public decimal? ENDORSEMENT_SUBLIMIT_AMT { get; set; }
        public decimal? ADJUSTMENT_PREMIUM_AMT { get; set; }
        public decimal? CONTRACT_AMOUNT_PCT { get; set; }
        public bool IS_ENABLED_MINIMUM_PREMIUM { get; set; }
        public decimal? ENABLED_MINIMUM_PREMIUM_AMNT { get; set; }
        public decimal? PREMIUM_AMT_DEPOSIT_PERCENT { get; set; }
        public decimal? FLAT_RATE_PCT { get; set; }
        public decimal? MAX_LIABILITY_AMT { get; set; }
        public decimal? FLAT_RATE_PJE { get; set; }
        public byte?[] DYNAMIC_PROPERTIES { get; set; }
        public bool? IS_VARIABLE_INDEX { get; set; }
        public decimal? VARIABLE_IDX_PCT { get; set; }
        public decimal? VARIABLE_IDX_PREMIUM_PCT { get; set; }
        public decimal? VARIABLE_IDX_AMT { get; set; }
        public decimal? VARIABLE_IDX_PREMIUM_AMT { get; set; }
        public decimal? PROPORTIONAL_COINSURANCE_PCT { get; set; }
        public byte? DECLARATION_PERIOD_CD { get; set; }
        public byte? ADJUST_PERIOD_CD { get; set; }
        public decimal? ANNUAL_LIMIT_ADDED_AMT { get; set; }
        public decimal? ENDORSEMENT_DECLARED_AMT { get; set; }
    }
    public class ISSRISK_COVER_DEDUCT : EntityBasic
    {
        public string QueryName { get; set; }
        public int RISK_COVER_ID { get; set; }
        public byte RATE_TYPE_CD { get; set; }
        public decimal? RATE { get; set; }
        public decimal DEDUCT_PREMIUM_AMT { get; set; }
        public decimal DEDUCT_VALUE { get; set; }
        public byte DEDUCT_UNIT_CD { get; set; }
        public byte? DEDUCT_SUBJECT_CD { get; set; }
        public decimal? MIN_DEDUCT_VALUE { get; set; }
        public byte? MIN_DEDUCT_UNIT_CD { get; set; }
        public byte? MIN_DEDUCT_SUBJECT_CD { get; set; }
        public decimal? MAX_DEDUCT_VALUE { get; set; }
        public byte? MAX_DEDUCT_UNIT_CD { get; set; }
        public byte? MAX_DEDUCT_SUBJECT_CD { get; set; }
        public byte? CURRENCY_CD { get; set; }
        public decimal ACC_DEDUCT_AMT { get; set; }
        public int? DEDUCT_ID { get; set; }

    }
    public class ISSRISK_COVER_CLAUSE : EntityBasic
    {
        public string QueryName { get; set; }
        public int RISK_COVER_ID { get; set; }
        public int CLAUSE_ID { get; set; }
        public byte CLAUSE_STATUS_CD { get; set; }
        public bool IS_CURRENT { get; set; }
    }

    public class COMPONENTES : EntityBasic
    {
        public byte COMPONENT_CD { get; set; }
        public int ENDORSEMENT_ID { get; set; }
        public decimal Suma { get; set; }
    }
    public class ISSPAYER_COMP : EntityBasic
    {
        public string QueryName { get; set; }
        public int ENDORSEMENT_ID { get; set; }
        public int POLICY_ID { get; set; }
        public int PAYER_COMP_ID { get; set; }
        public byte COMPONENT_CD { get; set; }
        public int PAYER_ID { get; set; }
        public byte RATE_TYPE_CD { get; set; }
        public decimal RATE { get; set; }
        public decimal CALC_BASE_AMT { get; set; }
        public decimal COMPONENT_AMT { get; set; }
        public byte? ADDIT_RATE_TYPE_CD { get; set; }
        public decimal? ADDIT_RATE { get; set; }
        public decimal? ADDIT_CALC_BASE_AMT { get; set; }
        public decimal? ADDIT_COMPONENT_AMT { get; set; }
        public Int16? LINE_BUSINESS_CD { get; set; }
        public Int16? STATE_CD { get; set; }
        public Int16? COUNTRY_CD { get; set; }
        public Int16? ECONOMIC_ACTIVITY_CD { get; set; }
        public int? COVERAGE_ID { get; set; }
        public Int16? TAX_CD { get; set; }
        public byte? TAX_CATEGORY_CD { get; set; }
        public byte? TAX_CONDITION_CD { get; set; }
        public decimal? EXEMPTION_PCT { get; set; }
        public byte?[] DYNAMIC_PROPERTIES { get; set; }
    }

    public class ISSRISK_COVER_DETAIL : EntityBasic
    {
        public string QueryName { get; set; }
        public int RISK_DETAIL_ID { get; set; }
        public int RISK_COVER_ID { get; set; }
        public decimal? SUBLIMIT_AMT { get; set; }
        public byte? RATE_TYPE_CD { get; set; }
        public decimal? RATE { get; set; }
        public decimal? PREMIUM_AMT { get; set; }
        public decimal? ACC_PREMIUM_AMT { get; set; }
        public byte? COVER_STATUS_CD { get; set; }
    }
    public class ISSRISK_DETAIL_ACCESSORY : EntityBasic
    {
        public string QueryName { get; set; }
        public int RISK_DETAIL_ID { get; set; }
        public int DETAIL_ID { get; set; }
        public string BRAND_NAME { get; set; }
        public string MODEL { get; set; }
    }
    public class ISSRISK_DETAIL : EntityBasic
    {
        public string QueryName { get; set; }
        public int RISK_DETAIL_ID { get; set; }
        public byte DETAIL_TYPE_CD { get; set; }
        public byte DETAIL_CLASS_CD { get; set; }

    }
    public class ISSRISK_COVER_DETAIL_DEDUCT : EntityBasic
    {
        public string QueryName { get; set; }
        public int RISK_DETAIL_ID { get; set; }
        public int RISK_COVER_ID { get; set; }
        public decimal DEDUCT_VALUE { get; set; }
        public byte DEDUCT_UNIT_CD { get; set; }
        public byte? DEDUCT_SUBJECT_CD { get; set; }
        public byte? CURRENCY_CD { get; set; }
    }
    public class ISSRISK_DETAIL_DESCRIPTION : EntityBasic
    {
        public string QueryName { get; set; }
        public int RISK_DETAIL_ID { get; set; }
        public string DESCRIPTION { get; set; }
    }

    public class CustomResult
    {
        public List<COMMBRANCH> COMMBRANCH { get; set; }
        public List<COMMPREFIX> COMMPREFIX { get; set; }
        public List<ISSPolicy> ISSPolicy { get; set; }
        public List<ISSCOPolicy> ISSCOPolicy { get; set; }
        public List<ISSENDORSEMENT> ISSENDORSEMENT { get; set; }
        public List<ISSCO_ENDORSEMENT> ISSCO_ENDORSEMENT { get; set; }
        public List<ISSCPT_ENDORSEMENT> ISSCPT_ENDORSEMENT { get; set; }
        public List<ISSGROUP_ENDORSEMENT> ISSGROUP_ENDORSEMENT { get; set; }
        public List<ISSCOINSURANCE_ACCEPT_ACCEPTED> ISSCOINSURANCE_ACCEPT_ACCEPTED { get; set; }
        public List<ISSCOINSURANCE_ASSIGNED> ISSCOINSURANCE_ASSIGNED { get; set; }
        public List<ISSPOLICY_AGENT> ISSPOLICY_AGENT { get; set; }
        public List<ISSCOMMISS_AGENT> ISSCOMMISS_AGENT { get; set; }
        public List<ISSPOLICY_CLAUSE> ISSPOLICY_CLAUSE { get; set; }
        public List<ISSENDORSEMENT_PAYER> ISSENDORSEMENT_PAYER { get; set; }
        public List<ISSFIRST_PAY_COMP> ISSFIRST_PAY_COMP { get; set; }
        public List<ISSPAYER_PAYMENT> ISSPAYER_PAYMENT { get; set; }
        public List<ISSCO_PAYER_PAYMENT_COMP> ISSCO_PAYER_PAYMENT_COMP { get; set; }
        public List<ISSCO_PAYER_PAYMENT_COMPSum> ISSCO_PAYER_PAYMENT_COMPSum { get; set; }
        public List<ISSRISK> ISSRISK { get; set; }
        public List<ISSCO_RISK> ISSCO_RISK { get; set; }
        public List<ISSCPT_RISK> ISSCPT_RISK { get; set; }
        public List<ISSCO_CPT_RISK_INFRINGEMENT> ISSCO_CPT_RISK_INFRINGEMENT { get; set; }
        public List<ISSRISK_VEHICLE> ISSRISK_VEHICLE { get; set; }
        public List<ISSCO_RISK_VEHICLE> ISSCO_RISK_VEHICLE { get; set; }
        public List<ISSRISK_VEHICLE_DRIVE> ISSRISK_VEHICLE_DRIVE { get; set; }
        public List<ISSRISK_BENEFICIARY> ISSRISK_BENEFICIARY { get; set; }
        public List<ISSRISK_CLAUSE> ISSRISK_CLAUSE { get; set; }
        public List<ISSRISK_SURETY> ISSRISK_SURETY { get; set; }
        public List<ISSCO_RISK_SURETY> ISSCO_RISK_SURETY { get; set; }
        public List<ISSRISK_LOCATION> ISSRISK_LOCATION { get; set; }
        public List<ISSRISK_SURETY_GUARANTEE> ISSRISK_SURETY_GUARANTEE { get; set; }
        public List<ISSENDO_RISK_COVERAGE> ISSENDO_RISK_COVERAGE { get; set; }
        public List<ISSRISK_COVERAGESum> ISSRISK_COVERAGESum { get; set; }
        public List<ISSRISK_COVERAGE> ISSRISK_COVERAGE { get; set; }
        public List<ISSRISK_COVER_DEDUCT> ISSRISK_COVER_DEDUCT { get; set; }
        public List<ISSRISK_COVER_CLAUSE> ISSRISK_COVER_CLAUSE { get; set; }
        public List<COMPONENTES> COMPONENTES { get; set; }
        public List<ISSPAYER_COMP> ISSPAYER_COMP { get; set; }
        public List<ISSRISK_COVER_DETAIL> ISSRISK_COVER_DETAIL { get; set; }
        public List<ISSRISK_DETAIL_ACCESSORY> ISSRISK_DETAIL_ACCESSORY { get; set; }
        public List<ISSRISK_DETAIL> ISSRISK_DETAIL { get; set; }
        public List<ISSRISK_COVER_DETAIL_DEDUCT> ISSRISK_COVER_DETAIL_DEDUCT { get; set; }
        public List<ISSRISK_DETAIL_DESCRIPTION> ISSRISK_DETAIL_DESCRIPTION { get; set; }
    }


}