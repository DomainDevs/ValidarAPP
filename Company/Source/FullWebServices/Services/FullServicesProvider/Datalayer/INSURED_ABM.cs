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
    /// Data access layer class for INSURED
    /// </summary>
    class INSURED_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public INSURED_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public INSURED_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public INSURED_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
        public bool Insert(INSURED businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.INSURED_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@INSURED_CD", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INSURED_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@BRANCH_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.BRANCH_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@INS_PROFILE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INS_PROFILE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@INS_SEGMENT_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INS_SEGMENT_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@ENTERED_DATE", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ENTERED_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@DECLINED_DATE", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.DECLINED_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@INS_DECLINED_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INS_DECLINED_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@MAIN_INSURED_IND_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.MAIN_INSURED_IND_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@OPERATIVE_ENDORSEMENT", AseDbType.Decimal, 9, ParameterDirection.Input, false, 19, 4, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.OPERATIVE_ENDORSEMENT)));
                sqlCommand.Parameters.Add(new AseParameter("@CHECK_PAYABLE_TO", AseDbType.VarChar, 80, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CHECK_PAYABLE_TO)));
                sqlCommand.Parameters.Add(new AseParameter("@ANNOTATIONS", AseDbType.VarChar, 80, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ANNOTATIONS)));
                sqlCommand.Parameters.Add(new AseParameter("@REFERRED_BY", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.REFERRED_BY)));
                sqlCommand.Parameters.Add(new AseParameter("@EXONERATION_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.EXONERATION_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@REQUIRED", AseDbType.Bit, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.REQUIRED)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("INSURED::Insert::Error occured.", ex);
            }
        }

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(INSURED businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.INSURED_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@INSURED_CD", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INSURED_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@BRANCH_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.BRANCH_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@INS_PROFILE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INS_PROFILE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@INS_SEGMENT_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INS_SEGMENT_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@ENTERED_DATE", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ENTERED_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@DECLINED_DATE", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.DECLINED_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@INS_DECLINED_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INS_DECLINED_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@MAIN_INSURED_IND_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.MAIN_INSURED_IND_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@OPERATIVE_ENDORSEMENT", AseDbType.Decimal, 9, ParameterDirection.Input, false, 19, 4, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.OPERATIVE_ENDORSEMENT)));
                sqlCommand.Parameters.Add(new AseParameter("@CHECK_PAYABLE_TO", AseDbType.VarChar, 80, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CHECK_PAYABLE_TO)));
                sqlCommand.Parameters.Add(new AseParameter("@ANNOTATIONS", AseDbType.VarChar, 80, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ANNOTATIONS)));
                sqlCommand.Parameters.Add(new AseParameter("@REFERRED_BY", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.REFERRED_BY)));
                sqlCommand.Parameters.Add(new AseParameter("@EXONERATION_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.EXONERATION_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@REQUIRED", AseDbType.Bit, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.REQUIRED)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("INSURED::Update::Error occured.", ex);
            }
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(INSURED businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.INSURED_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@INSURED_CD", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INSURED_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@BRANCH_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.BRANCH_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@INS_PROFILE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INS_PROFILE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@INS_SEGMENT_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INS_SEGMENT_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@ENTERED_DATE", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ENTERED_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@DECLINED_DATE", AseDbType.SmallDateTime, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.DECLINED_DATE, "dd/MM/yyyy")));
                sqlCommand.Parameters.Add(new AseParameter("@INS_DECLINED_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INS_DECLINED_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@MAIN_INSURED_IND_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.MAIN_INSURED_IND_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@OPERATIVE_ENDORSEMENT", AseDbType.Decimal, 9, ParameterDirection.Input, false, 19, 4, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.OPERATIVE_ENDORSEMENT)));
                sqlCommand.Parameters.Add(new AseParameter("@CHECK_PAYABLE_TO", AseDbType.VarChar, 80, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.CHECK_PAYABLE_TO)));
                sqlCommand.Parameters.Add(new AseParameter("@ANNOTATIONS", AseDbType.VarChar, 80, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ANNOTATIONS)));
                sqlCommand.Parameters.Add(new AseParameter("@REFERRED_BY", AseDbType.VarChar, 30, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.REFERRED_BY)));
                sqlCommand.Parameters.Add(new AseParameter("@EXONERATION_TYPE_CD", AseDbType.TinyInt, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.EXONERATION_TYPE_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@REQUIRED", AseDbType.Bit, 1, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.REQUIRED)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("INSURED::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(INSURED businessObject, IDataReader dataReader)
        {


            businessObject.INDIVIDUAL_ID = dataReader.GetInt32(dataReader.GetOrdinal(INSURED.INSUREDFields.INDIVIDUAL_ID.ToString()));

            businessObject.INSURED_CD = dataReader.GetInt32(dataReader.GetOrdinal(INSURED.INSUREDFields.INSURED_CD.ToString()));

            businessObject.BRANCH_CD = dataReader.GetInt32(dataReader.GetOrdinal(INSURED.INSUREDFields.BRANCH_CD.ToString()));

            businessObject.INS_PROFILE_CD = dataReader.GetInt32(dataReader.GetOrdinal(INSURED.INSUREDFields.INS_PROFILE_CD.ToString()));

            businessObject.INS_SEGMENT_CD = dataReader.GetInt32(dataReader.GetOrdinal(INSURED.INSUREDFields.INS_SEGMENT_CD.ToString()));

            businessObject.ENTERED_DATE = dataReader.GetString(dataReader.GetOrdinal(INSURED.INSUREDFields.ENTERED_DATE.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(INSURED.INSUREDFields.DECLINED_DATE.ToString())))
            {
                businessObject.DECLINED_DATE = dataReader.GetString(dataReader.GetOrdinal(INSURED.INSUREDFields.DECLINED_DATE.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(INSURED.INSUREDFields.INS_DECLINED_TYPE_CD.ToString())))
            {
                businessObject.INS_DECLINED_TYPE_CD = dataReader.GetString(dataReader.GetOrdinal(INSURED.INSUREDFields.INS_DECLINED_TYPE_CD.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(INSURED.INSUREDFields.MAIN_INSURED_IND_ID.ToString())))
            {
                businessObject.MAIN_INSURED_IND_ID = dataReader.GetString(dataReader.GetOrdinal(INSURED.INSUREDFields.MAIN_INSURED_IND_ID.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(INSURED.INSUREDFields.OPERATIVE_ENDORSEMENT.ToString())))
            {
                businessObject.OPERATIVE_ENDORSEMENT = dataReader.GetString(dataReader.GetOrdinal(INSURED.INSUREDFields.OPERATIVE_ENDORSEMENT.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(INSURED.INSUREDFields.CHECK_PAYABLE_TO.ToString())))
            {
                businessObject.CHECK_PAYABLE_TO = dataReader.GetString(dataReader.GetOrdinal(INSURED.INSUREDFields.CHECK_PAYABLE_TO.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(INSURED.INSUREDFields.ANNOTATIONS.ToString())))
            {
                businessObject.ANNOTATIONS = dataReader.GetString(dataReader.GetOrdinal(INSURED.INSUREDFields.ANNOTATIONS.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(INSURED.INSUREDFields.REFERRED_BY.ToString())))
            {
                businessObject.REFERRED_BY = dataReader.GetString(dataReader.GetOrdinal(INSURED.INSUREDFields.REFERRED_BY.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(INSURED.INSUREDFields.EXONERATION_TYPE_CD.ToString())))
            {
                businessObject.EXONERATION_TYPE_CD = dataReader.GetString(dataReader.GetOrdinal(INSURED.INSUREDFields.EXONERATION_TYPE_CD.ToString()));
            }

            businessObject.REQUIRED = dataReader.GetBoolean(dataReader.GetOrdinal(INSURED.INSUREDFields.REQUIRED.ToString()));


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of INSURED</returns>
        internal List<INSURED> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<INSURED> list = new List<INSURED>();

            while (dataReader.Read())
            {
                INSURED businessObject = new INSURED();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

    }
}
