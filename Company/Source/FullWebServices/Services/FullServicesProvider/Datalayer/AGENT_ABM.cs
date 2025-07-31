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
    /// Data access layer class for AGENT
    /// </summary>
    class AGENT_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public AGENT_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public AGENT_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public AGENT_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
        public bool Insert(AGENT businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.AGENT_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@AGENT_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.AGENT_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@CHECK_PAYABLE_TO", AseDbType.VarChar, 80, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CHECK_PAYABLE_TO)));
                sqlCommand.Parameters.Add(new AseParameter("@ENTERED_DATE", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ENTERED_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@DECLINED_DATE", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.DECLINED_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@AGENT_DECLINED_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.AGENT_DECLINED_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@LICENSE_NUMBER", AseDbType.BigInt, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.LICENSE_NUMBER)));
                sqlCommand.Parameters.Add(new AseParameter("@LICENSE_DATE", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.LICENSE_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@AGENT_GROUP_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.AGENT_GROUP_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@ANNOTATIONS", AseDbType.VarChar, 80, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ANNOTATIONS)));
                sqlCommand.Parameters.Add(new AseParameter("@REFERRED_BY", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.REFERRED_BY)));
                sqlCommand.Parameters.Add(new AseParameter("@ACC_EXECUTIVE_IND_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ACC_EXECUTIVE_IND_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@SALES_CHANNEL_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.SALES_CHANNEL_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@LOCKER", AseDbType.VarChar, 5, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.LOCKER)));
                sqlCommand.Parameters.Add(new AseParameter("@TYPE_LICENSE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.TYPE_LICENSE_CD)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("AGENT::Insert::Error occured.", ex);
            }
        }

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(AGENT businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.AGENT_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@AGENT_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.AGENT_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@CHECK_PAYABLE_TO", AseDbType.VarChar, 80, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CHECK_PAYABLE_TO)));
                sqlCommand.Parameters.Add(new AseParameter("@ENTERED_DATE", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ENTERED_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@DECLINED_DATE", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.DECLINED_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@AGENT_DECLINED_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.AGENT_DECLINED_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@LICENSE_NUMBER", AseDbType.BigInt, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.LICENSE_NUMBER)));
                sqlCommand.Parameters.Add(new AseParameter("@LICENSE_DATE", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.LICENSE_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@AGENT_GROUP_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.AGENT_GROUP_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@ANNOTATIONS", AseDbType.VarChar, 80, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ANNOTATIONS)));
                sqlCommand.Parameters.Add(new AseParameter("@REFERRED_BY", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.REFERRED_BY)));
                sqlCommand.Parameters.Add(new AseParameter("@ACC_EXECUTIVE_IND_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ACC_EXECUTIVE_IND_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@SALES_CHANNEL_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.SALES_CHANNEL_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@LOCKER", AseDbType.VarChar, 5, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.LOCKER)));
                sqlCommand.Parameters.Add(new AseParameter("@TYPE_LICENSE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.TYPE_LICENSE_CD)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("AGENT::Update::Error occured.", ex);
            }
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(AGENT businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.AGENT_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@AGENT_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.AGENT_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@CHECK_PAYABLE_TO", AseDbType.VarChar, 80, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CHECK_PAYABLE_TO)));
                sqlCommand.Parameters.Add(new AseParameter("@ENTERED_DATE", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ENTERED_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@DECLINED_DATE", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.DECLINED_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@AGENT_DECLINED_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.AGENT_DECLINED_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@LICENSE_NUMBER", AseDbType.BigInt, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.LICENSE_NUMBER)));
                sqlCommand.Parameters.Add(new AseParameter("@LICENSE_DATE", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.LICENSE_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@AGENT_GROUP_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.AGENT_GROUP_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@ANNOTATIONS", AseDbType.VarChar, 80, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ANNOTATIONS)));
                sqlCommand.Parameters.Add(new AseParameter("@REFERRED_BY", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.REFERRED_BY)));
                sqlCommand.Parameters.Add(new AseParameter("@ACC_EXECUTIVE_IND_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ACC_EXECUTIVE_IND_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@SALES_CHANNEL_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.SALES_CHANNEL_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@LOCKER", AseDbType.VarChar, 5, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.LOCKER)));
                sqlCommand.Parameters.Add(new AseParameter("@TYPE_LICENSE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.TYPE_LICENSE_CD)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("AGENT::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(AGENT businessObject, IDataReader dataReader)
        {


            businessObject.INDIVIDUAL_ID = dataReader.GetInt32(dataReader.GetOrdinal(AGENT.AGENTFields.INDIVIDUAL_ID.ToString()));

            businessObject.AGENT_TYPE_CD = dataReader.GetInt32(dataReader.GetOrdinal(AGENT.AGENTFields.AGENT_TYPE_CD.ToString()));

            businessObject.CHECK_PAYABLE_TO = dataReader.GetString(dataReader.GetOrdinal(AGENT.AGENTFields.CHECK_PAYABLE_TO.ToString()));

            businessObject.ENTERED_DATE = dataReader.GetString(dataReader.GetOrdinal(AGENT.AGENTFields.ENTERED_DATE.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(AGENT.AGENTFields.DECLINED_DATE.ToString())))
            {
                businessObject.DECLINED_DATE = dataReader.GetString(dataReader.GetOrdinal(AGENT.AGENTFields.DECLINED_DATE.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(AGENT.AGENTFields.AGENT_DECLINED_TYPE_CD.ToString())))
            {
                businessObject.AGENT_DECLINED_TYPE_CD = dataReader.GetString(dataReader.GetOrdinal(AGENT.AGENTFields.AGENT_DECLINED_TYPE_CD.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(AGENT.AGENTFields.LICENSE_NUMBER.ToString())))
            {
                businessObject.LICENSE_NUMBER = dataReader.GetString(dataReader.GetOrdinal(AGENT.AGENTFields.LICENSE_NUMBER.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(AGENT.AGENTFields.LICENSE_DATE.ToString())))
            {
                businessObject.LICENSE_DATE = dataReader.GetString(dataReader.GetOrdinal(AGENT.AGENTFields.LICENSE_DATE.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(AGENT.AGENTFields.AGENT_GROUP_CD.ToString())))
            {
                businessObject.AGENT_GROUP_CD = dataReader.GetString(dataReader.GetOrdinal(AGENT.AGENTFields.AGENT_GROUP_CD.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(AGENT.AGENTFields.ANNOTATIONS.ToString())))
            {
                businessObject.ANNOTATIONS = dataReader.GetString(dataReader.GetOrdinal(AGENT.AGENTFields.ANNOTATIONS.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(AGENT.AGENTFields.REFERRED_BY.ToString())))
            {
                businessObject.REFERRED_BY = dataReader.GetString(dataReader.GetOrdinal(AGENT.AGENTFields.REFERRED_BY.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(AGENT.AGENTFields.ACC_EXECUTIVE_IND_ID.ToString())))
            {
                businessObject.ACC_EXECUTIVE_IND_ID = dataReader.GetString(dataReader.GetOrdinal(AGENT.AGENTFields.ACC_EXECUTIVE_IND_ID.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(AGENT.AGENTFields.SALES_CHANNEL_CD.ToString())))
            {
                businessObject.SALES_CHANNEL_CD = dataReader.GetString(dataReader.GetOrdinal(AGENT.AGENTFields.SALES_CHANNEL_CD.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(AGENT.AGENTFields.LOCKER.ToString())))
            {
                businessObject.LOCKER = dataReader.GetString(dataReader.GetOrdinal(AGENT.AGENTFields.LOCKER.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(AGENT.AGENTFields.TYPE_LICENSE_CD.ToString())))
            {
                businessObject.TYPE_LICENSE_CD = dataReader.GetString(dataReader.GetOrdinal(AGENT.AGENTFields.TYPE_LICENSE_CD.ToString()));
            }


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of AGENT</returns>
        internal List<AGENT> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<AGENT> list = new List<AGENT>();

            while (dataReader.Read())
            {
                AGENT businessObject = new AGENT();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

    }
}
