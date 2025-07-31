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
    /// Data access layer class for INDIVIDUAL
    /// </summary>
    class INDIVIDUAL_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public INDIVIDUAL_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public INDIVIDUAL_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public INDIVIDUAL_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
        public bool Insert(INDIVIDUAL businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.INDIVIDUAL_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_TYPE_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@AT_DATA_ID", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.AT_DATA_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@AT_PAYMENT_ID", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.AT_PAYMENT_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@AT_AGENT_AGENCY_ID", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.AT_AGENT_AGENCY_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@OWNER_ROLE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.OWNER_ROLE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@ECONOMIC_ACTIVITY_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ECONOMIC_ACTIVITY_CD)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("INDIVIDUAL::Insert::Error occured.", ex);
            }
        }

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(INDIVIDUAL businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.INDIVIDUAL_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_TYPE_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@AT_DATA_ID", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.AT_DATA_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@AT_PAYMENT_ID", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.AT_PAYMENT_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@AT_AGENT_AGENCY_ID", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.AT_AGENT_AGENCY_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@OWNER_ROLE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.OWNER_ROLE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@ECONOMIC_ACTIVITY_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ECONOMIC_ACTIVITY_CD)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("INDIVIDUAL::Update::Error occured.", ex);
            }
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(INDIVIDUAL businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.INDIVIDUAL_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_TYPE_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@AT_DATA_ID", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.AT_DATA_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@AT_PAYMENT_ID", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.AT_PAYMENT_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@AT_AGENT_AGENCY_ID", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.AT_AGENT_AGENCY_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@OWNER_ROLE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.OWNER_ROLE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@ECONOMIC_ACTIVITY_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ECONOMIC_ACTIVITY_CD)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("INDIVIDUAL::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(INDIVIDUAL businessObject, IDataReader dataReader)
        {


            businessObject.INDIVIDUAL_ID = dataReader.GetInt32(dataReader.GetOrdinal(INDIVIDUAL.INDIVIDUALFields.INDIVIDUAL_ID.ToString()));

            businessObject.INDIVIDUAL_TYPE_CD = dataReader.GetInt32(dataReader.GetOrdinal(INDIVIDUAL.INDIVIDUALFields.INDIVIDUAL_TYPE_CD.ToString()));

            businessObject.AT_DATA_ID = dataReader.GetInt32(dataReader.GetOrdinal(INDIVIDUAL.INDIVIDUALFields.AT_DATA_ID.ToString()));

            businessObject.AT_PAYMENT_ID = dataReader.GetInt32(dataReader.GetOrdinal(INDIVIDUAL.INDIVIDUALFields.AT_PAYMENT_ID.ToString()));

            businessObject.AT_AGENT_AGENCY_ID = dataReader.GetInt32(dataReader.GetOrdinal(INDIVIDUAL.INDIVIDUALFields.AT_AGENT_AGENCY_ID.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(INDIVIDUAL.INDIVIDUALFields.OWNER_ROLE_CD.ToString())))
            {
                businessObject.OWNER_ROLE_CD = dataReader.GetString(dataReader.GetOrdinal(INDIVIDUAL.INDIVIDUALFields.OWNER_ROLE_CD.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(INDIVIDUAL.INDIVIDUALFields.ECONOMIC_ACTIVITY_CD.ToString())))
            {
                businessObject.ECONOMIC_ACTIVITY_CD = dataReader.GetString(dataReader.GetOrdinal(INDIVIDUAL.INDIVIDUALFields.ECONOMIC_ACTIVITY_CD.ToString()));
            }


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of INDIVIDUAL</returns>
        internal List<INDIVIDUAL> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<INDIVIDUAL> list = new List<INDIVIDUAL>();

            while (dataReader.Read())
            {
                INDIVIDUAL businessObject = new INDIVIDUAL();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

    }
}
