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
    /// Data access layer class for TECHNICAL_CARD_DESCRIPTION
    /// </summary>
    class TECHNICAL_CARD_DESCRIPTION_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public TECHNICAL_CARD_DESCRIPTION_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public TECHNICAL_CARD_DESCRIPTION_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public TECHNICAL_CARD_DESCRIPTION_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
        public bool Insert(TECHNICAL_CARD_DESCRIPTION businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.TECHNIC_CARD_DES_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@TECHNICAL_CARD_DESCRIPTION_CD", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.TECHNICAL_CARD_DESCRIPTION_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@TECHNICAL_CARD_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.TECHNICAL_CARD_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@DESCRIPTION_DATE", AseDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.DESCRIPTION_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@DESCRIPTION", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.DESCRIPTION)));
                sqlCommand.Parameters.Add(new AseParameter("@USER_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.USER_ID)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));

                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("TECHNICAL_CARD_DESCRIPTION::Insert::Error occured.", ex);
            }
        }

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(TECHNICAL_CARD_DESCRIPTION businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.TECHNIC_CARD_DES_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@TECHNICAL_CARD_DESCRIPTION_CD", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.TECHNICAL_CARD_DESCRIPTION_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@TECHNICAL_CARD_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.TECHNICAL_CARD_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@DESCRIPTION_DATE", AseDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.DESCRIPTION_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@DESCRIPTION", AseDbType.VarChar, 16, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.DESCRIPTION)));
                sqlCommand.Parameters.Add(new AseParameter("@USER_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.USER_ID)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));

                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("TECHNICAL_CARD_DESCRIPTION::Update::Error occured.", ex);
            }
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(TECHNICAL_CARD_DESCRIPTION businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.TECHNIC_CARD_DES_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@TECHNICAL_CARD_DESCRIPTION_CD", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.TECHNICAL_CARD_DESCRIPTION_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@TECHNICAL_CARD_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.TECHNICAL_CARD_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@DESCRIPTION_DATE", AseDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.DESCRIPTION_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@DESCRIPTION", AseDbType.Text, 16, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.DESCRIPTION)));
                sqlCommand.Parameters.Add(new AseParameter("@USER_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.USER_ID)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));

                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("TECHNICAL_CARD_DESCRIPTION::Delete::Error occured.", ex);
            }            
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(TECHNICAL_CARD_DESCRIPTION businessObject, IDataReader dataReader)
        {


            businessObject.TECHNICAL_CARD_DESCRIPTION_CD = dataReader.GetInt32(dataReader.GetOrdinal(TECHNICAL_CARD_DESCRIPTION.TECHNICAL_CARD_DESCRIPTIONFields.TECHNICAL_CARD_DESCRIPTION_CD.ToString()));

            businessObject.TECHNICAL_CARD_ID = dataReader.GetInt32(dataReader.GetOrdinal(TECHNICAL_CARD_DESCRIPTION.TECHNICAL_CARD_DESCRIPTIONFields.TECHNICAL_CARD_ID.ToString()));

            businessObject.DESCRIPTION_DATE = dataReader.GetString(dataReader.GetOrdinal(TECHNICAL_CARD_DESCRIPTION.TECHNICAL_CARD_DESCRIPTIONFields.DESCRIPTION_DATE.ToString()));

            businessObject.DESCRIPTION = dataReader.GetString(dataReader.GetOrdinal(TECHNICAL_CARD_DESCRIPTION.TECHNICAL_CARD_DESCRIPTIONFields.DESCRIPTION.ToString()));

            businessObject.USER_ID = dataReader.GetInt32(dataReader.GetOrdinal(TECHNICAL_CARD_DESCRIPTION.TECHNICAL_CARD_DESCRIPTIONFields.USER_ID.ToString()));


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of TECHNICAL_CARD_DESCRIPTION</returns>
        internal List<TECHNICAL_CARD_DESCRIPTION> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<TECHNICAL_CARD_DESCRIPTION> list = new List<TECHNICAL_CARD_DESCRIPTION>();

            while (dataReader.Read())
            {
                TECHNICAL_CARD_DESCRIPTION businessObject = new TECHNICAL_CARD_DESCRIPTION();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

    }
}
