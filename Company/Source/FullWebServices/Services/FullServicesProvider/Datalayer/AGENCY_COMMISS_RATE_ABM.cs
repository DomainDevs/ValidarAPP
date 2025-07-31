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
    /// Data access layer class for AGENCY_COMMISS_RATE
    /// </summary>
    class AGENCY_COMMISS_RATE_ABM : DataLayerBase
    {

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public AGENCY_COMMISS_RATE_ABM()
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public AGENCY_COMMISS_RATE_ABM(string Connection)
            : base(Connection)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public AGENCY_COMMISS_RATE_ABM(string Connection, string userId, int AppId, AseCommand Command)
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
        public bool Insert(AGENCY_COMMISS_RATE businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.AGENCY_COMIS_RATE_Insert";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@AGENCY_COMMISS_RATE_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.AGENCY_COMMISS_RATE_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@AGENT_AGENCY_ID", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.AGENT_AGENCY_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@PREFIX_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.PREFIX_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@ST_COMMISS_PCT", AseDbType.Decimal, 4, ParameterDirection.Input, false, 7, 4, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ST_COMMISS_PCT)));
                sqlCommand.Parameters.Add(new AseParameter("@LINE_BUSINESS_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.LINE_BUSINESS_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@SUB_LINE_BUSINESS_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.SUB_LINE_BUSINESS_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@ADDIT_COMMISS_PCT", AseDbType.Decimal, 4, ParameterDirection.Input, false, 7, 4, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ADDIT_COMMISS_PCT)));
                sqlCommand.Parameters.Add(new AseParameter("@SCH_COMMISS_PCT", AseDbType.Decimal, 4, ParameterDirection.Input, false, 7, 4, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.SCH_COMMISS_PCT)));
                sqlCommand.Parameters.Add(new AseParameter("@ST_DIS_COMMISS_PCT", AseDbType.Decimal, 4, ParameterDirection.Input, false, 7, 4, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ST_DIS_COMMISS_PCT)));
                sqlCommand.Parameters.Add(new AseParameter("@ADDIT_DIS_COMMISS_PCT", AseDbType.Decimal, 4, ParameterDirection.Input, false, 7, 4, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ADDIT_DIS_COMMISS_PCT)));
                sqlCommand.Parameters.Add(new AseParameter("@INC_COMMISS_AD_FAC_PCT", AseDbType.Decimal, 4, ParameterDirection.Input, false, 7, 4, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INC_COMMISS_AD_FAC_PCT)));
                sqlCommand.Parameters.Add(new AseParameter("@DIM_COMMISS_AD_FAC_PCT", AseDbType.Decimal, 4, ParameterDirection.Input, false, 7, 4, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.DIM_COMMISS_AD_FAC_PCT)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("AGENCY_COMMISS_RATE::Insert::Error occured.", ex);
            }
        }

        /// <summary>
        /// update row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully updated</returns>
        public bool Update(AGENCY_COMMISS_RATE businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.AGENCY_COMIS_RATE_Update";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@AGENCY_COMMISS_RATE_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.AGENCY_COMMISS_RATE_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@AGENT_AGENCY_ID", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.AGENT_AGENCY_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@PREFIX_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.PREFIX_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@ST_COMMISS_PCT", AseDbType.Decimal, 4, ParameterDirection.Input, false, 7, 4, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ST_COMMISS_PCT)));
                sqlCommand.Parameters.Add(new AseParameter("@LINE_BUSINESS_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.LINE_BUSINESS_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@SUB_LINE_BUSINESS_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.SUB_LINE_BUSINESS_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@ADDIT_COMMISS_PCT", AseDbType.Decimal, 4, ParameterDirection.Input, false, 7, 4, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ADDIT_COMMISS_PCT)));
                sqlCommand.Parameters.Add(new AseParameter("@SCH_COMMISS_PCT", AseDbType.Decimal, 4, ParameterDirection.Input, false, 7, 4, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.SCH_COMMISS_PCT)));
                sqlCommand.Parameters.Add(new AseParameter("@ST_DIS_COMMISS_PCT", AseDbType.Decimal, 4, ParameterDirection.Input, false, 7, 4, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ST_DIS_COMMISS_PCT)));
                sqlCommand.Parameters.Add(new AseParameter("@ADDIT_DIS_COMMISS_PCT", AseDbType.Decimal, 4, ParameterDirection.Input, false, 7, 4, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ADDIT_DIS_COMMISS_PCT)));
                sqlCommand.Parameters.Add(new AseParameter("@INC_COMMISS_AD_FAC_PCT", AseDbType.Decimal, 4, ParameterDirection.Input, false, 7, 4, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INC_COMMISS_AD_FAC_PCT)));
                sqlCommand.Parameters.Add(new AseParameter("@DIM_COMMISS_AD_FAC_PCT", AseDbType.Decimal, 4, ParameterDirection.Input, false, 7, 4, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.DIM_COMMISS_AD_FAC_PCT)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("AGENCY_COMMISS_RATE::Update::Error occured.", ex);
            }
        }

        /// <summary>
        /// delete row in the table
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <returns>true for successfully delete</returns>
        public bool Delete(AGENCY_COMMISS_RATE businessObject)
        {
            AseCommand sqlCommand = new AseCommand();
            sqlCommand = MainCommand;
            sqlCommand.CommandText = "SUP.AGENCY_COMIS_RATE_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new AseParameter("@AGENCY_COMMISS_RATE_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.AGENCY_COMMISS_RATE_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@INDIVIDUAL_ID", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INDIVIDUAL_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@AGENT_AGENCY_ID", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.AGENT_AGENCY_ID)));
                sqlCommand.Parameters.Add(new AseParameter("@PREFIX_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.PREFIX_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@ST_COMMISS_PCT", AseDbType.Decimal, 4, ParameterDirection.Input, false, 7, 4, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ST_COMMISS_PCT)));
                sqlCommand.Parameters.Add(new AseParameter("@LINE_BUSINESS_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.LINE_BUSINESS_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@SUB_LINE_BUSINESS_CD", AseDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.SUB_LINE_BUSINESS_CD)));
                sqlCommand.Parameters.Add(new AseParameter("@ADDIT_COMMISS_PCT", AseDbType.Decimal, 4, ParameterDirection.Input, false, 7, 4, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ADDIT_COMMISS_PCT)));
                sqlCommand.Parameters.Add(new AseParameter("@SCH_COMMISS_PCT", AseDbType.Decimal, 4, ParameterDirection.Input, false, 7, 4, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.SCH_COMMISS_PCT)));
                sqlCommand.Parameters.Add(new AseParameter("@ST_DIS_COMMISS_PCT", AseDbType.Decimal, 4, ParameterDirection.Input, false, 7, 4, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ST_DIS_COMMISS_PCT)));
                sqlCommand.Parameters.Add(new AseParameter("@ADDIT_DIS_COMMISS_PCT", AseDbType.Decimal, 4, ParameterDirection.Input, false, 7, 4, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.ADDIT_DIS_COMMISS_PCT)));
                sqlCommand.Parameters.Add(new AseParameter("@INC_COMMISS_AD_FAC_PCT", AseDbType.Decimal, 4, ParameterDirection.Input, false, 7, 4, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.INC_COMMISS_AD_FAC_PCT)));
                sqlCommand.Parameters.Add(new AseParameter("@DIM_COMMISS_AD_FAC_PCT", AseDbType.Decimal, 4, ParameterDirection.Input, false, 7, 4, "", DataRowVersion.Proposed, FullServicesProviderHelper.ToDBNull(businessObject.DIM_COMMISS_AD_FAC_PCT)));

                sqlCommand.Parameters.Add(new AseParameter("@usser", AseDbType.VarChar, 60, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, User));
                sqlCommand.Parameters.Add(new AseParameter("@id_aplication", AseDbType.Integer, 4, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Proposed, AppId));



                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new SupException("AGENCY_COMMISS_RATE::Delete::Error occured.", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populate business object from data reader
        /// </summary>
        /// <param name="businessObject">business object</param>
        /// <param name="dataReader">data reader</param>
        internal void PopulateBusinessObjectFromReader(AGENCY_COMMISS_RATE businessObject, IDataReader dataReader)
        {


            businessObject.AGENCY_COMMISS_RATE_ID = dataReader.GetInt32(dataReader.GetOrdinal(AGENCY_COMMISS_RATE.AGENCY_COMMISS_RATEFields.AGENCY_COMMISS_RATE_ID.ToString()));

            businessObject.INDIVIDUAL_ID = dataReader.GetInt32(dataReader.GetOrdinal(AGENCY_COMMISS_RATE.AGENCY_COMMISS_RATEFields.INDIVIDUAL_ID.ToString()));

            businessObject.AGENT_AGENCY_ID = dataReader.GetInt32(dataReader.GetOrdinal(AGENCY_COMMISS_RATE.AGENCY_COMMISS_RATEFields.AGENT_AGENCY_ID.ToString()));

            businessObject.PREFIX_CD = dataReader.GetInt32(dataReader.GetOrdinal(AGENCY_COMMISS_RATE.AGENCY_COMMISS_RATEFields.PREFIX_CD.ToString()));

            businessObject.ST_COMMISS_PCT = dataReader.GetDouble(dataReader.GetOrdinal(AGENCY_COMMISS_RATE.AGENCY_COMMISS_RATEFields.ST_COMMISS_PCT.ToString()));

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(AGENCY_COMMISS_RATE.AGENCY_COMMISS_RATEFields.LINE_BUSINESS_CD.ToString())))
            {
                businessObject.LINE_BUSINESS_CD = dataReader.GetString(dataReader.GetOrdinal(AGENCY_COMMISS_RATE.AGENCY_COMMISS_RATEFields.LINE_BUSINESS_CD.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(AGENCY_COMMISS_RATE.AGENCY_COMMISS_RATEFields.SUB_LINE_BUSINESS_CD.ToString())))
            {
                businessObject.SUB_LINE_BUSINESS_CD = dataReader.GetString(dataReader.GetOrdinal(AGENCY_COMMISS_RATE.AGENCY_COMMISS_RATEFields.SUB_LINE_BUSINESS_CD.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(AGENCY_COMMISS_RATE.AGENCY_COMMISS_RATEFields.ADDIT_COMMISS_PCT.ToString())))
            {
                businessObject.ADDIT_COMMISS_PCT = dataReader.GetString(dataReader.GetOrdinal(AGENCY_COMMISS_RATE.AGENCY_COMMISS_RATEFields.ADDIT_COMMISS_PCT.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(AGENCY_COMMISS_RATE.AGENCY_COMMISS_RATEFields.SCH_COMMISS_PCT.ToString())))
            {
                businessObject.SCH_COMMISS_PCT = dataReader.GetString(dataReader.GetOrdinal(AGENCY_COMMISS_RATE.AGENCY_COMMISS_RATEFields.SCH_COMMISS_PCT.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(AGENCY_COMMISS_RATE.AGENCY_COMMISS_RATEFields.ST_DIS_COMMISS_PCT.ToString())))
            {
                businessObject.ST_DIS_COMMISS_PCT = dataReader.GetString(dataReader.GetOrdinal(AGENCY_COMMISS_RATE.AGENCY_COMMISS_RATEFields.ST_DIS_COMMISS_PCT.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(AGENCY_COMMISS_RATE.AGENCY_COMMISS_RATEFields.ADDIT_DIS_COMMISS_PCT.ToString())))
            {
                businessObject.ADDIT_DIS_COMMISS_PCT = dataReader.GetString(dataReader.GetOrdinal(AGENCY_COMMISS_RATE.AGENCY_COMMISS_RATEFields.ADDIT_DIS_COMMISS_PCT.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(AGENCY_COMMISS_RATE.AGENCY_COMMISS_RATEFields.INC_COMMISS_AD_FAC_PCT.ToString())))
            {
                businessObject.INC_COMMISS_AD_FAC_PCT = dataReader.GetString(dataReader.GetOrdinal(AGENCY_COMMISS_RATE.AGENCY_COMMISS_RATEFields.INC_COMMISS_AD_FAC_PCT.ToString()));
            }

            if (!dataReader.IsDBNull(dataReader.GetOrdinal(AGENCY_COMMISS_RATE.AGENCY_COMMISS_RATEFields.DIM_COMMISS_AD_FAC_PCT.ToString())))
            {
                businessObject.DIM_COMMISS_AD_FAC_PCT = dataReader.GetString(dataReader.GetOrdinal(AGENCY_COMMISS_RATE.AGENCY_COMMISS_RATEFields.DIM_COMMISS_AD_FAC_PCT.ToString()));
            }


        }

        /// <summary>
        /// Populate business objects from the data reader
        /// </summary>
        /// <param name="dataReader">data reader</param>
        /// <returns>list of AGENCY_COMMISS_RATE</returns>
        internal List<AGENCY_COMMISS_RATE> PopulateObjectsFromReader(IDataReader dataReader)
        {

            List<AGENCY_COMMISS_RATE> list = new List<AGENCY_COMMISS_RATE>();

            while (dataReader.Read())
            {
                AGENCY_COMMISS_RATE businessObject = new AGENCY_COMMISS_RATE();
                PopulateBusinessObjectFromReader(businessObject, dataReader);
                list.Add(businessObject);
            }
            return list;

        }

        #endregion

    }
}
