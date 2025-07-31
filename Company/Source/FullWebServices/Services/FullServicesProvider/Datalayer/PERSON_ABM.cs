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
    /// Data access layer class for PERSON
    /// </summary>
    class PERSON_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public PERSON_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public PERSON_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public PERSON_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
        public bool Insert(PERSON businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.PERSON_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@SURNAME", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.SURNAME)));
                sqlCommand.Parameters.Add(new AseParameter("@NAME", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.NAME)));
                sqlCommand.Parameters.Add(new AseParameter("@GENDER", AseDbType.Char, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.GENDER)));
                sqlCommand.Parameters.Add(new AseParameter("@ID_CARD_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ID_CARD_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@ID_CARD_NO", AseDbType.VarChar, 14, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ID_CARD_NO)));
                sqlCommand.Parameters.Add(new AseParameter("@MARITAL_STATUS_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.MARITAL_STATUS_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@BIRTH_DATE", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.BIRTH_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@CHILDREN", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(string.IsNullOrEmpty(businessObject.CHILDREN) ? "0" : businessObject.CHILDREN)));
                sqlCommand.Parameters.Add(new AseParameter("@EDUCATIVE_LEVEL_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.EDUCATIVE_LEVEL_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@MOTHER_LAST_NAME", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.MOTHER_LAST_NAME)));
                sqlCommand.Parameters.Add(new AseParameter("@TRIBUTARY_ID_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.TRIBUTARY_ID_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@TRIBUTARY_ID_NO", AseDbType.VarChar, 14, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.TRIBUTARY_ID_NO)));
                sqlCommand.Parameters.Add(new AseParameter("@BIRTH_COUNTRY_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.BIRTH_COUNTRY_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@BIRTH_PLACE", AseDbType.VarChar, 70, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.BIRTH_PLACE)));
                sqlCommand.Parameters.Add(new AseParameter("@SPOUSE_NAME", AseDbType.VarChar, 70, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.SPOUSE_NAME)));
                sqlCommand.Parameters.Add(new AseParameter("@HOUSE_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.HOUSE_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@SOCIAL_LAYER_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.SOCIAL_LAYER_CD)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));

                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("PERSON::Insert::Error occured.", ex);
            }    
        }

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(PERSON businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.PERSON_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@SURNAME", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.SURNAME)));
                sqlCommand.Parameters.Add(new AseParameter("@NAME", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.NAME)));
                sqlCommand.Parameters.Add(new AseParameter("@GENDER", AseDbType.Char, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.GENDER)));
                sqlCommand.Parameters.Add(new AseParameter("@ID_CARD_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ID_CARD_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@ID_CARD_NO", AseDbType.VarChar, 14, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ID_CARD_NO)));
                sqlCommand.Parameters.Add(new AseParameter("@MARITAL_STATUS_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.MARITAL_STATUS_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@BIRTH_DATE", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.BIRTH_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@CHILDREN", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(string.IsNullOrEmpty(businessObject.CHILDREN) ? "0" : businessObject.CHILDREN)));
                sqlCommand.Parameters.Add(new AseParameter("@EDUCATIVE_LEVEL_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.EDUCATIVE_LEVEL_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@MOTHER_LAST_NAME", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.MOTHER_LAST_NAME)));
                sqlCommand.Parameters.Add(new AseParameter("@TRIBUTARY_ID_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.TRIBUTARY_ID_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@TRIBUTARY_ID_NO", AseDbType.VarChar, 14, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.TRIBUTARY_ID_NO)));
                sqlCommand.Parameters.Add(new AseParameter("@BIRTH_COUNTRY_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.BIRTH_COUNTRY_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@BIRTH_PLACE", AseDbType.VarChar, 70, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.BIRTH_PLACE)));
                sqlCommand.Parameters.Add(new AseParameter("@SPOUSE_NAME", AseDbType.VarChar, 70, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.SPOUSE_NAME)));
                sqlCommand.Parameters.Add(new AseParameter("@HOUSE_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.HOUSE_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@SOCIAL_LAYER_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.SOCIAL_LAYER_CD)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));

                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("PERSON::Update::Error occured.", ex);
            }
        }

        /// <summary>
        ////Edward Rubiano -- C11226 -- Guarda unicamente datos basicos -- 10/05/2016
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool UpdateU(PERSON businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.PERSON_UpdateU";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@SURNAME", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.SURNAME)));
                sqlCommand.Parameters.Add(new AseParameter("@NAME", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.NAME)));
                sqlCommand.Parameters.Add(new AseParameter("@ID_CARD_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ID_CARD_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@ID_CARD_NO", AseDbType.VarChar, 14, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ID_CARD_NO)));
                sqlCommand.Parameters.Add(new AseParameter("@MOTHER_LAST_NAME", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.MOTHER_LAST_NAME)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));

                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("PERSON::UpdateU::Error occured.", ex);
            }
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(PERSON businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.PERSON_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@SURNAME", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.SURNAME)));
                sqlCommand.Parameters.Add(new AseParameter("@NAME", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.NAME)));
                sqlCommand.Parameters.Add(new AseParameter("@GENDER", AseDbType.Char, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.GENDER)));
                sqlCommand.Parameters.Add(new AseParameter("@ID_CARD_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ID_CARD_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@ID_CARD_NO", AseDbType.VarChar, 14, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ID_CARD_NO)));
                sqlCommand.Parameters.Add(new AseParameter("@MARITAL_STATUS_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.MARITAL_STATUS_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@BIRTH_DATE", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.BIRTH_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@CHILDREN", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(string.IsNullOrEmpty(businessObject.CHILDREN) ? "0" : businessObject.CHILDREN)));
                sqlCommand.Parameters.Add(new AseParameter("@EDUCATIVE_LEVEL_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.EDUCATIVE_LEVEL_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@MOTHER_LAST_NAME", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.MOTHER_LAST_NAME)));
                sqlCommand.Parameters.Add(new AseParameter("@TRIBUTARY_ID_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.TRIBUTARY_ID_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@TRIBUTARY_ID_NO", AseDbType.VarChar, 14, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.TRIBUTARY_ID_NO)));
                sqlCommand.Parameters.Add(new AseParameter("@BIRTH_COUNTRY_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.BIRTH_COUNTRY_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@BIRTH_PLACE", AseDbType.VarChar, 70, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.BIRTH_PLACE)));
                sqlCommand.Parameters.Add(new AseParameter("@SPOUSE_NAME", AseDbType.VarChar, 70, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.SPOUSE_NAME)));
                sqlCommand.Parameters.Add(new AseParameter("@HOUSE_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.HOUSE_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@SOCIAL_LAYER_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.SOCIAL_LAYER_CD)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("PERSON::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(PERSON businessObject, IDataReader dataReader)
        {


            businessObject.INDIVIDUAL_ID = dataReader.GetInt32(dataReader.GetOrdinal(PERSON.PERSONFields.INDIVIDUAL_ID.ToString()));

            businessObject.SURNAME = dataReader.GetString(dataReader.GetOrdinal(PERSON.PERSONFields.SURNAME.ToString()));

            businessObject.NAME = dataReader.GetString(dataReader.GetOrdinal(PERSON.PERSONFields.NAME.ToString()));

            businessObject.GENDER = dataReader.GetString(dataReader.GetOrdinal(PERSON.PERSONFields.GENDER.ToString()));

            businessObject.ID_CARD_TYPE_CD = dataReader.GetInt32(dataReader.GetOrdinal(PERSON.PERSONFields.ID_CARD_TYPE_CD.ToString()));

            businessObject.ID_CARD_NO = dataReader.GetString(dataReader.GetOrdinal(PERSON.PERSONFields.ID_CARD_NO.ToString()));

            businessObject.MARITAL_STATUS_CD = dataReader.GetInt32(dataReader.GetOrdinal(PERSON.PERSONFields.MARITAL_STATUS_CD.ToString()));

            businessObject.BIRTH_DATE = dataReader.GetString(dataReader.GetOrdinal(PERSON.PERSONFields.BIRTH_DATE.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(PERSON.PERSONFields.CHILDREN.ToString())))
            {
                businessObject.CHILDREN = dataReader.GetString(dataReader.GetOrdinal(PERSON.PERSONFields.CHILDREN.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(PERSON.PERSONFields.EDUCATIVE_LEVEL_CD.ToString())))
            {
                businessObject.EDUCATIVE_LEVEL_CD = dataReader.GetString(dataReader.GetOrdinal(PERSON.PERSONFields.EDUCATIVE_LEVEL_CD.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(PERSON.PERSONFields.MOTHER_LAST_NAME.ToString())))
            {
                businessObject.MOTHER_LAST_NAME = dataReader.GetString(dataReader.GetOrdinal(PERSON.PERSONFields.MOTHER_LAST_NAME.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(PERSON.PERSONFields.TRIBUTARY_ID_TYPE_CD.ToString())))
            {
                businessObject.TRIBUTARY_ID_TYPE_CD = dataReader.GetString(dataReader.GetOrdinal(PERSON.PERSONFields.TRIBUTARY_ID_TYPE_CD.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(PERSON.PERSONFields.TRIBUTARY_ID_NO.ToString())))
            {
                businessObject.TRIBUTARY_ID_NO = dataReader.GetString(dataReader.GetOrdinal(PERSON.PERSONFields.TRIBUTARY_ID_NO.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(PERSON.PERSONFields.BIRTH_COUNTRY_CD.ToString())))
            {
                businessObject.BIRTH_COUNTRY_CD = dataReader.GetString(dataReader.GetOrdinal(PERSON.PERSONFields.BIRTH_COUNTRY_CD.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(PERSON.PERSONFields.BIRTH_PLACE.ToString())))
            {
                businessObject.BIRTH_PLACE = dataReader.GetString(dataReader.GetOrdinal(PERSON.PERSONFields.BIRTH_PLACE.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(PERSON.PERSONFields.SPOUSE_NAME.ToString())))
            {
                businessObject.SPOUSE_NAME = dataReader.GetString(dataReader.GetOrdinal(PERSON.PERSONFields.SPOUSE_NAME.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(PERSON.PERSONFields.HOUSE_TYPE_CD.ToString())))
            {
                businessObject.HOUSE_TYPE_CD = dataReader.GetString(dataReader.GetOrdinal(PERSON.PERSONFields.HOUSE_TYPE_CD.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(PERSON.PERSONFields.SOCIAL_LAYER_CD.ToString())))
            {
                businessObject.SOCIAL_LAYER_CD = dataReader.GetString(dataReader.GetOrdinal(PERSON.PERSONFields.SOCIAL_LAYER_CD.ToString()));
            }


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of PERSON</returns>
        internal List<PERSON> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<PERSON> list = new List<PERSON>();

            while (dataReader.Read())
            {
                PERSON businessObject = new PERSON();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

    }
}
