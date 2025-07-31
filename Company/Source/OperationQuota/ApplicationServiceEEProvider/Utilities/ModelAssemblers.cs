using Sistran.Core.Framework.DAF.Engine;
using System;
using System.Data;
using AQMOD = Sistran.Company.Application.OperationQuotaServices.EEProvider.Models.OperationQuota;

namespace Sistran.Company.Application.OperationQuotaServices.EEProvider.Utilities
{
    public class ModelAssemblers
    {
        #region AutomaticQuota

        public static DataTable GetDataTableAutomatic_Quota(AQMOD.AutomaticQuota automaticQuota)
        {
            IDynamicPropertiesSerializer dynamicPropertiesSerializer =
               new DynamicPropertiesSerializer();
            DataTable dataTable = new DataTable("INSERT_AUTOMATIC_QUOTA");
            #region FirsParms

            #region Columns
            dataTable.Columns.Add("INDIVIDUAL_ID", typeof(int));
            dataTable.Columns.Add("CUSTOMER_TYPE_CD", typeof(int));
            dataTable.Columns.Add("SUGGESTED_QUOTA", typeof(decimal));
            dataTable.Columns.Add("QUOTA_RECONSIDERATION", typeof(int));
            dataTable.Columns.Add("LEGALIZED_QUOTA", typeof(int));
            dataTable.Columns.Add("CURRENT_QUOTA", typeof(decimal));
            dataTable.Columns.Add("CURRENT_CLUSTER", typeof(int));
            dataTable.Columns.Add("QUOTA_DATE", typeof(DateTime));
            dataTable.Columns.Add("REQUESTED_BY", typeof(int));
            dataTable.Columns.Add("PREPARED_BY", typeof(string));
            dataTable.Columns.Add("GUARANTEE_TYPE_CD", typeof(int));
            dataTable.Columns.Add("GUARANTEE_STATUS_CD", typeof(int));
            dataTable.Columns.Add("OBSERVATION", typeof(string));
            dataTable.Columns.Add("COUNTRY_CD", typeof(int));
            dataTable.Columns.Add("CITY_CD", typeof(int));
            dataTable.Columns.Add("STATE_CD", typeof(string));

            #endregion

            DataRow rows = dataTable.NewRow();

            #region DataRows
            rows["INDIVIDUAL_ID"] = automaticQuota.IndividualId;
            rows["CUSTOMER_TYPE_CD"] = automaticQuota.CustomerTypeId;
            rows["SUGGESTED_QUOTA"] = automaticQuota.SuggestedQuota;
            rows["QUOTA_RECONSIDERATION"] = automaticQuota.QuotaReconsideration;
            rows["LEGALIZED_QUOTA"] = automaticQuota.LegalizedQuota;
            rows["CURRENT_QUOTA"] = automaticQuota.CurrentQuota;
            rows["CURRENT_CLUSTER"] = automaticQuota.CurrentCluster;
            rows["QUOTA_DATE"] = DateTime.Now;
            rows["REQUESTED_BY"] = automaticQuota.RequestedById;
            rows["PREPARED_BY"] = automaticQuota.ElaboratedId;
            rows["GUARANTEE_TYPE_CD"] = automaticQuota.TypeCollateral;
            rows["GUARANTEE_STATUS_CD"] = automaticQuota.CollateralStatus;
            rows["OBSERVATION"] = automaticQuota.Observations;
            rows["COUNTRY_CD"] = automaticQuota.CountryId;
            rows["CITY_CD"] = automaticQuota.CityId;
            rows["STATE_CD"] = automaticQuota.StateId;

            #endregion

            dataTable.Rows.Add(rows);
            #endregion
            return dataTable;
        }

        public static DataTable GetDataTableUtility(AQMOD.AutomaticQuota automaticQuota)
        {
            IDynamicPropertiesSerializer dynamicPropertiesSerializer =
               new DynamicPropertiesSerializer();
            DataTable dataTable = new DataTable("INSERT_UTILITY");
            #region FirsParms

            #region Columns
            dataTable.Columns.Add("UTILITY_DETAILS_CD", typeof(int));
            dataTable.Columns.Add("START_VALUE", typeof(decimal));
            dataTable.Columns.Add("END_VALUE", typeof(decimal));
            dataTable.Columns.Add("ABSOLUTE_VALUE", typeof(decimal));
            dataTable.Columns.Add("RELATIVE_VALUE", typeof(decimal));
            #endregion

            #region DataRows          
            if (automaticQuota.Utility != null)
            {
                foreach (var item in automaticQuota.Utility)
                {
                    if (item.UtilityDetails.FormUtilitys == 1)
                    {
                        DataRow rows = dataTable.NewRow();
                        rows["UTILITY_DETAILS_CD"] = item.UtilityDetails.UtilityId;
                        rows["START_VALUE"] = item.Start_Values;
                        rows["END_VALUE"] = item.End_value;
                        rows["ABSOLUTE_VALUE"] = item.Var_Abs;
                        rows["RELATIVE_VALUE"] = item.Var_Relativa;

                        dataTable.Rows.Add(rows);
                    }

                }

            }

            #endregion

            #endregion
            return dataTable;
        }
        public static DataTable GetDataTableUtilitySummary(AQMOD.AutomaticQuota automaticQuota)
        {
            IDynamicPropertiesSerializer dynamicPropertiesSerializer =
               new DynamicPropertiesSerializer();
            DataTable dataTable = new DataTable("INSERT_UTILITY_SUMMARY");
            #region FirsParms

            #region Columns
            dataTable.Columns.Add("UTILITY_DETAILS_CD", typeof(int));
            dataTable.Columns.Add("START_VALUE", typeof(decimal));
            dataTable.Columns.Add("END_VALUE", typeof(decimal));
            dataTable.Columns.Add("ABSOLUTE_VALUE", typeof(decimal));
            dataTable.Columns.Add("RELATIVE_VALUE", typeof(decimal));
            #endregion

            #region DataRows          
            if (automaticQuota.Utility != null)
            {
                foreach (var item in automaticQuota.Utility)
                {
                    if (item.UtilityDetails.FormUtilitys == 2)
                    {
                        DataRow rows = dataTable.NewRow();
                            rows["UTILITY_DETAILS_CD"] = item.UtilityDetails.Id;
                            rows["START_VALUE"] = item.Start_Values;
                            rows["END_VALUE"] = item.End_value;
                            rows["ABSOLUTE_VALUE"] = item.Var_Abs;
                            rows["RELATIVE_VALUE"] = item.Var_Relativa;
                        dataTable.Rows.Add(rows);
                    }

                }

            }

            #endregion

            #endregion
            return dataTable;
        }
        #endregion

        public static DataTable GetDataTableIndicator(AQMOD.AutomaticQuota automaticQuota)
        {
            IDynamicPropertiesSerializer dynamicPropertiesSerializer =
               new DynamicPropertiesSerializer();
            DataTable dataTable = new DataTable("INSERT_INDICATOR");
            #region FirsParms

            #region Columns
            dataTable.Columns.Add("INDICATOR_CONCEPT_CD", typeof(int));
            dataTable.Columns.Add("START_VALUE", typeof(decimal));
            dataTable.Columns.Add("END_VALUE", typeof(decimal));
            dataTable.Columns.Add("OBSERVATION", typeof(string));
            #endregion
            
            #region DataRows          
            if (automaticQuota.Indicator != null)
            {
                foreach (var item in automaticQuota.Indicator)
                {
                    DataRow rows = dataTable.NewRow();
                        rows["INDICATOR_CONCEPT_CD"] = item.ConceptIndicatorcd;
                        rows["START_VALUE"] = item.IndicatorIni;
                        rows["END_VALUE"] = item.IndicatorFin;
                        rows["OBSERVATION"] = item.Observation;
                    dataTable.Rows.Add(rows);
                }

            }

            #endregion

            #endregion
            return dataTable;
        }
        public static DataTable GetDataTableThird(AQMOD.AutomaticQuota automaticQuota)
        {
            IDynamicPropertiesSerializer dynamicPropertiesSerializer =
               new DynamicPropertiesSerializer();
            DataTable dataTable = new DataTable("INSERT_THIRD");
            #region FirsParms

            #region Columns

            dataTable.Columns.Add("RESTRICTIVE_LIST_CD", typeof(int));
            dataTable.Columns.Add("RISK_CENTER_LIST_CD", typeof(int));
            dataTable.Columns.Add("PROMISSORY_NOTE_SIGNATURE_CD", typeof(int));
            dataTable.Columns.Add("REPORT_LIST_SISCONC_CD", typeof(int));
            dataTable.Columns.Add("CIFIN_DATE", typeof(DateTime));
            dataTable.Columns.Add("PRINCIPAL_DEPTOR", typeof(decimal));
            dataTable.Columns.Add("CREDITORS", typeof(decimal));
            dataTable.Columns.Add("TOTAL", typeof(decimal));
            #endregion

            DataRow rows = dataTable.NewRow();

            #region DataRows          
            if (automaticQuota.Third != null)
            {
                rows["RESTRICTIVE_LIST_CD"] = automaticQuota.Third.Restrictive.Id;
                rows["RISK_CENTER_LIST_CD"] = automaticQuota.Third.RiskCenter.Id;
                rows["PROMISSORY_NOTE_SIGNATURE_CD"] = automaticQuota.Third.PromissoryNoteSignature.Id;
                rows["REPORT_LIST_SISCONC_CD"] = automaticQuota.Third.ReportListSisconc.Id;
                rows["CIFIN_DATE"] = automaticQuota.Third.CifinQuery;
                rows["PRINCIPAL_DEPTOR"] = automaticQuota.Third.PrincipalDebtor;
                rows["CREDITORS"] = automaticQuota.Third.Cosigner;
                rows["TOTAL"] = automaticQuota.Third.Total;
            }

            #endregion
            dataTable.Rows.Add(rows);
            #endregion
            return dataTable;
        }
    }
}
