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
    /// Data access layer class for UNIQUE_USERS
    /// </summary>
    class UNIQUE_USERS_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public UNIQUE_USERS_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public UNIQUE_USERS_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public UNIQUE_USERS_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
        public bool Insert(UNIQUE_USERS businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.UNIQUE_USERS_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            
            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@USER_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.USER_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@ACCOUNT_NAME", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ACCOUNT_NAME)));
                sqlCommand.Parameters.Add(new AseParameter("@PERSON_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.PERSON_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@AUTHENTICATION_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.AUTHENTICATION_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@USER_DOMAIN", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.USER_DOMAIN)));
                sqlCommand.Parameters.Add(new AseParameter("@DISABLED_DATE", AseDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.DISABLED_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@LOCK_DATE", AseDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.LOCK_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@EXPIRATION_DATE", AseDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.EXPIRATION_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@LOCK_PASSWORD", AseDbType.Bit, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.LOCK_PASSWORD)));
                sqlCommand.Parameters.Add(new AseParameter("@ACTIVATION_DATE", AseDbType.Bit, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ACTIVATION_DATE)));
                sqlCommand.Parameters.Add(new AseParameter("@CREATED_DATE", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CREATED_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@CREATED_USER_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CREATED_USER_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@MODIFIED_DATE", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.MODIFIED_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@MODIFIED_USER_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.MODIFIED_USER_ID)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));

                
                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("UNIQUE_USERS::Insert::Error occured.", ex);
            }            
        }

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(UNIQUE_USERS businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.UNIQUE_USERS_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;

         
            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@USER_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.USER_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@ACCOUNT_NAME", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ACCOUNT_NAME)));
                sqlCommand.Parameters.Add(new AseParameter("@PERSON_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.PERSON_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@AUTHENTICATION_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.AUTHENTICATION_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@USER_DOMAIN", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.USER_DOMAIN)));
                sqlCommand.Parameters.Add(new AseParameter("@DISABLED_DATE", AseDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.DISABLED_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@LOCK_DATE", AseDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.LOCK_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@EXPIRATION_DATE", AseDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.EXPIRATION_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@LOCK_PASSWORD", AseDbType.Bit, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.LOCK_PASSWORD)));
                sqlCommand.Parameters.Add(new AseParameter("@ACTIVATION_DATE", AseDbType.Bit, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ACTIVATION_DATE)));
                sqlCommand.Parameters.Add(new AseParameter("@CREATED_DATE", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CREATED_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@CREATED_USER_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CREATED_USER_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@MODIFIED_DATE", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.MODIFIED_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@MODIFIED_USER_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.MODIFIED_USER_ID)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));
                              

                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("UNIQUE_USERS::Update::Error occured.", ex);
            }         
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(UNIQUE_USERS businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.UNIQUE_USERS_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@USER_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.USER_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@ACCOUNT_NAME", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ACCOUNT_NAME)));
                sqlCommand.Parameters.Add(new AseParameter("@PERSON_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.PERSON_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@AUTHENTICATION_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.AUTHENTICATION_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@USER_DOMAIN", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.USER_DOMAIN)));
                sqlCommand.Parameters.Add(new AseParameter("@DISABLED_DATE", AseDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.DISABLED_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@LOCK_DATE", AseDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.LOCK_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@EXPIRATION_DATE", AseDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.EXPIRATION_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@LOCK_PASSWORD", AseDbType.Bit, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.LOCK_PASSWORD)));
                sqlCommand.Parameters.Add(new AseParameter("@ACTIVATION_DATE", AseDbType.Bit, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ACTIVATION_DATE)));
                sqlCommand.Parameters.Add(new AseParameter("@CREATED_DATE", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CREATED_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@CREATED_USER_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CREATED_USER_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@MODIFIED_DATE", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.MODIFIED_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@MODIFIED_USER_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.MODIFIED_USER_ID)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));

               

                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("UNIQUE_USERS::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(UNIQUE_USERS businessObject, IDataReader dataReader)
        {


            businessObject.USER_ID = dataReader.GetInt32(dataReader.GetOrdinal(UNIQUE_USERS.UNIQUE_USERSFields.USER_ID.ToString()));

            businessObject.ACCOUNT_NAME = dataReader.GetString(dataReader.GetOrdinal(UNIQUE_USERS.UNIQUE_USERSFields.ACCOUNT_NAME.ToString()));

            businessObject.PERSON_ID = dataReader.GetInt32(dataReader.GetOrdinal(UNIQUE_USERS.UNIQUE_USERSFields.PERSON_ID.ToString()));

            businessObject.AUTHENTICATION_TYPE_CD = dataReader.GetInt32(dataReader.GetOrdinal(UNIQUE_USERS.UNIQUE_USERSFields.AUTHENTICATION_TYPE_CD.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(UNIQUE_USERS.UNIQUE_USERSFields.USER_DOMAIN.ToString())))
            {
                businessObject.USER_DOMAIN = dataReader.GetString(dataReader.GetOrdinal(UNIQUE_USERS.UNIQUE_USERSFields.USER_DOMAIN.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(UNIQUE_USERS.UNIQUE_USERSFields.DISABLED_DATE.ToString())))
            {
                businessObject.DISABLED_DATE = dataReader.GetString(dataReader.GetOrdinal(UNIQUE_USERS.UNIQUE_USERSFields.DISABLED_DATE.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(UNIQUE_USERS.UNIQUE_USERSFields.LOCK_DATE.ToString())))
            {
                businessObject.LOCK_DATE = dataReader.GetString(dataReader.GetOrdinal(UNIQUE_USERS.UNIQUE_USERSFields.LOCK_DATE.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(UNIQUE_USERS.UNIQUE_USERSFields.EXPIRATION_DATE.ToString())))
            {
                businessObject.EXPIRATION_DATE = dataReader.GetString(dataReader.GetOrdinal(UNIQUE_USERS.UNIQUE_USERSFields.EXPIRATION_DATE.ToString()));
            }

            businessObject.LOCK_PASSWORD = dataReader.GetBoolean(dataReader.GetOrdinal(UNIQUE_USERS.UNIQUE_USERSFields.LOCK_PASSWORD.ToString()));

            businessObject.ACTIVATION_DATE = dataReader.GetBoolean(dataReader.GetOrdinal(UNIQUE_USERS.UNIQUE_USERSFields.ACTIVATION_DATE.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(UNIQUE_USERS.UNIQUE_USERSFields.CREATED_DATE.ToString())))
            {
                businessObject.CREATED_DATE = dataReader.GetString(dataReader.GetOrdinal(UNIQUE_USERS.UNIQUE_USERSFields.CREATED_DATE.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(UNIQUE_USERS.UNIQUE_USERSFields.CREATED_USER_ID.ToString())))
            {
                businessObject.CREATED_USER_ID = dataReader.GetString(dataReader.GetOrdinal(UNIQUE_USERS.UNIQUE_USERSFields.CREATED_USER_ID.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(UNIQUE_USERS.UNIQUE_USERSFields.MODIFIED_DATE.ToString())))
            {
                businessObject.MODIFIED_DATE = dataReader.GetString(dataReader.GetOrdinal(UNIQUE_USERS.UNIQUE_USERSFields.MODIFIED_DATE.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(UNIQUE_USERS.UNIQUE_USERSFields.MODIFIED_USER_ID.ToString())))
            {
                businessObject.MODIFIED_USER_ID = dataReader.GetString(dataReader.GetOrdinal(UNIQUE_USERS.UNIQUE_USERSFields.MODIFIED_USER_ID.ToString()));
            }


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of UNIQUE_USERS</returns>
        internal List<UNIQUE_USERS> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<UNIQUE_USERS> list = new List<UNIQUE_USERS>();

            while (dataReader.Read())
            {
                UNIQUE_USERS businessObject = new UNIQUE_USERS();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

    }
}
