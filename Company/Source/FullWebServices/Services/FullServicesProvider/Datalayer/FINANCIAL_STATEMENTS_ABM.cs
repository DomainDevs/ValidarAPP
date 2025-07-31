using System;
using System.Data;
using System.Data.SqlTypes;
using Sybase.Data.AseClient;
using System.Collections.Generic;
using Sistran.Co.Previsora.Application.FullServices.Models;
using Sybase.Data.AseClient;
using Sistran.Co.Previsora.Application.FullServicesProvider.BussinesLayer;
using Sistran.Co.Previsora.Application.FullServicesProvider.Helpers;
using Sistran.Co.Previsora.Application.FullServicesProvider.DataLayer;

namespace Sistran.Co.Previsora.Application.FullServices.Models.DataLayer
{
    /// <summary>
    /// Data access layer class for FINANCIAL_STATEMENTS
    /// </summary>
    class FINANCIAL_STATEMENTS_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public FINANCIAL_STATEMENTS_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public FINANCIAL_STATEMENTS_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public FINANCIAL_STATEMENTS_ABM(string Connection, string userId, int AppId, AseCommand Command)
            : base(Connection, userId, AppId, Command)
        {
            // Nothing for now.
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// insert new row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true of successfully insert</returns>
        public bool Insert(FINANCIAL_STATEMENTS businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.FINANCIAL_STATEM_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@TECHNICAL_CARD_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.TECHNICAL_CARD_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@BALANCE_DATE", AseDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.BALANCE_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@INVENTORY_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INVENTORY_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@ACCOUNTS_RECEIVABLE_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ACCOUNTS_RECEIVABLE_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@CASH_INVESTMENT_TEMPORARY_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CASH_INVESTMENT_TEMPORARY_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@CURRENT_ASSETS_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CURRENT_ASSETS_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@FIXED_GROSS_ASSETS_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.FIXED_GROSS_ASSETS_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@FIXED_NET_ASSETS_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.FIXED_NET_ASSETS_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@VALUATION_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.VALUATION_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@ASSETS_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ASSETS_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@CURRENT_LIABILITIES_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CURRENT_LIABILITIES_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@LONG_TERM_LIABILITIES_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.LONG_TERM_LIABILITIES_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@LIABILITIES_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.LIABILITIES_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@CAPITAL_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CAPITAL_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@REVALUATION_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.REVALUATION_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@SURPLUS_VALUE_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.SURPLUS_VALUE_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@OTHERS_SURPLUS_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.OTHERS_SURPLUS_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@RESERVES_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.RESERVES_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@PREMIUM_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.PREMIUM_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@ACCUMULATED_PROFIT_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ACCUMULATED_PROFIT_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@PATRIMONY_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.PATRIMONY_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@NET_SALES_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.NET_SALES_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@SALES_COST_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.SALES_COST_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@GROSS_PROFIT_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.GROSS_PROFIT_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@OPERATING_PROFIT_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.OPERATING_PROFIT_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@NET_PROFIT_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.NET_PROFIT_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@OTHERS_INCOME_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.OTHERS_INCOME_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@INFLATION_ADJUSTMENTS_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INFLATION_ADJUSTMENTS_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@INTERESTS_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INTERESTS_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@OTHERS_EXPENSE_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.OTHERS_EXPENSE_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@REGISTRATION_BALANCE_DATE", AseDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.REGISTRATION_BALANCE_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@BALANCE_USER_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.BALANCE_USER_ID)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("FINANCIAL_STATEMENTS::Insert::Error occured.", ex);
            }
        }

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(FINANCIAL_STATEMENTS businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.FINANCIAL_STATEM_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@TECHNICAL_CARD_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.TECHNICAL_CARD_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@BALANCE_DATE", AseDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.BALANCE_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@INVENTORY_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INVENTORY_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@ACCOUNTS_RECEIVABLE_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ACCOUNTS_RECEIVABLE_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@CASH_INVESTMENT_TEMPORARY_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CASH_INVESTMENT_TEMPORARY_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@CURRENT_ASSETS_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CURRENT_ASSETS_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@FIXED_GROSS_ASSETS_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.FIXED_GROSS_ASSETS_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@FIXED_NET_ASSETS_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.FIXED_NET_ASSETS_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@VALUATION_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.VALUATION_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@ASSETS_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ASSETS_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@CURRENT_LIABILITIES_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CURRENT_LIABILITIES_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@LONG_TERM_LIABILITIES_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.LONG_TERM_LIABILITIES_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@LIABILITIES_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.LIABILITIES_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@CAPITAL_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CAPITAL_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@REVALUATION_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.REVALUATION_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@SURPLUS_VALUE_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.SURPLUS_VALUE_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@OTHERS_SURPLUS_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.OTHERS_SURPLUS_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@RESERVES_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.RESERVES_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@PREMIUM_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.PREMIUM_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@ACCUMULATED_PROFIT_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ACCUMULATED_PROFIT_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@PATRIMONY_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.PATRIMONY_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@NET_SALES_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.NET_SALES_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@SALES_COST_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.SALES_COST_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@GROSS_PROFIT_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.GROSS_PROFIT_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@OPERATING_PROFIT_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.OPERATING_PROFIT_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@NET_PROFIT_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.NET_PROFIT_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@OTHERS_INCOME_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.OTHERS_INCOME_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@INFLATION_ADJUSTMENTS_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INFLATION_ADJUSTMENTS_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@INTERESTS_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INTERESTS_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@OTHERS_EXPENSE_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.OTHERS_EXPENSE_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@REGISTRATION_BALANCE_DATE", AseDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.REGISTRATION_BALANCE_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@BALANCE_USER_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.BALANCE_USER_ID)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("FINANCIAL_STATEMENTS::Update::Error occured.", ex);
            }
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(FINANCIAL_STATEMENTS businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.FINANCIAL_STATEM_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@TECHNICAL_CARD_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.TECHNICAL_CARD_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@BALANCE_DATE", AseDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.BALANCE_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@INVENTORY_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INVENTORY_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@ACCOUNTS_RECEIVABLE_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ACCOUNTS_RECEIVABLE_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@CASH_INVESTMENT_TEMPORARY_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CASH_INVESTMENT_TEMPORARY_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@CURRENT_ASSETS_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CURRENT_ASSETS_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@FIXED_GROSS_ASSETS_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.FIXED_GROSS_ASSETS_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@FIXED_NET_ASSETS_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.FIXED_NET_ASSETS_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@VALUATION_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.VALUATION_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@ASSETS_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ASSETS_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@CURRENT_LIABILITIES_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CURRENT_LIABILITIES_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@LONG_TERM_LIABILITIES_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.LONG_TERM_LIABILITIES_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@LIABILITIES_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.LIABILITIES_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@CAPITAL_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CAPITAL_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@REVALUATION_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.REVALUATION_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@SURPLUS_VALUE_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.SURPLUS_VALUE_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@OTHERS_SURPLUS_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.OTHERS_SURPLUS_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@RESERVES_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.RESERVES_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@PREMIUM_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.PREMIUM_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@ACCUMULATED_PROFIT_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ACCUMULATED_PROFIT_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@PATRIMONY_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.PATRIMONY_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@NET_SALES_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.NET_SALES_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@SALES_COST_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.SALES_COST_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@GROSS_PROFIT_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.GROSS_PROFIT_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@OPERATING_PROFIT_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.OPERATING_PROFIT_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@NET_PROFIT_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.NET_PROFIT_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@OTHERS_INCOME_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.OTHERS_INCOME_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@INFLATION_ADJUSTMENTS_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INFLATION_ADJUSTMENTS_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@INTERESTS_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INTERESTS_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@OTHERS_EXPENSE_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.OTHERS_EXPENSE_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@REGISTRATION_BALANCE_DATE", AseDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.REGISTRATION_BALANCE_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@BALANCE_USER_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.BALANCE_USER_ID)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("FINANCIAL_STATEMENTS::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(FINANCIAL_STATEMENTS businessObject, IDataReader dataReader)
        {


            businessObject.TECHNICAL_CARD_ID = dataReader.GetInt32(dataReader.GetOrdinal(FINANCIAL_STATEMENTS.FINANCIAL_STATEMENTSFields.TECHNICAL_CARD_ID.ToString()));

            businessObject.BALANCE_DATE = dataReader.GetString(dataReader.GetOrdinal(FINANCIAL_STATEMENTS.FINANCIAL_STATEMENTSFields.BALANCE_DATE.ToString()));

            businessObject.INVENTORY_AMT = dataReader.GetDouble(dataReader.GetOrdinal(FINANCIAL_STATEMENTS.FINANCIAL_STATEMENTSFields.INVENTORY_AMT.ToString()));

            businessObject.ACCOUNTS_RECEIVABLE_AMT = dataReader.GetDouble(dataReader.GetOrdinal(FINANCIAL_STATEMENTS.FINANCIAL_STATEMENTSFields.ACCOUNTS_RECEIVABLE_AMT.ToString()));

            businessObject.CASH_INVESTMENT_TEMPORARY_AMT = dataReader.GetDouble(dataReader.GetOrdinal(FINANCIAL_STATEMENTS.FINANCIAL_STATEMENTSFields.CASH_INVESTMENT_TEMPORARY_AMT.ToString()));

            businessObject.CURRENT_ASSETS_AMT = dataReader.GetDouble(dataReader.GetOrdinal(FINANCIAL_STATEMENTS.FINANCIAL_STATEMENTSFields.CURRENT_ASSETS_AMT.ToString()));

            businessObject.FIXED_GROSS_ASSETS_AMT = dataReader.GetDouble(dataReader.GetOrdinal(FINANCIAL_STATEMENTS.FINANCIAL_STATEMENTSFields.FIXED_GROSS_ASSETS_AMT.ToString()));

            businessObject.FIXED_NET_ASSETS_AMT = dataReader.GetDouble(dataReader.GetOrdinal(FINANCIAL_STATEMENTS.FINANCIAL_STATEMENTSFields.FIXED_NET_ASSETS_AMT.ToString()));

            businessObject.VALUATION_AMT = dataReader.GetDouble(dataReader.GetOrdinal(FINANCIAL_STATEMENTS.FINANCIAL_STATEMENTSFields.VALUATION_AMT.ToString()));

            businessObject.ASSETS_AMT = dataReader.GetDouble(dataReader.GetOrdinal(FINANCIAL_STATEMENTS.FINANCIAL_STATEMENTSFields.ASSETS_AMT.ToString()));

            businessObject.CURRENT_LIABILITIES_AMT = dataReader.GetDouble(dataReader.GetOrdinal(FINANCIAL_STATEMENTS.FINANCIAL_STATEMENTSFields.CURRENT_LIABILITIES_AMT.ToString()));

            businessObject.LONG_TERM_LIABILITIES_AMT = dataReader.GetDouble(dataReader.GetOrdinal(FINANCIAL_STATEMENTS.FINANCIAL_STATEMENTSFields.LONG_TERM_LIABILITIES_AMT.ToString()));

            businessObject.LIABILITIES_AMT = dataReader.GetDouble(dataReader.GetOrdinal(FINANCIAL_STATEMENTS.FINANCIAL_STATEMENTSFields.LIABILITIES_AMT.ToString()));

            businessObject.CAPITAL_AMT = dataReader.GetDouble(dataReader.GetOrdinal(FINANCIAL_STATEMENTS.FINANCIAL_STATEMENTSFields.CAPITAL_AMT.ToString()));

            businessObject.REVALUATION_AMT = dataReader.GetDouble(dataReader.GetOrdinal(FINANCIAL_STATEMENTS.FINANCIAL_STATEMENTSFields.REVALUATION_AMT.ToString()));

            businessObject.SURPLUS_VALUE_AMT = dataReader.GetDouble(dataReader.GetOrdinal(FINANCIAL_STATEMENTS.FINANCIAL_STATEMENTSFields.SURPLUS_VALUE_AMT.ToString()));

            businessObject.OTHERS_SURPLUS_AMT = dataReader.GetDouble(dataReader.GetOrdinal(FINANCIAL_STATEMENTS.FINANCIAL_STATEMENTSFields.OTHERS_SURPLUS_AMT.ToString()));

            businessObject.RESERVES_AMT = dataReader.GetDouble(dataReader.GetOrdinal(FINANCIAL_STATEMENTS.FINANCIAL_STATEMENTSFields.RESERVES_AMT.ToString()));

            businessObject.PREMIUM_AMT = dataReader.GetDouble(dataReader.GetOrdinal(FINANCIAL_STATEMENTS.FINANCIAL_STATEMENTSFields.PREMIUM_AMT.ToString()));

            businessObject.ACCUMULATED_PROFIT_AMT = dataReader.GetDouble(dataReader.GetOrdinal(FINANCIAL_STATEMENTS.FINANCIAL_STATEMENTSFields.ACCUMULATED_PROFIT_AMT.ToString()));

            businessObject.PATRIMONY_AMT = dataReader.GetDouble(dataReader.GetOrdinal(FINANCIAL_STATEMENTS.FINANCIAL_STATEMENTSFields.PATRIMONY_AMT.ToString()));

            businessObject.NET_SALES_AMT = dataReader.GetDouble(dataReader.GetOrdinal(FINANCIAL_STATEMENTS.FINANCIAL_STATEMENTSFields.NET_SALES_AMT.ToString()));

            businessObject.SALES_COST_AMT = dataReader.GetDouble(dataReader.GetOrdinal(FINANCIAL_STATEMENTS.FINANCIAL_STATEMENTSFields.SALES_COST_AMT.ToString()));

            businessObject.GROSS_PROFIT_AMT = dataReader.GetDouble(dataReader.GetOrdinal(FINANCIAL_STATEMENTS.FINANCIAL_STATEMENTSFields.GROSS_PROFIT_AMT.ToString()));

            businessObject.OPERATING_PROFIT_AMT = dataReader.GetDouble(dataReader.GetOrdinal(FINANCIAL_STATEMENTS.FINANCIAL_STATEMENTSFields.OPERATING_PROFIT_AMT.ToString()));

            businessObject.NET_PROFIT_AMT = dataReader.GetDouble(dataReader.GetOrdinal(FINANCIAL_STATEMENTS.FINANCIAL_STATEMENTSFields.NET_PROFIT_AMT.ToString()));

            businessObject.OTHERS_INCOME_AMT = dataReader.GetDouble(dataReader.GetOrdinal(FINANCIAL_STATEMENTS.FINANCIAL_STATEMENTSFields.OTHERS_INCOME_AMT.ToString()));

            businessObject.INFLATION_ADJUSTMENTS_AMT = dataReader.GetDouble(dataReader.GetOrdinal(FINANCIAL_STATEMENTS.FINANCIAL_STATEMENTSFields.INFLATION_ADJUSTMENTS_AMT.ToString()));

            businessObject.INTERESTS_AMT = dataReader.GetDouble(dataReader.GetOrdinal(FINANCIAL_STATEMENTS.FINANCIAL_STATEMENTSFields.INTERESTS_AMT.ToString()));

            businessObject.OTHERS_EXPENSE_AMT = dataReader.GetDouble(dataReader.GetOrdinal(FINANCIAL_STATEMENTS.FINANCIAL_STATEMENTSFields.OTHERS_EXPENSE_AMT.ToString()));

            businessObject.REGISTRATION_BALANCE_DATE = dataReader.GetString(dataReader.GetOrdinal(FINANCIAL_STATEMENTS.FINANCIAL_STATEMENTSFields.REGISTRATION_BALANCE_DATE.ToString()));

            businessObject.BALANCE_USER_ID = dataReader.GetInt32(dataReader.GetOrdinal(FINANCIAL_STATEMENTS.FINANCIAL_STATEMENTSFields.BALANCE_USER_ID.ToString()));


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of FINANCIAL_STATEMENTS</returns>
        internal List<FINANCIAL_STATEMENTS> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<FINANCIAL_STATEMENTS> list = new List<FINANCIAL_STATEMENTS>();

            while (dataReader.Read())
            {
                FINANCIAL_STATEMENTS businessObject = new FINANCIAL_STATEMENTS();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

    }
}
