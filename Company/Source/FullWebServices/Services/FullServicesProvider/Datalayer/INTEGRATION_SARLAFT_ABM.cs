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
	/// Data access layer class for INTEGRATION_SARLAFT
	/// </summary>
	class INTEGRATION_SARLAFT_ABM : DataLayerBase 
	{

        #region Constructor

		/// <summary>
		/// Class constructor
		/// </summary>
		public INTEGRATION_SARLAFT_ABM()
		{
			// Nothing for now.
		}

    	/// <summary>
		/// Class constructor
		/// </summary>
		public INTEGRATION_SARLAFT_ABM(string Connection)
            : base(Connection) 
		{
			// Nothing for now.
		}

        /// <summary>
        /// Class constructor
        /// </summary>
        public INTEGRATION_SARLAFT_ABM(string Connection, string userId, int AppId)
            : base(Connection, userId, AppId)
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
		public bool Insert(INTEGRATION_SARLAFT businessObject)
		{
			AseCommand	sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.INTEGRATION_SARLAFT_Insert";
			sqlCommand.CommandType = CommandType.StoredProcedure;

			try
			{
                sqlCommand.Parameters.Clear();
				sqlCommand.Parameters.Add(new AseParameter("@ID_PERSON", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ID_PERSON)));
				sqlCommand.Parameters.Add(new AseParameter("@ID_FORM", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ID_FORM)));
				sqlCommand.Parameters.Add(new AseParameter("@NUM_FORM", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.NUM_FORM)));
                sqlCommand.Parameters.Add(new AseParameter("@DATE_CREATE", AseDbType.DateTime, 16, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.DATE_CREATE, "dd/MM/yyyy")));
				sqlCommand.Parameters.Add(new AseParameter("@BRANCH", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.BRANCH)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));
 								
				sqlCommand.ExecuteNonQuery();
               
				return true;
			}
			catch(Exception ex)
			{				
				throw new SupException("INTEGRATION_SARLAFT::Insert::Error occured.", ex);
			}
		}

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(INTEGRATION_SARLAFT businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.INTEGRATION_SARLAFT_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@ID_PERSON", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ID_PERSON)));
				sqlCommand.Parameters.Add(new AseParameter("@ID_FORM", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ID_FORM)));
				sqlCommand.Parameters.Add(new AseParameter("@NUM_FORM", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.NUM_FORM)));
                sqlCommand.Parameters.Add(new AseParameter("@DATE_CREATE", AseDbType.DateTime, 16, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.DATE_CREATE, "dd/MM/yyyy")));
				sqlCommand.Parameters.Add(new AseParameter("@BRANCH", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.BRANCH)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));

                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("INTEGRATION_SARLAFT::Update::Error occured.", ex);
            }
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(INTEGRATION_SARLAFT businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.INTEGRATION_SARLAFT_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@ID_PERSON", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ID_PERSON)));
				sqlCommand.Parameters.Add(new AseParameter("@ID_FORM", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ID_FORM)));
				sqlCommand.Parameters.Add(new AseParameter("@NUM_FORM", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.NUM_FORM)));
				sqlCommand.Parameters.Add(new AseParameter("@DATE_CREATE", AseDbType.DateTime, 16, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.DATE_CREATE, "dd/MM/yyyy")));
				sqlCommand.Parameters.Add(new AseParameter("@BRANCH", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.BRANCH)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));

                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("INTEGRATION_SARLAFT::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(INTEGRATION_SARLAFT businessObject, IDataReader dataReader)
        {
				businessObject.ID_PERSON = dataReader.GetInt32(dataReader.GetOrdinal(INTEGRATION_SARLAFT.INTEGRATION_SARLAFTFields.ID_PERSON.ToString()));

				businessObject.ID_FORM = dataReader.GetInt32(dataReader.GetOrdinal(INTEGRATION_SARLAFT.INTEGRATION_SARLAFTFields.ID_FORM.ToString()));

				businessObject.NUM_FORM = dataReader.GetInt32(dataReader.GetOrdinal(INTEGRATION_SARLAFT.INTEGRATION_SARLAFTFields.NUM_FORM.ToString()));

				if (!dataReader.IsDBNull(dataReader.GetOrdinal(INTEGRATION_SARLAFT.INTEGRATION_SARLAFTFields.DATE_CREATE.ToString())))
				{
					businessObject.DATE_CREATE = dataReader.GetString(dataReader.GetOrdinal(INTEGRATION_SARLAFT.INTEGRATION_SARLAFTFields.DATE_CREATE.ToString()));
				}

				if (!dataReader.IsDBNull(dataReader.GetOrdinal(INTEGRATION_SARLAFT.INTEGRATION_SARLAFTFields.BRANCH.ToString())))
				{
					businessObject.BRANCH = dataReader.GetString(dataReader.GetOrdinal(INTEGRATION_SARLAFT.INTEGRATION_SARLAFTFields.BRANCH.ToString()));
				}


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of INTEGRATION_SARLAFT</returns>
        internal List<INTEGRATION_SARLAFT> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<INTEGRATION_SARLAFT> list = new List<INTEGRATION_SARLAFT>();

            while (dataReader.Read())
            {
                INTEGRATION_SARLAFT businessObject = new INTEGRATION_SARLAFT();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

	}
}
