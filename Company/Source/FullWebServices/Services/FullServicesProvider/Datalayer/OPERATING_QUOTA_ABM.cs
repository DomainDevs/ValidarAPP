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
	/// Data access layer class for OPERATING_QUOTA
	/// </summary>
	class OPERATING_QUOTA_ABM : DataLayerBase 
	{

        #region Constructor

		/// <summary>
		/// Class constructor
		/// </summary>
		public OPERATING_QUOTA_ABM()
		{
			// Nothing for now.
		}

    	/// <summary>
		/// Class constructor
		/// </summary>
		public OPERATING_QUOTA_ABM(string Connection)
            : base(Connection) 
		{
			// Nothing for now.
		}

        /// <summary>
        /// Class constructor
        /// </summary>
        public OPERATING_QUOTA_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
		public bool Insert(OPERATING_QUOTA businessObject)
		{
			AseCommand	sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.OPERATING_QUOTA_Insert";
			sqlCommand.CommandType = CommandType.StoredProcedure;

			try
			{
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_ID)));
				sqlCommand.Parameters.Add(new AseParameter("@LINE_BUSINESS_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.LINE_BUSINESS_CD)));
				sqlCommand.Parameters.Add(new AseParameter("@CURRENCY_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CURRENCY_CD)));
				sqlCommand.Parameters.Add(new AseParameter("@OPERATING_QUOTA_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.OPERATING_QUOTA_AMT)));
				sqlCommand.Parameters.Add(new AseParameter("@CURRENT_TO", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CURRENT_TO, "dd/MM/yyyy")));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));
 									
				sqlCommand.ExecuteNonQuery();
               
				return true;
			}
			catch(Exception ex)
			{				
				throw new SupException("OPERATING_QUOTA::Insert::Error occured.", ex);
			}
		}

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(OPERATING_QUOTA businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.OPERATING_QUOTA_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_ID)));
				sqlCommand.Parameters.Add(new AseParameter("@LINE_BUSINESS_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.LINE_BUSINESS_CD)));
				sqlCommand.Parameters.Add(new AseParameter("@CURRENCY_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CURRENCY_CD)));
				sqlCommand.Parameters.Add(new AseParameter("@OPERATING_QUOTA_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.OPERATING_QUOTA_AMT)));
				sqlCommand.Parameters.Add(new AseParameter("@CURRENT_TO", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CURRENT_TO, "dd/MM/yyyy")));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));
                 
                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("OPERATING_QUOTA::Update::Error occured.", ex);
            }
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(OPERATING_QUOTA businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.OPERATING_QUOTA_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_ID)));
				sqlCommand.Parameters.Add(new AseParameter("@LINE_BUSINESS_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.LINE_BUSINESS_CD)));
				sqlCommand.Parameters.Add(new AseParameter("@CURRENCY_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CURRENCY_CD)));
				sqlCommand.Parameters.Add(new AseParameter("@OPERATING_QUOTA_AMT", AseDbType.Double, 9, ParameterDirection.Input, false, 18, 2, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.OPERATING_QUOTA_AMT)));
				sqlCommand.Parameters.Add(new AseParameter("@CURRENT_TO", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CURRENT_TO, "dd/MM/yyyy")));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));
                 
                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("OPERATING_QUOTA::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(OPERATING_QUOTA businessObject, IDataReader dataReader)
        {

				businessObject.INDIVIDUAL_ID = dataReader.GetInt32(dataReader.GetOrdinal(OPERATING_QUOTA.OPERATING_QUOTAFields.INDIVIDUAL_ID.ToString()));

				businessObject.LINE_BUSINESS_CD = dataReader.GetInt32(dataReader.GetOrdinal(OPERATING_QUOTA.OPERATING_QUOTAFields.LINE_BUSINESS_CD.ToString()));

				businessObject.CURRENCY_CD = dataReader.GetInt32(dataReader.GetOrdinal(OPERATING_QUOTA.OPERATING_QUOTAFields.CURRENCY_CD.ToString()));

				businessObject.OPERATING_QUOTA_AMT = dataReader.GetDouble(dataReader.GetOrdinal(OPERATING_QUOTA.OPERATING_QUOTAFields.OPERATING_QUOTA_AMT.ToString()));

				businessObject.CURRENT_TO = dataReader.GetString(dataReader.GetOrdinal(OPERATING_QUOTA.OPERATING_QUOTAFields.CURRENT_TO.ToString()));

        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of OPERATING_QUOTA</returns>
        internal List<OPERATING_QUOTA> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<OPERATING_QUOTA> list = new List<OPERATING_QUOTA>();

            while (dataReader.Read())
            {
                OPERATING_QUOTA businessObject = new OPERATING_QUOTA();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

	}
}
