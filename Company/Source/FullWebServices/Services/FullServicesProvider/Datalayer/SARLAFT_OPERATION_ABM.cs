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
    /// Data access layer class for SARLAFT_OPERATION
    /// </summary>
    class SARLAFT_OPERATION_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public SARLAFT_OPERATION_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public SARLAFT_OPERATION_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public SARLAFT_OPERATION_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
        public bool Insert(SARLAFT_OPERATION businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.SARLAFT_OPERATION_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@SARLAFT_OPERATION_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.SARLAFT_OPERATION_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@SARLAFT_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.SARLAFT_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@PRODUCT_NUM", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.PRODUCT_NUM)));
                sqlCommand.Parameters.Add(new AseParameter("@PRODUCT_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.PRODUCT_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@ENTITY", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ENTITY)));
                sqlCommand.Parameters.Add(new AseParameter("@OPERATION_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.OPERATION_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@PRODUCT_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.PRODUCT_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@CURRENCY_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CURRENCY_CD)));
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
                throw new SupException("SARLAFT_OPERATION::Insert::Error occured.", ex);
            }
            
        }

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(SARLAFT_OPERATION businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.SARLAFT_OPERATION_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;


            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@SARLAFT_OPERATION_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.SARLAFT_OPERATION_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@SARLAFT_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.SARLAFT_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@PRODUCT_NUM", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.PRODUCT_NUM)));
                sqlCommand.Parameters.Add(new AseParameter("@PRODUCT_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.PRODUCT_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@ENTITY", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ENTITY)));
                sqlCommand.Parameters.Add(new AseParameter("@OPERATION_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.OPERATION_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@PRODUCT_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.PRODUCT_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@CURRENCY_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CURRENCY_CD)));
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
                throw new SupException("SARLAFT_OPERATION::Update::Error occured.", ex);
            }        
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(SARLAFT_OPERATION businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.SARLAFT_OPERATION_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@SARLAFT_OPERATION_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.SARLAFT_OPERATION_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@SARLAFT_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.SARLAFT_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@PRODUCT_NUM", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.PRODUCT_NUM)));
                sqlCommand.Parameters.Add(new AseParameter("@PRODUCT_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.PRODUCT_AMT)));
                sqlCommand.Parameters.Add(new AseParameter("@ENTITY", AseDbType.VarChar, 20, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ENTITY)));
                sqlCommand.Parameters.Add(new AseParameter("@OPERATION_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.OPERATION_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@PRODUCT_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.PRODUCT_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@CURRENCY_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CURRENCY_CD)));
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
                throw new SupException("SARLAFT_OPERATION::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(SARLAFT_OPERATION businessObject, IDataReader dataReader)
        {


            businessObject.SARLAFT_OPERATION_ID = dataReader.GetInt32(dataReader.GetOrdinal(SARLAFT_OPERATION.SARLAFT_OPERATIONFields.SARLAFT_OPERATION_ID.ToString()));

            businessObject.SARLAFT_ID = dataReader.GetInt32(dataReader.GetOrdinal(SARLAFT_OPERATION.SARLAFT_OPERATIONFields.SARLAFT_ID.ToString()));

            businessObject.PRODUCT_NUM = dataReader.GetString(dataReader.GetOrdinal(SARLAFT_OPERATION.SARLAFT_OPERATIONFields.PRODUCT_NUM.ToString()));

            businessObject.PRODUCT_AMT = dataReader.GetDouble(dataReader.GetOrdinal(SARLAFT_OPERATION.SARLAFT_OPERATIONFields.PRODUCT_AMT.ToString()));

            businessObject.ENTITY = dataReader.GetString(dataReader.GetOrdinal(SARLAFT_OPERATION.SARLAFT_OPERATIONFields.ENTITY.ToString()));

            businessObject.OPERATION_TYPE_CD = dataReader.GetInt32(dataReader.GetOrdinal(SARLAFT_OPERATION.SARLAFT_OPERATIONFields.OPERATION_TYPE_CD.ToString()));

            businessObject.PRODUCT_TYPE_CD = dataReader.GetInt32(dataReader.GetOrdinal(SARLAFT_OPERATION.SARLAFT_OPERATIONFields.PRODUCT_TYPE_CD.ToString()));

            businessObject.CURRENCY_CD = dataReader.GetInt32(dataReader.GetOrdinal(SARLAFT_OPERATION.SARLAFT_OPERATIONFields.CURRENCY_CD.ToString()));

            businessObject.COUNTRY_CD = dataReader.GetInt32(dataReader.GetOrdinal(SARLAFT_OPERATION.SARLAFT_OPERATIONFields.COUNTRY_CD.ToString()));

            businessObject.STATE_CD = dataReader.GetInt32(dataReader.GetOrdinal(SARLAFT_OPERATION.SARLAFT_OPERATIONFields.STATE_CD.ToString()));

            businessObject.CITY_CD = dataReader.GetInt32(dataReader.GetOrdinal(SARLAFT_OPERATION.SARLAFT_OPERATIONFields.CITY_CD.ToString()));


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of SARLAFT_OPERATION</returns>
        internal List<SARLAFT_OPERATION> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<SARLAFT_OPERATION> list = new List<SARLAFT_OPERATION>();

            while (dataReader.Read())
            {
                SARLAFT_OPERATION businessObject = new SARLAFT_OPERATION();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

    }
}
