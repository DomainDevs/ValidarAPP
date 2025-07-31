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
    /// Data access layer class for INDIVIDUAL_SARLAFT
    /// </summary>
    class INDIVIDUAL_SARLAFT_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public INDIVIDUAL_SARLAFT_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public INDIVIDUAL_SARLAFT_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public INDIVIDUAL_SARLAFT_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
        public bool Insert(INDIVIDUAL_SARLAFT businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.INDIVIDUAL_SARLAF_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@SARLAFT_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.SARLAFT_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@FORM_NUM", AseDbType.VarChar, 12, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.FORM_NUM)));
                sqlCommand.Parameters.Add(new AseParameter("@YEAR", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.YEAR)));
                sqlCommand.Parameters.Add(new AseParameter("@REGISTRATION_DATE", AseDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.REGISTRATION_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@AUTHORIZED_BY", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.AUTHORIZED_BY)));
                sqlCommand.Parameters.Add(new AseParameter("@FILLING_DATE", AseDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.FILLING_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@CHECK_DATE", AseDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CHECK_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@VERIFYING_EMPLOYEE", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.VERIFYING_EMPLOYEE)));
                sqlCommand.Parameters.Add(new AseParameter("@INTERVIEW_DATE", AseDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INTERVIEW_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@INTERVIEWER_NAME", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INTERVIEWER_NAME)));
                sqlCommand.Parameters.Add(new AseParameter("@INTERNATIONAL_OPERATIONS", AseDbType.Bit, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INTERNATIONAL_OPERATIONS)));
                sqlCommand.Parameters.Add(new AseParameter("@INTERVIEW_PLACE", AseDbType.VarChar, 120, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INTERVIEW_PLACE)));
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@USER_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.USER_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@BRANCH_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.BRANCH_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@ECONOMIC_ACTIVITY_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ECONOMIC_ACTIVITY_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@SECOND_ECONOMIC_ACTIVITY_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.SECOND_ECONOMIC_ACTIVITY_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@INTERVIEW_RESULT_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INTERVIEW_RESULT_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@PENDING_EVENT", AseDbType.Bit, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.PENDING_EVENT)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("INDIVIDUAL_SARLAFT::Insert::Error occured.", ex);
            }
        }

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(INDIVIDUAL_SARLAFT businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.INDIVIDUAL_SARLAF_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@SARLAFT_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.SARLAFT_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@FORM_NUM", AseDbType.VarChar, 12, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.FORM_NUM)));
                sqlCommand.Parameters.Add(new AseParameter("@YEAR", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.YEAR)));
                sqlCommand.Parameters.Add(new AseParameter("@REGISTRATION_DATE", AseDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.REGISTRATION_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@AUTHORIZED_BY", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.AUTHORIZED_BY)));
                sqlCommand.Parameters.Add(new AseParameter("@FILLING_DATE", AseDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.FILLING_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@CHECK_DATE", AseDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CHECK_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@VERIFYING_EMPLOYEE", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.VERIFYING_EMPLOYEE)));
                sqlCommand.Parameters.Add(new AseParameter("@INTERVIEW_DATE", AseDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INTERVIEW_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@INTERVIEWER_NAME", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INTERVIEWER_NAME)));
                sqlCommand.Parameters.Add(new AseParameter("@INTERNATIONAL_OPERATIONS", AseDbType.Bit, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INTERNATIONAL_OPERATIONS)));
                sqlCommand.Parameters.Add(new AseParameter("@INTERVIEW_PLACE", AseDbType.VarChar, 120, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INTERVIEW_PLACE)));
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@USER_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.USER_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@BRANCH_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.BRANCH_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@ECONOMIC_ACTIVITY_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ECONOMIC_ACTIVITY_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@SECOND_ECONOMIC_ACTIVITY_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.SECOND_ECONOMIC_ACTIVITY_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@INTERVIEW_RESULT_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INTERVIEW_RESULT_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@PENDING_EVENT", AseDbType.Bit, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.PENDING_EVENT)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("INDIVIDUAL_SARLAFT::Update::Error occured.", ex);
            }
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(INDIVIDUAL_SARLAFT businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.INDIVIDUAL_SARLAF_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@SARLAFT_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.SARLAFT_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@FORM_NUM", AseDbType.VarChar, 12, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.FORM_NUM)));
                sqlCommand.Parameters.Add(new AseParameter("@YEAR", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.YEAR)));
                sqlCommand.Parameters.Add(new AseParameter("@REGISTRATION_DATE", AseDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.REGISTRATION_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@AUTHORIZED_BY", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.AUTHORIZED_BY)));
                sqlCommand.Parameters.Add(new AseParameter("@FILLING_DATE", AseDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.FILLING_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@CHECK_DATE", AseDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CHECK_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@VERIFYING_EMPLOYEE", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.VERIFYING_EMPLOYEE)));
                sqlCommand.Parameters.Add(new AseParameter("@INTERVIEW_DATE", AseDbType.DateTime, 8, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INTERVIEW_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@INTERVIEWER_NAME", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INTERVIEWER_NAME)));
                sqlCommand.Parameters.Add(new AseParameter("@INTERNATIONAL_OPERATIONS", AseDbType.Bit, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INTERNATIONAL_OPERATIONS)));
                sqlCommand.Parameters.Add(new AseParameter("@INTERVIEW_PLACE", AseDbType.VarChar, 120, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INTERVIEW_PLACE)));
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@USER_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.USER_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@BRANCH_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.BRANCH_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@ECONOMIC_ACTIVITY_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ECONOMIC_ACTIVITY_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@SECOND_ECONOMIC_ACTIVITY_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.SECOND_ECONOMIC_ACTIVITY_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@INTERVIEW_RESULT_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INTERVIEW_RESULT_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@PENDING_EVENT", AseDbType.Bit, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.PENDING_EVENT)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("INDIVIDUAL_SARLAFT::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(INDIVIDUAL_SARLAFT businessObject, IDataReader dataReader)
        {


            businessObject.SARLAFT_ID = dataReader.GetInt32(dataReader.GetOrdinal(INDIVIDUAL_SARLAFT.INDIVIDUAL_SARLAFTFields.SARLAFT_ID.ToString()));

            businessObject.FORM_NUM = dataReader.GetString(dataReader.GetOrdinal(INDIVIDUAL_SARLAFT.INDIVIDUAL_SARLAFTFields.FORM_NUM.ToString()));

            businessObject.YEAR = dataReader.GetInt32(dataReader.GetOrdinal(INDIVIDUAL_SARLAFT.INDIVIDUAL_SARLAFTFields.YEAR.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(INDIVIDUAL_SARLAFT.INDIVIDUAL_SARLAFTFields.REGISTRATION_DATE.ToString())))
            {
                businessObject.REGISTRATION_DATE = dataReader.GetString(dataReader.GetOrdinal(INDIVIDUAL_SARLAFT.INDIVIDUAL_SARLAFTFields.REGISTRATION_DATE.ToString()));
            }

            businessObject.AUTHORIZED_BY = dataReader.GetString(dataReader.GetOrdinal(INDIVIDUAL_SARLAFT.INDIVIDUAL_SARLAFTFields.AUTHORIZED_BY.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(INDIVIDUAL_SARLAFT.INDIVIDUAL_SARLAFTFields.FILLING_DATE.ToString())))
            {
                businessObject.FILLING_DATE = dataReader.GetString(dataReader.GetOrdinal(INDIVIDUAL_SARLAFT.INDIVIDUAL_SARLAFTFields.FILLING_DATE.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(INDIVIDUAL_SARLAFT.INDIVIDUAL_SARLAFTFields.CHECK_DATE.ToString())))
            {
                businessObject.CHECK_DATE = dataReader.GetString(dataReader.GetOrdinal(INDIVIDUAL_SARLAFT.INDIVIDUAL_SARLAFTFields.CHECK_DATE.ToString()));
            }

            businessObject.VERIFYING_EMPLOYEE = dataReader.GetString(dataReader.GetOrdinal(INDIVIDUAL_SARLAFT.INDIVIDUAL_SARLAFTFields.VERIFYING_EMPLOYEE.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(INDIVIDUAL_SARLAFT.INDIVIDUAL_SARLAFTFields.INTERVIEW_DATE.ToString())))
            {
                businessObject.INTERVIEW_DATE = dataReader.GetString(dataReader.GetOrdinal(INDIVIDUAL_SARLAFT.INDIVIDUAL_SARLAFTFields.INTERVIEW_DATE.ToString()));
            }

            businessObject.INTERVIEWER_NAME = dataReader.GetString(dataReader.GetOrdinal(INDIVIDUAL_SARLAFT.INDIVIDUAL_SARLAFTFields.INTERVIEWER_NAME.ToString()));

            businessObject.INTERNATIONAL_OPERATIONS = dataReader.GetBoolean(dataReader.GetOrdinal(INDIVIDUAL_SARLAFT.INDIVIDUAL_SARLAFTFields.INTERNATIONAL_OPERATIONS.ToString()));

            businessObject.INTERVIEW_PLACE = dataReader.GetString(dataReader.GetOrdinal(INDIVIDUAL_SARLAFT.INDIVIDUAL_SARLAFTFields.INTERVIEW_PLACE.ToString()));

            businessObject.INDIVIDUAL_ID = dataReader.GetInt32(dataReader.GetOrdinal(INDIVIDUAL_SARLAFT.INDIVIDUAL_SARLAFTFields.INDIVIDUAL_ID.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(INDIVIDUAL_SARLAFT.INDIVIDUAL_SARLAFTFields.USER_ID.ToString())))
            {
                businessObject.USER_ID = dataReader.GetString(dataReader.GetOrdinal(INDIVIDUAL_SARLAFT.INDIVIDUAL_SARLAFTFields.USER_ID.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(INDIVIDUAL_SARLAFT.INDIVIDUAL_SARLAFTFields.BRANCH_CD.ToString())))
            {
                businessObject.BRANCH_CD = dataReader.GetString(dataReader.GetOrdinal(INDIVIDUAL_SARLAFT.INDIVIDUAL_SARLAFTFields.BRANCH_CD.ToString()));
            }

            businessObject.ECONOMIC_ACTIVITY_CD = dataReader.GetInt32(dataReader.GetOrdinal(INDIVIDUAL_SARLAFT.INDIVIDUAL_SARLAFTFields.ECONOMIC_ACTIVITY_CD.ToString()));

            businessObject.SECOND_ECONOMIC_ACTIVITY_CD = dataReader.GetInt32(dataReader.GetOrdinal(INDIVIDUAL_SARLAFT.INDIVIDUAL_SARLAFTFields.SECOND_ECONOMIC_ACTIVITY_CD.ToString()));

            businessObject.INTERVIEW_RESULT_CD = dataReader.GetInt32(dataReader.GetOrdinal(INDIVIDUAL_SARLAFT.INDIVIDUAL_SARLAFTFields.INTERVIEW_RESULT_CD.ToString()));

            businessObject.PENDING_EVENT = dataReader.GetBoolean(dataReader.GetOrdinal(INDIVIDUAL_SARLAFT.INDIVIDUAL_SARLAFTFields.PENDING_EVENT.ToString()));


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of INDIVIDUAL_SARLAFT</returns>
        internal List<INDIVIDUAL_SARLAFT> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<INDIVIDUAL_SARLAFT> list = new List<INDIVIDUAL_SARLAFT>();

            while (dataReader.Read())
            {
                INDIVIDUAL_SARLAFT businessObject = new INDIVIDUAL_SARLAFT();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

    }
}
