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
    /// Data access layer class for FINANCIAL_SARLAFT
    /// </summary>
    class FINANCIAL_SARLAFT_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public FINANCIAL_SARLAFT_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public FINANCIAL_SARLAFT_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public FINANCIAL_SARLAFT_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
        public bool Insert(FINANCIAL_SARLAFT businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.FINANCIAL_SARLAFT_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@SARLAFT_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.SARLAFT_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@INCOME_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INCOME_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@EXPENSE_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.EXPENSE_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@EXTRA_INCOME_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.EXTRA_INCOME_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@ASSETS_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ASSETS_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@LIABILITIES_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.LIABILITIES_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@DESCRIPTION", AseDbType.VarChar, 255, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.DESCRIPTION)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("FINANCIAL_SARLAFT::Insert::Error occured.", ex);
            }
        }

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(FINANCIAL_SARLAFT businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.FINANCIAL_SARLAFT_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@SARLAFT_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.SARLAFT_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@INCOME_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INCOME_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@EXPENSE_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.EXPENSE_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@EXTRA_INCOME_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.EXTRA_INCOME_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@ASSETS_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ASSETS_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@LIABILITIES_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.LIABILITIES_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@DESCRIPTION", AseDbType.VarChar, 255, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.DESCRIPTION)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("FINANCIAL_SARLAFT::Update::Error occured.", ex);
            }
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(FINANCIAL_SARLAFT businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.FINANCIAL_SARLAFT_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@SARLAFT_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.SARLAFT_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@INCOME_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INCOME_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@EXPENSE_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.EXPENSE_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@EXTRA_INCOME_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.EXTRA_INCOME_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@ASSETS_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ASSETS_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@LIABILITIES_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.LIABILITIES_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@DESCRIPTION", AseDbType.VarChar, 255, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.DESCRIPTION)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("FINANCIAL_SARLAFT::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(FINANCIAL_SARLAFT businessObject, IDataReader dataReader)
        {


            businessObject.SARLAFT_ID = dataReader.GetInt32(dataReader.GetOrdinal(FINANCIAL_SARLAFT.FINANCIAL_SARLAFTFields.SARLAFT_ID.ToString()));

            businessObject.INCOME_AMT = dataReader.GetDouble(dataReader.GetOrdinal(FINANCIAL_SARLAFT.FINANCIAL_SARLAFTFields.INCOME_AMT.ToString()));

            businessObject.EXPENSE_AMT = dataReader.GetDouble(dataReader.GetOrdinal(FINANCIAL_SARLAFT.FINANCIAL_SARLAFTFields.EXPENSE_AMT.ToString()));

            businessObject.EXTRA_INCOME_AMT = dataReader.GetDouble(dataReader.GetOrdinal(FINANCIAL_SARLAFT.FINANCIAL_SARLAFTFields.EXTRA_INCOME_AMT.ToString()));

            businessObject.ASSETS_AMT = dataReader.GetDouble(dataReader.GetOrdinal(FINANCIAL_SARLAFT.FINANCIAL_SARLAFTFields.ASSETS_AMT.ToString()));

            businessObject.LIABILITIES_AMT = dataReader.GetDouble(dataReader.GetOrdinal(FINANCIAL_SARLAFT.FINANCIAL_SARLAFTFields.LIABILITIES_AMT.ToString()));

            businessObject.DESCRIPTION = dataReader.GetString(dataReader.GetOrdinal(FINANCIAL_SARLAFT.FINANCIAL_SARLAFTFields.DESCRIPTION.ToString()));


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of FINANCIAL_SARLAFT</returns>
        internal List<FINANCIAL_SARLAFT> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<FINANCIAL_SARLAFT> list = new List<FINANCIAL_SARLAFT>();

            while (dataReader.Read())
            {
                FINANCIAL_SARLAFT businessObject = new FINANCIAL_SARLAFT();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

    }
}
