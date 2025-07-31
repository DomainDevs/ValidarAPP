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
    /// Data access layer class for INDIVIDUAL_LEGAL_REPRESENT
    /// </summary>
    class INDIVIDUAL_LEGAL_REPRESENT_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public INDIVIDUAL_LEGAL_REPRESENT_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public INDIVIDUAL_LEGAL_REPRESENT_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public INDIVIDUAL_LEGAL_REPRESENT_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
        public bool Insert(INDIVIDUAL_LEGAL_REPRESENT businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.INDIV_LEGAL_REPR_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@LEGAL_REPRESENTATIVE_NAME", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.LEGAL_REPRESENTATIVE_NAME)));
                sqlCommand.Parameters.Add(new AseParameter("@EXPEDITION_DATE", AseDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.EXPEDITION_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@EXPEDITION_PLACE", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.EXPEDITION_PLACE)));
                sqlCommand.Parameters.Add(new AseParameter("@BIRTH_DATE", AseDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.BIRTH_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@BIRTH_PLACE", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.BIRTH_PLACE)));
                sqlCommand.Parameters.Add(new AseParameter("@NATIONALITY", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.NATIONALITY)));
                sqlCommand.Parameters.Add(new AseParameter("@CITY", AseDbType.VarChar, 25, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CITY)));
                sqlCommand.Parameters.Add(new AseParameter("@PHONE", AseDbType.BigInt, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.PHONE)));
                sqlCommand.Parameters.Add(new AseParameter("@JOB_TITLE", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.JOB_TITLE)));
                sqlCommand.Parameters.Add(new AseParameter("@CELL_PHONE", AseDbType.BigInt, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CELL_PHONE)));
                sqlCommand.Parameters.Add(new AseParameter("@EMAIL", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.EMAIL)));
                sqlCommand.Parameters.Add(new AseParameter("@ADDRESS", AseDbType.VarChar, 180, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ADDRESS)));
                sqlCommand.Parameters.Add(new AseParameter("@ID_CARD_NO", AseDbType.VarChar, 14, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ID_CARD_NO)));
                sqlCommand.Parameters.Add(new AseParameter("@AUTHORIZATION_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.AUTHORIZATION_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@DESCRIPTION", AseDbType.VarChar, 255, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.DESCRIPTION)));
                sqlCommand.Parameters.Add(new AseParameter("@CURRENCY_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CURRENCY_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@ID_CARD_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ID_CARD_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@COUNTRY_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.COUNTRY_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@STATE_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.STATE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@CITY_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CITY_CD)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("INDIVIDUAL_LEGAL_REPRESENT::Insert::Error occured.", ex);
            }
        }

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(INDIVIDUAL_LEGAL_REPRESENT businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.INDIV_LEGAL_REPR_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@LEGAL_REPRESENTATIVE_NAME", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.LEGAL_REPRESENTATIVE_NAME)));
                sqlCommand.Parameters.Add(new AseParameter("@EXPEDITION_DATE", AseDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.EXPEDITION_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@EXPEDITION_PLACE", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.EXPEDITION_PLACE)));
                sqlCommand.Parameters.Add(new AseParameter("@BIRTH_DATE", AseDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.BIRTH_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@BIRTH_PLACE", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.BIRTH_PLACE)));
                sqlCommand.Parameters.Add(new AseParameter("@NATIONALITY", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.NATIONALITY)));
                sqlCommand.Parameters.Add(new AseParameter("@CITY", AseDbType.VarChar, 25, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CITY)));
                sqlCommand.Parameters.Add(new AseParameter("@PHONE", AseDbType.BigInt, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.PHONE)));
                sqlCommand.Parameters.Add(new AseParameter("@JOB_TITLE", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.JOB_TITLE)));
                sqlCommand.Parameters.Add(new AseParameter("@CELL_PHONE", AseDbType.BigInt, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CELL_PHONE)));
                sqlCommand.Parameters.Add(new AseParameter("@EMAIL", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.EMAIL)));
                sqlCommand.Parameters.Add(new AseParameter("@ADDRESS", AseDbType.VarChar, 180, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ADDRESS)));
                sqlCommand.Parameters.Add(new AseParameter("@ID_CARD_NO", AseDbType.VarChar, 14, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ID_CARD_NO)));
                sqlCommand.Parameters.Add(new AseParameter("@AUTHORIZATION_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.AUTHORIZATION_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@DESCRIPTION", AseDbType.VarChar, 255, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.DESCRIPTION)));
                sqlCommand.Parameters.Add(new AseParameter("@CURRENCY_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CURRENCY_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@ID_CARD_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ID_CARD_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@COUNTRY_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.COUNTRY_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@STATE_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.STATE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@CITY_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CITY_CD)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("INDIVIDUAL_LEGAL_REPRESENT::Update::Error occured.", ex);
            }
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(INDIVIDUAL_LEGAL_REPRESENT businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.INDIV_LEGAL_REPR_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@LEGAL_REPRESENTATIVE_NAME", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.LEGAL_REPRESENTATIVE_NAME)));
                sqlCommand.Parameters.Add(new AseParameter("@EXPEDITION_DATE", AseDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.EXPEDITION_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@EXPEDITION_PLACE", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.EXPEDITION_PLACE)));
                sqlCommand.Parameters.Add(new AseParameter("@BIRTH_DATE", AseDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.BIRTH_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@BIRTH_PLACE", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.BIRTH_PLACE)));
                sqlCommand.Parameters.Add(new AseParameter("@NATIONALITY", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.NATIONALITY)));
                sqlCommand.Parameters.Add(new AseParameter("@CITY", AseDbType.VarChar, 25, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CITY)));
                sqlCommand.Parameters.Add(new AseParameter("@PHONE", AseDbType.BigInt, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.PHONE)));
                sqlCommand.Parameters.Add(new AseParameter("@JOB_TITLE", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.JOB_TITLE)));
                sqlCommand.Parameters.Add(new AseParameter("@CELL_PHONE", AseDbType.BigInt, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CELL_PHONE)));
                sqlCommand.Parameters.Add(new AseParameter("@EMAIL", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.EMAIL)));
                sqlCommand.Parameters.Add(new AseParameter("@ADDRESS", AseDbType.VarChar, 180, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ADDRESS)));
                sqlCommand.Parameters.Add(new AseParameter("@ID_CARD_NO", AseDbType.VarChar, 14, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ID_CARD_NO)));
                sqlCommand.Parameters.Add(new AseParameter("@AUTHORIZATION_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.AUTHORIZATION_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@DESCRIPTION", AseDbType.VarChar, 255, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.DESCRIPTION)));
                sqlCommand.Parameters.Add(new AseParameter("@CURRENCY_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CURRENCY_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@ID_CARD_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ID_CARD_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@COUNTRY_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.COUNTRY_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@STATE_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.STATE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@CITY_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CITY_CD)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("INDIVIDUAL_LEGAL_REPRESENT::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(INDIVIDUAL_LEGAL_REPRESENT businessObject, IDataReader dataReader)
        {


            businessObject.INDIVIDUAL_ID = dataReader.GetInt32(dataReader.GetOrdinal(INDIVIDUAL_LEGAL_REPRESENT.INDIVIDUAL_LEGAL_REPRESENTFields.INDIVIDUAL_ID.ToString()));

            businessObject.LEGAL_REPRESENTATIVE_NAME = dataReader.GetString(dataReader.GetOrdinal(INDIVIDUAL_LEGAL_REPRESENT.INDIVIDUAL_LEGAL_REPRESENTFields.LEGAL_REPRESENTATIVE_NAME.ToString()));

            businessObject.EXPEDITION_DATE = dataReader.GetString(dataReader.GetOrdinal(INDIVIDUAL_LEGAL_REPRESENT.INDIVIDUAL_LEGAL_REPRESENTFields.EXPEDITION_DATE.ToString()));

            businessObject.EXPEDITION_PLACE = dataReader.GetString(dataReader.GetOrdinal(INDIVIDUAL_LEGAL_REPRESENT.INDIVIDUAL_LEGAL_REPRESENTFields.EXPEDITION_PLACE.ToString()));

            businessObject.BIRTH_DATE = dataReader.GetString(dataReader.GetOrdinal(INDIVIDUAL_LEGAL_REPRESENT.INDIVIDUAL_LEGAL_REPRESENTFields.BIRTH_DATE.ToString()));

            businessObject.BIRTH_PLACE = dataReader.GetString(dataReader.GetOrdinal(INDIVIDUAL_LEGAL_REPRESENT.INDIVIDUAL_LEGAL_REPRESENTFields.BIRTH_PLACE.ToString()));

            businessObject.NATIONALITY = dataReader.GetString(dataReader.GetOrdinal(INDIVIDUAL_LEGAL_REPRESENT.INDIVIDUAL_LEGAL_REPRESENTFields.NATIONALITY.ToString()));

            businessObject.CITY = dataReader.GetString(dataReader.GetOrdinal(INDIVIDUAL_LEGAL_REPRESENT.INDIVIDUAL_LEGAL_REPRESENTFields.CITY.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(INDIVIDUAL_LEGAL_REPRESENT.INDIVIDUAL_LEGAL_REPRESENTFields.PHONE.ToString())))
            {
                businessObject.PHONE = dataReader.GetString(dataReader.GetOrdinal(INDIVIDUAL_LEGAL_REPRESENT.INDIVIDUAL_LEGAL_REPRESENTFields.PHONE.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(INDIVIDUAL_LEGAL_REPRESENT.INDIVIDUAL_LEGAL_REPRESENTFields.JOB_TITLE.ToString())))
            {
                businessObject.JOB_TITLE = dataReader.GetString(dataReader.GetOrdinal(INDIVIDUAL_LEGAL_REPRESENT.INDIVIDUAL_LEGAL_REPRESENTFields.JOB_TITLE.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(INDIVIDUAL_LEGAL_REPRESENT.INDIVIDUAL_LEGAL_REPRESENTFields.CELL_PHONE.ToString())))
            {
                businessObject.CELL_PHONE = dataReader.GetString(dataReader.GetOrdinal(INDIVIDUAL_LEGAL_REPRESENT.INDIVIDUAL_LEGAL_REPRESENTFields.CELL_PHONE.ToString()));
            }

            businessObject.EMAIL = dataReader.GetString(dataReader.GetOrdinal(INDIVIDUAL_LEGAL_REPRESENT.INDIVIDUAL_LEGAL_REPRESENTFields.EMAIL.ToString()));

            businessObject.ADDRESS = dataReader.GetString(dataReader.GetOrdinal(INDIVIDUAL_LEGAL_REPRESENT.INDIVIDUAL_LEGAL_REPRESENTFields.ADDRESS.ToString()));

            businessObject.ID_CARD_NO = dataReader.GetString(dataReader.GetOrdinal(INDIVIDUAL_LEGAL_REPRESENT.INDIVIDUAL_LEGAL_REPRESENTFields.ID_CARD_NO.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(INDIVIDUAL_LEGAL_REPRESENT.INDIVIDUAL_LEGAL_REPRESENTFields.AUTHORIZATION_AMT.ToString())))
            {
                businessObject.AUTHORIZATION_AMT = dataReader.GetString(dataReader.GetOrdinal(INDIVIDUAL_LEGAL_REPRESENT.INDIVIDUAL_LEGAL_REPRESENTFields.AUTHORIZATION_AMT.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(INDIVIDUAL_LEGAL_REPRESENT.INDIVIDUAL_LEGAL_REPRESENTFields.DESCRIPTION.ToString())))
            {
                businessObject.DESCRIPTION = dataReader.GetString(dataReader.GetOrdinal(INDIVIDUAL_LEGAL_REPRESENT.INDIVIDUAL_LEGAL_REPRESENTFields.DESCRIPTION.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(INDIVIDUAL_LEGAL_REPRESENT.INDIVIDUAL_LEGAL_REPRESENTFields.CURRENCY_CD.ToString())))
            {
                businessObject.CURRENCY_CD = dataReader.GetString(dataReader.GetOrdinal(INDIVIDUAL_LEGAL_REPRESENT.INDIVIDUAL_LEGAL_REPRESENTFields.CURRENCY_CD.ToString()));
            }

            businessObject.ID_CARD_TYPE_CD = dataReader.GetInt32(dataReader.GetOrdinal(INDIVIDUAL_LEGAL_REPRESENT.INDIVIDUAL_LEGAL_REPRESENTFields.ID_CARD_TYPE_CD.ToString()));

            businessObject.COUNTRY_CD = dataReader.GetInt32(dataReader.GetOrdinal(INDIVIDUAL_LEGAL_REPRESENT.INDIVIDUAL_LEGAL_REPRESENTFields.COUNTRY_CD.ToString()));

            businessObject.STATE_CD = dataReader.GetInt32(dataReader.GetOrdinal(INDIVIDUAL_LEGAL_REPRESENT.INDIVIDUAL_LEGAL_REPRESENTFields.STATE_CD.ToString()));

            businessObject.CITY_CD = dataReader.GetInt32(dataReader.GetOrdinal(INDIVIDUAL_LEGAL_REPRESENT.INDIVIDUAL_LEGAL_REPRESENTFields.CITY_CD.ToString()));


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of INDIVIDUAL_LEGAL_REPRESENT</returns>
        internal List<INDIVIDUAL_LEGAL_REPRESENT> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<INDIVIDUAL_LEGAL_REPRESENT> list = new List<INDIVIDUAL_LEGAL_REPRESENT>();

            while (dataReader.Read())
            {
                INDIVIDUAL_LEGAL_REPRESENT businessObject = new INDIVIDUAL_LEGAL_REPRESENT();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

    }
}
