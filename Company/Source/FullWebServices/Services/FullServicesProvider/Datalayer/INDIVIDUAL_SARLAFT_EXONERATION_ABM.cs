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
    /// Data access layer class for INDIVIDUAL_SARLAFT_EXONERATION
    /// </summary>
    class INDIVIDUAL_SARLAFT_EXONERATION_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public INDIVIDUAL_SARLAFT_EXONERATION_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public INDIVIDUAL_SARLAFT_EXONERATION_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public INDIVIDUAL_SARLAFT_EXONERATION_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
        public bool Insert(INDIVIDUAL_SARLAFT_EXONERATION businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.IND_SARLAFT_EXONE_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@USER_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.USER_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@EXONERATION_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.EXONERATION_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@IS_EXONERATED", AseDbType.Bit, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.IS_EXONERATED)));
                sqlCommand.Parameters.Add(new AseParameter("@REGISTRATION_DATE", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.REGISTRATION_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@ROLE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ROLE_CD)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("INDIVIDUAL_SARLAFT_EXONERATION::Insert::Error occured.", ex);
            }
        }

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(INDIVIDUAL_SARLAFT_EXONERATION businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.IND_SARLAFT_EXONE_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@USER_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.USER_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@EXONERATION_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.EXONERATION_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@IS_EXONERATED", AseDbType.Bit, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.IS_EXONERATED)));
                sqlCommand.Parameters.Add(new AseParameter("@REGISTRATION_DATE", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.REGISTRATION_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@ROLE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ROLE_CD)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("INDIVIDUAL_SARLAFT_EXONERATION::Update::Error occured.", ex);
            }
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(INDIVIDUAL_SARLAFT_EXONERATION businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.INDIVIDUAL_SARLAFT_EXONERATION_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@USER_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.USER_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@EXONERATION_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.EXONERATION_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@IS_EXONERATED", AseDbType.Bit, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.IS_EXONERATED)));
                sqlCommand.Parameters.Add(new AseParameter("@REGISTRATION_DATE", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.REGISTRATION_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@ROLE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ROLE_CD)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("INDIVIDUAL_SARLAFT_EXONERATION::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(INDIVIDUAL_SARLAFT_EXONERATION businessObject, IDataReader dataReader)
        {


            businessObject.INDIVIDUAL_ID = dataReader.GetInt32(dataReader.GetOrdinal(INDIVIDUAL_SARLAFT_EXONERATION.INDIVIDUAL_SARLAFT_EXONERATIONFields.INDIVIDUAL_ID.ToString()));

            businessObject.USER_ID = dataReader.GetInt32(dataReader.GetOrdinal(INDIVIDUAL_SARLAFT_EXONERATION.INDIVIDUAL_SARLAFT_EXONERATIONFields.USER_ID.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(INDIVIDUAL_SARLAFT_EXONERATION.INDIVIDUAL_SARLAFT_EXONERATIONFields.EXONERATION_TYPE_CD.ToString())))
            {
                businessObject.EXONERATION_TYPE_CD = dataReader.GetString(dataReader.GetOrdinal(INDIVIDUAL_SARLAFT_EXONERATION.INDIVIDUAL_SARLAFT_EXONERATIONFields.EXONERATION_TYPE_CD.ToString()));
            }

            businessObject.IS_EXONERATED = dataReader.GetBoolean(dataReader.GetOrdinal(INDIVIDUAL_SARLAFT_EXONERATION.INDIVIDUAL_SARLAFT_EXONERATIONFields.IS_EXONERATED.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(INDIVIDUAL_SARLAFT_EXONERATION.INDIVIDUAL_SARLAFT_EXONERATIONFields.REGISTRATION_DATE.ToString())))
            {
                businessObject.REGISTRATION_DATE = dataReader.GetString(dataReader.GetOrdinal(INDIVIDUAL_SARLAFT_EXONERATION.INDIVIDUAL_SARLAFT_EXONERATIONFields.REGISTRATION_DATE.ToString()));
            }

            businessObject.ROLE_CD = dataReader.GetInt32(dataReader.GetOrdinal(INDIVIDUAL_SARLAFT_EXONERATION.INDIVIDUAL_SARLAFT_EXONERATIONFields.ROLE_CD.ToString()));


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of INDIVIDUAL_SARLAFT_EXONERATION</returns>
        internal List<INDIVIDUAL_SARLAFT_EXONERATION> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<INDIVIDUAL_SARLAFT_EXONERATION> list = new List<INDIVIDUAL_SARLAFT_EXONERATION>();

            while (dataReader.Read())
            {
                INDIVIDUAL_SARLAFT_EXONERATION businessObject = new INDIVIDUAL_SARLAFT_EXONERATION();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

    }
}
