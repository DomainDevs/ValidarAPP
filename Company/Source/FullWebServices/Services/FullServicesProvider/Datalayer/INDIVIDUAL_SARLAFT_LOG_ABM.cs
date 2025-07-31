using System;
using System.Data;
using System.Data.SqlTypes;
using Sybase.Data.AseClient;
using System.Collections.Generic;
using Sistran.Co.Previsora.Application.FullServices.Models;
using Sistran.Co.Previsora.Application.FullServicesProvider.BussinesLayer;
using Sistran.Co.Previsora.Application.FullServicesProvider.Helpers;
using Sistran.Co.Previsora.Application.FullServicesProvider.DataLayer;

namespace Sistran.Co.Previsora.Application.FullServices.Models.DataLayer
{
	/// <summary>
	/// Data access layer class for INDIVIDUAL_SARLAFT_LOG
	/// </summary>
	class INDIVIDUAL_SARLAFT_LOG_ABM : DataLayerBase 
	{

        #region Constructor

		/// <summary>
		/// Class constructor
		/// </summary>
		public INDIVIDUAL_SARLAFT_LOG_ABM()
		{
			// Nothing for now.
		}

    	/// <summary>
		/// Class constructor
		/// </summary>
		public INDIVIDUAL_SARLAFT_LOG_ABM(string Connection)
            : base(Connection) 
		{
			// Nothing for now.
		}

        /// <summary>
        /// Class constructor
        /// </summary>
        public INDIVIDUAL_SARLAFT_LOG_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
		public bool Insert(INDIVIDUAL_SARLAFT_LOG businessObject)
		{
			AseCommand	sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.INDIVI_SARL_LOG_Insert";
			sqlCommand.CommandType = CommandType.StoredProcedure;

			try
			{
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_ID)));
				sqlCommand.Parameters.Add(new AseParameter("@USER_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.USER_ID)));
				sqlCommand.Parameters.Add(new AseParameter("@EXONERATION_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.EXONERATION_TYPE_CD)));
				sqlCommand.Parameters.Add(new AseParameter("@IS_EXONERATED", AseDbType.Bit, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.IS_EXONERATED)));
				sqlCommand.Parameters.Add(new AseParameter("@CHANGE_DATE", AseDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CHANGE_DATE, "dd/MM/yyyy")));
				sqlCommand.Parameters.Add(new AseParameter("@ROLE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ROLE_CD)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));
 								
				sqlCommand.ExecuteNonQuery();
               
				return true;
			}
			catch(Exception ex)
			{				
				throw new SupException("INDIVIDUAL_SARLAFT_LOG::Insert::Error occured.", ex);
			}
		}

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(INDIVIDUAL_SARLAFT_LOG businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.INDIVI_SARL_LOG_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_ID)));
				sqlCommand.Parameters.Add(new AseParameter("@USER_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.USER_ID)));
				sqlCommand.Parameters.Add(new AseParameter("@EXONERATION_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.EXONERATION_TYPE_CD)));
				sqlCommand.Parameters.Add(new AseParameter("@IS_EXONERATED", AseDbType.Bit, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.IS_EXONERATED)));
				sqlCommand.Parameters.Add(new AseParameter("@CHANGE_DATE", AseDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CHANGE_DATE, "dd/MM/yyyy")));
				sqlCommand.Parameters.Add(new AseParameter("@ROLE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ROLE_CD)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));

                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("INDIVIDUAL_SARLAFT_LOG::Update::Error occured.", ex);
            }
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(INDIVIDUAL_SARLAFT_LOG businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.INDIVI_SARL_LOG_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_ID)));
				sqlCommand.Parameters.Add(new AseParameter("@USER_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.USER_ID)));
				sqlCommand.Parameters.Add(new AseParameter("@EXONERATION_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.EXONERATION_TYPE_CD)));
				sqlCommand.Parameters.Add(new AseParameter("@IS_EXONERATED", AseDbType.Bit, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.IS_EXONERATED)));
				sqlCommand.Parameters.Add(new AseParameter("@CHANGE_DATE", AseDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CHANGE_DATE, "dd/MM/yyyy")));
				sqlCommand.Parameters.Add(new AseParameter("@ROLE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ROLE_CD)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));

                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("INDIVIDUAL_SARLAFT_LOG::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(INDIVIDUAL_SARLAFT_LOG businessObject, IDataReader dataReader)
        {


				businessObject.INDIVIDUAL_ID = dataReader.GetInt32(dataReader.GetOrdinal(INDIVIDUAL_SARLAFT_LOG.INDIVIDUAL_SARLAFT_LOGFields.INDIVIDUAL_ID.ToString()));

				businessObject.USER_ID = dataReader.GetInt32(dataReader.GetOrdinal(INDIVIDUAL_SARLAFT_LOG.INDIVIDUAL_SARLAFT_LOGFields.USER_ID.ToString()));

				if (!dataReader.IsDBNull(dataReader.GetOrdinal(INDIVIDUAL_SARLAFT_LOG.INDIVIDUAL_SARLAFT_LOGFields.EXONERATION_TYPE_CD.ToString())))
				{
					businessObject.EXONERATION_TYPE_CD = dataReader.GetString(dataReader.GetOrdinal(INDIVIDUAL_SARLAFT_LOG.INDIVIDUAL_SARLAFT_LOGFields.EXONERATION_TYPE_CD.ToString()));
				}

				businessObject.IS_EXONERATED = dataReader.GetBoolean(dataReader.GetOrdinal(INDIVIDUAL_SARLAFT_LOG.INDIVIDUAL_SARLAFT_LOGFields.IS_EXONERATED.ToString()));

				if (!dataReader.IsDBNull(dataReader.GetOrdinal(INDIVIDUAL_SARLAFT_LOG.INDIVIDUAL_SARLAFT_LOGFields.CHANGE_DATE.ToString())))
				{
					businessObject.CHANGE_DATE = dataReader.GetString(dataReader.GetOrdinal(INDIVIDUAL_SARLAFT_LOG.INDIVIDUAL_SARLAFT_LOGFields.CHANGE_DATE.ToString()));
				}

				businessObject.ROLE_CD = dataReader.GetInt32(dataReader.GetOrdinal(INDIVIDUAL_SARLAFT_LOG.INDIVIDUAL_SARLAFT_LOGFields.ROLE_CD.ToString()));


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of INDIVIDUAL_SARLAFT_LOG</returns>
        internal List<INDIVIDUAL_SARLAFT_LOG> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<INDIVIDUAL_SARLAFT_LOG> list = new List<INDIVIDUAL_SARLAFT_LOG>();

            while (dataReader.Read())
            {
                INDIVIDUAL_SARLAFT_LOG businessObject = new INDIVIDUAL_SARLAFT_LOG();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

	}
}
