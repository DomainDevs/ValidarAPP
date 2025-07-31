using System;
using System.Data;

namespace Sistran.Company.Application.PrintingServicesEEProvider.DAOs
{
    public static class DatasetHelper
    {
        public static DataTable CreateCO_TMP_POLICY_RISK_COVERAGE()
        {
            DataTable CO_TMP_POLICY_RISK_COVERAGE = new DataTable("CO_TMP_POLICY_RISK_COVERAGE");
            CO_TMP_POLICY_RISK_COVERAGE.Columns.Add("PROCESS_ID", typeof(int));
            CO_TMP_POLICY_RISK_COVERAGE.Columns.Add("RISK_NUM", typeof(int));
            CO_TMP_POLICY_RISK_COVERAGE.Columns.Add("ROW_ID", typeof(int));
            CO_TMP_POLICY_RISK_COVERAGE.Columns.Add("COVERAGE_NUM", typeof(int));
            CO_TMP_POLICY_RISK_COVERAGE.Columns.Add("COVERAGE", typeof(string));
            CO_TMP_POLICY_RISK_COVERAGE.Columns.Add("COVERAGE_PREMIUM", typeof(string));
            CO_TMP_POLICY_RISK_COVERAGE.Columns.Add("COVERAGE_DEDUCT", typeof(string));
            CO_TMP_POLICY_RISK_COVERAGE.Columns.Add("CURRENT_FROM", typeof(DateTime));
            CO_TMP_POLICY_RISK_COVERAGE.Columns.Add("CURRENT_TO", typeof(DateTime));
            CO_TMP_POLICY_RISK_COVERAGE.Columns.Add("LIMIT_OCCURRENCE_AMT", typeof(string));
            CO_TMP_POLICY_RISK_COVERAGE.Columns.Add("SUB_LINE_BUSINESS_DESC", typeof(string));
            CO_TMP_POLICY_RISK_COVERAGE.Columns.Add("INSURED_OBJECT_DESC", typeof(string));
            CO_TMP_POLICY_RISK_COVERAGE.Columns.Add("IS_CHILD", typeof(bool));
            CO_TMP_POLICY_RISK_COVERAGE.Columns.Add("DEDUCT_VALUE", typeof(decimal));
            CO_TMP_POLICY_RISK_COVERAGE.Columns.Add("MIN_DEDUCT_VALUE", typeof(decimal));
            CO_TMP_POLICY_RISK_COVERAGE.Columns.Add("COVER_STATUS_CD", typeof(byte));
            CO_TMP_POLICY_RISK_COVERAGE.Columns.Add("RISK_STATUS_CD", typeof(byte));
            CO_TMP_POLICY_RISK_COVERAGE.Columns.Add("LNB_DESC", typeof(string));

            return CO_TMP_POLICY_RISK_COVERAGE;
        }
        public static DataTable CreateCO_TMP_POLICY_COVER()
        {
            DataTable CO_TMP_POLICY_COVER = new DataTable("CO_TMP_POLICY_COVER");

            CO_TMP_POLICY_COVER.Columns.Add("PROCESS_ID", typeof(int));
            CO_TMP_POLICY_COVER.Columns.Add("RISK_NUM", typeof(int));
            CO_TMP_POLICY_COVER.Columns.Add("BRANCH_CD", typeof(byte));
            CO_TMP_POLICY_COVER.Columns.Add("PREFIX_CD", typeof(int));
            CO_TMP_POLICY_COVER.Columns.Add("POLICY_NUMBER", typeof(decimal));
            CO_TMP_POLICY_COVER.Columns.Add("ISSUE_DATE_DAY", typeof(int));
            CO_TMP_POLICY_COVER.Columns.Add("ISSUE_DATE_MONTH", typeof(int));
            CO_TMP_POLICY_COVER.Columns.Add("ISSUE_DATE_YEAR", typeof(int));
            CO_TMP_POLICY_COVER.Columns.Add("ENDORSEMENT_TYPE_DESC", typeof(string));
            CO_TMP_POLICY_COVER.Columns.Add("DOCUMENT_NUM", typeof(decimal));
            CO_TMP_POLICY_COVER.Columns.Add("BRANCH", typeof(string));
            CO_TMP_POLICY_COVER.Columns.Add("POLICY_HOLDER_NAME", typeof(string));
            CO_TMP_POLICY_COVER.Columns.Add("POLICY_HOLDER_ADD", typeof(string));
            CO_TMP_POLICY_COVER.Columns.Add("POLICY_HOLDER_DOC_TYPE", typeof(string));
            CO_TMP_POLICY_COVER.Columns.Add("POLICY_HOLDER_DOC", typeof(string));
            CO_TMP_POLICY_COVER.Columns.Add("POLICY_HOLDER_PHONE", typeof(long));
            CO_TMP_POLICY_COVER.Columns.Add("CURRENCY", typeof(string));
            CO_TMP_POLICY_COVER.Columns.Add("EXCHANGE_RATE", typeof(decimal));
            CO_TMP_POLICY_COVER.Columns.Add("SALE_POINT", typeof(string));
            CO_TMP_POLICY_COVER.Columns.Add("CURRENT_FROM_DAY", typeof(int));
            CO_TMP_POLICY_COVER.Columns.Add("CURRENT_FROM_MONTH", typeof(int));
            CO_TMP_POLICY_COVER.Columns.Add("CURRENT_FROM_YEAR", typeof(int));
            CO_TMP_POLICY_COVER.Columns.Add("CURRENT_FROM_HOUR", typeof(string));
            CO_TMP_POLICY_COVER.Columns.Add("CURRENT_TO_DAY", typeof(int));
            CO_TMP_POLICY_COVER.Columns.Add("CURRENT_TO_MONTH", typeof(int));
            CO_TMP_POLICY_COVER.Columns.Add("CURRENT_TO_YEAR", typeof(int));
            CO_TMP_POLICY_COVER.Columns.Add("CURRENT_TO_HOUR", typeof(string));
            CO_TMP_POLICY_COVER.Columns.Add("DAY_COUNT", typeof(int));
            CO_TMP_POLICY_COVER.Columns.Add("PAYER_NAME", typeof(string));
            CO_TMP_POLICY_COVER.Columns.Add("PAYMENT_METHOD", typeof(string));
            CO_TMP_POLICY_COVER.Columns.Add("PREFIX", typeof(string));
            CO_TMP_POLICY_COVER.Columns.Add("BRANCH_ADD", typeof(string));
            CO_TMP_POLICY_COVER.Columns.Add("BRANCH_PHONE", typeof(long));
            CO_TMP_POLICY_COVER.Columns.Add("BRANCH_CITY", typeof(string));
            CO_TMP_POLICY_COVER.Columns.Add("BRANCH_COUNTRY", typeof(string));
            CO_TMP_POLICY_COVER.Columns.Add("CONDITION_TEXT", typeof(string));
            CO_TMP_POLICY_COVER.Columns.Add("POLICY_HOLDER_ID", typeof(int));
            CO_TMP_POLICY_COVER.Columns.Add("POLICY_ID", typeof(int));
            CO_TMP_POLICY_COVER.Columns.Add("ENDORSEMENT_ID", typeof(int));
            CO_TMP_POLICY_COVER.Columns.Add("PREMIUM_AMT", typeof(decimal));
            CO_TMP_POLICY_COVER.Columns.Add("LIMIT_AMT", typeof(decimal));
            CO_TMP_POLICY_COVER.Columns.Add("POLICY_TYPE", typeof(string));
            CO_TMP_POLICY_COVER.Columns.Add("EXPENSES", typeof(decimal));
            CO_TMP_POLICY_COVER.Columns.Add("TAX", typeof(decimal));
            CO_TMP_POLICY_COVER.Columns.Add("PAY_EXP_DATE", typeof(DateTime));
            CO_TMP_POLICY_COVER.Columns.Add("AGREED_PAYMENT_METHOD", typeof(string));
            CO_TMP_POLICY_COVER.Columns.Add("BILLING_GROUP", typeof(string));
            CO_TMP_POLICY_COVER.Columns.Add("ASSISTANCE", typeof(byte));
            CO_TMP_POLICY_COVER.Columns.Add("CURRENCY_SYMBOL", typeof(string));
            CO_TMP_POLICY_COVER.Columns.Add("USER_NAME", typeof(string));
            CO_TMP_POLICY_COVER.Columns.Add("BUSINESS_TYPE_DESC", typeof(string));
            CO_TMP_POLICY_COVER.Columns.Add("BUSINESS_TYPE_CD", typeof(byte));
            CO_TMP_POLICY_COVER.Columns.Add("CROP_RC_POLICY_NUMBER", typeof(decimal));
            CO_TMP_POLICY_COVER.Columns.Add("RISK_STATUS_DESCRIPTION", typeof(string));
            CO_TMP_POLICY_COVER.Columns.Add("EFFECT_PERIOD", typeof(int));
            CO_TMP_POLICY_COVER.Columns.Add("PRODUCT_DESCRIPTION", typeof(string));
            CO_TMP_POLICY_COVER.Columns.Add("TEMP_TYPE_CD", typeof(byte));
            CO_TMP_POLICY_COVER.Columns.Add("REQUEST_ID", typeof(int));
            CO_TMP_POLICY_COVER.Columns.Add("PRODUCT_FORM_NUMBER", typeof(string));
            CO_TMP_POLICY_COVER.Columns.Add("ACCESORIES_TOTAL", typeof(decimal));
            CO_TMP_POLICY_COVER.Columns.Add("RISK_ID", typeof(int));
            CO_TMP_POLICY_COVER.Columns.Add("INTERMEDIARY_NAME", typeof(string));
            CO_TMP_POLICY_COVER.Columns.Add("LIMIT_AMT_RC", typeof(decimal));
            CO_TMP_POLICY_COVER.Columns.Add("BAR_CODE", typeof(string));

            return CO_TMP_POLICY_COVER;
        }

        public static DataTable CreateCO_TMP_POLICY_CLAUSES()
        {
            DataTable CO_TMP_POLICY_CLAUSES = new DataTable("CO_TMP_POLICY_CLAUSES");

            CO_TMP_POLICY_CLAUSES.Columns.Add("PROCESS_ID", typeof(int));
            CO_TMP_POLICY_CLAUSES.Columns.Add("ENDORSEMENT_ID", typeof(int));
            CO_TMP_POLICY_CLAUSES.Columns.Add("POLICY_ID", typeof(string));

            return CO_TMP_POLICY_CLAUSES;
        }
        public static DataTable CreateCO_TMP_POLICY()
        {
            DataTable CO_TMP_POLICY = new DataTable("CO_TMP_POLICY");

            CO_TMP_POLICY.Columns.Add("PROCESS_ID", typeof(int));
            CO_TMP_POLICY.Columns.Add("RISK_NUM", typeof(int));
            CO_TMP_POLICY.Columns.Add("BRANCH_CD", typeof(byte));
            CO_TMP_POLICY.Columns.Add("PREFIX_CD", typeof(int));
            CO_TMP_POLICY.Columns.Add("POLICY_NUMBER", typeof(decimal));
            CO_TMP_POLICY.Columns.Add("ISSUE_DATE_DAY", typeof(int));
            CO_TMP_POLICY.Columns.Add("ISSUE_DATE_MONTH", typeof(int));
            CO_TMP_POLICY.Columns.Add("ISSUE_DATE_YEAR", typeof(int));
            CO_TMP_POLICY.Columns.Add("ENDORSEMENT_TYPE_DESC", typeof(string));
            CO_TMP_POLICY.Columns.Add("DOCUMENT_NUM", typeof(decimal));
            CO_TMP_POLICY.Columns.Add("BRANCH", typeof(string));
            CO_TMP_POLICY.Columns.Add("POLICY_HOLDER_NAME", typeof(string));
            CO_TMP_POLICY.Columns.Add("POLICY_HOLDER_ADD", typeof(string));
            CO_TMP_POLICY.Columns.Add("POLICY_HOLDER_DOC_TYPE", typeof(string));
            CO_TMP_POLICY.Columns.Add("POLICY_HOLDER_DOC", typeof(string));
            CO_TMP_POLICY.Columns.Add("POLICY_HOLDER_PHONE", typeof(long));
            CO_TMP_POLICY.Columns.Add("CURRENCY", typeof(string));
            CO_TMP_POLICY.Columns.Add("EXCHANGE_RATE", typeof(decimal));
            CO_TMP_POLICY.Columns.Add("SALE_POINT", typeof(string));
            CO_TMP_POLICY.Columns.Add("CURRENT_FROM_DAY", typeof(int));
            CO_TMP_POLICY.Columns.Add("CURRENT_FROM_MONTH", typeof(int));
            CO_TMP_POLICY.Columns.Add("CURRENT_FROM_YEAR", typeof(int));
            CO_TMP_POLICY.Columns.Add("CURRENT_FROM_HOUR", typeof(string));
            CO_TMP_POLICY.Columns.Add("CURRENT_TO_DAY", typeof(int));
            CO_TMP_POLICY.Columns.Add("CURRENT_TO_MONTH", typeof(int));
            CO_TMP_POLICY.Columns.Add("CURRENT_TO_YEAR", typeof(int));
            CO_TMP_POLICY.Columns.Add("CURRENT_TO_HOUR", typeof(string));
            CO_TMP_POLICY.Columns.Add("DAY_COUNT", typeof(int));
            CO_TMP_POLICY.Columns.Add("PAYER_NAME", typeof(string));
            CO_TMP_POLICY.Columns.Add("PAYMENT_METHOD", typeof(string));
            CO_TMP_POLICY.Columns.Add("PREFIX", typeof(string));
            CO_TMP_POLICY.Columns.Add("BRANCH_ADD", typeof(string));
            CO_TMP_POLICY.Columns.Add("BRANCH_PHONE", typeof(long));
            CO_TMP_POLICY.Columns.Add("BRANCH_CITY", typeof(string));
            CO_TMP_POLICY.Columns.Add("BRANCH_COUNTRY", typeof(string));
            CO_TMP_POLICY.Columns.Add("CONDITION_TEXT", typeof(string));
            CO_TMP_POLICY.Columns.Add("POLICY_HOLDER_ID", typeof(int));
            CO_TMP_POLICY.Columns.Add("POLICY_ID", typeof(int));
            CO_TMP_POLICY.Columns.Add("ENDORSEMENT_ID", typeof(int));
            CO_TMP_POLICY.Columns.Add("PREMIUM_AMT", typeof(decimal));
            CO_TMP_POLICY.Columns.Add("LIMIT_AMT", typeof(decimal));
            CO_TMP_POLICY.Columns.Add("POLICY_TYPE", typeof(string));
            CO_TMP_POLICY.Columns.Add("EXPENSES", typeof(decimal));
            CO_TMP_POLICY.Columns.Add("TAX", typeof(decimal));
            CO_TMP_POLICY.Columns.Add("PAY_EXP_DATE", typeof(DateTime));
            CO_TMP_POLICY.Columns.Add("AGREED_PAYMENT_METHOD", typeof(string));
            CO_TMP_POLICY.Columns.Add("BILLING_GROUP", typeof(string));
            CO_TMP_POLICY.Columns.Add("ASSISTANCE", typeof(byte));
            CO_TMP_POLICY.Columns.Add("CURRENCY_SYMBOL", typeof(string));
            CO_TMP_POLICY.Columns.Add("USER_NAME", typeof(string));
            CO_TMP_POLICY.Columns.Add("BUSINESS_TYPE_DESC", typeof(string));
            CO_TMP_POLICY.Columns.Add("BUSINESS_TYPE_CD", typeof(byte));
            CO_TMP_POLICY.Columns.Add("CROP_RC_POLICY_NUMBER", typeof(decimal));
            CO_TMP_POLICY.Columns.Add("RISK_STATUS_DESCRIPTION", typeof(string));
            CO_TMP_POLICY.Columns.Add("EFFECT_PERIOD", typeof(int));
            CO_TMP_POLICY.Columns.Add("PRODUCT_DESCRIPTION", typeof(string));
            CO_TMP_POLICY.Columns.Add("TEMP_TYPE_CD", typeof(byte));
            CO_TMP_POLICY.Columns.Add("REQUEST_ID", typeof(int));
            CO_TMP_POLICY.Columns.Add("PRODUCT_FORM_NUMBER", typeof(string));
            CO_TMP_POLICY.Columns.Add("ACCESORIES_TOTAL", typeof(decimal));
            CO_TMP_POLICY.Columns.Add("RISK_ID", typeof(int));
            CO_TMP_POLICY.Columns.Add("INTERMEDIARY_NAME", typeof(string));
            CO_TMP_POLICY.Columns.Add("LIMIT_AMT_RC", typeof(decimal));
            CO_TMP_POLICY.Columns.Add("BAR_CODE", typeof(string));

            return CO_TMP_POLICY;
        }

        internal static DataTable CreateJUDICIAL_SURETY()
        {
            DataTable JUDICIAL_SURETY = new DataTable("JUDICIAL_SURETY");

            JUDICIAL_SURETY.Columns.Add("BRANCH_ID", typeof(int));
            JUDICIAL_SURETY.Columns.Add("BRANCH_DESCRIPTION", typeof(string));
            JUDICIAL_SURETY.Columns.Add("BRANCH_CITY_DESCRIPTION", typeof(string));
            JUDICIAL_SURETY.Columns.Add("SALE_POINT_DESCRIPTION", typeof(string));
            JUDICIAL_SURETY.Columns.Add("PREFIX_ID", typeof(int));
            JUDICIAL_SURETY.Columns.Add("PREFIX_DESCRIPTION", typeof(string));
            JUDICIAL_SURETY.Columns.Add("PRODUCT_DESCRIPTION", typeof(string));
            JUDICIAL_SURETY.Columns.Add("POLICY_TYPE_DESCRIPTION", typeof(string));
            JUDICIAL_SURETY.Columns.Add("CURRENCY_DESCRIPTION", typeof(string));
            JUDICIAL_SURETY.Columns.Add("CURRENCY_SYMBOL", typeof(string));
            JUDICIAL_SURETY.Columns.Add("EXCHANGE_RATE", typeof(decimal));
            JUDICIAL_SURETY.Columns.Add("ENDORSEMENT_TYPE_DESCRIPTION", typeof(string));
            JUDICIAL_SURETY.Columns.Add("TEMPORAL_TYPE_ID", typeof(int));
            JUDICIAL_SURETY.Columns.Add("TEMPORAL_ID", typeof(int));
            JUDICIAL_SURETY.Columns.Add("POLICY_NUMBER", typeof(decimal));
            JUDICIAL_SURETY.Columns.Add("ENDORSEMENT_NUMBER", typeof(int));
            JUDICIAL_SURETY.Columns.Add("ISSUE_DATE_DAY", typeof(int));
            JUDICIAL_SURETY.Columns.Add("ISSUE_DATE_MONTH", typeof(int));
            JUDICIAL_SURETY.Columns.Add("ISSUE_DATE_YEAR", typeof(int));
            JUDICIAL_SURETY.Columns.Add("CURRENT_FROM_DAY", typeof(int));
            JUDICIAL_SURETY.Columns.Add("CURRENT_FROM_MONTH", typeof(int));
            JUDICIAL_SURETY.Columns.Add("CURRENT_FROM_YEAR", typeof(int));
            JUDICIAL_SURETY.Columns.Add("CURRENT_FROM_HOUR", typeof(string));
            JUDICIAL_SURETY.Columns.Add("CURRENT_TO_DAY", typeof(int));
            JUDICIAL_SURETY.Columns.Add("CURRENT_TO_MONTH", typeof(int));
            JUDICIAL_SURETY.Columns.Add("CURRENT_TO_YEAR", typeof(int));
            JUDICIAL_SURETY.Columns.Add("CURRENT_TO_HOUR", typeof(string));
            JUDICIAL_SURETY.Columns.Add("PAYER_NAME", typeof(string));
            JUDICIAL_SURETY.Columns.Add("PAYMENT_PLAN_DESCRIPTION", typeof(string));
            JUDICIAL_SURETY.Columns.Add("FIRST_PAYMENT_DAY", typeof(int));
            JUDICIAL_SURETY.Columns.Add("FIRST_PAYMENT_MONTH", typeof(int));
            JUDICIAL_SURETY.Columns.Add("FIRST_PAYMENT_YEAR", typeof(int));
            JUDICIAL_SURETY.Columns.Add("BILLING_GROUP_DESCRIPTION", typeof(string));
            JUDICIAL_SURETY.Columns.Add("USER_NAME", typeof(string));
            JUDICIAL_SURETY.Columns.Add("LIMIT_AMOUNT", typeof(decimal));
            JUDICIAL_SURETY.Columns.Add("PREMIUM", typeof(decimal));
            JUDICIAL_SURETY.Columns.Add("EXPENSES", typeof(decimal));
            JUDICIAL_SURETY.Columns.Add("TAXES", typeof(decimal));
            JUDICIAL_SURETY.Columns.Add("HOLDER_NAME", typeof(string));
            JUDICIAL_SURETY.Columns.Add("HOLDER_ADDRESS", typeof(string));
            JUDICIAL_SURETY.Columns.Add("HOLDER_DOCUMENT_TYPE", typeof(string));
            JUDICIAL_SURETY.Columns.Add("HOLDER_DOCUMENT_NUMBER", typeof(string));
            JUDICIAL_SURETY.Columns.Add("HOLDER_PHONE", typeof(string));
            JUDICIAL_SURETY.Columns.Add("INSURED_NAME", typeof(string));
            JUDICIAL_SURETY.Columns.Add("INSURED_ADDRESS", typeof(string));
            JUDICIAL_SURETY.Columns.Add("INSURED_DOCUMENT_TYPE", typeof(string));
            JUDICIAL_SURETY.Columns.Add("INSURED_DOCUMENT_NUMBER", typeof(string));
            JUDICIAL_SURETY.Columns.Add("INSURED_PHONE", typeof(string));
            JUDICIAL_SURETY.Columns.Add("BENEFICIARY_NAME", typeof(string));
            JUDICIAL_SURETY.Columns.Add("BENEFICIARY_ADDRESS", typeof(string));
            JUDICIAL_SURETY.Columns.Add("BENEFICIARY_DOCUMENT_TYPE", typeof(string));
            JUDICIAL_SURETY.Columns.Add("BENEFICIARY_DOCUMENT_NUMBER", typeof(string));
            JUDICIAL_SURETY.Columns.Add("BENEFICIARY_PHONE", typeof(string));
            JUDICIAL_SURETY.Columns.Add("TEXTS", typeof(string));
            JUDICIAL_SURETY.Columns.Add("ARTICLE_DESCRIPTION", typeof(string));
            JUDICIAL_SURETY.Columns.Add("ARTICLE_TEXT", typeof(string));
            JUDICIAL_SURETY.Columns.Add("COURT_NUMBER", typeof(string));
            JUDICIAL_SURETY.Columns.Add("COURT_DESCRIPTION", typeof(string));
            JUDICIAL_SURETY.Columns.Add("PROCESS_NUMBER", typeof(string));
            JUDICIAL_SURETY.Columns.Add("COUNTRY_DESCRIPTION", typeof(string));
            JUDICIAL_SURETY.Columns.Add("STATE_DESCRIPTION", typeof(string));
            JUDICIAL_SURETY.Columns.Add("CITY_DESCRIPTION", typeof(string));

            return JUDICIAL_SURETY;
        }

        internal static DataTable CreateCLAUSES()
        {
            DataTable CLAUSES = new DataTable("CLAUSE");

            CLAUSES.Columns.Add("TITLE", typeof(string));
            CLAUSES.Columns.Add("TEXT", typeof(string));

            return CLAUSES;
        }

        internal static DataTable CreateQUOTAS()
        {
            DataTable QUOTAS = new DataTable("CO_POLICY_PAYER");

            QUOTAS.Columns.Add("PAYMENT_DATE", typeof(DateTime));
            QUOTAS.Columns.Add("PAYMENT_AMOUNT", typeof(decimal));

            return QUOTAS;
        }

        internal static DataTable CreateCONVECTION()
        {
            DataTable CONVECTION = new DataTable("CO_TMP_POLICY");

            CONVECTION.Columns.Add("BRANCH_CD", typeof(int));
            CONVECTION.Columns.Add("PREFIX_CD", typeof(int));
            CONVECTION.Columns.Add("POLICY_NUMBER", typeof(decimal));
            CONVECTION.Columns.Add("TEMPORAL_ID", typeof(int));
            CONVECTION.Columns.Add("ISSUE_DATE_DAY", typeof(int));
            CONVECTION.Columns.Add("ISSUE_DATE_MONTH", typeof(int));
            CONVECTION.Columns.Add("ISSUE_DATE_YEAR", typeof(int));
            CONVECTION.Columns.Add("BRANCH", typeof(string));
            CONVECTION.Columns.Add("CURRENCY", typeof(string));
            CONVECTION.Columns.Add("PREMIUM_AMT", typeof(decimal));
            CONVECTION.Columns.Add("EXPENSES", typeof(decimal));
            CONVECTION.Columns.Add("TAX", typeof(decimal));
            CONVECTION.Columns.Add("AGREED_PAYMENT_METHOD", typeof(string));
            CONVECTION.Columns.Add("USER_NAME", typeof(string));
            CONVECTION.Columns.Add("BUSINESS_TYPE_CD", typeof(int));
            CONVECTION.Columns.Add("TEMP_TYPE_CD", typeof(int));

            return CONVECTION;
        }

        internal static DataTable CreateCOVERAGE()
        {
            DataTable COVERAGE = new DataTable("COVERAGE");

            COVERAGE.Columns.Add("NUMBER", typeof(int));
            COVERAGE.Columns.Add("DESCRIPTION", typeof(string));
            COVERAGE.Columns.Add("PREMIUM", typeof(string));
            COVERAGE.Columns.Add("DEDUCTIBLE", typeof(string));
            COVERAGE.Columns.Add("DEDUCTIBLE_VALUE", typeof(decimal));
            COVERAGE.Columns.Add("DEDUCTIBLE_VALUE_MINIMUM", typeof(decimal));

            return COVERAGE;
        }

        internal static DataTable CreateAGENCY()
        {
            DataTable AGENCY = new DataTable("AGENCY");

            AGENCY.Columns.Add("CODE", typeof(int));
            AGENCY.Columns.Add("TYPE_DESCRIPTION", typeof(string));
            AGENCY.Columns.Add("NAME", typeof(string));
            AGENCY.Columns.Add("PARTICIPATION", typeof(decimal));
            AGENCY.Columns.Add("IS_MAIN", typeof(bool));

            return AGENCY;
        }

        internal static DataTable CreateCOINSURANCE()
        {
            DataTable COINSURANCE = new DataTable("COINSURANCE");

            COINSURANCE.Columns.Add("ID", typeof(int));
            COINSURANCE.Columns.Add("DESCRIPTION", typeof(string));
            COINSURANCE.Columns.Add("PARTICIPATION", typeof(decimal));
            COINSURANCE.Columns.Add("IS_MAIN", typeof(bool));

            return COINSURANCE;
        }

        public static DataTable CreateCLAUSE()
        {
            DataTable CLAUSE = new DataTable("CLAUSE");

            CLAUSE.Columns.Add("CLAUSE_ID", typeof(int));
            CLAUSE.Columns.Add("CLAUSE_NAME", typeof(string));
            CLAUSE.Columns.Add("CLAUSE_TITLE", typeof(string));
            CLAUSE.Columns.Add("CURRENT_FROM", typeof(DateTime));
            CLAUSE.Columns.Add("CURRENT_TO", typeof(DateTime));
            CLAUSE.Columns.Add("CONDITION_LEVEL_CD", typeof(byte));
            CLAUSE.Columns.Add("CLAUSE_TEXT", typeof(string));

            return CLAUSE;
        }

        public static DataTable CreateCO_TMP_POLICY_VEHICLE()
        {
            DataTable CO_TMP_POLICY_VEHICLE = new DataTable("CO_TMP_POLICY_VEHICLE");

            CO_TMP_POLICY_VEHICLE.Columns.Add("PROCESS_ID", typeof(int));
            CO_TMP_POLICY_VEHICLE.Columns.Add("RISK_NUM", typeof(int));
            CO_TMP_POLICY_VEHICLE.Columns.Add("VEHICLE_TYPE", typeof(string));
            CO_TMP_POLICY_VEHICLE.Columns.Add("VEHICLE_COLOR", typeof(string));
            CO_TMP_POLICY_VEHICLE.Columns.Add("VEHICLE_LICENSE", typeof(string));
            CO_TMP_POLICY_VEHICLE.Columns.Add("VEHICLE_MAKE", typeof(string));
            CO_TMP_POLICY_VEHICLE.Columns.Add("VEHICLE_YEAR", typeof(int));
            CO_TMP_POLICY_VEHICLE.Columns.Add("VEHICLE_ENGINE_SER", typeof(string));
            CO_TMP_POLICY_VEHICLE.Columns.Add("VEHICLE_BODY", typeof(string));
            CO_TMP_POLICY_VEHICLE.Columns.Add("VEHICLE_MODEL", typeof(string));
            CO_TMP_POLICY_VEHICLE.Columns.Add("VEHICLE_FASECOLDA_CD", typeof(string));
            CO_TMP_POLICY_VEHICLE.Columns.Add("VEHICLE_CHASSIS_SER", typeof(string));
            CO_TMP_POLICY_VEHICLE.Columns.Add("VEHICLE_USE", typeof(string));
            CO_TMP_POLICY_VEHICLE.Columns.Add("VEHICLE_WEIGHT", typeof(int));
            CO_TMP_POLICY_VEHICLE.Columns.Add("VEHICLE_ADDRESS", typeof(string));
            CO_TMP_POLICY_VEHICLE.Columns.Add("PASSENGER_QTY", typeof(int));
            CO_TMP_POLICY_VEHICLE.Columns.Add("GOOD_EXPERIENCE_NUM", typeof(int));
            CO_TMP_POLICY_VEHICLE.Columns.Add("INSURED_AGE", typeof(int));
            CO_TMP_POLICY_VEHICLE.Columns.Add("RATING_ZONE_DESCRIPTION", typeof(string));
            CO_TMP_POLICY_VEHICLE.Columns.Add("IS_NEW", typeof(bool));
            CO_TMP_POLICY_VEHICLE.Columns.Add("GENDER", typeof(string));

            return CO_TMP_POLICY_VEHICLE;
        }

        public static DataTable CreateCO_POLICY_PAYER()
        {
            DataTable CO_POLICY_PAYER = new DataTable("CO_POLICY_PAYER");

            CO_POLICY_PAYER.Columns.Add("PROCESS_ID", typeof(int));
            CO_POLICY_PAYER.Columns.Add("PAYMENT_DATE", typeof(DateTime));
            CO_POLICY_PAYER.Columns.Add("PAYMENT_AMOUNT", typeof(decimal));

            return CO_POLICY_PAYER;
        }

        public static DataTable CreateCO_POLICY_AGENT()
        {
            DataTable CO_POLICY_AGENT = new DataTable("CO_POLICY_AGENT");

            CO_POLICY_AGENT.Columns.Add("PROCESS_ID", typeof(int));
            CO_POLICY_AGENT.Columns.Add("AGENT_CODE", typeof(int));
            CO_POLICY_AGENT.Columns.Add("AGENT_TYPE_DESC", typeof(string));
            CO_POLICY_AGENT.Columns.Add("AGENT_NAME", typeof(string));
            CO_POLICY_AGENT.Columns.Add("PARTICIPATION", typeof(decimal));
            CO_POLICY_AGENT.Columns.Add("IS_MAIN", typeof(bool));

            return CO_POLICY_AGENT;
        }

        public static DataTable CreateBUSINESS_TYPE()
        {
            DataTable BUSINESS_TYPE = new DataTable("BUSINESS_TYPE");

            BUSINESS_TYPE.Columns.Add("BUSINESS_TYPE_CD", typeof(int));
            BUSINESS_TYPE.Columns.Add("SMALL_DESCRIPTION", typeof(string));

            return BUSINESS_TYPE;
        }

        public static DataTable CreateCO_TMP_POLICY_RISK_DETAIL()
        {
            DataTable CO_TMP_POLICY_RISK_DETAIL = new DataTable("CO_TMP_POLICY_RISK_DETAIL");

            CO_TMP_POLICY_RISK_DETAIL.Columns.Add("PROCESS_ID", typeof(int));
            CO_TMP_POLICY_RISK_DETAIL.Columns.Add("RISK_NUM", typeof(int));
            CO_TMP_POLICY_RISK_DETAIL.Columns.Add("DETAIL", typeof(string));

            return CO_TMP_POLICY_RISK_DETAIL;
        }

        public static DataTable CreateCO_TMP_POLICY_RISK_COLLECTIVE()
        {
            DataTable CO_TMP_POLICY_RISK_COLLECTIVE = new DataTable("CO_TMP_POLICY_RISK_COLLECTIVE");

            CO_TMP_POLICY_RISK_COLLECTIVE.Columns.Add("CURRENT_FROM", typeof(DateTime));
            CO_TMP_POLICY_RISK_COLLECTIVE.Columns.Add("CURRENT_TO", typeof(DateTime));

            return CO_TMP_POLICY_RISK_COLLECTIVE;
        }
        public static DataTable CreateCO_TMP_POLICY_RISK()
        {
            DataTable CO_TMP_POLICY_RISK = new DataTable("CO_TMP_POLICY_RISK");

            CO_TMP_POLICY_RISK.Columns.Add("PROCESS_ID", typeof(int));
            CO_TMP_POLICY_RISK.Columns.Add("RISK_NUM", typeof(int));
            CO_TMP_POLICY_RISK.Columns.Add("INSURED_NAME", typeof(string));
            CO_TMP_POLICY_RISK.Columns.Add("INSURED_ADD", typeof(string));
            CO_TMP_POLICY_RISK.Columns.Add("BENEFICIARY_NAME", typeof(string));
            CO_TMP_POLICY_RISK.Columns.Add("BENEFICIARY_ADD", typeof(string));
            CO_TMP_POLICY_RISK.Columns.Add("ENTRENCHED_NAME", typeof(string));
            CO_TMP_POLICY_RISK.Columns.Add("ENTRENCHED_ADD", typeof(string));
            CO_TMP_POLICY_RISK.Columns.Add("INSURED_DOC_TYPE", typeof(string));
            CO_TMP_POLICY_RISK.Columns.Add("BENEFICIARY_DOC_TYPE", typeof(string));
            CO_TMP_POLICY_RISK.Columns.Add("ENTRENCHED_DOC_TYPE", typeof(string));
            CO_TMP_POLICY_RISK.Columns.Add("INSURED_DOC", typeof(string));
            CO_TMP_POLICY_RISK.Columns.Add("INSURED_PHONE", typeof(long));
            CO_TMP_POLICY_RISK.Columns.Add("BENEFICIARY_DOC", typeof(string));
            CO_TMP_POLICY_RISK.Columns.Add("BENEFICIARY_PHONE", typeof(long));
            CO_TMP_POLICY_RISK.Columns.Add("ENTRENCHED_DOC", typeof(string));
            CO_TMP_POLICY_RISK.Columns.Add("ENTRENCHED_PHONE", typeof(long));
            CO_TMP_POLICY_RISK.Columns.Add("INSURED_ID", typeof(int));
            CO_TMP_POLICY_RISK.Columns.Add("BENEFICIARY_ID", typeof(int));
            CO_TMP_POLICY_RISK.Columns.Add("ENTRENCHED_ID", typeof(int));
            CO_TMP_POLICY_RISK.Columns.Add("CONDITION_TEXT", typeof(string));
            CO_TMP_POLICY_RISK.Columns.Add("BENEFICIARY_INDIVIDUAL_TYPE_CD", typeof(int));
            CO_TMP_POLICY_RISK.Columns.Add("PREMIUM_AMT", typeof(decimal));
            CO_TMP_POLICY_RISK.Columns.Add("LIMIT_AMT", typeof(decimal));
            CO_TMP_POLICY_RISK.Columns.Add("EXPENSES", typeof(decimal));
            CO_TMP_POLICY_RISK.Columns.Add("TAX", typeof(decimal));
            CO_TMP_POLICY_RISK.Columns.Add("RATING_ZONE_DESC", typeof(string));

            return CO_TMP_POLICY_RISK;
        }

        public static DataTable CreateCO_TMP_POLICY_TEXT()
        {
            DataTable table = new DataTable("CO_TMP_POLICY_TEXT");

            table.Columns.Add("PROCESS_ID", typeof(int));
            table.Columns.Add("COMPLETE_TEXT", typeof(string));
            table.Columns.Add("FIRST_PAGE_TEXT", typeof(string));
            table.Columns.Add("SECOND_PAGE_TEXT", typeof(string));

            return table;
        }

        public static DataTable CreateCO_POLICY_REPORT_DETAIL_STRING()
        {
            DataTable table = new DataTable("CO_POLICY_REPORT_DETAIL_STRING");

            table.Columns.Add("TEMP_ID", typeof(int));
            table.Columns.Add("PROCESS_ID", typeof(int));
            table.Columns.Add("RISK_NUM", typeof(int));
            table.Columns.Add("DETAIL", typeof(string));
            table.Columns.Add("RISK_ID", typeof(int));

            return table;
        }

        public static DataTable CreateCO_TMP_POLICY_COINSURANCE()
        {
            DataTable table = new DataTable("CO_TMP_POLICY_COINSURANCE");

            table.Columns.Add("PROCESS_ID", typeof(int));
            table.Columns.Add("INSURANCE_COMPANY_ID", typeof(int));
            table.Columns.Add("INSURANCE_COMPANY_DESC", typeof(string));
            table.Columns.Add("PART_CIA_PCT", typeof(decimal));
            table.Columns.Add("IS_MAIN_COMPANY", typeof(bool));

            return table;
        }

        public static DataTable CreateCO_TMP_POLICY_RISK_BENEFICIARY()
        {
            DataTable table = new DataTable("CO_TMP_POLICY_RISK_BENEFICIARY");

            table.Columns.Add("PROCESS_ID", typeof(int));
            table.Columns.Add("RISK_NUM", typeof(int));
            table.Columns.Add("BENEFICIARY_NAME", typeof(string));
            table.Columns.Add("BENEFICIARY_ADD", typeof(string));
            table.Columns.Add("BENEFICIARY_DOC_TYPE", typeof(string));
            table.Columns.Add("BENEFICIARY_DOC", typeof(string));
            table.Columns.Add("BENEFICIARY_PHONE", typeof(long));
            table.Columns.Add("BENEFICIARY_ID", typeof(int));

            return table;
        }

        public static DataTable CreateCO_COVERAGE()
        {
            DataTable table = new DataTable("CO_COVERAGE");

            table.Columns.Add("COVERAGE_ID", typeof(int));
            table.Columns.Add("COVERAGE_NUM", typeof(int));
            table.Columns.Add("IS_IMPRESSION", typeof(bool));
            table.Columns.Add("IS_CHILD", typeof(bool));
            table.Columns.Add("IS_ACC_MIN_PREMIUM", typeof(bool));

            return table;
        }

        public static DataTable CreateCO_TMP_POLICY_LOCATION()
        {
            DataTable table = new DataTable("CO_TMP_POLICY_LOCATION");

            table.Columns.Add("PROCESS_ID", typeof(int));
            table.Columns.Add("RISK_NUM", typeof(int));
            table.Columns.Add("LOCATION_ADDRESS", typeof(string));

            return table;
        }

        public static DataTable CreateCO_POLICY_REPORT_DETAIL()
        {
            DataTable table = new DataTable("CO_POLICY_REPORT_DETAIL");

            table.Columns.Add("TEMP_ID", typeof(int));
            table.Columns.Add("PROCESS_ID", typeof(int));
            table.Columns.Add("RISK_NUM", typeof(int));
            table.Columns.Add("DESCRIPTION", typeof(string));
            table.Columns.Add("DETAIL", typeof(string));
            table.Columns.Add("PREMIUM_DETAIL", typeof(decimal));
            table.Columns.Add("RISK_ID", typeof(int));

            return table;
        }

        internal static DataTable CreateACCESORY()
        {
            DataTable ACCESORY = new DataTable("ACCESORY");

            ACCESORY.Columns.Add("TYPE_DESCRIPTION", typeof(string));
            ACCESORY.Columns.Add("MAKE_DESCRIPTION", typeof(string));
            ACCESORY.Columns.Add("INSURED_AMOUNT", typeof(decimal));

            return ACCESORY;
        }

        internal static DataTable createCO_TMP_POLICY_RISK_SURETY_CONTRACT()
        {
            DataTable table = new DataTable("CO_TMP_POLICY_RISK_SURETY_CONTRACT");

            table.Columns.Add("PROCESS_ID", typeof(int));
            table.Columns.Add("OBJECT_CONTRACT", typeof(string));

            return table;
        }
    }
}