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
    /// Data access layer class for CO_INSURED
    /// </summary>
    class CO_INSURED_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public CO_INSURED_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public CO_INSURED_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public CO_INSURED_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
        public bool Insert(CO_INSURED businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.CO_INSURED_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@SIGNING_DATE", AseDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.SIGNING_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@AUTHORIZED_BY", AseDbType.VarChar, 40, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.AUTHORIZED_BY)));
                sqlCommand.Parameters.Add(new AseParameter("@ID_CARD_NO", AseDbType.VarChar, 14, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ID_CARD_NO)));
                sqlCommand.Parameters.Add(new AseParameter("@ID_CARD_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ID_CARD_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@USER_CREATE", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.USER_CREATE)));
                sqlCommand.Parameters.Add(new AseParameter("@CREATE_DATE", AseDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CREATE_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@LAST_UPDATE_USER", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.LAST_UPDATE_USER)));
                sqlCommand.Parameters.Add(new AseParameter("@LAST_UPDATE_DATE", AseDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.LAST_UPDATE_DATE, "dd/MM/yyyy")));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("CO_INSURED::Insert::Error occured.", ex);
            }
        }

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(CO_INSURED businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.CO_INSURED_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@SIGNING_DATE", AseDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.SIGNING_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@AUTHORIZED_BY", AseDbType.VarChar, 40, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.AUTHORIZED_BY)));
                sqlCommand.Parameters.Add(new AseParameter("@ID_CARD_NO", AseDbType.VarChar, 14, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ID_CARD_NO)));
                sqlCommand.Parameters.Add(new AseParameter("@ID_CARD_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ID_CARD_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@USER_CREATE", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.USER_CREATE)));
                sqlCommand.Parameters.Add(new AseParameter("@CREATE_DATE", AseDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CREATE_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@LAST_UPDATE_USER", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.LAST_UPDATE_USER)));
                sqlCommand.Parameters.Add(new AseParameter("@LAST_UPDATE_DATE", AseDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.LAST_UPDATE_DATE, "dd/MM/yyyy")));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("CO_INSURED::Update::Error occured.", ex);
            }
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(CO_INSURED businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.CO_INSURED_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@SIGNING_DATE", AseDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.SIGNING_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@AUTHORIZED_BY", AseDbType.VarChar, 40, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.AUTHORIZED_BY)));
                sqlCommand.Parameters.Add(new AseParameter("@ID_CARD_NO", AseDbType.VarChar, 14, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ID_CARD_NO)));
                sqlCommand.Parameters.Add(new AseParameter("@ID_CARD_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ID_CARD_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@USER_CREATE", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.USER_CREATE)));
                sqlCommand.Parameters.Add(new AseParameter("@CREATE_DATE", AseDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CREATE_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@LAST_UPDATE_USER", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.LAST_UPDATE_USER)));
                sqlCommand.Parameters.Add(new AseParameter("@LAST_UPDATE_DATE", AseDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.LAST_UPDATE_DATE, "dd/MM/yyyy")));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("CO_INSURED::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(CO_INSURED businessObject, IDataReader dataReader)
        {


            businessObject.INDIVIDUAL_ID = dataReader.GetInt32(dataReader.GetOrdinal(CO_INSURED.CO_INSUREDFields.INDIVIDUAL_ID.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(CO_INSURED.CO_INSUREDFields.SIGNING_DATE.ToString())))
            {
                businessObject.SIGNING_DATE = dataReader.GetString(dataReader.GetOrdinal(CO_INSURED.CO_INSUREDFields.SIGNING_DATE.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(CO_INSURED.CO_INSUREDFields.AUTHORIZED_BY.ToString())))
            {
                businessObject.AUTHORIZED_BY = dataReader.GetString(dataReader.GetOrdinal(CO_INSURED.CO_INSUREDFields.AUTHORIZED_BY.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(CO_INSURED.CO_INSUREDFields.ID_CARD_NO.ToString())))
            {
                businessObject.ID_CARD_NO = dataReader.GetString(dataReader.GetOrdinal(CO_INSURED.CO_INSUREDFields.ID_CARD_NO.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(CO_INSURED.CO_INSUREDFields.ID_CARD_TYPE_CD.ToString())))
            {
                businessObject.ID_CARD_TYPE_CD = dataReader.GetString(dataReader.GetOrdinal(CO_INSURED.CO_INSUREDFields.ID_CARD_TYPE_CD.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(CO_INSURED.CO_INSUREDFields.USER_CREATE.ToString())))
            {
                businessObject.USER_CREATE = dataReader.GetString(dataReader.GetOrdinal(CO_INSURED.CO_INSUREDFields.USER_CREATE.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(CO_INSURED.CO_INSUREDFields.CREATE_DATE.ToString())))
            {
                businessObject.CREATE_DATE = dataReader.GetString(dataReader.GetOrdinal(CO_INSURED.CO_INSUREDFields.CREATE_DATE.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(CO_INSURED.CO_INSUREDFields.LAST_UPDATE_USER.ToString())))
            {
                businessObject.LAST_UPDATE_USER = dataReader.GetString(dataReader.GetOrdinal(CO_INSURED.CO_INSUREDFields.LAST_UPDATE_USER.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(CO_INSURED.CO_INSUREDFields.LAST_UPDATE_DATE.ToString())))
            {
                businessObject.LAST_UPDATE_DATE = dataReader.GetString(dataReader.GetOrdinal(CO_INSURED.CO_INSUREDFields.LAST_UPDATE_DATE.ToString()));
            }


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of CO_INSURED</returns>
        internal List<CO_INSURED> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<CO_INSURED> list = new List<CO_INSURED>();

            while (dataReader.Read())
            {
                CO_INSURED businessObject = new CO_INSURED();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

    }
}
