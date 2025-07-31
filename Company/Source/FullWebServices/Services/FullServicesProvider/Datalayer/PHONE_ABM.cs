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
    /// Data access layer class for PHONE
    /// </summary>
    class PHONE_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public PHONE_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public PHONE_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public PHONE_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
        public bool Insert(PHONE businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.PHONE_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@DATA_ID", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.DATA_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@PHONE_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.PHONE_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@PHONE_NUMBER", AseDbType.BigInt, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.PHONE_NUMBER)));
                sqlCommand.Parameters.Add(new AseParameter("@EXTENSION", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.EXTENSION)));
                sqlCommand.Parameters.Add(new AseParameter("@COUNTRY_CODE", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.COUNTRY_CODE)));
                sqlCommand.Parameters.Add(new AseParameter("@CITY_CODE", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CITY_CODE)));
                sqlCommand.Parameters.Add(new AseParameter("@SCHEDULE_AVAILABILITY", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.SCHEDULE_AVAILABILITY)));
                sqlCommand.Parameters.Add(new AseParameter("@COUNTRY_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.COUNTRY_CD))); //SUPDB
                sqlCommand.Parameters.Add(new AseParameter("@STATE_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.STATE_CD))); //SUPDB
                sqlCommand.Parameters.Add(new AseParameter("@CITY_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CITY_CD))); //SUPDB
                sqlCommand.Parameters.Add(new AseParameter("@IS_HOME", AseDbType.Bit, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.IS_HOME))); //SUPDB

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));

                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("PHONE::Insert::Error occured.", ex);
            }
        }

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(PHONE businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.PHONE_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@DATA_ID", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.DATA_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@PHONE_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.PHONE_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@PHONE_NUMBER", AseDbType.BigInt, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.PHONE_NUMBER)));
                sqlCommand.Parameters.Add(new AseParameter("@EXTENSION", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.EXTENSION)));
                sqlCommand.Parameters.Add(new AseParameter("@COUNTRY_CODE", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.COUNTRY_CODE)));
                sqlCommand.Parameters.Add(new AseParameter("@CITY_CODE", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CITY_CODE)));
                sqlCommand.Parameters.Add(new AseParameter("@SCHEDULE_AVAILABILITY", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.SCHEDULE_AVAILABILITY)));
                sqlCommand.Parameters.Add(new AseParameter("@COUNTRY_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.COUNTRY_CD))); //SUPDB
                sqlCommand.Parameters.Add(new AseParameter("@STATE_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.STATE_CD))); //SUPDB
                sqlCommand.Parameters.Add(new AseParameter("@CITY_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CITY_CD))); //SUPDB
                sqlCommand.Parameters.Add(new AseParameter("@IS_HOME", AseDbType.Bit, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.IS_HOME))); //SUPDB

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("PHONE::Update::Error occured.", ex);
            }
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(PHONE businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.PHONE_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@DATA_ID", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.DATA_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@PHONE_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.PHONE_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@PHONE_NUMBER", AseDbType.BigInt, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.PHONE_NUMBER)));
                sqlCommand.Parameters.Add(new AseParameter("@EXTENSION", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.EXTENSION)));
                sqlCommand.Parameters.Add(new AseParameter("@COUNTRY_CODE", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.COUNTRY_CODE)));
                sqlCommand.Parameters.Add(new AseParameter("@CITY_CODE", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CITY_CODE)));
                sqlCommand.Parameters.Add(new AseParameter("@SCHEDULE_AVAILABILITY", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.SCHEDULE_AVAILABILITY)));
                sqlCommand.Parameters.Add(new AseParameter("@COUNTRY_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.COUNTRY_CD))); //SUPDB
                sqlCommand.Parameters.Add(new AseParameter("@STATE_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.STATE_CD))); //SUPDB
                sqlCommand.Parameters.Add(new AseParameter("@CITY_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CITY_CD))); //SUPDB
                sqlCommand.Parameters.Add(new AseParameter("@IS_HOME", AseDbType.Bit, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.IS_HOME))); //SUPDB

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("PHONE::Delete::Error occured.", ex);
            }
        }

        /// <summary>
        /// get address by individual to validate update or insert
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        public bool GetPhoneIndividual(int individualId)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.GET_PHONE_DATA";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@ID_PERSONA", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(individualId)));
                IDataReader dataReader = sqlCommand.ExecuteReader();

                return dataReader.Read();
            }
            catch (Exception ex)
            {
                throw new SupException("PHONE::Get::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(PHONE businessObject, IDataReader dataReader)
        {


            businessObject.INDIVIDUAL_ID = dataReader.GetInt32(dataReader.GetOrdinal(PHONE.PHONEFields.INDIVIDUAL_ID.ToString()));

            businessObject.DATA_ID = dataReader.GetInt32(dataReader.GetOrdinal(PHONE.PHONEFields.DATA_ID.ToString()));

            businessObject.PHONE_TYPE_CD = dataReader.GetInt32(dataReader.GetOrdinal(PHONE.PHONEFields.PHONE_TYPE_CD.ToString()));

            businessObject.PHONE_NUMBER = dataReader.GetInt32(dataReader.GetOrdinal(PHONE.PHONEFields.PHONE_NUMBER.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(PHONE.PHONEFields.EXTENSION.ToString())))
            {
                businessObject.EXTENSION = dataReader.GetString(dataReader.GetOrdinal(PHONE.PHONEFields.EXTENSION.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(PHONE.PHONEFields.COUNTRY_CODE.ToString())))
            {
                businessObject.COUNTRY_CODE = dataReader.GetString(dataReader.GetOrdinal(PHONE.PHONEFields.COUNTRY_CODE.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(PHONE.PHONEFields.CITY_CODE.ToString())))
            {
                businessObject.CITY_CODE = dataReader.GetString(dataReader.GetOrdinal(PHONE.PHONEFields.CITY_CODE.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(PHONE.PHONEFields.SCHEDULE_AVAILABILITY.ToString())))
            {
                businessObject.SCHEDULE_AVAILABILITY = dataReader.GetString(dataReader.GetOrdinal(PHONE.PHONEFields.SCHEDULE_AVAILABILITY.ToString()));
            }

            businessObject.COUNTRY_CD = dataReader.GetInt32(dataReader.GetOrdinal(PHONE.PHONEFields.COUNTRY_CD.ToString())); //SUPDB

            businessObject.STATE_CD = dataReader.GetInt32(dataReader.GetOrdinal(PHONE.PHONEFields.STATE_CD.ToString())); //SUPDB

            businessObject.CITY_CD = dataReader.GetInt32(dataReader.GetOrdinal(PHONE.PHONEFields.CITY_CD.ToString())); //SUPDB

            businessObject.IS_HOME = dataReader.GetBoolean(dataReader.GetOrdinal(PHONE.PHONEFields.IS_HOME.ToString())); //SUPDB
        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of PHONE</returns>
        internal List<PHONE> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<PHONE> list = new List<PHONE>();

            while (dataReader.Read())
            {
                PHONE businessObject = new PHONE();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

    }
}
