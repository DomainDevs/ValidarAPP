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
	/// Data access layer class for CO_PRV_INDIVIDUAL
	/// </summary>
	class CO_PRV_INDIVIDUAL_ABM : DataLayerBase 
	{

        #region Constructor

		/// <summary>
		/// Class constructor
		/// </summary>
		public CO_PRV_INDIVIDUAL_ABM()
		{
			// Nothing for now.
		}

    	/// <summary>
		/// Class constructor
		/// </summary>
		public CO_PRV_INDIVIDUAL_ABM(string Connection)
            : base(Connection) 
		{
			// Nothing for now.
		}

        /// <summary>
        /// Class constructor
        /// </summary>
        public CO_PRV_INDIVIDUAL_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
		public bool Insert(CO_PRV_INDIVIDUAL businessObject)
		{
			AseCommand	sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.CO_PRV_INDIVIDUAL_Insert";
			sqlCommand.CommandType = CommandType.StoredProcedure;

			try
			{
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_ID)));
				sqlCommand.Parameters.Add(new AseParameter("@ECONOMIC_ACTIVITY_CD_NEW", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ECONOMIC_ACTIVITY_CD_NEW)));
				sqlCommand.Parameters.Add(new AseParameter("@SECOND_ECONOMIC_ACTIVITY_CD_NEW", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.SECOND_ECONOMIC_ACTIVITY_CD_NEW)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));
 								
				
				sqlCommand.ExecuteNonQuery();
               
				return true;
			}
			catch(Exception ex)
			{				
				throw new SupException("CO_PRV_INDIVIDUAL::Insert::Error occured.", ex);
			}
		}

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(CO_PRV_INDIVIDUAL businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.CO_PRV_INDIVIDUAL_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_ID)));
				sqlCommand.Parameters.Add(new AseParameter("@ECONOMIC_ACTIVITY_CD_NEW", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ECONOMIC_ACTIVITY_CD_NEW)));
				sqlCommand.Parameters.Add(new AseParameter("@SECOND_ECONOMIC_ACTIVITY_CD_NEW", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.SECOND_ECONOMIC_ACTIVITY_CD_NEW)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));
                 
                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("CO_PRV_INDIVIDUAL::Update::Error occured.", ex);
            }
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(CO_PRV_INDIVIDUAL businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.CO_PRV_INDIVIDUAL_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_ID)));
				sqlCommand.Parameters.Add(new AseParameter("@ECONOMIC_ACTIVITY_CD_NEW", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ECONOMIC_ACTIVITY_CD_NEW)));
				sqlCommand.Parameters.Add(new AseParameter("@SECOND_ECONOMIC_ACTIVITY_CD_NEW", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.SECOND_ECONOMIC_ACTIVITY_CD_NEW)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));
                 
                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("CO_PRV_INDIVIDUAL::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(CO_PRV_INDIVIDUAL businessObject, IDataReader dataReader)
        {


				businessObject.INDIVIDUAL_ID = dataReader.GetInt32(dataReader.GetOrdinal(CO_PRV_INDIVIDUAL.CO_PRV_INDIVIDUALFields.INDIVIDUAL_ID.ToString()));

				if (!dataReader.IsDBNull(dataReader.GetOrdinal(CO_PRV_INDIVIDUAL.CO_PRV_INDIVIDUALFields.ECONOMIC_ACTIVITY_CD_NEW.ToString())))
				{
					businessObject.ECONOMIC_ACTIVITY_CD_NEW = dataReader.GetString(dataReader.GetOrdinal(CO_PRV_INDIVIDUAL.CO_PRV_INDIVIDUALFields.ECONOMIC_ACTIVITY_CD_NEW.ToString()));
				}

				if (!dataReader.IsDBNull(dataReader.GetOrdinal(CO_PRV_INDIVIDUAL.CO_PRV_INDIVIDUALFields.SECOND_ECONOMIC_ACTIVITY_CD_NEW.ToString())))
				{
					businessObject.SECOND_ECONOMIC_ACTIVITY_CD_NEW = dataReader.GetString(dataReader.GetOrdinal(CO_PRV_INDIVIDUAL.CO_PRV_INDIVIDUALFields.SECOND_ECONOMIC_ACTIVITY_CD_NEW.ToString()));
				}


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of CO_PRV_INDIVIDUAL</returns>
        internal List<CO_PRV_INDIVIDUAL> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<CO_PRV_INDIVIDUAL> list = new List<CO_PRV_INDIVIDUAL>();

            while (dataReader.Read())
            {
                CO_PRV_INDIVIDUAL businessObject = new CO_PRV_INDIVIDUAL();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

	}
}
