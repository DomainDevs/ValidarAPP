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
    /// Data access layer class for ADDRESS
    /// </summary>
    class ADDRESS_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public ADDRESS_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public ADDRESS_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public ADDRESS_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
        public bool Insert(ADDRESS businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.ADDRESS_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@DATA_ID", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.DATA_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@ADDRESS_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ADDRESS_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@IS_MAILING_ADDRESS", AseDbType.Bit, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.IS_MAILING_ADDRESS)));
                sqlCommand.Parameters.Add(new AseParameter("@STREET_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.STREET_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@STREET", AseDbType.VarChar, 180, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.STREET)));
                sqlCommand.Parameters.Add(new AseParameter("@CITY_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CITY_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@HOUSE_NUMBER", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.HOUSE_NUMBER)));
                sqlCommand.Parameters.Add(new AseParameter("@FLOOR", AseDbType.VarChar, 3, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.FLOOR)));
                sqlCommand.Parameters.Add(new AseParameter("@APARTMENT", AseDbType.VarChar, 5, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.APARTMENT)));
                sqlCommand.Parameters.Add(new AseParameter("@ZIP_CODE", AseDbType.VarChar, 10, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ZIP_CODE)));
                sqlCommand.Parameters.Add(new AseParameter("@URBANIZATION", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.URBANIZATION)));
                sqlCommand.Parameters.Add(new AseParameter("@STATE_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.STATE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@COUNTRY_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.COUNTRY_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@IS_HOME", AseDbType.Bit, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.IS_HOME))); //SUPDB

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));

                

                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("ADDRESS::Insert::Error occured.", ex);
            }
        }

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(ADDRESS businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.ADDRESS_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@DATA_ID", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.DATA_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@ADDRESS_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ADDRESS_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@IS_MAILING_ADDRESS", AseDbType.Bit, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.IS_MAILING_ADDRESS)));
                sqlCommand.Parameters.Add(new AseParameter("@STREET_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.STREET_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@STREET", AseDbType.VarChar, 180, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.STREET)));
                sqlCommand.Parameters.Add(new AseParameter("@CITY_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CITY_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@HOUSE_NUMBER", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.HOUSE_NUMBER)));
                sqlCommand.Parameters.Add(new AseParameter("@FLOOR", AseDbType.VarChar, 3, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.FLOOR)));
                sqlCommand.Parameters.Add(new AseParameter("@APARTMENT", AseDbType.VarChar, 5, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.APARTMENT)));
                sqlCommand.Parameters.Add(new AseParameter("@ZIP_CODE", AseDbType.VarChar, 10, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ZIP_CODE)));
                sqlCommand.Parameters.Add(new AseParameter("@URBANIZATION", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.URBANIZATION)));
                sqlCommand.Parameters.Add(new AseParameter("@STATE_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.STATE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@COUNTRY_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.COUNTRY_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@IS_HOME", AseDbType.Bit, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.IS_HOME))); //SUPDB

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));


                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("ADDRESS::Update::Error occured.", ex);
            }
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(ADDRESS businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.ADDRESS_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@DATA_ID", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.DATA_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@ADDRESS_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ADDRESS_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@IS_MAILING_ADDRESS", AseDbType.Bit, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.IS_MAILING_ADDRESS)));
                sqlCommand.Parameters.Add(new AseParameter("@STREET_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.STREET_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@STREET", AseDbType.VarChar, 180, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.STREET)));
                sqlCommand.Parameters.Add(new AseParameter("@CITY_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CITY_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@HOUSE_NUMBER", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.HOUSE_NUMBER)));
                sqlCommand.Parameters.Add(new AseParameter("@FLOOR", AseDbType.VarChar, 3, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.FLOOR)));
                sqlCommand.Parameters.Add(new AseParameter("@APARTMENT", AseDbType.VarChar, 5, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.APARTMENT)));
                sqlCommand.Parameters.Add(new AseParameter("@ZIP_CODE", AseDbType.VarChar, 10, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ZIP_CODE)));
                sqlCommand.Parameters.Add(new AseParameter("@URBANIZATION", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.URBANIZATION)));
                sqlCommand.Parameters.Add(new AseParameter("@STATE_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.STATE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@COUNTRY_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.COUNTRY_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@IS_HOME", AseDbType.Bit, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.IS_HOME))); //SUPDB

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));


                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("ADDRESS::Delete::Error occured.", ex);
            }
        }

        /// <summary>
        /// get address by individual to validate update or insert
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        public bool GetAddressIndividual(int individualId)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.GET_ADDRESS_DATA";
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
                throw new SupException("ADDRESS::Get::Error occured.", ex);
            }
        }
               
        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(ADDRESS businessObject, IDataReader dataReader)
        {


            businessObject.INDIVIDUAL_ID = dataReader.GetInt32(dataReader.GetOrdinal(ADDRESS.ADDRESSFields.INDIVIDUAL_ID.ToString()));

            businessObject.DATA_ID = dataReader.GetInt32(dataReader.GetOrdinal(ADDRESS.ADDRESSFields.DATA_ID.ToString()));

            businessObject.ADDRESS_TYPE_CD = dataReader.GetInt32(dataReader.GetOrdinal(ADDRESS.ADDRESSFields.ADDRESS_TYPE_CD.ToString()));

            businessObject.IS_MAILING_ADDRESS = dataReader.GetBoolean(dataReader.GetOrdinal(ADDRESS.ADDRESSFields.IS_MAILING_ADDRESS.ToString()));

            businessObject.STREET_TYPE_CD = dataReader.GetInt32(dataReader.GetOrdinal(ADDRESS.ADDRESSFields.STREET_TYPE_CD.ToString()));

            businessObject.STREET = dataReader.GetString(dataReader.GetOrdinal(ADDRESS.ADDRESSFields.STREET.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(ADDRESS.ADDRESSFields.CITY_CD.ToString())))
            {
                businessObject.CITY_CD = dataReader.GetString(dataReader.GetOrdinal(ADDRESS.ADDRESSFields.CITY_CD.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(ADDRESS.ADDRESSFields.HOUSE_NUMBER.ToString())))
            {
                businessObject.HOUSE_NUMBER = dataReader.GetString(dataReader.GetOrdinal(ADDRESS.ADDRESSFields.HOUSE_NUMBER.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(ADDRESS.ADDRESSFields.FLOOR.ToString())))
            {
                businessObject.FLOOR = dataReader.GetString(dataReader.GetOrdinal(ADDRESS.ADDRESSFields.FLOOR.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(ADDRESS.ADDRESSFields.APARTMENT.ToString())))
            {
                businessObject.APARTMENT = dataReader.GetString(dataReader.GetOrdinal(ADDRESS.ADDRESSFields.APARTMENT.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(ADDRESS.ADDRESSFields.ZIP_CODE.ToString())))
            {
                businessObject.ZIP_CODE = dataReader.GetString(dataReader.GetOrdinal(ADDRESS.ADDRESSFields.ZIP_CODE.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(ADDRESS.ADDRESSFields.URBANIZATION.ToString())))
            {
                businessObject.URBANIZATION = dataReader.GetString(dataReader.GetOrdinal(ADDRESS.ADDRESSFields.URBANIZATION.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(ADDRESS.ADDRESSFields.STATE_CD.ToString())))
            {
                businessObject.STATE_CD = dataReader.GetString(dataReader.GetOrdinal(ADDRESS.ADDRESSFields.STATE_CD.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(ADDRESS.ADDRESSFields.COUNTRY_CD.ToString())))
            {
                businessObject.COUNTRY_CD = dataReader.GetString(dataReader.GetOrdinal(ADDRESS.ADDRESSFields.COUNTRY_CD.ToString()));
            }

            businessObject.IS_HOME = dataReader.GetBoolean(dataReader.GetOrdinal(ADDRESS.ADDRESSFields.IS_HOME.ToString())); //SUPDB

        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of ADDRESS</returns>
        internal List<ADDRESS> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<ADDRESS> list = new List<ADDRESS>();

            while (dataReader.Read())
            {
                ADDRESS businessObject = new ADDRESS();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

    }
}
