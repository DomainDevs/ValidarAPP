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
	/// Data access layer class for INDIVIDUAL_TAX_EXEMPTION
	/// </summary>
	class INDIVIDUAL_TAX_EXEMPTION_ABM : DataLayerBase 
	{

        #region Constructor

		/// <summary>
		/// Class constructor
		/// </summary>
		public INDIVIDUAL_TAX_EXEMPTION_ABM()
		{
			// Nothing for now.
		}

    	/// <summary>
		/// Class constructor
		/// </summary>
		public INDIVIDUAL_TAX_EXEMPTION_ABM(string Connection)
            : base(Connection) 
		{
			// Nothing for now.
		}

        /// <summary>
        /// Class constructor
        /// </summary>
        public INDIVIDUAL_TAX_EXEMPTION_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
		public bool Insert(INDIVIDUAL_TAX_EXEMPTION businessObject)
		{
			AseCommand	sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.TAX_EXEMPTION_Insert";
			sqlCommand.CommandType = CommandType.StoredProcedure;

			try
			{
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@IND_TAX_EXEMPTION_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.IND_TAX_EXEMPTION_ID)));
				sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_ID)));
				sqlCommand.Parameters.Add(new AseParameter("@TAX_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.TAX_CD)));
				sqlCommand.Parameters.Add(new AseParameter("@EXEMPTION_PCT", AseDbType.Decimal, 4, ParameterDirection.Input, false, 7, 4, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.EXEMPTION_PCT)));
				sqlCommand.Parameters.Add(new AseParameter("@CURRENT_FROM", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CURRENT_FROM, "dd/MM/yyyy")));
				sqlCommand.Parameters.Add(new AseParameter("@CURRENT_TO", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CURRENT_TO, "dd/MM/yyyy")));
				sqlCommand.Parameters.Add(new AseParameter("@TAX_CATEGORY_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.TAX_CATEGORY_CD)));
				sqlCommand.Parameters.Add(new AseParameter("@STATE_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.STATE_CD)));
				sqlCommand.Parameters.Add(new AseParameter("@COUNTRY_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.COUNTRY_CD)));
				sqlCommand.Parameters.Add(new AseParameter("@BULLETIN_DATE", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.BULLETIN_DATE, "dd/MM/yyyy")));
				sqlCommand.Parameters.Add(new AseParameter("@RESOLUTION_NUMBER", AseDbType.VarChar, 15, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.RESOLUTION_NUMBER)));
				sqlCommand.Parameters.Add(new AseParameter("@HAS_FULL_RETENTION", AseDbType.Bit, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.HAS_FULL_RETENTION)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));
 								
				sqlCommand.ExecuteNonQuery();
               
				return true;
			}
			catch(Exception ex)
			{				
				throw new SupException("INDIVIDUAL_TAX_EXEMPTION::Insert::Error occured.", ex);
			}
		}

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(INDIVIDUAL_TAX_EXEMPTION businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.TAX_EXEMPTION_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@IND_TAX_EXEMPTION_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.IND_TAX_EXEMPTION_ID)));
				sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_ID)));
				sqlCommand.Parameters.Add(new AseParameter("@TAX_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.TAX_CD)));
				sqlCommand.Parameters.Add(new AseParameter("@EXEMPTION_PCT", AseDbType.Decimal, 4, ParameterDirection.Input, false, 7, 4, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.EXEMPTION_PCT)));
				sqlCommand.Parameters.Add(new AseParameter("@CURRENT_FROM", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CURRENT_FROM, "dd/MM/yyyy")));
				sqlCommand.Parameters.Add(new AseParameter("@CURRENT_TO", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CURRENT_TO, "dd/MM/yyyy")));
				sqlCommand.Parameters.Add(new AseParameter("@TAX_CATEGORY_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.TAX_CATEGORY_CD)));
				sqlCommand.Parameters.Add(new AseParameter("@STATE_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.STATE_CD)));
				sqlCommand.Parameters.Add(new AseParameter("@COUNTRY_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.COUNTRY_CD)));
				sqlCommand.Parameters.Add(new AseParameter("@BULLETIN_DATE", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.BULLETIN_DATE, "dd/MM/yyyy")));
				sqlCommand.Parameters.Add(new AseParameter("@RESOLUTION_NUMBER", AseDbType.VarChar, 15, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.RESOLUTION_NUMBER)));
				sqlCommand.Parameters.Add(new AseParameter("@HAS_FULL_RETENTION", AseDbType.Bit, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.HAS_FULL_RETENTION)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));

                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("INDIVIDUAL_TAX_EXEMPTION::Update::Error occured.", ex);
            }
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(INDIVIDUAL_TAX_EXEMPTION businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.TAX_EXEMPTION_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@IND_TAX_EXEMPTION_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.IND_TAX_EXEMPTION_ID)));
				sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_ID)));
				sqlCommand.Parameters.Add(new AseParameter("@TAX_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.TAX_CD)));
				sqlCommand.Parameters.Add(new AseParameter("@EXEMPTION_PCT", AseDbType.Decimal, 4, ParameterDirection.Input, false, 7, 4, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.EXEMPTION_PCT)));
				sqlCommand.Parameters.Add(new AseParameter("@CURRENT_FROM", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CURRENT_FROM, "dd/MM/yyyy")));
				sqlCommand.Parameters.Add(new AseParameter("@CURRENT_TO", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CURRENT_TO, "dd/MM/yyyy")));
				sqlCommand.Parameters.Add(new AseParameter("@TAX_CATEGORY_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.TAX_CATEGORY_CD)));
				sqlCommand.Parameters.Add(new AseParameter("@STATE_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.STATE_CD)));
				sqlCommand.Parameters.Add(new AseParameter("@COUNTRY_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.COUNTRY_CD)));
				sqlCommand.Parameters.Add(new AseParameter("@BULLETIN_DATE", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.BULLETIN_DATE, "dd/MM/yyyy")));
				sqlCommand.Parameters.Add(new AseParameter("@RESOLUTION_NUMBER", AseDbType.VarChar, 15, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.RESOLUTION_NUMBER)));
				sqlCommand.Parameters.Add(new AseParameter("@HAS_FULL_RETENTION", AseDbType.Bit, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.HAS_FULL_RETENTION)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));
                 
                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("INDIVIDUAL_TAX_EXEMPTION::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(INDIVIDUAL_TAX_EXEMPTION businessObject, IDataReader dataReader)
        {


				businessObject.IND_TAX_EXEMPTION_ID = dataReader.GetInt32(dataReader.GetOrdinal(INDIVIDUAL_TAX_EXEMPTION.INDIVIDUAL_TAX_EXEMPTIONFields.IND_TAX_EXEMPTION_ID.ToString()));

				businessObject.INDIVIDUAL_ID = dataReader.GetInt32(dataReader.GetOrdinal(INDIVIDUAL_TAX_EXEMPTION.INDIVIDUAL_TAX_EXEMPTIONFields.INDIVIDUAL_ID.ToString()));

				businessObject.TAX_CD = dataReader.GetInt32(dataReader.GetOrdinal(INDIVIDUAL_TAX_EXEMPTION.INDIVIDUAL_TAX_EXEMPTIONFields.TAX_CD.ToString()));

                businessObject.EXEMPTION_PCT = dataReader.GetDouble(dataReader.GetOrdinal(INDIVIDUAL_TAX_EXEMPTION.INDIVIDUAL_TAX_EXEMPTIONFields.EXEMPTION_PCT.ToString()));

				businessObject.CURRENT_FROM = dataReader.GetString(dataReader.GetOrdinal(INDIVIDUAL_TAX_EXEMPTION.INDIVIDUAL_TAX_EXEMPTIONFields.CURRENT_FROM.ToString()));

				if (!dataReader.IsDBNull(dataReader.GetOrdinal(INDIVIDUAL_TAX_EXEMPTION.INDIVIDUAL_TAX_EXEMPTIONFields.CURRENT_TO.ToString())))
				{
					businessObject.CURRENT_TO = dataReader.GetString(dataReader.GetOrdinal(INDIVIDUAL_TAX_EXEMPTION.INDIVIDUAL_TAX_EXEMPTIONFields.CURRENT_TO.ToString()));
				}

				if (!dataReader.IsDBNull(dataReader.GetOrdinal(INDIVIDUAL_TAX_EXEMPTION.INDIVIDUAL_TAX_EXEMPTIONFields.TAX_CATEGORY_CD.ToString())))
				{
					businessObject.TAX_CATEGORY_CD = dataReader.GetString(dataReader.GetOrdinal(INDIVIDUAL_TAX_EXEMPTION.INDIVIDUAL_TAX_EXEMPTIONFields.TAX_CATEGORY_CD.ToString()));
				}

				if (!dataReader.IsDBNull(dataReader.GetOrdinal(INDIVIDUAL_TAX_EXEMPTION.INDIVIDUAL_TAX_EXEMPTIONFields.STATE_CD.ToString())))
				{
					businessObject.STATE_CD = dataReader.GetString(dataReader.GetOrdinal(INDIVIDUAL_TAX_EXEMPTION.INDIVIDUAL_TAX_EXEMPTIONFields.STATE_CD.ToString()));
				}

				if (!dataReader.IsDBNull(dataReader.GetOrdinal(INDIVIDUAL_TAX_EXEMPTION.INDIVIDUAL_TAX_EXEMPTIONFields.COUNTRY_CD.ToString())))
				{
					businessObject.COUNTRY_CD = dataReader.GetString(dataReader.GetOrdinal(INDIVIDUAL_TAX_EXEMPTION.INDIVIDUAL_TAX_EXEMPTIONFields.COUNTRY_CD.ToString()));
				}

				if (!dataReader.IsDBNull(dataReader.GetOrdinal(INDIVIDUAL_TAX_EXEMPTION.INDIVIDUAL_TAX_EXEMPTIONFields.BULLETIN_DATE.ToString())))
				{
					businessObject.BULLETIN_DATE = dataReader.GetString(dataReader.GetOrdinal(INDIVIDUAL_TAX_EXEMPTION.INDIVIDUAL_TAX_EXEMPTIONFields.BULLETIN_DATE.ToString()));
				}

				if (!dataReader.IsDBNull(dataReader.GetOrdinal(INDIVIDUAL_TAX_EXEMPTION.INDIVIDUAL_TAX_EXEMPTIONFields.RESOLUTION_NUMBER.ToString())))
				{
					businessObject.RESOLUTION_NUMBER = dataReader.GetString(dataReader.GetOrdinal(INDIVIDUAL_TAX_EXEMPTION.INDIVIDUAL_TAX_EXEMPTIONFields.RESOLUTION_NUMBER.ToString()));
				}

				businessObject.HAS_FULL_RETENTION = dataReader.GetBoolean(dataReader.GetOrdinal(INDIVIDUAL_TAX_EXEMPTION.INDIVIDUAL_TAX_EXEMPTIONFields.HAS_FULL_RETENTION.ToString()));


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of INDIVIDUAL_TAX_EXEMPTION</returns>
        internal List<INDIVIDUAL_TAX_EXEMPTION> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<INDIVIDUAL_TAX_EXEMPTION> list = new List<INDIVIDUAL_TAX_EXEMPTION>();

            while (dataReader.Read())
            {
                INDIVIDUAL_TAX_EXEMPTION businessObject = new INDIVIDUAL_TAX_EXEMPTION();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

	}
}
