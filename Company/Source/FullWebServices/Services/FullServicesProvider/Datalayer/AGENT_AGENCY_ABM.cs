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
    /// Data access layer class for AGENT_AGENCY
    /// </summary>
    class AGENT_AGENCY_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public AGENT_AGENCY_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public AGENT_AGENCY_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public AGENT_AGENCY_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
        public bool Insert(AGENT_AGENCY businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.AGENT_AGENCY_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@AGENT_AGENCY_ID", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.AGENT_AGENCY_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@DESCRIPTION", AseDbType.VarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.DESCRIPTION)));
                sqlCommand.Parameters.Add(new AseParameter("@BRANCH_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.BRANCH_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@ANNOTATIONS", AseDbType.VarChar, 80, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ANNOTATIONS)));
                sqlCommand.Parameters.Add(new AseParameter("@AGENT_CD", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.AGENT_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@AGENCY_GROUP_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.AGENCY_GROUP_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@DECLINED_DATE", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.DECLINED_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@AGENT_DECLINED_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.AGENT_DECLINED_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@AGENT_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.AGENT_TYPE_CD)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("AGENT_AGENCY::Insert::Error occured.", ex);
            }
        }

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(AGENT_AGENCY businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.AGENT_AGENCY_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@AGENT_AGENCY_ID", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.AGENT_AGENCY_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@DESCRIPTION", AseDbType.VarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.DESCRIPTION)));
                sqlCommand.Parameters.Add(new AseParameter("@BRANCH_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.BRANCH_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@ANNOTATIONS", AseDbType.VarChar, 80, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ANNOTATIONS)));
                sqlCommand.Parameters.Add(new AseParameter("@AGENT_CD", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.AGENT_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@AGENCY_GROUP_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.AGENCY_GROUP_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@DECLINED_DATE", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.DECLINED_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@AGENT_DECLINED_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.AGENT_DECLINED_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@AGENT_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.AGENT_TYPE_CD)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("AGENT_AGENCY::Update::Error occured.", ex);
            }
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(AGENT_AGENCY businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.AGENT_AGENCY_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@AGENT_AGENCY_ID", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.AGENT_AGENCY_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@DESCRIPTION", AseDbType.VarChar, 50, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.DESCRIPTION)));
                sqlCommand.Parameters.Add(new AseParameter("@BRANCH_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.BRANCH_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@ANNOTATIONS", AseDbType.VarChar, 80, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ANNOTATIONS)));
                sqlCommand.Parameters.Add(new AseParameter("@AGENT_CD", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.AGENT_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@AGENCY_GROUP_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.AGENCY_GROUP_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@DECLINED_DATE", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.DECLINED_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@AGENT_DECLINED_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.AGENT_DECLINED_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@AGENT_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.AGENT_TYPE_CD)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("AGENT_AGENCY::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(AGENT_AGENCY businessObject, IDataReader dataReader)
        {


            businessObject.INDIVIDUAL_ID = dataReader.GetInt32(dataReader.GetOrdinal(AGENT_AGENCY.AGENT_AGENCYFields.INDIVIDUAL_ID.ToString()));

            businessObject.AGENT_AGENCY_ID = dataReader.GetInt32(dataReader.GetOrdinal(AGENT_AGENCY.AGENT_AGENCYFields.AGENT_AGENCY_ID.ToString()));

            businessObject.DESCRIPTION = dataReader.GetString(dataReader.GetOrdinal(AGENT_AGENCY.AGENT_AGENCYFields.DESCRIPTION.ToString()));

            businessObject.BRANCH_CD = dataReader.GetInt32(dataReader.GetOrdinal(AGENT_AGENCY.AGENT_AGENCYFields.BRANCH_CD.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(AGENT_AGENCY.AGENT_AGENCYFields.ANNOTATIONS.ToString())))
            {
                businessObject.ANNOTATIONS = dataReader.GetString(dataReader.GetOrdinal(AGENT_AGENCY.AGENT_AGENCYFields.ANNOTATIONS.ToString()));
            }

            businessObject.AGENT_CD = dataReader.GetInt32(dataReader.GetOrdinal(AGENT_AGENCY.AGENT_AGENCYFields.AGENT_CD.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(AGENT_AGENCY.AGENT_AGENCYFields.AGENCY_GROUP_CD.ToString())))
            {
                businessObject.AGENCY_GROUP_CD = dataReader.GetString(dataReader.GetOrdinal(AGENT_AGENCY.AGENT_AGENCYFields.AGENCY_GROUP_CD.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(AGENT_AGENCY.AGENT_AGENCYFields.DECLINED_DATE.ToString())))
            {
                businessObject.DECLINED_DATE = dataReader.GetString(dataReader.GetOrdinal(AGENT_AGENCY.AGENT_AGENCYFields.DECLINED_DATE.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(AGENT_AGENCY.AGENT_AGENCYFields.AGENT_DECLINED_TYPE_CD.ToString())))
            {
                businessObject.AGENT_DECLINED_TYPE_CD = dataReader.GetString(dataReader.GetOrdinal(AGENT_AGENCY.AGENT_AGENCYFields.AGENT_DECLINED_TYPE_CD.ToString()));
            }

            businessObject.AGENT_TYPE_CD = dataReader.GetInt32(dataReader.GetOrdinal(AGENT_AGENCY.AGENT_AGENCYFields.AGENT_TYPE_CD.ToString()));


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of AGENT_AGENCY</returns>
        internal List<AGENT_AGENCY> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<AGENT_AGENCY> list = new List<AGENT_AGENCY>();

            while (dataReader.Read())
            {
                AGENT_AGENCY businessObject = new AGENT_AGENCY();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

    }
}
