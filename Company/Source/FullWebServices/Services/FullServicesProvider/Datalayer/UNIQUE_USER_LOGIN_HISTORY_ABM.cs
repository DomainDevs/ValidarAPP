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
    /// Data access layer class for UNIQUE_USER_LOGIN_HISTORY
    /// </summary>
    class UNIQUE_USER_LOGIN_HISTORY_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public UNIQUE_USER_LOGIN_HISTORY_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public UNIQUE_USER_LOGIN_HISTORY_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public UNIQUE_USER_LOGIN_HISTORY_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
        public bool Insert(UNIQUE_USER_LOGIN_HISTORY businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.UNIQ_USR_LOG_HIST_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            
            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@USER_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.USER_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@CHANGE_DATE", AseDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CHANGE_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@PASSWORD", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.PASSWORD)));
                sqlCommand.Parameters.Add(new AseParameter("@PASSWORD_INITIALIZED", AseDbType.Bit, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.PASSWORD_INITIALIZED)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));

                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("UNIQUE_USER_LOGIN_HISTORY::Insert::Error occured.", ex);
            }       
        }

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(UNIQUE_USER_LOGIN_HISTORY businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.UNIQ_USR_LOG_HIST_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;
                        
            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@USER_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.USER_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@CHANGE_DATE", AseDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CHANGE_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@PASSWORD", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.PASSWORD)));
                sqlCommand.Parameters.Add(new AseParameter("@PASSWORD_INITIALIZED", AseDbType.Bit, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.PASSWORD_INITIALIZED)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));
                                
                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("UNIQUE_USER_LOGIN_HISTORY::Update::Error occured.", ex);
            }    
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(UNIQUE_USER_LOGIN_HISTORY businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.UNIQ_USR_LOG_HIST_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@USER_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.USER_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@CHANGE_DATE", AseDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CHANGE_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@PASSWORD", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.PASSWORD)));
                sqlCommand.Parameters.Add(new AseParameter("@PASSWORD_INITIALIZED", AseDbType.Bit, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.PASSWORD_INITIALIZED)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));

               

                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("UNIQUE_USER_LOGIN_HISTORY::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(UNIQUE_USER_LOGIN_HISTORY businessObject, IDataReader dataReader)
        {


            businessObject.USER_ID = dataReader.GetInt32(dataReader.GetOrdinal(UNIQUE_USER_LOGIN_HISTORY.UNIQUE_USER_LOGIN_HISTORYFields.USER_ID.ToString()));

            businessObject.CHANGE_DATE = dataReader.GetString(dataReader.GetOrdinal(UNIQUE_USER_LOGIN_HISTORY.UNIQUE_USER_LOGIN_HISTORYFields.CHANGE_DATE.ToString()));

            businessObject.PASSWORD = dataReader.GetString(dataReader.GetOrdinal(UNIQUE_USER_LOGIN_HISTORY.UNIQUE_USER_LOGIN_HISTORYFields.PASSWORD.ToString()));

            businessObject.PASSWORD_INITIALIZED = dataReader.GetBoolean(dataReader.GetOrdinal(UNIQUE_USER_LOGIN_HISTORY.UNIQUE_USER_LOGIN_HISTORYFields.PASSWORD_INITIALIZED.ToString()));


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of UNIQUE_USER_LOGIN_HISTORY</returns>
        internal List<UNIQUE_USER_LOGIN_HISTORY> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<UNIQUE_USER_LOGIN_HISTORY> list = new List<UNIQUE_USER_LOGIN_HISTORY>();

            while (dataReader.Read())
            {
                UNIQUE_USER_LOGIN_HISTORY businessObject = new UNIQUE_USER_LOGIN_HISTORY();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

    }
}
